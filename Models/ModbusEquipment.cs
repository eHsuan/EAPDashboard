using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Enums;
using CyntecMqttLib.Interfaces;

namespace EAPDashboard.Models
{
    public class ModbusEquipment : EquipmentBase
    {
        private CancellationTokenSource _linkedCts;
        private IModbusMaster _master;

        private List<ModbusEventConfig> _events = new List<ModbusEventConfig>();
        private readonly Dictionary<string, ushort> _lastTriggerValues = new Dictionary<string, ushort>();
        private readonly System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest> _statusBuffer = new System.Collections.Concurrent.ConcurrentQueue<EQPSTATUSRequest>();
        
        private bool _isFirstPoll = true;
        private DateTime _lastPlcHeartbeatTime = DateTime.Now;

        public string Ip { get; set; }
        public int Port { get; set; } = 502;
        public int PollingIntervalMs { get; set; } = 1000;

        public void SetEvents(List<ModbusEventConfig> events) => _events = events ?? new List<ModbusEventConfig>();

        public ModbusEquipment()
        {
            Protocol = "ModbusTCP";
            Connection = ConnectionStatus.Disconnected;
        }

        public override void Start(CancellationToken externalCancellationToken, MqttBus bus, int heartbeatIntervalSeconds)
        {
            Stop();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
            LoadAlarmMappings();
            _ = RunModbusTask(_linkedCts.Token);
            _ = StartStatusBufferProcessor(_linkedCts.Token);
        }

        private async Task RunModbusTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try {
                    if (Connection != ConnectionStatus.Connected) {
                        Connection = ConnectionStatus.Connecting;
                        _master = await MainForm.GetModbusMasterAsync(Ip, Port);
                        if (_master != null) {
                            Connection = ConnectionStatus.Connected;
                            _isFirstPoll = true; _lastTriggerValues.Clear(); _lastPlcHeartbeatTime = DateTime.Now;
                            OnLogReceived(LogLevel.Info, "[CONN] PLC 已透過連線池取得實例。");
                        }
                    }
                    if (Connection == ConnectionStatus.Connected) {
                        await PollEventsAsync(token);
                        if ((DateTime.Now - _lastPlcHeartbeatTime).TotalSeconds > 30) {
                            OnLogReceived(LogLevel.Warn, "PLC 心跳逾時。");
                            Connection = ConnectionStatus.Disconnected;
                        }
                    }
                } catch (Exception ex) {
                    OnLogReceived(LogLevel.Error, $"Modbus 輪詢異常: {ex.Message}");
                    Connection = ConnectionStatus.Disconnected;
                }
                await Task.Delay(PollingIntervalMs, token);
            }
        }

        private async Task PollEventsAsync(CancellationToken token)
        {
            foreach (var evt in _events) {
                if (token.IsCancellationRequested) return;
                try {
                    ushort[] reg = await _master.ReadHoldingRegistersAsync(1, evt.TriggerAddr, 1);
                    ushort curRaw = reg[0];

                    if (evt.HandshakeType == "HEARTBEAT") {
                        int bit = (curRaw >> evt.BitOffset) & 1;
                        if (!_lastTriggerValues.ContainsKey(evt.EventName) || bit != _lastTriggerValues[evt.EventName]) {
                            _lastPlcHeartbeatTime = DateTime.Now; _lastTriggerValues[evt.EventName] = (ushort)bit;
                        }
                        continue;
                    }

                    if (_isFirstPoll) { _lastTriggerValues[evt.EventName] = curRaw; continue; }
                    _lastTriggerValues.TryGetValue(evt.EventName, out ushort lastValue);

                    bool isTriggered = false;
                    if (evt.HandshakeType == "ROLLING") {
                        isTriggered = (curRaw != lastValue && curRaw != 0);
                    } else if (evt.HandshakeType == "BIT") {
                        int curBit = (curRaw >> evt.BitOffset) & 1;
                        int lastBit = (lastValue >> evt.BitOffset) & 1;
                        isTriggered = (lastBit == 0 && curBit == 1);
                    }

                    if (isTriggered) {
                        OnLogReceived(LogLevel.Info, $"[TRIGGER] {evt.EventName} (Mode:{evt.HandshakeType})");
                        await ExecuteLogicInternal(evt, curRaw, token);
                    } else if (evt.HandshakeType == "BIT") {
                        int curBit = (curRaw >> evt.BitOffset) & 1;
                        int lastBit = (lastValue >> evt.BitOffset) & 1;
                        if (lastBit == 1 && curBit == 0) {
                            await WriteAckBit(evt.AckAddr, evt.BitOffset, false);
                            _lastTriggerValues[evt.EventName] = curRaw;
                        }
                    }
                    _lastTriggerValues[evt.EventName] = curRaw;
                } catch { throw; }
            }
            _isFirstPoll = false;
        }

        private async Task ExecuteLogicInternal(ModbusEventConfig evt, ushort triggerVal, CancellationToken token)
        {
            var data = await ReadDataPoints(evt);
            string res = "FAIL|Unknown";
            Func<string, string, string> getVal = (key, def) => data.ContainsKey(key) ? data[key] : def;

            switch (evt.EventName.ToUpper()) {
                case "TRACKIN":
                    res = await ProcessTrackInAsync(getVal("CassetteID", ""), getVal("WorkOrder", ""));
                    if (res.StartsWith("PP-SELECT")) {
                        string[] p = res.Split('|');
                        string wo = p[1];
                        string inputParams = p.Length > 3 ? p[3] : "";

                        // 1. 取得子設備配置
                        var subConfigs = DatabaseHelper.GetSubEqpConfigs(Id);
                        if (subConfigs.Count > 0) {
                            OnLogReceived(LogLevel.Info, $"[TRACKIN] 偵測到 {subConfigs.Count} 個子設備配置，開始寫入配方...");
                            foreach (var sub in subConfigs) {
                                string subRecipe = GetMesParamValue(inputParams, sub.MesParamKey);
                                if (!string.IsNullOrEmpty(subRecipe)) {
                                    var dp = evt.DataPoints.FirstOrDefault(x => x.Key == sub.DataKey && x.AccessType == "WRITE");
                                    if (dp != null) {
                                        OnLogReceived(LogLevel.Info, $"[TRACKIN] 寫入子設備 {sub.SubEqpName} 配方: {subRecipe} 到位址 {dp.Address}");
                                        await WriteAscii(dp.Address, subRecipe, dp.Length);
                                    } else {
                                        OnLogReceived(LogLevel.Warn, $"[TRACKIN] 找不到子設備 {sub.SubEqpName} 對應的 DataPoint: {sub.DataKey}");
                                    }
                                } else {
                                    OnLogReceived(LogLevel.Warn, $"[TRACKIN] MES 參數清單中找不到子設備 {sub.SubEqpName} 的 Key: {sub.MesParamKey}");
                                }
                            }
                        } else {
                            // 相容舊邏輯：若無子設備配置，則尋找預設 RecipeID 寫入位址
                            var rp = evt.DataPoints.FirstOrDefault(x => x.Key == "RecipeID" && x.AccessType == "WRITE");
                            if (rp != null) await WriteAscii(rp.Address, p[2], rp.Length);
                        }

                        res = await ProcessTrackInFinalAsync(wo);
                    } break;
                case "TRACKOUT":
 res = await ProcessTrackOutAsync(getVal("WorkOrder", ""), getVal("Qty", "1")); break;
                case "USERVERIFY": res = await ProcessUserVerifyAsync(getVal("Barcode", "")); break;
                case "ALARM": 
                    string mc = TransformAlarmCode(getVal("AlarmCode", ""));
                    HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = mc });
                    res = "PASS|Alarm OK"; break;
                case "STATUS": HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = getVal("Status", "3") }); res = "PASS|OK"; break;
                case "TIMESYNC":
                    var tp = evt.DataPoints.FirstOrDefault(x => x.Key == "SystemTime" && x.AccessType == "WRITE");
                    if (tp != null) await WriteAscii(tp.Address, DateTime.Now.ToString("yyyyMMddHHmmss"), tp.Length);
                    res = "PASS|OK"; break;
                case "DATAREPORT":
                    await SaveProductionDataAsync("DataReport", getVal("WorkOrder", ""), data);
                    res = "PASS|Data OK"; break;
            }

            var mp = evt.DataPoints.FirstOrDefault(x => x.Key == "ResultMsg" && x.AccessType == "WRITE");
            if (mp != null) await WriteAscii(mp.Address, res.Split('|').Last(), mp.Length);

            // 交握 ACK
            if (evt.HandshakeType == "ROLLING") {
                await _master.WriteSingleRegisterAsync(1, evt.AckAddr, triggerVal);
            } else if (evt.HandshakeType == "BIT") {
                await WriteAckBit(evt.AckAddr, evt.BitOffset, true);
            }
            
            OnLogReceived(LogLevel.Info, $"[FINISH] {evt.EventName} Result: {res.Split('|')[0]}");
        }

        private async Task WriteAckBit(ushort addr, int bitOffset, bool value)
        {
            // 讀取當前字組
            ushort[] reg = await _master.ReadHoldingRegistersAsync(1, addr, 1);
            ushort current = reg[0];
            
            // 修改特定位元
            if (value) current |= (ushort)(1 << bitOffset);
            else current &= (ushort)~(1 << bitOffset);
            
            // 寫回字組
            await _master.WriteSingleRegisterAsync(1, addr, current);
        }

        private async Task<Dictionary<string, string>> ReadDataPoints(ModbusEventConfig evt) {
            var res = new Dictionary<string, string>();
            foreach (var dp in evt.DataPoints.Where(x => x.AccessType == "READ")) {
                ushort[] d = await _master.ReadHoldingRegistersAsync(1, dp.Address, dp.Length);
                res[dp.Key] = dp.DataType == "ASCII" ? DecodeAscii(d) : d[0].ToString();
            } return res;
        }

        private string DecodeAscii(ushort[] regs) {
            var b = new List<byte>(); foreach (var r in regs) { b.Add((byte)(r >> 8)); b.Add((byte)(r & 0xFF)); }
            return Encoding.ASCII.GetString(b.ToArray()).Trim('\0', ' ');
        }

        private async Task WriteAscii(ushort addr, string txt, int len) {
            byte[] b = Encoding.ASCII.GetBytes(txt.PadRight(len * 2, '\0'));
            ushort[] r = new ushort[len]; for (int i = 0; i < len; i++) r[i] = (ushort)((b[i * 2] << 8) | b[i * 2 + 1]);
            await _master.WriteMultipleRegistersAsync(1, addr, r);
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
        public override Task SendTimeCalibration() => Task.CompletedTask;
        public override void Stop() => _linkedCts?.Cancel();
    }
}
