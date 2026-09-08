using System;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSocketRS232 : IcyntecSocket
{
	private cyntecSerialPortByte m_RS232;

	public string PortName = "COM1";

	public int BaudRate = 9600;

	public StopBits stopBits = StopBits.One;

	public Parity parity = Parity.None;

	private bool m_IsNeedInit = true;

	public Encoding encoding { get; set; }

	public byte[] IP { get; set; }

	public int port { get; set; }

	public int SendTimeout { get; set; }

	public int ReceiveTimeout { get; set; }

	public DateTime LastActiveTime { get; set; }

	public string name { get; set; }

	public object tag { get; set; }

	public string sessionID { get; set; }

	public int HeartbeatTime { get; set; }

	public event HOnSocketError onError;

	public event HOnSocketError onDisconnected;

	public event HOnSocketMessageArrive onMessageArrive;

	public event HOnSocketSend onMessageSend;

	public event HOnSocketSend onHeartbeat;

	public void Close()
	{
		if (m_RS232 != null)
		{
			m_RS232.Close();
		}
		cyntecThread.Sleep(5);
	}

	public cyntecSocketRS232()
	{
		encoding = Encoding.UTF8;
	}

	public void Init()
	{
		m_IsNeedInit = false;
		m_RS232 = new cyntecSerialPortByte();
		m_RS232.PortName = PortName;
		m_RS232.BaudRate = BaudRate;
		m_RS232.StopBits = StopBits.One;
		m_RS232.Parity = Parity.None;
		m_RS232.onMessageArrive += m_RS232_onMessageArrive;
		m_RS232.onError += m_RS232_onError;
		m_RS232.Open();
	}

	private void m_RS232_onError(object sender, string message)
	{
		if (this.onError != null)
		{
			this.onError(this, message);
		}
	}

	private void m_RS232_onMessageArrive(byte[] data)
	{
		if (this.onMessageArrive != null)
		{
			this.onMessageArrive(this, data);
		}
	}

	public void Init(Socket o)
	{
		throw new NotImplementedException();
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
				this.onError(this, "RS232發送初始化異常, " + ex.Message);
			}
			return;
		}
		try
		{
			if (this.onMessageSend != null)
			{
				this.onMessageSend(this, msg);
			}
			m_RS232.Write(msg, 0, msg.Length);
		}
		catch (Exception ex2)
		{
			m_IsNeedInit = true;
			if (this.onError != null)
			{
				this.onError(this, ex2.Message);
			}
		}
	}

	public void Send(string msg)
	{
		byte[] bytes = encoding.GetBytes(msg);
		Send(bytes);
	}
}
