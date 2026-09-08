using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace cyntec.Base;

public class cyntecThread : IcyntecThread
{
	public static bool IS_STOP = false;

	private static Dictionary<string, cyntecThread> m_ThreadList = new Dictionary<string, cyntecThread>();

	private string m_ID = Guid.NewGuid().ToString();

	public string ParentID = Guid.NewGuid().ToString();

	public string Description;

	private bool m_IS_STOP = true;

	private Thread m_Thread;

	private bool m_Is_Abort = false;

	private bool m_IsInProcess = false;

	public string m_Name;

	private ThreadPriority m_Priority;

	public static Dictionary<string, cyntecThread> ThreadList => m_ThreadList;

	public string ID => m_ID;

	public int SleepTime { get; set; }

	public bool IsInProcess
	{
		get
		{
			return m_IsInProcess;
		}
		set
		{
			m_IsInProcess = value;
		}
	}

	public string Name
	{
		get
		{
			return m_Name;
		}
		set
		{
			m_Name = value;
		}
	}

	public ThreadPriority Priority
	{
		get
		{
			return m_Priority;
		}
		set
		{
			m_Priority = value;
		}
	}

	public virtual event ThreadProcessEvent onProcess;

	public event ThreadProcessXEvent onProcessX;

	public event ThreadStopEvent onStop;

	public event ThreadErrorEvent onError;

	public static void AbortAll()
	{
		while (m_ThreadList.Count > 0)
		{
			m_ThreadList.First().Value.Abort();
		}
	}

	public static void Sleep(int milliSeconds)
	{
		Thread.Sleep(milliSeconds);
	}

	public cyntecThread()
	{
		m_ThreadList.Add(ID, this);
	}

	~cyntecThread()
	{
		if (m_ThreadList.ContainsKey(ID))
		{
			m_ThreadList.Remove(ID);
		}
	}

	public void Abort()
	{
		m_Is_Abort = true;
		try
		{
			if (m_Thread != null)
			{
				bool isAlive = m_Thread.IsAlive;
				m_Thread.Abort();
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
		finally
		{
			if (m_ThreadList.ContainsKey(ID))
			{
				m_ThreadList.Remove(ID);
			}
		}
	}

	public void Start()
	{
		if (m_IS_STOP && m_Thread != null)
		{
			try
			{
				m_Thread.Abort();
			}
			catch
			{
			}
		}
		if (m_IS_STOP)
		{
			ThreadStart start = m_Process;
			m_Thread = new Thread(start);
			m_Thread.Name = m_Name;
			m_Thread.Priority = Priority;
			m_Thread.Start();
			m_IS_STOP = false;
		}
	}

	public void Stop()
	{
		m_IS_STOP = true;
		if (this.onStop != null)
		{
			this.onStop();
		}
	}

	private void m_Process()
	{
		while (true)
		{
			try
			{
				if (m_IS_STOP || IS_STOP)
				{
					break;
				}
				Thread.Sleep(SleepTime + 1);
				m_IsInProcess = true;
				if (this.onProcess != null)
				{
					this.onProcess();
				}
				if (this.onProcessX != null)
				{
					this.onProcessX(this);
				}
				m_IsInProcess = false;
			}
			catch (Exception ex)
			{
				if (this.onError != null)
				{
					this.onError(ex.Message);
				}
			}
			finally
			{
				m_IsInProcess = false;
			}
		}
	}
}
