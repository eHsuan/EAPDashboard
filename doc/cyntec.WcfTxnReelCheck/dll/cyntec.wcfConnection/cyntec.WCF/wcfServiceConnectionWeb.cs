using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using cyntec.Base;
using cyntec.wcfConnection.wcfServiceRef;

namespace cyntec.WCF;

public class wcfServiceConnectionWeb : wcfServiceConnection
{
	public wcfServiceConnectionWeb()
	{
	}

	public wcfServiceConnectionWeb(string serverUrl, string ClientUrl, string userId, string password)
	{
		m_ServerUrl = serverUrl;
		m_ClientUrl = ClientUrl;
		m_UserId = userId;
		m_password = password;
		m_ReConnection();
		m_LastActiveTime = cyntecDateTime.Now();
	}

	internal override void m_ReConnection()
	{
		Binding binding = null;
		if (m_ServerUrl.ToUpper().StartsWith("NET.TCP"))
		{
			try
			{
				binding = new NetTcpBinding("cyntec.WCF.wcfServiceBase");
			}
			catch
			{
				binding = new NetTcpBinding();
				Console.WriteLine("WCF.wcfServiceBase設定只能使用預設值(可能沒有檔案或沒有設定)");
			}
			if ((binding as NetTcpBinding).MaxBufferSize == 65536)
			{
				(binding as NetTcpBinding).MaxReceivedMessageSize = 104857600L;
				(binding as NetTcpBinding).MaxBufferPoolSize = 1000000L;
				(binding as NetTcpBinding).ReaderQuotas.MaxStringContentLength = 10000000;
				(binding as NetTcpBinding).ReaderQuotas.MaxArrayLength = 123000000;
				(binding as NetTcpBinding).Security.Mode = SecurityMode.None;
				binding.OpenTimeout = new TimeSpan(0, 0, 3);
				binding.CloseTimeout = new TimeSpan(0, 1, 15);
				binding.ReceiveTimeout = new TimeSpan(0, 1, 15);
				binding.SendTimeout = new TimeSpan(0, 0, 15);
			}
		}
		else
		{
			if (!m_ServerUrl.ToUpper().StartsWith("HTTP"))
			{
				throw new Exception("HOST URL 設定不正確");
			}
			binding = new WSDualHttpBinding();
			(binding as WSDualHttpBinding).ClientBaseAddress = new Uri(m_ClientUrl);
			(binding as WSDualHttpBinding).MaxReceivedMessageSize = 104857600L;
			(binding as WSDualHttpBinding).MaxBufferPoolSize = 1000000L;
			(binding as WSDualHttpBinding).ReaderQuotas.MaxStringContentLength = 10000000;
			(binding as WSDualHttpBinding).ReaderQuotas.MaxArrayLength = 123000000;
			(binding as WSDualHttpBinding).Security.Mode = WSDualHttpSecurityMode.None;
			binding.OpenTimeout = new TimeSpan(0, 0, 3);
			binding.CloseTimeout = new TimeSpan(0, 0, 15);
			binding.SendTimeout = new TimeSpan(0, 0, 15);
		}
		m_CallBack = new wcfServiceCallback();
		context = new InstanceContext(m_CallBack);
		EndpointAddress remoteAddress = new EndpointAddress(m_ServerUrl);
		m_proxy = new IwcfServiceClient(context, binding, remoteAddress);
		m_LastActiveTime = cyntecDateTime.Now();
	}

	public new static wcfServiceConnection NewConnection(string serverUrl, string ClientUrl, string userId, string password)
	{
		return new wcfServiceConnectionWeb(serverUrl, ClientUrl, userId, password);
	}
}
