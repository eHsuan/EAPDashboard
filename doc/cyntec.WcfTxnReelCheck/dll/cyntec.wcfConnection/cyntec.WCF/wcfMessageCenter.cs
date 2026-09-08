using System;
using System.Threading;
using cyntec.Base;

namespace cyntec.WCF;

public class wcfMessageCenter : AComponentBasic
{
	private cyntecThreadBuffer<byte[]> m_BroadcastThread = null;

	public readonly string guid = Guid.NewGuid().ToString();

	public event ConnBroadCastDelegate onBroadcast;

	public void Broadcast(IwcfMessage msg)
	{
		byte[] obj = msg.ToByte();
		Broadcast(obj);
	}

	public void Broadcast(byte[] obj)
	{
		if (obj == null)
		{
			m_LogError("wcfMessageCenter Broadcast Broadcast 參數不可為null");
			throw new Exception("Broadcast 參數不可為null");
		}
		try
		{
			lock (m_BroadcastThread)
			{
				m_BroadcastThread.Enqueue(obj);
			}
		}
		catch (Exception ex)
		{
			m_LogError("Broadcast Error" + ex);
		}
	}

	public void Initialize()
	{
		if (m_BroadcastThread != null)
		{
			m_BroadcastThread.Stop();
		}
		else
		{
			m_BroadcastThread = new cyntecThreadBuffer<byte[]>();
			m_BroadcastThread.Name = "wcfIMessageCenter.m_BroadcastThread";
			m_BroadcastThread.Description = "create on Initialize()";
		}
		m_BroadcastThread.Name = "wcfMessageCenter";
		m_BroadcastThread.Priority = ThreadPriority.Normal;
		m_BroadcastThread.onProcess -= m_BroadcastThread_onProcess;
		m_BroadcastThread.onProcess += m_BroadcastThread_onProcess;
		m_BroadcastThread.SleepTime = 1;
		m_BroadcastThread.Start();
	}

	private void m_BroadcastThread_onProcess()
	{
		byte[] array = null;
		try
		{
			lock (m_BroadcastThread)
			{
				while (m_BroadcastThread.BufferCount > 0 && m_BroadcastThread.BufferCount != 0)
				{
					lock (m_BroadcastThread)
					{
						array = m_BroadcastThread.Dequeue();
					}
					if (array == null)
					{
						break;
					}
					if (this.onBroadcast != null)
					{
						this.onBroadcast(null, array);
					}
				}
			}
		}
		catch (Exception ex)
		{
			m_LogError("wcfMessageCenter m_BroadcastThread_onProcess " + ex.Message);
		}
	}

	public void Stop()
	{
		if (m_BroadcastThread != null)
		{
			m_BroadcastThread.Stop();
		}
	}
}
