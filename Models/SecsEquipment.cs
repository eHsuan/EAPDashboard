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

namespace EAPDashboard.Models
{
    public class SecsEquipment : EquipmentBase
    {
        private CancellationTokenSource _linkedCts;
        private ISecsGem _secsGem;
        private SecsDictionary _dictionary;
        private System.Timers.Timer _retryTimer;

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

        private async Task RunConnectionTask(CancellationToken token)
        {
            if (Connection != ConnectionStatus.Disconnected) return;

            try
            {
                // 1. Explicit Parameter Validation
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

                    // 2. Start state monitoring task BEFORE message iteration
                    // This ensures we see state changes even if the message loop is blocked waiting for connection
                    var commTask = Task.Run(async () => {
                        ConnectionState lastState = (ConnectionState)99; // Force initial report
                        while (!token.IsCancellationRequested && _secsGem != null)
                        {
                            var currentState = connection.State;
                            if (currentState != lastState)
                            {
                                OnLogReceived(LogLevel.Info, $"[HSMS] Transport State: {currentState}");
                                lastState = currentState;
                                
                                if (currentState == ConnectionState.Selected)
                                {
                                    OnLogReceived(LogLevel.Info, "[CONN] Transport Selected. Attempting logic handshake (S1F13)...");
                                    await SendS1F13(gem);
                                }
                                else if ((int)currentState == 0) // Disconnected
                                {
                                    Connection = ConnectionStatus.Disconnected;
                                }
                            }
                            await Task.Delay(2000, token); // Faster check for debugging
                        }
                    }, token);

                    // 3. Message loop
                    OnLogReceived(LogLevel.Debug, "[CONN] Entering Primary Message iteration...");
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
                if (rootList != null && rootList.Count >= 2)
                {
                    string ceid = rootList.Items[1].GetString();
                    string eventName = _dictionary?.GetEventName(ceid) ?? ceid;
                    OnLogReceived(LogLevel.Info, $"[EVENT] {eventName} (CEID:{ceid})");
                }
                await message.TryReplyAsync(new SecsMessage(6, 12) { SecsItem = Item.B(0) });
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"[ERROR] S6F11: {ex.Message}"); }
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
                    bool isSet = (alcd & 0x80) != 0;
                    OnLogReceived(LogLevel.Info, $"[ALARM] {(isSet ? "SET" : "CLEAR")}: {altx}");
                    State = isSet ? EquipmentState.Alarm : EquipmentState.Idle;
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
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Debug, $"[CONN] S1F13 attempt failed: {ex.Message}");
            }
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
