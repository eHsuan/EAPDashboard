using System.ServiceModel;

namespace cyntec.WCF;

[ServiceContract]
public interface IwcfServiceCallback
{
	[OperationContract(IsOneWay = true)]
	void SystemBroadcast(string rawData);

	[OperationContract(IsOneWay = true)]
	void SystemCommandReply(string msg);

	[OperationContract(IsOneWay = true)]
	void ObjectBroadCast(byte[] rawData);
}
