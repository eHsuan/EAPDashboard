using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using EAP.ReelCheck.Communication;
using EAP.ReelCheck.Engine;
using EAP.ReelCheck.Models;

namespace EAP.ReelCheck.Service;

/// <summary>
/// EAP 防混料高階整合服務
/// 負責協調防呆狀態機引擎與 MES 通訊
/// </summary>
public class ReelCheckService
{
    private readonly ILogger<ReelCheckService> _logger;
    private readonly ReelCheckEngine _engine;
    private readonly IReelCheckMesService _mesService;

    public ReelCheckEngine Engine => _engine;
    public bool IsInitialized => _engine.IsInitialized;

    public event EventHandler<ReelCheckResult>? OnScanResult;
    public event EventHandler<ReelCheckResult>? OnReelBlocked;

    public ReelCheckService(IReelCheckMesService mesService, ILogger<ReelCheckService>? logger = null)
    {
        _mesService = mesService ?? throw new ArgumentNullException(nameof(mesService));
        _logger = logger ?? NullLogger<ReelCheckService>.Instance;
        _engine = new ReelCheckEngine();
        _engine.OnCheckCompleted += (sender, result) => OnScanResult?.Invoke(this, result);
    }

    /// <summary>
    /// 自動向 MES 查詢工單資訊與 Reel 清單並完成防呆引擎初始化
    /// </summary>
    public async Task<WorkOrderContext> InitializeWorkOrderAsync(string woNo, string machNo, string userId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(woNo);
        _logger.LogInformation("從 MES 自動載入工單防混資料：WO={WoNo}, Machine={MachNo}, User={UserId}", woNo, machNo, userId);

        var context = await _mesService.QueryWorkOrderInfoAsync(woNo, machNo, userId, ct).ConfigureAwait(false);
        if (context == null)
        {
            throw new InvalidOperationException($"無法取得工單 [{woNo}] 之資料 (Context 為空)");
        }

        if (string.IsNullOrWhiteSpace(context.MatNo) || string.IsNullOrWhiteSpace(context.BatchNo))
        {
            throw new InvalidOperationException($"工單 [{woNo}] 缺少料號或批號資訊，無法進行防混料檢核");
        }

        var boundReels = await _mesService.QueryReelListAsync(woNo, ct).ConfigureAwait(false);

        _engine.Initialize(context, boundReels ?? Enumerable.Empty<ReelItem>());
        _logger.LogInformation("防混引擎初始化完成，已綁定 {Count} 筆 Reel", boundReels?.Count ?? 0);
        return context;
    }

    /// <summary>
    /// 透過現有上下文初始化工單與載入 MES 基準資料
    /// </summary>
    public async Task InitializeWorkOrderAsync(WorkOrderContext context, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (string.IsNullOrWhiteSpace(context.MatNo) || string.IsNullOrWhiteSpace(context.BatchNo))
        {
            throw new InvalidOperationException($"工單 [{context.WONo}] 缺少料號或批號資訊，無法進行防混料檢核");
        }

        _logger.LogInformation("開始初始化工單防混：WO={WoNo}, Machine={MachNo}, MatNo={MatNo}", context.WONo, context.MachineNo, context.MatNo);

        // 向 MES 查詢該工單之所有 Reel 清單
        var boundReels = await _mesService.QueryReelListAsync(context.WONo, ct).ConfigureAwait(false);
        _logger.LogInformation("自 MES 取回 {Count} 筆 Reel 清單", boundReels?.Count ?? 0);

        _engine.Initialize(context, boundReels ?? Enumerable.Empty<ReelItem>());
        _logger.LogInformation("防混引擎初始化完成");
    }

    /// <summary>
    /// 處理條碼掃描事件
    /// </summary>
    public async Task<ReelCheckResult> ScanAsync(string rawBarcode, CancellationToken ct = default)
    {
        // 1. 動態狀態檢查 (WOConfirm_Get: 檢查工單是否被刪除 Confirm、被重新過帳或已出站)
        if (_engine.CurrentContext != null && _engine.CurrentContext.ConfirmInTime != DateTime.MinValue)
        {
            var statusCheck = await _mesService.CheckWorkOrderStatusAsync(
                _engine.CurrentContext.WONo,
                _engine.CurrentContext.WorkCenterNo,
                _engine.CurrentContext.ConfirmInTime,
                ct).ConfigureAwait(false);

            if (!statusCheck.IsOk)
            {
                var lockResult = ReelCheckResult.Ng(
                    rawBarcode,
                    statusCheck.ErrorCode,
                    statusCheck.Message,
                    CheckDisposition.BlockWorkOrder);

                _logger.LogCritical("工單動態狀態檢核異常！觸發防呆鎖單：[{Code}] {Msg}", statusCheck.ErrorCode, statusCheck.Message);
                await _mesService.BlockWorkOrderAsync(
                    _engine.CurrentContext.WONo,
                    statusCheck.ErrorCode,
                    statusCheck.Message,
                    ct).ConfigureAwait(false);

                OnReelBlocked?.Invoke(this, lockResult);
                return lockResult;
            }
        }

        var result = _engine.ProcessBarcode(rawBarcode);

        // 2. 處理 1D 非本單條碼反查防呆 (NG-0105 / NG-0106 / NG-0107)
        if (result.BarcodeType == BarcodeKind.Unknown1D)
        {
            string candidateBarcode = result.DecodedReelId;
            try
            {
                // A. 反查是否為全廠其他工單之批號 (NG-0105)
                var batchWo = await _mesService.QueryWorkOrderByBatchNoAsync(candidateBarcode, ct).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(batchWo))
                {
                    result = ReelCheckResult.CrossWoNg(
                        rawBarcode,
                        "NG-0105",
                        $"上機異常(NG-0105), 當前作業工單:{_engine.CurrentContext?.WONo}, 但掃進來的T/P No.條碼:{candidateBarcode}, 屬於另一個工單:{batchWo}",
                        candidateBarcode,
                        batchWo);
                }
                else
                {
                    // B. 查詢是否為其他料號主檔 (NG-0106)
                    bool isMat = await _mesService.IsMaterialExistsAsync(candidateBarcode, ct).ConfigureAwait(false);
                    if (isMat)
                    {
                        result = ReelCheckResult.Ng(
                            rawBarcode,
                            "NG-0106",
                            $"上機異常(NG-0106), 當前作業工單:{_engine.CurrentContext?.WONo}, 但掃進來的料號條碼:{candidateBarcode}, 屬於另一個工單(未知)",
                            CheckDisposition.BlockWorkOrder,
                            BarcodeKind.MatNo);
                    }
                    else
                    {
                        // C. 皆非，判定為無法識別條碼 (NG-0107)
                        result = ReelCheckResult.Ng(
                            rawBarcode,
                            "NG-0107",
                            $"上機異常(NG-0107), 當前作業工單:{_engine.CurrentContext?.WONo}, 但掃進來的條碼:{candidateBarcode}無法識別。",
                            CheckDisposition.BlockWorkOrder,
                            BarcodeKind.Unknown);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "向 MES 反查 1D 條碼 [{Barcode}] 失敗: {Msg}", candidateBarcode, ex.Message);
            }
        }

        // 3. 若本機判定清單找不到 2D Reel (NG-0301)，向 MES 進行反查是否屬於其他工單 (NG-0401)
        if (!result.IsSuccess && result.ErrorCode == "NG-0301" && !string.IsNullOrEmpty(result.DecodedReelId))
        {
            try
            {
                var otherWo = await _mesService.QueryOtherWorkOrderByBarcodeAsync(result.DecodedReelId, ct).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(otherWo) &&
                    !string.Equals(otherWo, _engine.CurrentContext?.WONo, StringComparison.OrdinalIgnoreCase))
                {
                    // 升級判定為 NG-0401：當前工單 A，但掃到其他工單 B 的 Reel
                    result = ReelCheckResult.CrossWoNg(
                        rawBarcode,
                        "NG-0401",
                        $"與工單不匹配(NG-0401), 當前工單({_engine.CurrentContext?.WONo}), 但掃到其他工單({otherWo})的Reel({result.DecodedReelId})。",
                        result.DecodedReelId,
                        otherWo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "向 MES 反查 ReelID [{ReelId}] 所屬工單失敗: {Msg}", result.DecodedReelId, ex.Message);
            }
        }

        // 4. 若判定為重大混料需鎖單
        if (result.ShouldBlockWorkOrder && _engine.CurrentContext != null)
        {
            _logger.LogWarning("觸發防呆鎖單機制！原因: [{Code}] {Msg}", result.ErrorCode, result.Message);
            try
            {
                // 1. 鎖定當前工單
                await _mesService.BlockWorkOrderAsync(
                    _engine.CurrentContext.WONo,
                    result.ErrorCode,
                    result.Message,
                    ct
                ).ConfigureAwait(false);

                // 2. 若為 NG-0401 / NG-0105 跨工單混料，同步鎖定被誤拿料捲之問題工單 (ProblemWoNo)
                if (!string.IsNullOrWhiteSpace(result.ProblemWoNo))
                {
                    _logger.LogWarning("觸發雙向鎖單！鎖定被誤拿料捲之問題工單: {OtherWo}", result.ProblemWoNo);
                    await _mesService.BlockWorkOrderAsync(
                        result.ProblemWoNo,
                        "NG-0401B",
                        $"問題工單({result.ProblemWoNo})的條碼在工單({_engine.CurrentContext.WONo})的作業當中發現",
                        ct
                    ).ConfigureAwait(false);
                }

                OnReelBlocked?.Invoke(this, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "呼叫 MES 鎖定工單失敗: {Msg}", ex.Message);
            }
        }
        // 若檢驗合格且為 2D Reel 標籤，非同步向 MES 回寫過帳歷程
        else if (result.IsSuccess && result.BarcodeType == BarcodeKind.Reel2D && result.ActiveReel != null && _engine.CurrentContext != null)
        {
            var ctx = _engine.CurrentContext;
            var reel = result.ActiveReel;

            _ = Task.Run(async () =>
            {
                try
                {
                    // 1. 更新 Reel 狀態為 11
                    await _mesService.UpdateReelStatusAsync(reel.ReelId, "11", ctx.MachineNo, ctx.OperatorId, reel.StartTime, DateTime.Now, CancellationToken.None);
                    // 2. 寫入上料歷史日誌
                    await _mesService.AddReelLogAsync(ctx.WONo, reel.ReelId, reel.MatNo, reel.BatchNo, ctx.OperatorId, "00005", CancellationToken.None);
                    // 3. 儲存防呆紀錄
                    await _mesService.SaveReelCheckRecordAsync(ctx.WONo, ctx.MachineNo, reel.ReelId, ctx.MatNo, ctx.BatchNo, reel.ReelScanCount, reel.MatNoScanCount, reel.TPNoScanCount, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "非同步回寫 MES Reel 狀態失敗: {Msg}", ex.Message);
                }
            }, CancellationToken.None);
        }

        return result;
    }

    /// <summary>
    /// 工單結束作業 (WoFinish)，結算最後一捲指標並回寫 MES
    /// </summary>
    public async Task<ReelCheckResult> FinishWorkOrderAsync(CancellationToken ct = default)
    {
        if (_engine.CurrentContext == null)
        {
            return ReelCheckResult.Ng(string.Empty, "NG-0001", "防混引擎尚未初始化工單資訊", CheckDisposition.Warning);
        }

        var ctx = _engine.CurrentContext;
        var finishResult = _engine.FinishWorkOrder();

        if (!finishResult.IsSuccess && finishResult.ShouldBlockWorkOrder)
        {
            _logger.LogWarning("工單結案檢驗不合格！原因: [{Code}] {Msg}", finishResult.ErrorCode, finishResult.Message);
            try
            {
                await _mesService.BlockWorkOrderAsync(ctx.WONo, finishResult.ErrorCode, finishResult.Message, ct).ConfigureAwait(false);
                OnReelBlocked?.Invoke(this, finishResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "呼叫 MES 鎖定工單失敗: {Msg}", ex.Message);
            }
            return finishResult;
        }

        if (finishResult.IsSuccess)
        {
            _logger.LogInformation("工單結案合格，回寫 MES 結案歷程...");
            try
            {
                await _mesService.FinishWorkOrderAsync(ctx.WONo, finishResult.ActiveReel, ctx.MachineNo, ctx.OperatorId, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "呼叫 MES FinishWorkOrder 發生異常: {Msg}", ex.Message);
            }
        }

        return finishResult;
    }

    /// <summary>
    /// 取得當前 Reel 即時狀態列表
    /// </summary>
    public IReadOnlyList<ReelItem> GetReelList() => _engine.GetReelSnapshot();
}
