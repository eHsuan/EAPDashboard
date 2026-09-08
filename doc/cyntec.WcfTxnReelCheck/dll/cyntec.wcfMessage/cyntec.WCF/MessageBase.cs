using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using cyntec.Base;

namespace cyntec.WCF;

[Serializable]
public abstract class MessageBase<T> : IwcfMessage where T : IwcfMessage, new()
{
	private string m_Message = "";

	private string m_msgID = Guid.NewGuid().ToString();

	private string m_XElementData;

	private string m_msgFrom = "";

	private string m_msgTo = "";

	private string m_msgTxnID;

	private DateTime m_MsgCreateDate = DateTime.Now;

	private DateTime m_MsgFinishDate = DateTime.Now;

	public static string className => typeof(T).Name;

	public string msgFileName => GetType().Assembly.ManifestModule.Name;

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

	public string msgClassName => GetType().UnderlyingSystemType.Name;

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

	public string msgFrom
	{
		get
		{
			return m_msgFrom;
		}
		set
		{
			m_msgFrom = value;
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
		return cyntecByteConvert.ObjectToByteArray((object)this);
	}

	public static T ToObject(byte[] arrBytes)
	{
		return (T)cyntecByteConvert.ByteArrayToObject(arrBytes);
	}

	public void clientBroadcast(string receiverAddress, wcfServiceConnection wcfConn)
	{
		wcfConn.Broadcast(receiverAddress, ToByte());
	}

	public void clientBroadcast(wcfServiceConnection wcfConn)
	{
		wcfConn.Broadcast(msgTo, ToByte());
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

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
