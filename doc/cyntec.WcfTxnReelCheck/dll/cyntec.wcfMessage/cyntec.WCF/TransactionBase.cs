using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using cyntec.Base;
using cyntec.Tools;

namespace cyntec.WCF;

[Serializable]
public abstract class TransactionBase<T, U> : IwcfTransaction, IwcfMessage where T : IwcfTransaction, new() where U : TransactionResultBase, new()
{
	public static int TxnCount;

	[NonSerialized]
	public bool m_IsClientExecute = false;

	private U m_Result;

	protected U m_txnResult = new U();

	private Dictionary<string, object> m_serverParmList;

	private string m_Message = "";

	private string m_XElementData;

	private string m_msgID = Guid.NewGuid().ToString();

	private string m_mfgFrom = Environment.MachineName;

	private string m_msgTo = "";

	private string m_msgTxnID = Environment.MachineName + "," + typeof(T).FullName + "," + NewSeq() + "-" + Guid.NewGuid().ToString().Substring(0, 3) + ",";

	public DateTime TxnCreateDate = DateTime.Now;

	private DateTime m_MsgCreateDate = DateTime.Now;

	private DateTime m_MsgFinishDate = DateTime.Now;

	public U Result
	{
		get
		{
			if (m_IsClientExecute)
			{
				return m_Result;
			}
			return m_txnResult;
		}
		set
		{
			m_Result = value;
		}
	}

	public U atServerResult => m_txnResult;

	public string msgFileName => typeof(T).GetType().Assembly.ManifestModule.Name;

	public string msgFullClassName => typeof(T).GetType().FullName;

	public Dictionary<string, object> serverParmList
	{
		get
		{
			return m_serverParmList;
		}
		set
		{
			m_serverParmList = value;
		}
	}

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

	public bool IsTxnError
	{
		get
		{
			if (Result == null)
			{
				return true;
			}
			if (Result.isError)
			{
				return true;
			}
			return false;
		}
	}

	public string TxnErrorMsg
	{
		get
		{
			if (Result == null)
			{
				return typeof(T).Name + "的交易回傳值為null, " + Message;
			}
			if (Result.isError)
			{
				return Result.replyMsg + " (" + typeof(T).Name + ")";
			}
			return "";
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

	public string LogFinishFormat
	{
		get
		{
			MsgFinishDate = DateTime.Now;
			if (Result != null && !Result.isError)
			{
				return ToKey() + ", 交易花費(s), " + (MsgFinishDate - MsgCreateDate).TotalSeconds + Environment.NewLine;
			}
			if (Result != null && Result.isError)
			{
				return ToKey() + ", 交易花費(s), " + (MsgFinishDate - MsgCreateDate).TotalSeconds + ", 異常訊息," + Environment.NewLine + Result.replyMsg + Environment.NewLine;
			}
			return ToKey() + ", 交易花費(s), " + (MsgFinishDate - MsgCreateDate).TotalSeconds + ", 異常訊息," + Environment.NewLine + Message + Environment.NewLine;
		}
	}

	public double DBAccumulation { get; set; }

	public bool IsOnEage { get; set; }

	public int OnEageCount { get; set; }

	public static int NewSeq()
	{
		if (TxnCount < int.MaxValue)
		{
			return ++TxnCount;
		}
		TxnCount = 0;
		return TxnCount;
	}

	public static T ToObject(byte[] arrBytes)
	{
		T val = (T)cyntecByteConvert.ByteArrayToObject(arrBytes);
		return (T)cyntecByteConvert.ByteArrayToObject(arrBytes);
	}

	public U clientExecute(wcfServiceConnection wcfConn)
	{
		lock (wcfConn)
		{
			try
			{
				m_IsClientExecute = true;
				if (wcfConn == null)
				{
					throw new Exception("clientExecute需要變數wcfConn(不可為null)");
				}
				byte[] array = wcfConn.ServiceCommand(ToByte());
				if (array == null && wcfConn.HttpCommandLastException != null)
				{
					throw new Exception(wcfConn.HttpCommandLastException.Message + ", " + wcfConn.ServerUrl);
				}
				object obj = cyntecByteConvert.ByteArrayToObject(array);
				if (obj.GetType().Name == typeof(TransactionResultBase).Name && (obj as TransactionResultBase).isError)
				{
					throw new Exception((obj as TransactionResultBase).Message);
				}
				Result = (U)obj;
				return Result;
			}
			catch (Exception ex)
			{
				Message = ex.Message + "," + ex.StackTrace;
				return null;
			}
		}
	}

	public abstract IwcfMessage serverExecute();

	public virtual IwcfMessage serverExecute(wcfServiceConnectionWebAPI oWebAPIConn)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		if (oWebAPIConn == null)
		{
			return serverExecute();
		}
		if (oWebAPIConn.IsWebAPI)
		{
			cyntecDateTime val = new cyntecDateTime();
			string text = ToKey();
			Dictionary<string, object> dictionary = serverParmList;
			serverParmList = null;
			CyntecTxnRequest val2 = new CyntecTxnRequest();
			val2.TXN_ID = msgTxnID;
			val2.TXN_NAME = "TxnWebAPI_Call";
			val2.TXN_REQUEST_TIME = DateTime.Now;
			cyntecJSON.ItemValueSet(val2.TXN_PARAM_LIST, "PCNAME", (object)Environment.MachineName);
			cyntecJSON.ItemValueSet(val2.TXN_PARAM_LIST, "WCFTXN", (object)ToByte());
			bool flag = false;
			string text2 = "";
			if (oWebAPIConn.IsCallMyHttp)
			{
				oWebAPIConn.LogInfo("轉呼WebAPI(TxnWebAPI_Call)(MyHttpPost)開始,@ " + text + "," + oWebAPIConn.URL);
				text2 = oWebAPIConn.MyHttpPost(oWebAPIConn.URL, val2.ToJsonString(), oWebAPIConn.Timeout, Encoding.UTF8, ref flag);
			}
			else
			{
				oWebAPIConn.LogInfo("轉呼WebAPI(TxnWebAPI_Call)(cyntecHTTP.HttpPost)開始,@ " + text + "," + oWebAPIConn.URL);
				text2 = cyntecHTTP.HttpPost(oWebAPIConn.URL, val2.ToJsonString(), oWebAPIConn.Timeout, Encoding.UTF8, ref flag);
			}
			serverParmList = dictionary;
			if (flag)
			{
				Result = new U();
				Result.isError = true;
				Result.Message = text2 + "(TxnWebAPI_Call WebAPI HTTP Call)";
				oWebAPIConn.LogError("轉呼WebAPI異常,@ " + LogFinishFormat);
				return (IwcfMessage)(object)Result;
			}
			CyntecTxnReply val3 = cyntecJSON.ToObject<CyntecTxnReply>(text2);
			if (val3.IS_TXN_ERROR)
			{
				Result = new U();
				Result.isError = true;
				Result.Message = val3.TXN_ERROR_MSG + "(TxnWebAPI_Call WebAPI HTTP Call reply)";
				oWebAPIConn.LogError("轉呼WebAPI異常,@ " + LogFinishFormat);
				return (IwcfMessage)(object)Result;
			}
			byte[] array = Convert.FromBase64String(cyntecJSON.ItemValueGet<string>(val3.TXN_PARAM_LIST, "TxnResult"));
			object obj = cyntecByteConvert.ByteArrayToObject(array);
			m_txnResult = (U)obj;
			oWebAPIConn.LogInfo("轉呼WebAPI完成,@ " + LogFinishFormat);
			return (IwcfMessage)(object)m_txnResult;
		}
		return serverExecute();
	}

	public virtual IwcfTransactionResult EageExecute(wcfServiceConnectionWebAPI oWebAPIConn)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		string text = ToKey();
		oWebAPIConn.LogInfo("轉呼WebAPI開始,@ " + text + "," + oWebAPIConn.URL);
		Dictionary<string, object> dictionary = serverParmList;
		serverParmList = null;
		CyntecTxnRequest val2 = new CyntecTxnRequest();
		val2.TXN_ID = msgTxnID;
		val2.TXN_NAME = "TxnWebAPI_Call";
		val2.TXN_REQUEST_TIME = DateTime.Now;
		cyntecJSON.ItemValueSet(val2.TXN_PARAM_LIST, "PCNAME", (object)Environment.MachineName);
		cyntecJSON.ItemValueSet(val2.TXN_PARAM_LIST, "WCFTXN", (object)ToByte());
		bool flag = false;
		string text2 = cyntecHTTP.HttpPost(oWebAPIConn.URL, val2.ToJsonString(), oWebAPIConn.Timeout, Encoding.UTF8, ref flag);
		serverParmList = dictionary;
		if (flag)
		{
			Result = new U();
			Result.isError = true;
			Result.Message = text2 + "(WebAPI HTTP Call)";
			oWebAPIConn.LogError("轉呼WebAPI異常,@ " + LogFinishFormat);
			return (IwcfTransactionResult)(object)Result;
		}
		CyntecTxnReply val3 = cyntecJSON.ToObject<CyntecTxnReply>(text2);
		if (val3.IS_TXN_ERROR)
		{
			Result = new U();
			Result.isError = true;
			Result.Message = val3.TXN_ERROR_MSG + "(WebAPI HTTP Call reply)";
			oWebAPIConn.LogError("轉呼WebAPI異常,@ " + LogFinishFormat);
			return (IwcfTransactionResult)(object)Result;
		}
		byte[] array = Convert.FromBase64String(cyntecJSON.ItemValueGet<string>(val3.TXN_PARAM_LIST, "TxnResult"));
		object obj = cyntecByteConvert.ByteArrayToObject(array);
		if (((U)obj).isError)
		{
			oWebAPIConn.LogInfo("轉呼WebAPI異常,@ " + LogFinishFormat + "," + ((U)obj).replyMsg);
		}
		else
		{
			oWebAPIConn.LogInfo("轉呼WebAPI完成,@ " + LogFinishFormat);
		}
		return (IwcfTransactionResult)(object)(U)obj;
	}

	public byte[] ToByte()
	{
		return cyntecByteConvert.ObjectToByteArray((object)this);
	}

	public virtual string ToKey()
	{
		if (Message.Trim() == "")
		{
			return msgTxnID;
		}
		string[] array = msgTxnID.Split(new string[1] { "," }, StringSplitOptions.None);
		if (array.Length != 0 && Message.StartsWith(array[0].Trim()))
		{
			string text = "";
			if (array.Length > 1)
			{
				text = Message + ",@," + array[1];
			}
			if (array.Length > 2)
			{
				text = text + "," + array[2];
			}
			if (array.Length > 3)
			{
				text = text + "," + array[3];
			}
			if (text.Length > 0)
			{
				return text;
			}
		}
		return Message + ",@," + msgTxnID;
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
