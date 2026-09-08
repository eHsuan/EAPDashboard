using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.wcfConnection.wcfServiceRef;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class IwcfServiceClient : DuplexClientBase<IwcfService>, IwcfService
{
	public IwcfServiceClient(InstanceContext callbackInstance)
		: base(callbackInstance)
	{
	}

	public IwcfServiceClient(InstanceContext callbackInstance, string endpointConfigurationName)
		: base(callbackInstance, endpointConfigurationName)
	{
	}

	public IwcfServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public IwcfServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public IwcfServiceClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
		: base(callbackInstance, binding, remoteAddress)
	{
	}

	public string Initialize(string clientId, string sessionId, string homeAddress)
	{
		return base.Channel.Initialize(clientId, sessionId, homeAddress);
	}

	public void CloseSession()
	{
		base.Channel.CloseSession();
	}

	public string GetServiceName()
	{
		return base.Channel.GetServiceName();
	}

	public byte[] HostCommand(byte[] cmd)
	{
		return base.Channel.HostCommand(cmd);
	}

	public byte[] ServiceCommand(byte[] cmd)
	{
		return base.Channel.ServiceCommand(cmd);
	}

	public void Broadcast(string receiverAddress, byte[] msg)
	{
		base.Channel.Broadcast(receiverAddress, msg);
	}

	public string Heartbeat()
	{
		return base.Channel.Heartbeat();
	}
}
