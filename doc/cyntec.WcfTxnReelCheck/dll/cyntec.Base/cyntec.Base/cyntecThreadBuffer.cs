using System.Collections.Generic;

namespace cyntec.Base;

public class cyntecThreadBuffer<T> : cyntecThread
{
	private Queue<T> m_Buffer = new Queue<T>();

	public Queue<T> Buffer => m_Buffer;

	public int BufferCount
	{
		get
		{
			lock (m_Buffer)
			{
				return m_Buffer.Count;
			}
		}
	}

	public override event ThreadProcessEvent onProcess;

	public cyntecThreadBuffer()
	{
		base.onProcess -= cyntecThreadBuffer_onProcess;
		base.onProcess += cyntecThreadBuffer_onProcess;
	}

	private void cyntecThreadBuffer_onProcess()
	{
		if (m_Buffer.Count > 0 && onProcess != null)
		{
			onProcess();
		}
	}

	public void Enqueue(T obj)
	{
		lock (m_Buffer)
		{
			m_Buffer.Enqueue(obj);
		}
	}

	public T Dequeue()
	{
		lock (m_Buffer)
		{
			return m_Buffer.Dequeue();
		}
	}
}
