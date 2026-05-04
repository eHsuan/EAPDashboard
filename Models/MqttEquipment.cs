using System;
using System.Collections.Generic;
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
            public string WorkOrder { get; set; }
            public string RecipeID { get; set; }
            public string UserID { get; set; }
            public string CassetteID { get; set; }
        }
        private readonly Dictionary<string, TrackInContext> _pendingTrackIn = new Dictionary<string, TrackInContext>();
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        public string Topic { get; set; }
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

        private static readonly Dictionary<int, EquipmentState> StatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Running }, { 6, EquipmentState.Running },
           { 1, EquipmentState.Alarm }, { 10, EquipmentState.Alarm }, { 11, EquipmentState.Alarm },
           { 2, EquipmentState.Paused }, { 3, EquipmentState.Paused }, { 4, EquipmentState.Paused },
           { 5, EquipmentState.Paused }, { 7, EquipmentState.Paused }, { 8, EquipmentState.Paused },
           { 9, EquipmentState.Paused }, { 12, EquipmentState.Paused }
        };

        private void LoadDictionary()
        {
            string cleanId = Id?.Trim();
            if (string.IsNullOrEmpty(cleanId))
            {
                OnLogReceived(LogLevel.Warn, "[MQTT] Cannot load dictionary: EqpId is NULL.");
                EqpDictionary = new SecsDictionary();
                return;
            }

            string dictPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dictionaries", $"{cleanId}.json");
            if (System.IO.File.Exists(dictPath))
            {
                EqpDictionary = SecsDictionary.Load(dictPath);
                OnLogReceived(LogLevel.Info, $"[MQTT] Dictionary loaded for {cleanId} from {dictPath}");
            }
            else
            {
                EqpDictionary = new SecsDictionary();
                OnLogReceived(LogLevel.Warn, $"[MQTT] Dictionary NOT FOUND for {cleanId}. Expected: {dictPath}");
            }
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            LoadAlarmMappings();
            LoadDictionary();

            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _commandTopic = $"{Topic?.TrimEnd('/')}/Command";
            _reportTopic = $"{Topic?.TrimEnd('/')}/Report";

            OnLogReceived(LogLevel.Info, $"[START] MQTT module started for {Id}.");

            _bus.SubscribeAsync(_reportTopic, (topic, payload) => HandleReport(payload));

            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval * 1000);
            _heartbeatTimer.Elapsed += (s, e) => CheckHeartbeat();
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;

            _ = StartStatusBufferProcessor(_linkedCts.Token);
            _ = SendAreYouThere();
            _ = SendStatusRequest();
        }

        private async void HandleReport(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            _lastResponseTime = DateTime.Now;

            try {
                var envelope = JsonConvert.DeserializeObject<CyntecMqttLib.Contracts.MessageEnvelope>(json);
                if (envelope == null) return;

                string messageId = envelope.MessageId;
                string innerJson = envelope.Payload?.ToString();
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
                    OnLogReceived(LogLevel.Info, $"[MQTT] Connection restored for {Id}.");
                }

                BaseMessage msg = MessageFactory.Create(innerJson);
                if (msg.CMD != CommandType.AreYouThere && msg.CMD != CommandType.IamHere)
                {
                    OnLogReceived(LogLevel.Info, $"[RECV] {JsonConvert.SerializeObject(msg, _logSettings)}");
                }

                switch (msg.CMD)
                {
                    case CommandType.AreYouThere: await SendReply(new IamHereMessage { CMD = CommandType.IamHere }); break;
                    case CommandType.IamHere: break;
                    case CommandType.ReplyStatusReq:
                        var rs = (ReplyStatusReqMessage)msg;
                        if (StatusMap.TryGetValue(rs.CurrentStatus, out var st)) State = st;
                        OnLogReceived(LogLevel.Info, $"Status Reply Recv: {Id}, Status: {rs.CurrentStatus}");
                        break;
                    case CommandType.TrackInReq:
                        var ti = (TrackInReqMessage)msg;
                        // 1. 取得工單
                        string wo = QueryWorkOrderFromDb(ti.CassetteID) ?? ti.WorkOrder;
                        // 2. 向 MES 請求
                        string tir = await ProcessTrackInAsync(ti.CassetteID, wo, ti.UserID);
                        if (tir.StartsWith("PP-SELECT")) {
                            string[] tp = tir.Split('|');
                            _pendingTrackIn[Id] = new TrackInContext { WorkOrder = tp[1], RecipeID = tp[2], UserID = ti.UserID, CassetteID = ti.CassetteID };
                            
                            // 3. 判定配方提取 Key (根據子機配置)
                            string finalRecipe = "";
                            if (this.IsMesSimulator) {
                                finalRecipe = new Random().Next(1000, 9999).ToString();
                                OnLogReceived(LogLevel.Info, $"[SIM] MES Simulator detected. Random Recipe: {finalRecipe}");
                            } else {
                                // 嘗試動態尋找該子機的配置 (此處假設 MQTT 設備也是子機架構的一部分)
                                var evtConfig = DatabaseHelper.GetPlcConfigs(this.Id).FirstOrDefault(x => x.CmdType == CommandType.TrackInReq);
                                finalRecipe = GetMesParamValue(tp[5], evtConfig?.RecipeParamKey) ?? tp[2];
                            }

                            await SendCommand(new RemoteCMDMessage { 
                                CMD = CommandType.RemoteCMD, 
                                RemoteCMDType = RemoteCommandType.PP_SELECT, 
                                RecipeID = finalRecipe 
                            });
                        } else {
                            await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = tir.Split('|').Last() });
                        }
                        break;
                    case CommandType.ReplyRemoteCMD:
                        var rrm = (ReplyRemoteCMDMessage)msg;
                        if (rrm.RemoteCMDType == RemoteCommandType.PP_SELECT) {
                            if (_pendingTrackIn.TryGetValue(Id, out var ctx)) {
                                _pendingTrackIn.Remove(Id);
                                if (rrm.RtnResult == RtnResult.PASS) {
                                    CurrentRecipe = ctx.RecipeID;
                                    string fin = await ProcessTrackInFinalAsync(ctx.WorkOrder, ctx.UserID, ctx.CassetteID);
                                    await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = fin.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = fin.Split('|').Last() });
                                } else {
                                    await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = $"Recipe Change Failed: {rrm.RtnMsg}" });
                                }
                            }
                        }
                        break;
                    case CommandType.TrackOutReq:
                        var to = (TrackOutReqMessage)msg;
                        string tor = await ProcessTrackOutAsync(to.WorkOrder, to.Qty.ToString(), to.UserID, to.CassetteID, new List<string>());
                        await SendReply(new ReplyTrackOutReqMessage { CMD = CommandType.ReplyTrackOutReq, RtnResult = tor.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = tor.Split('|').Last() });
                        break;
                    case CommandType.ALARM:
                        var alm = (AlarmMessage)msg;
                        string mc = TransformAlarmCode(alm.AlarmCode);
                        State = EquipmentState.Alarm;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = mc });
                        break;
                    case CommandType.StatusChangeReport:
                        var scm = (StatusChangeReportMessage)msg;
                        if (StatusMap.TryGetValue(scm.NewStatus, out var nst)) State = nst;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = scm.NewStatus.ToString() });
                        await SendReply(new ReplyStatusChangeReportMessage { CMD = CommandType.ReplyStatusChangeReport, RtnResult = RtnResult.PASS });
                        break;
                    case CommandType.ProcessDataReport:
                        var pdr = (ProcessDataReportMessage)msg;
                        await SaveProductionDataAsync("DataReport", pdr.WorkOrder, pdr);
                        await SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS });
                        break;
                    case CommandType.EventReport:
                        var evm = (EventReportMessage)msg;
                        await HandleEventReport(evm, innerJson);
                        break;
                }
            } catch (Exception ex) { OnLogReceived(LogLevel.Error, $"Parse Report Failed: {ex.Message}"); }
        }

        private async Task HandleEventReport(EventReportMessage evm, string rawJson)
        {
            string eventId = evm.EventID;
            string logicalEvent = EqpDictionary?.GetEventName(eventId) ?? eventId;
            logicalEvent = logicalEvent.ToUpper();

            OnLogReceived(LogLevel.Info, $"[EVENT] {Id} EventID: {eventId} (Mapped: {logicalEvent})");

            string getValByLogicalName(string logicalName) 
            {
                if (evm.Data == null) return null;
                return evm.Data.FirstOrDefault(x => (EqpDictionary?.GetVariableName(x.Parameter) ?? x.Parameter).Equals(logicalName, StringComparison.OrdinalIgnoreCase))?.Value;
            }

            switch (logicalEvent)
            {
                case "USER_VERIFY":
                    string uid = getValByLogicalName("UserID");
                    string bcd = getValByLogicalName("Barcode") ?? uid;
                    string uvRes = await ProcessUserVerifyAsync(bcd, uid);
                    string[] uvp = uvRes.Split('|');
                    await SendReply(new ReplyEventReportMessage { 
                        CMD = CommandType.ReplyEventReport, 
                        RtnResult = uvp[0] == "PASS" ? RtnResult.PASS : RtnResult.FAIL, 
                        RtnMsg = uvp[2] 
                    });
                    break;

                case "PANEL_IN":
                    await SaveProductionDataAsync("PanelIn", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS });
                    break;

                case "PANEL_OUT":
                    await SaveProductionDataAsync("PanelOut", "", evm);
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS });
                    break;

                default:
                    await SendReply(new ReplyEventReportMessage { CMD = CommandType.ReplyEventReport, RtnResult = RtnResult.PASS });
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

        private async Task SendReply<T>(T replyMsg) where T : BaseMessage {
            replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss"); replyMsg.Machine = Id;
            await SendCommand(replyMsg);
            if (replyMsg.CMD != CommandType.IamHere)
            {
                OnLogReceived(LogLevel.Info, $"[SEND] {JsonConvert.SerializeObject(replyMsg, _logSettings)}");
            }
        }

        private async Task SendCommand(object cmd) {
            if (_bus == null || !_bus.IsConnected) return;
            await _bus.PublishAsync(_commandTopic, cmd);
        }

        public override void Stop() { _heartbeatTimer?.Stop(); _heartbeatTimer?.Dispose(); _linkedCts?.Cancel(); }
    }
}
