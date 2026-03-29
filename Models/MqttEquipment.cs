using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Models;
using CyntecMqttLib.Services;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Utils;


namespace EAPDashboard.Models
{
    public class MqttEquipment : EquipmentBase
    {
        private readonly Random _random = new Random();
        private CancellationTokenSource _linkedCts; 
        private System.Timers.Timer _heartbeatTimer;
        private DateTime _lastResponseTime = DateTime.MinValue;
        private int _heartbeatInterval = 30;
        private string _currentOperator = "UNKNOWN"; // 用於快取登入成功的人員 ID

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
        public MqttBus _bus;
        public MqttEquipment()
        {
            Protocol = "MQTT";
            Connection = ConnectionStatus.Disconnected;
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            // 確保先停止舊任務，防止重複啟動
            Stop();

            _bus = bus;
            _heartbeatInterval = heartbeatIntervalSeconds;
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            
            Task.Run(async () =>
            {
                if (_bus != null && _bus.IsConnected)
                {
                    try
                    {
                        // 確保 BaseTopic 結尾正確處理
                        string baseTopic = Topic.TrimEnd('/') + "/";
                        string reportTopic = baseTopic + "Report";
                        
                        OnLogReceived(LogLevel.Info, $"設備監控啟動... 監聽: {reportTopic}, 心跳間隔: {heartbeatIntervalSeconds}s");
                        
                        // 1. 訂閱 Report Topic (接收來自機台的訊息)
                        await _bus.SubscribeAsync(reportTopic, (topic, message) => HandleCommand(message, topic));
                        
                        // 2. 啟動心跳計時器
                        StartHeartbeatTimer();

                        // 3. 啟動狀態補傳背景服務
                        StartStatusBufferProcessor(_linkedCts.Token);

                        // 4. 初始發送一次 AreYouThere 與 StatusReq
                        SendAreYouThere();
                        SendStatusRequest();
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Error, $"啟動監控失敗: {ex.Message}");
                        Connection = ConnectionStatus.Disconnected;
                    }
                }
            }, _linkedCts.Token);
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
                                _statusBuffer.TryDequeue(out _); // 發送成功才從隊列移除
                                OnLogReceived(LogLevel.Info, $"[補傳成功] 設備狀態/警報已同步至 MES (佇列剩餘: {_statusBuffer.Count})");
                            }
                            catch { /* 補傳失敗則等待下一次循環 */ }
                        }
                    }
                    await Task.Delay(30000, ct); // 每 30 秒嘗試補傳一次
                }
            }, ct);
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req)
        {
            _ = Task.Run(() =>
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

        private void StartHeartbeatTimer()
        {
            _heartbeatTimer?.Stop();
            _heartbeatTimer?.Dispose();

            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval * 1000);
            _heartbeatTimer.Elapsed += (s, e) =>
            {
                // 發送心跳
                SendAreYouThere();

                // 檢查是否超時 (規範 T1 Timeout 預設為 30s，這裡使用配置的間隔作為超時判定)
                if (_lastResponseTime != DateTime.MinValue && (DateTime.Now - _lastResponseTime).TotalSeconds > _heartbeatInterval + 5)
                {
                    if (Connection != ConnectionStatus.Disconnected)
                    {
                        OnLogReceived(LogLevel.Warn, $"通訊逾時: 超過 {_heartbeatInterval} 秒未收到回應。");
                        Connection = ConnectionStatus.Disconnected;
                    }
                }
            };
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;
        }

        private static readonly Newtonsoft.Json.JsonSerializerSettings _logSettings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        private void SendAreYouThere()
        {
            if (_bus == null || !_bus.IsConnected) return;

            try
            {
                var msg = new AreYouThereMessage
                {
                    CMD = CommandType.AreYouThere,
                    Date = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Machine = Id
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(msg, _logSettings);
                string commandTopic = Topic.TrimEnd('/') + "/Command";
                
                _bus.PublishAsync(commandTopic, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"發送心跳失敗: {ex.Message}");
            }
        }

        private void SendStatusRequest()
        {
            if (_bus == null || !_bus.IsConnected) return;
            try
            {
                var msg = new StatusReqMessage
                {
                    CMD = CommandType.StatusReq,
                    Date = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Machine = Id
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(msg, _logSettings);
                string commandTopic = Topic.TrimEnd('/') + "/Command";
                _bus.PublishAsync(commandTopic, json);
                OnLogReceived(LogLevel.Info, $"發送狀態查詢: {Id}");
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"發送狀態查詢失敗: {ex.Message}"); }
        }

        public void SendTimeCalibration()
        {
            if (_bus == null || !_bus.IsConnected) return;
            try
            {
                var msg = new TimeCalibrateMessage
                {
                    CMD = CommandType.TimeCalibrate,
                    Date = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Machine = Id
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(msg, _logSettings);
                string commandTopic = Topic.TrimEnd('/') + "/Command";
                _bus.PublishAsync(commandTopic, json);
                OnLogReceived(LogLevel.Info, $"發送時間校正指令給機台: {Id}");
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"發送時間校正失敗: {ex.Message}"); }
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
        private void HandleCommand(string Message , string topic )  //邏輯判斷
        {
            if (string.IsNullOrEmpty(Message)) return;

            // 只要收到任何訊息，就更新最後回應時間並標記為已連線
            _lastResponseTime = DateTime.Now;
            
            // 1. 處理 CyntecMqttLib 的 MessageEnvelope 封裝
            string innerJson = Message;
            try
            {
                var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(Message);
                if (envelope != null && envelope["Payload"] != null)
                {
                    innerJson = envelope["Payload"].ToString();
                }
            }
            catch { /* 若非 Envelope 格式則嘗試原始解析 */ }

            if (Connection != ConnectionStatus.Connected)
            {
                Connection = ConnectionStatus.Connected;
                OnLogReceived(LogLevel.Info, "設備已連線 (收到訊息回應)");
            }
            
            // 嘗試將收到的訊息美化顯示 (確保 CMD 轉文字)
            string displayMessage = Message;
            try 
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(Message);
                displayMessage = Newtonsoft.Json.JsonConvert.SerializeObject(obj, _logSettings);
            } catch { }

            OnLogReceived(LogLevel.Info, $"收到資料: {displayMessage}");
                                                           
            BaseMessage msg = MessageFactory.Create(innerJson);     //對正確的 Json 做反序列化

            switch (msg.CMD) //處理個別 CMD
            {
                case CommandType.AreYouThere:
                    // 規範 2.7.2: 設備發送 AreYouThere，EAP 回覆 IamHere
                    SendReply(new IamHereMessage { CMD = CommandType.IamHere });
                    break;

                case CommandType.IamHere:
                    // 這是 EAP 發送 AreYouThere 後，機台的回覆
                    break;

                case CommandType.ReplyStatusReq:
                    var _replyStatus = (ReplyStatusReqMessage)msg;
                    if (int.TryParse(_replyStatus.CurrentStatus, out int statusCode))
                    {
                        if (StatusMap.TryGetValue(statusCode, out var syncState)) State = syncState;
                    }
                    if (!string.IsNullOrEmpty(_replyStatus.RecipeID)) CurrentRecipe = _replyStatus.RecipeID;
                    OnLogReceived(LogLevel.Info, $"收到狀態查詢回覆: {Id}, 狀態: {_replyStatus.CurrentStatus}, Recipe: {_replyStatus.RecipeID ?? "N/A"}");
                    break;

                case CommandType.ReplyTimeCalibrate:
                    var _replyTime = (ReplyTimeCalibrateMessage)msg;
                    OnLogReceived(LogLevel.Info, $"收到機台時間校正回覆: {Id}, 結果: {_replyTime.RtnResult}");
                    break;

                case CommandType.UserID_Verify:
                    var _userVerify = (UserIDVerifyMessage)msg;
                    string requestBarcode = string.IsNullOrEmpty(_userVerify.Barcode) ? _userVerify.UserID : _userVerify.Barcode;
                    OnLogReceived(LogLevel.Info, $"收到人員驗證請求，Barcode: {requestBarcode}");
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            // 呼叫 MES UserVerify 交易
                            var mesReq = new UserVerifyRequest 
                            { 
                                EqpNo = Id, 
                                User2DBarCode = requestBarcode // 使用從機台取得的 Barcode
                                // UserID 與 UserPassword 在建構子中已設為 null，序列化時會被忽略
                            };
                            
                            UserVerifyReply mesReply = MESMessageFactory.CallMes<UserVerifyRequest, UserVerifyReply>(mesReq);
                            
                            var reply = new ReplyUserIDVerifyMessage
                            {
                                CMD = CommandType.ReplyUserID_Verify,
                                UserID = mesReply.UserID ?? "", // 回傳 MES 解析後的正確工號
                                Barcode = requestBarcode, // 回傳原始掃描值
                                RtnResult = mesReply.Result == "success" ? RtnResult.PASS : RtnResult.FAIL,
                                RtnMsg = mesReply.Result == "success" ? $"歡迎登入, {mesReply.UserName}" : mesReply.Message
                            };

                            if (reply.RtnResult == RtnResult.PASS && !string.IsNullOrEmpty(reply.UserID))
                            {
                                _currentOperator = reply.UserID; // 成功登入後記錄操作員
                            }

                            SendReply(reply);
                        }
                        catch (Exception ex) 
                        { 
                            OnLogReceived(LogLevel.Error, $"人員驗證異常: {ex.Message}");
                            SendReply(new ReplyUserIDVerifyMessage { CMD = CommandType.ReplyUserID_Verify, Barcode = requestBarcode, RtnResult = RtnResult.FAIL, RtnMsg = "EAP 系統異常" });
                        }
                    });
                    break;

                case CommandType.TrackInReq:
                    var _trackIn = (TrackInReqMessage)msg;
                    OnLogReceived(LogLevel.Info, $"收到進站請求: {_trackIn.MaterialID}, 工單: {_trackIn.WorkOrder}, 載具: {_trackIn.CassetteID}");
                    
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            string finalWorkOrder = _trackIn.WorkOrder;

                            // 分流邏輯：
                            if (string.IsNullOrEmpty(finalWorkOrder))
                            {
                                if (!string.IsNullOrEmpty(_trackIn.CassetteID))
                                {
                                    // 情境 2: 只有 CassetteID，需要查詢資料庫
                                    OnLogReceived(LogLevel.Info, $"正在查詢載具 {_trackIn.CassetteID} 綁定的工單...");
                                    finalWorkOrder = QueryWorkOrderFromDb(_trackIn.CassetteID);
                                    
                                    if (string.IsNullOrEmpty(finalWorkOrder))
                                    {
                                        OnLogReceived(LogLevel.Error, $"載具 {_trackIn.CassetteID} 查詢不到綁定工單，拒絕進站。");
                                        SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "載具未綁定工單" });
                                        return;
                                    }
                                    OnLogReceived(LogLevel.Info, $"載具 {_trackIn.CassetteID} 綁定工單為: {finalWorkOrder}");
                                }
                                else
                                {
                                    // 異常情境: 兩者皆無值
                                    OnLogReceived(LogLevel.Error, "進站請求遺漏工單與載具資訊，拒絕進站。");
                                    SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "遺漏工單或載具資訊" });
                                    return;
                                }
                            }
                            // 情境 1: 有 WorkOrder (finalWorkOrder 已有值)，續行流程

                            // 1. 呼叫 MES WOQRY 獲取工單詳細資訊
                            var qryReq = new WOQRYRequest 
                            { 
                                WONO = finalWorkOrder,
                                EqpNo = Id,
                                UserID = string.IsNullOrEmpty(_trackIn.UserID) ? _currentOperator : _trackIn.UserID
                            };
                            WOQRYReply qryReply = MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq);
                            
                            if (qryReply.Result != "success")
                            {
                                SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = qryReply.Message });
                                return;
                            }

                            string recipeID = ExtractRecipeID(qryReply.InputParamListDT);
                            OnLogReceived(LogLevel.Info, $"工單查詢成功，取得 RecipeID: {recipeID}");

                            var ppSelect = new PPSelectMessage { CMD = CommandType.PP_SELECT, RecipeID = recipeID };
                            SendReply(ppSelect);

                            lock (_pendingTrackIn)
                            {
                                // 更新請求內容為最終工單，以便後續進站使用
                                _trackIn.WorkOrder = finalWorkOrder;
                                _pendingTrackIn[Id] = new TrackInContext { OriginalRequest = _trackIn, RecipeID = recipeID };
                            }
                        }
                        catch (Exception ex) { OnLogReceived(LogLevel.Error, $"進站啟動異常: {ex.Message}"); }
                    });
                    break;

                case CommandType.ReplyPP_SELECT:
                    var _replyPP = (ReplyPPSelectMessage)msg;
                    OnLogReceived(LogLevel.Info, $"收到機台 Recipe 切換回覆: {_replyPP.RtnResult}");
                    _ = Task.Run(() =>
                    {
                        TrackInContext context = null;
                        lock (_pendingTrackIn)
                        {
                            if (_pendingTrackIn.TryGetValue(Id, out context)) _pendingTrackIn.Remove(Id);
                        }

                        if (_replyPP.RtnResult == RtnResult.PASS)
                        {
                            if (context != null)
                            {
                                this.CurrentRecipe = context.RecipeID;
                            }
                            else
                            {
                                try { this.CurrentRecipe = Newtonsoft.Json.Linq.JObject.Parse(innerJson)["RecipeID"]?.ToString(); } catch { }
                            }

                            if (context == null) return; 

                            try
                            {
                                var checkInReq = new WOCHECKINRequest 
                                { 
                                    WONO = context.OriginalRequest.WorkOrder,
                                    EqpNo = Id,
                                    UserID = _currentOperator
                                };
                                WOCHECKINReply checkInReply = MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(checkInReq);

                                var reply = new ReplyTrackInReqMessage
                                {
                                    CMD = CommandType.ReplyTrackInReq,
                                    RtnResult = (checkInReply.Result == "success" || checkInReply.Result == "PASS") ? RtnResult.PASS : RtnResult.FAIL,
                                    RtnMsg = checkInReply.Message
                                };
                                SendReply(reply);
                            }
                            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"進站確認異常: {ex.Message}"); }
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Error, "機台 Recipe 切換失敗，終止進站流程。");
                            if (context != null)
                            {
                                SendReply(new ReplyTrackInReqMessage { CMD = CommandType.ReplyTrackInReq, RtnResult = RtnResult.FAIL, RtnMsg = "機台 Recipe 切換失敗" });
                            }
                        }
                    });
                    break;

                case CommandType.TrackOutReq:
                    var _trackOut = (TrackOutReqMessage)msg;
                    OnLogReceived(LogLevel.Info, $"處理出站請求: {_trackOut.MaterialID}, 數量: {_trackOut.Qty}");
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            var mesReq = new WOCHECKOUTRequest 
                            { 
                                WONO = _trackOut.WorkOrder, 
                                EqpNo = Id,
                                UserID = string.IsNullOrEmpty(_trackOut.UserID) ? _currentOperator : _trackOut.UserID,
                                OutPut = _trackOut.Qty.ToString()
                            };
                            WOCHECKOUTReply mesReply = MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(mesReq);
                            
                            var reply = new ReplyTrackOutReqMessage
                            {
                                CMD = CommandType.ReplyTrackOutReq,
                                RtnResult = (mesReply.Result == "success" || mesReply.Result == "PASS") ? RtnResult.PASS : RtnResult.FAIL,
                                RtnMsg = mesReply.Message
                            };
                            SendReply(reply);
                        }
                        catch (Exception ex) { OnLogReceived(LogLevel.Error, $"出站處理異常: {ex.Message}"); }
                    });
                    break;

                case CommandType.ALARM:
                    var _Alarm = (AlarmMessage)msg;
                    State = EquipmentState.Alarm;
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = _Alarm.AlarmCode });
                    break;

                case CommandType.StatusChangeReport:
                    var _Status = (StatusChangeReportMessage)msg;
                    if (StatusMap.TryGetValue(_Status.NewStatus, out var reportState)) State = reportState;
                    
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = _Status.NewStatus.ToString() });
                    SendReply(new ReplyStatusChangeReportMessage { CMD = CommandType.ReplyStatusChangeReport, RtnResult = RtnResult.PASS });
                    break;

                case CommandType.ProcessDataReport:
                    var _data = (ProcessDataReportMessage)msg;
                    OnLogReceived(LogLevel.Info, $"處理製程資料上報: {_data.MaterialID}");
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            // 呼叫 MES 記錄 EDC (邏輯略)
                            SendReply(new ReplyProcessDataReportMessage { CMD = CommandType.ReplyProcessDataReport, RtnResult = RtnResult.PASS });
                        }
                        catch (Exception ex) { OnLogReceived(LogLevel.Error, $"製程資料上報異常: {ex.Message}"); }
                    });
                    break;

                case CommandType.BatchProcessDataReport:
                    var _batchData = (BatchProcessDataReportMessage)msg;
                    OnLogReceived(LogLevel.Info, $"處理批次製程資料上報: 數量 {_batchData.BatchQty}");
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            // 呼叫 MES 記錄批次 EDC (邏輯略)
                            SendReply(new ReplyBatchProcessDataReportMessage { CMD = CommandType.ReplyBatchProcessDataReport, RtnResult = RtnResult.PASS });
                        }
                        catch (Exception ex) { OnLogReceived(LogLevel.Error, $"批次資料上報異常: {ex.Message}"); }
                    });
                    break;

                default:
                    OnLogReceived(LogLevel.Debug, $"未處理或回覆型訊息: {msg.CMD}");
                    break;
            } 
        }

        private string QueryWorkOrderFromDb(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString))
            {
                OnLogReceived(LogLevel.Error, "SQL Server 配置缺失，無法執行載具查詢。");
                return null;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string sql = $"SELECT TOP 1 {DbConfig.WoColumn} FROM {DbConfig.BindingTable} WHERE {DbConfig.CassetteColumn} = @cid";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"資料庫查詢異常: {ex.Message}");
                return null;
            }
        }

        private string ExtractRecipeID(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(inputParamListDt);
                var recipeObj = System.Linq.Enumerable.FirstOrDefault(list, x => 
                    x["ParamName"]?.ToString() == "RECIPEID" || x["RefFieldCode"]?.ToString() == "RECIPE ID");
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            }
            catch { return "ERROR"; }
        }

        private void SendReply<T>(T replyMsg) where T : BaseMessage
        {
            if (_bus == null || !_bus.IsConnected) return;
            try
            {
                replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
                replyMsg.Machine = Id;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(replyMsg, _logSettings);
                string commandTopic = Topic.TrimEnd('/') + "/Command";
                _bus.PublishAsync(commandTopic, json);
                OnLogReceived(LogLevel.Info, $"發送資料: {json}");
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"發送回覆失敗: {ex.Message}"); }
        }
    }
}
