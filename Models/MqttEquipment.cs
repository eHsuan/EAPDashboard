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
        }
        private readonly Dictionary<string, TrackInContext> _pendingTrackIn = new Dictionary<string, TrackInContext>();
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        public string Topic { get; set; }

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

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            Stop();
            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _commandTopic = $"{Topic.TrimEnd('/')}/Command";
            _reportTopic = $"{Topic.TrimEnd('/')}/Report";

            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            LoadAlarmMappings();

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

            string innerJson = json;
            try {
                var envelope = JObject.Parse(json);
                if (envelope["Payload"] != null) innerJson = envelope["Payload"].ToString();
            } catch { }

            if (Connection != ConnectionStatus.Connected) Connection = ConnectionStatus.Connected;

            try {
                var obj = JsonConvert.DeserializeObject(innerJson);
                OnLogReceived(LogLevel.Info, $"[RECV] {JsonConvert.SerializeObject(obj, _logSettings)}");

                BaseMessage msg = MessageFactory.Create(innerJson);
                switch (msg.CMD)
                {
                    case CommandType.AreYouThere: await SendReply(new IamHereMessage { CMD = CommandType.IamHere }); break;
                    case CommandType.IamHere: break;
                    case CommandType.ReplyStatusReq:
                        var rs = (ReplyStatusReqMessage)msg;
                        if (int.TryParse(rs.CurrentStatus, out int sc) && StatusMap.TryGetValue(sc, out var st)) State = st;
                        if (!string.IsNullOrEmpty(rs.RecipeID)) CurrentRecipe = rs.RecipeID;
                        OnLogReceived(LogLevel.Info, $"收到狀態查詢回覆: {Id}, 狀態: {rs.CurrentStatus}, Recipe: {rs.RecipeID ?? "N/A"}");
                        break;
                    case CommandType.UserID_Verify:
                        var uv = JsonConvert.DeserializeObject<UserIDVerifyMessage>(innerJson);
                        string res = await ProcessUserVerifyAsync(string.IsNullOrEmpty(uv.Barcode) ? uv.UserID : uv.Barcode);
                        string[] p = res.Split('|');
                        await SendReply(new ReplyUserIDVerifyMessage { CMD = CommandType.ReplyUserID_Verify, UserID = p[1], RtnResult = p[0] == "PASS" ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = p[2] });
                        break;
                    case CommandType.TrackInReq:
                        var ti = JsonConvert.DeserializeObject<TrackInReqMessage>(innerJson);
                        string tir = await ProcessTrackInAsync(ti.CassetteID, ti.WorkOrder);
                        if (tir.StartsWith("PP-SELECT")) {
                            string[] tp = tir.Split('|');
                            _pendingTrackIn[Id] = new TrackInContext { WorkOrder = tp[1], RecipeID = tp[2] };
                            await SendReply(new PPSelectMessage { CMD = CommandType.PP_SELECT, RecipeID = tp[2] });
                        } else {
                            await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = tir.Split('|').Last() });
                        }
                        break;
                    case CommandType.ReplyPP_SELECT:
                        var rps = JsonConvert.DeserializeObject<ReplyPPSelectMessage>(innerJson);
                        if (_pendingTrackIn.TryGetValue(Id, out var ctx)) {
                            _pendingTrackIn.Remove(Id);
                            if (rps.RtnResult == RtnResult.PASS) {
                                CurrentRecipe = ctx.RecipeID;
                                string fin = await ProcessTrackInFinalAsync(ctx.WorkOrder);
                                await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = fin.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL, RtnMsg = fin.Split('|').Last() });
                            } else {
                                await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "機台 Recipe 切換失敗" });
                            }
                        }
                        break;
                    case CommandType.TrackOutReq:
                        var to = JsonConvert.DeserializeObject<TrackOutReqMessage>(innerJson);
                        string tor = await ProcessTrackOutAsync(to.WorkOrder, to.Qty.ToString());
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
                        var pdr = JsonConvert.DeserializeObject<ProcessDataReportMessage>(innerJson);
                        await SaveProductionDataAsync("DataReport", pdr.WorkOrder, pdr);
                        await SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS });
                        break;
                    case CommandType.BatchProcessDataReport:
                        var bpdr = JsonConvert.DeserializeObject<BatchProcessDataReportMessage>(innerJson);
                        await SaveProductionDataAsync("BatchDataReport", "", bpdr);
                        await SendReply(new ReplyBatchProcessDataReportMessage { CMD = CommandType.ReplyBatchProcessDataReport, RtnResult = RtnResult.PASS });
                        break;
                    case CommandType.PanelInReport:
                        var pIn = JsonConvert.DeserializeObject<PanelInReportMessage>(innerJson);
                        await SaveProductionDataAsync("PanelIn", "", pIn);
                        await SendReply(new ReplyPanelInReportMessage { CMD = CommandType.ReplyPanelInReport, RtnResult = RtnResult.PASS });
                        break;
                    case CommandType.PanelOutReport:
                        var pOut = JsonConvert.DeserializeObject<PanelOutReportMessage>(innerJson);
                        await SaveProductionDataAsync("PanelOut", "", pOut);
                        await SendReply(new ReplyPanelOutReportMessage { CMD = CommandType.ReplyPanelOutReport, RtnResult = RtnResult.PASS });
                        break;
                }
            } catch (Exception ex) { OnLogReceived(LogLevel.Error, $"解析 Report 失敗: {ex.Message}"); }
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req) {
            Task.Run(() => { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); } 
            catch { _statusBuffer.Enqueue(req); } });
        }

        private async Task StartStatusBufferProcessor(CancellationToken ct) {
            while (!ct.IsCancellationRequested) {
                if (!_statusBuffer.IsEmpty && _statusBuffer.TryPeek(out var req)) {
                    try { await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req)); _statusBuffer.TryDequeue(out _); }
                    catch { await Task.Delay(30000, ct); }
                }
                await Task.Delay(5000, ct);
            }
        }

        private void CheckHeartbeat() {
            _ = SendAreYouThere();
            if (_lastResponseTime != DateTime.MinValue && (DateTime.Now - _lastResponseTime).TotalSeconds > _heartbeatInterval + 5) {
                if (Connection != ConnectionStatus.Disconnected) Connection = ConnectionStatus.Disconnected;
            }
        }

        private async Task SendAreYouThere() => await SendCommand(new AreYouThereMessage { CMD = CommandType.AreYouThere, Machine = Id });
        private async Task SendStatusRequest() => await SendCommand(new StatusReqMessage { CMD = CommandType.StatusReq, Machine = Id });
        public override async Task SendTimeCalibration() => await SendCommand(new TimeCalibrateMessage { CMD = CommandType.TimeCalibrate, Machine = Id });

        private async Task SendReply<T>(T replyMsg) where T : BaseMessage {
            replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss"); replyMsg.Machine = Id;
            await SendCommand(replyMsg);
            OnLogReceived(LogLevel.Info, $"[SEND] {JsonConvert.SerializeObject(replyMsg, _logSettings)}");
        }

        private async Task SendCommand(object cmd) {
            if (_bus == null || !_bus.IsConnected) return;
            await _bus.PublishAsync(_commandTopic, JsonConvert.SerializeObject(cmd, _logSettings));
        }

        public override void Stop() { _heartbeatTimer?.Stop(); _heartbeatTimer?.Dispose(); _linkedCts?.Cancel(); }
    }
}
