using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceEQP;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class Eqp_PortalSoapClient : ClientBase<Eqp_PortalSoap>, Eqp_PortalSoap
{
	public Eqp_PortalSoapClient()
	{
	}

	public Eqp_PortalSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public Eqp_PortalSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public Eqp_PortalSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public Eqp_PortalSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string EqpTransaction(string pParameter)
	{
		return base.Channel.EqpTransaction(pParameter);
	}

	public string InsertEqpErrorLog(string pParameter)
	{
		return base.Channel.InsertEqpErrorLog(pParameter);
	}

	public string InsertEqpTransactionLog(string pParameter)
	{
		return base.Channel.InsertEqpTransactionLog(pParameter);
	}
}
