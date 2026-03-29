using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace CyntecMqttLib.Services;

internal class MqttConnection : IDisposable
{
	private readonly IMqttClient _client;

	private readonly IMqttClientOptions _options;

	private CancellationTokenSource _reconnectCts;

	public IMqttClient Client => _client;

	public bool IsConnected => _client.IsConnected;

	public event Action Connected;
	public event Action Disconnected;

	public MqttConnection(string host, int port, string clientId, string username, string password)
	{
		MqttFactory factory = new MqttFactory();
		_client = factory.CreateMqttClient();
		_options = new MqttClientOptionsBuilder().WithTcpServer(host, (int?)port).WithClientId(clientId ?? Guid.NewGuid().ToString()).WithCredentials(username, password)
			.Build();
		
		_client.UseDisconnectedHandler(async e =>
		{
			Disconnected?.Invoke();
			await StartReconnect();
		});

		_client.UseConnectedHandler(e =>
		{
			Connected?.Invoke();
		});
	}

	public async Task ConnectAsync()
	{
		if (!_client.IsConnected)
		{
			await _client.ConnectAsync(_options, CancellationToken.None);
		}
	}

	private async Task StartReconnect()
	{
		if (_reconnectCts != null)
		{
			return;
		}
		_reconnectCts = new CancellationTokenSource();
		try
		{
			while (!_reconnectCts.Token.IsCancellationRequested)
			{
				try
				{
					if (!_client.IsConnected)
					{
						await ConnectAsync();
						if (_client.IsConnected)
						{
							break; // 連線成功，跳出重連迴圈
						}
					}
					else
					{
						break;
					}
				}
				catch
				{
					// 連線失敗，等待下次重試
				}
				await Task.Delay(3000, _reconnectCts.Token);
			}
		}
		catch (TaskCanceledException) { }
		finally
		{
			_reconnectCts?.Dispose();
			_reconnectCts = null;
		}
	}

	public async Task DisconnectAsync()
	{
		_reconnectCts?.Cancel();
		if (_client.IsConnected)
		{
			await _client.DisconnectAsync();
		}
	}

	public void Dispose()
	{
		_reconnectCts?.Cancel();
		_client?.Dispose();
	}
}
