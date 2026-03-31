using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;
using Microsoft.Extensions.Options;
using Secs4Net;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Enums;

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private CancellationTokenSource _linkedCts;
        private ISecsGem _secsGem;
        private SecsDictionary _dictionary;
        private System.Timers.Timer _retryTimer;
        private string _currentOperator = "UNKNOWN";

        // 用於追蹤正在處理中的進站上下文
        private class TrackInContext
        {
            public string CassetteID { get; set; }
            public string WorkOrder { get; set; }
            public string RecipeID { get; set; }
        }
        private readonly Dictionary<string, TrackInContext> _pendingTrackIn = new Dictionary<string, TrackInContext>();

        // 用於存放暫時發送失敗的狀態更新請求 (斷線緩衝)
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        private static readonly Dictionary<int, EquipmentState> SecsStatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Down },    // DOWN
           { 1, EquipmentState.Idle },    // READY
           { 2, EquipmentState.Idle },    // IDLE
           { 3, EquipmentState.Running }, // RUN
           { 4, EquipmentState.Paused },  // PAUSE
           { 5, EquipmentState.Down },    // MAINTE
           { 6, EquipmentState.Idle }     // READY OFF
        };

        public string Ip { get; set; }
        public int Port { get; set; }
        public ushort DeviceId { get; set; } = 0;
        public string DictionaryPath { get; set; }
        public bool IsActive { get; set; } = true;

        // 資料庫設定
        public DbSettings DbConfig { get; set; }

        public SecsEquipment()
        {
            Protocol = "SECS";
            Connection = ConnectionStatus.Disconnected;
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus _bus, int heartbeatIntervalSeconds)
        {
            try
            {
                OnLogReceived(LogLevel.Info, $"[START] SECS module initialization for {Id}...");
                
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
                CancellationToken token = _linkedCts.Token;

                // Load Dictionary
                string fullDictPath = Path.IsPathRooted(DictionaryPath) ? DictionaryPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictionaryPath);
                if (File.Exists(fullDictPath))
                {
                    try {
                        _dictionary = SecsDictionary.Load(fullDictPath);
                        OnLogReceived(LogLevel.Info, $"[START] Dictionary loaded: {fullDictPath}");
                    } catch (Exception ex) {
                        OnLogReceived(LogLevel.Error, $"[ERROR] Dictionary parse failed: {ex.Message}");
                    }
                }

                // Start status buffer processor
                StartStatusBufferProcessor(token);

                // 啟動重連監控
                _retryTimer = new System.Timers.Timer(15000);
                _retryTimer.Elapsed += (s, e) => {
                    if (Connection == ConnectionStatus.Disconnected && !token.IsCancellationRequested)
                    {
                        OnLogReceived(LogLevel.Debug, "[RETRY] Triggering task...");
                        _ = RunConnectionTask(token);
                    }
                };
                _retryTimer.AutoReset = true;
                _retryTimer.Enabled = true;

                _ = RunConnectionTask(token);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Fatal, $"[CRITICAL] Start crash: {ex.Message}");
            }
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
                                await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req));
                                _statusBuffer.TryDequeue(out _);
                                OnLogReceived(LogLevel.Info, $"[補傳成功] SECS 設備狀態/警報已同步至 MES (佇列剩餘: {_statusBuffer.Count})");
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
                    OnLogReceived(LogLevel.Warn, $"[MES 斷線] SECS 狀態同步失敗，已進入緩衝區。原因: {ex.Message}");
                    _statusBuffer.Enqueue(req);
                }
            });
        }

        private async Task RunConnectionTask(CancellationToken token)
        {
            if (Connection != ConnectionStatus.Disconnected) return;

            try
            {
                if (!IPAddress.TryParse(Ip, out var ipAddr))
                {
                    OnLogReceived(LogLevel.Error, $"[CONN] Invalid IP address format: {Ip}");
                    return;
                }
                OnLogReceived(LogLevel.Info, $"[CONN] IP validated: {ipAddr}. Port: {Port}. DeviceID: {DeviceId}");

                Connection = ConnectionStatus.Connecting;
                var options = new SecsGemOptions
                {
                    IpAddress = Ip,
                    Port = Port,
                    DeviceId = DeviceId,
                    IsActive = IsActive,
                    T3 = 45000,
                    T5 = 10000,
                    T6 = 5000, 
                    T7 = 10000,
                    T8 = 5000,
                    LinkTestInterval = 60000
                };

                var optionsWrapper = Options.Create(options);
                var logger = new SecsLogger(this);
                
                OnLogReceived(LogLevel.Info, $"[CONN] Opening {(IsActive ? "Active" : "Passive")} HSMS Transport...");
                
                await using (var connection = new HsmsConnection(optionsWrapper, logger))
                using (var gem = new SecsGem(optionsWrapper, connection, logger))
                {
                    _secsGem = gem;
                    OnLogReceived(LogLevel.Info, "[CONN] HSMS Transport and SECS-II engine instantiated.");

                    var commTask = Task.Run(async () => {
                        ConnectionState lastState = (ConnectionState)99;
                        while (!token.IsCancellationRequested && _secsGem != null)
                        {
                            var currentState = connection.State;
                            if (currentState != lastState)
                            {
                                OnLogReceived(LogLevel.Info, $"[HSMS] Transport State: {currentState}");
                                lastState = currentState;
                                
                                if (currentState == ConnectionState.Selected)
                                {
                                    OnLogReceived(LogLevel.Info, "[CONN] Transport Selected. Initiating S1F13 Handshake...");
                                    await SendS1F13(gem);
                                }
                                else if ((int)currentState == 0) // Disconnected
                                {
                                    Connection = ConnectionStatus.Disconnected;
                                }
                            }
                            await Task.Delay(1000, token);
                        }
                    }, token);

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            OnLogReceived(LogLevel.Debug, "[CONN] Listening for Primary Messages...");
                            await foreach (var primaryMessageWrapper in gem.GetPrimaryMessageAsync(token))
                            {
                                using (primaryMessageWrapper.PrimaryMessage)
                                {
                                    if (Connection != ConnectionStatus.Connected)
                                    {
                                        Connection = ConnectionStatus.Connected;
                                        OnLogReceived(LogLevel.Info, "[CONN] Logic Link established via Message reception.");
                                    }
                                    await HandlePrimaryMessage(primaryMessageWrapper);
                                }
                            }
                        }
                        catch (OperationCanceledException) { }
                        catch (Exception ex)
                        {
                            OnLogReceived(LogLevel.Error, $"[CONN] Message Loop Exception: {ex.Message}");
                        }
                    }, token);

                    OnLogReceived(LogLevel.Info, "[CONN] Starting HSMS Transport...");
                    connection.Start(token);
                    await Task.Delay(-1, token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[CONN] Task Exception: {ex.Message}");
            }
            finally
            {
                _secsGem = null;
                Connection = ConnectionStatus.Disconnected;
                OnLogReceived(LogLevel.Info, "[CONN] Transport closed.");
            }
        }

        private async Task HandlePrimaryMessage(PrimaryMessageWrapper message)
        {
            var s = message.PrimaryMessage.S;
            var f = message.PrimaryMessage.F;
            OnLogReceived(LogLevel.Info, $"[RECV] S{s}F{f}");

            switch (s)
            {
                case 1:
                    if (f == 1) await message.TryReplyAsync(new SecsMessage(1, 2) { SecsItem = Item.L(Item.A(Name), Item.A(Id)) });
                    else if (f == 13) await message.TryReplyAsync(new SecsMessage(1, 14) { SecsItem = Item.B(0) });
                    break;
                case 6:
                    if (f == 11) await HandleS6F11(message);
                    break;
                case 5:
                    if (f == 1) await HandleS5F1(message);
                    break;
            }
        }

        private async Task HandleS6F11(PrimaryMessageWrapper message)
        {
            try
            {
                var rootList = message.PrimaryMessage.SecsItem;
                if (rootList == null || rootList.Count < 3) return;

                string ceid = rootList.Items[1].GetString();
                string eventName = _dictionary?.GetEventName(ceid) ?? ceid;
                OnLogReceived(LogLevel.Info, $"[EVENT] {eventName} (CEID:{ceid})");
// 處理狀態同步 (嘗試從報告列表中找出 VID 102)
string statusCode = ExtractVidValue(rootList, "102");
if (int.TryParse(statusCode, out int code))
{
    if (SecsStatusMap.TryGetValue(code, out var mappedState))
    {
        // 狀態保護邏輯：如果目前是 Alarm 且新狀態是 Idle/Running，除非是特定解除事件，否則不隨便覆蓋
        if (State == EquipmentState.Alarm && (mappedState == EquipmentState.Idle || mappedState == EquipmentState.Running))
        {
            // 保持為 Alarm，不執行狀態變更
        }
        else
        {
            State = mappedState;
        }
    }
}

                switch (ceid)
                {
                    case "200": // AUTO FRAME LOADING (Track-In)
                        await HandleTrackInEvent(rootList);
                        break;
                    case "206": // AUTO UNLOADING (Track-Out)
                        await HandleTrackOutEvent(rootList);
                        break;
                    case "50": case "51": case "52": case "53":
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = statusCode ?? "3" });
                        break;
                }

                await message.TryReplyAsync(new SecsMessage(6, 12) { SecsItem = Item.B(0) });
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] S6F11: {ex.Message}"); }
        }

        private async Task HandleTrackInEvent(Item rootList)
        {
            try
            {
                string recipeId = ExtractVidValue(rootList, "111") ?? "UNKNOWN";
                string cassetteId = ExtractVidValue(rootList, "300") ?? ExtractVidValue(rootList, "320") ?? "UNKNOWN";

                OnLogReceived(LogLevel.Info, $"[AUTO] 偵測到進站事件: Cassette={cassetteId}, Recipe={recipeId}");

                string wo = QueryWorkOrderFromDb(cassetteId);
                if (string.IsNullOrEmpty(wo))
                {
                    OnLogReceived(LogLevel.Warn, $"[AUTO] 進站中止: 載具 {cassetteId} 在資料庫中查無綁定工單。");
                    return;
                }

                var qryReq = new WOQRYRequest { WONO = wo, EqpNo = Id, UserID = _currentOperator };
                var qryReply = await Task.Run(() => MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq));

                if (qryReply.Result != "success")
                {
                    OnLogReceived(LogLevel.Error, $"[MES] WOQRY 失敗: {qryReply.Message}");
                    return;
                }

                string targetRecipe = ExtractRecipeFromMes(qryReply.InputParamListDT);
                
                OnLogReceived(LogLevel.Info, $"[CMD] 向機台要求下換 Recipe (PPID): {targetRecipe}");
                var remoteCmd = new SecsMessage(2, 41)
                {
                    SecsItem = Item.L(
                        Item.A("PP-SELECT"),
                        Item.L(
                            Item.L(Item.A("PPID"), Item.A(targetRecipe))
                        )
                    )
                };

                using (var reply = await _secsGem.SendAsync(remoteCmd))
                {
                    if (reply.S == 2 && reply.F == 42)
                    {
                        byte hcack = reply.SecsItem.Items[0].FirstValue<byte>();
                        if (hcack == 0)
                        {
                            OnLogReceived(LogLevel.Info, "[AUTO] 機台已接受 PP-SELECT，執行正式進站報工...");
                            var checkInReq = new WOCHECKINRequest { WONO = wo, EqpNo = Id, UserID = _currentOperator };
                            var checkInReply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(checkInReq));
                            OnLogReceived(LogLevel.Info, $"[MES] 進站報工完成: {checkInReply.Result}");
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Error, $"[CMD] 機台拒絕 PP-SELECT 指令. HCACK: {hcack}");
                        }
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] 自動進站流程崩潰: {ex.Message}"); }
        }

        private async Task HandleTrackOutEvent(Item rootList)
        {
            try
            {
                string wo = ExtractVidValue(rootList, "111") ?? "UNKNOWN";
                OnLogReceived(LogLevel.Info, $"[AUTO] 執行自動出站報工: WorkOrder={wo}");

                var mesReq = new WOCHECKOUTRequest { WONO = wo, EqpNo = Id, UserID = _currentOperator, OutPut = "1" };
                var mesReply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(mesReq));
                OnLogReceived(LogLevel.Info, $"[MES] 出站報工結果: {mesReply.Result}");
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] 出站流程異常: {ex.Message}"); }
        }

        private string QueryWorkOrderFromDb(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return null;
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string sql = $"SELECT TOP 1 {DbConfig.WoColumn} FROM {DbConfig.BindingTable} WHERE {DbConfig.CassetteColumn} = @cid";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        return cmd.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"資料庫查詢失敗: {ex.Message}"); return null; }
        }

        private string ExtractVidValue(Item rootList, string targetVid)
        {
            if (rootList == null) return null;
            
            // 遞迴搜尋所有非 List 項目
            if (rootList.Format != SecsFormat.List)
            {
                // 根據不同的格式使用正確的泛型型別提取值
                switch (rootList.Format)
                {
                    case SecsFormat.ASCII:
                    case SecsFormat.JIS8:
                        return rootList.GetString();
                    case SecsFormat.U1:
                    case SecsFormat.Binary:
                        return rootList.FirstValue<byte>().ToString();
                    case SecsFormat.U2:
                        return rootList.FirstValue<ushort>().ToString();
                    case SecsFormat.U4:
                        return rootList.FirstValue<uint>().ToString();
                    case SecsFormat.U8:
                        return rootList.FirstValue<ulong>().ToString();
                    case SecsFormat.I1:
                        return rootList.FirstValue<sbyte>().ToString();
                    case SecsFormat.I2:
                        return rootList.FirstValue<short>().ToString();
                    case SecsFormat.I4:
                        return rootList.FirstValue<int>().ToString();
                    case SecsFormat.I8:
                        return rootList.FirstValue<long>().ToString();
                    case SecsFormat.F4:
                        return rootList.FirstValue<float>().ToString();
                    case SecsFormat.F8:
                        return rootList.FirstValue<double>().ToString();
                    case SecsFormat.Boolean:
                        return rootList.FirstValue<bool>().ToString();
                    default:
                        return string.Empty;
                }
            }

            foreach (var item in rootList.Items)
            {
                var found = ExtractVidValue(item, targetVid);
                if (!string.IsNullOrEmpty(found)) return found;
            }
            return null; 
        }

        private string ExtractRecipeFromMes(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(inputParamListDt);
                var recipeObj = list.FirstOrDefault(x => x["ParamName"]?.ToString() == "RECIPEID" || x["RefFieldCode"]?.ToString() == "RECIPE ID");
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            }
            catch { return "ERROR"; }
        }

        private async Task HandleS5F1(PrimaryMessageWrapper message)
        {
            try
            {
                var rootList = message.PrimaryMessage.SecsItem;
                if (rootList != null && rootList.Count >= 3)
                {
                    byte alcd = rootList.Items[0].FirstValue<byte>();
                    string altx = rootList.Items[2].GetString();
                    
                    // SECS 標準：第 8 位元為 1 表示發生，為 0 表示清除。
                    // 為了容錯，如果模擬器發送 1 或 0 (非標準位元操作)，也進行相應判定。
                    bool isSet = (alcd & 0x80) != 0 || alcd == 1;
                    
                    OnLogReceived(LogLevel.Info, $"[ALARM] (RawALCD:0x{alcd:X2}) {(isSet ? "SET" : "CLEAR")}: {altx}");
                    
                    if (isSet)
                    {
                        State = EquipmentState.Alarm;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = altx });
                    }
                    else
                    {
                        // 告警清除時，不強制轉為 IDLE，應讓後續事件更新真實狀態。
                        // 若要清除 UI 紅色狀態，可考慮轉為 Idle 或保持原樣直到 S6F11。
                        if (State == EquipmentState.Alarm) State = EquipmentState.Idle;
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = altx });
                    }
                }
                await message.TryReplyAsync(new SecsMessage(5, 2) { SecsItem = Item.B(0) });
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] S5F1: {ex.Message}"); }
        }

        private async Task SendS1F13(ISecsGem gem)
        {
            try
            {
                using (var reply = await gem.SendAsync(new SecsMessage(1, 13) { SecsItem = Item.L() }))
                {
                    if (reply.S == 1 && reply.F == 14)
                    {
                        byte commack = reply.SecsItem.Items[0].FirstValue<byte>();
                        if (commack == 0)
                        {
                            OnLogReceived(LogLevel.Info, "[CONN] S1F13 Success. Comm established.");
                            Connection = ConnectionStatus.Connected;
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, $"[CONN] S1F13 Denied. COMMACK: {commack}");
                        }
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Debug, $"[CONN] S1F13 attempt failed: {ex.Message}"); }
        }

        public override async Task SendTimeCalibration()
        {
            var gem = _secsGem;
            if (Connection != ConnectionStatus.Connected || gem == null) return;
            try
            {
                string timeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                using (var reply = await gem.SendAsync(new SecsMessage(2, 31) { SecsItem = Item.A(timeStr) }))
                {
                    if (reply.S == 2 && reply.F == 32) OnLogReceived(LogLevel.Info, "[CMD] Time sync success.");
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] Time sync failed: {ex.Message}"); }
        }

        public override void Stop()
        {
            _retryTimer?.Stop();
            _retryTimer?.Dispose();
            _linkedCts?.Cancel();
            _linkedCts?.Dispose();
        }

        private class SecsLogger : ISecsGemLogger
        {
            private readonly SecsEquipment _eq;
            public SecsLogger(SecsEquipment eq) => _eq = eq;
            public void MessageIn(SecsMessage m, int id) => _eq.OnLogReceived(LogLevel.Info, $"[<-] S{m.S}F{m.F}");
            public void MessageOut(SecsMessage m, int id) => _eq.OnLogReceived(LogLevel.Info, $"[->] S{m.S}F{m.F}");
            public void Debug(string m) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m}");
            public void Info(string m) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m}");
            public void Warning(string m) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m}");
            public void Error(string m) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m}");
            public void Error(string m, Exception ex) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m} {ex?.Message}");
            public void Error(string m, SecsMessage msg, Exception ex) => _eq.OnLogReceived(LogLevel.Info, $"[Secs4Net] {m} S{msg?.S}F{msg?.F} {ex?.Message}");
        }
    }
}
