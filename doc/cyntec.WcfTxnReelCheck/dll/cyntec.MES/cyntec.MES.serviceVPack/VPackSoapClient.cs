using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceVPack;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class VPackSoapClient : ClientBase<VPackSoap>, VPackSoap
{
	public VPackSoapClient()
	{
	}

	public VPackSoapClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public VPackSoapClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public VPackSoapClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public VPackSoapClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string HelloWorld()
	{
		return base.Channel.HelloWorld();
	}

	public ResultOfString StorBoxBinding(string userID, string password, string woNo, string barcode, string storBoxNo, string empNo, string eqpNo)
	{
		return base.Channel.StorBoxBinding(userID, password, woNo, barcode, storBoxNo, empNo, eqpNo);
	}
}
