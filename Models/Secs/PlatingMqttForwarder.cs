using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;

namespace EAPDashboard.Models.Secs
{
    /// <summary>
    /// PL0084 專用 MQTT 轉發器，負責將電鍍槽實時數據、機台狀態及警報轉發至外部 MQTT Broker (192.168.1.245:30013)
    /// 內建隔離防護與超時控制，確保外部網路異常不影響主程式運作。
    /// </summary>
    public class PlatingMqttForwarder
    {
        private readonly string _broker;
        private readonly int _port;
        private readonly string _clientId;

        public PlatingMqttForwarder(string broker = "192.168.1.245", int port = 30013, string? clientId = null)
        {
            _broker = broker;
            _port = port;
            _clientId = clientId ?? ("EAP_PL0084_" + Guid.NewGuid().ToString("N").Substring(0, 8));
        }

        public async Task PublishAsync(string topic, string payload)
        {
            try
            {
                var factory = new MqttFactory();
                using var client = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(_broker, _port)
                    .WithClientId(_clientId)
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(5))
                    .Build();

                await client.ConnectAsync(options);

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(payload ?? string.Empty))
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await client.PublishAsync(message);
                await client.DisconnectAsync();
            }
            catch
            {
                // 轉發異常隔離，不向上拋出
            }
        }
    }
}
