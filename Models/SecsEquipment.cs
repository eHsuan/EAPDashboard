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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Core.Messages;

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private CancellationTokenSource _linkedCts;
        private ISecsGem _secsGem;
        private SecsDictionary _dictionary;
        private System.Timers.Timer _retryTimer;
        private System.Timers.Timer _aliveCheckTimer;
        private bool _aliveCheckAwaitingReply = false;

        private List<string> _activeTraceParameters = new List<string>();
        private readonly object _traceLock = new object();

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        private static readonly Dictionary<int, EquipmentState> SecsStatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Down }, { 1, EquipmentState.Idle }, { 2, EquipmentState.Idle },
           { 3, EquipmentState.Running }, { 4, EquipmentState.Paused }, { 5, EquipmentState.Down }, { 6, EquipmentState.Idle }
        };

        public string Ip { get; set; }
        public int Port { get; set; }
        public ushort DeviceId { get; set; } = 0;
        public string DictionaryPath { get; set; }
        public bool IsActive { get; set; } = true;

        public SecsEquipment()
        {
            Protocol = "SECS";
            Connection = ConnectionStatus.Disconnected;
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30)
        {
            try {
                OnLogReceived(LogLevel.Info, $"[START] SECS module initialized for {Id}.");
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);

                string fullDictPath = Path.IsPathRooted(DictionaryPath) ? DictionaryPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictionaryPath);
                if (File.Exists(fullDictPath)) _dictionary = SecsDictionary.Load(fullDictPath);

                LoadAlarmMappings();
                _ = StartStatusBufferProcessor(_linkedCts.Token);

                // 啟動主動連線偵測 Loop
                _retryTimer = new System.Timers.Timer(15000);
                _retryTimer.Elapsed += (s, e) => { if (Connection == ConnectionStatus.Disconnected) _ = RunConnectionTask(_linkedCts.Token); };
                _retryTimer.AutoReset = true; _retryTimer.Enabled = true;

                // 啟動應用層 S1F1 存活檢測計時器
                _aliveCheckTimer = new System.Timers.Timer(30000); // 每 30 秒 Alive Check
                _aliveCheckTimer.Elapsed += async (s, e) => await SendS1F1();
                _aliveCheckTimer.AutoReset = true; _aliveCheckTimer.Enabled = true;

                _ = RunConnectionTask(_linkedCts.Token);
            } catch (Exception ex) { OnLogReceived(LogLevel.Fatal, $"[CRITICAL] SECS Crash: {ex.Message}"); }
        }

        private async Task SendS1F1()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return;

            if (_aliveCheckAwaitingReply)
            {
                OnLogReceived(LogLevel.Error, "[ALIVE_CHECK] S1F1 response timeout (30s). Equipment software application frozen. Forcing reconnect...");
                _aliveCheckAwaitingReply = false;
                Connection = ConnectionStatus.Disconnected;
                (_secsGem as IDisposable)?.Dispose(); // 強制觸發連線斷開
                return;
            }

            try
            {
                _aliveCheckAwaitingReply = true;
                using (var reply = await _secsGem.SendAsync(SecsMessages.S1F1()))
                {
                    if (reply.S == 1 && reply.F == 2)
                    {
                        _aliveCheckAwaitingReply = false;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[ALIVE_CHECK] Send S1F1 failed: {ex.Message}. Disconnecting...");
                _aliveCheckAwaitingReply = false;
                Connection = ConnectionStatus.Disconnected;
            }
        }

        private async Task RunConnectionTask(CancellationToken token)
        {
            if (Connection != ConnectionStatus.Disconnected) return;
            try {
                Connection = ConnectionStatus.Connecting;
                var options = new SecsGemOptions { IpAddress = Ip, Port = Port, DeviceId = DeviceId, IsActive = IsActive, T3 = 45000, T5 = 10000, T6 = 5000, T7 = 10000, T8 = 5000 };
                var optionsWrapper = Options.Create(options);
                var logger = new SecsLogger(this);
                
                await using (var connection = new HsmsConnection(optionsWrapper, logger))
                using (var gem = new SecsGem(optionsWrapper, connection, logger))
                {
                    _secsGem = gem;
                    var commTask = Task.Run(async () => {
                        ConnectionState lastState = (ConnectionState)99;
                        while (!token.IsCancellationRequested && _secsGem != null) {
                            if (connection.State != lastState) {
                                OnLogReceived(LogLevel.Info, $"[HSMS] State: {connection.State}");
                                lastState = connection.State;
                                if (connection.State == ConnectionState.Selected) await SendS1F13(gem);
                                else if ((int)connection.State == 0) Connection = ConnectionStatus.Disconnected;
                            }
                            await Task.Delay(1000, token);
                        }
                    }, token);

                    _ = Task.Run(async () => {
                        try {
                            await foreach (var wrapper in gem.GetPrimaryMessageAsync(token)) {
                                using (wrapper.PrimaryMessage) {
                                    if (Connection != ConnectionStatus.Connected) Connection = ConnectionStatus.Connected;
                                    await HandlePrimaryMessage(wrapper);
                                }
                            }
                        } catch { }
                    }, token);

                    connection.Start(token);
                    await Task.Delay(-1, token);
                }
            } catch { Connection = ConnectionStatus.Disconnected; }
        }

        private async Task HandlePrimaryMessage(PrimaryMessageWrapper message)
        {
            var s = message.PrimaryMessage.S; var f = message.PrimaryMessage.F;
            OnLogReceived(LogLevel.Info, $"[RECV] S{s}F{f}");
            switch (s) {
                case 1:
                    if (f == 1) await HandleS1F1(message);
                    else if (f == 13) await HandleS1F13(message);
                    break;
                case 5: 
                    if (f == 1) await HandleS5F1(message); 
                    break;
                case 6: 
                    if (f == 1) await HandleS6F1(message);
                    else if (f == 11) await HandleS6F11(message); 
                    break;
                case 10:
                    if (f == 3) await HandleS10F3(message);
                    break;
            }
        }

        private async Task HandleS1F1(PrimaryMessageWrapper message)
        {
            await message.TryReplyAsync(SecsMessages.S1F2(Name, Id));
        }

        private async Task HandleS10F3(PrimaryMessageWrapper message)
        {
            try {
                var data = message.PrimaryMessage.ParseS10F3();
                OnLogReceived(LogLevel.Info, $"[TERMINAL] Msg from Eqp (TID:{data.Tid}): {data.Text}");
                await message.TryReplyAsync(SecsMessages.S10F4());
            } catch (Exception ex) {
                OnLogReceived(LogLevel.Error, $"[S10F3] Error processing Terminal Message: {ex.Message}");
            }
        }

        private async Task HandleS1F13(PrimaryMessageWrapper message)
        {
            await message.TryReplyAsync(SecsMessages.S1F14(0));
        }

        private async Task HandleS5F1(PrimaryMessageWrapper message)
        {
            try {
                var data = message.PrimaryMessage.ParseS5F1();
                string alidStr = data.Alid.ToString();

                string mc = TransformAlarmCode(alidStr);
                if (mc == alidStr || data.Alid == 0)
                {
                    mc = TransformAlarmCode(data.Altx);
                }

                if (data.IsSet) { 
                    State = EquipmentState.Alarm; 
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = mc }); 
                }
                else { 
                    if (State == EquipmentState.Alarm) State = EquipmentState.Idle; 
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = mc }); 
                }
                await message.TryReplyAsync(SecsMessages.S5F2());
            } catch (Exception ex) {
                OnLogReceived(LogLevel.Error, $"[S5F1] Error processing Alarm: {ex.Message}");
            }
        }

        private async Task HandleS6F1(PrimaryMessageWrapper message)
        {
            try
            {
                var data = message.PrimaryMessage.ParseS6F1();
                OnLogReceived(LogLevel.Debug, $"[RECV] S6F1 Trace Data (TRID:{data.Trid}, SMPLN:{data.Smpln})");

                List<string> paramNames;
                lock (_traceLock)
                {
                    paramNames = _activeTraceParameters.ToList();
                }

                var payload = new Dictionary<string, object>();
                payload["TRID"] = data.Trid;
                payload["SampleNo"] = data.Smpln;
                payload["SampleTime"] = data.Stime;

                var traceParams = new List<object>();
                var parameterResults = new List<ParameterResultItem>();

                if (data.Values != null && data.Values.Count > 0)
                {
                    for (int i = 0; i < data.Values.Count; i++)
                    {
                        string paramName = (i < paramNames.Count) ? paramNames[i] : $"SVID_Index_{i}";
                        string valStr = ExtractVidValue(data.Values.Items[i], "0");

                        traceParams.Add(new { ParameterName = paramName, Value = valStr });
                        
                        double.TryParse(valStr, out double doubleVal);
                        parameterResults.Add(new ParameterResultItem
                        {
                            Parameter = paramName,
                            Value = doubleVal
                        });
                    }
                }

                payload["Parameters"] = traceParams;

                // 儲存生產資料 (TraceData 不寫入 SQLite 以免資料膨脹，僅記錄到 DataLogs CSV 檔案)
                await SaveProductionDataAsync("TraceData", "", payload, saveToDb: false);

                // 報工給 MES
                if (parameterResults.Count > 0)
                {
                    await ProcessKeyParametersReportAsync(parameterResults);
                }

                // 回覆 S6F2
                await message.TryReplyAsync(SecsMessages.S6F2());
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S6F1] Error processing Trace Data: {ex.Message}");
            }
        }

        private async Task HandleS6F11(PrimaryMessageWrapper message)
        {
            try {
                var data = message.PrimaryMessage.ParseS6F11();
                string ceidStr = data.Ceid.ToString();
                string logicalEvent = _dictionary?.GetEventName(ceidStr) ?? ceidStr;
                OnLogReceived(LogLevel.Info, $"[EVENT] {logicalEvent} (CEID:{ceidStr})");

                string statusVid = GetVidByLogicalName("Status") ?? GetVidByLogicalName("NewStatus") ?? "102";
                string statusCode = ExtractVidValue(data.Reports, statusVid);
                
                if (!string.IsNullOrEmpty(statusCode)) {
                    string mappedStatusStr = _dictionary?.MapStatus(statusCode) ?? statusCode;
                    if (State != EquipmentState.Alarm) {
                        State = ParseStateString(mappedStatusStr);
                    }
                }

                string upperEvent = logicalEvent.ToUpper();
                if (upperEvent == "TRACK_IN" || ceidStr == "200") {
                    await HandleS6F11TrackIn(data.Reports);
                }
                else if (upperEvent == "TRACK_OUT" || ceidStr == "206") {
                    await HandleS6F11TrackOut(data.Reports);
                }
                else if (upperEvent == "PROCESS_DATA" || upperEvent == "KEY_PARAMETERS" || ceidStr == "102") {
                    await HandleS6F11ProcessData(data.Reports, ceidStr, logicalEvent);
                }
                else if (upperEvent == "STATUS_CHANGE" || new[] { "50", "51", "52", "53" }.Contains(ceidStr)) {
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = statusCode ?? "3" });
                }

                await message.TryReplyAsync(SecsMessages.S6F12());
            } catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] S6F11: {ex.Message}"); }
        }

        private async Task HandleS6F11TrackIn(Item reports)
        {
            string cassetteVid = GetVidByLogicalName("CassetteID") ?? "300";
            string cid = ExtractVidValue(reports, cassetteVid);
            if (string.IsNullOrEmpty(cid)) {
                cid = ExtractVidValue(reports, "300") ?? ExtractVidValue(reports, "320") ?? "UNKNOWN";
            }

            var info = QueryBindingFromGateway(cid);
            string wo = info?.WorkOrder;
            List<string> panels = info?.Panels ?? new List<string>();

            string res = await ProcessTrackInAsync(cid, wo, panels, "UNKNOWN");
            if (res.StartsWith("PP-SELECT")) {
                string[] p = res.Split('|');
                
                var parameters = new Dictionary<string, string>();
                parameters["PPID"] = p[2];
                
                bool cmdSuccess = await SendS2F41("PP-SELECT", parameters);
                if (cmdSuccess) {
                    string fin = await ProcessTrackInFinalAsync(p[1], "UNKNOWN", cid);
                    OnLogReceived(LogLevel.Info, $"[AUTO] 進站報工完成: {fin}");
                } else {
                    OnLogReceived(LogLevel.Error, $"[AUTO] PP-SELECT command rejected by equipment. TrackIn aborted.");
                }
            }
        }

        private async Task HandleS6F11TrackOut(Item reports)
        {
            string woVid = GetVidByLogicalName("WorkOrder") ?? GetVidByLogicalName("RecipeID") ?? "111";
            string rid = ExtractVidValue(reports, woVid);
            if (string.IsNullOrEmpty(rid)) {
                rid = "UNKNOWN";
            }

            string res = await ProcessTrackOutAsync(rid, "1", "UNKNOWN", "UNKNOWN", new List<string>());
            OnLogReceived(LogLevel.Info, $"[AUTO] 出站報工結果: {res}");
        }

        private async Task HandleS6F11ProcessData(Item reports, string ceid, string logicalEvent)
        {
            try
            {
                var extractedData = new Dictionary<string, string>();
                var parameterResults = new List<ParameterResultItem>();

                if (_dictionary?.Variables != null)
                {
                    foreach (var kvp in _dictionary.Variables)
                    {
                        string vid = kvp.Key;
                        string varName = kvp.Value;
                        string val = ExtractVidValue(reports, vid);
                        if (!string.IsNullOrEmpty(val))
                        {
                            extractedData[varName] = val;

                            double.TryParse(val, out double doubleVal);
                            parameterResults.Add(new ParameterResultItem
                            {
                                Parameter = varName,
                                Value = doubleVal
                            });
                        }
                    }
                }

                // 儲存至本地資料庫及日誌檔
                await SaveProductionDataAsync(logicalEvent, "", extractedData);

                // 報工給 MES
                if (parameterResults.Count > 0)
                {
                    await ProcessKeyParametersReportAsync(parameterResults);
                }

                OnLogReceived(LogLevel.Info, $"[ProcessData] Dynamic extraction complete. Extracted {extractedData.Count} parameter(s) for event {logicalEvent} and reported to MES.");
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[ProcessData] Error processing Event S6F11: {ex.Message}");
            }
        }

        public async Task<bool> SendS1F17()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, "[S1F17] Sending Request ON-LINE...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S1F17()))
                {
                    if (reply.S == 1 && reply.F == 18 && reply.SecsItem != null)
                    {
                        byte onlack = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S1F18] Request ON-LINE Acknowledge: {onlack}");
                        return onlack == 0 || onlack == 2;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S1F17] Send S1F17 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F23(uint trid, uint dsper, uint tots, List<string> svids, List<string> paramNames)
        {
            lock (_traceLock)
            {
                _activeTraceParameters = paramNames ?? new List<string>();
            }
            return await SendS2F23Internal(trid, dsper, tots, svids);
        }

        private async Task<bool> SendS2F23Internal(uint trid, uint dsper, uint tots, List<string> svids)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F23] Initializing Trace (TRID:{trid}, DSPER:{dsper}, TOTS:{tots})");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F23(trid, dsper, tots, svids)))
                {
                    if (reply.S == 2 && reply.F == 24 && reply.SecsItem != null)
                    {
                        byte checkVal = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F24] Trace Initialize Ack: {checkVal}");
                        return checkVal == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F23] Send S2F23 Error: {ex.Message}");
            }
            return false;
        }

        private async Task<bool> SendS2F31()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                string hostTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                OnLogReceived(LogLevel.Info, $"[S2F31] Sending Date and Time Set ({hostTime})...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F31(hostTime)))
                {
                    if (reply.S == 2 && reply.F == 32 && reply.SecsItem != null)
                    {
                        byte tiack = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F32] Date and Time Set Acknowledge: {tiack}");
                        return tiack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F31] Send S2F31 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F37(bool enable)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F37] Sending Enable Events (CEED:{enable}, ALL)...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F37(enable)))
                {
                    if (reply.S == 2 && reply.F == 38 && reply.SecsItem != null)
                    {
                        byte erack = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F38] Enable Events Acknowledge: {erack}");
                        return erack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F37] Send S2F37 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F41(string rcmd, Dictionary<string, string> parameters = null)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F41] Sending Host Command: {rcmd}");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F41(rcmd, parameters)))
                {
                    if (reply.S == 2 && reply.F == 42 && reply.SecsItem != null)
                    {
                        byte hcack = reply.SecsItem.Items[0].FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F42] Host Command Ack: {hcack}");
                        return hcack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F41] Send Remote Command Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS5F3(bool enable)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S5F3] Sending Enable Alarms (ALED:{enable}, ALL)...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S5F3(enable)))
                {
                    if (reply.S == 5 && reply.F == 4 && reply.SecsItem != null)
                    {
                        byte alack = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S5F4] Enable Alarms Acknowledge: {alack}");
                        return alack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S5F3] Send S5F3 Error: {ex.Message}");
            }
            return false;
        }

        public override async Task SendTimeCalibration()
        {
            await SendS2F31();
        }

        private string GetVidByLogicalName(string logicalName)
        {
            if (_dictionary?.Variables == null) return null;
            foreach (var kvp in _dictionary.Variables)
            {
                if (kvp.Value.Equals(logicalName, StringComparison.OrdinalIgnoreCase))
                    return kvp.Key;
            }
            return null;
        }

        private EquipmentState ParseStateString(string stateStr)
        {
            if (string.IsNullOrEmpty(stateStr)) return EquipmentState.Idle;
            switch (stateStr.ToUpper())
            {
                case "RUN":
                case "RUNNING":
                    return EquipmentState.Running;
                case "ALARM":
                case "DOWN":
                    return EquipmentState.Down;
                case "PAUSE":
                case "PAUSED":
                    return EquipmentState.Paused;
                case "IDLE":
                    return EquipmentState.Idle;
            }

            if (int.TryParse(stateStr, out int code) && SecsStatusMap.TryGetValue(code, out var ms))
            {
                return ms;
            }

            return EquipmentState.Idle;
        }

        private string ExtractVidValue(Item root, string vid)
        {
            if (root == null) return null;
            if (root.Format != SecsFormat.List) {
                switch (root.Format) {
                    case SecsFormat.ASCII: return root.GetString();
                    case SecsFormat.U1: case SecsFormat.Binary: return root.FirstValue<byte>().ToString();
                    case SecsFormat.U2: return root.FirstValue<ushort>().ToString();
                    case SecsFormat.U4: return root.FirstValue<uint>().ToString();
                    case SecsFormat.I4: return root.FirstValue<int>().ToString();
                    case SecsFormat.Boolean: return root.FirstValue<bool>().ToString();
                    default: return string.Empty;
                }
            }
            if (string.IsNullOrEmpty(vid)) return string.Empty;
            foreach (var item in root.Items) {
                var f = ExtractVidValue(item, vid);
                if (!string.IsNullOrEmpty(f)) return f;
            }
            return null;
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req) { Task.Run(() => { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); } catch { _statusBuffer.Enqueue(req); } }); }
        private async Task StartStatusBufferProcessor(CancellationToken ct) {
            while (!ct.IsCancellationRequested) {
                if (!_statusBuffer.IsEmpty && _statusBuffer.TryPeek(out var req)) {
                    try { await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req)); _statusBuffer.TryDequeue(out _); }
                    catch { await Task.Delay(30000, ct); }
                }
                await Task.Delay(5000, ct);
            }
        }

        private async Task SendS1F13(ISecsGem gem) 
        { 
            try { 
                using (var reply = await gem.SendAsync(SecsMessages.S1F13())) { 
                    if (reply.S == 1 && reply.F == 14 && reply.SecsItem.Items[0].FirstValue<byte>() == 0) {
                        Connection = ConnectionStatus.Connected; 
                        OnLogReceived(LogLevel.Info, "[HSMS] Communication Established. Running initialization sequence...");
                        
                        _ = Task.Run(async () =>
                        {
                            await RunInitializationSequenceAsync();
                        });
                    }
                } 
            } catch { } 
        }

        private async Task RunInitializationSequenceAsync()
        {
            try
            {
                OnLogReceived(LogLevel.Info, "[INIT] Starting SECS/GEM Initialization Sequence...");

                // Step 1: S1F17 (Request ON-LINE)
                await Task.Delay(300);
                bool onlineOk = await SendS1F17();
                if (!onlineOk) OnLogReceived(LogLevel.Warn, "[INIT] S1F17 (Request ON-LINE) failed or rejected. Continuing sequence...");

                // Step 2: S2F31 (Date and Time Set)
                await Task.Delay(300);
                bool timeOk = await SendS2F31();
                if (!timeOk) OnLogReceived(LogLevel.Warn, "[INIT] S2F31 (Date & Time Set) failed or rejected. Continuing sequence...");

                // Step 3: S2F37 (Enable Events)
                await Task.Delay(300);
                bool eventsOk = await SendS2F37(true);
                if (!eventsOk) OnLogReceived(LogLevel.Warn, "[INIT] S2F37 (Enable Events) failed or rejected. Continuing sequence...");

                // Step 4: S5F3 (Enable Alarms)
                await Task.Delay(300);
                bool alarmsOk = await SendS5F3(true);
                if (!alarmsOk) OnLogReceived(LogLevel.Warn, "[INIT] S5F3 (Enable Alarms) failed or rejected. Continuing sequence...");

                // Step 5: S2F23 (Trace Initialize TOTS=0)
                await Task.Delay(300);
                bool traceOk = await SendS2F23Internal(1, 0, 0, new List<string>());
                if (traceOk) OnLogReceived(LogLevel.Info, "[INIT] S2F23 Trace initialization cleanup complete.");
                else OnLogReceived(LogLevel.Warn, "[INIT] S2F23 Trace initialization cleanup failed. Continuing...");

                OnLogReceived(LogLevel.Info, "[INIT] SECS/GEM Initialization Sequence completed.");
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[INIT] Error in Initialization Sequence: {ex.Message}");
            }
        }

        public async Task<List<string>> QueryEquipmentStatusAsync(List<uint> vids)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return new List<string>();
            try
            {
                OnLogReceived(LogLevel.Info, $"[S1F3] Requesting Status for {vids.Count} VIDs...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S1F3(vids)))
                {
                    if (reply.S == 1 && reply.F == 4)
                    {
                        var values = reply.ParseS1F4();
                        OnLogReceived(LogLevel.Info, $"[S1F4] Received {values.Count} Status values from equipment.");
                        return values;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S1F3] Selected Status Request failed: {ex.Message}");
            }
            return new List<string>();
        }

        public async Task<bool> SendStartCommandAsync() => await SendS2F41("START");
        public async Task<bool> SendStopCommandAsync() => await SendS2F41("STOP");
        public async Task<bool> SendPauseCommandAsync() => await SendS2F41("PAUSE");
        public async Task<bool> SendResumeCommandAsync() => await SendS2F41("RESUME");

        public async Task<bool> SendNewEquipmentConstantsAsync(Dictionary<uint, string> ecidValues)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F15] Sending New Equipment Constants ({ecidValues.Count} IDs)...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F15(ecidValues)))
                {
                    if (reply.S == 2 && reply.F == 16 && reply.SecsItem != null)
                    {
                        byte eac = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F16] New Equipment Constant Ack: {eac}");
                        return eac == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F15] New Equipment Constant Send failed: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendTerminalMessageAsync(string text, byte tid = 0)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S10F3] Sending Terminal Message (TID:{tid}): {text}");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S10F3(text, tid)))
                {
                    if (reply.S == 10 && reply.F == 4 && reply.SecsItem != null)
                    {
                        byte ackc10 = reply.SecsItem.FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S10F4] Terminal Display Ack: {ackc10}");
                        return ackc10 == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S10F3] Terminal Message Send failed: {ex.Message}");
            }
            return false;
        }
        
        public override void Stop() 
        { 
            _retryTimer?.Stop(); 
            _retryTimer?.Dispose();
            _aliveCheckTimer?.Stop();
            _aliveCheckTimer?.Dispose();
            _linkedCts?.Cancel(); 
        }

        private class SecsLogger : ISecsGemLogger {
            private readonly SecsEquipment _eq; public SecsLogger(SecsEquipment eq) => _eq = eq;
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
