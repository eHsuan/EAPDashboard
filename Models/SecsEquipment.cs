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

        public override void Start(CancellationToken externalCancellationToken, MqttBus _bus, int heartbeatIntervalSeconds)
        {
            try {
                OnLogReceived(LogLevel.Info, $"[START] SECS module initialized for {Id}.");
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);
                
                string fullDictPath = Path.IsPathRooted(DictionaryPath) ? DictionaryPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictionaryPath);
                if (File.Exists(fullDictPath)) _dictionary = SecsDictionary.Load(fullDictPath);

                LoadAlarmMappings();
                _ = StartStatusBufferProcessor(_linkedCts.Token);

                _retryTimer = new System.Timers.Timer(15000);
                _retryTimer.Elapsed += (s, e) => { if (Connection == ConnectionStatus.Disconnected) _ = RunConnectionTask(_linkedCts.Token); };
                _retryTimer.AutoReset = true; _retryTimer.Enabled = true;

                _ = RunConnectionTask(_linkedCts.Token);
            } catch (Exception ex) { OnLogReceived(LogLevel.Fatal, $"[CRITICAL] SECS Crash: {ex.Message}"); }
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
                    if (f == 1) await message.TryReplyAsync(new SecsMessage(1, 2) { SecsItem = Item.L(Item.A(Name), Item.A(Id)) });
                    else if (f == 13) await message.TryReplyAsync(new SecsMessage(1, 14) { SecsItem = Item.B(0) });
                    break;
                case 6: if (f == 11) await HandleS6F11(message); break;
                case 5: if (f == 1) await HandleS5F1(message); break;
            }
        }

        private async Task HandleS6F11(PrimaryMessageWrapper message)
        {
            try {
                var root = message.PrimaryMessage.SecsItem;
                if (root == null || root.Count < 3) return;
                string ceid = root.Items[1].FirstValue<uint>().ToString();
                OnLogReceived(LogLevel.Info, $"[EVENT] {_dictionary?.GetEventName(ceid) ?? ceid} (CEID:{ceid})");

                string statusCode = ExtractVidValue(root.Items[2], "102");
                if (int.TryParse(statusCode, out int code) && SecsStatusMap.TryGetValue(code, out var ms)) {
                    if (State != EquipmentState.Alarm) State = ms;
                }

                if (ceid == "200") await HandleTrackInEvent(root);
                else if (ceid == "206") await HandleTrackOutEvent(root);
                else if (new[] { "50", "51", "52", "53" }.Contains(ceid)) HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = statusCode ?? "3" });

                await message.TryReplyAsync(new SecsMessage(6, 12) { SecsItem = Item.B(0) });
            } catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] S6F11: {ex.Message}"); }
        }

        private async Task HandleTrackInEvent(Item root)
        {
            string rid = ExtractVidValue(root, "111") ?? "UNKNOWN";
            string cid = ExtractVidValue(root, "300") ?? ExtractVidValue(root, "320") ?? "UNKNOWN";
            string res = await ProcessTrackInAsync(cid, null);
            if (res.StartsWith("PP-SELECT")) {
                string[] p = res.Split('|');
                var remoteCmd = new SecsMessage(2, 41) { SecsItem = Item.L(Item.A("PP-SELECT"), Item.L(Item.L(Item.A("PPID"), Item.A(p[2])))) };
                using (var reply = await _secsGem.SendAsync(remoteCmd)) {
                    if (reply.S == 2 && reply.F == 42 && reply.SecsItem.Items[0].FirstValue<byte>() == 0) {
                        string fin = await ProcessTrackInFinalAsync(p[1]);
                        OnLogReceived(LogLevel.Info, $"[AUTO] 進站報工完成: {fin}");
                    }
                }
            }
        }

        private async Task HandleTrackOutEvent(Item root)
        {
            string rid = ExtractVidValue(root, "111") ?? "UNKNOWN";
            string res = await ProcessTrackOutAsync(rid, "1");
            OnLogReceived(LogLevel.Info, $"[AUTO] 出站報工結果: {res}");
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
            foreach (var item in root.Items) {
                var f = ExtractVidValue(item, vid);
                if (!string.IsNullOrEmpty(f)) return f;
            }
            return null;
        }

        private async Task HandleS5F1(PrimaryMessageWrapper message)
        {
            try {
                var root = message.PrimaryMessage.SecsItem;
                if (root != null && root.Count >= 3) {
                    byte alcd = root.Items[0].FirstValue<byte>(); string altx = root.Items[2].GetString();
                    bool isSet = (alcd & 0x80) != 0 || alcd == 1;
                    string mc = TransformAlarmCode(altx);
                    if (isSet) { State = EquipmentState.Alarm; HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "1", ErrorCode = mc }); }
                    else { if (State == EquipmentState.Alarm) State = EquipmentState.Idle; HandleEqpStatusCall(new EQPSTATUSRequest { EqpNo = Id, StatusCode = "2", ErrorCode = mc }); }
                }
                await message.TryReplyAsync(new SecsMessage(5, 2) { SecsItem = Item.B(0) });
            } catch { }
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

        private async Task SendS1F13(ISecsGem gem) { try { using (var reply = await gem.SendAsync(new SecsMessage(1, 13) { SecsItem = Item.L() })) { if (reply.S == 1 && reply.F == 14 && reply.SecsItem.Items[0].FirstValue<byte>() == 0) Connection = ConnectionStatus.Connected; } } catch { } }
        public override async Task SendTimeCalibration() { if (Connection == ConnectionStatus.Connected && _secsGem != null) await _secsGem.SendAsync(new SecsMessage(2, 31) { SecsItem = Item.A(DateTime.Now.ToString("yyyyMMddHHmmss")) }); }
        public override void Stop() { _retryTimer?.Stop(); _linkedCts?.Cancel(); }

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
