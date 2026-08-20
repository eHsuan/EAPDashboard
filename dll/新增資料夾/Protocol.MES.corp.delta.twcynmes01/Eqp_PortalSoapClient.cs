using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace Protocol.MES.corp.delta.twcynmes01;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "8.0.0")]
public class Eqp_PortalSoapClient : ClientBase<Eqp_PortalSoap>, Eqp_PortalSoap
{
	public enum EndpointConfiguration
	{
		Eqp_PortalSoap,
		Eqp_PortalSoap12
	}

	public Eqp_PortalSoapClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public Eqp_PortalSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public Eqp_PortalSoapClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public Eqp_PortalSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Task<string> EqpTransactionAsync(string pParameter)
	{
		return base.Channel.EqpTransactionAsync(pParameter);
	}

	public Task<string> InsertEqpErrorLogAsync(string pParameter)
	{
		return base.Channel.InsertEqpErrorLogAsync(pParameter);
	}

	public Task<string> InsertEqpTransactionLogAsync(string pParameter)
	{
		return base.Channel.InsertEqpTransactionLogAsync(pParameter);
	}

	public Task<string> EqpStatusRefreshAsync(string pParameter)
	{
		return base.Channel.EqpStatusRefreshAsync(pParameter);
	}

	public Task<string> InsertParameterAsync(string pParameter)
	{
		return base.Channel.InsertParameterAsync(pParameter);
	}

	public Task<string> CheckUserCertAsync(string pParameter)
	{
		return base.Channel.CheckUserCertAsync(pParameter);
	}

	public Task<string> CheckUserAsync(string pParameter)
	{
		return base.Channel.CheckUserAsync(pParameter);
	}

	public Task<string> CheckWOAsync(string pParameter)
	{
		return base.Channel.CheckWOAsync(pParameter);
	}

	public Task<string> CheckTraceMatAsync(string pParameter)
	{
		return base.Channel.CheckTraceMatAsync(pParameter);
	}

	public Task<string> CheckWOAlreadyCheckInAsync(string pParameter)
	{
		return base.Channel.CheckWOAlreadyCheckInAsync(pParameter);
	}

	public Task<string> WOQryAsync(string pParameter)
	{
		return base.Channel.WOQryAsync(pParameter);
	}

	public Task<string> WOInCheckAsync(string pParameter)
	{
		return base.Channel.WOInCheckAsync(pParameter);
	}

	public Task<string> WO_CheckAlreadyInAsync(string pParameter)
	{
		return base.Channel.WO_CheckAlreadyInAsync(pParameter);
	}

	public Task<string> WO_InsertUseMachAsync(string pParameter)
	{
		return base.Channel.WO_InsertUseMachAsync(pParameter);
	}

	public Task<string> WOCheckInAsync(string pParameter)
	{
		return base.Channel.WOCheckInAsync(pParameter);
	}

	public Task<string> WOCheckIn_JustCheckAsync(string pParameter)
	{
		return base.Channel.WOCheckIn_JustCheckAsync(pParameter);
	}

	public Task<string> WOParmCodeQryAsync(string pParameter)
	{
		return base.Channel.WOParmCodeQryAsync(pParameter);
	}

	public Task<string> GetParaAsync(string pParameter)
	{
		return base.Channel.GetParaAsync(pParameter);
	}

	public Task<string> GetWorkCenterAllParaAsync(string pParameter)
	{
		return base.Channel.GetWorkCenterAllParaAsync(pParameter);
	}

	public Task<string> EqpMatAddAsync(string pParameter)
	{
		return base.Channel.EqpMatAddAsync(pParameter);
	}

	public Task<string> EqpMatAdd_BatchAsync(string pParameter)
	{
		return base.Channel.EqpMatAdd_BatchAsync(pParameter);
	}

	public Task<string> EqpMatAdd_Batch_SkipRepeatAsync(string pParameter)
	{
		return base.Channel.EqpMatAdd_Batch_SkipRepeatAsync(pParameter);
	}

	public Task<string> EqpWOInputAddAsync(string pParameter)
	{
		return base.Channel.EqpWOInputAddAsync(pParameter);
	}

	public Task<string> EqpWOInputAdd_BatchAsync(string pParameter)
	{
		return base.Channel.EqpWOInputAdd_BatchAsync(pParameter);
	}

	public Task<string> EqpCarrierAddAsync(string pParameter)
	{
		return base.Channel.EqpCarrierAddAsync(pParameter);
	}

	public Task<string> EqpCarrierAdd_BatchAsync(string pParameter)
	{
		return base.Channel.EqpCarrierAdd_BatchAsync(pParameter);
	}

	public Task<string> EqpPartProcessAsync(string pParameter)
	{
		return base.Channel.EqpPartProcessAsync(pParameter);
	}

	public Task<string> EqpPartProcess_BatchAsync(string pParameter)
	{
		return base.Channel.EqpPartProcess_BatchAsync(pParameter);
	}

	public Task<string> EqpTestValueRecordAsync(string pParameter)
	{
		return base.Channel.EqpTestValueRecordAsync(pParameter);
	}

	public Task<string> MouldInfoAsync(string pParameter)
	{
		return base.Channel.MouldInfoAsync(pParameter);
	}

	public Task<string> MouldInfo_ByIDAsync(string pParameter)
	{
		return base.Channel.MouldInfo_ByIDAsync(pParameter);
	}

	public Task<string> MouldOnlineAsync(string pParameter)
	{
		return base.Channel.MouldOnlineAsync(pParameter);
	}

	public Task<string> MouldOnlineByIDAsync(string pParameter)
	{
		return base.Channel.MouldOnlineByIDAsync(pParameter);
	}

	public Task<string> GetMoverRsnAsync(string pParameter)
	{
		return base.Channel.GetMoverRsnAsync(pParameter);
	}

	public Task<string> MouldOffLineAsync(string pParameter)
	{
		return base.Channel.MouldOffLineAsync(pParameter);
	}

	public Task<string> MouldOffLine_ByIDAsync(string pParameter)
	{
		return base.Channel.MouldOffLine_ByIDAsync(pParameter);
	}

	public Task<string> MouldOutputUpdateAsync(string pParameter)
	{
		return base.Channel.MouldOutputUpdateAsync(pParameter);
	}

	public Task<string> MouldOutputUpdate_ByIDAsync(string pParameter)
	{
		return base.Channel.MouldOutputUpdate_ByIDAsync(pParameter);
	}

	public Task<string> MachOnlineMouldQueryAsync(string pParameter)
	{
		return base.Channel.MachOnlineMouldQueryAsync(pParameter);
	}

	public Task<string> UploadTestResultFileAsync(string pParameter)
	{
		return base.Channel.UploadTestResultFileAsync(pParameter);
	}

	public Task<string> CarrierInAsync(string pParameter)
	{
		return base.Channel.CarrierInAsync(pParameter);
	}

	public Task<string> WoCheckInCarrierCheckAsync(string pParameter)
	{
		return base.Channel.WoCheckInCarrierCheckAsync(pParameter);
	}

	public Task<string> WoCheckOutCarrierCheckAsync(string pParameter)
	{
		return base.Channel.WoCheckOutCarrierCheckAsync(pParameter);
	}

	public Task<string> GetWONoByToolNoAsync(string pParameter)
	{
		return base.Channel.GetWONoByToolNoAsync(pParameter);
	}

	public Task<bool> ConnectionTestAsync()
	{
		return base.Channel.ConnectionTestAsync();
	}

	public Task<string> MES_CarrierInAsync(string pParameter)
	{
		return base.Channel.MES_CarrierInAsync(pParameter);
	}

	public Task<string> MES_CarrierIn_JustCheckAsync(string pParameter)
	{
		return base.Channel.MES_CarrierIn_JustCheckAsync(pParameter);
	}

	public Task<string> MES_CheckEdcRawDataAsync(string pParameter)
	{
		return base.Channel.MES_CheckEdcRawDataAsync(pParameter);
	}

	public Task<string> WOBomQryAsync(string pParameter)
	{
		return base.Channel.WOBomQryAsync(pParameter);
	}

	public virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), (Action<IAsyncResult>)((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		switch (endpointConfiguration)
		{
		case EndpointConfiguration.Eqp_PortalSoap:
		{
			BasicHttpBinding val4 = new BasicHttpBinding();
			((HttpBindingBase)val4).MaxBufferSize = int.MaxValue;
			((HttpBindingBase)val4).ReaderQuotas = XmlDictionaryReaderQuotas.Max;
			((HttpBindingBase)val4).MaxReceivedMessageSize = 2147483647L;
			((HttpBindingBase)val4).AllowCookies = true;
			return (Binding)(object)val4;
		}
		case EndpointConfiguration.Eqp_PortalSoap12:
		{
			CustomBinding val = new CustomBinding();
			TextMessageEncodingBindingElement val2 = new TextMessageEncodingBindingElement();
			((MessageEncodingBindingElement)val2).MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
			((Collection<BindingElement>)(object)val.Elements).Add((BindingElement)(object)val2);
			HttpTransportBindingElement val3 = new HttpTransportBindingElement();
			val3.AllowCookies = true;
			val3.MaxBufferSize = int.MaxValue;
			((TransportBindingElement)val3).MaxReceivedMessageSize = 2147483647L;
			((Collection<BindingElement>)(object)val.Elements).Add((BindingElement)(object)val3);
			return (Binding)(object)val;
		}
		default:
			throw new InvalidOperationException($"找不到名為 '{endpointConfiguration}' 的端點。");
		}
	}

	private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		return (EndpointAddress)(endpointConfiguration switch
		{
			EndpointConfiguration.Eqp_PortalSoap => (object)new EndpointAddress("http://twcynmes01.delta.corp/CyntecDataCenter/service/Eqp/Eqp_Portal.asmx"), 
			EndpointConfiguration.Eqp_PortalSoap12 => (object)new EndpointAddress("http://twcynmes01.delta.corp/CyntecDataCenter/service/Eqp/Eqp_Portal.asmx"), 
			_ => throw new InvalidOperationException($"找不到名為 '{endpointConfiguration}' 的端點。"), 
		});
	}
}
