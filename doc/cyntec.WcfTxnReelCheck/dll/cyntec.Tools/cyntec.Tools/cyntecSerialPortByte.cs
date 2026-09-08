using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSerialPortByte : SerialPort
{
	public delegate void MessageArrive(byte[] data);

	private cyntecThread _oThreadX = null;

	private cyntecThread _NoteReceiveThreadX = null;

	private bool _IsReceiveEnable = false;

	public bool IsAutoRead = true;

	public Queue<byte[]> MessageQueue = new Queue<byte[]>();

	public event HOnError onError;

	public event MessageArrive onMessageArrive;

	public new void Open()
	{
		base.Open();
		base.DataReceived += cyntecSerialPortByte_DataReceived;
		_IsReceiveEnable = true;
		if (_NoteReceiveThreadX == null)
		{
			_NoteReceiveThreadX = new cyntecThread();
			_NoteReceiveThreadX.Name = "cyntecSerialPortByte._NoteReceiveThreadX_onProcess";
			_NoteReceiveThreadX.Description = "create on open()";
			_NoteReceiveThreadX.SleepTime = 1;
			_NoteReceiveThreadX.onProcess += _NoteReceiveThreadX_onProcess;
			_NoteReceiveThreadX.Start();
		}
	}

	private void cyntecSerialPortByte_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			int bytesToRead = base.BytesToRead;
			while (true)
			{
				cyntecThread.Sleep(20);
				if (bytesToRead != base.BytesToRead)
				{
					bytesToRead = base.BytesToRead;
					continue;
				}
				break;
			}
			byte[] array = new byte[base.BytesToRead];
			int num = Read(array, 0, base.BytesToRead);
			MessageQueue.Enqueue(array);
		}
		catch (Exception ex)
		{
			if (this.onError != null)
			{
				this.onError(this, "cyntecSerialPortByte_DataReceived, " + ex.Message);
			}
		}
	}

	private void _NoteReceiveThreadX_onProcess()
	{
		if (IsAutoRead)
		{
			byte[] array = Read();
			if (array.Length != 0 && this.onMessageArrive != null)
			{
				this.onMessageArrive(array);
			}
		}
	}

	private void _oThreadX_onProcess()
	{
	}

	private void _NoteReceiveFunction()
	{
		while (_IsReceiveEnable)
		{
			if (IsAutoRead)
			{
				byte[] array = Read();
				if (array.Length != 0 && this.onMessageArrive != null)
				{
					this.onMessageArrive(array);
				}
			}
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
		base.Close();
		Thread.Sleep(100);
	}

	private void _oThreadJob()
	{
		while (_IsReceiveEnable)
		{
			try
			{
				byte[] array = new byte[1024];
				int num = Read(array, 0, 0);
				if (num > 0)
				{
					MessageQueue.Enqueue(array);
				}
				Thread.Sleep(10);
			}
			catch (Exception ex)
			{
				if (this.onError != null)
				{
					this.onError(this, "cyntecSerialPort Error: " + ex.Message + Environment.NewLine);
				}
			}
		}
	}

	public byte[] Read()
	{
		List<byte> list = new List<byte>();
		lock (MessageQueue)
		{
			while (MessageQueue.Count > 0)
			{
				byte[] collection = MessageQueue.Dequeue();
				list.AddRange(collection);
			}
		}
		return list.ToArray();
	}
}
