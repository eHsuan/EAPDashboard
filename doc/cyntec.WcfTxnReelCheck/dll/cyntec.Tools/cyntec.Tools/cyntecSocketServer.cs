using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSocketServer<T> where T : IcyntecSocket, new()
{
	public byte[] IP;

	public int portNo = -1;

	public IPEndPoint localEndPoint;

	private Socket m_Listener = null;

	public Dictionary<IcyntecSocket, IcyntecSocket> m_ClinetList = new Dictionary<IcyntecSocket, IcyntecSocket>();

	public Encoding Endcoding = Encoding.UTF8;

	public int MaxConnection = 10;

	public int SendTimeOut = 0;

	public int ReceiveTimeOut = 0;

	public int HeartbeatTime = 0;

	public Thread m_Thread = null;

	public bool IsStop = false;

	public bool IsKeepClient = true;

	public string ServerID = Guid.NewGuid().ToString();

	public int SessionCount = 0;

	public event HOnSocketConnected onConnected;

	public event HOnSocketError onError;

	public event HOnSocketMessageArrive onMessageArrive;

	public event HOnSocketError onDisconnected;

	public event HOnSocketSend onHeartbeat;

	public event HOnSocketSend onMessageSend;

	public string IPToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (IP == null)
		{
			return "";
		}
		byte[] iP = IP;
		foreach (byte value in iP)
		{
			stringBuilder.Append(value).Append(".");
		}
		if (stringBuilder.Length > 1)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}

	public void Close()
	{
		IsStop = true;
		if (m_Listener != null)
		{
			m_Listener.Close();
		}
		foreach (IcyntecSocket value in m_ClinetList.Values)
		{
			try
			{
				value.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Close Client error:: " + ex.Message);
			}
		}
		m_Thread = null;
	}

	public void Start()
	{
		if (m_Thread != null)
		{
			return;
		}
		if (IP == null)
		{
			throw new Exception("沒有指定IP");
		}
		if (portNo == -1)
		{
			throw new Exception("沒有指定PortNo");
		}
		IPAddress address = new IPAddress(IP);
		localEndPoint = new IPEndPoint(address, portNo);
		try
		{
			m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_Listener.SendTimeout = SendTimeOut;
			m_Listener.ReceiveTimeout = ReceiveTimeOut;
			m_Listener.Bind(localEndPoint);
			m_Listener.Listen(MaxConnection);
			m_Thread = new Thread(m_Thread_onProcess);
			m_Thread.Start();
		}
		catch (Exception ex)
		{
			throw new Exception("Socket無法開啟, " + IPToString() + ":" + portNo + ", " + ex.Message + "," + ex.StackTrace);
		}
	}

	public void Send(byte[] msg)
	{
		List<IcyntecSocket> list = null;
		lock (m_ClinetList)
		{
			list = m_ClinetList.Values.ToList();
		}
		foreach (IcyntecSocket item in list)
		{
			item.Send(msg);
		}
	}

	private void m_Thread_onProcess()
	{
		while (!IsStop && !cyntecThread.IS_STOP)
		{
			cyntecThread.Sleep(1);
			try
			{
				Socket o = m_Listener.Accept();
				IcyntecSocket icyntecSocket = new T();
				icyntecSocket.SendTimeout = SendTimeOut;
				icyntecSocket.ReceiveTimeout = ReceiveTimeOut;
				icyntecSocket.HeartbeatTime = HeartbeatTime;
				icyntecSocket.onMessageArrive += o_onMessageArrive;
				icyntecSocket.onDisconnected += o_onDisconnected;
				icyntecSocket.onError += o_onError;
				icyntecSocket.onHeartbeat += o_onHeartbeat;
				icyntecSocket.onMessageSend += o_onMessageSend;
				lock (m_ClinetList)
				{
					SessionCount++;
					icyntecSocket.sessionID = ServerID + ":" + SessionCount;
					m_ClinetList.Add(icyntecSocket, icyntecSocket);
				}
				icyntecSocket.Init(o);
				if (this.onConnected != null)
				{
					this.onConnected(icyntecSocket);
				}
			}
			catch (Exception ex)
			{
				if (this.onError != null)
				{
					this.onError(this, GetType().Name + ".m_Thread_onProcess," + ex.Message);
				}
			}
		}
	}

	private void o_onMessageSend(object Sender, byte[] msg)
	{
		if (this.onMessageSend != null)
		{
			this.onMessageSend(Sender, msg);
		}
	}

	private void o_onHeartbeat(object Sender, byte[] msg)
	{
		if (this.onHeartbeat != null)
		{
			this.onHeartbeat(Sender, msg);
		}
	}

	private void o_onDisconnected(object Sender, string msg)
	{
		IcyntecSocket icyntecSocket = Sender as IcyntecSocket;
		lock (m_Listener)
		{
			if (m_ClinetList.ContainsKey(icyntecSocket))
			{
				m_ClinetList.Remove(icyntecSocket);
			}
		}
		if (this.onDisconnected != null)
		{
			this.onDisconnected(icyntecSocket, "disconnected");
		}
	}

	private void o_onError(object Sender, string msg)
	{
		lock (m_ClinetList)
		{
			m_ClinetList.Remove(Sender as IcyntecSocket);
		}
		try
		{
			IcyntecSocket icyntecSocket = Sender as IcyntecSocket;
			icyntecSocket.Close();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		if (this.onError != null)
		{
			this.onError(Sender, msg);
		}
	}

	private void o_onMessageArrive(object Sender, byte[] msg)
	{
		IcyntecSocket icyntecSocket = (IcyntecSocket)Sender;
		if (this.onMessageArrive != null)
		{
			this.onMessageArrive(Sender, msg);
		}
		if (!IsKeepClient)
		{
			m_ClinetList.Remove(icyntecSocket);
			icyntecSocket.Close();
		}
	}
}
