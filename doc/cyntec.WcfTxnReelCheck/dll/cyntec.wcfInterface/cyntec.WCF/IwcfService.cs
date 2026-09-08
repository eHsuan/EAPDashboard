using System;
using System.ServiceModel;

namespace cyntec.WCF;

[ServiceContract(Namespace = "cyntec.WCF", CallbackContract = typeof(IwcfServiceCallback), SessionMode = SessionMode.Required)]
public interface IwcfService
{
	string Client_Id { get; }

	string Session_Id { get; }

	DateTime StartTime { get; }

	DateTime LastBroadcastTime { get; }

	DateTime LastHeartBeatTime { get; }

	DateTime LastActiveTime { get; set; }

	double IdleTime { get; }

	bool IsSalfReply { get; set; }

	string Address { get; }

	ulong UsageCount { get; set; }

	ulong ExceptionCount { get; set; }

	string LastException { get; set; }

	string LastExceptionTxnID { get; set; }

	BroadCastDelegate BroadCastCallProcess { get; }

	event CommandDelegate onHostCommand;

	event CommandDelegate onServiceCommand;

	[OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
	string Initialize(string clientId, string sessionId, string homeAddress);

	[OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
	void CloseSession();

	[OperationContract]
	string GetServiceName();

	[OperationContract]
	byte[] HostCommand(byte[] cmd);

	[OperationContract]
	byte[] ServiceCommand(byte[] cmd);

	[OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = false)]
	void Broadcast(string receiverAddress, byte[] msg);

	[OperationContract]
	string Heartbeat();
}
