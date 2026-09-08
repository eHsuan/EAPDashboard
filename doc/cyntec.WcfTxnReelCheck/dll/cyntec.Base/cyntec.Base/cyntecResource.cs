using System.Reflection;
using System.Resources;
using System.Threading;

namespace cyntec.Base;

public class cyntecResource
{
	public string baseName = "";

	public ResourceManager RM = null;

	public event HOnError onError;

	public cyntecResource()
	{
	}

	public cyntecResource(string name, Assembly asm)
	{
		baseName = name;
		RM = new ResourceManager(baseName, asm);
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
			InvokeOnError(GetType().Name + ",MISS," + Thread.CurrentThread.CurrentCulture.ToString() + "," + tag + "," + baseName);
			return "(MISS)" + tag;
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
			InvokeOnError(GetType().Name + ",MISS," + tag + "," + baseName + "," + strDeault);
			return strDeault;
		}
	}
}
