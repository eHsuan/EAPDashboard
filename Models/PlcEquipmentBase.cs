using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Protocol.Core.Enums;
using Protocol.MES.Messages;
using Protocol.MES.Factory;

namespace EAPDashboard.Models
{
    public class PlcEquipmentBase : EquipmentBase
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        
        protected readonly IPlcDriver _driver;
        protected List<PlcEventConfig> _events = new List<PlcEventConfig>();
        protected CancellationTokenSource _linkedCts;
        private bool _isFirstPoll = true;
        private readonly Dictionary<int, ushort> _lastValues = new Dictionary<int, ushort>();
        private DateTime _lastHeartbeatTime = DateTime.Now;
        private int _heartbeatTimeoutSeconds = 20;

        public PlcEquipmentBase(IPlcDriver driver)
        {
            _driver = driver;
            // 橋接驅動層日誌至業務邏輯層
            _driver.LogHandler += (lvl, msg) => OnLogReceived(lvl, msg);
        }

        public void SetEvents(List<PlcEventConfig> events)
        {
            _events = events;
        }

        public override async void Start(CancellationToken cancellationToken, CyntecMqttLib.Interfaces.MqttBus _bus, int heartbeatIntervalSeconds)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var ct = _linkedCts.Token;
            _lastHeartbeatTime = DateTime.Now;
            // 逾時門檻設定為心跳間隔的 2 倍，至少 20 秒
            _heartbeatTimeoutSeconds = Math.Max(20, heartbeatIntervalSeconds * 2);

            // 啟動緩衝處理器
            _ = Task.Run(() => StartStatusBufferProcessor(ct), ct);
            _ = Task.Run(() => StartBindingRetryProcessor(ct), ct);

            _ = Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        // Watchdog: 檢查心跳是否逾時 (包含 Connecting 階段)
                        if ((Connection == ConnectionStatus.Connected) && 
                            (DateTime.Now - _lastHeartbeatTime).TotalSeconds > _heartbeatTimeoutSeconds)
                        {
                            OnLogReceived(LogLevel.Warn, $"PLC 心跳偵測逾時 (超過 {_heartbeatTimeoutSeconds} 秒未翻轉)，強制斷線重連");
                            Connection = ConnectionStatus.Disconnected;
                            _driver.Disconnect();
                        }

                        if (!_driver.IsConnected)
                        {
                            Connection = ConnectionStatus.Disconnected;
                            bool ok = await _driver.ConnectAsync(Ip, Port);
                            if (ok)
                            {
                                Connection = ConnectionStatus.Connecting;
                                OnLogReceived(LogLevel.Info, "PLC 實體連線成功，等待心跳翻轉...");
                                _isFirstPoll = true;
                                _lastHeartbeatTime = DateTime.Now;
                            }
                            else
                            {
                                await Task.Delay(5000, ct);
                                continue;
                            }
                        }

                        await ExecuteLogicInternal(ct);
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Error, $"輪詢異常: {ex.Message}");
                        Connection = ConnectionStatus.Disconnected;
                    }

                    // 動態調整輪詢間隔：Connecting 時採用 3 秒以避開取樣混疊，Connected 時恢復 1 秒確保即時性
                    int pollingDelay = (Connection == ConnectionStatus.Connecting) ? 3000 : 1000;
                    await Task.Delay(pollingDelay, ct);
                }
            }, ct);
        }

        private async Task ExecuteLogicInternal(CancellationToken ct)
        {
            foreach (var evt in _events.Where(x => x.EventName.ToUpper() != "PP_SELECT"))
            {
                if (ct.IsCancellationRequested) break;

                try
                {
                    bool isTriggered = false;

                    // 1. 獨立處理 HEARTBEAT 邏輯
                    if (evt.EventName.ToUpper() == "HEARTBEAT" || evt.HandshakeType == "HEARTBEAT")
                    {
                        ushort curRaw = await _driver.ReadWordAsync(evt.TriggerAddr);
                        _lastValues.TryGetValue(evt.EventId, out ushort lastValue);

                        if (curRaw != lastValue)
                        {
                            _lastHeartbeatTime = DateTime.Now;
                            if (Connection == ConnectionStatus.Connecting)
                            {
                                Connection = ConnectionStatus.Connected;
                                OnLogReceived(LogLevel.Info, "偵測到心跳翻轉，PLC 進入連線狀態 (Connected)");
                            }
                        }
                        _lastValues[evt.EventId] = curRaw;
                        // 心跳事件僅用於維持連線狀態，不觸發後續業務流程
                        continue;
                    }

                    // 2. 業務邏輯處理 (ROLLING / BIT)
                    if (evt.HandshakeType == "ROLLING")
                    {
                        ushort curRaw = await _driver.ReadWordAsync(evt.TriggerAddr);
                        _lastValues.TryGetValue(evt.EventId, out ushort lastValue);
                        isTriggered = (curRaw != lastValue && curRaw != 0);
                        _lastValues[evt.EventId] = curRaw;
                    }
                    else if (evt.HandshakeType == "BIT")
                    {
                        bool bitVal = await _driver.ReadBitAsync(evt.TriggerAddr, evt.TrigBitOffset);
                        _lastValues.TryGetValue(evt.EventId, out ushort lastBit);
                        isTriggered = (bitVal && lastBit == 0);
                        _lastValues[evt.EventId] = (ushort)(bitVal ? 1 : 0);

                        if (!bitVal && lastBit == 1)
                        {
                            await _driver.WriteBitAsync(evt.AckAddr, false, evt.AckBitOffset);
                        }
                    }

                    if (isTriggered && !_isFirstPoll)
                    {
                        OnLogReceived(LogLevel.Info, $"偵測到事件觸發: {evt.EventName} (SubEqp: {evt.SubEqpID ?? "N/A"})");
                        await HandlePlcEvent(evt);
                        
                        if (evt.HandshakeType == "ROLLING")
                        {
                            await _driver.WriteWordAsync(evt.AckAddr, _lastValues[evt.EventId]);
                        }
                        else if (evt.HandshakeType == "BIT")
                        {
                            await _driver.WriteBitAsync(evt.AckAddr, true, evt.AckBitOffset);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLogReceived(LogLevel.Warn, $"事件 {evt.EventName} 處理失敗: {ex.Message}");
                }
            }
            _isFirstPoll = false;
        }

        protected virtual async Task HandlePlcEvent(PlcEventConfig evt)
        {
            string reportEqpId = string.IsNullOrEmpty(evt.SubEqpID) ? this.Id : evt.SubEqpID;

            var data = new Dictionary<string, string>();
            foreach (var dp in evt.DataPoints.Where(x => x.AccessType == "READ"))
            {
                if (dp.DataType == "ASCII")
                    data[dp.Key] = await _driver.ReadAsciiAsync(dp.Address, dp.Length);
                else
                    data[dp.Key] = (await _driver.ReadWordAsync(dp.Address)).ToString();
            }

            string getVal(string key, string def) => data.ContainsKey(key) ? data[key] : def;
            string res = "";

            switch (evt.EventName.ToUpper())
            {
                case "TRACK_IN":
                    string tiUserId = getVal("UserID", "");
                    res = await ProcessTrackInAsync(getVal("CassetteID", ""), getVal("WorkOrder", ""), tiUserId, reportEqpId);
                    
                    if (res.StartsWith("PP-SELECT"))
                    {
                        string[] p = res.Split('|');
                        string wo = p[1];
                        string inputParams = p[5];

                        var subConfigs = DatabaseHelper.GetSubEqpConfigs(Id);
                        if (subConfigs.Count > 0)
                        {
                            foreach (var sub in subConfigs)
                            {
                                string subRecipe = GetMesParamValue(inputParams, sub.RecipeParamKey);
                                if (!string.IsNullOrEmpty(subRecipe))
                                {
                                    var dp = evt.DataPoints.FirstOrDefault(x => x.Key == sub.RecipeDataKey && x.AccessType == "WRITE");
                                    if (dp != null) await _driver.WriteAsciiAsync(dp.Address, subRecipe, dp.Length);
                                }
                            }
                        }
                        else
                        {
                            var rp = evt.DataPoints.FirstOrDefault(x => x.Key == "RecipeID" && x.AccessType == "WRITE");
                            if (rp != null) await _driver.WriteAsciiAsync(rp.Address, p[2], rp.Length);
                        }

                        var ppConfig = _events.FirstOrDefault(x => x.EventName.ToUpper() == "PP_SELECT");
                        bool isPpSuccess = true;
                        if (ppConfig != null)
                        {
                            OnLogReceived(LogLevel.Info, $"[TRACK_IN] 觸發 PLC 配方切換指令 ({ppConfig.TriggerAddr}.{ppConfig.TrigBitOffset})");
                            await _driver.WriteBitAsync(ppConfig.TriggerAddr, true, ppConfig.TrigBitOffset);
                            bool isAcked = await WaitForPlcAckAsync(ppConfig.AckAddr, ppConfig.AckBitOffset, timeoutSec: 15);
                            var resDp = ppConfig.DataPoints.FirstOrDefault(x => x.Key == "Result");
                            ushort ppResult = 1;
                            if (resDp != null) ppResult = await _driver.ReadWordAsync(resDp.Address);
                            await _driver.WriteBitAsync(ppConfig.TriggerAddr, false, ppConfig.TrigBitOffset);
                            isPpSuccess = isAcked && ppResult == 1;
                        }

                        if (isPpSuccess)
                        {
                            OnLogReceived(LogLevel.Info, "[TRACK_IN] PLC 配方切換成功 (或無須交握)，執行報工...");
                            string finalRes = await ProcessTrackInFinalAsync(wo, tiUserId, reportEqpId);
                            string[] frp = finalRes.Split('|');
                            bool isPass = frp[0] == "PASS";
                            
                            string mappedCode = DatabaseHelper.GetMappedRtnCode("TRACK_IN", frp[1]);
                            OnLogReceived(isPass ? LogLevel.Info : LogLevel.Warn, $"[TRACK_IN] 報工結果: {frp[1]} - {frp[2]} (寫回 PLC: {mappedCode})");
                            
                            string panelListStr = p.Length > 6 ? p[6] : "";
                            if (!string.IsNullOrEmpty(panelListStr))
                            {
                                string[] panels = panelListStr.Split(',');
                                for (int i = 1; i <= 10; i++)
                                {
                                    var panelDp = evt.DataPoints.FirstOrDefault(x => x.Key == $"PanelList{i}" && x.AccessType == "WRITE");
                                    if (panelDp != null)
                                    {
                                        string pid = (i <= panels.Length) ? panels[i - 1] : "";
                                        await _driver.WriteAsciiAsync(panelDp.Address, pid, panelDp.Length);
                                    }
                                }
                            }

                            var tiResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                            var tiCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                            if (tiResultDp != null) await _driver.WriteWordAsync(tiResultDp.Address, (ushort)(isPass ? 1 : 2));
                            if (tiCodeDp != null) 
                            {
                                if (ushort.TryParse(mappedCode, out ushort tiCodeVal)) await _driver.WriteWordAsync(tiCodeDp.Address, tiCodeVal);
                                else await _driver.WriteAsciiAsync(tiCodeDp.Address, mappedCode, tiCodeDp.Length);
                            }
                            res = finalRes;
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, "[TRACK_IN] PLC 配方切換失敗，終止流程");
                            var tiResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                            var tiCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                            if (tiResultDp != null) await _driver.WriteWordAsync(tiResultDp.Address, 2);
                            if (tiCodeDp != null) await _driver.WriteAsciiAsync(tiCodeDp.Address, "PP_FAIL", tiCodeDp.Length);
                            res = "FAIL|PP_FAIL|配方切換失敗";
                        }
                    }
                    else
                    {
                        string[] p = res.Split('|');
                        string mappedCode = DatabaseHelper.GetMappedRtnCode("TRACK_IN", p[1]);
                        OnLogReceived(LogLevel.Warn, $"[TRACK_IN] 驗證失敗: {p[1]} - {p[2]} (寫回 PLC: {mappedCode})");
                        
                        var tiResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var tiCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (tiResultDp != null) await _driver.WriteWordAsync(tiResultDp.Address, 2);
                        if (tiCodeDp != null) 
                        {
                            if (ushort.TryParse(mappedCode, out ushort tiCodeVal)) await _driver.WriteWordAsync(tiCodeDp.Address, tiCodeVal);
                            else await _driver.WriteAsciiAsync(tiCodeDp.Address, mappedCode, tiCodeDp.Length);
                        }
                    }
                    break;

                case "TRACK_OUT":
                    string toWorkOrder = getVal("WorkOrder", "");
                    string toQtyStr = getVal("Qty", "0");
                    string toUserId = getVal("UserID", "");
                    string toCassetteId = getVal("CassetteID", "");
                    
                    var toPanels = new List<string>();
                    for (int i = 1; i <= 10; i++) {
                        string pid = getVal($"PanelList{i}", "");
                        if (!string.IsNullOrEmpty(pid)) toPanels.Add(pid);
                    }

                    int toQty = int.TryParse(toQtyStr, out int q) ? q : 0;
                    if (toPanels.Count != toQty) {
                        OnLogReceived(LogLevel.Warn, $"[TRACK_OUT] 數量不符: PLC Qty={toQty}, 實際 Panel 數={toPanels.Count}");
                        var toResDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var toMsgDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (toResDp != null) await _driver.WriteWordAsync(toResDp.Address, 2);
                        if (toMsgDp != null) await _driver.WriteAsciiAsync(toMsgDp.Address, "QTY_MISMATCH", toMsgDp.Length);
                        res = "FAIL|QTY_MISMATCH|數量不符";
                    }
                    else {
                        string toRes = await ProcessTrackOutAsync(toWorkOrder, toQtyStr, toUserId, toCassetteId, toPanels, reportEqpId);
                        string[] top = toRes.Split('|');
                        bool isToPass = top[0] == "PASS";
                        string toMesCode = top[1];
                        
                        string toMappedCode = DatabaseHelper.GetMappedRtnCode("TRACK_OUT", toMesCode);
                        OnLogReceived(isToPass ? LogLevel.Info : LogLevel.Warn, $"[TRACK_OUT] MES 回覆: {toMesCode} - {top[2]} (寫回 PLC: {toMappedCode})");

                        var toResDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var toCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (toResDp != null) await _driver.WriteWordAsync(toResDp.Address, (ushort)(isToPass ? 1 : 2));
                        if (toCodeDp != null) 
                        {
                            if (ushort.TryParse(toMappedCode, out ushort toCodeVal)) await _driver.WriteWordAsync(toCodeDp.Address, toCodeVal);
                            else await _driver.WriteAsciiAsync(toCodeDp.Address, toMappedCode, toCodeDp.Length);
                        }
                        res = toRes;
                    }
                    break;

                case "PANEL_IN":
                    string piRes = await ProcessPanelInAsync(getVal("PanelID", ""), getVal("CassetteID", ""), reportEqpId);
                    string[] piParts = piRes.Split('|');
                    string piMapped = DatabaseHelper.GetMappedRtnCode("PANEL_IN", piParts[1]);
                    
                    var piResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var piCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (piResultDp != null) await _driver.WriteWordAsync(piResultDp.Address, 1);
                    if (piCodeDp != null) 
                    {
                        if (ushort.TryParse(piMapped, out ushort piCodeVal)) await _driver.WriteWordAsync(piCodeDp.Address, piCodeVal);
                        else await _driver.WriteAsciiAsync(piCodeDp.Address, piMapped, piCodeDp.Length);
                    }
                    res = piRes;
                    break;

                case "PANEL_OUT":
                    string poRes = await ProcessPanelOutAsync(getVal("PanelID", ""), getVal("CassetteID", ""), getVal("Result", "1"), reportEqpId);
                    string[] poParts = poRes.Split('|');
                    string poMapped = DatabaseHelper.GetMappedRtnCode("PANEL_OUT", poParts[1]);

                    var poResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var poCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (poResultDp != null) await _driver.WriteWordAsync(poResultDp.Address, 1);
                    if (poCodeDp != null) 
                    {
                        if (ushort.TryParse(poMapped, out ushort poCodeVal)) await _driver.WriteWordAsync(poCodeDp.Address, poCodeVal);
                        else await _driver.WriteAsciiAsync(poCodeDp.Address, poMapped, poCodeDp.Length);
                    }
                    res = poRes;
                    break;

                case "ALARM":
                    string alStatus = getVal("AlarmStatus", "1");
                    string alRes = await ProcessAlarmAsync(getVal("AlarmCode", "9999"), alStatus == "1", reportEqpId);
                    string[] alParts = alRes.Split('|');
                    bool isAlPass = alParts[0] == "PASS";
                    string alMappedCode = DatabaseHelper.GetMappedRtnCode("ALARM", alParts[1]);
                    OnLogReceived(isAlPass ? LogLevel.Info : LogLevel.Warn, $"[ALARM] MES 回覆: {alParts[1]} - {alParts[2]} (寫回 PLC: {alMappedCode})");

                    var alResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var alCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (alResultDp != null) await _driver.WriteWordAsync(alResultDp.Address, (ushort)(isAlPass ? 1 : 2));
                    if (alCodeDp != null) 
                    {
                        if (ushort.TryParse(alMappedCode, out ushort alCodeVal)) await _driver.WriteWordAsync(alCodeDp.Address, alCodeVal);
                        else await _driver.WriteAsciiAsync(alCodeDp.Address, alMappedCode, alCodeDp.Length);
                    }
                    res = alRes;
                    break;

                case "STATUS":
                    string stRes = await ProcessStatusChangeAsync(getVal("OldStatus", "3"), getVal("NewStatus", "3"), reportEqpId);
                    string[] stParts = stRes.Split('|');
                    bool isStPass = stParts[0] == "PASS";
                    string stMappedCode = DatabaseHelper.GetMappedRtnCode("STATUS", stParts[1]);
                    OnLogReceived(isStPass ? LogLevel.Info : LogLevel.Warn, $"[STATUS] MES 回覆: {stParts[1]} - {stParts[2]} (寫回 PLC: {stMappedCode})");

                    var stResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var stCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (stResultDp != null) await _driver.WriteWordAsync(stResultDp.Address, (ushort)(isStPass ? 1 : 2));
                    if (stCodeDp != null) 
                    {
                        if (ushort.TryParse(stMappedCode, out ushort stCodeVal)) await _driver.WriteWordAsync(stCodeDp.Address, stCodeVal);
                        else await _driver.WriteAsciiAsync(stCodeDp.Address, stMappedCode, stCodeDp.Length);
                    }
                    res = stRes;
                    break;

                case "USER_VERIFY":
                    string uvRes = await ProcessUserVerifyAsync(getVal("Barcode", ""), getVal("UserID", ""), reportEqpId);
                    string[] uvParts = uvRes.Split('|');
                    bool isUvPass = uvParts[0] == "PASS";
                    string mappedRtnCode = DatabaseHelper.GetMappedRtnCode("USER_VERIFY", uvParts[1]);
                    string mesUserId = uvParts.Length > 3 ? uvParts[3] : "";
                    OnLogReceived(isUvPass ? LogLevel.Info : LogLevel.Warn, $"[USER_VERIFY] MES 回覆: {uvParts[1]} - {uvParts[2]} (UserID: {mesUserId}, 寫回 PLC: {mappedRtnCode})");

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
                    break;

                case "KEY_PARAMETERS":
                case "KPI_PARAMETERS":
                    try {
                        var kpiStartDp = evt.DataPoints.FirstOrDefault(x => x.Key == "KPI_Start" && x.AccessType == "READ");
                        if (kpiStartDp == null) break;

                        ushort[] rawKpiData = await _driver.ReadAsync(kpiStartDp.Address, 60);
                        var mappings = DatabaseHelper.GetKpiMappings(this.Id);
                        var paramList = new List<Protocol.Core.Messages.KeyParameter>();

                        for (int i = 0; i < 20; i++) {
                            var map = mappings.FirstOrDefault(m => m.ParaIndex == i + 1 && m.IsEnabled);
                            if (map != null) {
                                double val = (short)rawKpiData[i * 3] / Math.Pow(10, map.DecimalPlaces);
                                double lsl = (short)rawKpiData[i * 3 + 1] / Math.Pow(10, map.DecimalPlaces);
                                double usl = (short)rawKpiData[i * 3 + 2] / Math.Pow(10, map.DecimalPlaces);
                                paramList.Add(new Protocol.Core.Messages.KeyParameter { Parameter = map.ParameterName, Value = val.ToString($"F{map.DecimalPlaces}"), LSL = lsl.ToString($"F{map.DecimalPlaces}"), USL = usl.ToString($"F{map.DecimalPlaces}") });
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
            }

            var msgDp = evt.DataPoints.FirstOrDefault(x => x.Key == "ResultMsg" && x.AccessType == "WRITE");
            if (msgDp != null && !string.IsNullOrEmpty(res)) await _driver.WriteAsciiAsync(msgDp.Address, res.Split('|').Last(), msgDp.Length);
        }

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();
        private void HandleEqpStatusCall(EQPSTATUSRequest req) { Task.Run(() => { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); } catch { _statusBuffer.Enqueue(req); } }); }
        private async Task StartStatusBufferProcessor(CancellationToken ct) { while (!ct.IsCancellationRequested) { if (_statusBuffer.TryPeek(out var req)) { try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); _statusBuffer.TryDequeue(out _); } catch { await Task.Delay(5000, ct); continue; } } await Task.Delay(1000, ct); } }
        protected async Task<bool> WaitForPlcAckAsync(string address, int bitOffset, int timeoutSec) { DateTime start = DateTime.Now; while ((DateTime.Now - start).TotalSeconds < timeoutSec) { if (await _driver.ReadBitAsync(address, bitOffset)) return true; await Task.Delay(500); } return false; }
        public override async Task SendTimeCalibration() => await Task.CompletedTask;
        public override void Stop() { _linkedCts?.Cancel(); _driver.Disconnect(); }
    }
}
