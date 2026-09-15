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
        var result = _engine.ProcessBarcode(rawBarcode);

        // 若本機判定清單找不到 Reel (NG-0301)，向 MES 進行反查是否屬於其他工單 (NG-0401)
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

        // 若判定為重大混料需鎖單
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

                // 2. 若為 NG-0401 跨工單混料，同步鎖定被誤拿料捲之問題工單 (ProblemWoNo)
                if (!string.IsNullOrWhiteSpace(result.ProblemWoNo))
                {
                    _logger.LogWarning("觸發雙向鎖單！鎖定被誤拿料捲之問題工單: {OtherWo}", result.ProblemWoNo);
                    await _mesService.BlockWorkOrderAsync(
                        result.ProblemWoNo,
                        "NG-0401B",
                        $"問題工單({result.ProblemWoNo})的Reel({result.DecodedReelId})在工單({_engine.CurrentContext.WONo})的作業當中發現",
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
    /// 取得當前 Reel 即時狀態列表
    /// </summary>
    public IReadOnlyList<ReelItem> GetReelList() => _engine.GetReelSnapshot();
}
