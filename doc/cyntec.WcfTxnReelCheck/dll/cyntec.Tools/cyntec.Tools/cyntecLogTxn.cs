using System;

namespace cyntec.Tools;

public class cyntecLogTxn : cyntecLog
{
	public string TxnName = "TxnName";

	public cyntecLog ParentLog = null;

	public void init(cyntecLog oParent, string strTxnName)
	{
		TxnName = strTxnName;
		ParentLog = oParent;
		if (ParentLog.LogKeywordList.ContainsKey(TxnName))
		{
			IS_LOG_DEBUG = true;
			IS_LOG_ERROR = true;
			IS_LOG_INFO = true;
			IS_LOG_SQL = true;
			IS_LOG_WARING = true;
		}
		else
		{
			IS_LOG_DEBUG = ParentLog.IS_LOG_DEBUG;
			IS_LOG_ERROR = ParentLog.IS_LOG_ERROR;
			IS_LOG_INFO = IS_LOG_INFO;
			IS_LOG_SQL = IS_LOG_SQL;
			IS_LOG_WARING = IS_LOG_WARING;
		}
	}

	public override void LogDebug(string keyword, string msg)
	{
		ParentLog.LogDebug(keyword, msg);
	}

	public override void LogError(Exception ex)
	{
		ParentLog.LogError(ex);
	}

	public override void LogDebug(string msg)
	{
		if (IS_LOG_DEBUG)
		{
			ParentLog.mWriteLog(msg, "DEBUG");
		}
	}

	public override void LogError(string keyword, string msg)
	{
		ParentLog.LogError(keyword, msg);
	}

	public override void LogError(string msg)
	{
		ParentLog.LogError(msg);
	}

	public override void LogErrorMustShow(string msg)
	{
		ParentLog.LogErrorMustShow(msg);
	}

	public override void LogInfo(string msg)
	{
		if (IS_LOG_INFO)
		{
			ParentLog.mWriteLog(msg, "INFO");
		}
	}

	public override void LogInfoMustShow(string msg)
	{
		ParentLog.LogInfoMustShow(msg);
	}

	public override void LogSQL(string msg)
	{
		if (IS_LOG_SQL)
		{
			ParentLog.mWriteLog(msg, "SQL");
		}
	}

	public override void LogWarn(string msg)
	{
		ParentLog.LogWarn(msg);
	}

	public static cyntecLogTxn ToCyntecLogTxn(cyntecLog oParent, string TxnName)
	{
		cyntecLogTxn cyntecLogTxn2 = new cyntecLogTxn();
		cyntecLogTxn2.init(oParent, TxnName);
		return cyntecLogTxn2;
	}
}
