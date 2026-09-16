using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using cyntec.Base;
using cyntec.MES;
using cyntec.WCF;
using EAP.ReelCheck.Configuration;
using EAP.ReelCheck.Models;

namespace EAP.ReelCheck.Communication;

/// <summary>
/// 基於 Cyntec WCF 交易通道的 MES 通訊服務實作 (Adapter 模式)
/// 負責將底層二進位 WCF 通訊與原生 EAP.ReelCheck 模型進行隔離與轉換
/// </summary>
public class WcfReelCheckMesService : IReelCheckMesService, IDisposable
{
    private readonly ILogger<WcfReelCheckMesService> _logger;
    private readonly ReelCheckOptions _options;
    private readonly object _connLock = new();
    private wcfServiceConnection? _txnConn;
    private bool _disposed;

    public WcfReelCheckMesService(ReelCheckOptions? options = null, ILogger<WcfReelCheckMesService>? logger = null)
    {
        _options = options ?? new ReelCheckOptions();
        _logger = logger ?? NullLogger<WcfReelCheckMesService>.Instance;
        EnsureConnection();
    }

    private void EnsureConnection()
    {
        lock (_connLock)
        {
            if (_txnConn != null && _txnConn.IsInitSuccess) return;

            try
            {
                _logger.LogInformation("正在建立 MES WCF 連線: Server={ServerUrl}, Client={ClientUrl}", _options.ServerUrl, _options.ClientUrl);
                _txnConn = wcfServiceConnection.NewConnection(_options.ServerUrl, _options.ClientUrl, null, null);

                if (_options.BackupServerUrls != null && _options.BackupServerUrls.Count > 0)
                {
                    foreach (var backupUrl in _options.BackupServerUrls)
                    {
                        if (!string.IsNullOrWhiteSpace(backupUrl))
                        {
                            _txnConn.ServerUrlList.Add(backupUrl);
                        }
                    }
                }

                _txnConn.Client_Id = _options.ClientIdPrefix + Environment.MachineName;
                _txnConn.Address = _txnConn.Client_Id;
                _txnConn.IsReconnect = _options.AutoReconnect;
                _txnConn.OpenTimeout = _options.TimeoutSeconds;
                _txnConn.CloseTimeout = _options.TimeoutSeconds;

                _txnConn.onError += (sender, msg) => _logger.LogError("[WCF Error] {Msg}", msg);
                _txnConn.onLog += (sender, msg) => _logger.LogDebug("[WCF Log] {Msg}", msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化 MES WCF 連線物件失敗: {Msg}", ex.Message);
            }
        }
    }

    /// <inheritdoc />
    public async Task<bool> PingAsync(CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                try
                {
                    EnsureConnection();
                    return _txnConn != null;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "MES WCF Ping 異常: {Msg}", ex.Message);
                    return false;
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<WorkOrderContext> QueryWorkOrderInfoAsync(string woNo, string machNo, string userId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(woNo);

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) throw new InvalidOperationException("WCF 連線未就緒");

                var context = new WorkOrderContext
                {
                    WONo = woNo.Trim().ToUpperInvariant(),
                    MachineNo = machNo ?? string.Empty,
                    OperatorId = userId ?? string.Empty
                };

                // 1. 查詢工單基本資訊 (WoInfo_Get)
                try
                {
                    var woTxn = new WoInfo_Get { _WoNo = context.WONo };
                    woTxn.clientExecute(_txnConn);

                    if (woTxn.IsTxnError)
                    {
                        _logger.LogError("查詢工單基本資料失敗: {Msg}", woTxn.TxnErrorMsg);
                        throw new InvalidOperationException($"MES 查詢工單失敗: {woTxn.TxnErrorMsg}");
                    }

                    if (woTxn.Result?._WO?._ItemList == null || woTxn.Result._WO._ItemList.Count == 0)
                    {
                        _logger.LogWarning("MES 查無工單基本資料 (ItemList 為空): WO={WoNo}", context.WONo);
                        throw new InvalidOperationException($"MES 查無工單 [{context.WONo}] 之基本資訊");
                    }

                    var mainWo = woTxn.Result._WO._ItemList[0];
                    context.MatNo = mainWo.MatNo?.Trim() ?? string.Empty;
                    context.BatchNo = mainWo.BatchNo?.Trim() ?? string.Empty;
                    context.WorkCenterNo = mainWo.CurrentWorkCenter?.Trim() ?? string.Empty;
                    context.OrderType = mainWo.OrderType?.Trim() ?? string.Empty;

                    try
                    {
                        if (woTxn.Result._WOConfirm != null)
                        {
                            var confirmStep = woTxn.Result._WOConfirm.GetDesignatedStep(context.WorkCenterNo)
                                              ?? woTxn.Result._WOConfirm.GetDesignatedStep("TP");
                            if (confirmStep != null)
                            {
                                context.ConfirmInTime = confirmStep.InTime;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "取得初始 ConfirmInTime 略過: {Msg}", ex.Message);
                    }
                }
                catch (Exception ex) when (ex is not InvalidOperationException)
                {
                    _logger.LogError(ex, "呼叫 WoInfo_Get 連線異常: {Msg}", ex.Message);
                    throw new InvalidOperationException($"呼叫 MES 查詢工單發生通訊異常: {ex.Message}", ex);
                }

                // 2. 查詢機台防呆檢核參數 (EQP_OP_CheckList_Get / MESSystemParameter_Get)
                if (!string.IsNullOrWhiteSpace(context.MachineNo))
                {
                    try
                    {
                        var eqpTxn = new EQP_OP_CheckList_Get { _MachNo = context.MachineNo };
                        eqpTxn.clientExecute(_txnConn);

                        if (!eqpTxn.IsTxnError && eqpTxn.Result?._ItemList != null)
                        {
                            foreach (var item in eqpTxn.Result._ItemList)
                            {
                                if (item.Operation != null && item.Operation.Contains("REEL", StringComparison.OrdinalIgnoreCase))
                                {
                                    context.IsCheckEnabled = item.IsEnable;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "呼叫 EQP_OP_CheckList_Get 失敗，套用預設檢核門檻: {Msg}", ex.Message);
                    }
                }

                return context;
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<List<ReelItem>> QueryReelListAsync(string woNo, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(woNo);

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) throw new InvalidOperationException("WCF 連線未就緒");

                var list = new List<ReelItem>();

                try
                {
                    var reelTxn = new WO_ReelID_Get { _WoNo = woNo.Trim().ToUpperInvariant() };
                    reelTxn.clientExecute(_txnConn);

                    if (reelTxn.IsTxnError)
                    {
                        _logger.LogError("查詢工單 Reel 清單失敗: {Msg}", reelTxn.TxnErrorMsg);
                        throw new InvalidOperationException($"MES 查詢工單 Reel 清單失敗: {reelTxn.TxnErrorMsg}");
                    }

                    if (reelTxn.Result?._ItemList != null)
                    {
                        foreach (var raw in reelTxn.Result._ItemList)
                        {
                            var item = new ReelItem
                            {
                                ReelId = raw.ReelID?.Trim() ?? string.Empty,
                                WONo = raw.WONo?.Trim() ?? woNo,
                                MatNo = raw.MatNo?.Trim() ?? string.Empty,
                                BatchNo = raw.BatchNo?.Trim() ?? string.Empty,
                                Status = string.IsNullOrWhiteSpace(raw.Status) ? "00" : raw.Status.Trim(),
                                TPStatus = string.IsNullOrWhiteSpace(raw.TPStatus) ? "00" : raw.TPStatus.Trim(),
                                TPMachNo = raw.TPMachNo?.Trim() ?? string.Empty,
                                ReelScanCount = raw.ReelIDCount,
                                MatNoScanCount = raw.MatNoCount,
                                TPNoScanCount = raw.TPNoCount
                            };

                            if (raw.TPStartTime != DateTime.MinValue) item.StartTime = raw.TPStartTime;
                            if (raw.TPEndTime != DateTime.MinValue) item.EndTime = raw.TPEndTime;

                            list.Add(item);
                        }
                    }

                    _logger.LogInformation("工單 [{WoNo}] 取得 {Count} 筆 Reel 清單", woNo, list.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "呼叫 WO_ReelID_Get 失敗: {Msg}", ex.Message);
                }

                return list;
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateReelStatusAsync(string reelId, string statusCode, string machNo, string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reelId);

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return false;

                try
                {
                    var updTxn = new WO_ReelID_Update
                    {
                        _reelID = reelId.Trim(),
                        _statusCode = statusCode ?? "11",
                        _machNo = machNo ?? string.Empty,
                        _UpdateUser = userId ?? string.Empty,
                        _startTime = startTime == DateTime.MinValue ? DateTime.Now : startTime,
                        _endTime = endTime == DateTime.MinValue ? DateTime.Now : endTime
                    };

                    updTxn.clientExecute(_txnConn);

                    if (updTxn.IsTxnError)
                    {
                        _logger.LogError("回寫 Reel [{ReelId}] 狀態失敗: {Msg}", reelId, updTxn.TxnErrorMsg);
                        return false;
                    }

                    _logger.LogInformation("成功更新 Reel [{ReelId}] 狀態為 {Status}", reelId, statusCode);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "呼叫 WO_ReelID_Update 發生異常: {Msg}", ex.Message);
                    return false;
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> AddReelLogAsync(string woNo, string reelId, string matNo, string batchNo, string userId, string action = "00005", CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return false;

                try
                {
                    var logTxn = new WO_ReelIDLog_Add();
                    logTxn._Condition.Action = action;
                    logTxn._Condition.WONO = woNo ?? string.Empty;
                    logTxn._Condition.ReelID = reelId ?? string.Empty;
                    logTxn._Condition.MatNo = matNo ?? string.Empty;
                    logTxn._Condition.BatchNo = batchNo ?? string.Empty;
                    logTxn._Condition.UserID = userId ?? string.Empty;
                    logTxn._Condition.CreateUser = userId ?? string.Empty;
                    logTxn._Condition.CreateDate = DateTime.Now;
                    logTxn._Condition.Status = "11";

                    logTxn.clientExecute(_txnConn);

                    if (logTxn.IsTxnError)
                    {
                        _logger.LogWarning("寫入 Reel 歷程日誌失敗: {Msg}", logTxn.TxnErrorMsg);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "呼叫 WO_ReelIDLog_Add 發生異常: {Msg}", ex.Message);
                    return false;
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> SaveReelCheckRecordAsync(string woNo, string machNo, string reelId, string matNo, string batchNo, int reelCount, int matCount, int tpCount, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return false;

                try
                {
                    var saveTxn = new WO_ReelCheck_Save();
                    var checkItem = new WO_ReelCheck
                    {
                        WoNo = woNo ?? string.Empty,
                        EqpNo = machNo ?? string.Empty,
                        ReelID = reelId ?? string.Empty,
                        MatNo = matNo ?? string.Empty,
                        TPNO = batchNo ?? string.Empty,
                        ReelIDCount = reelCount,
                        MatNoCount = matCount,
                        TPNoCount = tpCount,
                        UpdateTime = DateTime.Now,
                        ComputerName = Environment.MachineName
                    };

                    saveTxn._WoReelCheckList.Add(checkItem);
                    saveTxn.clientExecute(_txnConn);

                    if (saveTxn.IsTxnError)
                    {
                        _logger.LogWarning("儲存防呆計數紀錄失敗: {Msg}", saveTxn.TxnErrorMsg);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "呼叫 WO_ReelCheck_Save 發生異常: {Msg}", ex.Message);
                    return false;
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> BlockWorkOrderAsync(string woNo, string reasonCode, string remark, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return false;

                try
                {
                    _logger.LogCritical("觸發 MES 重大防混鎖單！工單: [{WoNo}], 原因代碼: [{Code}], 備註: [{Remark}]", woNo, reasonCode, remark);

                    // 1. 查詢工單當前工作中心
                    string currentWc = string.Empty;
                    try
                    {
                        var woGet = new Workorder_Get { _WoNo = woNo.Trim() };
                        woGet.clientExecute(_txnConn);
                        if (!woGet.IsTxnError && woGet.Result?._ItemList != null && woGet.Result._ItemList.Count > 0)
                        {
                            currentWc = woGet.Result._ItemList[0].CurrentWorkCenter ?? string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "鎖單前查詢工單工作中心異常: {Msg}", ex.Message);
                    }

                    // 2. 組裝並發送 WO_Block_Save (原廠鎖單規格)
                    var blockTxn = new WO_Block_Save();
                    var blockItem = new WO_Block
                    {
                        WONo = woNo.Trim(),
                        WorkCenterNo = currentWc,
                        BlockCode = string.IsNullOrWhiteSpace(reasonCode) || reasonCode.StartsWith("NG-") ? "C2" : reasonCode,
                        BlockReason = $"[{reasonCode}] {remark}",
                        Enable = true,
                        Remark = remark ?? string.Empty,
                        CreateUser = "SYSTEM",
                        ChangeUser = _options.ClientIdPrefix,
                        CreateDate = DateTime.Now,
                        ChangeDate = DateTime.Now
                    };
                    blockTxn._ItemList.Add(blockItem);
                    blockTxn.clientExecute(_txnConn);

                    if (blockTxn.IsTxnError)
                    {
                        _logger.LogError("MES 執行 WO_Block_Save 失敗: {Msg}", blockTxn.TxnErrorMsg);
                        return false;
                    }

                    _logger.LogInformation("MES 實體鎖單成功: WO=[{WoNo}], BlockCode=[{Code}]", woNo, blockItem.BlockCode);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "呼叫 MES 鎖定工單失敗: {Msg}", ex.Message);
                    return false;
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<WorkOrderStatusCheckResult> CheckWorkOrderStatusAsync(string woNo, string workCenterNo, DateTime initialInTime, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(woNo)) return WorkOrderStatusCheckResult.Ok();

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return WorkOrderStatusCheckResult.Ok();

                try
                {
                    var confirmTxn = new WOConfirm_Get { _WoNo = woNo.Trim() };
                    confirmTxn.clientExecute(_txnConn);

                    if (confirmTxn.IsTxnError)
                    {
                        _logger.LogWarning("WOConfirm_Get 查詢失敗: {Msg}", confirmTxn.TxnErrorMsg);
                        return WorkOrderStatusCheckResult.Ok(); // 通訊失敗時不誤發鎖單
                    }

                    if (confirmTxn.Result == null) return WorkOrderStatusCheckResult.Ok();

                    var step = confirmTxn.Result.GetDesignatedStep(workCenterNo)
                               ?? confirmTxn.Result.GetDesignatedStep("TP");

                    if (step == null)
                    {
                        return WorkOrderStatusCheckResult.Ng("NG-0201", "工單狀態異動(NG-0201), 工單可能已被刪除 Confirm。");
                    }

                    if (initialInTime != DateTime.MinValue && step.IsInTimeNG(initialInTime))
                    {
                        return WorkOrderStatusCheckResult.Ng("NG-0202",
                            $"工單狀態異動(NG-0202), 工單已被重新過帳 (進站時間 {initialInTime:yyyy-MM-dd HH:mm:ss} 與最新時間 {step.InTime:yyyy-MM-dd HH:mm:ss} 不一致)");
                    }

                    if (string.Equals(step.Finished, "Y", StringComparison.OrdinalIgnoreCase))
                    {
                        return WorkOrderStatusCheckResult.Ng("NG-0203", "工單狀態變動(NG-0203), 工單出站狀態改變: 工單已出站");
                    }

                    return WorkOrderStatusCheckResult.Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "檢驗工單動態狀態發生異常: {Msg}", ex.Message);
                    return WorkOrderStatusCheckResult.Ok();
                }
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<string?> QueryWorkOrderByBatchNoAsync(string batchNo, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(batchNo)) return null;

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return null;

                try
                {
                    var txn = new WO_ReelID_Get_WCF { _BatchNo = batchNo.Trim() };
                    txn.clientExecute(_txnConn);

                    if (!txn.IsTxnError && txn.Result?._itemList != null && txn.Result._itemList.Count > 0)
                    {
                        return txn.Result._itemList[0].WONo;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "反查全廠 1D 批號歸屬工單失敗: {Msg}", ex.Message);
                }

                return null;
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> IsMaterialExistsAsync(string matNo, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(matNo)) return false;

        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return false;

                try
                {
                    var txn = new MatView_Get { _MatNo = matNo.Trim() };
                    txn.clientExecute(_txnConn);

                    if (!txn.IsTxnError && txn.Result?._itemList != null && txn.Result._itemList.Count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "查詢料號主檔是否存在失敗: {Msg}", ex.Message);
                }

                return false;
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> FinishWorkOrderAsync(string woNo, ReelItem? lastReel, string machNo, string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(woNo)) return false;

        return await Task.Run(async () =>
        {
            _logger.LogInformation("開始執行工單結案 (WoFinish): WO=[{WoNo}]", woNo);

            if (lastReel != null)
            {
                DateTime now = DateTime.Now;
                DateTime start = lastReel.StartTime == DateTime.MinValue ? now : lastReel.StartTime;
                await UpdateReelStatusAsync(lastReel.ReelId, "11", machNo, userId, start, now, ct).ConfigureAwait(false);
                await AddReelLogAsync(woNo, lastReel.ReelId, lastReel.MatNo, lastReel.BatchNo, userId, "00005", ct).ConfigureAwait(false);
                await SaveReelCheckRecordAsync(woNo, machNo, lastReel.ReelId, lastReel.MatNo, lastReel.BatchNo, lastReel.ReelScanCount, lastReel.MatNoScanCount, lastReel.TPNoScanCount, ct).ConfigureAwait(false);
            }

            return true;
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<string?> QueryOtherWorkOrderByBarcodeAsync(string barcode, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            lock (_connLock)
            {
                EnsureConnection();
                if (_txnConn == null) return null;

                try
                {
                    var reelTxn = new WO_ReelID_Get { _ReelID = barcode.Trim() };
                    reelTxn.clientExecute(_txnConn);

                    if (!reelTxn.IsTxnError && reelTxn.Result?._ItemList != null && reelTxn.Result._ItemList.Count > 0)
                    {
                        return reelTxn.Result._ItemList[0].WONo;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "反查全廠 Reel 歸屬工單: {Msg}", ex.Message);
                }

                return null;
            }
        }, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> SavePackingAsync(string woNo, string batchNo, string matNo, string reelId, string userId, string workCenterNo, CancellationToken ct = default)
    {
        return await Task.FromResult(true);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        lock (_connLock)
        {
            if (_txnConn != null)
            {
                try
                {
                    _txnConn.Dispose();
                }
                catch { }
                _txnConn = null;
            }
        }
    }
}
