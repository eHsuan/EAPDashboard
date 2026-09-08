using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecLog
{
	public string strTag = "";

	private bool IsNewFile = true;

	private bool IsFirstWrite = true;

	public string GUID = "";

	public bool IsLongTimeFormat = false;

	public bool IsLogType = true;

	private static readonly ConcurrentDictionary<string, object> _appLogLocks = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

	private TraceListener m_AppTracer;

	private string m_SubFileName = ".log";

	private UnhandledExceptionEventHandler m_UE_EventHandler;

	private DateTime m_checkTime = DateTime.Now;

	public bool IS_REPLY_CONSOLE = false;

	public bool IS_LOG_ERROR = false;

	public bool IS_LOG_INFO = false;

	public bool IS_LOG_DEBUG = false;

	public bool IS_LOG_SQL = false;

	public bool IS_LOG_WARING = false;

	public int KEEP_LOG_DAY = 90;

	public bool IS_AUTO_PURGE = false;

	public bool IS_LONG_NAME = false;

	private int m_PurgeWaitCount = 0;

	private string m_AppName;

	private string m_CurrentFileName;

	private cyntecThread m_oPurgeProcess;

	private DateTime m_LastCheckPurgeTime = DateTime.Now;

	private string m_FilePath;

	private int m_MAX_LOG_SIZE = 1000000;

	private bool m_IsAppTracerCreating = false;

	private DateTime m_InitTraceTime = DateTime.MaxValue;

	private const string EVENT_SOURCE = "CyntecLog";

	private const string EVENT_LOG = "Application";

	private bool IsLogError = false;

	private DateTime LogErrorTime;

	private DateTime m_NewFileTime = DateTime.MinValue;

	private string m_LastNewFile = "";

	private int m_LogMsgLength = 0;

	private Dictionary<string, string> m_LogKeywordList = new Dictionary<string, string>();

	private Dictionary<string, string> m_ShowConsolKeywordList = new Dictionary<string, string>();

	public string AppName
	{
		get
		{
			return m_AppName;
		}
		set
		{
			m_AppName = value;
		}
	}

	public string CurrentFileName => m_CurrentFileName;

	public string DefaultFullPath => m_FilePath + m_AppName + "-" + DateTime.Now.ToString("yyyyMMdd") + "_00001" + m_SubFileName;

	public string DefaultFileName => m_AppName + "-" + DateTime.Now.ToString("yyyyMMdd") + "_00001";

	public string DefaultMainFileName => m_AppName + "-" + DateTime.Now.ToString("yyyyMMdd");

	public string FILE_PATH
	{
		get
		{
			if (m_FilePath.Substring(m_FilePath.Length - 1, 1) == "\\")
			{
				return m_FilePath;
			}
			return m_FilePath + "\\";
		}
		set
		{
			m_FilePath = value;
		}
	}

	public int MAX_LOG_SIZE
	{
		get
		{
			return m_MAX_LOG_SIZE;
		}
		set
		{
			m_MAX_LOG_SIZE = value;
		}
	}

	public bool IsTraceUnhandledException
	{
		get
		{
			return m_UE_EventHandler != null;
		}
		set
		{
			if (value && m_UE_EventHandler == null)
			{
				m_UE_EventHandler = mLogUnhandledException;
				AppDomain.CurrentDomain.UnhandledException += m_UE_EventHandler;
			}
			else if (!value && m_UE_EventHandler != null)
			{
				AppDomain.CurrentDomain.UnhandledException -= m_UE_EventHandler;
				m_UE_EventHandler = null;
			}
		}
	}

	public Dictionary<string, string> LogKeywordList => m_LogKeywordList;

	public Dictionary<string, string> ShowConsolKeywordList => m_ShowConsolKeywordList;

	public event HLogMessage onLog;

	private static object GetAppLogLock(string appName)
	{
		return _appLogLocks.GetOrAdd(appName, (string _) => new object());
	}

	public static void WirteLog(string path, string file, string message)
	{
		if (!cyntecFile.IsDirExist(path))
		{
			cyntecFile.CreateDir(path);
		}
		File.WriteAllText(path + file, cyntecDateTime.NowToMSSQL() + " :: " + message);
	}

	public static void AppendLog(string path, string file, string message)
	{
		if (!cyntecFile.IsDirExist(path))
		{
			cyntecFile.CreateDir(path);
		}
		File.AppendAllText(path + file, cyntecDateTime.NowToMSSQL() + " :: " + message);
	}

	private void m_InvokeLog(string msg)
	{
		if (this.onLog != null)
		{
			this.onLog(msg);
		}
	}

	public void PurgeImmediately()
	{
		try
		{
			cyntecFile.Purge(KEEP_LOG_DAY, FILE_PATH, "*" + m_SubFileName);
		}
		catch (Exception ex)
		{
			try
			{
				WirteLog(FILE_PATH, "Error" + DateTime.Now.ToString("yyyyMMdd") + ".log", "Purge Error :" + ex.Message);
			}
			catch
			{
			}
		}
	}

	public void Purge()
	{
		Thread thread = new Thread(oPurgeProcess_onProcess);
		thread.Start();
	}

	private void oPurgeProcess_onProcess()
	{
		if (KEEP_LOG_DAY <= 0)
		{
			return;
		}
		try
		{
			cyntecFile.Purge(KEEP_LOG_DAY, FILE_PATH, "*" + m_SubFileName);
		}
		catch (Exception ex)
		{
			try
			{
				WirteLog(FILE_PATH, "Error" + DateTime.Now.ToString("yyyyMMdd") + ".log", "Purge Error :" + ex.Message);
			}
			catch
			{
			}
		}
	}

	public cyntecLog(string appName, string logFilePath, string subFileName, int logMaxSize)
	{
		m_AppName = appName;
		m_FilePath = logFilePath;
		m_MAX_LOG_SIZE = logMaxSize;
		m_SubFileName = subFileName;
	}

	public cyntecLog()
	{
		GUID = Guid.NewGuid().ToString();
	}

	~cyntecLog()
	{
		if (m_AppTracer != null)
		{
			m_AppTracer.WriteLine(mGetTime() + ",cyntecLog解構," + GUID);
			m_AppTracer.Flush();
		}
	}

	public void init(string mainDirectory, cyntecXML config)
	{
		IS_LOG_INFO = config.GetNode(mainDirectory, "LOG", "IS_LOG_INFO").ToBoolean();
		IS_LOG_ERROR = config.GetNode(mainDirectory, "LOG", "IS_LOG_ERROR").ToBoolean();
		IS_LOG_WARING = config.GetNode(mainDirectory, "LOG", "IS_LOG_WARING").ToBoolean();
		IS_LOG_SQL = config.GetNode(mainDirectory, "LOG", "IS_LOG_SQL").ToBoolean();
		IS_LOG_DEBUG = config.GetNode(mainDirectory, "LOG", "IS_LOG_DEBUG").ToBoolean();
	}

	private void mLogUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		StackTrace stackTrace = new StackTrace();
		string msg = stackTrace.GetFrame(1).GetMethod().Name + stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
		LogError(msg);
		LogError("unhandler execption: " + e.ToString());
	}

	public void TraceUnHandleError()
	{
		UnhandledExceptionEventHandler unhandledExceptionEventHandler = mLogUnhandledException;
		AppDomain.CurrentDomain.UnhandledException += mLogUnhandledException;
	}

	private TraceListener mGetCurrentTracer(string appName)
	{
		foreach (TraceListener listener in Trace.Listeners)
		{
			if (listener.Name == m_AppName)
			{
				return listener;
			}
		}
		return null;
	}

	private void mInitTrace()
	{
		string text = mGetLogFileName();
		if (m_AppTracer == null)
		{
			m_AppTracer = mGetCurrentTracer(m_AppName);
		}
		while (m_IsAppTracerCreating && m_AppTracer == null)
		{
			cyntecThread.Sleep(1);
		}
		if (m_AppTracer == null)
		{
			m_IsAppTracerCreating = true;
			m_AppTracer = new TextWriterTraceListener(text, m_AppName);
			lock (m_AppTracer)
			{
				m_CurrentFileName = text;
				Trace.Listeners.Add(m_AppTracer);
				FileInfo fileInfo = new FileInfo(m_CurrentFileName);
				if (!fileInfo.Exists)
				{
					m_AppTracer.WriteLine(mGetTime() + ",cyntecLog初始," + GUID + ",<<<檔案最前端>>>");
					m_AppTracer.Flush();
				}
				return;
			}
		}
		if (text != m_CurrentFileName)
		{
			if (text.ToUpper().EndsWith("_INIT.log".ToUpper()))
			{
				text = text.Substring(0, text.Length - 9) + m_SubFileName;
			}
			lock (m_AppTracer)
			{
				m_AppTracer.WriteLine(mGetTime() + ",cyntecLog檔案結束," + GUID + ",<<<檔案最終端>>>,下一個檔案名稱," + text);
				m_AppTracer.Flush();
				bool isThreadSafe = m_AppTracer.IsThreadSafe;
				m_AppTracer.Close();
				Trace.Listeners.Remove(m_AppTracer.Name);
				m_AppTracer = new TextWriterTraceListener(text, m_AppName);
				Trace.Listeners.Add(m_AppTracer);
				m_AppTracer.WriteLine(mGetTime() + ",cyntecLog檔案更換," + GUID + ",<<<檔案最前端>>>,上一個檔案名稱," + m_CurrentFileName);
				m_CurrentFileName = text;
			}
		}
	}

	private void WriteEventLogFallback(string message, Exception ex = null)
	{
		try
		{
			if (!EventLog.SourceExists("CyntecLog"))
			{
				EventLog.CreateEventSource("CyntecLog", "Application");
			}
			string text = message;
			if (ex != null)
			{
				text = text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
			}
			EventLog.WriteEntry("CyntecLog", text, EventLogEntryType.Error);
		}
		catch
		{
		}
	}

	public void mWriteLog(string msg, string category)
	{
		if (IsLogError && DateTime.Now < LogErrorTime.AddMinutes(5.0))
		{
			return;
		}
		IsLogError = false;
		m_LogMsgLength += msg.Length;
		m_PurgeWaitCount++;
		if (m_PurgeWaitCount > 100000 || IsFirstWrite)
		{
			m_PurgeWaitCount = 0;
			IsFirstWrite = false;
			if (IS_AUTO_PURGE)
			{
				Purge();
			}
		}
		string message = ((!IsLogType) ? (mGetTime() + "," + msg) : (mGetTime() + "," + category.PadRight(5) + "," + msg));
		try
		{
			lock (GetAppLogLock(m_AppName))
			{
				if (m_AppTracer == null)
				{
					mInitTrace();
				}
				if (m_InitTraceTime == DateTime.MaxValue || (DateTime.Now - m_InitTraceTime).TotalSeconds > 10.0)
				{
					mInitTrace();
					m_InitTraceTime = DateTime.Now;
				}
				m_AppTracer.WriteLine(message);
				m_AppTracer.Flush();
			}
			if (IS_REPLY_CONSOLE)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(msg);
			}
			m_InvokeLog(msg);
		}
		catch (Exception ex)
		{
			IsLogError = true;
			LogErrorTime = DateTime.Now;
			try
			{
				WirteLog(FILE_PATH, "Error" + DateTime.Now.ToString("yyyyMMdd") + ".log", "mWriteLog Error," + ex.Message);
			}
			catch
			{
			}
		}
	}

	private string m_NewFile(string oldFile)
	{
		m_NewFileTime = DateTime.Now;
		string text = oldFile;
		text = text.Replace(m_SubFileName, "");
		if (text.ToUpper().EndsWith("_INIT"))
		{
			text = text.Substring(0, text.Length - 5);
		}
		int.TryParse(oldFile.Substring(text.Length - 5, 5), out var result);
		result++;
		string text2 = "";
		if (IsNewFile)
		{
			text2 = "_INIT";
		}
		if (IS_LONG_NAME)
		{
			m_LastNewFile = FILE_PATH + m_AppName + "-" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + result.ToString("00000") + text2 + m_SubFileName;
		}
		else
		{
			m_LastNewFile = FILE_PATH + DefaultMainFileName + "_" + result.ToString("00000") + text2 + m_SubFileName;
		}
		return m_LastNewFile;
	}

	private string mGetLogFileName()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(m_FilePath);
		if (!directoryInfo.Exists)
		{
			Directory.CreateDirectory(m_FilePath);
		}
		FileInfo[] files = directoryInfo.GetFiles(DefaultMainFileName + "*");
		string text = ((files.GetLength(0) <= 0) ? DefaultFullPath : (FILE_PATH + files[files.GetUpperBound(0)].ToString()));
		FileInfo fileInfo = new FileInfo(text);
		if (!fileInfo.Exists)
		{
			IsNewFile = false;
			return text;
		}
		if (IsNewFile && files.Length != 0)
		{
			string result = m_NewFile(text);
			IsNewFile = false;
			return result;
		}
		if (fileInfo.Length > m_MAX_LOG_SIZE || m_LogMsgLength > m_MAX_LOG_SIZE)
		{
			m_LogMsgLength = 0;
			return m_NewFile(text);
		}
		return text;
	}

	public virtual void LogSQL(string msg)
	{
		if (IS_LOG_SQL)
		{
			mWriteLog(msg, "SQL");
		}
	}

	public virtual void LogError(string msg)
	{
		if (IS_LOG_ERROR)
		{
			mWriteLog(msg, "ERROR");
		}
	}

	public virtual void LogError(Exception ex)
	{
		if (IS_LOG_ERROR)
		{
			Exception ex2 = ex.InnerException;
			if (ex2 == null)
			{
				ex2 = ex;
			}
			mWriteLog(ex2.Message + "\n" + ex2.StackTrace, "ERROR");
		}
	}

	public virtual void LogError(string keyword, string msg)
	{
		if (LogKeywordList.Count == 0 || m_LogKeywordList.ContainsKey(keyword.ToUpper()))
		{
			LogError(keyword + ", " + msg);
		}
		if (m_ShowConsolKeywordList.Count == 0 || m_ShowConsolKeywordList.ContainsKey(keyword.ToUpper()))
		{
			Console.WriteLine(keyword + ", " + msg);
		}
	}

	public virtual void LogWarn(string msg)
	{
		if (IS_LOG_WARING)
		{
			mWriteLog(msg, "WARING");
		}
	}

	public virtual void LogInfo(string msg)
	{
		if (IS_LOG_INFO)
		{
			mWriteLog(msg, "INFO");
		}
	}

	public virtual void LogInfoMustShow(string msg)
	{
		if (!IS_REPLY_CONSOLE)
		{
			Console.WriteLine(msg);
		}
		LogInfo(msg);
	}

	public virtual void LogErrorMustShow(string msg)
	{
		if (!IS_REPLY_CONSOLE)
		{
			Console.WriteLine(msg);
		}
		LogError(msg);
	}

	public void WriteMsg(string msg)
	{
		mInitTrace();
		m_AppTracer.Write(msg);
		m_AppTracer.Flush();
		if (IS_REPLY_CONSOLE)
		{
			Console.Write(msg);
		}
	}

	public virtual void LogDebug(string msg)
	{
		if (IS_LOG_DEBUG)
		{
			mWriteLog(msg, "DEBUG");
		}
	}

	public virtual void LogDebug(string keyword, string msg)
	{
		if (LogKeywordList.Count == 0 || m_LogKeywordList.ContainsKey(keyword.ToUpper()))
		{
			LogDebug(keyword + ", " + msg);
		}
		if (m_ShowConsolKeywordList.Count == 0 || m_ShowConsolKeywordList.ContainsKey(keyword.ToUpper()))
		{
			Console.WriteLine(keyword + ", " + msg);
		}
	}

	private string mGetTime()
	{
		if (IsLongTimeFormat)
		{
			return cyntecDateTime.NowToMSSQL();
		}
		return DateTime.Now.ToString("yyyyMMdd HHmmss.fff");
	}

	private string mGetType(string type)
	{
		return "[" + type + "]";
	}

	public virtual void Init(cyntecXML config)
	{
		IS_LOG_INFO = config.GetNode("SCT", "LOG", "IS_LOG_INFO").ToBoolean();
		IS_LOG_WARING = config.GetNode("SCT", "LOG", "IS_LOG_WARING").ToBoolean();
		IS_LOG_ERROR = config.GetNode("SCT", "LOG", "IS_LOG_ERROR").ToBoolean();
		IS_LOG_SQL = config.GetNode("SCT", "LOG", "IS_LOG_SQL").ToBoolean();
	}
}
