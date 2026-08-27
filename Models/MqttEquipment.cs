using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Models;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Protocol.Drivers.Secs;
using Protocol.LocalData;

namespace EAPDashboard.Models
{
    public class MqttEquipment : EquipmentBase
    {
        private static readonly HashSet<string> _processedMessageIds = new HashSet<string>();
        private static readonly object _msgIdLock = new object();

        private CancellationTokenSource _linkedCts; 
        private System.Timers.Timer _heartbeatTimer;
        private DateTime _lastResponseTime = DateTime.MinValue;
        private int _heartbeatInterval = 30;

        private string _commandTopic;
        private string _reportTopic;
        private MqttBus _bus;

        private class TrackInContext
        {
            public List<string> WorkOrder { get; set; } = new List<string>();
            public string RecipeID { get; set; }
            public string UserID { get; set; }
            public List<string> CassetteID { get; set; } = new List<string>();
            public DateTime Timestamp { get; set; }
            public List<string> Panels { get; set; }
            public string TransactionID { get; set; }
        }
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, TrackInContext> _pendingTrackIn = new System.Collections.Concurrent.ConcurrentDictionary<string, TrackInContext>();
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        private string _currentStatusCode = "3";
        public string Topic { get; set; }
        public string StatusType { get; set; } = "Delta";
        public SecsDictionary EqpDictionary { get; set; }

        public MqttEquipment()
        {
            Protocol = "MQTT";
            Connection = ConnectionStatus.Disconnected;
        }

        private static readonly JsonSerializerSettings _logSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        private static readonly Dictionary<int, EquipmentState> DeltaStatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Running }, { 6, EquipmentState.Running },
           { 1, EquipmentState.Alarm }, { 10, EquipmentState.Alarm }, { 11, EquipmentState.Alarm },
           { 2, EquipmentState.Paused }, { 3, EquipmentState.Paused }, { 4, EquipmentState.Paused },
           { 5, EquipmentState.Paused }, { 7, EquipmentState.Paused }, { 8, EquipmentState.Paused },
           { 9, EquipmentState.Paused }, { 12, EquipmentState.Paused }
        };

        private static readonly Dictionary<int, EquipmentState> CyntecStatusMap = new Dictionary<int, EquipmentState>()
        {
           { 1, EquipmentState.Running },
           { 2, EquipmentState.Alarm },
           { 3, EquipmentState.Idle }
        };

        public Dictionary<int, EquipmentState> GetActiveStatusMap()
        {
            if ("Cyntec".Equals(StatusType, StringComparison.OrdinalIgnoreCase))
            {
                return CyntecStatusMap;
            }
            return DeltaStatusMap;
        }

        private void LoadDictionary()
        {
            string cleanId = Id?.Trim();
            if (string.IsNullOrEmpty(cleanId))
            {
                EqpDictionary = new SecsDictionary();
                return;
            }

            string dictPath = !string.IsNullOrEmpty(DictionaryPath)
                ? (Path.IsPathRooted(DictionaryPath) ? DictionaryPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictionaryPath))
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", $"{cleanId}.json");

            if (File.Exists(dictPath))
            {
                EqpDictionary = SecsDictionary.Load(dictPath);
                OnLogReceived(LogLevel.Info, $"[MQTT] Dictionary loaded for {cleanId} from {dictPath}");
            }
            else
            {
                EqpDictionary = new SecsDictionary();
                // 非 SECS / MQTT 協定機台若無字典檔，採用預設空的字典並靜默處理
            }
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            LoadAlarmMappings();
            LoadDictionary();
            LoadCachesFromDisk();

            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _commandTopic = $"{Topic?.TrimEnd('/')}/Command";
            _reportTopic = $"{Topic?.TrimEnd('/')}/Report";

            OnLogReceived(LogLevel.Info, $"[START] MQTT module started for {Id}. Base Topic: {Topic}, Command Topic: {_commandTopic}, Report Topic: {_reportTopic}");

            if (_bus != null)
            {
                _bus.Connected += OnBrokerConnected;
                _bus.SubscribeAsync(_reportTopic, (topic, payload) => HandleReport(payload));
            }

            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval * 1000);
            _heartbeatTimer.Elapsed += (s, e) => CheckHeartbeat();
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;

            _ = StartStatusBufferProcessor(_linkedCts.Token);
            _ = MonitorTrackInTimeout(_linkedCts.Token);
            _ = SendAreYouThere();
            _ = SendStatusRequest();
            _ = StartPeriodicStatusReportLoop(_linkedCts.Token);
            StartHourlyAvailabilityTimer(_linkedCts.Token);
        }

        private void OnBrokerConnected()
        {
            OnLogReceived(LogLevel.Info, $"[MQTT] Broker connection restored. Sending probes for {Id}...");
            _ = SendAreYouThere();
            _ = SendStatusRequest();
        }

        private async Task MonitorTrackInTimeout(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (_pendingTrackIn.TryGetValue(Id, out var ctx))
                    {
                        if ((DateTime.Now - ctx.Timestamp).TotalSeconds > 30)
                        {
                            if (_pendingTrackIn.TryRemove(Id, out _))
                            {
                                OnLogReceived(LogLevel.Error, $"[TrackIn] Timeout waiting for ReplyRemoteCMD (PP-SELECT) for WO: {ctx.WorkOrder}");
                                await SendReply(new ReplyTrackInReqMessage 
                                { 
                                    CMD = CommandType.ReplyTrackInReq, 
                                    RtnResult = RtnResult.PASS == RtnResult.FAIL ? RtnResult.PASS : RtnResult.FAIL, // Use FAIL
                                    RtnMsg = "Timeout waiting for machine to change recipe (30s)." 
                                });
                            }
                        }
                    }
                }
                catch (Exception ex) { OnLogReceived(LogLevel.Error, $"TrackIn Monitor Error: {ex.Message}"); }
                await Task.Delay(5000, ct);
            }
        }

        private async void HandleReport(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            // 智慧拒收邏輯
            if (IsStopping)
            {
                // 解析出指令類型 (簡化判斷，通常 Request 結尾不含 Reply 字樣)
                try {
                    var jo = JObject.Parse(json);
                    string innerJson = jo["Payload"]?.ToString();
                    if (!string.IsNullOrEmpty(innerJson))
                    {
                        var innerJo = JObject.Parse(innerJson);
                        string cmdStr = innerJo["CMD"]?.ToString() ?? "";
                        // 如果是 Request 類型的指令且不是 Reply
                        if (cmdStr.ToUpper().Contains("REQ") && !cmdStr.ToUpper().Contains("REPLY"))
                        {
                            OnLogReceived(LogLevel.Warn, string.Format("[MQTT] Rejecting Request during shutdown: {0}", cmdStr));
                            // TODO: 可在此實作回傳拒絕訊息
                            return;
                        }
                    }
                } catch { }
            }

            IncrementTask();
            _lastResponseTime = DateTime.Now;

            try {
                if (string.IsNullOrWhiteSpace(json)) return;

                string messageId = null;
                string innerJson = null;

                var jObj = JObject.Parse(json);
                if (jObj.TryGetValue("Payload", StringComparison.OrdinalIgnoreCase, out var payloadToken) && payloadToken != null && payloadToken.Type != JTokenType.Null)
                {
                    messageId = jObj["MessageId"]?.ToString();
                    innerJson = payloadToken.Type == JTokenType.String ? payloadToken.ToString() : payloadToken.ToString(Formatting.None);
                }
                else
                {
                    messageId = jObj["MessageId"]?.ToString();
                    innerJson = json;
                }

                if (string.IsNullOrEmpty(innerJson)) return;

                // Idempotency Filter
                if (!string.IsNullOrEmpty(messageId))
                {
                    lock (_msgIdLock)
                    {
                        if (_processedMessageIds.Contains(messageId)) return;
                        _processedMessageIds.Add(messageId);
                        if (_processedMessageIds.Count > 1000) _processedMessageIds.Remove(_processedMessageIds.First());
                    }
                }

                if (Connection != ConnectionStatus.Connected)
                {
                    Connection = ConnectionStatus.Connected;
                    OnLogReceived(LogLevel.Info, string.Format("[MQTT] Connection restored for {0}.", Id));
                }

                BaseMessage msg = MessageFactory.Create(innerJson);
                if (msg.CMD != CommandType.AreYouThere && msg.CMD != CommandType.IamHere)
                {
                    OnLogReceived(LogLevel.Info, string.Format("[RECV] {0}", JsonConvert.SerializeObject(msg, _logSettings)));
                }

                switch (msg.CMD)
                {
                    case CommandType.AreYouThere: await SendReply(new IamHereMessage { CMD = CommandType.IamHere }); break;
                    case CommandType.IamHere: break;
                    case CommandType.ReplyStatusReq:
                        var rs = (ReplyStatusReqMessage)msg;
                        int currentStatusInt = int.TryParse(rs.CurrentStatus?.ToString(), out int sval) ? sval : 0;
                        _currentStatusCode = currentStatusInt.ToString();
                        if (GetActiveStatusMap().TryGetValue(currentStatusInt, out var st)) 
                        {
                            State = st;
                            string errorCode = (st == EquipmentState.Alarm) ? TransformAlarmCode(rs.AlarmCode) : "0";
                            HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = _currentStatusCode, ErrorCode = errorCode });
                        }
                        OnLogReceived(LogLevel.Info, string.Format("Status Reply Recv: {0}, Status: {1}, Alarm: {2}", Id, rs.CurrentStatus, rs.AlarmCode ?? "None"));
                        break;
                    case CommandType.TrackInReq:
                        var ti = (TrackInReqMessage)msg;
                        List<string> tiCassettes = ti.CassetteID ?? new List<string>();
                        List<string> tiWos = ti.WorkOrder ?? new List<string>();
                        List<string> panels = new List<string>();
                        string tir = await ProcessTrackInAsync(tiCassettes, tiWos, panels, ti.UserID);
                        if (tir.StartsWith("PP-SELECT")) {
                            string[] tp = tir.Split('|');
                            _pendingTrackIn[Id] = new TrackInContext { 
                                WorkOrder = tiWos, 
                                RecipeID = tp[2], 
                                UserID = ti.UserID, 
                                CassetteID = tiCassettes,
                                Timestamp = DateTime.Now,
                                Panels = panels,
                                TransactionID = ti.TransactionID
                            };
                            string finalRecipe = tp[2];

                            var rcmd = new RemoteCMDMessage { 
                                CMD = CommandType.RemoteCMD, 
                                RemoteCMDType = RemoteCommandType.PP_SELECT, 
                                RecipeID = finalRecipe 
                            };
                            rcmd.TransactionID = ti.TransactionID;
                            await SendCommand(rcmd);
                        } else {
                            await SendReply(new ReplyTrackInReqMessage { 
                                CMD = CommandType.ReplyTrackInReq, 
                                RtnResult = RtnResult.FAIL, 
                                RtnMsg = tir.Split('|').Last(),
                                PanelID = string.Join(",", panels)
                            }, msg);
                        }
                        break;
                    case CommandType.ReplyRemoteCMD:
                        var rrm = (ReplyRemoteCMDMessage)msg;
                        if (rrm.RemoteCMDType == RemoteCommandType.PP_SELECT) {
                            if (_pendingTrackIn.TryRemove(Id, out var ctx)) {
                                if (rrm.RtnResult == RtnResult.PASS) {
                                    CurrentRecipe = ctx.RecipeID;
                                    string fin = await ProcessTrackInFinalAsync(ctx.WorkOrder, ctx.UserID, ctx.CassetteID);
                                    await SendReply(new ReplyTrackInReqMessage { 
                                        CMD = CommandType.ReplyTrackInReq, 
                                        RtnResult = fin.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, 
                                        RtnMsg = fin.Split('|').Last(),
                                        PanelID = string.Join(",", ctx.Panels)
                                    }, rrm);
                                } else {
                                    OnLogReceived(LogLevel.Warn, string.Format("[TrackIn] Machine rejected PP-SELECT: {0}", rrm.RtnMsg));
                                    await SendReply(new ReplyTrackInReqMessage { 
                                        CMD = CommandType.ReplyTrackInReq, 
                                        RtnResult = RtnResult.FAIL, 
                                        RtnMsg = string.Format("Recipe Change Failed: {0}", rrm.RtnMsg),
                                        PanelID = string.Join(",", ctx.Panels)
                                    }, rrm);
                                }
                            } else {
                                OnLogReceived(LogLevel.Warn, string.Format("[TrackIn] Received ReplyRemoteCMD(PP-SELECT) but no pending context found for {0}. Maybe timeout or manual trigger.", Id));
                            }
                        }
                        break;
                    case CommandType.TrackOutReq:
                        var to = (TrackOutReqMessage)msg;
                        List<string> toPanels = new List<string>();
                        string rawPanelId = to.PanelID ?? to.MaterialID;
                        if (!string.IsNullOrEmpty(rawPanelId))
                        {
                            try {
                                if (rawPanelId.Trim().StartsWith("["))
                                {
                                    toPanels = JsonConvert.DeserializeObject<List<string>>(rawPanelId);
                                }
                                else
                                {
                                    toPanels = rawPanelId.Split(',')
                                        .Select(p => p.Trim())
                                        .Where(p => !string.IsNullOrEmpty(p))
                                        .ToList();
                                }
                            } catch { toPanels.Add(rawPanelId); }
                        }
                        List<string> toWos = to.WorkOrder ?? new List<string>();
                        List<string> toCassettes = to.CassetteID ?? new List<string>();

                        string torQty = to.Qty?.ToString() ?? "0";
                        if (!string.IsNullOrEmpty(Id) && Id.StartsWith("TP", StringComparison.OrdinalIgnoreCase))
                        {
                            double tpPassQtyTotal = 0;
                            foreach (var wo in toWos)
                            {
                                var (tpOk, tpMsg, adjustedNg, passQty) = ValidateAndAdjustTpTrackOut(wo);
                                if (!tpOk)
                                {
                                    await SendReply(new ReplyTrackOutReqMessage
                                    {
                                        CMD = CommandType.ReplyTrackOutReq,
                                        RtnResult = RtnResult.FAIL,
                                        RtnMsg = tpMsg
                                    }, msg);
                                    return;
                                }
                                tpPassQtyTotal += passQty;
                            }
                            torQty = tpPassQtyTotal.ToString();
                        }

                        string tor = await ProcessTrackOutAsync(toWos, torQty, to.UserID, toCassettes, toPanels);
                        string[] torParts = tor.Split('|');
                        string rtnMsg = torParts.Length > 2 ? torParts[2] : (torParts.Length > 1 ? torParts[1] : "");
                        await SendReply(new ReplyTrackOutReqMessage { CMD = CommandType.ReplyTrackOutReq, RtnResult = tor.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = rtnMsg }, msg);
                        break;
                    case CommandType.AlarmReport:
                        var alm = (AlarmMessage)msg;
                        string mc = TransformAlarmCode(alm.AlarmCode);
                        bool isAlarmSet = string.IsNullOrEmpty(alm.AlarmStatus) || alm.AlarmStatus == "Start" || alm.AlarmStatus == "1";
                        
                        if (isAlarmSet) {
                            string alarmText = !string.IsNullOrEmpty(alm.AlarmMsg) ? alm.AlarmMsg : $"Alarm {alm.AlarmCode}";
                            RaiseAlarmLocalDb(alm.AlarmCode, alarmText);
                        } else {
                            ClearAlarmLocalDb(alm.AlarmCode);
                        }

                        HandleEqpStatusCall(new EQPSTATUSRequest { 
                            EqpNo = Id, 
                            StatusCode = isAlarmSet ? "1" : "2", 
                            ErrorCode = mc 
                        });
                        await SendReply(new ReplyAlarmMessage { CMD = CommandType.ReplyALARM, RtnResult = RtnResult.PASS }, msg);
                        break;
                    case CommandType.StatusChangeReport:
                        var scm = (StatusChangeReportMessage)msg;
                        int newStatusInt = int.TryParse(scm.NewStatus?.ToString(), out int nval) ? nval : 0;
                        _currentStatusCode = newStatusInt.ToString();
                        if (GetActiveStatusMap().TryGetValue(newStatusInt, out var nst)) State = nst;
                        WriteStatusToLocalDb(_currentStatusCode);
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = _currentStatusCode });
                        await SendReply(new ReplyStatusChangeReportMessage { CMD = CommandType.ReplyStatusChangeReport, RtnResult = RtnResult.PASS }, msg);
                        break;
                    case CommandType.ProcessDataReport:
                        var pdr = (ProcessDataReportMessage)msg;
                        ExtractAndWriteParametersFromMqtt(pdr);
                        await SaveProductionDataAsync("DataReport", pdr.WorkOrder, pdr, saveToDb: false);
                        await SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS }, msg);
                        break;
                    case CommandType.RtnTraceData:
                        var rtd = (RtnTraceDataMessage)msg;
                        ExtractAndWriteParametersFromMqtt(rtd);
                        await SaveProductionDataAsync("TraceData", "", rtd, saveToDb: false);
                        await SendReply(new ReplyRtnTraceDataMessage { CMD = CommandType.ReplyRtnTraceData, RtnResult = RtnResult.PASS, RtnMsg = "Trace data received successfully" }, msg);
                        break;
                    case CommandType.ReplyTimeCalibrate:
                    case CommandType.ReplyTerminalDisplay:
                    case CommandType.ReplySelectParameter:
                    case CommandType.ReplyEditParameter:
                    case CommandType.ReplyInitTraceData:
                    case CommandType.ReplyRtnTraceData:
                        OnLogReceived(LogLevel.Info, string.Format("[RECV] ACK for {0} received.", msg.CMD));
                        break;
                    case CommandType.EventReport:
                        var evm = (EventReportMessage)msg;
                        await HandleEventReport(evm, innerJson);
                        break;
                }
            } catch (Exception ex) { OnLogReceived(LogLevel.Error, string.Format("Parse Report Failed: {0}", ex.Message)); }
            finally { DecrementTask(); }
        }

        private async Task HandleEventReport(EventReportMessage evm, string rawJson)
        {
            string rawEventId = evm.EventID ?? "";
            string logicalEvent = EqpDictionary?.GetEventName(rawEventId) ?? rawEventId;
            string upperEvent = logicalEvent.ToUpper();

            if (upperEvent.StartsWith("UNKNOWN_CEID_"))
            {
                upperEvent = upperEvent.Substring("UNKNOWN_CEID_".Length);
            }

            OnLogReceived(LogLevel.Info, $"[EVENT] {Id} EventID (CEID): {rawEventId} (Mapped: {logicalEvent})");

            string getValByLogicalName(string logicalName) 
            {
                if (evm.Data == null) return null;
                return evm.Data.FirstOrDefault(x => 
                    x.Parameter.Equals(logicalName, StringComparison.OrdinalIgnoreCase) ||
                    (EqpDictionary?.GetVariableName(x.Parameter) ?? x.Parameter).Equals(logicalName, StringComparison.OrdinalIgnoreCase) ||
                    (EqpDictionary?.Variables != null && EqpDictionary.Variables.Any(kvp => kvp.Value.Equals(logicalName, StringComparison.OrdinalIgnoreCase) && kvp.Key.Equals(x.Parameter, StringComparison.OrdinalIgnoreCase)))
                )?.Value;
            }

            switch (upperEvent)
            {
                case "USER_VERIFY":
                case "USERVERIFY":
                case "USERIDVERIFY":
                    string userBarcode = null;
                    string userId = null;
                    string userPassword = null;

                    if (evm.Data != null)
                    {
                        foreach (var item in evm.Data)
                        {
                            if (string.IsNullOrEmpty(item.Parameter)) continue;
                            string varName = EqpDictionary?.GetVariableName(item.Parameter);
                            if (string.IsNullOrEmpty(varName)) continue;

                            if (varName.Equals("User_Barcode", StringComparison.OrdinalIgnoreCase))
                            {
                                userBarcode = item.Value;
                            }
                            else if (varName.Equals("UserID", StringComparison.OrdinalIgnoreCase) || varName.Equals("User_ID", StringComparison.OrdinalIgnoreCase))
                            {
                                userId = item.Value;
                            }
                            else if (varName.Equals("UserPassword", StringComparison.OrdinalIgnoreCase) || varName.Equals("User_Password", StringComparison.OrdinalIgnoreCase))
                            {
                                userPassword = item.Value;
                            }
                        }
                    }

                    // 互斥條件：若有 User_Barcode 則直接放棄 UserID 與 UserPassword
                    if (!string.IsNullOrEmpty(userBarcode))
                    {
                        userId = null;
                        userPassword = null;
                    }

                    string uvRes = await ProcessUserVerifyAsync(userBarcode, userId, userPassword);
                    string[] uvp = uvRes.Split('|');
                    string rtnMsg = uvp.Length > 2 && !string.IsNullOrEmpty(uvp[2]) ? uvp[2] : (uvp.Length > 3 ? uvp[3] : "");
                    await SendReply(new ReplyEventReportMessage { 
                        CMD = CommandType.ReplyEventReport, 
                        RtnResult = uvp[0] == "PASS" ? RtnResult.PASS : RtnResult.FAIL, 
                        RtnMsg = rtnMsg 
                    }, evm);
                    break;

                case "NGCODE_REPORT":
                case "NGCODEREPORT":
                    string ngWo = getValByLogicalName("WorkOrder") ?? getValByLogicalName("WONO") ?? getValByLogicalName("WO") ?? _workOrderNgCodesMap.Keys.FirstOrDefault();
                    string ngCode = getValByLogicalName("NGCode") ?? getValByLogicalName("NG_Code");
                    string ngName = getValByLogicalName("NGName") ?? getValByLogicalName("NG_Name");
                    string ngQty = getValByLogicalName("NGQty") ?? getValByLogicalName("NG_Qty") ?? getValByLogicalName("Qty");
                    string ngUnit = getValByLogicalName("NGUnit") ?? getValByLogicalName("NG_Unit") ?? getValByLogicalName("Unit");
                    string ngUserId = getValByLogicalName("UserID") ?? getValByLogicalName("User_ID") ?? getValByLogicalName("Operator");
                    string ngFlowId = getValByLogicalName("FlowID") ?? getValByLogicalName("Flow_ID") ?? "";

                    RecordNGCodeReport(ngWo, ngCode, ngName, ngQty, ngUnit);

                    var (wongPass, wongCode, wongMsg) = await ProcessEqpWONGAddAsync(ngWo, ngCode, ngQty, ngUnit, ngFlowId, ngUserId);
                    await SendReply(new ReplyEventReportMessage { 
                        CMD = CommandType.ReplyEventReport, 
                        RtnResult = wongPass ? RtnResult.PASS : RtnResult.FAIL,
                        RtnMsg = wongMsg
                    }, evm);
                    break;

                case "LEFTOVERS_REPORT":
                case "LEFTOVERSREPORT":
                    string leftoverWo = getValByLogicalName("WorkOrder") ?? getValByLogicalName("WONO") ?? getValByLogicalName("WO") ?? _workOrderNgCodesMap.Keys.FirstOrDefault();
                    string leftoverQty = getValByLogicalName("Qty") ?? getValByLogicalName("Quantity");
                    string leftoverReelId = getValByLogicalName("ReelID") ?? getValByLogicalName("Reel_ID") ?? getValByLogicalName("ReelId");
                    string leftoverUserId = getValByLogicalName("UserID") ?? getValByLogicalName("User_ID") ?? getValByLogicalName("Operator");

                    var (leftoverPass, leftoverMsg) = await ProcessAddProductLeftoverAsync(leftoverWo, leftoverQty, leftoverReelId, leftoverUserId);
                    await SendReply(new ReplyEventReportMessage
                    {
                        CMD = CommandType.ReplyEventReport,
                        RtnResult = leftoverPass ? RtnResult.PASS : RtnResult.FAIL,
                        RtnMsg = leftoverMsg
                    }, evm);
                    break;

                case "PRODUCT_LEFTOVER_REPORT":
                case "PRODUCTLEFTOVERREPORT":
                    string pLeftoverWo = getValByLogicalName("WorkOrder") ?? getValByLogicalName("WONO") ?? getValByLogicalName("WO") ?? _workOrderNgCodesMap.Keys.FirstOrDefault();
                    string pLeftoverReelId = getValByLogicalName("ReelID") ?? getValByLogicalName("Reel_ID") ?? getValByLogicalName("ReelId");
                    string pLeftoverWoRef = getValByLogicalName("LeftoverWorkOrder") ?? getValByLogicalName("LeftOverWONo") ?? getValByLogicalName("Leftover_WorkOrder");
                    string pLeftoverBatch = getValByLogicalName("LeftoverBatchNo") ?? getValByLogicalName("LeftOverBatchNo") ?? getValByLogicalName("Leftover_BatchNo");
                    string pAddQty = getValByLogicalName("AddQty") ?? getValByLogicalName("Add_Qty");
                    string pGapQty = getValByLogicalName("GapQty") ?? getValByLogicalName("Gap_Qty");
                    string pLeftoverUserId = getValByLogicalName("UserID") ?? getValByLogicalName("User_ID") ?? getValByLogicalName("Operator");

                    var (pLeftoverPass, pLeftoverMsg) = await ProcessUseProductLeftoverAsync(pLeftoverWo, pLeftoverReelId, pLeftoverWoRef, pLeftoverBatch, pAddQty, pGapQty, pLeftoverUserId);
                    await SendReply(new ReplyEventReportMessage
                    {
                        CMD = CommandType.ReplyEventReport,
                        RtnResult = pLeftoverPass ? RtnResult.PASS : RtnResult.FAIL,
                        RtnMsg = pLeftoverMsg
                    }, evm);
                    break;

                case "TOTAL_COUNT_REPORT":
                case "TOTALCOUNTREPORT":
                case "TOTAL_COUNT":
                    string tcWo = getValByLogicalName("WorkOrder") ?? getValByLogicalName("WONO") ?? getValByLogicalName("WO");
                    string tcReelCount = getValByLogicalName("ReelCount") ?? getValByLogicalName("Reel_Count");
                    string tcQtyPerReel = getValByLogicalName("QtyPerReel") ?? getValByLogicalName("Qty_Per_Reel");

                    bool hasWo = !string.IsNullOrWhiteSpace(tcWo);
                    bool hasReelCount = !string.IsNullOrWhiteSpace(tcReelCount);
                    bool hasQtyPerReel = !string.IsNullOrWhiteSpace(tcQtyPerReel);

                    if (hasWo && hasReelCount && hasQtyPerReel)
                    {
                        RecordTotalCountReport(tcWo, tcReelCount, tcQtyPerReel);
                        await SendReply(new ReplyEventReportMessage
                        {
                            CMD = CommandType.ReplyEventReport,
                            RtnResult = RtnResult.PASS,
                            RtnMsg = "TotalCountReport received successfully"
                        }, evm);
                    }
                    else
                    {
                        var missingParams = new List<string>();
                        if (!hasWo) missingParams.Add("WorkOrder");
                        if (!hasReelCount) missingParams.Add("ReelCount");
                        if (!hasQtyPerReel) missingParams.Add("QtyPerReel");

                        string failMsg = $"TotalCountReport validation failed: Missing required parameters ({string.Join(", ", missingParams)}). WorkOrder, ReelCount, QtyPerReel must all have valid values.";
                        OnLogReceived(LogLevel.Warn, $"[TotalCountReport] {Id} {failMsg}");

                        await SendReply(new ReplyEventReportMessage
                        {
                            CMD = CommandType.ReplyEventReport,
                            RtnResult = RtnResult.FAIL,
                            RtnMsg = failMsg
                        }, evm);
                    }
                    break;

                case "KPIV_REPORT":
                    if (evm.Data != null && evm.Data.Count > 0)
                    {
                        var groups = new Dictionary<string, ParameterItem>(StringComparer.OrdinalIgnoreCase);

                        foreach (var item in evm.Data)
                        {
                            if (item == null || string.IsNullOrWhiteSpace(item.Parameter)) continue;

                            string varName = EqpDictionary?.GetVariableName(item.Parameter) ?? item.Parameter;
                            if (string.IsNullOrWhiteSpace(varName)) continue;

                            string baseName = varName.Trim();
                            string type = "VALUE";

                            if (baseName.EndsWith("-U", StringComparison.OrdinalIgnoreCase))
                            {
                                baseName = baseName.Substring(0, baseName.Length - 2).Trim();
                                type = "USL";
                            }
                            else if (baseName.EndsWith("-L", StringComparison.OrdinalIgnoreCase))
                            {
                                baseName = baseName.Substring(0, baseName.Length - 2).Trim();
                                type = "LSL";
                            }

                            if (!groups.TryGetValue(baseName, out var paramItem))
                            {
                                paramItem = new ParameterItem
                                {
                                    P_Name = baseName,
                                    P_Value = "",
                                    P_LSL = "",
                                    P_USL = ""
                                };
                                groups[baseName] = paramItem;
                            }

                            string valStr = item.Value ?? "";
                            if (type == "USL")
                            {
                                paramItem.P_USL = valStr;
                            }
                            else if (type == "LSL")
                            {
                                paramItem.P_LSL = valStr;
                            }
                            else
                            {
                                paramItem.P_Value = valStr;
                            }
                        }

                        var paramList = groups.Values.ToList();
                        var statusReq = new EQPSTATUSRequest
                        {
                            EqpNo = Id,
                            StatusCode = _currentStatusCode ?? "3",
                            ParameterList = JsonConvert.SerializeObject(paramList)
                        };

                        OnLogReceived(LogLevel.Info, $"[KPIV] Reporting {paramList.Count} KPIV parameter(s) via EQPSTATUS for {Id}");
                        HandleEqpStatusCall(statusReq);
                    }
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
                    break;

                default:
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
                    break;
            }
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req) {
            SetLastStatus(req.StatusCode, req.ErrorCode);
            Task.Run(() => { 
                try { 
                    OnLogReceived(LogLevel.Info, $"[MES SEND] EQPSTATUS Request: EqpNo={req.EqpNo}, StatusCode={req.StatusCode}, ErrorCode={req.ErrorCode}");
                    var reply = CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(req); 
                    if (reply != null) {
                        OnLogReceived(LogLevel.Info, $"[MES RECV] EQPSTATUS Reply: Result={reply.Result}, ResultCode={reply.ResultCode}, Message={reply.Message}");
                    } else {
                        OnLogReceived(LogLevel.Warn, $"[MES RECV] EQPSTATUS Reply is null.");
                    }
                } 
                catch (Exception ex) { 
                    OnLogReceived(LogLevel.Error, $"[MES ERROR] EQPSTATUS Failed (Enqueued for retry): {ex.Message}");
                    _statusBuffer.Enqueue(req); 
                } 
            });
        }

        private async Task StartStatusBufferProcessor(CancellationToken ct) {
            while (!ct.IsCancellationRequested) {
                if (!_statusBuffer.IsEmpty && _statusBuffer.TryPeek(out var req)) {
                    try { 
                        OnLogReceived(LogLevel.Info, $"[MES RETRY SEND] EQPSTATUS Request: EqpNo={req.EqpNo}, StatusCode={req.StatusCode}");
                        var reply = await Task.Run(() => CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(req)); 
                        if (reply != null) {
                            OnLogReceived(LogLevel.Info, $"[MES RETRY RECV] EQPSTATUS Reply: Result={reply.Result}, Message={reply.Message}");
                        }
                        _statusBuffer.TryDequeue(out _); 
                    }
                    catch (Exception ex) { 
                        OnLogReceived(LogLevel.Error, $"[MES RETRY ERROR] Retry EQPSTATUS Failed: {ex.Message}");
                        await Task.Delay(5000, ct); 
                    }
                }
                await Task.Delay(1000, ct);
            }
        }

        private void CheckHeartbeat() {
            _ = SendAreYouThere();
            if (_lastResponseTime != DateTime.MinValue && (DateTime.Now - _lastResponseTime).TotalSeconds > _heartbeatInterval + 5) {
                if (Connection != ConnectionStatus.Disconnected) 
                {
                    Connection = ConnectionStatus.Disconnected;
                    OnLogReceived(LogLevel.Warn, $"[MQTT] Connection lost for {Id} (Heartbeat timeout).");
                }
            }
        }

        private async Task SendAreYouThere() => await SendCommand(new AreYouThereMessage { CMD = CommandType.AreYouThere, Machine = Id });
        private async Task SendStatusRequest() => await SendCommand(new StatusReqMessage { CMD = CommandType.StatusReq, Machine = Id });
        public override async Task SendTimeCalibration() => await SendCommand(new TimeCalibrateMessage { CMD = CommandType.TimeCalibrate, Machine = Id });
        public override async Task SendTerminalDisplay(string message)
        {
            string safeMsg = SanitizeTerminalMessage(message);
            if (!string.IsNullOrEmpty(safeMsg))
            {
                await SendCommand(new TerminalDisplayMessage { CMD = CommandType.TerminalDisplay, Machine = Id, Message = safeMsg });
            }
        }

        private async Task SendReply<T>(T replyMsg, BaseMessage requestMsg = null) where T : BaseMessage {
            replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss"); replyMsg.Machine = Id;
            if (requestMsg != null)
            {
                replyMsg.TransactionID = requestMsg.TransactionID;
            }
            await SendCommand(replyMsg);
        }

        private async Task SendCommand(object cmd) {
            if (_bus == null || !_bus.IsConnected) return;
            if (cmd is BaseMessage bm)
            {
                if (string.IsNullOrEmpty(bm.TransactionID))
                {
                    bm.TransactionID = Guid.NewGuid().ToString("N");
                }
            }
            await _bus.PublishAsync(_commandTopic, cmd);
            if (cmd is BaseMessage bm2)
            {
                if (bm2.CMD != CommandType.AreYouThere && bm2.CMD != CommandType.IamHere)
                {
                    OnLogReceived(LogLevel.Info, $"[SEND] {JsonConvert.SerializeObject(bm2, _logSettings)}");
                }
            }
        }

        public override void Stop() 
        { 
            if (_bus != null)
            {
                _bus.Connected -= OnBrokerConnected;
            }
            _heartbeatTimer?.Stop(); 
            _heartbeatTimer?.Dispose(); 
            _linkedCts?.Cancel(); 
        }

        private void ExtractAndWriteParametersFromMqtt(object itemsObj)
        {
            if (itemsObj == null || LocalDataServiceInstance == null) return;
            try
            {
                var now = DateTimeOffset.UtcNow;
                var list = new List<ParameterData>();

                string json = JsonConvert.SerializeObject(itemsObj);
                JToken token = JToken.Parse(json);
                JArray array = null;

                if (token is JObject jObj)
                {
                    if (jObj["ProcessData"] is JArray pdArr) array = pdArr;
                    else if (jObj["TraceData"] is JArray tdArr) array = tdArr;
                    else if (jObj["Data"] is JArray dArr) array = dArr;
                }
                else if (token is JArray jArr)
                {
                    array = jArr;
                }

                if (array != null)
                {
                    foreach (var itemToken in array)
                    {
                        if (itemToken is JObject item)
                        {
                            string code = item["ParamCode"]?.ToString() ?? item["ParamName"]?.ToString() ?? item["Parameter"]?.ToString();
                            if (string.IsNullOrEmpty(code)) continue;

                            string name = item["ParamName"]?.ToString() ?? code;
                            decimal val = decimal.TryParse(item["Value"]?.ToString(), out decimal v) ? v : 0m;
                            decimal? upper = decimal.TryParse(item["UpperLimit"]?.ToString(), out decimal u) ? u : (decimal?)null;
                            decimal? lower = decimal.TryParse(item["LowerLimit"]?.ToString(), out decimal l) ? l : (decimal?)null;
                            bool isAlarm = (upper.HasValue && val > upper.Value) || (lower.HasValue && val < lower.Value);

                            list.Add(new ParameterData(code, name, val, upper, lower, isAlarm, now, 0.1m));
                        }
                    }
                }

                if (list.Count > 0)
                {
                    WriteParametersLocalDb(list);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"ExtractAndWriteParametersFromMqtt Error: {ex.Message}");
            }
        }
    }
}
