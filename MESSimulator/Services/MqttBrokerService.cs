using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

namespace MESSimulator.Services
{
    public class ConnectedClientInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public DateTime ConnectedTime { get; set; } = DateTime.Now;
        public DateTime LastActivityTime { get; set; } = DateTime.Now;
        private readonly HashSet<string> _subscribedTopics = new HashSet<string>(StringComparer.Ordinal);
        private readonly object _topicLock = new object();

        public void AddTopic(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return;
            lock (_topicLock)
            {
                _subscribedTopics.Add(topic);
                LastActivityTime = DateTime.Now;
            }
        }

        public void RemoveTopic(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return;
            lock (_topicLock)
            {
                _subscribedTopics.Remove(topic);
                LastActivityTime = DateTime.Now;
            }
        }

        public List<string> GetTopicsSnapshot()
        {
            lock (_topicLock)
            {
                return _subscribedTopics.OrderBy(t => t).ToList();
            }
        }

        public string SubscribedTopicsDisplay
        {
            get
            {
                var list = GetTopicsSnapshot();
                return list.Count == 0 ? "(無訂閱)" : string.Join(" , ", list);
            }
        }
    }

    public class MqttBrokerService : IDisposable
    {
        private IMqttServer _mqttServer;
        private readonly ConcurrentDictionary<string, ConnectedClientInfo> _clients = new ConcurrentDictionary<string, ConnectedClientInfo>(StringComparer.Ordinal);

        public bool IsRunning { get; private set; }
        public int Port { get; private set; } = 1883;

        public event Action<string> LogMessage;
        public event Action ClientsChanged;

        public async Task StartAsync(int port = 1883)
        {
            if (IsRunning) return;

            Port = port;
            var factory = new MqttFactory();
            _mqttServer = factory.CreateMqttServer();

            // 1. 驗證連線與記錄 Endpoint
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port)
                .WithConnectionValidator(context =>
                {
                    context.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success;
                    var info = _clients.GetOrAdd(context.ClientId, id => new ConnectedClientInfo
                    {
                        ClientId = id,
                        ConnectedTime = DateTime.Now
                    });
                    info.Endpoint = context.Endpoint ?? "Unknown";
                    info.LastActivityTime = DateTime.Now;
                })
                .WithSubscriptionInterceptor(context =>
                {
                    context.AcceptSubscription = true;
                    if (_clients.TryGetValue(context.ClientId, out var info))
                    {
                        info.AddTopic(context.TopicFilter?.Topic);
                    }
                    Log($"[SUBSCRIBE] Client '{context.ClientId}' 訂閱 Topic: {context.TopicFilter?.Topic}");
                    ClientsChanged?.Invoke();
                })
                .WithApplicationMessageInterceptor(context =>
                {
                    context.AcceptPublish = true;
                    if (!string.IsNullOrEmpty(context.ClientId) && _clients.TryGetValue(context.ClientId, out var info))
                    {
                        info.LastActivityTime = DateTime.Now;
                    }
                })
                .Build();

            // 2. Client 連線事件
            _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
            {
                var info = _clients.GetOrAdd(e.ClientId, id => new ConnectedClientInfo
                {
                    ClientId = id,
                    Endpoint = "Connected",
                    ConnectedTime = DateTime.Now,
                    LastActivityTime = DateTime.Now
                });
                Log($"[CONNECT] Client 已連線: {e.ClientId} ({info.Endpoint})");
                ClientsChanged?.Invoke();
            });

            // 3. Client 斷線事件
            _mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e =>
            {
                _clients.TryRemove(e.ClientId, out _);
                Log($"[DISCONNECT] Client 已斷線: {e.ClientId} (Type: {e.DisconnectType})");
                ClientsChanged?.Invoke();
            });

            // 4. Client 訂閱與取消訂閱補充事件 (保險同步)
            _mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(e =>
            {
                if (_clients.TryGetValue(e.ClientId, out var info))
                {
                    info.AddTopic(e.TopicFilter?.Topic);
                }
                ClientsChanged?.Invoke();
            });

            _mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(e =>
            {
                if (_clients.TryGetValue(e.ClientId, out var info))
                {
                    info.RemoveTopic(e.TopicFilter);
                }
                Log($"[UNSUBSCRIBE] Client '{e.ClientId}' 取消訂閱 Topic: {e.TopicFilter}");
                ClientsChanged?.Invoke();
            });

            await _mqttServer.StartAsync(options);
            IsRunning = true;
            Log($"MQTT Broker 已啟動於 Port {port}。");
            ClientsChanged?.Invoke();
        }

        public async Task StopAsync()
        {
            if (!IsRunning || _mqttServer == null) return;

            try
            {
                await _mqttServer.StopAsync();
            }
            catch (Exception ex)
            {
                Log($"[ERR] 停止 MQTT Broker 時發生錯誤: {ex.Message}");
            }
            finally
            {
                IsRunning = false;
                _clients.Clear();
                Log("MQTT Broker 已停止。");
                ClientsChanged?.Invoke();
            }
        }

        public List<ConnectedClientInfo> GetConnectedClientsSnapshot()
        {
            return _clients.Values.OrderBy(c => c.ConnectedTime).ToList();
        }

        private void Log(string msg)
        {
            LogMessage?.Invoke(msg);
        }

        public void Dispose()
        {
            try
            {
                if (IsRunning && _mqttServer != null)
                {
                    _mqttServer.StopAsync().GetAwaiter().GetResult();
                }
                (_mqttServer as IDisposable)?.Dispose();
            }
            catch { }
        }
    }
}
