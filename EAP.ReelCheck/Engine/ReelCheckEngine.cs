using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EAP.ReelCheck.Models;
using EAP.ReelCheck.Parsers;

namespace EAP.ReelCheck.Engine;

/// <summary>
/// 防呆防混核心狀態機引擎
/// 100% 封裝 Cyntec 產線防混驗證規則，零外部舊庫依賴
/// </summary>
public class ReelCheckEngine
{
    private readonly object _syncRoot = new();
    private WorkOrderContext? _context;
    private readonly ConcurrentDictionary<string, ReelItem> _reels = new(StringComparer.OrdinalIgnoreCase);
    private ReelItem? _activeReel;

    public bool IsInitialized => _context != null;
    public WorkOrderContext? CurrentContext => _context;
    public ReelItem? ActiveReel => _activeReel;

    public event EventHandler<ReelCheckResult>? OnCheckCompleted;

    /// <summary>
    /// 初始化工單上下文與合法 Reel 清單
    /// </summary>
    public void Initialize(WorkOrderContext context, IEnumerable<ReelItem> initialReels)
    {
        ArgumentNullException.ThrowIfNull(context);

        lock (_syncRoot)
        {
            _context = context;
            _reels.Clear();
            _activeReel = null;

            foreach (var reel in initialReels)
            {
                _reels[reel.ReelId] = reel;
            }
        }
    }

    /// <summary>
    /// 處理條碼掃描輸入 (核心防混進入點)
    /// </summary>
    public ReelCheckResult ProcessBarcode(string rawBarcode, DateTime? scanTime = null)
    {
        DateTime time = scanTime ?? DateTime.Now;

        lock (_syncRoot)
        {
            if (_context == null)
            {
                return ReelCheckResult.Ng(rawBarcode, "NG-0001", "防混引擎尚未初始化工單資訊", CheckDisposition.Warning);
            }

            // 1. 開關豁免檢核
            if (!_context.IsCheckEnabled)
            {
                return ReelCheckResult.Ignored(rawBarcode, "防呆檢驗開關未開啟 (Property20=N)，略過檢驗");
            }

            if (string.Equals(_context.OrderType, "ZE01", StringComparison.OrdinalIgnoreCase))
            {
                return ReelCheckResult.Ignored(rawBarcode, "工程試作工單 (ZE01)，略過檢驗");
            }

            // 2. 前置格式清洗與硬體異常檢核
            var filterResult = BarcodeFilter.SanitizeAndValidate(rawBarcode, _context.MatNo, _context.BatchNo);
            if (filterResult.IsHardwareError)
            {
                var hwNg = ReelCheckResult.Ng(rawBarcode, filterResult.ErrorCode, filterResult.Reason, CheckDisposition.Warning);
                OnCheckCompleted?.Invoke(this, hwNg);
                return hwNg;
            }

            if (filterResult.IsIgnored)
            {
                var ignoreResult = ReelCheckResult.Ignored(rawBarcode, filterResult.Reason);
                OnCheckCompleted?.Invoke(this, ignoreResult);
                return ignoreResult;
            }

            string cleaned = filterResult.CleanedBarcode;

            // 3. 判定條碼型態與分流檢驗
            ReelCheckResult checkResult;

            // 分流 A: 1D 大標籤料號 (比對主料號或副料號)
            if (string.Equals(cleaned, _context.MatNo, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(_context.SubMatNo) && string.Equals(cleaned, _context.SubMatNo, StringComparison.OrdinalIgnoreCase)))
            {
                checkResult = ProcessMatNoScan(cleaned, time);
            }
            // 分流 B: 1D 大標籤批號 (比對主批號或副批號)
            else if (string.Equals(cleaned, _context.BatchNo, StringComparison.OrdinalIgnoreCase) ||
                     (!string.IsNullOrEmpty(_context.SubBatchNo) && string.Equals(cleaned, _context.SubBatchNo, StringComparison.OrdinalIgnoreCase)))
            {
                checkResult = ProcessTapingNoScan(cleaned, time);
            }
            // 分流 C: 2D Reel 標籤條碼 或 未知 1D 條碼
            else
            {
                var parsed2D = Barcode2DParser.Parse(cleaned);
                if (parsed2D.IsValid && parsed2D.Is2D)
                {
                    checkResult = ProcessReel2DScan(cleaned, rawBarcode, time);
                }
                else
                {
                    // 既非本單主/副料號、主/副批號，亦非標準 2D Reel 標籤 => 標記為 Unknown1D 轉交服務層反查全廠 MES (NG-0105/0106/0107)
                    checkResult = ReelCheckResult.Unknown1DResult(rawBarcode, cleaned);
                }
            }

            OnCheckCompleted?.Invoke(this, checkResult);
            return checkResult;
        }
    }

    /// <summary>
    /// 處理 1D 料號條碼掃描
    /// </summary>
    private ReelCheckResult ProcessMatNoScan(string barcode, DateTime time)
    {
        // 主料製程但掃入副料號檢驗 (NG-0101)
        if (_context!.IsMainMaterial && !string.IsNullOrEmpty(_context.SubMatNo) &&
            string.Equals(barcode, _context.SubMatNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(barcode, "NG-0101", $"當前為主料作業，但掃入副料號 [{barcode}]", CheckDisposition.BlockWorkOrder, BarcodeKind.MatNo);
        }

        // 副料製程但掃入主料號檢驗 (NG-0102)
        if (!_context.IsMainMaterial && !string.IsNullOrEmpty(_context.SubMatNo) &&
            string.Equals(barcode, _context.MatNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(barcode, "NG-0102", $"當前為副料作業，但掃入主料號 [{barcode}]", CheckDisposition.BlockWorkOrder, BarcodeKind.MatNo);
        }

        if (_activeReel != null)
        {
            _activeReel.MatNoScanCount++;
            _activeReel.LastUpdateTime = time;
        }

        return ReelCheckResult.Ok(barcode, BarcodeKind.MatNo, "大標籤料號掃描成功", _activeReel);
    }

    /// <summary>
    /// 處理 1D 批號 (Taping No) 條碼掃描
    /// </summary>
    private ReelCheckResult ProcessTapingNoScan(string barcode, DateTime time)
    {
        if (_activeReel != null)
        {
            _activeReel.TPNoScanCount++;
            _activeReel.LastUpdateTime = time;
        }

        // 主料製程但掃入副料批號檢驗 (NG-0103)
        if (_context!.IsMainMaterial && !string.IsNullOrEmpty(_context.SubBatchNo) &&
            string.Equals(barcode, _context.SubBatchNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(barcode, "NG-0103", $"上機異常(NG-0103), 當前作業是主料, T/P NO.:[{_context.BatchNo}], 但掃進來的是副料條碼 [{barcode}]", CheckDisposition.BlockWorkOrder, BarcodeKind.TapingNo);
        }

        // 副料製程但掃入主料批號檢驗 (NG-0104)
        if (!_context.IsMainMaterial && !string.IsNullOrEmpty(_context.SubBatchNo) &&
            string.Equals(barcode, _context.BatchNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(barcode, "NG-0104", $"上機異常(NG-0104), 當前作業是副料, T/P No.:[{_context.SubBatchNo}], 但掃進來的是主料條碼 [{barcode}]", CheckDisposition.BlockWorkOrder, BarcodeKind.TapingNo);
        }

        return ReelCheckResult.Ok(barcode, BarcodeKind.TapingNo, "大標籤批號(TapingNo)掃描成功", _activeReel);
    }

    /// <summary>
    /// 處理 2D Reel 標籤條碼掃描 (防混主戰場)
    /// </summary>
    private ReelCheckResult ProcessReel2DScan(string cleanedBarcode, string rawBarcode, DateTime time)
    {
        var parsed = Barcode2DParser.Parse(cleanedBarcode);
        if (!parsed.IsValid || !parsed.Is2D)
        {
            return ReelCheckResult.Ng(rawBarcode, "NG-9905", "掃描內容非標準 2D 條碼或無法辨識之格式", CheckDisposition.Warning);
        }

        string decodedReelId = parsed.DecodedReelId;

        // 【防混規則 1：工單料號比對 (NG-0106)】
        if (!string.Equals(parsed.MatNo, _context!.MatNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(rawBarcode, "NG-0106", $"料號防混異常：標籤料號 [{parsed.MatNo}] 與當前工單料號 [{_context.MatNo}] 不符！", CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        // 【防混規則 2：工單批號比對 (NG-0105)】
        if (!string.Equals(parsed.TapingNo, _context.BatchNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(rawBarcode, "NG-0105", $"批號防混異常：標籤批號 [{parsed.TapingNo}] 與當前工單批號 [{_context.BatchNo}] 不符！", CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        // 【防混規則 3：Reel 工單綁定檢驗 (NG-0301 / NG-0401)】
        if (!_reels.TryGetValue(decodedReelId, out var targetReel))
        {
            return new ReelCheckResult
            {
                IsSuccess = false,
                Disposition = CheckDisposition.BlockWorkOrder,
                ErrorCode = "NG-0301",
                RawBarcode = rawBarcode,
                DecodedReelId = decodedReelId,
                BarcodeType = BarcodeKind.Reel2D,
                Message = $"歸屬防混異常：Reel ID [{decodedReelId}] 不屬於本工單合法清單！"
            };
        }

        // 【防混規則 4：Reel 狀態防呆 (NG-0108 - 防止已出站重複投料)】
        if (!string.Equals(targetReel.Status, "00") && !string.Equals(targetReel.Status, "11"))
        {
            return ReelCheckResult.Ng(rawBarcode, "NG-0108", $"Reel 狀態異常：[{decodedReelId}] 當前狀態為 [{targetReel.Status}]，不可重複投料！", CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        // 【防混規則 5：機台綁定防呆 (NG-0109 - 防止拿至錯誤機台)】
        if (!string.IsNullOrWhiteSpace(targetReel.TPMachNo) &&
            !string.Equals(targetReel.TPMachNo, _context.MachineNo, StringComparison.OrdinalIgnoreCase))
        {
            return ReelCheckResult.Ng(rawBarcode, "NG-0109", $"機台綁定異常：Reel ID [{decodedReelId}] 原綁定於機台 [{targetReel.TPMachNo}]，不可於本機台 [{_context.MachineNo}] 生產！", CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        // 【防混規則 6：換捲結算 (檢驗上一捲生產指標)】
        if (_activeReel != null && !string.Equals(_activeReel.ReelId, decodedReelId, StringComparison.OrdinalIgnoreCase))
        {
            var prevResult = ValidatePreviousReel(_activeReel, time);
            if (!prevResult.IsSuccess)
            {
                return prevResult;
            }

            _activeReel.EndTime = time;
            _activeReel.IsCompleted = true;
            _activeReel.IsCurrentActive = false;
        }

        // 【更新當前 Reel 狀態】
        targetReel.Status = "11"; // 作業中
        targetReel.TPStatus = "11";
        targetReel.TPMachNo = _context.MachineNo;
        targetReel.WONo = _context.WONo;
        if (targetReel.StartTime == DateTime.MinValue)
        {
            targetReel.StartTime = time;
        }
        targetReel.LastUpdateTime = time;
        targetReel.ReelScanCount++;
        targetReel.IsCurrentActive = true;

        _activeReel = targetReel;

        return new ReelCheckResult
        {
            IsSuccess = true,
            Disposition = CheckDisposition.Pass,
            RawBarcode = rawBarcode,
            DecodedReelId = decodedReelId,
            BarcodeType = BarcodeKind.Reel2D,
            Message = $"Reel [{decodedReelId}] 檢驗合格，上機狀態正常",
            ActiveReel = targetReel
        };
    }

    /// <summary>
    /// 檢驗上一捲 Reel 是否符合最低作業時間與掃描次數門檻
    /// </summary>
    private ReelCheckResult ValidatePreviousReel(ReelItem prevReel, DateTime currentTime)
    {
        // 檢驗最小作業時間 (NG-0113)
        if (_context!.MinReelDurationMinutes > 0 && prevReel.StartTime != DateTime.MinValue)
        {
            decimal durationMinutes = (decimal)(currentTime - prevReel.StartTime).TotalMinutes;
            if (durationMinutes < _context.MinReelDurationMinutes)
            {
                return ReelCheckResult.Ng(prevReel.ReelId, "NG-0113",
                    $"作業時間異常：上一捲 [{prevReel.ReelId}] 生產時間為 {durationMinutes:0.0} 分鐘，未達門檻 {_context.MinReelDurationMinutes} 分鐘！",
                    CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
            }
        }

        // 檢驗大標籤料號掃描次數 (NG-0111)
        if (prevReel.MatNoScanCount < _context.MinMatNoScanCount)
        {
            return ReelCheckResult.Ng(prevReel.ReelId, "NG-0111",
                $"防呆次數異常：上一捲 [{prevReel.ReelId}] 大標籤料號掃描次數 ({prevReel.MatNoScanCount}) 未達標準 ({_context.MinMatNoScanCount})！",
                CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        // 檢驗大標籤批號掃描次數 (NG-0112)
        if (prevReel.TPNoScanCount < _context.MinTPNoScanCount)
        {
            return ReelCheckResult.Ng(prevReel.ReelId, "NG-0112",
                $"防呆次數異常：上一捲 [{prevReel.ReelId}] 大標籤批號掃描次數 ({prevReel.TPNoScanCount}) 未達標準 ({_context.MinTPNoScanCount})！",
                CheckDisposition.BlockWorkOrder, BarcodeKind.Reel2D);
        }

        return ReelCheckResult.Ok(prevReel.ReelId, BarcodeKind.Reel2D, "前一捲作業檢驗合格", prevReel);
    }

    /// <summary>
    /// 工單結束作業 (WoFinish)，結算最後一捲 Reel 作業指標並完成工單
    /// </summary>
    public ReelCheckResult FinishWorkOrder(DateTime? finishTime = null)
    {
        DateTime time = finishTime ?? DateTime.Now;

        lock (_syncRoot)
        {
            if (_context == null)
            {
                return ReelCheckResult.Ng(string.Empty, "NG-0001", "防混引擎尚未初始化工單資訊", CheckDisposition.Warning);
            }

            // 若當前有正在作業之最後一捲 Reel，檢驗指標
            if (_activeReel != null)
            {
                var valResult = ValidatePreviousReel(_activeReel, time);
                if (!valResult.IsSuccess)
                {
                    OnCheckCompleted?.Invoke(this, valResult);
                    return valResult;
                }

                _activeReel.EndTime = time;
                _activeReel.IsCompleted = true;
                _activeReel.IsCurrentActive = false;
            }

            var finishResult = ReelCheckResult.Ok(
                string.Empty,
                BarcodeKind.Unknown,
                $"工單 [{_context.WONo}] 結案完成 (WoFinish)，最後一捲生產檢驗合格",
                _activeReel);

            OnCheckCompleted?.Invoke(this, finishResult);
            return finishResult;
        }
    }

    /// <summary>
    /// 取得當前所有 Reel 清單快照
    /// </summary>
    public IReadOnlyList<ReelItem> GetReelSnapshot()
    {
        lock (_syncRoot)
        {
            return _reels.Values.ToList();
        }
    }
}
