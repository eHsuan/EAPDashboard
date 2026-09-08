using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using cyntec.Base;

namespace cyntec.Tools;

public class CyntecSocketBase : IcyntecSocket
{
	public Socket mySocket = null;

	private cyntecThread m_Thread;

	private bool m_IsNeedInit = true;

	public Encoding encoding { get; set; }

	public byte[] IP { get; set; }

	public int port { get; set; }

	public int SendTimeout { get; set; }

	public int ReceiveTimeout { get; set; }

	public DateTime LastActiveTime { get; set; }

	public int IdleTimeout { get; set; }

	public string name { get; set; }

	public object tag { get; set; }

	public string sessionID { get; set; }

	public int HeartbeatTime { get; set; }

	public event HOnSocketError onError;

	public event HOnSocketError onDisconnected;

	public event HOnSocketMessageArrive onMessageArrive;

	public event HOnSocketSend onHeartbeat;

	public event HOnSocketSend onMessageSend;

	public void Close()
	{
		if (m_Thread != null)
		{
			m_Thread.Stop();
		}
		if (mySocket != null)
		{
			mySocket.Close();
			mySocket.Dispose();
		}
		cyntecThread.Sleep(5);
	}

	public CyntecSocketBase()
	{
		encoding = Encoding.UTF8;
		HeartbeatTime = 60000;
	}

	public void Init()
	{
		m_IsNeedInit = false;
		Close();
		IPAddress address = new IPAddress(IP);
		IPEndPoint remoteEP = new IPEndPoint(address, port);
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket.SendTimeout = SendTimeout;
		socket.ReceiveTimeout = ReceiveTimeout;
		mySocket = socket;
		try
		{
			mySocket.Connect(remoteEP);
		}
		catch (Exception ex)
		{
			m_IsNeedInit = true;
			if (this.onError != null)
			{
				this.onError(this, ex.Message);
			}
		}
		m_Thread = new cyntecThread();
		m_Thread.Name = "CyntecSocketBase.m_Thread_onProcess";
		m_Thread.Description = "create on Init()";
		m_Thread.SleepTime = 1;
		m_Thread.onProcess += m_Thread_onProcess;
		m_Thread.Start();
	}

	private void m_Thread_onProcess()
	{
		int num = -1;
		List<byte> list = new List<byte>();
		try
		{
			if (mySocket.Available == 0)
			{
				if (HeartbeatTime != 0 && (DateTime.Now - LastActiveTime).TotalMilliseconds > (double)HeartbeatTime && this.onHeartbeat != null)
				{
					this.onHeartbeat(this, null);
				}
				return;
			}
			do
			{
				byte[] array = new byte[65536];
				num = mySocket.Receive(array);
				Array.Resize(ref array, num);
				list.AddRange(array);
			}
			while (mySocket.Available != 0);
			LastActiveTime = DateTime.Now;
		}
		catch (SocketException ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "Socket接收異常, " + sessionID + Environment.NewLine + ex.Message);
			}
			m_Thread.Abort();
			m_IsNeedInit = true;
			cyntecThread.Sleep(1000);
			return;
		}
		catch (Exception ex2)
		{
			if (this.onError != null)
			{
				this.onError(this, "Socket接收異常, " + ex2.Message);
			}
			return;
		}
		if (mySocket.Poll(1, SelectMode.SelectRead) && mySocket.Available == 0)
		{
			m_IsNeedInit = true;
			m_Thread.Stop();
			m_Thread = null;
			mySocket = null;
			if (this.onDisconnected != null)
			{
				this.onDisconnected(this, "disconnected");
			}
		}
		else if (num > 0 && this.onMessageArrive != null)
		{
			this.onMessageArrive(this, list.ToArray());
		}
	}

	public void Init(Socket o)
	{
		m_IsNeedInit = false;
		mySocket = o;
		if (o != null)
		{
			o.SendTimeout = SendTimeout;
			o.ReceiveTimeout = ReceiveTimeout;
		}
		m_Thread = new cyntecThread();
		m_Thread.Name = o.LocalEndPoint.ToString() + "," + o.Handle + ", CyntecSocketBase.m_Thread_onProcess";
		m_Thread.Description = "create on Init(System.Net.Sockets.Socket o)";
		m_Thread.SleepTime = 1;
		m_Thread.onProcess += m_Thread_onProcess;
		m_Thread.Start();
		LastActiveTime = DateTime.Now;
	}

	public void Send(string msg)
	{
		byte[] bytes = encoding.GetBytes(msg);
		Send(bytes);
	}

	public void Send(Encoding encoding, string msg)
	{
		byte[] bytes = encoding.GetBytes(msg);
		Send(bytes);
	}

	public void Send(byte[] msg)
	{
		LastActiveTime = DateTime.Now;
		try
		{
			if (m_IsNeedInit)
			{
				Init();
			}
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "Socket發送初始化異常, " + ex.Message);
			}
			return;
		}
		try
		{
			if (this.onMessageSend != null)
			{
				this.onMessageSend(this, msg);
			}
			mySocket.Send(msg);
		}
		catch (SocketException ex2)
		{
			if (this.onError != null)
			{
				this.onError(this, "socket發送異常, " + sessionID + ", " + Environment.NewLine + ex2.Message);
			}
		}
		catch (Exception ex3)
		{
			m_IsNeedInit = true;
			if (this.onError != null)
			{
				this.onError(this, ex3.Message);
			}
		}
	}
}
