using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Models;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Utils;
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
        private string _currentOperator = "UNKNOWN";

        private string _commandTopic;
        private string _reportTopic;
        private MqttBus _bus;

        // 資料庫設定
        public DbSettings DbConfig { get; set; }

        // 用於追蹤正在處理中的進站上下文
        private class TrackInContext
        {
            public TrackInReqMessage OriginalRequest { get; set; }
            public string RecipeID { get; set; }
        }
        private readonly Dictionary<string, TrackInContext> _pendingTrackIn = new Dictionary<string, TrackInContext>();
        
        // 用於存放暫時發送失敗的狀態更新請求 (斷線緩衝)
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        public string Topic { get; set; }

        public MqttEquipment()
        {
            Protocol = "MQTT";
            Connection = ConnectionStatus.Disconnected;
        }

        private static readonly Newtonsoft.Json.JsonSerializerSettings _logSettings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        private static readonly Dictionary<int, EquipmentState> StatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Running },
           { 6, EquipmentState.Running },
           { 1, EquipmentState.Alarm },
           { 10, EquipmentState.Alarm },
           { 11, EquipmentState.Alarm },
           { 2, EquipmentState.Paused },
           { 3, EquipmentState.Paused },
           { 4, EquipmentState.Paused },
           { 5, EquipmentState.Paused },
           { 7, EquipmentState.Paused },
           { 8, EquipmentState.Paused },
           { 9, EquipmentState.Paused },
           { 12, EquipmentState.Paused }
       };

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            Stop();

            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _commandTopic = $"{Topic.TrimEnd('/')}/Command";
            _reportTopic = $"{Topic.TrimEnd('/')}/Report";

            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            CancellationToken token = _linkedCts.Token;

            OnLogReceived(LogLevel.Info, $"[START] MQTT module started for {Id} on {Topic}. Heartbeat: {heartbeatIntervalSeconds}s");

            // Subscribe to reports
            _bus.SubscribeAsync(_reportTopic, (topic, payload) => 
            {
                if (token.IsCancellationRequested) return;
                HandleReport(payload);
            });

            // Start internal heartbeat check
            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval * 1000);
            _heartbeatTimer.Elapsed += (s, e) => CheckHeartbeat();
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;

            // Start status buffer processor
            StartStatusBufferProcessor(token);

            // Initial requests
            _ = SendAreYouThere();
            _ = SendStatusRequest();
        }

        private void StartStatusBufferProcessor(CancellationToken ct)
        {
            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (!_statusBuffer.IsEmpty)
                    {
                        if (_statusBuffer.TryPeek(out var req))
                        {
                            try
                            {
                                MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req);
                                _statusBuffer.TryDequeue(out _);
                                OnLogReceived(LogLevel.Info, $"[補傳成功] 設備狀態/警報已同步至 MES (佇列剩餘: {_statusBuffer.Count})");
                            }
                            catch { /* Wait for next cycle */ }
                        }
                    }
                    await Task.Delay(30000, ct);
                }
            }, ct);
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req)
        {
            Task.Run(() =>
            {
                try
                {
                    MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req);
                }
                catch (Exception ex)
                {
                    OnLogReceived(LogLevel.Warn, $"[MES 斷線] 狀態同步失敗，已進入緩衝區。原因: {ex.Message}");
                    _statusBuffer.Enqueue(req);
                }
            });
        }

        private void CheckHeartbeat()
        {
            _ = SendAreYouThere();

            if (_lastResponseTime != DateTime.MinValue && (DateTime.Now - _lastResponseTime).TotalSeconds > _heartbeatInterval + 5)
            {
                if (Connection != ConnectionStatus.Disconnected)
                {
                    OnLogReceived(LogLevel.Warn, $"通訊逾時: 超過 {_heartbeatInterval} 秒未收到回應。");
                    Connection = ConnectionStatus.Disconnected;
                }
            }
        }

        private async void HandleReport(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            _lastResponseTime = DateTime.Now;

            string innerJson = json;
            try
            {
                var envelope = JObject.Parse(json);
                if (envelope != null && envelope["Payload"] != null)
                {
                    innerJson = envelope["Payload"].ToString();
                }
            }
            catch { }

            if (Connection != ConnectionStatus.Connected)
            {
                Connection = ConnectionStatus.Connected;
                OnLogReceived(LogLevel.Info, "設備已連線 (收到訊息回應)");
            }

            try
            {
                var obj = JsonConvert.DeserializeObject(innerJson);
                string displayMessage = JsonConvert.SerializeObject(obj, _logSettings);
                OnLogReceived(LogLevel.Info, $"[RECV] {displayMessage}");

                BaseMessage msg = MessageFactory.Create(innerJson);

                switch (msg.CMD)
                {
                    case CommandType.AreYouThere:
                        await SendReply(new IamHereMessage { CMD = CommandType.IamHere });
                        break;

                    case CommandType.IamHere:
                        // 收到機台對 EAP 心跳的回覆，不需額外處理
                        break;

                    case CommandType.ReplyStatusReq:
                        var replyStatus = (ReplyStatusReqMessage)msg;
                        if (int.TryParse(replyStatus.CurrentStatus, out int statusCode))
                        {
                            if (StatusMap.TryGetValue(statusCode, out var syncState)) State = syncState;
                        }
                        if (!string.IsNullOrEmpty(replyStatus.RecipeID)) CurrentRecipe = replyStatus.RecipeID;
                        OnLogReceived(LogLevel.Info, $"收到狀態查詢回覆: {Id}, 狀態: {replyStatus.CurrentStatus}, Recipe: {replyStatus.RecipeID ?? "N/A"}");
                        break;

                    case CommandType.ReplyTimeCalibrate:
                        var replyTime = (ReplyTimeCalibrateMessage)msg;
                        OnLogReceived(LogLevel.Info, $"收到機台時間校正回覆: {Id}, 結果: {replyTime.RtnResult}");
                        break;

                    case CommandType.UserID_Verify:
                        await HandleUserIDVerify(innerJson);
                        break;

                    case CommandType.TrackInReq:
                        await HandleTrackIn(innerJson);
                        break;

                    case CommandType.ReplyPP_SELECT:
                        await HandleReplyPPSelect(innerJson);
                        break;

                    case CommandType.TrackOutReq:
                        await HandleTrackOut(innerJson);
                        break;

                    case CommandType.ALARM:
                        var alarmMsg = (AlarmMessage)msg;
                        State = EquipmentState.Alarm;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = alarmMsg.AlarmCode });
                        break;

                    case CommandType.StatusChangeReport:
                        var statusMsg = (StatusChangeReportMessage)msg;
                        if (StatusMap.TryGetValue(statusMsg.NewStatus, out var reportState)) State = reportState;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = statusMsg.NewStatus.ToString() });
                        await SendReply(new ReplyStatusChangeReportMessage { CMD = CommandType.ReplyStatusChangeReport, RtnResult = RtnResult.PASS });
                        break;

                    case CommandType.ProcessDataReport:
                        await SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS });
                        break;

                    case CommandType.BatchProcessDataReport:
                        await SendReply(new ReplyBatchProcessDataReportMessage { CMD = CommandType.ReplyBatchProcessDataReport, RtnResult = RtnResult.PASS });
                        break;

                    default:
                        OnLogReceived(LogLevel.Debug, $"未處理訊息: {msg.CMD}");
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[ERROR] MQTT Report: {ex.Message}");
            }
        }

        private async Task HandleUserIDVerify(string json)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<UserIDVerifyMessage>(json);
                string barcode = string.IsNullOrEmpty(msg.Barcode) ? msg.UserID : msg.Barcode;
                OnLogReceived(LogLevel.Info, $"執行人員驗證: {barcode}");

                var mesReq = new UserVerifyRequest { EqpNo = Id, User2DBarCode = barcode };
                var mesReply = await Task.Run(() => MESMessageFactory.CallMes<UserVerifyRequest, UserVerifyReply>(mesReq));

                var reply = new ReplyUserIDVerifyMessage
                {
                    CMD = CommandType.ReplyUserID_Verify,
                    UserID = mesReply.UserID ?? "",
                    Barcode = barcode,
                    RtnResult = mesReply.Result == "success" ? RtnResult.PASS : RtnResult.FAIL,
                    RtnMsg = mesReply.Result == "success" ? $"歡迎登入, {mesReply.UserName}" : mesReply.Message
                };

                if (reply.RtnResult == RtnResult.PASS) _currentOperator = reply.UserID;
                await SendReply(reply);
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"人員驗證失敗: {ex.Message}"); }
        }

        private async Task HandleTrackIn(string json)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<TrackInReqMessage>(json);
                string wo = msg.WorkOrder;

                if (string.IsNullOrEmpty(wo))
                {
                    if (!string.IsNullOrEmpty(msg.CassetteID))
                    {
                        OnLogReceived(LogLevel.Info, $"查詢載具 {msg.CassetteID} 綁定工單...");
                        wo = QueryWorkOrderFromDb(msg.CassetteID);
                        if (string.IsNullOrEmpty(wo))
                        {
                            await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "載具未綁定工單" });
                            return;
                        }
                    }
                    else
                    {
                        await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "遺漏工單或載具資訊" });
                        return;
                    }
                }

                var qryReq = new WOQRYRequest { WONO = wo, EqpNo = Id, UserID = string.IsNullOrEmpty(msg.UserID) ? _currentOperator : msg.UserID };
                var qryReply = await Task.Run(() => MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq));

                if (qryReply.Result != "success")
                {
                    await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = qryReply.Message });
                    return;
                }

                string recipe = ExtractRecipeID(qryReply.InputParamListDT);
                lock (_pendingTrackIn)
                {
                    msg.WorkOrder = wo;
                    _pendingTrackIn[Id] = new TrackInContext { OriginalRequest = msg, RecipeID = recipe };
                }

                await SendReply(new PPSelectMessage { CMD = CommandType.PP_SELECT, RecipeID = recipe });
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"進站啟動失敗: {ex.Message}"); }
        }

        private async Task HandleReplyPPSelect(string json)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<ReplyPPSelectMessage>(json);
                TrackInContext context = null;
                lock (_pendingTrackIn) { if (_pendingTrackIn.TryGetValue(Id, out context)) _pendingTrackIn.Remove(Id); }

                if (msg.RtnResult == RtnResult.PASS && context != null)
                {
                    CurrentRecipe = context.RecipeID;
                    var checkInReq = new WOCHECKINRequest { WONO = context.OriginalRequest.WorkOrder, EqpNo = Id, UserID = _currentOperator };
                    var checkInReply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(checkInReq));

                    await SendReply(new ReplyTrackInReqMessage
                    {
                        CMD = CommandType.ReplyTrackInReq,
                        RtnResult = (checkInReply.Result == "success" || checkInReply.Result == "PASS") ? RtnResult.PASS : RtnResult.FAIL,
                        RtnMsg = checkInReply.Message
                    });
                }
                else if (context != null)
                {
                    await SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "機台 Recipe 切換失敗" });
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"進站報工失敗: {ex.Message}"); }
        }

        private async Task HandleTrackOut(string json)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<TrackOutReqMessage>(json);
                var mesReq = new WOCHECKOUTRequest { WONO = msg.WorkOrder, EqpNo = Id, UserID = string.IsNullOrEmpty(msg.UserID) ? _currentOperator : msg.UserID, OutPut = msg.Qty.ToString() };
                var mesReply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(mesReq));

                await SendReply(new ReplyTrackOutReqMessage
                {
                    CMD = CommandType.ReplyTrackOutReq,
                    RtnResult = (mesReply.Result == "success" || mesReply.Result == "PASS") ? RtnResult.PASS : RtnResult.FAIL,
                    RtnMsg = mesReply.Message
                });
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"出站報工失敗: {ex.Message}"); }
        }

        private string QueryWorkOrderFromDb(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return null;
            try
            {
                using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string sql = $"SELECT TOP 1 {DbConfig.WoColumn} FROM {DbConfig.BindingTable} WHERE {DbConfig.CassetteColumn} = @cid";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        return cmd.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"資料庫查詢失敗: {ex.Message}"); return null; }
        }

        private string ExtractRecipeID(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try
            {
                var list = JsonConvert.DeserializeObject<List<JObject>>(inputParamListDt);
                var recipeObj = list.FirstOrDefault(x => x["ParamName"]?.ToString() == "RECIPEID" || x["RefFieldCode"]?.ToString() == "RECIPE ID");
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            }
            catch { return "ERROR"; }
        }

        private async Task SendAreYouThere()
        {
            var msg = new AreYouThereMessage { CMD = CommandType.AreYouThere, Date = DateTime.Now.ToString("yyyyMMddHHmmss"), Machine = Id };
            await SendCommand(msg);
        }

        private async Task SendStatusRequest()
        {
            var msg = new StatusReqMessage { CMD = CommandType.StatusReq, Date = DateTime.Now.ToString("yyyyMMddHHmmss"), Machine = Id };
            await SendCommand(msg);
            OnLogReceived(LogLevel.Info, $"發送狀態查詢: {Id}");
        }

        public override async Task SendTimeCalibration()
        {
            var msg = new TimeCalibrateMessage { CMD = CommandType.TimeCalibrate, Date = DateTime.Now.ToString("yyyyMMddHHmmss"), Machine = Id };
            await SendCommand(msg);
        }

        private async Task SendReply<T>(T replyMsg) where T : BaseMessage
        {
            replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            replyMsg.Machine = Id;
            await SendCommand(replyMsg);
            OnLogReceived(LogLevel.Info, $"[SEND] {JsonConvert.SerializeObject(replyMsg, _logSettings)}");
        }

        private async Task SendCommand(object cmd)
        {
            if (_bus == null || !_bus.IsConnected) return;
            string json = JsonConvert.SerializeObject(cmd, _logSettings);
            await _bus.PublishAsync(_commandTopic, json);
        }

        public override void Stop()
        {
            _heartbeatTimer?.Stop();
            _heartbeatTimer?.Dispose();
            _heartbeatTimer = null;

            _linkedCts?.Cancel();
            _linkedCts?.Dispose();
            _linkedCts = null;
        }
    }
}
