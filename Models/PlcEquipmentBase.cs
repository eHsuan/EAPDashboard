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
using Protocol.Drivers.Mitsubishi;

namespace EAPDashboard.Models
{
    public class PlcEquipmentBase : EquipmentBase
    {
        protected readonly IPlcDriver _driver;
        protected List<PlcEventConfig> _events = new List<PlcEventConfig>();
        protected List<PlcRecipeConfig> _recipeConfigs = new List<PlcRecipeConfig>();
        private CancellationTokenSource _linkedCts;
        private DateTime _lastPlcHeartbeatTime = DateTime.MinValue;
        private bool _lastHeartbeatValue = false;
        private static readonly Random _rnd = new Random(); // 類別層級確保隨機性

        public string Ip { get; set; }
        public int Port { get; set; }

        public PlcEquipmentBase(IPlcDriver driver)
        {
            _driver = driver;
        }

        public void SetEvents(List<PlcEventConfig> events) => _events = events;

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            Connection = ConnectionStatus.Disconnected; 
            LoadAlarmMappings();
            _recipeConfigs = DatabaseHelper.GetRecipeConfigs(this.Id);
            OnLogReceived(LogLevel.Debug, $"[PLC] Loaded {_recipeConfigs.Count} recipe mapping(s) for {Id}");
            
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
                if (Connection == ConnectionStatus.Disconnected)
                {
                    Connection = ConnectionStatus.Connecting;
                    OnLogReceived(LogLevel.Debug, $"[PLC] {Id} Attempting to connect to {Ip}:{Port}...");
                    bool ok = await _driver.ConnectAsync(Ip, Port);
                    if (ok) 
                    {
                        _lastPlcHeartbeatTime = DateTime.Now;
                        _lastHeartbeatValue = await _driver.ReadBitAsync(_events.FirstOrDefault(e => e.EventName.ToUpper() == "HEARTBEAT")?.TriggerAddr ?? "", 0);
                        OnLogReceived(LogLevel.Debug, $"[PLC] {Id} TCP Socket established. Waiting for first heartbeat signal...");
                    }
                    else
                    {
                        Connection = ConnectionStatus.Disconnected;
                        OnLogReceived(LogLevel.Warn, $"[PLC] {Id} Connection failed to {Ip}:{Port}. Retrying in 5s...");
                    }
                }
                await Task.Delay(5000, ct);
            }
        }

        private async Task WatchdogLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if ((Connection == ConnectionStatus.Connected || Connection == ConnectionStatus.Connecting) && _driver.IsConnected)
                {
                    try 
                    {
                        var hbEvt = _events.FirstOrDefault(e => e.EventName.ToUpper() == "HEARTBEAT" || e.EventName.ToUpper() == "WATCHDOG");
                        if (hbEvt != null)
                        {
                            bool currentBit = await _driver.ReadBitAsync(hbEvt.TriggerAddr, hbEvt.TrigBitOffset);
                            if (currentBit != _lastHeartbeatValue)
                            {
                                if (Connection == ConnectionStatus.Connecting)
                                {
                                    Connection = ConnectionStatus.Connected;
                                    OnLogReceived(LogLevel.Info, string.Format("[PLC] {0} Heartbeat detected. System is now Online.", Id));
                                }
                                _lastHeartbeatValue = currentBit;
                                _lastPlcHeartbeatTime = DateTime.Now;
                                if (!string.IsNullOrEmpty(hbEvt.AckAddr))
                                {
                                    await _driver.WriteBitAsync(hbEvt.AckAddr, currentBit, hbEvt.AckBitOffset);
                                }
                            }
                        }

                        // Timeout Check (針對 Connecting 或 Connected)
                        double timeoutLimit = 15; 
                        if (_lastPlcHeartbeatTime != DateTime.MinValue && (DateTime.Now - _lastPlcHeartbeatTime).TotalSeconds > timeoutLimit)
                        {
                            string reason = (Connection == ConnectionStatus.Connecting) ? "First heartbeat signal not received" : "Watchdog timeout";
                            OnLogReceived(LogLevel.Error, string.Format("[PLC] {0} {1}, disconnecting...", Id, reason));
                            Connection = ConnectionStatus.Disconnected;
                            _driver.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("[Watchdog] Error: {0}", ex.Message));
                    }
                }
                await Task.Delay(1000, ct);
            }
        }

        private async Task EventPollingLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (Connection == ConnectionStatus.Connected && !IsStopping)
                {
                    foreach (var evt in _events)
                    {
                        if (evt.EventName.ToUpper() == "HEARTBEAT" || evt.EventName.ToUpper() == "WATCHDOG") continue;

                        try {
                            bool isTrig = await _driver.ReadBitAsync(evt.TriggerAddr, evt.TrigBitOffset);
                            if (isTrig)
                            {
                                IncrementTask();
                                _ = Task.Run(async () => {
                                    try {
                                        OnLogReceived(LogLevel.Debug, evt.EventName, string.Format("[PLC] {0} Event detected: {1} (CMD: {2})", Id, evt.EventName, evt.CmdType));
                                        foreach (var dp in evt.DataPoints.Where(x => x.AccessType == "READ"))
                                        {
                                            if (dp.DataType == "ASCII") dp.Value = await _driver.ReadAsciiAsync(dp.Address, dp.Length);
                                            else dp.Value = (await _driver.ReadWordAsync(dp.Address)).ToString();
                                        }

                                        await ProcessPlcEventAsync(evt);

                                        if (!string.IsNullOrEmpty(evt.AckAddr))
                                        {
                                            await _driver.WriteBitAsync(evt.AckAddr, true, evt.AckBitOffset);
                                            // 等待 PLC 看到 Ack 後將 Trigger OFF
                                            if (await WaitForPlcOffAsync(evt.TriggerAddr, evt.TrigBitOffset, 10)) 
                                            {
                                                await _driver.WriteBitAsync(evt.AckAddr, false, evt.AckBitOffset);
                                            }
                                            else
                                            {
                                                OnLogReceived(LogLevel.Error, evt.EventName, string.Format("[PLC] Handshake Error: PLC Trigger {0} remained ON too long. Forcing Ack OFF.", evt.TriggerAddr));
                                                await _driver.WriteBitAsync(evt.AckAddr, false, evt.AckBitOffset);
                                            }
                                        }
                                    }
                                    finally { DecrementTask(); }
                                });
                            }
                        } catch (Exception ex) { OnLogReceived(LogLevel.Error, evt.EventName, string.Format("Polling Error ({0}): {1}", evt.EventName, ex.Message)); }
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

            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC_EVENT] {Id} Processing: {evt.EventName} via CMD: {evt.CmdType}");

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
                        if (uvResultDp != null) 
                        {
                            ushort rVal = (ushort)(isUvPass ? 1 : 2);
                            await _driver.WriteWordAsync(uvResultDp.Address, rVal);
                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult {rVal} to {uvResultDp.Address}.");
                        }
                        else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnResult PLC Datapoint Not Found.");

                        var uvCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (uvCodeDp != null) 
                        {
                            if (ushort.TryParse(mappedRtnCode, out ushort uvCodeVal)) 
                            {
                                await _driver.WriteWordAsync(uvCodeDp.Address, uvCodeVal);
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnCode {uvCodeVal} to {uvCodeDp.Address}.");
                            }
                            else 
                            {
                                await _driver.WriteAsciiAsync(uvCodeDp.Address, mappedRtnCode, uvCodeDp.Length);
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnCode {mappedRtnCode} to {uvCodeDp.Address}.");
                            }
                        }
                        else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnCode PLC Datapoint Not Found.");

                        var uvUserDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnUserID" && x.AccessType == "WRITE");
                        if (uvUserDp != null) 
                        {
                            await _driver.WriteAsciiAsync(uvUserDp.Address, mesUserId, 3);
                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnUserID {mesUserId} to {uvUserDp.Address}.");
                        }
                        else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnUserID PLC Datapoint Not Found.");
                        
                        res = uvRes;
                    }
                    else if (name == "PANEL_IN")
                    {
                        string pCid = getVal("CassetteID", "UNKNOWN");
                        res = await ProcessPanelInAsync(getVal("PanelID", ""), pCid, reportEqpId);
                        if (!res.StartsWith("PASS")) resultCode = 501;
                    }
                    else if (name == "PANEL_OUT")
                    {
                        string pCid = getVal("CassetteID", "UNKNOWN");
                        res = await ProcessPanelOutAsync(getVal("PanelID", ""), pCid, "PASS", reportEqpId);
                        if (!res.StartsWith("PASS")) resultCode = 601;
                    }
                    break;

                case CommandType.TrackInReq:
                    string cid = getVal("CassetteID", "");
                    string uid = getVal("UserID", "");
                    var info = QueryBindingFromGateway(cid);
                    string wo = info?.WorkOrder;
                    List<string> boundPanels = info?.Panels ?? new List<string>();
                    
                    if (string.IsNullOrEmpty(wo))
                    {
                        res = "FAIL|W001|Cassette Not Bound to WO";
                        resultCode = 101; 
                    }
                    else
                    {
                        string tir = await ProcessTrackInAsync(cid, wo, boundPanels, uid, reportEqpId);
                        res = tir;
                        if (tir.StartsWith("PP-SELECT"))
                        {
                            string[] tp = tir.Split('|');
                            string firstRecipeId = "";

                            foreach (var rConfig in _recipeConfigs)
                            {
                                string recipeId = "";
                                if (this.IsMesSimulator) {
                                    recipeId = _rnd.Next(1000, 9999).ToString();
                                    OnLogReceived(LogLevel.Debug, evt.EventName, $"[SIMULATOR] Random RecipeID generated: {recipeId} for {rConfig.Description}");
                                } else {
                                    recipeId = GetMesParamValue(tp[5], rConfig.RecipeParamKey) ?? "UNKNOWN";
                                }

                                if (string.IsNullOrEmpty(firstRecipeId)) firstRecipeId = recipeId;

                                var rcpDp = evt.DataPoints.FirstOrDefault(x => x.Key == rConfig.RecipeDataKey && x.AccessType == "WRITE");
                                if (rcpDp != null)
                                {
                                    if (ushort.TryParse(recipeId, out ushort rVal))
                                    {
                                        await _driver.WriteWordAsync(rcpDp.Address, rVal);
                                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write Recipe {rConfig.Description} Value {recipeId} to {rcpDp.Address}.");
                                    }
                                    else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] RecipeID '{recipeId}' is not numeric, skipping WriteWord for {rcpDp.Address}");
                                }
                                else
                                {
                                    OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} Recipe {rConfig.Description} PLC Datapoint Not Found.");
                                }
                            }

                            var resDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                            if (resDp != null)
                            {
                                await _driver.WriteWordAsync(resDp.Address, 1);
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult 1 (PP-SELECT Start) to {resDp.Address}.");
                            }
                            else
                            {
                                OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnResult PLC Datapoint Not Found.");
                            }

                            // --- PP-SELECT Handshake Start (EAP is Trigger, EQ is Ack) ---
                            var ppEvt = _events.FirstOrDefault(e => e.EventName.ToUpper() == "PP-SELECT" || e.EventName.ToUpper() == "PP_SELECT");
                            var handshakeTarget = ppEvt ?? evt; 

                            if (ppEvt == null) OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} Independent PP-SELECT config not found, falling back to TrackIn bits (Roles swapped).");

                            // EAP ON Trigger (通知機台執行命令)
                            if (!string.IsNullOrEmpty(handshakeTarget.TriggerAddr))
                            {
                                await _driver.WriteBitAsync(handshakeTarget.TriggerAddr, true, handshakeTarget.TrigBitOffset);
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Turn On PP-SELECT Request Bit {handshakeTarget.TriggerAddr}.");
                            }

                            // 等待機台 ON Ack (等待命令執行完成)
                            if (await WaitForPlcAckAsync(handshakeTarget.AckAddr, handshakeTarget.AckBitOffset, 20))
                            {
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Receive PP-SELECT Ack Bit {handshakeTarget.AckAddr}.{handshakeTarget.AckBitOffset}.");
                                // EAP OFF Trigger (完成交握)
                                if (!string.IsNullOrEmpty(handshakeTarget.TriggerAddr))
                                {
                                    await _driver.WriteBitAsync(handshakeTarget.TriggerAddr, false, handshakeTarget.TrigBitOffset);
                                    OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Turn Off PP-SELECT Request Bit {handshakeTarget.TriggerAddr}.");
                                }
                                
                                string fin = await ProcessTrackInFinalAsync(tp[1], uid, cid, reportEqpId);
                                res = fin;
                                if (fin.StartsWith("PASS"))
                                {
                                    string[] fp = fin.Split('|');
                                    List<string> panels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(tp[6]);
                                    for (int i = 0; i < 10; i++)
                                    {
                                        var pDp = evt.DataPoints.FirstOrDefault(x => x.Key == $"PanelList{i + 1}" && x.AccessType == "WRITE");
                                        if (pDp != null)
                                        {
                                            string pVal = i < panels.Count ? panels[i] : "";
                                            await _driver.WriteAsciiAsync(pDp.Address, pVal, pDp.Length);
                                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write PanelID {pVal} to {pDp.Address}.");
                                        }
                                        else
                                        {
                                            OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} PanelList{i + 1} PLC Datapoint Not Found.");
                                        }
                                            
                                    }

                                    var woDp = evt.DataPoints.FirstOrDefault(x => x.Key == "WorkOrder" && x.AccessType == "WRITE");
                                    if (woDp != null)
                                    {
                                        await _driver.WriteAsciiAsync(woDp.Address, tp[1], woDp.Length);
                                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write WorkOrder {tp[1]} to {woDp.Address}.");
                                    }
                                    else
                                    {
                                        OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} WorkOrder PLC Datapoint Not Found.");
                                    }

                                    var rtnCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                                    if (rtnCodeDp != null)
                                    {
                                        await _driver.WriteWordAsync(rtnCodeDp.Address, 0);
                                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnCode 0 to {rtnCodeDp.Address}.");
                                    }
                                    else
                                    {
                                        OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnCode PLC Datapoint Not Found.");
                                    }


                                    if (resDp != null)
                                    {
                                        await _driver.WriteWordAsync(resDp.Address, 3);
                                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult 3 (Final PASS) to {resDp.Address}.");
                                    }
                                    resultCode = 0;
                                }
                                else { resultCode = 202; }
                            }
                            else 
                            { 
                                res = "FAIL|T001|PLC Handshake Timeout"; 
                                resultCode = 102;
                                OnLogReceived(LogLevel.Error, evt.EventName, $"[PLC] {Id} PP-SELECT Ack Timeout {handshakeTarget.AckAddr}.{handshakeTarget.AckBitOffset}.");
                            }
                        }
                        else
                        {
                            resultCode = 201;
                        }
                    }

                    if (res.StartsWith("FAIL") || res.StartsWith("ERROR"))
                    {
                        var failResDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        if (failResDp != null) 
                        {
                            await _driver.WriteWordAsync(failResDp.Address, 2);
                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult 2 (Fail State) to {failResDp.Address}.");
                        }
                    }
                    break;

                case CommandType.TrackOutReq:
                    string toCid = getVal("CassetteID", "");
                    string toQty = getVal("Qty", "0");
                    string toUid = getVal("UserID", "");
                    
                    // 1. 讀取 EQ 報上來的 WorkOrder
                    string toWo = "";
                    var toWoDp = evt.DataPoints.FirstOrDefault(x => x.Key == "WorkOrder" && x.AccessType == "READ");
                    if (toWoDp != null)
                    {
                        toWo = await _driver.ReadAsciiAsync(toWoDp.Address, toWoDp.Length);
                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Read WorkOrder {toWo} from {toWoDp.Address}.");
                    }
                    else
                    {
                        OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} WorkOrder PLC Datapoint Not Found (READ).");
                        // Fallback to Gateway if PLC DP not defined
                        var toInfo = QueryBindingFromGateway(toCid);
                        toWo = toInfo?.WorkOrder ?? "";
                    }

                    // 2. 讀取 EQ 報上來的 PanelList 1~10
                    List<string> toPanels = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        var pDp = evt.DataPoints.FirstOrDefault(x => x.Key == $"PanelList{i + 1}" && x.AccessType == "READ");
                        if (pDp != null)
                        {
                            string pId = await _driver.ReadAsciiAsync(pDp.Address, pDp.Length);
                            if (!string.IsNullOrEmpty(pId)) toPanels.Add(pId);
                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Read PanelID {pId} from {pDp.Address}.");
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} PanelList{i + 1} PLC Datapoint Not Found (READ).");
                        }
                    }

                    // 3. 執行過站與資料庫綁定
                    res = await ProcessTrackOutAsync(toWo, toQty, toUid, toCid, toPanels, reportEqpId);
                    
                    if (res.StartsWith("PASS")) resultCode = 0; 
                    else resultCode = 301;

                    // 4. 回寫 RtnResult
                    var toResDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    if (toResDp != null) 
                    {
                        ushort rVal = (ushort)(res.StartsWith("PASS") ? 1 : 2);
                        await _driver.WriteWordAsync(toResDp.Address, rVal);
                        OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult {rVal} to {toResDp.Address}.");
                    }
                    else
                    {
                        OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnResult PLC Datapoint Not Found (WRITE).");
                    }
                    break;

                case CommandType.ProcessDataReport:
                    try {
                        var kpiStartDp = evt.DataPoints.FirstOrDefault(x => x.Key == "KPI_Start" && x.AccessType == "READ");
                        if (kpiStartDp != null) 
                        {
                            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Reading KPI Data starting from {kpiStartDp.Address}.");
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
                            res = kpiRes;
                            if (res.StartsWith("PASS")) resultCode = 0; else resultCode = 401;

                            var kpiResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                            if (kpiResultDp != null) 
                            {
                                ushort rVal = (ushort)(res.StartsWith("PASS") ? 1 : 2);
                                await _driver.WriteWordAsync(kpiResultDp.Address, rVal);
                                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write RtnResult {rVal} to {kpiResultDp.Address}.");
                            }
                            else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnResult PLC Datapoint Not Found.");
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} KPI_Start PLC Datapoint Not Found.");
                        }
                    } catch (Exception ex) { OnLogReceived(LogLevel.Error, evt.EventName, $"KPI Processing Error: {ex.Message}"); }
                    break;

                case CommandType.StatusChangeReport:
                    string rawStatus = getVal("NewStatus", "0");
                    State = MapPlcStatusToSystemState(rawStatus);
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = reportEqpId, StatusCode = rawStatus });
                    break;
            }

            var codeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
            if (codeDp != null) 
            {
                await _driver.WriteWordAsync(codeDp.Address, (ushort)resultCode);
                OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC] {Id} Write Final RtnCode {resultCode} to {codeDp.Address}.");
            }
            else OnLogReceived(LogLevel.Warn, evt.EventName, $"[PLC] {Id} RtnCode PLC Datapoint Not Found.");
            
            OnLogReceived(LogLevel.Debug, evt.EventName, $"[PLC_EVENT] {Id} Result: {res} (Code: {resultCode})");
        }

        private EquipmentState MapPlcStatusToSystemState(string rawValue)
        {
            switch (rawValue) 
            { 
                case "1": return EquipmentState.Alarm;
                case "2": case "4": case "5": case "7": case "8": case "9": case "12": case "3": return EquipmentState.Idle;
                case "0": case "6": return EquipmentState.Running;
                case "11": case "10": return EquipmentState.Down; 
                default: return EquipmentState.Paused; 
            }
        }

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();
        private void HandleEqpStatusCall(EQPSTATUSRequest req) 
        { 
            Task.Run(() => { 
                try 
                {
                    MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); 
                } 
                catch 
                { 
                    _statusBuffer.Enqueue(req); 
                } 
            });}
        private async Task StartStatusBufferProcessor(CancellationToken ct) 
        { 
            while (!ct.IsCancellationRequested) 
            { 
                if (_statusBuffer.TryPeek(out var req))
                { 
                    try 
                    { 
                        MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); _statusBuffer.TryDequeue(out _); 
                    } 
                    catch 
                    { 
                        await Task.Delay(5000, ct); continue; 
                    } 
                } await Task.Delay(1000, ct); 
            } 
        }
        protected async Task<bool> WaitForPlcAckAsync(string address, int bitOffset, int timeoutSec) 
        {
            DateTime start = DateTime.Now; 
            while ((DateTime.Now - start).TotalSeconds < timeoutSec)
            { 
                if (await _driver.ReadBitAsync(address, bitOffset)) return true; 
                await Task.Delay(500); 
            } 
            return false; 
        }
        protected async Task<bool> WaitForPlcOffAsync(string address, int bitOffset, int timeoutSec) 
        {
            DateTime start = DateTime.Now; 
            while ((DateTime.Now - start).TotalSeconds < timeoutSec)
            { 
                if (!(await _driver.ReadBitAsync(address, bitOffset))) return true; 
                await Task.Delay(500); 
            } 
            return false; 
        }
        public override async Task SendTimeCalibration() => await Task.CompletedTask;
        public override void Stop() { _linkedCts?.Cancel(); _driver.Disconnect(); }
    }
}
