using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.ReelID;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://cncynmes/", ConfigurationName = "ReelID.ReelIDSoap")]
public interface ReelIDSoap
{
	[OperationContract(Action = "http://cncynmes/HelloWorld", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	string HelloWorld();

	[OperationContract(Action = "http://cncynmes/GetDecodeBarcode", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReelBarcode GetDecodeBarcode(string encodeBarcode);

	[OperationContract(Action = "http://cncynmes/UpdateReelID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReturnMsg UpdateReelID(string reelID, string workCenterTypeCode, string statusCode, string statusRemark, string updateUser);

	[OperationContract(Action = "http://cncynmes/UpdateReelID2", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReturnMsg UpdateReelID2(string reelID, string workCenterTypeCode, string statusCode, string statusRemark, string updateUser, string logKeyword);

	[OperationContract(Action = "http://cncynmes/UpdateTPReelID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReturnMsg UpdateTPReelID(string reelID, string statusCode, string statusRemark, string machNo, string startTime, string endTime, string updateUser);

	[OperationContract(Action = "http://cncynmes/UpdateTPReelID2", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReturnMsg UpdateTPReelID2(string reelID, string statusCode, string statusRemark, string machNo, string startTime, string endTime, string updateUser, string logKeyword);

	[OperationContract(Action = "http://cncynmes/GetReelStatus", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	ReelStatus[] GetReelStatus();

	[OperationContract(Action = "http://cncynmes/GetByReelID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	WO_ReelID GetByReelID(string reelID);

	[OperationContract(Action = "http://cncynmes/GetByReelID2", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	WO_ReelID GetByReelID2(string reelID, string logKeyword);

	[OperationContract(Action = "http://cncynmes/GetListByWONo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	WO_ReelID[] GetListByWONo(string woNo);

	[OperationContract(Action = "http://cncynmes/AddLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	bool AddLog(WO_ReelIDLog pModel);

	[OperationContract(Action = "http://cncynmes/GetLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	WO_ReelIDLog[] GetLog(string pReelID);
}
