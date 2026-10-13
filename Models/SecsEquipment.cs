using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Secs4Net;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Core.Messages;
using Protocol.Drivers.Secs;
using CyntecMqttLib.Interfaces;
using CyntecMqttLib.Models;
using EAPDashboard.Models.Secs;

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private CancellationTokenSource? _linkedCts;
        private SecsClient? _secsClient;
        private SecsDictionary? _dictionary;
        private PlatingMqttForwarder? _platingMqtt;

        private static readonly HashSet<string> Pl0084WarningList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "L0007","L0008","L0010","L0015","L0016","L0022","L0025","L0027","L0028","L0029",
            "L0030","L0031","L0032","L0034","L0038","L0043","L0045","L0054","L0055","L0056",
            "L0058","L0070","L0073","L0075","L0076","L0077","L0078","L0079","L0080","L0091",
            "L0093","L2001","L2002","L2003","L2005","L2006","L2007","L2009","L2010","L2011",
            "L2013","L2014","L2015","L2017","L2018","L2019","L2021","L2022","L2023","L2025",
            "L2026","L2027","L2029","L2030","L2031","L2033","L2034","L2035","L2037","L2038",
            "L2039","L2040","L2041","L2042","L2043","L2044","L2045","L2046","L2047","L2048",
            "L2049","L2050","L2051","L2052","L2053","L2054","L2055","L2056","L2057","L2062",
            "L2063","L2160","L2161","L2162","L2163","L2164","L2165","L2166","L2167","L2168",
            "L2169","L2170","L2171","L2172","L2173","L2174","L2347","L2348","L2349","L2350",
            "L2351","L2352","L2353","L2354","L2355","L2356","L2496","L2497","L2498","L2499",
            "L2500","L2501","L2502","L2503","L2504","L2505","L2512","L2513","L2514","L2515",
            "L2516","L2517","L2518","L2519","L2520","L2521"
        };

        private List<string> _activeTraceParameters = new List<string>();
        private readonly object _traceLock = new object();

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();

        private static readonly Dictionary<int, EquipmentState> SecsStatusMap = new Dictionary<int, EquipmentState>()
        {
           { 0, EquipmentState.Down }, { 1, EquipmentState.Idle }, { 2, EquipmentState.Idle },
           { 3, EquipmentState.Running }, { 4, EquipmentState.Paused }, { 5, EquipmentState.Down }, { 6, EquipmentState.Idle }
        };

        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public ushort DeviceId { get; set; } = 0;
        public string TimeFormat { get; set; } = "16";
        public bool IsActive { get; set; } = true;

        public SecsEquipment()
        {
            Protocol = "SECS";
            Connection = ConnectionStatus.Disconnected;
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus? bus = null, int heartbeatIntervalSeconds = 30)
        {
            try
            {
                OnLogReceived(LogLevel.Info, $"[START] SECS module adapter initialized for {Id}.");
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);

                string dictTarget = !string.IsNullOrEmpty(DictionaryPath) ? DictionaryPath : Path.Combine("Dictionaries", $"{Id}.json");
                string fullDictPath = Path.IsPathRooted(dictTarget) ? dictTarget : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dictTarget);
                if (File.Exists(fullDictPath)) _dictionary = SecsDictionary.Load(fullDictPath);

                LoadAlarmMappings();
                LoadCachesFromDisk();
                _ = StartStatusBufferProcessor(_linkedCts.Token);

                // 實例化 DLL 中的 SecsClient
                _secsClient = new SecsClient
                {
                    Ip = Ip,
                    Port = Port,
                    DeviceId = DeviceId,
                    IsActive = IsActive,
                    TimeFormat = TimeFormat,
                    Name = Name,
                    Id = Id
                };

                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase))
                {
                    _platingMqtt = new PlatingMqttForwarder();
                }

                // 訂閱事件
                _secsClient.ConnectionStatusChanged += (s, e) =>
                {
                    Connection = (ConnectionStatus)(int)e.NewStatus;
                    if (Connection == ConnectionStatus.Connected)
                    {
                        _ = Task.Run(async () => await InitializeTraceForEquipmentAsync());
                    }
                };

                _secsClient.PrimaryMessageReceived += async (s, e) =>
                {
                    await HandlePrimaryMessage(e.Wrapper);
                };

                _secsClient.LogOccurred += (s, e) =>
                {
                    OnLogReceived((LogLevel)(int)e.Level, e.Message);
                };

                // 啟動 DLL 的通訊元件
                _secsClient.Start(_linkedCts.Token);
                _ = StartPeriodicStatusReportLoop(_linkedCts.Token);
                StartHourlyAvailabilityTimer(_linkedCts.Token);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Fatal, $"[CRITICAL] SECS Adapter Crash: {ex.Message}");
            }
        }

        public override void Stop()
        {
            _linkedCts?.Cancel();
            _secsClient?.Stop();
            _secsClient?.Dispose();
            Connection = ConnectionStatus.Disconnected;
        }

        private async Task HandlePrimaryMessage(PrimaryMessageWrapper message)
        {
            var s = message.PrimaryMessage.S;
            var f = message.PrimaryMessage.F;
            OnLogReceived(LogLevel.Info, $"[RECV] S{s}F{f}");
            switch (s)
            {
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
            try
            {
                var data = message.PrimaryMessage.ParseS10F3();
                OnLogReceived(LogLevel.Info, $"[TERMINAL] Msg from Eqp (TID:{data.Tid}): {data.Text}");
                await message.TryReplyAsync(SecsMessages.S10F4());
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S10F3] Error processing Terminal Message: {ex.Message}");
            }
        }

        private async Task HandleS1F13(PrimaryMessageWrapper message)
        {
            await message.TryReplyAsync(SecsMessages.S1F14(0));
        }

        private async Task HandleS5F1(PrimaryMessageWrapper message)
        {
            try
            {
                var data = message.PrimaryMessage.ParseS5F1();
                string alidStr = data.Alid.ToString();

                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase))
                {
                    if (alidStr.Contains("#"))
                    {
                        await message.TryReplyAsync(SecsMessages.S5F2());
                        return;
                    }
                    if (Pl0084WarningList.Contains(alidStr))
                    {
                        OnLogReceived(LogLevel.Info, $"[PL0084] 異常代碼 {alidStr} 為 Warning，不上拋不記錄。");
                        await message.TryReplyAsync(SecsMessages.S5F2());
                        return;
                    }
                    string alarmMsg = data.Altx;
                    try
                    {
                        var cv = JsonConvert.DeserializeObject<ConverAlarmMsg>(data.Altx);
                        if (cv?.FileContent != null && cv.FileContent.Length > 0)
                        {
                            alarmMsg = Encoding.UTF8.GetString(cv.FileContent);
                        }
                    }
                    catch { }

                    if (data.IsSet)
                    {
                        RaiseAlarmLocalDb(alidStr, alarmMsg);
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "ALARM", ErrorCode = alidStr });
                        _ = _platingMqtt?.PublishAsync("Yellow/PL0084/SECS/ALARM/", alidStr);
                    }
                    else
                    {
                        ClearAlarmLocalDb(alidStr);
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = alidStr });
                    }
                    await message.TryReplyAsync(SecsMessages.S5F2());
                    return;
                }

                if (Id.Equals("DR0009", StringComparison.OrdinalIgnoreCase))
                {
                    if (alidStr == "0330-0000" || alidStr == "0311-0000" || string.IsNullOrEmpty(data.Altx))
                    {
                        OnLogReceived(LogLevel.Info, $"[DR0009] 代碼 {alidStr} 為程式正常結束訊號，非警報。");
                        await message.TryReplyAsync(SecsMessages.S5F2());
                        return;
                    }
                    if (data.IsSet)
                    {
                        RaiseAlarmLocalDb(alidStr, data.Altx);
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "ALARM", ErrorCode = alidStr });
                    }
                    else
                    {
                        ClearAlarmLocalDb(alidStr);
                        HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = alidStr });
                    }
                    await message.TryReplyAsync(SecsMessages.S5F2());
                    return;
                }

                string mc = TransformAlarmCode(alidStr);
                if (mc == alidStr || data.Alid == 0)
                {
                    mc = TransformAlarmCode(data.Altx);
                }

                if (data.IsSet)
                {
                    RaiseAlarmLocalDb(alidStr, data.Altx);
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = mc });
                }
                else
                {
                    ClearAlarmLocalDb(alidStr);
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = mc });
                }
                await message.TryReplyAsync(SecsMessages.S5F2());
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S5F1] Error processing Alarm: {ex.Message}");
            }
        }

        private async Task HandleS6F1(PrimaryMessageWrapper message)
        {
            try
            {
                var data = message.PrimaryMessage.ParseS6F1();
                OnLogReceived(LogLevel.Debug, $"[RECV] S6F1 Trace Data (TRID:{data.Trid}, SMPLN:{data.Smpln})");

                await message.TryReplyAsync(SecsMessages.S6F2());

                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase))
                {
                    await HandlePL0084TraceData(data);
                    return;
                }
                if (Id.Equals("DR0009", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleDR0009TraceData(data);
                    return;
                }

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

                        parameterResults.Add(new ParameterResultItem
                        {
                            Parameter = paramName,
                            Value = valStr
                        });
                    }
                }

                payload["Parameters"] = traceParams;

                // 儲存生產資料 (寫入本地 PostgreSQL)
                await SaveProductionDataAsync("TraceData", "", payload, saveToDb: true);

                // 報工給 MES
                if (parameterResults.Count > 0)
                {
                    await ProcessKeyParametersReportAsync(parameterResults);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S6F1] Error processing Trace Data: {ex.Message}");
            }
        }

        private async Task HandleS6F11(PrimaryMessageWrapper message)
        {
            try
            {
                var data = message.PrimaryMessage.ParseS6F11();
                string ceidStr = data.Ceid.ToString();
                string logicalEvent = _dictionary?.GetEventName(ceidStr) ?? ceidStr;
                OnLogReceived(LogLevel.Info, $"[EVENT] {logicalEvent} (CEID:{ceidStr})");

                await message.TryReplyAsync(SecsMessages.S6F12());

                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase))
                {
                    if (ceidStr == "10002")
                    {
                        string status = ExtractVidValue(data.Reports, "1021") ?? ExtractVidValue(data.Reports, "Status") ?? "RUN";
                        State = ParseStateString(status);
                        _ = _platingMqtt?.PublishAsync("Yellow/PL0084/SECS/Status/", status);
                        if (!status.Equals("ALARM", StringComparison.OrdinalIgnoreCase))
                        {
                            HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = status });
                        }
                        await SaveProductionDataAsync("StatusChange", "", new { Status = status });
                    }
                    return;
                }

                if (Id.Equals("DR0009", StringComparison.OrdinalIgnoreCase))
                {
                    if (ceidStr == "10002")
                    {
                        string status = ExtractVidValue(data.Reports, "1104") ?? ExtractVidValue(data.Reports, "Status") ?? "RUN";
                        State = ParseStateString(status);
                        if (!status.Equals("ALARM", StringComparison.OrdinalIgnoreCase))
                        {
                            HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = status });
                        }
                        await SaveProductionDataAsync("StatusChange", "", new { Status = status });
                    }
                    else if (ceidStr == "10003")
                    {
                        await HandleDR0009EdcData(data.Reports);
                    }
                    return;
                }

                string statusVid = GetVidByLogicalName("Status") ?? GetVidByLogicalName("NewStatus") ?? "102";
                string statusCode = ExtractVidValue(data.Reports, statusVid);

                if (!string.IsNullOrEmpty(statusCode))
                {
                    string mappedStatusStr = _dictionary?.MapStatus(statusCode) ?? statusCode;
                    if (State != EquipmentState.Alarm)
                    {
                        State = ParseStateString(mappedStatusStr);
                    }
                }

                string upperEvent = logicalEvent.ToUpper();
                if (upperEvent == "TRACK_IN" || upperEvent == "TRACKIN")
                {
                    await HandleS6F11TrackIn(data.Reports);
                }
                else if (upperEvent == "TRACK_OUT" || upperEvent == "TRACKOUT")
                {
                    await HandleS6F11TrackOut(data.Reports);
                }
                else if (upperEvent == "PROCESS_DATA" || upperEvent == "KEY_PARAMETERS" || upperEvent == "PROCESSDATA" || upperEvent == "KEYPARAMETERS")
                {
                    await HandleS6F11ProcessData(data.Reports, ceidStr, logicalEvent);
                }
                else if (upperEvent == "STATUS_CHANGE" || upperEvent == "STATUSCHANGE")
                {
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = statusCode ?? "3" });
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[ERROR] S6F11: {ex.Message}");
            }
        }

        private async Task HandleS6F11TrackIn(Item reports)
        {
            string cassetteVid = GetVidByLogicalName("CassetteID") ?? "300";
            string cid = ExtractVidValue(reports, cassetteVid);
            if (string.IsNullOrEmpty(cid))
            {
                cid = ExtractVidValue(reports, "300") ?? ExtractVidValue(reports, "320") ?? "UNKNOWN";
            }

            string opVid = GetVidByLogicalName("Operator") ?? GetVidByLogicalName("UserID") ?? GetVidByLogicalName("User_ID");
            string userId = (!string.IsNullOrEmpty(opVid) ? ExtractVidValue(reports, opVid) : null) ?? "UNKNOWN";

            string wo = ExtractVidValue(reports, "310") ?? ExtractVidValue(reports, "301") ?? "";
            List<string> panels = new List<string>();

            string res = await ProcessTrackInAsync(cid, wo, panels, userId);
            if (res.StartsWith("PP-SELECT"))
            {
                string[] p = res.Split('|');

                var parameters = new Dictionary<string, string>();
                parameters["PPID"] = p[2];

                bool cmdSuccess = await SendS2F41("PP-SELECT", parameters);
                if (cmdSuccess)
                {
                    string fin = await ProcessTrackInFinalAsync(p[1], userId, cid);
                    OnLogReceived(LogLevel.Info, $"[AUTO] 進站報工完成: {fin}");
                }
                else
                {
                    OnLogReceived(LogLevel.Error, $"[AUTO] PP-SELECT command rejected by equipment. TrackIn aborted.");
                }
            }
        }

        private async Task HandleS6F11TrackOut(Item reports)
        {
            string woVid = GetVidByLogicalName("WorkOrder") ?? GetVidByLogicalName("RecipeID") ?? "111";
            string rid = ExtractVidValue(reports, woVid);
            if (string.IsNullOrEmpty(rid))
            {
                rid = "UNKNOWN";
            }

            string opVid = GetVidByLogicalName("Operator") ?? GetVidByLogicalName("UserID") ?? GetVidByLogicalName("User_ID");
            string userId = (!string.IsNullOrEmpty(opVid) ? ExtractVidValue(reports, opVid) : null) ?? "UNKNOWN";

            string res = await ProcessTrackOutAsync(rid, "1", userId, "UNKNOWN", new List<string>());
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

                            parameterResults.Add(new ParameterResultItem
                            {
                                Parameter = varName,
                                Value = val
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

        public async Task<List<string>> SendS1F3(List<uint> vids)
        {
            if (_secsClient == null || Connection != ConnectionStatus.Connected) return new List<string>();
            try
            {
                OnLogReceived(LogLevel.Info, $"[S1F3] Requesting Status for {vids.Count} VIDs...");
                using (var reply = await _secsClient.SendAsync(SecsMessages.S1F3(vids)))
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

        public async Task<bool> SendS2F23(string trid, uint dsper, uint tots, List<string> svids, List<string> paramNames)
        {
            lock (_traceLock)
            {
                _activeTraceParameters = paramNames ?? new List<string>();
            }
            if (_secsClient == null) return false;
            return await _secsClient.SendS2F23Internal(trid, dsper, tots, svids);
        }

        public async Task<bool> SendS2F41(string rcmd, Dictionary<string, string> parameters = null)
        {
            if (_secsClient == null || Connection != ConnectionStatus.Connected) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F41] Sending Host Command: {rcmd}");
                using (var reply = await _secsClient.SendAsync(SecsMessages.S2F41(rcmd, parameters)))
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

        public async Task<bool> SendS2F41Start() => await SendS2F41("START");
        public async Task<bool> SendS2F41Stop() => await SendS2F41("STOP");
        public async Task<bool> SendS2F41Pause() => await SendS2F41("PAUSE");
        public async Task<bool> SendS2F41Resume() => await SendS2F41("RESUME");

        public async Task<bool> SendS2F15(Dictionary<uint, string> ecidValues)
        {
            if (_secsClient == null || Connection != ConnectionStatus.Connected) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S2F15] Sending New Equipment Constants ({ecidValues.Count} IDs)...");
                using (var reply = await _secsClient.SendAsync(SecsMessages.S2F15(ecidValues)))
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

        public async Task<bool> SendS10F3(string text, byte tid = 0)
        {
            if (_secsClient == null || Connection != ConnectionStatus.Connected) return false;
            try
            {
                OnLogReceived(LogLevel.Info, $"[S10F3] Sending Terminal Message (TID:{tid}): {text}");
                using (var reply = await _secsClient.SendAsync(SecsMessages.S10F3(text, tid)))
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

        public override async Task SendTimeCalibration()
        {
            if (_secsClient != null)
            {
                await _secsClient.SendS2F31();
            }
        }

        public override async Task SendTerminalDisplay(string message)
        {
            string safeMsg = SanitizeTerminalMessage(message);
            if (!string.IsNullOrEmpty(safeMsg))
            {
                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase) || Id.Equals("DR0009", StringComparison.OrdinalIgnoreCase))
                {
                    string binaryStr = FormatTerminalTextAsBinary(safeMsg);
                    await SendS10F3(binaryStr);
                }
                else
                {
                    await SendS10F3(safeMsg);
                }
            }
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
            if (root.Format != SecsFormat.List)
            {
                switch (root.Format)
                {
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
            foreach (var item in root.Items)
            {
                var f = ExtractVidValue(item, vid);
                if (!string.IsNullOrEmpty(f)) return f;
            }
            return null;
        }

        private void HandleEqpStatusCall(EQPSTATUSRequest req)
        {
            Task.Run(() =>
            {
                try { CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(req); }
                catch { _statusBuffer.Enqueue(req); }
            });
        }

        private async Task StartStatusBufferProcessor(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (!_statusBuffer.IsEmpty && _statusBuffer.TryPeek(out var req))
                {
                    try
                    {
                        await Task.Run(() => CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(req));
                        _statusBuffer.TryDequeue(out _);
                    }
                    catch
                    {
                        await Task.Delay(30000, ct);
                    }
                }
                await Task.Delay(5000, ct);
            }
        }

        private async Task InitializeTraceForEquipmentAsync()
        {
            try
            {
                await Task.Delay(1500);
                if (Id.Equals("PL0084", StringComparison.OrdinalIgnoreCase))
                {
                    var svids = new List<string> { "1021", "1022" };
                    var paramNames = new List<string> { "Status", "PlatingRawData" };
                    await SendS2F23("1", 5, 0, svids, paramNames);
                    OnLogReceived(LogLevel.Info, "[PL0084] S2F23 Trace Data Initialized (SVID: 1021, 1022)");
                }
                else if (Id.Equals("DR0009", StringComparison.OrdinalIgnoreCase))
                {
                    var svids = new List<string> { "1104", "1008", "1009", "1011", "1002" };
                    var paramNames = new List<string> { "Status", "ProgTimeMin", "ProgTimeSec", "RunCount", "WorkOrder" };
                    await SendS2F23("1", 5, 0, svids, paramNames);
                    OnLogReceived(LogLevel.Info, "[DR0009] S2F23 Trace Data Initialized (SVID: 1104, 1008, 1009, 1011, 1002)");
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[InitializeTrace] Error initializing trace for {Id}: {ex.Message}");
            }
        }

        private async Task HandlePL0084TraceData(S6F1Data data)
        {
            try
            {
                var values = new List<string>();
                if (data.Values != null)
                {
                    ExtractAllValues(data.Values, values);
                }

                string status = values.Count > 0 ? values[0] : "RUN";
                State = ParseStateString(status);

                string rawJson = values.Count > 1 ? values[1] : string.Empty;
                string decodedJson = rawJson;
                try
                {
                    if (!string.IsNullOrEmpty(rawJson))
                    {
                        var payload = JsonConvert.DeserializeObject<PlatingRawPayload>(rawJson.Replace("\uFEFF", ""));
                        if (!string.IsNullOrEmpty(payload?.FileContent))
                        {
                            byte[] bytes = Convert.FromBase64String(payload.FileContent);
                            decodedJson = Encoding.UTF8.GetString(bytes);
                        }
                    }
                }
                catch
                {
                    decodedJson = rawJson;
                }

                // 發送 MQTT (背景異步非阻塞)
                _ = _platingMqtt?.PublishAsync("Yellow/PL0084/SECS/RAWDATA/", decodedJson);
                _ = _platingMqtt?.PublishAsync("Yellow/PL0084/SECS/Status/", status);

                // 解析電鍍槽參數
                var parameterResults = new List<ParameterResultItem>();
                var extractedParams = new Dictionary<string, object>();
                try
                {
                    if (!string.IsNullOrEmpty(decodedJson))
                    {
                        var lgm = JsonConvert.DeserializeObject<LongmenJson_Data>(decodedJson);
                        if (lgm?.Rtn_Value != null)
                        {
                            foreach (var val in lgm.Rtn_Value)
                            {
                                if (val.Col1 == "005#")
                                {
                                    string temp = val.Col8?.Trim().Replace("℃", string.Empty).Trim() ?? "";
                                    parameterResults.Add(new ParameterResultItem { Parameter = "熱水洗#5-溫度", Value = temp });
                                    extractedParams["熱水洗#5-溫度"] = temp;
                                }
                                else if (val.Col1 == "016#")
                                {
                                    string temp = val.Col8?.Trim().Replace("℃", string.Empty).Trim() ?? "";
                                    parameterResults.Add(new ParameterResultItem { Parameter = "酸洗#16-溫度", Value = temp });
                                    extractedParams["酸洗#16-溫度"] = temp;
                                }
                                else if (val.Col1 == "019#")
                                {
                                    string temp = val.Col8?.Trim().Replace("℃", string.Empty).Trim() ?? "";
                                    parameterResults.Add(new ParameterResultItem { Parameter = "熱水洗#19-溫度", Value = temp });
                                    extractedParams["熱水洗#19-溫度"] = temp;
                                }
                                else if (val.Col1 == "020#")
                                {
                                    string temp = val.Col8?.Trim().Replace("℃", string.Empty).Trim() ?? "";
                                    parameterResults.Add(new ParameterResultItem { Parameter = "酸脫#20-溫度", Value = temp });
                                    extractedParams["酸脫#20-溫度"] = temp;
                                }
                                else if (val.Col1 == "009#" || val.Col1 == "010#" || val.Col1 == "011#" || val.Col1 == "012#" || val.Col1 == "013#")
                                {
                                    string slotNum = val.Col1.TrimStart('0').TrimEnd('#');
                                    string col15 = val.Col15?.Replace("A", "").Replace("V", "").Trim() ?? "";
                                    string col17 = val.Col17?.Replace("A", "").Replace("V", "").Trim() ?? "";
                                    string currentIa = col15.Split('/').ElementAtOrDefault(0)?.Trim() ?? "00.0";
                                    string currentVa = col15.Split('/').ElementAtOrDefault(1)?.Trim() ?? "0";
                                    string currentIb = col17.Split('/').ElementAtOrDefault(0)?.Trim() ?? "00.0";
                                    string currentVb = col17.Split('/').ElementAtOrDefault(1)?.Trim() ?? "0";
                                    string temp = val.Col8?.Trim().Replace("℃", "").Trim() ?? "";

                                    parameterResults.Add(new ParameterResultItem { Parameter = $"鍍銅#{slotNum}-A面-電流", Value = currentIa });
                                    parameterResults.Add(new ParameterResultItem { Parameter = $"鍍銅#{slotNum}-B面-電流", Value = currentIb });
                                    parameterResults.Add(new ParameterResultItem { Parameter = $"鍍銅#{slotNum}-A面-電壓", Value = currentVa });
                                    parameterResults.Add(new ParameterResultItem { Parameter = $"鍍銅#{slotNum}-B面-電壓", Value = currentVb });
                                    parameterResults.Add(new ParameterResultItem { Parameter = $"鍍銅#{slotNum}-溫度", Value = temp });

                                    extractedParams[$"鍍銅#{slotNum}-A面-電流"] = currentIa;
                                    extractedParams[$"鍍銅#{slotNum}-B面-電流"] = currentIb;
                                    extractedParams[$"鍍銅#{slotNum}-A面-電壓"] = currentVa;
                                    extractedParams[$"鍍銅#{slotNum}-B面-電壓"] = currentVb;
                                    extractedParams[$"鍍銅#{slotNum}-溫度"] = temp;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLogReceived(LogLevel.Warn, $"[PL0084] Error parsing LongmenJson: {ex.Message}");
                }

                // 儲存至本地 PostgreSQL
                await SaveProductionDataAsync("PlatingTrace", "", new { Status = status, Parameters = extractedParams }, saveToDb: true);

                // 上報 MES 機台狀態
                if (!status.Equals("ALARM", StringComparison.OrdinalIgnoreCase))
                {
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = status, UserID = "System" });
                }

                // 報工給 MES
                if (parameterResults.Count > 0)
                {
                    await ProcessKeyParametersReportAsync(parameterResults);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[PL0084] Error in HandlePL0084TraceData: {ex.Message}");
            }
        }

        private async Task HandleDR0009TraceData(S6F1Data data)
        {
            try
            {
                var values = new List<string>();
                if (data.Values != null)
                {
                    ExtractAllValues(data.Values, values);
                }

                string status = values.Count > 0 ? values[0] : "RUN";
                State = ParseStateString(status);

                string minStr = values.Count > 1 ? values[1] : "0";
                string secStr = values.Count > 2 ? values[2] : "0";
                string countStr = values.Count > 3 ? values[3] : "0";
                string wono = values.Count > 4 ? values[4] : "";

                int min = int.TryParse(minStr, out var m) ? m : 0;
                int sec = int.TryParse(secStr, out var s) ? s : 0;
                int totalSec = (min * 60) + sec;

                var tracePayload = new Dictionary<string, object>
                {
                    ["Status"] = status,
                    ["CycleTime"] = totalSec,
                    ["RunCount"] = countStr,
                    ["WorkOrder"] = wono
                };

                // 儲存至本地 PostgreSQL
                await SaveProductionDataAsync("LaserTrace", wono, tracePayload, saveToDb: true);

                // 上報 MES
                HandleEqpStatusCall(new EQPSTATUSRequest
                {
                    EqpNo = Id,
                    StatusCode = status,
                    UserID = "System",
                    CycleTime = totalSec.ToString(),
                    OutPutQty = countStr,
                    TotalOutPutQty = countStr,
                    WONO = wono
                });
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[DR0009] Error in HandleDR0009TraceData: {ex.Message}");
            }
        }

        private async Task HandleDR0009EdcData(Item reports)
        {
            try
            {
                string wono = "";
                string user = "System";
                string laserJsonStr = "";

                var values = new List<string>();
                ExtractAllValues(reports, values);

                if (values.Count >= 3)
                {
                    wono = values[0];
                    user = values[1];
                    laserJsonStr = values[2];
                }
                else if (values.Count > 0)
                {
                    laserJsonStr = values.FirstOrDefault(v => v.Contains("UP_Aperture") || v.Contains("DOWN_Aperture")) ?? "";
                    wono = values.FirstOrDefault(v => !v.Contains("{") && v != user) ?? "";
                }

                if (string.IsNullOrEmpty(laserJsonStr))
                {
                    OnLogReceived(LogLevel.Warn, "[DR0009] EDC Data received but LaserJson string is empty.");
                    return;
                }

                var laserJson = JsonConvert.DeserializeObject<LaserJson>(laserJsonStr);
                if (laserJson == null) return;

                var edcList = new List<dtEDCRawDataItem>();

                if (laserJson.UP_Aperture != null)
                {
                    for (int i = 0; i < laserJson.UP_Aperture.Count; i++)
                    {
                        edcList.Add(new dtEDCRawDataItem
                        {
                            COMPONENTNO = "1",
                            FIELDCODE = "CHECK_UP_APERTURE",
                            VALUE = laserJson.UP_Aperture[i],
                            Memo = "EAP上傳",
                            SORTINDEX = (i + 1).ToString()
                        });
                    }
                }

                if (laserJson.DOWN_Aperture != null)
                {
                    for (int i = 0; i < laserJson.DOWN_Aperture.Count; i++)
                    {
                        edcList.Add(new dtEDCRawDataItem
                        {
                            COMPONENTNO = "1",
                            FIELDCODE = "CHECK_DOWN_APERTURE",
                            VALUE = laserJson.DOWN_Aperture[i],
                            Memo = "EAP上傳",
                            SORTINDEX = (i + 1).ToString()
                        });
                    }
                }

                var edcReq = new EDCDATAADDRequest
                {
                    EqpNo = Id,
                    WONO = wono,
                    UserID = string.IsNullOrEmpty(user) ? "System" : user,
                    IsLast = "Y",
                    ISCOMPONENTMODE = "N",
                    dtEDCRawData = JsonConvert.SerializeObject(edcList)
                };
                if (edcReq.dtEDCRawDatalist != null && edcList.Count > 0)
                {
                    edcReq.dtEDCRawDatalist.AddRange(edcList);
                }

                // 儲存至本地 PostgreSQL
                await SaveProductionDataAsync("LaserEDC", wono, edcList, saveToDb: true);

                // 上報 MES WebService (EDCDATAADD)
                await Task.Run(() =>
                {
                    try
                    {
                        var reply = CallMesWithLog<EDCDATAADDRequest, EDCDATAADDReply>(edcReq);
                        OnLogReceived(LogLevel.Info, $"[DR0009] Upload EDC Result: {reply?.Result}, Msg: {reply?.Message}");
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Error, $"[DR0009] Upload EDC Error: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[DR0009] Error handling EDC data: {ex.Message}");
            }
        }

        public async Task<bool> SendS2F41PlatingRecipe(string rcmd, string recipeId, string wono, string wonoqty)
        {
            if (_secsClient == null || Connection != ConnectionStatus.Connected) return false;
            try
            {
                string cpval1 = $"{wono}-1-{wonoqty}";
                OnLogReceived(LogLevel.Info, $"[S2F41] Sending Plating Recipe Command: {rcmd}, Recipe: {recipeId}, CPVAL1: {cpval1}, CPVAL2: {wonoqty}");

                var msg = new SecsMessage(2, 41)
                {
                    Name = "Host Command Send",
                    SecsItem = Item.L(
                        Item.A(rcmd),
                        Item.L(
                            Item.L(
                                Item.A(recipeId),
                                Item.A(cpval1),
                                Item.A(wonoqty)
                            )
                        )
                    )
                };

                using (var reply = await _secsClient.SendAsync(msg))
                {
                    if (reply.S == 2 && reply.F == 42 && reply.SecsItem != null)
                    {
                        byte hcack = reply.SecsItem.Items[0].FirstValue<byte>();
                        OnLogReceived(LogLevel.Info, $"[S2F42] Plating Recipe Ack: {hcack}");
                        return hcack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[S2F41] Send Plating Recipe Error: {ex.Message}");
            }
            return false;
        }

        private static string FormatTerminalTextAsBinary(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            return string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }

        private void ExtractAllValues(Item item, List<string> list)
        {
            if (item == null) return;
            if (item.Format != SecsFormat.List)
            {
                string val;
                switch (item.Format)
                {
                    case SecsFormat.ASCII: val = item.GetString(); break;
                    case SecsFormat.U1: case SecsFormat.Binary: val = item.FirstValue<byte>().ToString(); break;
                    case SecsFormat.U2: val = item.FirstValue<ushort>().ToString(); break;
                    case SecsFormat.U4: val = item.FirstValue<uint>().ToString(); break;
                    case SecsFormat.I4: val = item.FirstValue<int>().ToString(); break;
                    case SecsFormat.Boolean: val = item.FirstValue<bool>().ToString(); break;
                    default: val = item.GetString(); break;
                }
                list.Add(val);
            }
            else
            {
                foreach (var sub in item.Items)
                {
                    ExtractAllValues(sub, list);
                }
            }
        }
    }
}
