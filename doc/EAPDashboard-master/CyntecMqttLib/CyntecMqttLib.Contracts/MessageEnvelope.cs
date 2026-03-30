using System;

namespace CyntecMqttLib.Contracts;

/// <summary>
/// MQTT 訊息標準封包（強制）
/// </summary>
public class MessageEnvelope
{
	public string MessageId { get; set; }

	public DateTime TimestampUtc { get; set; }

	public object Payload { get; set; }

	public static MessageEnvelope Create<T>(T payload)
	{
		return new MessageEnvelope
		{
			MessageId = Guid.NewGuid().ToString(),
			TimestampUtc = DateTime.UtcNow,
			Payload = payload
		};
	}
}
