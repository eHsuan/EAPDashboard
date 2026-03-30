using System;
using System.Threading.Tasks;
using CyntecMqttLib.Contracts;
using CyntecMqttLib.Internal;
using CyntecMqttLib.Services;

namespace CyntecMqttLib.Interfaces;

/// <summary>
/// MQTT 標準通訊匯流排
/// Client / Server 皆使用此類別
/// </summary>
public class MqttBus : IDisposable
{
	private readonly MqttConnection _connection;

	private readonly Publisher _publisher;

	private readonly Subscriber _subscriber;

	public bool IsConnected => _connection.IsConnected;

	public event Action Connected
	{
		add { _connection.Connected += value; }
		remove { _connection.Connected -= value; }
	}

	public event Action Disconnected
	{
		add { _connection.Disconnected += value; }
		remove { _connection.Disconnected -= value; }
	}

	public MqttBus(string host, int port = 1883, string clientId = null, string username = null, string password = null)
	{
		_connection = new MqttConnection(host, port, clientId, username, password);
		_publisher = new Publisher(_connection);
		_subscriber = new Subscriber(_connection);
	}

	public Task ConnectAsync()
	{
		return _connection.ConnectAsync();
	}

	public Task DisconnectAsync()
	{
		return _connection.DisconnectAsync();
	}

	public Task PublishAsync<T>(string topic, T payload)
	{
		MessageEnvelope envelope = MessageEnvelope.Create(payload);
		return _publisher.PublishAsync(topic, envelope);
	}

	public Task SubscribeAsync(string topic, Action<string, string> handler)
	{
		return _subscriber.SubscribeAsync(topic, handler);
	}

	public void Dispose()
	{
		_connection.Dispose();
	}
}
