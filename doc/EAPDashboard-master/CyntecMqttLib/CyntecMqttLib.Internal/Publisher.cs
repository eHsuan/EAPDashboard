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

	public Task PublishAsync(string topic, object payload)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		MqttApplicationMessage msg = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(JsonUtil.Serialize(payload)).WithQualityOfServiceLevel((MqttQualityOfServiceLevel)1)
			.Build();
		return MqttClientExtensions.PublishAsync(_conn.Client, msg);
	}
}
