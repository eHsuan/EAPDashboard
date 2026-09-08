using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using cyntec.Base;

namespace cyntec.WCF;

[Serializable]
public class wcfHelloConnection : IwcfMessage
{
	private string m_Message = "";

	private string m_XElementData;

	private string m_msgID = Guid.NewGuid().ToString();

	private string m_mfgFrom;

	private string m_msgTo;

	private string m_msgTxnID;

	private DateTime m_MsgCreateDate = DateTime.Now;

	private DateTime m_MsgFinishDate = DateTime.Now;

	public static string className
	{
		get
		{
			wcfHelloConnection wcfHelloConnection2 = new wcfHelloConnection();
			return wcfHelloConnection2.msgClassName;
		}
	}

	public string msgFileName => GetType().Assembly.ManifestModule.Name;

	public string msgClassName => GetType().UnderlyingSystemType.Name;

	public string msgFullClassName => GetType().FullName;

	public string Message
	{
		get
		{
			return m_Message;
		}
		set
		{
			m_Message = value;
		}
	}

	public string XElementData
	{
		get
		{
			return m_XElementData;
		}
		set
		{
			m_XElementData = value;
		}
	}

	public string msgID
	{
		get
		{
			return m_msgID;
		}
		set
		{
			m_msgID = value;
		}
	}

	public string msgFrom
	{
		get
		{
			return m_mfgFrom;
		}
		set
		{
			m_mfgFrom = value;
		}
	}

	public string msgTo
	{
		get
		{
			return m_msgTo;
		}
		set
		{
			m_msgTo = value;
		}
	}

	public string msgTxnID
	{
		get
		{
			return m_msgTxnID;
		}
		set
		{
			m_msgTxnID = value;
		}
	}

	public DateTime MsgCreateDate
	{
		get
		{
			return m_MsgCreateDate;
		}
		set
		{
			m_MsgCreateDate = value;
		}
	}

	public DateTime MsgFinishDate
	{
		get
		{
			return m_MsgFinishDate;
		}
		set
		{
			m_MsgFinishDate = value;
		}
	}

	public byte[] ToByte()
	{
		return cyntecByteConvert.ObjectToByteArray(this);
	}

	public XElement ToXElement()
	{
		try
		{
			return XElement.Parse(XElementData);
		}
		catch
		{
			return null;
		}
	}

	public wcfHelloConnection()
	{
	}

	public wcfHelloConnection(SerializationInfo info, StreamingContext context)
	{
		if (info.MemberCount != 0)
		{
		}
	}
}
