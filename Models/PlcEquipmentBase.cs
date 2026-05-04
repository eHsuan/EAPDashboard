using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.Core.Enums;
using Protocol.Core.Messages;

namespace EAPDashboard.Models
{
    public class PlcEquipmentBase : EquipmentBase
    {
        protected readonly IPlcDriver _driver;
        protected List<PlcEventConfig> _events = new List<PlcEventConfig>();
        private CancellationTokenSource _linkedCts;
        private DateTime _lastPlcHeartbeatTime = DateTime.MinValue;
        private bool _lastHeartbeatValue = false;
        private MqttBus _bus; // 保存 MQTT bus 用於發送指令

        public string Ip { get; set; }
        public int Port { get; set; }

        public PlcEquipmentBase(IPlcDriver driver)
        {
            _driver = driver;
        }

        public void SetEvents(List<PlcEventConfig> events) => _events = events;

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            _bus = bus;
            Connection = ConnectionStatus.Disconnected; 
            LoadAlarmMappings();
            
            _ = Task.Run(() => ConnectionLoop(_linkedCts.Token));
            _ = Task.Run(() => EventPollingLoop(_linkedCts.Token));
            _ = Task.Run(() => WatchdogLoop(_linkedCts.Token));
            _ = StartStatusBufferProcessor(_linkedCts.Token);
            _ = StartBindingRetryProcessor(_linkedCts.Token);
        }

        private async Task ConnectionLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (Connection != ConnectionStatus.Connected)
                {
                    Connection = ConnectionStatus.Connecting;
                    bool ok = await _driver.ConnectAsync(Ip, Port);
                    if (ok) 
                    {
                        _lastPlcHeartbeatTime = DateTime.Now;
                        Connection = ConnectionStatus.Connected;
                        OnLogReceived(LogLevel.Info, $"[PLC] {Id} Connected to {Ip}:{Port}");
                    }
                    else
                    {
                        Connection = ConnectionStatus.Disconnected;
                        OnLogReceived(LogLevel.Warn, $"[PLC] {Id} Connection failed to {Ip}:{Port}. Retrying...");
                    }
                }
                await Task.Delay(5000, ct);
            }
        }

        private async Task WatchdogLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (Connection == ConnectionStatus.Connected)
                {
                    try 
                    {
                        var hbEvt = _events.FirstOrDefault(e => e.EventName.ToUpper() == "HEARTBEAT" || e.EventName.ToUpper() == "WATCHDOG");
                        if (hbEvt != null)
                        {
                            bool currentBit = await _driver.ReadBitAsync(hbEvt.TriggerAddr, hbEvt.TrigBitOffset);
                            if (currentBit != _lastHeartbeatValue)
                            {
                                _lastHeartbeatValue = currentBit;
                                _lastPlcHeartbeatTime = DateTime.Now;
                                if (!string.IsNullOrEmpty(hbEvt.AckAddr))
                                {
                                    await _driver.WriteBitAsync(hbEvt.AckAddr, currentBit, hbEvt.AckBitOffset);
                                }
                            }
                        }

                        if (_lastPlcHeartbeatTime != DateTime.MinValue && (DateTime.Now - _lastPlcHeartbeatTime).TotalSeconds > 15)
                        {
                            OnLogReceived(LogLevel.Error, $"[PLC] {Id} Watchdog timeout, disconnecting...");
                            Connection = ConnectionStatus.Disconnected;
                            _driver.Disconnect();
                        }
                    }
                    catch { }
                }
                await Task.Delay(1000, ct);
            }
        }

        private async Task EventPollingLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (Connection == ConnectionStatus.Connected)
                {
                    foreach (var evt in _events)
                    {
                        if (evt.EventName.ToUpper() == "HEARTBEAT" || evt.EventName.ToUpper() == "WATCHDOG") continue;

                        try {
                            bool isTrig = await _driver.ReadBitAsync(evt.TriggerAddr, evt.TrigBitOffset);
                            if (isTrig)
                            {
                                OnLogReceived(LogLevel.Debug, $"[PLC] {Id} Event detected: {evt.EventName} (CMD: {evt.CmdType})");
                                foreach (var dp in evt.DataPoints.Where(x => x.AccessType == "READ"))
                                {
                                    if (dp.DataType == "ASCII") dp.Value = await _driver.ReadAsciiAsync(dp.Address, dp.Length);
                                    else dp.Value = (await _driver.ReadWordAsync(dp.Address)).ToString();
                                }

                                await ProcessPlcEventAsync(evt);

                                if (!string.IsNullOrEmpty(evt.AckAddr))
                                {
                                    await _driver.WriteBitAsync(evt.AckAddr, true, evt.AckBitOffset);
                                    if (await WaitForPlcAckAsync(evt.TriggerAddr, evt.TrigBitOffset, 10)) 
                                    {
                                        await _driver.WriteBitAsync(evt.AckAddr, false, evt.AckBitOffset);
                                    }
                                }
                            }
                        } catch (Exception ex) { OnLogReceived(LogLevel.Error, $"Polling Error ({evt.EventName}): {ex.Message}"); }
                    }
                }
                await Task.Delay(1000, ct);
            }
        }

        protected virtual async Task ProcessPlcEventAsync(PlcEventConfig evt)
        {
            string reportEqpId = evt.SubEqpID ?? this.Id;
            string getVal(string key, string def) => evt.DataPoints.FirstOrDefault(x => x.Key == key)?.Value ?? def;
            string res = "";
            int resultCode = 0;

            OnLogReceived(LogLevel.Info, $"[PLC_EVENT] {Id} Processing: {evt.EventName} via CMD: {evt.CmdType}");

            switch (evt.CmdType)
            {
                case CommandType.ALARM:
                    string alarmCode = getVal("AlarmCode", "UNKNOWN");
                    bool isSet = getVal("AlarmStatus", "1") == "1";
                    res = await ProcessAlarmAsync(alarmCode, isSet, reportEqpId);
                    break;

                case CommandType.EventReport:
                    string name = evt.EventName.ToUpper();
                    if (name == "USER_VERIFY")
                    {
                        string uvRes = await ProcessUserVerifyAsync(getVal("Barcode", ""), getVal("UserID", ""), reportEqpId);
                        string[] uvParts = uvRes.Split('|');
                        bool isUvPass = uvParts[0] == "PASS";
                        string mappedRtnCode = DatabaseHelper.GetMappedRtnCode("USER_VERIFY", uvParts[1]);
                        string mesUserId = uvParts.Length > 3 ? uvParts[3] : "";

                        var uvResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var uvCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        var uvUserDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnUserID" && x.AccessType == "WRITE");

                        if (uvResultDp != null) await _driver.WriteWordAsync(uvResultDp.Address, (ushort)(isUvPass ? 1 : 2));
                        if (uvCodeDp != null) 
                        {
                            if (ushort.TryParse(mappedRtnCode, out ushort uvCodeVal)) await _driver.WriteWordAsync(uvCodeDp.Address, uvCodeVal);
                            else await _driver.WriteAsciiAsync(uvCodeDp.Address, mappedRtnCode, uvCodeDp.Length);
                        }
                        if (uvUserDp != null) await _driver.WriteAsciiAsync(uvUserDp.Address, mesUserId, 3);
                        res = uvRes;
                    }
                    else if (name == "PANEL_IN") res = await ProcessPanelInAsync(getVal("PanelID", ""), "TODO_CASSETTE", reportEqpId);
                    else if (name == "PANEL_OUT") res = await ProcessPanelOutAsync(getVal("PanelID", ""), "TODO_CASSETTE", "PASS", reportEqpId);
                    break;

                case CommandType.TrackInReq:
                    string cid = getVal("CassetteID", "");
                    string uid = getVal("UserID", "");
                    // 1. 工單檢索
                    string wo = QueryWorkOrderFromDb(cid) ?? "";
                    // 2. 向 MES 請求 (即便 WO 為空也要呼叫以符合模擬器隨機邏輯)
                    string tir = await ProcessTrackInAsync(cid, wo, uid, reportEqpId);
                    
                    if (tir.StartsWith("PP-SELECT"))
                    {
                        string[] tp = tir.Split('|');
                        string finalRecipe = "";

                        // 3. 判定是否為模擬器
                        if (this.IsMesSimulator) {
                            finalRecipe = new Random().Next(1000, 9999).ToString();
                            OnLogReceived(LogLevel.Info, $"[SIM] MES Simulator detected. Random Recipe: {finalRecipe}");
                        } else {
                            finalRecipe = GetMesParamValue(tp[5], evt.RecipeParamKey) ?? tp[2];
                        }

                        // 4. 準備數據：寫入 PLC 記憶體
                        var rcpDp = evt.DataPoints.FirstOrDefault(x => x.Key == evt.RecipeDataKey && x.AccessType == "WRITE");
                        if (rcpDp != null) await _driver.WriteAsciiAsync(rcpDp.Address, finalRecipe, rcpDp.Length);

                        var resDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        if (resDp != null) await _driver.WriteWordAsync(resDp.Address, 1); 

                        // 5. 觸發動作：RemoteCMD PP-SELECT
                        await SendRemoteCommandAsync(RemoteCommandType.PP_SELECT, finalRecipe);

                        // 6. 等待 PLC 切換完成握手
                        if (await WaitForPlcAckAsync(evt.TriggerAddr, evt.TrigBitOffset, 20))
                        {
                            string fin = await ProcessTrackInFinalAsync(tp[1], uid, cid, reportEqpId);
                            if (fin.StartsWith("PASS"))
                            {
                                string[] fp = fin.Split('|');
                                List<string> panels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(fp[3]);
                                for (int i = 0; i < 10; i++)
                                {
                                    var pDp = evt.DataPoints.FirstOrDefault(x => x.Key == $"PanelList_{i + 1}" && x.AccessType == "WRITE");
                                    if (pDp != null) await _driver.WriteAsciiAsync(pDp.Address, i < panels.Count ? panels[i] : "", pDp.Length);
                                }
                                if (resDp != null) await _driver.WriteWordAsync(resDp.Address, 3); 
                            }
                            res = fin;
                        }
                    }
                    else res = tir;
                    break;

                case CommandType.TrackOutReq:
                    string toCid = getVal("CassetteID", "");
                    string toQty = getVal("Qty", "0");
                    string toUid = getVal("UserID", "");
                    string toWo = QueryWorkOrderFromDb(toCid) ?? "";
                    res = await ProcessTrackOutAsync(toWo, toQty, toUid, toCid, new List<string>(), reportEqpId);
                    break;

                case CommandType.ProcessDataReport:
                    try {
                        var kpiStartDp = evt.DataPoints.FirstOrDefault(x => x.Key == "KPI_Start" && x.AccessType == "READ");
                        if (kpiStartDp == null) break;
                        ushort[] rawKpiData = await _driver.ReadAsync(kpiStartDp.Address, 60);
                        var mappings = DatabaseHelper.GetKpiMappings(this.Id);
                        var paramList = new List<ParameterResultItem>();
                        for (int i = 0; i < 20; i++) {
                            var map = mappings.FirstOrDefault(m => m.ParaIndex == i + 1 && m.IsEnabled);
                            if (map != null) {
                                double val = (short)rawKpiData[i * 3] / Math.Pow(10, map.DecimalPlaces);
                                paramList.Add(new ParameterResultItem { Parameter = map.ParameterName, Value = val });
                            }
                        }
                        string kpiRes = await ProcessKeyParametersReportAsync(paramList, reportEqpId);
                        string[] kpiParts = kpiRes.Split('|');
                        var kpiResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var kpiCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (kpiResultDp != null) await _driver.WriteWordAsync(kpiResultDp.Address, (ushort)(kpiParts[0] == "PASS" ? 1 : 2));
                        if (kpiCodeDp != null) {
                            if (ushort.TryParse(kpiParts[1], out ushort kpiCodeVal)) await _driver.WriteWordAsync(kpiCodeDp.Address, kpiCodeVal);
                            else await _driver.WriteAsciiAsync(kpiCodeDp.Address, kpiParts[1], kpiCodeDp.Length);
                        }
                        res = kpiRes;
                    } catch { }
                    break;

                case CommandType.StatusChangeReport:
                    string rawStatus = getVal("NewStatus", "0");
                    State = MapPlcStatusToSystemState(rawStatus);
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = reportEqpId, StatusCode = rawStatus });
                    break;
            }

            var msgDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
            if (msgDp != null && !string.IsNullOrEmpty(res)) await _driver.WriteWordAsync(msgDp.Address, (ushort)resultCode);
            OnLogReceived(LogLevel.Info, $"[PLC_EVENT] {Id} Result: {res}");
        }

        private async Task SendRemoteCommandAsync(RemoteCommandType type, string recipeId)
        {
            if (_bus == null || !_bus.IsConnected) return;
            var cmd = new RemoteCMDMessage { CMD = CommandType.RemoteCMD, RemoteCMDType = type, RecipeID = recipeId, Machine = Id, Date = DateTime.Now.ToString("yyyyMMddHHmmss") };
            await _bus.PublishAsync($"{this.Id}/Command", cmd);
        }

        private EquipmentState MapPlcStatusToSystemState(string rawValue)
        {
            switch (rawValue) 
            { 
                case "1": return EquipmentState.Alarm;
                case "2":
                case "4":
                case "5":
                case "7":
                case "8":
                case "9":
                case "12":
                case "3": return EquipmentState.Idle;
                case "0":
                case "6": return EquipmentState.Running;
                case "11":
                case "10": return EquipmentState.Down; 
                default: return EquipmentState.Paused; 
            }
        }

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();
        private void HandleEqpStatusCall(EQPSTATUSRequest req) { Task.Run(() => { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); } catch { _statusBuffer.Enqueue(req); } }); }
        private async Task StartStatusBufferProcessor(CancellationToken ct) { while (!ct.IsCancellationRequested) { if (_statusBuffer.TryPeek(out var req)) { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); _statusBuffer.TryDequeue(out _); } catch { await Task.Delay(5000, ct); continue; } } await Task.Delay(1000, ct); } }
        protected async Task<bool> WaitForPlcAckAsync(string address, int bitOffset, int timeoutSec) { DateTime start = DateTime.Now; while ((DateTime.Now - start).TotalSeconds < timeoutSec) { if (!await _driver.ReadBitAsync(address, bitOffset)) return true; await Task.Delay(500); } return false; }
        public override async Task SendTimeCalibration() => await Task.CompletedTask;
        public override void Stop() { _linkedCts?.Cancel(); _driver.Disconnect(); }
    }
}
