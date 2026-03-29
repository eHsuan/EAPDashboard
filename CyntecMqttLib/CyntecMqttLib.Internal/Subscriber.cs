using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CyntecMqttLib.Services;
using MQTTnet;
using MQTTnet.Client;

namespace CyntecMqttLib.Internal;

internal class Subscriber
{
	private readonly MqttConnection _conn;

	private readonly Dictionary<string, List<Action<string, string>>> _handlers = new Dictionary<string, List<Action<string, string>>>();

	private bool _handlerRegistered = false;

	private readonly object _lock = new object();

	public Subscriber(MqttConnection conn)
	{
		_conn = conn;
	}

	public async Task SubscribeAsync(string topic, Action<string, string> handler)
	{
		EnsureHandlerRegistered();
		lock (_lock)
		{
			if (!_handlers.TryGetValue(topic, out var list))
			{
				list = new List<Action<string, string>>();
				_handlers[topic] = list;
			}
			list.Add(handler);
		}
		await MqttClientExtensions.SubscribeAsync(_conn.Client, topic);
	}

	private void EnsureHandlerRegistered()
	{
		if (_handlerRegistered)
		{
			return;
		}
		lock (_lock)
		{
			if (_handlerRegistered)
			{
				return;
			}
			MqttClientExtensions.UseApplicationMessageReceivedHandler(_conn.Client, (Action<MqttApplicationMessageReceivedEventArgs>)delegate(MqttApplicationMessageReceivedEventArgs e)
			{
				string topic = e.ApplicationMessage.Topic;
				byte[] payload = e.ApplicationMessage.Payload;
				string arg = ((payload != null) ? Encoding.UTF8.GetString(payload) : string.Empty);
				List<Action<string, string>> list;
				lock (_lock)
				{
					list = new List<Action<string, string>>();
					foreach (KeyValuePair<string, List<Action<string, string>>> current in _handlers)
					{
						if (MqttTopicMatcher.IsMatch(topic, current.Key))
						{
							list.AddRange(current.Value.ToArray());
						}
					}
				}
				foreach (Action<string, string> current2 in list)
				{
					try
					{
						current2(topic, arg);
					}
					catch
					{
					}
				}
			});
			_handlerRegistered = true;
		}
	}
}
