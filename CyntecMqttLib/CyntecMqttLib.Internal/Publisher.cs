using System.Threading.Tasks;
using CyntecMqttLib.Services;
using CyntecMqttLib.Util;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace CyntecMqttLib.Internal;

internal class Publisher
{
	private readonly MqttConnection _conn;

	public Publisher(MqttConnection conn)
	{
		_conn = conn;
	}

	public Task PublishAsync(string topic, object payload, int qos = 1, bool retain = false)
	{
		MqttApplicationMessage msg = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(JsonUtil.Serialize(payload))
			.WithQualityOfServiceLevel((MqttQualityOfServiceLevel)qos)
			.WithRetainFlag(retain)
			.Build();
		return MqttClientExtensions.PublishAsync(_conn.Client, msg);
	}
}
