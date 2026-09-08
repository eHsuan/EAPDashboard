using System;
using System.Text;
using cyntec.Tools;

namespace cyntec.WCF;

public class wcfServiceConnectionWebAPI
{
	public bool IsWebAPI = false;

	public string URL = "";

	public int Timeout = 6000;

	public cyntecLog Log = null;

	public bool IsCallMyHttp = false;

	public void LogDebug(string msg)
	{
		if (Log != null)
		{
			Log.LogDebug(msg);
		}
	}

	public void LogInfo(string msg)
	{
		if (Log != null)
		{
			Log.LogInfo(msg);
		}
	}

	public void LogError(string msg)
	{
		if (Log != null)
		{
			Log.LogError(msg);
		}
	}

	public virtual string MyHttpPost(string posturl, string postData, int Timeout, Encoding encoding, ref bool IsHttpPostError)
	{
		throw new NotImplementedException();
	}
}
