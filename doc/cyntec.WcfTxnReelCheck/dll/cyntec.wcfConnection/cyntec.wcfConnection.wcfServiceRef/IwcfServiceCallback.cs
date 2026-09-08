using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.wcfConnection.wcfServiceRef;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IwcfServiceCallback
{
	[OperationContract(IsOneWay = true, Action = "cyntec.WCF/IwcfService/SystemBroadcast")]
	void SystemBroadcast(string rawData);

	[OperationContract(IsOneWay = true, Action = "cyntec.WCF/IwcfService/SystemCommandReply")]
	void SystemCommandReply(string msg);

	[OperationContract(IsOneWay = true, Action = "cyntec.WCF/IwcfService/ObjectBroadCast")]
	void ObjectBroadCast(byte[] rawData);
}
