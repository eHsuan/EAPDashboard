using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using cyntec.Base;
using cyntec.Tools;
using cyntec.wcfConnection.wcfServiceRef;

namespace cyntec.WCF;

public class wcfServiceConnection : AComponentBasic, IDisposable
{
	public DateTime StartTime;

	public DateTime EndTime;

	public bool IsExecuting = false;

	public bool IsReconnect = true;

	public bool IsInitSuccess = false;

	private bool m_IsHeartBeat = true;

	public List<string> ServerUrlList = new List<string>();

	private bool m_IsBroadcase = true;

	private static int m_TxnSEQ;

	private cyntecThreadBuffer<byte[]> m_BroadcastThread = null;

	public readonly string guid = Guid.NewGuid().ToString();

	public string Client_Id = "";

	public string Session_Id = "";

	public string Address = "";

	private string m_ServiceName;

	private cyntecThread m_HeartbeatThread = null;

	internal string m_LastActiveTime = null;

	public int OpenTimeout = 0;

	public int CloseTimeout = 0;

	internal IwcfServiceClient m_proxy;

	internal wcfServiceCallback m_CallBack;

	internal InstanceContext context;

	internal string m_ServerUrl = "";

	internal string m_ClientUrl = "";

	internal string m_UserId = "";

	internal string m_password = "";

	private Binding objBinding = null;

	private int m_UrlIndex = 0;

	public int HttpCommandErrorCount = 0;

	public Exception HttpCommandLastException = null;

	public DateTime ExecuteStartTime;

	public DateTime ExecuteEndTime;

	public bool IsHeartBeat
	{
		get
		{
			return m_IsHeartBeat;
		}
		set
		{
			m_IsHeartBeat = value;
			if (m_HeartbeatThread != null && !m_IsHeartBeat)
			{
				m_HeartbeatThread.Stop();
			}
		}
	}

	public bool IsBroadcase
	{
		get
		{
			return m_IsHeartBeat;
		}
		set
		{
			if (m_BroadcastThread != null && !m_IsBroadcase)
			{
				m_BroadcastThread.Stop();
			}
		}
	}

	public string ServiceName => m_ServiceName;

	public int ReceiveTimeout
	{
		get
		{
			if (objBinding != null)
			{
				return objBinding.ReceiveTimeout.Seconds;
			}
			return 0;
		}
		set
		{
			objBinding.ReceiveTimeout = new TimeSpan(0, 0, value);
		}
	}

	public int SendTimeout
	{
		get
		{
			if (objBinding != null)
			{
				return objBinding.SendTimeout.Seconds;
			}
			return 0;
		}
		set
		{
			objBinding.SendTimeout = new TimeSpan(0, 0, value);
		}
	}

	public string ServerUrl => m_ServerUrl;

	public string ClientUrl => m_ClientUrl;

	private ThreadProcessEvent m_ThreadProcess => m_HeartbeatThread_onProcess;

	public bool IsHTTP => ServerUrl.Trim().ToUpper().StartsWith("HTTP");

	public event ConnBroadCastDelegate onBroadcast;

	public event BroadCastMesssageDelegate onBroadcastMessage;

	public virtual event ServiceCommandDelegate onServiceCommand;

	~wcfServiceConnection()
	{
		Console.WriteLine("wcfServiceConnection解構");
	}

	public static string NewTxnID(string itemName)
	{
		m_TxnSEQ++;
		if (m_TxnSEQ > 999999)
		{
			m_TxnSEQ = 1;
		}
		return Environment.MachineName + "@" + DateTime.Now.ToString("yyMMdd@HHmmssfff") + "@" + itemName + "@" + m_TxnSEQ.ToString("000000");
	}

	public wcfServiceConnection()
	{
	}

	public wcfServiceConnection(string serverUrl, string ClientUrl, string userId, string password)
	{
		m_ServerUrl = serverUrl;
		m_ClientUrl = ClientUrl;
		m_UserId = userId;
		m_password = password;
		m_ReConnection();
		m_LastActiveTime = cyntecDateTime.Now();
	}

	public static wcfServiceConnection NewConnection(string serverUrl, string ClientUrl, string userId, string password)
	{
		return new wcfServiceConnection(serverUrl, ClientUrl, userId, password);
	}

	private void m_HeartbeatThread_onProcess()
	{
		try
		{
			double totalSeconds = (DateTime.Now - cyntecDateTime.ToDateTime(m_LastActiveTime)).TotalSeconds;
			if (!(totalSeconds > 60.0) || IsHTTP)
			{
				return;
			}
			if (!IsInitSuccess)
			{
				m_Log("wcf connection 未初始化成功...嘗試初始化");
				try
				{
					Initialize();
				}
				catch (Exception ex)
				{
					m_LogError("因WCF connection 初始化失敗, 故無法執行 Heartbeat: " + ex.Message);
					return;
				}
			}
			m_LastActiveTime = cyntecDateTime.Now();
			string text = m_proxy.Heartbeat();
		}
		catch (Exception ex2)
		{
			m_LogError("wcfServiceConnections.m_HeartbeatThread_onProcess()異常," + ex2.Message);
			m_LastActiveTime = cyntecDateTime.Now();
			m_LogError("HeartBeat發送NG");
			m_LogError("HeartBeat發送NG訊息: " + ex2.Message);
			cyntecConsol.LogMsg("Heartbeat重發");
			try
			{
				m_ReConnection();
				Initialize();
				HostCommand(null);
				m_LogError("Heartbeat重發OK");
			}
			catch
			{
				m_LogError("Heartbeat重發NG");
				m_LogError("Heartbeat重發NG訊息: " + ex2.Message);
			}
		}
	}

	internal virtual void m_ReConnection()
	{
		try
		{
			if (m_ServerUrl.ToUpper().StartsWith("NET.TCP"))
			{
				try
				{
					objBinding = new NetTcpBinding("cyntec.WCF.wcfServiceBase");
				}
				catch
				{
					objBinding = new NetTcpBinding();
					m_LogError(this, "WCF.wcfServiceBase設定只能使用預設值(可能沒有檔案或沒有設定)");
				}
				if ((objBinding as NetTcpBinding).MaxBufferSize == 65536)
				{
					(objBinding as NetTcpBinding).MaxReceivedMessageSize = 104857600L;
					(objBinding as NetTcpBinding).MaxBufferPoolSize = 1000000L;
					(objBinding as NetTcpBinding).ReaderQuotas.MaxStringContentLength = 10000000;
					(objBinding as NetTcpBinding).ReaderQuotas.MaxArrayLength = 123000000;
					(objBinding as NetTcpBinding).Security.Mode = SecurityMode.None;
					objBinding.OpenTimeout = new TimeSpan(0, 0, 3);
					objBinding.CloseTimeout = new TimeSpan(0, 1, 15);
					objBinding.ReceiveTimeout = new TimeSpan(0, 1, 15);
					objBinding.SendTimeout = new TimeSpan(0, 0, 15);
				}
			}
			else
			{
				if (!m_ServerUrl.ToUpper().StartsWith("HTTP"))
				{
					throw new Exception("HOST URL 設定不正確");
				}
				objBinding = new WSDualHttpBinding();
				(objBinding as WSDualHttpBinding).ClientBaseAddress = new Uri(m_ClientUrl);
				(objBinding as WSDualHttpBinding).MaxReceivedMessageSize = 104857600L;
				(objBinding as WSDualHttpBinding).MaxBufferPoolSize = 1000000L;
				(objBinding as WSDualHttpBinding).ReaderQuotas.MaxStringContentLength = 10000000;
				(objBinding as WSDualHttpBinding).ReaderQuotas.MaxArrayLength = 123000000;
				(objBinding as WSDualHttpBinding).Security.Mode = WSDualHttpSecurityMode.None;
				objBinding.OpenTimeout = new TimeSpan(0, 0, 3);
				objBinding.CloseTimeout = new TimeSpan(0, 0, 15);
				objBinding.SendTimeout = new TimeSpan(0, 0, 15);
			}
			m_CallBack = new wcfServiceCallback();
			context = new InstanceContext(m_CallBack);
			m_CallBack.onBroadcast -= m_CallBack_onBroadcast;
			m_CallBack.onBroadcast += m_CallBack_onBroadcast;
			if (IsHTTP)
			{
				m_Log("HTTP執行m_ReConnection");
			}
			else
			{
				EndpointAddress remoteAddress = new EndpointAddress(m_ServerUrl);
				m_proxy = new IwcfServiceClient(context, objBinding, remoteAddress);
			}
			m_LastActiveTime = cyntecDateTime.Now();
		}
		catch (Exception ex)
		{
			throw new Exception("m_ReConnection, " + ex.Message);
		}
	}

	private void m_CallBack_onBroadcast(object session, byte[] rawData)
	{
		if (!IsBroadcase)
		{
			return;
		}
		try
		{
			m_BroadcastThread.Enqueue(rawData);
		}
		catch (Exception ex)
		{
			m_LogError("wcfServiceConnections m_CallBack_onBroadcast " + ex.Message);
		}
	}

	public virtual byte[] HostCommand(byte[] rawData)
	{
		m_LastActiveTime = cyntecDateTime.Now();
		try
		{
			if (IsHTTP)
			{
				m_Log("HTTP不能執行HostCommand");
				return null;
			}
			if (!IsInitSuccess)
			{
				m_Log("wcf connection 未初始化成功...發送HostCommand前初始化");
				Initialize();
			}
			return m_proxy.HostCommand(rawData);
		}
		catch (Exception ex)
		{
			m_LogError("wcfServiceConnections HostCommand " + ex.Message);
			m_LogError("HostCommand第1次執行...NG");
			m_LogError("HostCommand第1次執行錯誤訊息: " + ex.Message);
			try
			{
				m_ReConnection();
				Initialize();
				byte[] result = m_proxy.HostCommand(rawData);
				cyntecConsol.LogMsg("HostCommand第2次執行...OK");
				return result;
			}
			catch (Exception ex2)
			{
				m_LogError("wcfServiceConnections HostCommand 2# " + ex2.Message);
				m_LogError("HostCommand第2次執行...NG");
				m_LogError("HostCommand第2次執行錯誤訊息: " + ex2.Message);
				throw new Exception("HostCommand第2次執行失敗 " + ex2.Message);
			}
		}
	}

	private void m_LogTxnResult(IwcfTransactionResult result, IwcfMessage oMessage, string txnID, string replace)
	{
		string text = "";
		text = ((oMessage == null) ? ((result != null && !result.isError) ? ("WCF TXN 成功%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + ",呼叫起始," + cyntecDateTime.ToMSSQL(StartTime) + ",呼叫結束," + cyntecDateTime.ToMSSQL(EndTime) + Environment.NewLine) : ((result != null) ? ("WCF TXN 失敗%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + Environment.NewLine + ", 異常訊息, " + result.replyMsg + ",呼叫起始," + cyntecDateTime.ToMSSQL(StartTime) + ",呼叫結束," + cyntecDateTime.ToMSSQL(EndTime) + Environment.NewLine) : ("WCF TXN 失敗%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + ",呼叫起始," + cyntecDateTime.ToMSSQL(StartTime) + ",呼叫結束," + cyntecDateTime.ToMSSQL(EndTime) + Environment.NewLine))) : ((result != null && !result.isError) ? ("WCF TXN 成功%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + Environment.NewLine + ",T1起始," + cyntecDateTime.ToMSSQL(StartTime) + ",T1結束," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T1花費(s)" + (oMessage.MsgCreateDate - StartTime).TotalSeconds + Environment.NewLine + ",T2起始," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T2結束," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T2花費(s)" + (oMessage.MsgFinishDate - oMessage.MsgCreateDate).TotalSeconds + Environment.NewLine + ",T3起始," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T3結束," + cyntecDateTime.ToMSSQL(EndTime) + ",T3花費(s)" + (EndTime - oMessage.MsgFinishDate).TotalSeconds + Environment.NewLine) : ((result != null) ? ("WCF TXN 失敗%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + Environment.NewLine + ", 異常訊息, " + result.replyMsg + Environment.NewLine + ",T1起始," + cyntecDateTime.ToMSSQL(StartTime) + ",T1結束," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T1花費(s)" + (oMessage.MsgCreateDate - StartTime).TotalSeconds + Environment.NewLine + ",T2起始," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T2結束," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T2花費(s)" + (oMessage.MsgFinishDate - oMessage.MsgCreateDate).TotalSeconds + Environment.NewLine + ",T3起始," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T3結束," + cyntecDateTime.ToMSSQL(EndTime) + ",T3花費(s)" + (EndTime - oMessage.MsgFinishDate).TotalSeconds + Environment.NewLine) : ("WCF TXN 失敗%RETRY%, " + txnID + ", 交易花費(s), " + (EndTime - StartTime).TotalSeconds + Environment.NewLine + ",T1起始," + cyntecDateTime.ToMSSQL(StartTime) + ",T1結束," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T1花費(s)" + (oMessage.MsgCreateDate - StartTime).TotalSeconds + Environment.NewLine + ",T2起始," + cyntecDateTime.ToMSSQL(oMessage.MsgCreateDate) + ",T2結束," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T2花費(s)" + (oMessage.MsgFinishDate - oMessage.MsgCreateDate).TotalSeconds + Environment.NewLine + ",T3起始," + cyntecDateTime.ToMSSQL(oMessage.MsgFinishDate) + ",T3結束," + cyntecDateTime.ToMSSQL(EndTime) + ",T3花費(s)" + (EndTime - oMessage.MsgFinishDate).TotalSeconds + Environment.NewLine))));
		m_Log(text.Replace("%RETRY%", replace));
	}

	public virtual byte[] ServiceCommandHTTP(byte[] rawData)
	{
		if (HttpCommandErrorCount >= 2)
		{
			HttpCommandErrorCount = 0;
			return null;
		}
		if (HttpCommandErrorCount == 0)
		{
			m_LastActiveTime = cyntecDateTime.NowToMSSQL();
		}
		HttpCommandLastException = null;
		IwcfMessage iwcfMessage = null;
		object obj = null;
		string text = "";
		try
		{
			obj = cyntecByteConvert.ByteArrayToObject(rawData);
			if (obj != null)
			{
				iwcfMessage = obj as IwcfMessage;
			}
			if (!IsInitSuccess)
			{
				m_Log("wcf connection 未初始化成功...發送ServiceCommandHTTP前初始化");
				try
				{
					Initialize();
				}
				catch (Exception ex)
				{
					throw new Exception("因WCF connection 初始化失敗, 故無法執行 ServiceCommandHTTP" + ex.Message);
				}
			}
			text = ((iwcfMessage == null) ? "WCF TXN 交易參數無法識別" : (iwcfMessage.msgTxnID + " ,@ " + iwcfMessage.msgID + " ,@ " + cyntecByteConvert.AssemblyName(obj)));
			HttpWebRequest httpWebRequest = WebRequest.Create(ServerUrl) as HttpWebRequest;
			httpWebRequest.ReadWriteTimeout = 60000;
			httpWebRequest.Method = "POST";
			httpWebRequest.Headers.Add("SOAPAction", "http://tempuri.org/IWCF/WcfTxn_Call");
			httpWebRequest.ContentType = "text/xml;charset=UTF-8";
			string text2 = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\" xmlns:wcf=\"http://schemas.datacontract.org/2004/07/WcfEntrance\">" + Environment.NewLine + "<soapenv:Header/>" + Environment.NewLine + "<soapenv:Body>" + Environment.NewLine + "   <tem:WcfTxn_Call>" + Environment.NewLine + "      <tem:condition>" + Environment.NewLine + "         <wcf:_ClientName>@PCNAME@</wcf:_ClientName>" + Environment.NewLine + "         <wcf:wcftxn>@WCFTXN@</wcf:wcftxn>" + Environment.NewLine + "      </tem:condition>" + Environment.NewLine + "   </tem:WcfTxn_Call>" + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + " </soapenv:Envelope>" + Environment.NewLine;
			text2 = text2.Replace("@PCNAME@", Client_Id);
			text2 = text2.Replace("@WCFTXN@", Convert.ToBase64String(rawData));
			byte[] bytes = Encoding.UTF8.GetBytes(text2);
			httpWebRequest.ContentLength = bytes.Length;
			StartTime = DateTime.Now;
			httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string text3 = streamReader.ReadToEnd();
			EndTime = DateTime.Now;
			text3 = text3.Replace("<s:", "<");
			text3 = text3.Replace("</s:", "</");
			text3 = text3.Replace("<a:", "<");
			text3 = text3.Replace("</a:", "</");
			cyntecXML cyntecXML2 = new cyntecXML();
			cyntecXML2.LoadXml(text3);
			cyntecXML2.RootTag = "Envelope";
			byte[] array = Convert.FromBase64String(cyntecXML2.GetNode("Body", "WcfTxn_CallResponse", "WcfTxn_CallResult", "wcfTxnResult").ToString());
			IwcfTransactionResult iwcfTransactionResult = null;
			object obj2 = null;
			IwcfMessage oMessage = null;
			if (array != null)
			{
				obj2 = cyntecByteConvert.ByteArrayToObject(array);
				iwcfTransactionResult = obj2 as IwcfTransactionResult;
				oMessage = obj2 as IwcfMessage;
			}
			if (this.onServiceCommand != null)
			{
				this.onServiceCommand(obj as IwcfTransaction, iwcfTransactionResult, iwcfMessage);
			}
			m_LogTxnResult(iwcfTransactionResult, oMessage, text, "");
			return array;
		}
		catch (WebException ex2)
		{
			HttpCommandErrorCount++;
			m_LogError("HTTP第" + HttpCommandErrorCount + "次異常," + ex2.Message);
			m_LogError(ServerUrl);
			HttpCommandLastException = ex2;
			if (ServerUrlList.Count > 0)
			{
				if (m_UrlIndex >= ServerUrlList.Count - 1)
				{
					m_UrlIndex = 0;
				}
				else
				{
					m_UrlIndex++;
				}
				m_ServerUrl = ServerUrlList[m_UrlIndex];
				m_Log("轉換連線, " + ServerUrl);
			}
			return ServiceCommand(rawData);
		}
		catch (Exception ex3)
		{
			m_LogError("未知異常, " + ex3.Message);
			return null;
		}
	}

	private void ExecuteNetTcp(byte[] rawData, ref byte[] oReturn, ref bool IsOK, ref string ErrorCode, ref string ErrorMsg, int ExecuteCount)
	{
		try
		{
			DateTime now = DateTime.Now;
			if ((DateTime.Now - cyntecDateTime.ToDateTime(m_LastActiveTime)).TotalMinutes > 1.0)
			{
				m_proxy.Heartbeat();
				m_Log("主機" + m_ServerUrl + ",WCF TXN Heartbeat,正常,花費(秒)," + (DateTime.Now - now).TotalSeconds);
			}
		}
		catch (Exception ex)
		{
			m_Log("主機" + m_ServerUrl + ",WCF TXN Heartbeat,異常," + ex.Message);
			cyntecThread.Sleep(3000);
		}
		m_LastActiveTime = cyntecDateTime.Now();
		IwcfMessage iwcfMessage = null;
		object obj = null;
		string text = "";
		IsOK = true;
		ErrorCode = "";
		ErrorMsg = "";
		try
		{
			obj = cyntecByteConvert.ByteArrayToObject(rawData);
			if (obj == null)
			{
				throw new Exception("轉換之後的交易物件為null");
			}
			iwcfMessage = obj as IwcfMessage;
			if (iwcfMessage == null)
			{
				throw new Exception("無法轉換IwcfMessage物件");
			}
			text = iwcfMessage.msgTxnID + " ,@ " + iwcfMessage.msgID + " ,@ " + cyntecByteConvert.AssemblyName(obj);
		}
		catch (Exception ex2)
		{
			IsOK = false;
			ErrorCode = "00-WCF TXN,轉換交易物件失敗";
			ErrorMsg = ex2.Message;
			m_LogTxnResult(null, null, m_ServerUrl + "," + ErrorCode + "," + ErrorMsg, "(" + ExecuteCount + ")");
			return;
		}
		try
		{
			if (!IsInitSuccess)
			{
				m_Log("WCF TXN 未初始化...發送ServiceCommand前初始化");
				Initialize();
			}
		}
		catch (Exception ex3)
		{
			IsOK = false;
			ErrorCode = "01-WCF TXN,初始化失敗";
			if (ex3.Message.Contains("Winsock"))
			{
				ErrorCode = "02-WCF TXN,初始化失敗(Winsock)";
			}
			else if (ex3.Message.Contains("Faulted"))
			{
				ErrorCode = "03-WCF TXN,初始化失敗(狀態Faulted)";
			}
			else if (ex3.Message.Contains("沒有 DNS 項目"))
			{
				ErrorCode = "05-WCF TXN,初始化失敗(沒有 DNS 項目)";
			}
			if (iwcfMessage == null)
			{
				ErrorMsg = ex3.Message;
			}
			else
			{
				ErrorMsg = iwcfMessage.msgTxnID + "," + ex3.Message;
			}
			m_LogTxnResult(null, null, m_ServerUrl + "," + ErrorCode + "," + ErrorMsg, "(" + ExecuteCount + ")");
			return;
		}
		IwcfTransactionResult iwcfTransactionResult = null;
		IwcfMessage oMessage = null;
		try
		{
			StartTime = DateTime.Now;
			oReturn = m_proxy.ServiceCommand(rawData);
			EndTime = DateTime.Now;
			object obj2 = null;
			if (oReturn != null)
			{
				obj2 = cyntecByteConvert.ByteArrayToObject(oReturn);
				iwcfTransactionResult = obj2 as IwcfTransactionResult;
				oMessage = obj2 as IwcfMessage;
			}
			m_LogTxnResult(iwcfTransactionResult, oMessage, m_ServerUrl + "," + text, "(" + ExecuteCount + ")");
			if (this.onServiceCommand != null)
			{
				this.onServiceCommand(obj as IwcfTransaction, iwcfTransactionResult, iwcfMessage);
			}
		}
		catch (Exception ex4)
		{
			EndTime = DateTime.Now;
			IsOK = false;
			ErrorCode = "11-WCF TXN,執行失敗";
			if (ex4.Message.Contains("Winsock"))
			{
				ErrorCode = "12-WCF TXN,執行失敗(Winsock)";
			}
			else if (ex4.Message.Contains("Faulted"))
			{
				ErrorCode = "13-WCF TXN,執行失敗(狀態Faulted)";
			}
			else if (ex4.Message.Contains("OperationTimeout"))
			{
				ErrorCode = "14-WCF TXN,執行失敗(OperationTimeout)";
			}
			else if (ex4.Message.Contains("沒有 DNS 項目"))
			{
				ErrorCode = "15-WCF TXN,執行失敗(沒有 DNS 項目)";
			}
			ErrorMsg = ex4.Message;
			m_LogTxnResult(iwcfTransactionResult, oMessage, m_ServerUrl + "," + ErrorCode + "," + ErrorMsg, "(" + ExecuteCount + ")");
		}
	}

	private byte[] ExecuteNetTcpLoop(byte[] rawData)
	{
		byte[] oReturn = null;
		bool IsOK = false;
		string ErrorCode = "";
		string ErrorMsg = "";
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		bool flag4 = true;
		int num = 4;
		int num2 = 1;
		while (num > num2)
		{
			ExecuteNetTcp(rawData, ref oReturn, ref IsOK, ref ErrorCode, ref ErrorMsg, num2);
			if (IsOK)
			{
				return oReturn;
			}
			if (ErrorCode.StartsWith("00"))
			{
				throw new Exception(ErrorCode + ", " + ErrorMsg);
			}
			if (ErrorCode.StartsWith("01"))
			{
				try
				{
					Initialize();
				}
				catch (Exception ex)
				{
					m_Log("ExecuteNetTcpLoop, 初始化失敗, " + ex.Message);
				}
			}
			else if (ErrorCode.StartsWith("02") || ErrorCode.StartsWith("12"))
			{
				if (!flag)
				{
					flag = true;
				}
			}
			else if (ErrorCode.StartsWith("03") || ErrorCode.StartsWith("13"))
			{
				if (!flag2)
				{
					flag2 = true;
				}
			}
			else if (!ErrorCode.StartsWith("11") && ErrorCode.StartsWith("14") && !IsReconnect)
			{
				m_Log("OperationTimeout, IsReconnect=false, 所以不嘗試重新連線");
				throw new Exception(ErrorCode + ", " + ErrorMsg);
			}
			if (flag3 && num2 >= 2)
			{
				flag3 = false;
				flag4 = true;
				if (ServerUrlList.Count > 0)
				{
					if (m_UrlIndex >= ServerUrlList.Count - 1)
					{
						m_UrlIndex = 0;
					}
					else
					{
						m_UrlIndex++;
					}
					m_ServerUrl = ServerUrlList[m_UrlIndex];
					m_Log("WCF TXN, 準備切換至, " + m_ServerUrl);
				}
			}
			try
			{
				m_ReConnection();
				Initialize();
			}
			catch
			{
			}
			num2++;
			if (num > num2)
			{
				cyntecThread.Sleep(3000);
			}
			DateTime now = DateTime.Now;
			if (flag4)
			{
				flag4 = false;
				try
				{
					string text = m_proxy.Heartbeat();
					m_Log("主機" + m_ServerUrl + ",WCF TXN Heartbeat,正常,花費(秒)," + (DateTime.Now - now).TotalSeconds);
				}
				catch (Exception ex2)
				{
					m_Log("主機" + m_ServerUrl + ",WCF TXN Heartbeat,異常,花費(秒)," + (DateTime.Now - now).TotalSeconds + ", " + ex2.Message);
				}
			}
		}
		throw new Exception(ErrorCode + "(" + num2 + "), " + ErrorMsg);
	}

	public virtual byte[] ServiceCommand(byte[] rawData)
	{
		try
		{
			ExecuteStartTime = DateTime.Now;
			IsExecuting = true;
			if (IsHTTP)
			{
				return ServiceCommandHTTP(rawData);
			}
			return ExecuteNetTcpLoop(rawData);
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			ExecuteStartTime = DateTime.Now;
			IsExecuting = false;
		}
	}

	public virtual byte[] ServiceCommand2(byte[] rawData)
	{
		m_LastActiveTime = cyntecDateTime.Now();
		IwcfMessage iwcfMessage = null;
		object obj = null;
		string txnID = "";
		try
		{
			obj = cyntecByteConvert.ByteArrayToObject(rawData);
			if (obj != null)
			{
				iwcfMessage = obj as IwcfMessage;
			}
			if (!IsInitSuccess)
			{
				m_Log("wcf connection 未初始化成功...發送ServiceCommand前初始化");
				try
				{
					Initialize();
				}
				catch (Exception ex)
				{
					throw new Exception("因WCF connection 初始化失敗, 故無法執行 ServiceCommand" + ex.Message);
				}
			}
			txnID = ((iwcfMessage == null) ? "WCF TXN 交易參數無法識別" : (iwcfMessage.msgTxnID + " ,@ " + iwcfMessage.msgID + " ,@ " + cyntecByteConvert.AssemblyName(obj)));
			byte[] array = m_proxy.ServiceCommand(rawData);
			IwcfTransactionResult iwcfTransactionResult = null;
			if (array != null)
			{
				iwcfTransactionResult = cyntecByteConvert.ByteArrayToObject(array) as IwcfTransactionResult;
			}
			if (this.onServiceCommand != null)
			{
				this.onServiceCommand(obj as IwcfTransaction, iwcfTransactionResult, iwcfMessage);
			}
			m_LogTxnResult(iwcfTransactionResult, null, txnID, "");
			return array;
		}
		catch (Exception ex2)
		{
			string text = "";
			text = ((iwcfMessage == null) ? ("WCF TXN 異常(第1次),  異常訊息, " + ex2.Message + Environment.NewLine + ", 異常訊息, ") : ("WCF TXN 異常(第1次), " + iwcfMessage.msgTxnID + ", 異常訊息, " + ex2.Message));
			m_LogError(text);
			if (!IsReconnect && text.Contains("OperationTimeout"))
			{
				throw new Exception(text);
			}
			try
			{
				m_LastActiveTime = cyntecDateTime.Now();
				m_ReConnection();
				Initialize();
				byte[] array2 = m_proxy.ServiceCommand(rawData);
				IwcfTransactionResult result = null;
				if (array2 != null)
				{
					result = cyntecByteConvert.ByteArrayToObject(array2) as IwcfTransactionResult;
				}
				m_LogTxnResult(result, null, txnID, "(第2次)");
				return array2;
			}
			catch (Exception ex3)
			{
				text = "ServiceCommand第2次NG, " + ex3.Message;
				if (ServerUrlList.Count > 0)
				{
					if (m_UrlIndex >= ServerUrlList.Count - 1)
					{
						m_UrlIndex = 0;
					}
					else
					{
						m_UrlIndex++;
					}
					m_ServerUrl = ServerUrlList[m_UrlIndex];
					text = text + "WCF準備切換至," + m_ServerUrl;
				}
				m_LogError(text);
				throw new Exception(text);
			}
		}
	}

	public void Broadcast(string receverAddress, byte[] obj)
	{
		if (obj == null)
		{
			m_LogError("wcfServiceConnections Broadcast Broadcast 參數不可為null");
			throw new Exception("Broadcast 參數不可為null");
		}
		m_LastActiveTime = cyntecDateTime.Now();
		try
		{
			m_proxy.Broadcast(receverAddress, obj);
		}
		catch (Exception ex)
		{
			m_LogError("wcfServiceConnections Broadcast" + ex.Message);
			cyntecConsol.LogMsg("Broadcast 發生錯誤: " + ex.Message);
		}
	}

	public virtual string Initialize()
	{
		m_Log("準備連線, " + ServerUrl);
		if (IsHTTP)
		{
			m_Log("HTTP執行Initialize");
			IsInitSuccess = true;
			Session_Id = Guid.NewGuid().ToString();
			return Session_Id;
		}
		m_LastActiveTime = cyntecDateTime.Now();
		if (m_proxy.State == CommunicationState.Created)
		{
			Session_Id = m_proxy.Initialize(Client_Id, Session_Id, Address);
		}
		m_ServiceName = m_proxy.GetServiceName();
		if (m_BroadcastThread != null)
		{
			m_BroadcastThread.Abort();
		}
		m_BroadcastThread = new cyntecThreadBuffer<byte[]>();
		m_BroadcastThread.Name = "wcfServiceConnection.m_BroadcastThread_onProcess," + Client_Id;
		m_BroadcastThread.Description = "create on Initialize";
		m_BroadcastThread.onProcess -= m_BroadcastThread_onProcess;
		m_BroadcastThread.onProcess += m_BroadcastThread_onProcess;
		m_BroadcastThread.SleepTime = 1;
		m_BroadcastThread.Start();
		m_Log("wcfServiceConnection Initialize");
		m_Log("wcfServiceConnection Client_Id=" + Client_Id);
		m_Log("wcfServiceConnection Session_Id=" + Session_Id);
		m_Log("wcfServiceConnection Address=" + Address);
		m_Log("wcfServiceConnection Initialize broadcast test ...");
		wcfHelloConnection wcfHelloConnection2 = new wcfHelloConnection();
		wcfHelloConnection2.msgFrom = Client_Id;
		wcfHelloConnection2.msgTo = m_ServerUrl;
		wcfHelloConnection2.Message = "Hello, this is conection init message from " + Client_Id + " :: " + Session_Id;
		Broadcast(wcfHelloConnection2.msgTo, wcfHelloConnection2.ToByte());
		IsInitSuccess = true;
		return Session_Id;
	}

	private void m_BroadcastThread_onProcess()
	{
		byte[] array = null;
		try
		{
			lock (m_BroadcastThread)
			{
				while (m_BroadcastThread.BufferCount > 0 && m_BroadcastThread.BufferCount != 0)
				{
					array = m_BroadcastThread.Dequeue();
					if (array == null)
					{
						break;
					}
					if (this.onBroadcast != null)
					{
						this.onBroadcast(null, array);
					}
					object obj = cyntecByteConvert.ByteArrayToObject(array);
					if (this.onBroadcastMessage != null && obj is IwcfMessage)
					{
						IwcfMessage msg = obj as IwcfMessage;
						this.onBroadcastMessage(msg);
					}
				}
			}
		}
		catch (Exception ex)
		{
			m_LogError("wcfServiceConnection m_BroadcastThread_onProcess " + ex.Message);
		}
	}

	public void Stop()
	{
		if (m_HeartbeatThread != null)
		{
			m_HeartbeatThread.Stop();
		}
	}

	public void CloseSession()
	{
		if (m_BroadcastThread != null)
		{
			m_BroadcastThread.Stop();
			m_BroadcastThread.Abort();
		}
		if (m_HeartbeatThread != null)
		{
			m_HeartbeatThread.Stop();
			m_HeartbeatThread.Abort();
			m_HeartbeatThread = null;
		}
		try
		{
			if (m_proxy != null)
			{
				m_proxy.Close();
				m_proxy = null;
			}
		}
		catch (Exception ex)
		{
			m_LogError(ex.Message);
		}
		try
		{
			if (context != null)
			{
				context.Close();
				context = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (m_CallBack != null)
			{
				m_CallBack.onBroadcast -= m_CallBack_onBroadcast;
				m_CallBack = null;
			}
		}
		catch (Exception)
		{
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		GC.Collect();
	}
}
