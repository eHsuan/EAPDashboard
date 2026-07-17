using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Secs4Net;

namespace Protocol.Drivers.Secs
{
    public enum ConnectionStatus { Disconnected, Connecting, Connected }
    public enum LogLevel { Debug, Info, Warn, Error, Fatal }

    public class LogMessageEventArgs : EventArgs
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public ConnectionStatus NewStatus { get; set; }
    }

    public class PrimaryMessageEventArgs : EventArgs
    {
        public PrimaryMessageWrapper Wrapper { get; set; }
        public PrimaryMessageEventArgs(PrimaryMessageWrapper wrapper) => Wrapper = wrapper;
    }

    public class SecsClient : IDisposable
    {
        private CancellationTokenSource? _linkedCts;
        private ISecsGem? _secsGem;
        private System.Timers.Timer? _retryTimer;
        private System.Timers.Timer? _aliveCheckTimer;
        private bool _aliveCheckAwaitingReply = false;

        private readonly SemaphoreSlim _connLock = new SemaphoreSlim(1, 1);
        private CancellationTokenSource? _connectionCts;

        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public ushort DeviceId { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string TimeFormat { get; set; } = "16";
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        private ConnectionStatus _connection = ConnectionStatus.Disconnected;
        public ConnectionStatus Connection
        {
            get => _connection;
            private set
            {
                if (_connection != value)
                {
                    _connection = value;
                    ConnectionStatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs { NewStatus = value });
                }
            }
        }

        public event EventHandler<LogMessageEventArgs>? LogOccurred;
        public event EventHandler<ConnectionStatusChangedEventArgs>? ConnectionStatusChanged;
        public event EventHandler<PrimaryMessageEventArgs>? PrimaryMessageReceived;

        public void Start(CancellationToken externalCancellationToken)
        {
            try
            {
                OnLog(LogLevel.Info, $"[START] SECS client initialized for {Name} ({Id}).");
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken);

                // 啟動主動連線偵測 Loop
                _retryTimer = new System.Timers.Timer(15000);
                _retryTimer.Elapsed += (s, e) => { if (Connection == ConnectionStatus.Disconnected && _linkedCts != null) _ = RunConnectionTask(_linkedCts.Token); };
                _retryTimer.AutoReset = true;
                _retryTimer.Enabled = true;

                // 啟動應用層 S1F1 存活檢測計時器
                _aliveCheckTimer = new System.Timers.Timer(30000);
                _aliveCheckTimer.Elapsed += async (s, e) => await SendS1F1();
                _aliveCheckTimer.AutoReset = true;
                _aliveCheckTimer.Enabled = true;

                _ = RunConnectionTask(_linkedCts.Token);
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Fatal, $"[CRITICAL] SECS Client Start Crash: {ex.Message}");
            }
        }

        public void Stop()
        {
            _retryTimer?.Stop();
            _retryTimer?.Dispose();
            _aliveCheckTimer?.Stop();
            _aliveCheckTimer?.Dispose();
            _linkedCts?.Cancel();
            _connectionCts?.Cancel();
            _connectionCts?.Dispose();
            Connection = ConnectionStatus.Disconnected;
        }

        public async Task<SecsMessage> SendAsync(SecsMessage message)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null)
                throw new InvalidOperationException("SECS client is not connected.");
            return await _secsGem.SendAsync(message);
        }

        private async Task SendS1F1()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return;

            if (_aliveCheckAwaitingReply)
            {
                OnLog(LogLevel.Error, "[ALIVE_CHECK] S1F1 response timeout (30s). Equipment software application frozen. Forcing reconnect...");
                _aliveCheckAwaitingReply = false;
                Connection = ConnectionStatus.Disconnected;
                _connectionCts?.Cancel(); // 關閉舊 Socket 與線程
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
                OnLog(LogLevel.Warn, $"[ALIVE_CHECK] Send S1F1 failed: {ex.Message}. Disconnecting...");
                _aliveCheckAwaitingReply = false;
                Connection = ConnectionStatus.Disconnected;
            }
        }

        private async Task RunConnectionTask(CancellationToken token)
        {
            if (!await _connLock.WaitAsync(0)) return;
            try
            {
                if (Connection != ConnectionStatus.Disconnected) return;

                _connectionCts?.Cancel();
                _connectionCts?.Dispose();
                _connectionCts = CancellationTokenSource.CreateLinkedTokenSource(token);
                var localToken = _connectionCts.Token;

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
                        while (!localToken.IsCancellationRequested && _secsGem != null)
                        {
                            if (connection.State != lastState)
                            {
                                OnLog(LogLevel.Info, $"[HSMS] State: {connection.State}");
                                lastState = connection.State;
                                if (connection.State == ConnectionState.Selected) await SendS1F13(gem);
                                else if ((int)connection.State == 0)
                                {
                                    Connection = ConnectionStatus.Disconnected;
                                    _connectionCts?.Cancel();
                                }
                            }
                            await Task.Delay(1000, localToken);
                        }
                    }, localToken);

                    _ = Task.Run(async () => {
                        try
                        {
                            await foreach (var wrapper in gem.GetPrimaryMessageAsync(localToken))
                            {
                                using (wrapper.PrimaryMessage)
                                {
                                    if (Connection != ConnectionStatus.Connected) Connection = ConnectionStatus.Connected;
                                    PrimaryMessageReceived?.Invoke(this, new PrimaryMessageEventArgs(wrapper));
                                }
                            }
                        }
                        catch { }
                    }, localToken);

                    connection.Start(localToken);
                    await Task.Delay(-1, localToken);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[HSMS] Connection task error: {ex.Message}");
            }
            finally
            {
                Connection = ConnectionStatus.Disconnected;
                _secsGem = null;
                _connLock.Release();
            }
        }

        private async Task SendS1F13(ISecsGem gem)
        {
            try
            {
                using (var reply = await gem.SendAsync(SecsMessages.S1F13()))
                {
                    if (reply.S == 1 && reply.F == 14 && reply.SecsItem.Items[0].FirstValue<byte>() == 0)
                    {
                        Connection = ConnectionStatus.Connected;
                        OnLog(LogLevel.Info, "[HSMS] Communication Established. Running initialization sequence...");
                        _ = Task.Run(async () => { await RunInitializationSequenceAsync(); });
                    }
                }
            }
            catch { }
        }

        public async Task<bool> SendS1F17()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLog(LogLevel.Info, "[S1F17] Sending Request ON-LINE...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S1F17()))
                {
                    if (reply.S == 1 && reply.F == 18 && reply.SecsItem != null)
                    {
                        byte onlack = reply.SecsItem.FirstValue<byte>();
                        OnLog(LogLevel.Info, $"[S1F18] Request ON-LINE Acknowledge: {onlack}");
                        return onlack == 0 || onlack == 2;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[S1F17] Send S1F17 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F31()
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                string formatStr = "yyyyMMddHHmmss00";
                if (TimeFormat == "14") formatStr = "yyyyMMddHHmmss";
                else if (TimeFormat == "12") formatStr = "yyMMddHHmmss";

                string hostTime = DateTime.Now.ToString(formatStr);
                OnLog(LogLevel.Info, $"[S2F31] Sending Date and Time Set ({hostTime})...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F31(hostTime)))
                {
                    if (reply.S == 2 && reply.F == 32 && reply.SecsItem != null)
                    {
                        byte tiack = reply.SecsItem.FirstValue<byte>();
                        OnLog(LogLevel.Info, $"[S2F32] Date and Time Set Acknowledge: {tiack}");
                        return tiack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[S2F31] Send S2F31 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F37(bool enable)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLog(LogLevel.Info, $"[S2F37] Sending Enable Events (CEED:{enable}, ALL)...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F37(enable)))
                {
                    if (reply.S == 2 && reply.F == 38 && reply.SecsItem != null)
                    {
                        byte erack = reply.SecsItem.FirstValue<byte>();
                        OnLog(LogLevel.Info, $"[S2F38] Enable Events Acknowledge: {erack}");
                        return erack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[S2F37] Send S2F37 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS5F3(bool enable)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLog(LogLevel.Info, $"[S5F3] Sending Enable Alarms (ALED:{enable}, ALL)...");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S5F3(enable)))
                {
                    if (reply.S == 5 && reply.F == 4 && reply.SecsItem != null)
                    {
                        byte alack = reply.SecsItem.FirstValue<byte>();
                        OnLog(LogLevel.Info, $"[S5F4] Enable Alarms Acknowledge: {alack}");
                        return alack == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[S5F3] Send S5F3 Error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendS2F23Internal(string trid, uint dsper, uint tots, List<string> svids)
        {
            if (Connection != ConnectionStatus.Connected || _secsGem == null) return false;
            try
            {
                OnLog(LogLevel.Info, $"[S2F23] Initializing Trace (TRID:{trid}, DSPER:{dsper}, TOTS:{tots})");
                using (var reply = await _secsGem.SendAsync(SecsMessages.S2F23(trid, dsper, tots, svids)))
                {
                    if (reply.S == 2 && reply.F == 24 && reply.SecsItem != null)
                    {
                        byte checkVal = reply.SecsItem.FirstValue<byte>();
                        OnLog(LogLevel.Info, $"[S2F24] Trace Initialize Ack: {checkVal}");
                        return checkVal == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[S2F23] Send S2F23 Error: {ex.Message}");
            }
            return false;
        }

        private async Task RunInitializationSequenceAsync()
        {
            try
            {
                OnLog(LogLevel.Info, "[INIT] Starting SECS/GEM Initialization Sequence...");

                // Step 1: S1F17 (Request ON-LINE)
                await Task.Delay(300);
                bool onlineOk = await SendS1F17();
                if (!onlineOk) OnLog(LogLevel.Warn, "[INIT] S1F17 (Request ON-LINE) failed or rejected. Continuing sequence...");

                // Step 2: S2F31 (Date and Time Set)
                await Task.Delay(300);
                bool timeOk = await SendS2F31();
                if (!timeOk) OnLog(LogLevel.Warn, "[INIT] S2F31 (Date & Time Set) failed or rejected. Continuing sequence...");

                // Step 3: S2F37 (Enable Events)
                await Task.Delay(300);
                bool eventsOk = await SendS2F37(true);
                if (!eventsOk) OnLog(LogLevel.Warn, "[INIT] S2F37 (Enable Events) failed or rejected. Continuing sequence...");

                // Step 4: S5F3 (Enable Alarms)
                await Task.Delay(300);
                bool alarmsOk = await SendS5F3(true);
                if (!alarmsOk) OnLog(LogLevel.Warn, "[INIT] S5F3 (Enable Alarms) failed or rejected. Continuing sequence...");

                // Step 5: S2F23 (Trace Initialize TOTS=0)
                await Task.Delay(300);
                bool traceOk = await SendS2F23Internal("1", 0, 0, new List<string>());
                if (traceOk) OnLog(LogLevel.Info, "[INIT] S2F23 Trace initialization cleanup complete.");
                else OnLog(LogLevel.Warn, "[INIT] S2F23 Trace initialization cleanup failed. Continuing...");

                OnLog(LogLevel.Info, "[INIT] SECS/GEM Initialization Sequence completed.");
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[INIT] Error in Initialization Sequence: {ex.Message}");
            }
        }

        private void OnLog(LogLevel level, string message)
        {
            LogOccurred?.Invoke(this, new LogMessageEventArgs { Level = level, Message = message });
        }

        public void Dispose()
        {
            Stop();
            _connLock.Dispose();
            GC.SuppressFinalize(this);
        }

        private class SecsLogger : ISecsGemLogger
        {
            private readonly SecsClient _client;
            public SecsLogger(SecsClient client) => _client = client;
            public void MessageIn(SecsMessage m, int id) => _client.OnLog(LogLevel.Info, $"[<-] S{m.S}F{m.F}");
            public void MessageOut(SecsMessage m, int id) => _client.OnLog(LogLevel.Info, $"[->] S{m.S}F{m.F}");
            public void Debug(string m) => _client.OnLog(LogLevel.Debug, $"[Secs4Net] {m}");
            public void Info(string m) => _client.OnLog(LogLevel.Info, $"[Secs4Net] {m}");
            public void Warning(string m) => _client.OnLog(LogLevel.Warn, $"[Secs4Net] {m}");
            public void Error(string m) => _client.OnLog(LogLevel.Error, $"[Secs4Net] {m}");
            public void Error(string m, Exception ex) => _client.OnLog(LogLevel.Error, $"[Secs4Net] {m} {ex?.Message}");
            public void Error(string m, SecsMessage msg, Exception ex) => _client.OnLog(LogLevel.Error, $"[Secs4Net] {m} S{msg?.S}F{msg?.F} {ex?.Message}");
        }
    }
}
