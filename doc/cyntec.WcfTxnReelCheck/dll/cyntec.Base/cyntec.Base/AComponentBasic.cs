using System;

namespace cyntec.Base;

public abstract class AComponentBasic
{
	public bool IsErrorOnConsol = false;

	public bool IsLogOnConsol = false;

	private object m_Tag;

	public object Tag
	{
		get
		{
			return m_Tag;
		}
		set
		{
			m_Tag = value;
		}
	}

	public virtual string Description => "AComponentBasic";

	public virtual string Remark => "";

	public event HOnError onError;

	public event HOnLog onLog;

	public event HOnLogObj onLogObj;

	protected void m_LogError(object sender, string err)
	{
		if (IsErrorOnConsol)
		{
			Console.WriteLine("異常:: " + err);
		}
		if (this.onError != null)
		{
			this.onError(sender, err);
		}
	}

	protected void m_LogError(string err)
	{
		m_LogError(this, err);
	}

	protected void m_Log(object sender, string msg)
	{
		if (IsLogOnConsol)
		{
			Console.WriteLine(msg);
		}
		if (this.onLog != null)
		{
			this.onLog(sender, msg);
		}
	}

	protected void m_Log(string msg)
	{
		m_Log(this, msg);
	}
}
