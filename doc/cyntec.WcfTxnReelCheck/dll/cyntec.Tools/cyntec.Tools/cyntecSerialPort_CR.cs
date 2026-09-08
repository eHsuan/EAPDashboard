using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSerialPort_CR : SerialPort
{
	public delegate void MessageArrive(string msg);

	private string m_ID = default(Guid).ToString();

	private cyntecThread _oThreadX = null;

	private cyntecThread _NoteReceiveThreadX = null;

	private bool _IsReceiveEnable = false;

	public bool IsAutoRead = true;

	public Queue<string> MessageQueue = new Queue<string>();

	public string ID => m_ID;

	public event HOnError onError;

	public event MessageArrive onMessageArrive;

	~cyntecSerialPort_CR()
	{
	}

	private void cynSerialPort_ReadLineEvent(string line)
	{
		Console.WriteLine(line);
	}

	public new void Open()
	{
		base.Open();
		_IsReceiveEnable = true;
		if (_oThreadX == null)
		{
			_oThreadX = new cyntecThread();
			_oThreadX.Name = "cyntecSerialPort._oThreadX_onProcess";
			_oThreadX.Description = "create on Open()";
			_oThreadX.SleepTime = 1;
			_oThreadX.onProcess += _oThreadX_onProcess;
			_oThreadX.Start();
		}
		if (_NoteReceiveThreadX == null)
		{
			_NoteReceiveThreadX = new cyntecThread();
			_NoteReceiveThreadX.Name = "cyntecSerialPort._NoteReceiveThreadX_onProcess";
			_NoteReceiveThreadX.Description = "create on Open()";
			_NoteReceiveThreadX.SleepTime = 1;
			_NoteReceiveThreadX.onProcess += _NoteReceiveThreadX_onProcess;
			_NoteReceiveThreadX.Start();
		}
	}

	private void _NoteReceiveThreadX_onProcess()
	{
		try
		{
			if (IsAutoRead)
			{
				string text = Read();
				if (text != "" && this.onMessageArrive != null)
				{
					this.onMessageArrive(text);
				}
			}
			cyntecThread.Sleep(10);
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, GetType().Name + ", " + ex.Message);
			}
		}
	}

	private void _oThreadX_onProcess()
	{
		string text = "";
		try
		{
			text = ReadLine();
			if (text != "")
			{
				lock (MessageQueue)
				{
					MessageQueue.Enqueue(text);
					return;
				}
			}
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "cyntecSerialPort Error: " + ex.Message + Environment.NewLine);
			}
		}
	}

	private void _NoteReceiveFunction()
	{
		while (_IsReceiveEnable)
		{
			if (IsAutoRead)
			{
				string text = Read();
				if (text != "" && this.onMessageArrive != null)
				{
					this.onMessageArrive(text);
				}
			}
			Thread.Sleep(10);
		}
	}

	public void Clear()
	{
		lock (MessageQueue)
		{
			MessageQueue.Clear();
		}
		DiscardInBuffer();
	}

	public new void Close()
	{
		_IsReceiveEnable = false;
		try
		{
			base.Close();
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			if (this.onError != null)
			{
				this.onError(this, " RS232 Port ID = " + base.PortName + ", Error : " + message);
			}
		}
		if (_NoteReceiveThreadX != null)
		{
			_NoteReceiveThreadX.Abort();
			_NoteReceiveThreadX = null;
		}
		if (_oThreadX != null)
		{
			_oThreadX.Abort();
			_oThreadX = null;
		}
	}

	public string Read()
	{
		string result = "";
		lock (MessageQueue)
		{
			if (MessageQueue.Count > 0)
			{
				result = MessageQueue.Dequeue();
			}
		}
		return result;
	}
}
