using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EAPDashboard.Models;
using EAPDashboard.Models.Smt;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Base;

namespace EAPDashboard.Services
{
    /// <summary>
    /// SMT 線別專用業務邏輯服務 (負責 Panel In / Panel Out / 抽片比對結單與 MES 串接)
    /// </summary>
    public class SmtLineService
    {
        public LineConfig Config { get; }
        private readonly SemaphoreSlim _lineLock = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<string, SmtWorkOrderState> _woCache = new ConcurrentDictionary<string, SmtWorkOrderState>(StringComparer.OrdinalIgnoreCase);

        private string _station1LastWo;
        private string _station2LastWo;

        public event Action<LogLevel, string> LogReceived;

        /// <summary>
        /// MES 呼叫委派 (支援依賴注入與單元測試 Mock)
        /// </summary>
        public Func<MESBaseMessage, Task<MESBaseMessage>> MesCaller { get; set; }

        public SmtLineService(LineConfig config, Func<MESBaseMessage, Task<MESBaseMessage>> mesCaller = null)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            MesCaller = mesCaller ?? DefaultCallMesAsync;

            // 啟動時從 Local DB 載入進行中工單與歷史狀態
            LoadStatesFromDatabase();
        }

        private void LoadStatesFromDatabase()
        {
            try
            {
                var activeList = DatabaseHelper.GetActiveSmtWorkOrders(Config.LineId);
                foreach (var wo in activeList)
                {
                    _woCache[wo.WorkOrder] = wo;
                }

                var (st1Wo, st2Wo) = DatabaseHelper.GetSmtLineCurrent(Config.LineId);
                _station1LastWo = st1Wo;
                _station2LastWo = st2Wo;

                OnLog(LogLevel.Info, $"[SMT Line {Config.LineId}] Loaded {activeList.Count} active WO(s). St1_Last_WO: {_station1LastWo ?? "None"}, St2_Last_WO: {_station2LastWo ?? "None"}");
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId}] LoadStatesFromDatabase error: {ex.Message}");
            }
        }

        private void OnLog(LogLevel level, string msg)
        {
            LogReceived?.Invoke(level, msg);
            System.Diagnostics.Debug.WriteLine($"[{level}] {msg}");
        }

        /// <summary>
        /// 預設呼叫 MES WebService 實作
        /// </summary>
        private static async Task<MESBaseMessage> DefaultCallMesAsync(MESBaseMessage request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // 利用動態調用 MESMessageFactory.CallMes
                    var method = typeof(MESMessageFactory).GetMethods()
                        .FirstOrDefault(m => m.Name == "CallMes" && m.IsGenericMethod && m.GetParameters().Length == 1);

                    if (method != null)
                    {
                        var reqType = request.GetType();
                        var replyTypeName = reqType.FullName.Replace("Request", "Reply");
                        var replyType = reqType.Assembly.GetType(replyTypeName);
                        if (replyType != null)
                        {
                            var genericMethod = method.MakeGenericMethod(reqType, replyType);
                            return (MESBaseMessage)genericMethod.Invoke(null, new object[] { request });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[MES CALL ERROR] {ex.Message}");
                }
                return null;
            });
        }

        /// <summary>
        /// 從 Panel ID 解析工單號碼 (預設去除末段序號，例如 WO_A_001 -> WO_A, WO01-001 -> WO01, WO-01_001 -> WO-01)
        /// </summary>
        public static string ExtractWorkOrder(string panelId)
        {
            if (string.IsNullOrWhiteSpace(panelId)) return string.Empty;
            string clean = panelId.Trim();
            int underscoreIdx = clean.LastIndexOf('_');
            if (underscoreIdx > 0) return clean.Substring(0, underscoreIdx);

            int hyphenIdx = clean.LastIndexOf('-');
            if (hyphenIdx > 0) return clean.Substring(0, hyphenIdx);

            return clean;
        }

        /// <summary>
        /// 取得或初始化指定工單在本地快取之狀態
        /// </summary>
        public SmtWorkOrderState GetOrCreateWorkOrderState(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return null;

            return _woCache.GetOrAdd(workOrder, wo =>
            {
                var dbState = DatabaseHelper.GetSmtWorkOrderState(Config.LineId, wo);
                if (dbState != null) return dbState;

                var newState = new SmtWorkOrderState
                {
                    LineId = Config.LineId,
                    WorkOrder = wo,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                };
                DatabaseHelper.SaveSmtWorkOrderState(newState);
                return newState;
            });
        }

        /// <summary>
        /// 取得線別當前所有在製工單狀態清單
        /// </summary>
        public List<SmtWorkOrderState> GetCurrentWorkOrders()
        {
            return _woCache.Values.OrderByDescending(w => w.UpdatedTime).ToList();
        }

        public (string St1LastWo, string St2LastWo) GetCurrentLastWos()
        {
            return (_station1LastWo, _station2LastWo);
        }

        #region 第一站 (Station 1) 業務邏輯

        /// <summary>
        /// 第一站 Panel In 事件處理 (硬體觸發 + 換單防呆進站 + Component進站)
        /// </summary>
        public async Task<bool> HandleStation1PanelInAsync(string panelId, string userId = "SMT_AUTO")
        {
            if (string.IsNullOrWhiteSpace(panelId)) return false;
            string currentWo = ExtractWorkOrder(panelId);
            if (string.IsNullOrWhiteSpace(currentWo)) return false;

            await _lineLock.WaitAsync();
            try
            {
                OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St1] Panel In: Panel={panelId}, Resolved WO={currentWo}");

                var woState = GetOrCreateWorkOrderState(currentWo);

                // 判斷當前 WO 是否與上一筆 WO 相同
                bool isSameWo = string.Equals(currentWo, _station1LastWo, StringComparison.OrdinalIgnoreCase);

                if (!isSameWo)
                {
                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St1] WorkOrder changed! Old: {_station1LastWo ?? "None"} -> New: {currentWo}");

                    // 檢查上一筆 WO 是否尚未出站 (尾數批防呆)
                    if (!string.IsNullOrEmpty(_station1LastWo) && _woCache.TryGetValue(_station1LastWo, out var lastWoState))
                    {
                        if (!lastWoState.Station1_Finished && lastWoState.Station1_Out_Count > 0)
                        {
                            int lastActualQty = lastWoState.Station1_Out_Count;
                            lastWoState.Station1_Actual_QTY = lastActualQty;
                            lastWoState.Station1_Finished = true;
                            lastWoState.Station1_FinishedTime = DateTime.Now;
                            lastWoState.UpdatedTime = DateTime.Now;

                            OnLog(LogLevel.Warn, $"[SMT Line {Config.LineId} - St1] Previous WO {_station1LastWo} tail batch auto-checkout. Actual_QTY={lastActualQty}");

                            // 呼叫 MES: 上一筆工單出站 (Track-Out)
                            await CallMesTrackOutAsync(_station1LastWo, lastActualQty, userId, station: null);

                            DatabaseHelper.SaveSmtWorkOrderState(lastWoState);
                        }
                    }

                    // 呼叫 MES: 新工單進站 (Track-In)
                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St1] Calling MES Track-In for new WO: {currentWo}");
                    await CallMesTrackInAsync(currentWo, userId, station: null);

                    _station1LastWo = currentWo;
                    DatabaseHelper.SaveSmtLineCurrent(Config.LineId, _station1LastWo, _station2LastWo);
                }

                woState.Station1_In_Count += 1;
                woState.UpdatedTime = DateTime.Now;
                DatabaseHelper.SaveSmtWorkOrderState(woState);

                // 呼叫 MES: Component 進站
                await CallMesComponentCheckInAsync(currentWo, panelId, userId, station: null);

                return true;
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId} - St1] HandleStation1PanelIn error: {ex.Message}");
                return false;
            }
            finally
            {
                _lineLock.Release();
            }
        }

        /// <summary>
        /// 第一站 Panel Out 事件處理 (硬體觸發 + 滿批次出站)
        /// </summary>
        public async Task<bool> HandleStation1PanelOutAsync(string panelId, string userId = "SMT_AUTO")
        {
            if (string.IsNullOrWhiteSpace(panelId)) return false;
            string currentWo = ExtractWorkOrder(panelId);
            if (string.IsNullOrWhiteSpace(currentWo)) return false;

            await _lineLock.WaitAsync();
            try
            {
                var woState = GetOrCreateWorkOrderState(currentWo);
                woState.Station1_Out_Count += 1;
                woState.UpdatedTime = DateTime.Now;

                OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St1] Panel Out: Panel={panelId}, WO={currentWo}, St1_Count={woState.Station1_Out_Count}");

                // 判斷是否達到物理滿批次 (預設 10 片)
                if (woState.Station1_Out_Count >= Config.BatchCapacity && !woState.Station1_Finished)
                {
                    int actualQty = Config.BatchCapacity;
                    woState.Station1_Actual_QTY = actualQty;
                    woState.Station1_Finished = true;
                    woState.Station1_FinishedTime = DateTime.Now;

                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St1] WO {currentWo} reached full capacity ({Config.BatchCapacity}). Calling MES Track-Out.");

                    // 呼叫 MES: 工單出站 (Track-Out)
                    await CallMesTrackOutAsync(currentWo, actualQty, userId, station: null);
                }

                DatabaseHelper.SaveSmtWorkOrderState(woState);
                return true;
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId} - St1] HandleStation1PanelOut error: {ex.Message}");
                return false;
            }
            finally
            {
                _lineLock.Release();
            }
        }

        #endregion

        #region 第二站 (Station 2 / Post AOI) 業務邏輯

        /// <summary>
        /// 第二站 Panel In 事件處理 (換單防呆進站)
        /// </summary>
        public async Task<bool> HandleStation2PanelInAsync(string panelId, string userId = "SMT_AUTO")
        {
            if (string.IsNullOrWhiteSpace(panelId)) return false;
            string currentWo = ExtractWorkOrder(panelId);
            if (string.IsNullOrWhiteSpace(currentWo)) return false;

            await _lineLock.WaitAsync();
            try
            {
                OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St2] Panel In: Panel={panelId}, Resolved WO={currentWo}");

                var woState = GetOrCreateWorkOrderState(currentWo);
                bool isSameWo = string.Equals(currentWo, _station2LastWo, StringComparison.OrdinalIgnoreCase);

                if (!isSameWo)
                {
                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St2] WorkOrder changed! Old: {_station2LastWo ?? "None"} -> New: {currentWo}");

                    // 尾數防呆: 上一筆若尚未在第二站出站，且已累積數量
                    if (!string.IsNullOrEmpty(_station2LastWo) && _woCache.TryGetValue(_station2LastWo, out var lastWoState))
                    {
                        if (!lastWoState.Station2_Finished && lastWoState.Station2_Out_Count > 0)
                        {
                            int lastActualQty = lastWoState.Station2_Out_Count;
                            lastWoState.Station2_Actual_QTY = lastActualQty;
                            lastWoState.Station2_Finished = true;
                            lastWoState.Station2_FinishedTime = DateTime.Now;
                            lastWoState.UpdatedTime = DateTime.Now;

                            OnLog(LogLevel.Warn, $"[SMT Line {Config.LineId} - St2] Previous WO {_station2LastWo} tail batch auto-checkout. Actual_QTY={lastActualQty}");

                            // 呼叫 MES: 第二站工單出站 (帶入 Station = Post_AOI)
                            await CallMesTrackOutAsync(_station2LastWo, lastActualQty, userId, station: "Post_AOI");

                            DatabaseHelper.SaveSmtWorkOrderState(lastWoState);
                        }
                    }

                    // 呼叫 MES: 第二站新工單進站 (帶入 Station = Post_AOI)
                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St2] Calling MES Track-In for new WO: {currentWo} (Station=Post_AOI)");
                    await CallMesTrackInAsync(currentWo, userId, station: "Post_AOI");

                    _station2LastWo = currentWo;
                    DatabaseHelper.SaveSmtLineCurrent(Config.LineId, _station1LastWo, _station2LastWo);
                }

                woState.Station2_In_Count += 1;
                woState.UpdatedTime = DateTime.Now;
                DatabaseHelper.SaveSmtWorkOrderState(woState);

                return true;
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId} - St2] HandleStation2PanelIn error: {ex.Message}");
                return false;
            }
            finally
            {
                _lineLock.Release();
            }
        }

        /// <summary>
        /// 第二站 Panel Out 事件處理 (滿批次 10 片出站 或 等於第一站實際結單數量出站)
        /// </summary>
        public async Task<bool> HandleStation2PanelOutAsync(string panelId, string userId = "SMT_AUTO")
        {
            if (string.IsNullOrWhiteSpace(panelId)) return false;
            string currentWo = ExtractWorkOrder(panelId);
            if (string.IsNullOrWhiteSpace(currentWo)) return false;

            await _lineLock.WaitAsync();
            try
            {
                var woState = GetOrCreateWorkOrderState(currentWo);
                woState.Station2_Out_Count += 1;
                woState.UpdatedTime = DateTime.Now;

                OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St2] Panel Out: Panel={panelId}, WO={currentWo}, St2_Count={woState.Station2_Out_Count}");

                // 核心出站時機判定:
                // 1. 滿物理上限 10 片
                // 2. 第一站已結單，且第二站累積刷片數達到了第一站最終結單數量 (例如抽片後第一站結單 6 片，第二站刷滿 6 片自動出站)
                bool isFullBatch = woState.Station2_Out_Count >= Config.BatchCapacity;
                bool isMatchStation1Final = woState.Station1_Finished && 
                                            woState.Station1_Actual_QTY.HasValue && 
                                            woState.Station2_Out_Count >= woState.Station1_Actual_QTY.Value;

                if ((isFullBatch || isMatchStation1Final) && !woState.Station2_Finished)
                {
                    int actualQty = woState.Station2_Out_Count;
                    woState.Station2_Actual_QTY = actualQty;
                    woState.Station2_Finished = true;
                    woState.Station2_FinishedTime = DateTime.Now;

                    string reason = isFullBatch ? $"Full Batch ({Config.BatchCapacity})" : $"Matched Station1 Final Qty ({woState.Station1_Actual_QTY})";
                    OnLog(LogLevel.Info, $"[SMT Line {Config.LineId} - St2] WO {currentWo} Triggered Track-Out. Reason={reason}, Out_Count={actualQty}");

                    // 呼叫 MES: 第二站工單出站 (帶入 Station = Post_AOI)
                    await CallMesTrackOutAsync(currentWo, actualQty, userId, station: "Post_AOI");
                }

                DatabaseHelper.SaveSmtWorkOrderState(woState);
                return true;
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId} - St2] HandleStation2PanelOut error: {ex.Message}");
                return false;
            }
            finally
            {
                _lineLock.Release();
            }
        }

        #endregion

        #region WebUI 人工介入 (強制結單出站)

        /// <summary>
        /// 人員透過 WebUI 強制出站 (可指定 Station 1 或 Station 2)
        /// </summary>
        public async Task<(bool Success, string Message, int ActualQty)> ForceTrackOutAsync(string stationCode, string workOrder, string userId = "WEB_USER")
        {
            if (string.IsNullOrWhiteSpace(workOrder))
            {
                return (false, "工單代號不得為空", 0);
            }

            await _lineLock.WaitAsync();
            try
            {
                var woState = GetOrCreateWorkOrderState(workOrder);
                bool isStation1 = string.Equals(stationCode, "1", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(stationCode, "Station1", StringComparison.OrdinalIgnoreCase);

                if (isStation1)
                {
                    int actualQty = woState.Station1_Out_Count;
                    woState.Station1_Actual_QTY = actualQty;
                    woState.Station1_Finished = true;
                    woState.Station1_FinishedTime = DateTime.Now;
                    woState.UpdatedTime = DateTime.Now;

                    OnLog(LogLevel.Warn, $"[SMT Line {Config.LineId}] WebUI Manual Force Track-Out for Station 1: WO={workOrder}, Qty={actualQty}, User={userId}");

                    await CallMesTrackOutAsync(workOrder, actualQty, userId, station: null);
                    DatabaseHelper.SaveSmtWorkOrderState(woState);

                    return (true, $"第一站工單 {workOrder} 強制出站成功，結單數量: {actualQty}", actualQty);
                }
                else
                {
                    int actualQty = woState.Station2_Out_Count;
                    woState.Station2_Actual_QTY = actualQty;
                    woState.Station2_Finished = true;
                    woState.Station2_FinishedTime = DateTime.Now;
                    woState.UpdatedTime = DateTime.Now;

                    OnLog(LogLevel.Warn, $"[SMT Line {Config.LineId}] WebUI Manual Force Track-Out for Station 2: WO={workOrder}, Qty={actualQty}, User={userId}");

                    await CallMesTrackOutAsync(workOrder, actualQty, userId, station: "Post_AOI");
                    DatabaseHelper.SaveSmtWorkOrderState(woState);

                    return (true, $"第二站工單 {workOrder} 強制出站成功，結單數量: {actualQty}", actualQty);
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT Line {Config.LineId}] ForceTrackOut error: {ex.Message}");
                return (false, $"強制出站失敗: {ex.Message}", 0);
            }
            finally
            {
                _lineLock.Release();
            }
        }

        #endregion

        #region MES API 呼叫包裝

        private static (string Result, string Message) GetReplyInfo(MESBaseMessage reply)
        {
            if (reply == null) return ("Fail", "Reply is null");
            try
            {
                var resProp = reply.GetType().GetProperty("Result");
                var msgProp = reply.GetType().GetProperty("Message");
                string res = resProp?.GetValue(reply)?.ToString() ?? "Unknown";
                string msg = msgProp?.GetValue(reply)?.ToString() ?? "";
                return (res, msg);
            }
            catch
            {
                return ("Unknown", "");
            }
        }

        private string GetFirstEquipmentId(string station)
        {
            if (string.Equals(station, "Post_AOI", StringComparison.OrdinalIgnoreCase))
            {
                return Config.Station2?.EquipmentIds?.FirstOrDefault() ?? $"{Config.LineId}_AOI";
            }
            return Config.Station1?.EquipmentIds?.FirstOrDefault() ?? $"{Config.LineId}_ST1";
        }

        private async Task CallMesTrackInAsync(string workOrder, string userId, string station)
        {
            var req = new WOCHECKINRequest
            {
                WONO = workOrder,
                EqpNo = GetFirstEquipmentId(station),
                UserID = userId
            };

            if (!string.IsNullOrEmpty(station))
            {
                try
                {
                    var prop = req.GetType().GetProperty("Station");
                    prop?.SetValue(req, station);
                }
                catch { }
            }

            OnLog(LogLevel.Info, $"[MES SEND] WOCHECKINRequest: WO={workOrder}, Eqp={req.EqpNo}, Station={station ?? "None"}");
            var reply = await MesCaller(req);
            var info = GetReplyInfo(reply);
            OnLog(LogLevel.Info, $"[MES RECV] WOCHECKINReply for {workOrder}: Result={info.Result}, Message={info.Message}");
        }

        private async Task CallMesTrackOutAsync(string workOrder, int actualQty, string userId, string station)
        {
            var req = new WOCHECKOUTRequest
            {
                WONO = workOrder,
                EqpNo = GetFirstEquipmentId(station),
                UserID = userId,
                OutPut = actualQty.ToString()
            };

            if (!string.IsNullOrEmpty(station))
            {
                try
                {
                    var prop = req.GetType().GetProperty("Station");
                    prop?.SetValue(req, station);
                }
                catch { }
            }

            OnLog(LogLevel.Info, $"[MES SEND] WOCHECKOUTRequest: WO={workOrder}, Eqp={req.EqpNo}, OutPut={actualQty}, Station={station ?? "None"}");
            var reply = await MesCaller(req);
            var info = GetReplyInfo(reply);
            OnLog(LogLevel.Info, $"[MES RECV] WOCHECKOUTReply for {workOrder}: Result={info.Result}, Message={info.Message}");
        }

        private async Task CallMesComponentCheckInAsync(string workOrder, string panelId, string userId, string station)
        {
            var req = new ComponentBatchInRequest
            {
                WONO = workOrder,
                EqpNo = GetFirstEquipmentId(station),
                UserID = userId
            };

            OnLog(LogLevel.Info, $"[MES SEND] ComponentBatchInRequest: WO={workOrder}, PanelID={panelId}, Eqp={req.EqpNo}");
            var reply = await MesCaller(req);
            var info = GetReplyInfo(reply);
            OnLog(LogLevel.Info, $"[MES RECV] ComponentBatchInReply for Panel {panelId}: Result={info.Result}, Message={info.Message}");
        }

        #endregion
    }
}
