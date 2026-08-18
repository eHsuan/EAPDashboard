using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private CancellationTokenSource? _linkedCts;
        private SecsClient? _secsClient;
        private SecsDictionary? _dictionary;

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
        public string DictionaryPath { get; set; } = string.Empty;
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

                string fullDictPath = Path.IsPathRooted(DictionaryPath) ? DictionaryPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictionaryPath);
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

                // 訂閱事件
                _secsClient.ConnectionStatusChanged += (s, e) =>
                {
                    Connection = (ConnectionStatus)(int)e.NewStatus;
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
            try
            {
                var data = message.PrimaryMessage.ParseS6F11();
                string ceidStr = data.Ceid.ToString();
                string logicalEvent = _dictionary?.GetEventName(ceidStr) ?? ceidStr;
                OnLogReceived(LogLevel.Info, $"[EVENT] {logicalEvent} (CEID:{ceidStr})");

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

                await message.TryReplyAsync(SecsMessages.S6F12());
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

            var info = QueryBindingFromGateway(cid);
            string wo = info?.WorkOrder;
            List<string> panels = info?.Panels ?? new List<string>();

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
            await SendS10F3(message);
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
    }
}
