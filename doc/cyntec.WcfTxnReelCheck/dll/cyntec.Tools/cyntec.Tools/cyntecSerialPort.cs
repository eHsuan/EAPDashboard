using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSerialPort : SerialPort
{
	public delegate void MessageArrive(string msg);

	private string m_ID = default(Guid).ToString();

	private cyntecThread _oThreadX = null;

	private cyntecThread _NoteReceiveThreadX = null;

	private bool _IsReceiveEnable = false;

	public bool IsAutoRead = true;

	public Queue<string> MessageQueue = new Queue<string>();

	public bool IsReadExisting = false;

	public StringBuilder MsgBuffer = null;

	public string ReplyMsg = "";

	public string ID => m_ID;

	public event HOnError onError;

	public event MessageArrive onMessageArrive;

	~cyntecSerialPort()
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
		if (!_IsReceiveEnable)
		{
			return;
		}
		string text = "";
		try
		{
			if (MsgBuffer == null)
			{
				MsgBuffer = new StringBuilder();
			}
			if (IsReadExisting)
			{
				text = ReadExisting();
				if (text.Length > 0)
				{
					MsgBuffer.Append(text);
				}
				if (!MsgBuffer.ToString().EndsWith(base.NewLine))
				{
					if (MsgBuffer.Length > 4096 && this.onError != null)
					{
						this.onError(this, "串口接收異常,訊息累計超過4096個字元,但沒有接受到結束符號");
					}
					return;
				}
				text = (string.IsNullOrWhiteSpace(base.NewLine) ? MsgBuffer.ToString().Substring(0, MsgBuffer.Length - base.NewLine.Length) : MsgBuffer.ToString());
			}
			else
			{
				text = ReadLine();
			}
			ReplyMsg = text;
			MsgBuffer = null;
			if (this.onError != null)
			{
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (this.onMessageArrive != null)
				{
					this.onMessageArrive(text);
				}
			}
			else if (this.onError == null)
			{
			}
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "_oThreadX_onProcess Exception, " + ex.Message + " (" + GetType().Name + ")");
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
		try
		{
			MessageQueue.Clear();
			DiscardInBuffer();
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "Clear()發生異常, " + ex.Message);
			}
		}
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
			_oThreadX.Stop();
			cyntecThread.Sleep(10);
			_oThreadX.Abort();
			_oThreadX = null;
		}
	}

	public string Read()
	{
		string result = "";
		try
		{
			if (MessageQueue.Count > 0)
			{
				result = MessageQueue.Dequeue();
			}
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "Read()發生異常," + ex.Message);
			}
		}
		return result;
	}
}
