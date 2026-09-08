using System;

namespace cyntec.Base;

public class cyntecException : SystemException
{
	private string m_strMessage = "";

	public EexecptionType type = EexecptionType.Note;

	private string m_MsgID = null;

	private string[] m_Parms;

	public string MsgID => m_MsgID;

	public string[] Parms => m_Parms;

	public override string Message => m_strMessage;

	protected cyntecException()
	{
	}

	public cyntecException(object msgID)
		: base(msgID.ToString())
	{
		m_MsgID = msgID.ToString();
	}

	public cyntecException(object msgID, params string[] parm)
	{
		m_MsgID = msgID.ToString();
		m_Parms = parm;
	}

	public cyntecException(string msgID, EexecptionType type)
	{
		m_MsgID = msgID.ToString();
		this.type = type;
	}

	public cyntecException(object msgID, Exception innerException)
		: base(innerException.Message)
	{
		m_strMessage = msgID.ToString();
		type = EexecptionType.Error;
		m_MsgID = msgID.ToString();
		m_strMessage = innerException.Message;
	}

	public cyntecException(string msgID, string Message, params string[] parm)
	{
		m_strMessage = Message;
		type = EexecptionType.Error;
		m_MsgID = msgID.ToString();
	}

	public override string ToString()
	{
		return "MSG_ID= " + m_MsgID + " DETAIL= " + m_strMessage;
	}

	public static string GetMsgId(Exception ex)
	{
		if (ex.GetType().Name == typeof(cyntecException).Name)
		{
			cyntecException ex2 = (cyntecException)ex;
			return ex2.MsgID;
		}
		return "Unknow";
	}
}
