using System;

namespace cyntec.Tools;

public class cyntecLogFunction
{
	public cyntecLog m_log;

	public string strTag = "";

	public void init(cyntecLog log, string tag)
	{
		m_log = log;
		strTag = tag;
	}

	public virtual void LogDebug(string msg)
	{
		if (m_log.IS_LOG_DEBUG)
		{
			m_log.mWriteLog(msg, strTag + "DEBUG");
		}
	}

	public virtual void LogError(Exception ex)
	{
		if (m_log.IS_LOG_ERROR)
		{
			Exception ex2 = ex.InnerException;
			if (ex2 == null)
			{
				ex2 = ex;
			}
			m_log.mWriteLog(ex2.Message + "\n" + ex2.StackTrace, strTag + "ERROR");
		}
	}

	public virtual void LogError(string msg)
	{
		if (m_log.IS_LOG_ERROR)
		{
			m_log.mWriteLog(msg, strTag + "ERROR");
		}
	}

	public virtual void LogErrorMustShow(string msg)
	{
		if (!m_log.IS_REPLY_CONSOLE)
		{
			Console.WriteLine(msg);
		}
		LogError(msg);
	}

	public virtual void LogInfo(string msg)
	{
		if (m_log.IS_LOG_INFO)
		{
			m_log.mWriteLog(msg, strTag + "INFO");
		}
	}

	public virtual void LogWarn(string msg)
	{
		if (m_log.IS_LOG_WARING)
		{
			m_log.mWriteLog(msg, strTag + "WARING");
		}
	}

	public virtual void LogSQL(string msg)
	{
		if (m_log.IS_LOG_SQL)
		{
			m_log.mWriteLog(msg, strTag + "SQL");
		}
	}
}
