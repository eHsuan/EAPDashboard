using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.wcfConnection.wcfServiceRef;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "cyntec.WCF", ConfigurationName = "wcfServiceRef.IwcfService", CallbackContract = typeof(IwcfServiceCallback), SessionMode = SessionMode.Required)]
public interface IwcfService
{
	[OperationContract(Action = "cyntec.WCF/IwcfService/Initialize", ReplyAction = "cyntec.WCF/IwcfService/InitializeResponse")]
	string Initialize(string clientId, string sessionId, string homeAddress);

	[OperationContract(IsOneWay = true, IsTerminating = true, IsInitiating = false, Action = "cyntec.WCF/IwcfService/CloseSession")]
	void CloseSession();

	[OperationContract(Action = "cyntec.WCF/IwcfService/GetServiceName", ReplyAction = "cyntec.WCF/IwcfService/GetServiceNameResponse")]
	string GetServiceName();

	[OperationContract(Action = "cyntec.WCF/IwcfService/HostCommand", ReplyAction = "cyntec.WCF/IwcfService/HostCommandResponse")]
	byte[] HostCommand(byte[] cmd);

	[OperationContract(Action = "cyntec.WCF/IwcfService/ServiceCommand", ReplyAction = "cyntec.WCF/IwcfService/ServiceCommandResponse")]
	byte[] ServiceCommand(byte[] cmd);

	[OperationContract(IsInitiating = false, Action = "cyntec.WCF/IwcfService/Broadcast", ReplyAction = "cyntec.WCF/IwcfService/BroadcastResponse")]
	void Broadcast(string receiverAddress, byte[] msg);

	[OperationContract(Action = "cyntec.WCF/IwcfService/Heartbeat", ReplyAction = "cyntec.WCF/IwcfService/HeartbeatResponse")]
	string Heartbeat();
}
