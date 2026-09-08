using System;
using System.ComponentModel;

namespace cyntec.Base;

public class cyntecResourceComponent
{
	public Type _type;

	private ComponentResourceManager RM = null;

	public event HOnError onError;

	public cyntecResourceComponent()
	{
	}

	public cyntecResourceComponent(Type type)
	{
		_type = type;
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(_type);
	}

	public void InvokeOnError(string strMsg)
	{
		if (this.onError != null)
		{
			this.onError(this, strMsg);
		}
	}

	public virtual string ToString(string tag)
	{
		try
		{
			return RM.GetString(tag);
		}
		catch
		{
			InvokeOnError("MISS," + tag);
			return tag;
		}
	}

	public virtual string ToString(string tag, string strDeault)
	{
		try
		{
			return RM.GetString(tag);
		}
		catch
		{
			InvokeOnError("MISS," + tag + "," + strDeault);
			return "*" + strDeault;
		}
	}
}
