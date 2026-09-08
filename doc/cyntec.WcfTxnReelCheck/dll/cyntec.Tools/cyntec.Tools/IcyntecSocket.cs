using System;
using System.Net.Sockets;
using System.Text;

namespace cyntec.Tools;

public interface IcyntecSocket
{
	byte[] IP { get; set; }

	int port { get; set; }

	int SendTimeout { get; set; }

	int ReceiveTimeout { get; set; }

	int HeartbeatTime { get; set; }

	DateTime LastActiveTime { get; set; }

	string name { get; set; }

	object tag { get; set; }

	string sessionID { get; set; }

	Encoding encoding { get; set; }

	event HOnSocketError onError;

	event HOnSocketError onDisconnected;

	event HOnSocketMessageArrive onMessageArrive;

	event HOnSocketSend onMessageSend;

	event HOnSocketSend onHeartbeat;

	void Close();

	void Init();

	void Init(Socket o);

	void Send(byte[] msg);

	void Send(string msg);
}
