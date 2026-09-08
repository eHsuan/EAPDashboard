using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cyntec.Windows;

public abstract class cyntecWinEntryBase
{
	protected Dictionary<string, object> parmList = new Dictionary<string, object>();

	public object this[string key]
	{
		get
		{
			if (parmList.ContainsKey(key.ToUpper()))
			{
				return parmList[key.ToUpper()];
			}
			return null;
		}
		set
		{
			if (parmList.ContainsKey(key.ToUpper()))
			{
				parmList[key.ToUpper()] = value;
			}
			else
			{
				parmList.Add(key.ToUpper(), value);
			}
		}
	}

	public abstract string FormName { get; }

	public static string NEW_SYSID()
	{
		return Guid.NewGuid().ToString();
	}

	public void AddItem(string key, object item)
	{
		if (parmList == null)
		{
			parmList = new Dictionary<string, object>();
		}
		parmList.Add(key.ToUpper(), item);
	}

	public abstract void Execute(Form mainForm);
}
