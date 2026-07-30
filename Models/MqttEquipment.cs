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
           { 2, EquipmentState.Idle },
           { 3, EquipmentState.Alarm }
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

            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _commandTopic = $"{Topic?.TrimEnd('/')}/Command";
            _reportTopic = $"{Topic?.TrimEnd('/')}/Report";

            OnLogReceived(LogLevel.Info, $"[START] MQTT module started for {Id}. Base Topic: {Topic}, Command Topic: {_commandTopic}, Report Topic: {_reportTopic}");

            _bus.SubscribeAsync(_reportTopic, (topic, payload) => HandleReport(payload));

            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval * 1000);
            _heartbeatTimer.Elapsed += (s, e) => CheckHeartbeat();
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;

            _ = StartStatusBufferProcessor(_linkedCts.Token);
            _ = MonitorTrackInTimeout(_linkedCts.Token);
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
                        
                        var info = QueryBindingFromGateway(tiCassettes.FirstOrDefault());
                        if (info != null && !string.IsNullOrEmpty(info.WorkOrder))
                        {
                            tiWos = new List<string> { info.WorkOrder };
                        }
                        List<string> panels = info?.Panels ?? new List<string>();
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
                            string finalRecipe = "";
                            if (this.IsMesSimulator) {
                                finalRecipe = new Random().Next(1000, 9999).ToString();
                                OnLogReceived(LogLevel.Info, string.Format("[SIM] MES Simulator detected. Random Recipe: {0}", finalRecipe));
                            } else {
                                finalRecipe = tp[2];
                            }

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
                        string tor = await ProcessTrackOutAsync(toWos, to.Qty.ToString(), to.UserID, toCassettes, toPanels);
                        await SendReply(new ReplyTrackOutReqMessage { CMD = CommandType.ReplyTrackOutReq, RtnResult = tor.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = tor.Split('|').Last() }, msg);
                        break;
                    case CommandType.AlarmReport:
                        var alm = (AlarmMessage)msg;
                        string mc = TransformAlarmCode(alm.AlarmCode);
                        bool isAlarmSet = string.IsNullOrEmpty(alm.AlarmStatus) || alm.AlarmStatus == "Start" || alm.AlarmStatus == "1";
                        
                        if (isAlarmSet) {
                            State = EquipmentState.Alarm;
                        } else {
                            if (State == EquipmentState.Alarm) {
                                State = EquipmentState.Idle;
                            }
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
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = _currentStatusCode });
                        await SendReply(new ReplyStatusChangeReportMessage { CMD = CommandType.ReplyStatusChangeReport, RtnResult = RtnResult.PASS }, msg);
                        break;
                    case CommandType.ProcessDataReport:
                        var pdr = (ProcessDataReportMessage)msg;
                        await SaveProductionDataAsync("DataReport", pdr.WorkOrder, pdr, saveToDb: false);
                        await SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS }, msg);
                        break;
                    case CommandType.RtnTraceData:
                        var rtd = (RtnTraceDataMessage)msg;
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
            string rawEventId = evm.EventID;
            string logicalEvent = EqpDictionary?.GetEventName(rawEventId) ?? rawEventId;
            string upperEvent = logicalEvent.ToUpper();

            OnLogReceived(LogLevel.Info, $"[EVENT] {Id} EventID (CEID): {rawEventId} (Mapped: {logicalEvent})");

            string getValByLogicalName(string logicalName) 
            {
                if (evm.Data == null) return null;
                return evm.Data.FirstOrDefault(x => 
                    x.Parameter.Equals(logicalName, StringComparison.OrdinalIgnoreCase) ||
                    (EqpDictionary?.GetVariableName(x.Parameter) ?? x.Parameter).Equals(logicalName, StringComparison.OrdinalIgnoreCase)
                )?.Value;
            }

            switch (upperEvent)
            {
                case "USER_VERIFY":
                case "500":
                case "100":
                    string uid = getValByLogicalName("UserID");
                    string bcd = getValByLogicalName("Barcode") ?? uid;
                    string uvRes = await ProcessUserVerifyAsync(bcd, uid);
                    string[] uvp = uvRes.Split('|');
                    await SendReply(new ReplyEventReportMessage { 
                        CMD = CommandType.ReplyEventReport, 
                        RtnResult = uvp[0] == "PASS" ? RtnResult.PASS : RtnResult.FAIL, 
                        RtnMsg = uvp[2] 
                    }, evm);
                    break;

                case "BATCH_PROCESS_DATA":
                case "101":
                    await SaveProductionDataAsync("BatchData", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
                    break;

                case "KEY_PARAMETERS":
                case "102":
                    await SaveProductionDataAsync("KeyParams", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
                    break;

                case "PANEL_IN":
                case "103":
                    await SaveProductionDataAsync("PanelIn", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
                    break;

                case "PANEL_OUT":
                case "104":
                    await SaveProductionDataAsync("PanelOut", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS }, evm);
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
            Task.Run(() => { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); } 
            catch { _statusBuffer.Enqueue(req); } });
        }

        private async Task StartStatusBufferProcessor(CancellationToken ct) {
            while (!ct.IsCancellationRequested) {
                if (!_statusBuffer.IsEmpty && _statusBuffer.TryPeek(out var req)) {
                    try { await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req)); _statusBuffer.TryDequeue(out _); }
                    catch { await Task.Delay(5000, ct); }
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
        public override async Task SendTerminalDisplay(string message) => await SendCommand(new TerminalDisplayMessage { CMD = CommandType.TerminalDisplay, Machine = Id, Message = message });

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
                if (bm2.CMD != CommandType.AreYouThere && bm2.CMD != CommandType.IamHere && bm2.CMD != CommandType.StatusReq)
                {
                    OnLogReceived(LogLevel.Info, $"[SEND] {JsonConvert.SerializeObject(bm2, _logSettings)}");
                }
            }
        }

        public override void Stop() { _heartbeatTimer?.Stop(); _heartbeatTimer?.Dispose(); _linkedCts?.Cancel(); }
    }
}
