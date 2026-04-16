using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
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

        public PlcEquipmentBase(IPlcDriver driver)
        {
            _driver = driver;
        }

        public void SetEvents(List<PlcEventConfig> events)
        {
            _events = events;
        }

        public override async void Start(CancellationToken cancellationToken, CyntecMqttLib.Interfaces.MqttBus _bus, int heartbeatIntervalSeconds)
        {
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var ct = _linkedCts.Token;

            // 啟動緩衝處理器
            _ = Task.Run(() => StartStatusBufferProcessor(ct), ct);
            _ = Task.Run(() => StartBindingRetryProcessor(ct), ct);

            _ = Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        if (!_driver.IsConnected)
                        {
                            Connection = ConnectionStatus.Disconnected;
                            bool ok = await _driver.ConnectAsync(Ip, Port);
                            if (ok)
                            {
                                Connection = ConnectionStatus.Connected;
                                OnLogReceived(LogLevel.Info, "PLC 已連線");
                                _isFirstPoll = true;
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
                    await Task.Delay(1000, ct);
                }
            }, ct);
        }

        private async Task ExecuteLogicInternal(CancellationToken ct)
        {
            // 僅輪詢非 PP_SELECT 的事件 (PP_SELECT 用作配置，不參與被動輪詢)
            foreach (var evt in _events.Where(x => x.EventName.ToUpper() != "PP_SELECT"))
            {
                if (ct.IsCancellationRequested) break;

                try
                {
                    bool isTriggered = false;

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

                        // 自動清除 Ack Bit (當 Trigger Bit 變回 0 時)
                        if (!bitVal && lastBit == 1)
                        {
                            await _driver.WriteBitAsync(evt.AckAddr, false, evt.AckBitOffset);
                        }
                    }

                    if (isTriggered && !_isFirstPoll)
                    {
                        OnLogReceived(LogLevel.Info, $"偵測到事件觸發: {evt.EventName} (SubEqp: {evt.SubEqpID ?? "N/A"})");
                        await HandlePlcEvent(evt);
                        
                        // 交握結束 (寫回 Ack)
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
            // 決定上報給 MES 的設備 ID (優先使用子機 ID)
            string reportEqpId = string.IsNullOrEmpty(evt.SubEqpID) ? this.Id : evt.SubEqpID;

            // 讀取該事件關聯的所有 DataPoints
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
                        string tiMesCode = p[3];
                        string tiMesMsg = p[4];

                        // 1. 寫入各子設備配方
                        var subConfigs = DatabaseHelper.GetSubEqpConfigs(Id);
                        if (subConfigs.Count > 0)
                        {
                            foreach (var sub in subConfigs)
                            {
                                string subRecipe = GetMesParamValue(inputParams, sub.MesParamKey);
                                if (!string.IsNullOrEmpty(subRecipe))
                                {
                                    var dp = evt.DataPoints.FirstOrDefault(x => x.Key == sub.DataKey && x.AccessType == "WRITE");
                                    if (dp != null) await _driver.WriteAsciiAsync(dp.Address, subRecipe, dp.Length);
                                }
                            }
                        }
                        else
                        {
                            var rp = evt.DataPoints.FirstOrDefault(x => x.Key == "RecipeID" && x.AccessType == "WRITE");
                            if (rp != null) await _driver.WriteAsciiAsync(rp.Address, p[2], rp.Length);
                        }

                        // 2. 二階段交握：PP_SELECT
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
                            
                            // 寫回 Panel List (1-10)
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
                            if (tiCodeDp != null) await _driver.WriteAsciiAsync(tiCodeDp.Address, mappedCode, tiCodeDp.Length);
                            
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
                        if (tiCodeDp != null) await _driver.WriteAsciiAsync(tiCodeDp.Address, mappedCode, tiCodeDp.Length);
                    }
                    break;

                case "TRACK_OUT":
                    string toWorkOrder = getVal("WorkOrder", "");
                    string toQtyStr = getVal("Qty", "0");
                    string toUserId = getVal("UserID", "");
                    string toCassetteId = getVal("CassetteID", "");
                    
                    // 讀取 Panel List 1-10 並校驗數量
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
                        // 執行業務流 (含 MES 出站與 DB 綁定)
                        string toRes = await ProcessTrackOutAsync(toWorkOrder, toQtyStr, toUserId, toCassetteId, toPanels, reportEqpId);
                        string[] top = toRes.Split('|');
                        bool isToPass = top[0] == "PASS";
                        string toMesCode = top[1];
                        
                        string toMappedCode = DatabaseHelper.GetMappedRtnCode("TRACK_OUT", toMesCode);
                        OnLogReceived(isToPass ? LogLevel.Info : LogLevel.Warn, $"[TRACK_OUT] MES 回覆: {toMesCode} - {top[2]} (寫回 PLC: {toMappedCode})");

                        var toResDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                        var toCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                        if (toResDp != null) await _driver.WriteWordAsync(toResDp.Address, (ushort)(isToPass ? 1 : 2));
                        if (toCodeDp != null) await _driver.WriteAsciiAsync(toCodeDp.Address, toMappedCode, toCodeDp.Length);
                        res = toRes;
                    }
                    break;

                case "PANEL_IN":
                    string piCid = getVal("CassetteID", "");
                    string piPid = getVal("PanelID", "");
                    
                    // 1. 執行本地記錄
                    string piRes = await ProcessPanelInAsync(piPid, piCid, reportEqpId);
                    string[] piParts = piRes.Split('|');

                    // 2. 轉譯 (雖然目前固定 PASS)
                    string piMapped = DatabaseHelper.GetMappedRtnCode("PANEL_IN", piParts[1]);
                    
                    // 3. 寫回 PLC
                    var piResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var piCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (piResultDp != null) await _driver.WriteWordAsync(piResultDp.Address, 1);
                    if (piCodeDp != null) await _driver.WriteAsciiAsync(piCodeDp.Address, piMapped, piCodeDp.Length);
                    
                    res = piRes;
                    break;

                case "PANEL_OUT":
                    string poCid = getVal("CassetteID", "");
                    string poPid = getVal("PanelID", "");
                    string poCheck = getVal("Result", "1"); // PLC 判定
                    
                    // 1. 執行本地記錄
                    string poRes = await ProcessPanelOutAsync(poPid, poCid, poCheck, reportEqpId);
                    string[] poParts = poRes.Split('|');

                    // 2. 轉譯
                    string poMapped = DatabaseHelper.GetMappedRtnCode("PANEL_OUT", poParts[1]);

                    // 3. 寫回 PLC
                    var poResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var poCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");
                    if (poResultDp != null) await _driver.WriteWordAsync(poResultDp.Address, 1);
                    if (poCodeDp != null) await _driver.WriteAsciiAsync(poCodeDp.Address, poMapped, poCodeDp.Length);
                    
                    res = poRes;
                    break;

                case "ALARM":
                    string alStatus = getVal("AlarmStatus", "1"); // 1:發生, 2:解除
                    string alCode = getVal("AlarmCode", "9999");
                    string alType = getVal("AlarmType", "1");    // 1:Alarm, 2:Warning
                    
                    // 1. 執行上報 (回傳格式: Result|Code|Message)
                    string alRes = await ProcessAlarmAsync(alCode, alStatus == "1", reportEqpId);
                    string[] alParts = alRes.Split('|');
                    bool isAlPass = alParts[0] == "PASS";
                    string alMesCode = alParts[1];

                    // 2. 轉譯代碼並記錄 Log
                    string alMappedCode = DatabaseHelper.GetMappedRtnCode("ALARM", alMesCode);
                    OnLogReceived(isAlPass ? LogLevel.Info : LogLevel.Warn, $"[ALARM] MES 回覆: {alMesCode} - {alParts[2]} (寫回 PLC: {alMappedCode})");

                    // 3. 寫回 PLC
                    var alResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var alCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");

                    if (alResultDp != null) await _driver.WriteWordAsync(alResultDp.Address, (ushort)(isAlPass ? 1 : 2));
                    if (alCodeDp != null) await _driver.WriteAsciiAsync(alCodeDp.Address, alMappedCode, alCodeDp.Length);
                    
                    res = alRes;
                    break;

                case "STATUS":
                    string oldSt = getVal("OldStatus", "3");
                    string newSt = getVal("NewStatus", "3");
                    
                    // 1. 執行上報與 UI 更新
                    string stRes = await ProcessStatusChangeAsync(oldSt, newSt, reportEqpId);
                    string[] stParts = stRes.Split('|');
                    bool isStPass = stParts[0] == "PASS";

                    // 2. 轉譯代碼並記錄 Log
                    string stMappedCode = DatabaseHelper.GetMappedRtnCode("STATUS", stParts[1]);
                    OnLogReceived(isStPass ? LogLevel.Info : LogLevel.Warn, $"[STATUS] MES 回覆: {stParts[1]} - {stParts[2]} (寫回 PLC: {stMappedCode})");

                    // 3. 寫回 PLC
                    var stResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var stCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");

                    if (stResultDp != null) await _driver.WriteWordAsync(stResultDp.Address, (ushort)(isStPass ? 1 : 2));
                    if (stCodeDp != null) await _driver.WriteAsciiAsync(stCodeDp.Address, stMappedCode, stCodeDp.Length);
                    
                    res = stRes;
                    break;

                case "USER_VERIFY":
                    string barcode = getVal("Barcode", "");
                    string userId = getVal("UserID", "");
                    
                    string uvRes = await ProcessUserVerifyAsync(barcode, userId, reportEqpId);
                    string[] uvParts = uvRes.Split('|');
                    bool isUvPass = uvParts[0] == "PASS";
                    string uvMesCode = uvParts[1];
                    string uvMesMsg = uvParts[2];

                    string mappedRtnCode = DatabaseHelper.GetMappedRtnCode("USER_VERIFY", uvMesCode);
                    OnLogReceived(isUvPass ? LogLevel.Info : LogLevel.Warn, $"[USER_VERIFY] MES 回覆: {uvMesCode} - {uvMesMsg} (轉譯 PLC 代碼: {mappedRtnCode})");

                    var uvResultDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnResult" && x.AccessType == "WRITE");
                    var uvCodeDp = evt.DataPoints.FirstOrDefault(x => x.Key == "RtnCode" && x.AccessType == "WRITE");

                    if (uvResultDp != null) await _driver.WriteWordAsync(uvResultDp.Address, (ushort)(isUvPass ? 1 : 2));
                    if (uvCodeDp != null) await _driver.WriteAsciiAsync(uvCodeDp.Address, mappedRtnCode, uvCodeDp.Length);
                    
                    res = uvRes;
                    break;
            }

            var msgDp = evt.DataPoints.FirstOrDefault(x => x.Key == "ResultMsg" && x.AccessType == "WRITE");
            if (msgDp != null && !string.IsNullOrEmpty(res))
            {
                await _driver.WriteAsciiAsync(msgDp.Address, res.Split('|').Last(), msgDp.Length);
            }
        }

        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();
        
        private void HandleEqpStatusCall(EQPSTATUSRequest req)
        {
            Task.Run(() => {
                try { MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req); }
                catch { _statusBuffer.Enqueue(req); }
            });
        }

        private async Task StartStatusBufferProcessor(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (_statusBuffer.TryPeek(out var req))
                {
                    try
                    {
                        MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(req);
                        _statusBuffer.TryDequeue(out _);
                    }
                    catch { await Task.Delay(5000, ct); continue; }
                }
                await Task.Delay(1000, ct);
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

        public override async Task SendTimeCalibration()
        {
            await Task.CompletedTask;
        }

        public override void Stop()
        {
            _linkedCts?.Cancel();
            _driver.Disconnect();
        }
    }
}
