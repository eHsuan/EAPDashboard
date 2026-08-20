using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Protocol.MES.corp.delta.twcynmes01;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "8.0.0")]
[ServiceContract(Namespace = "CyntecMES", ConfigurationName = "Protocol.MES.corp.delta.twcynmes01.Eqp_PortalSoap")]
public interface Eqp_PortalSoap
{
	[OperationContract(Action = "CyntecMES/EqpTransaction", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpTransactionAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/InsertEqpErrorLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> InsertEqpErrorLogAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/InsertEqpTransactionLog", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> InsertEqpTransactionLogAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpStatusRefresh", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpStatusRefreshAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/InsertParameter", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> InsertParameterAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CheckUserCert", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CheckUserCertAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CheckUser", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CheckUserAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CheckWO", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CheckWOAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CheckTraceMat", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CheckTraceMatAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CheckWOAlreadyCheckIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CheckWOAlreadyCheckInAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOQryAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOInCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOInCheckAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WO_CheckAlreadyIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WO_CheckAlreadyInAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WO_InsertUseMach", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WO_InsertUseMachAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOCheckIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOCheckInAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOCheckIn_JustCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOCheckIn_JustCheckAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOParmCodeQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOParmCodeQryAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/GetPara", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> GetParaAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/GetWorkCenterAllPara", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> GetWorkCenterAllParaAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpMatAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpMatAddAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpMatAdd_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpMatAdd_BatchAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpMatAdd_Batch_SkipRepeat", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpMatAdd_Batch_SkipRepeatAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpWOInputAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpWOInputAddAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpWOInputAdd_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpWOInputAdd_BatchAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpCarrierAdd", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpCarrierAddAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpCarrierAdd_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpCarrierAdd_BatchAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpPartProcess", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpPartProcessAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpPartProcess_Batch", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpPartProcess_BatchAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/EqpTestValueRecord", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> EqpTestValueRecordAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldInfo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldInfoAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldInfo_ByID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldInfo_ByIDAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOnline", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOnlineAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOnlineByID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOnlineByIDAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/GetMoverRsn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> GetMoverRsnAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOffLine", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOffLineAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOffLine_ByID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOffLine_ByIDAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOutputUpdate", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOutputUpdateAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MouldOutputUpdate_ByID", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MouldOutputUpdate_ByIDAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MachOnlineMouldQuery", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MachOnlineMouldQueryAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/UploadTestResultFile", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> UploadTestResultFileAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/CarrierIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> CarrierInAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WoCheckInCarrierCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WoCheckInCarrierCheckAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WoCheckOutCarrierCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WoCheckOutCarrierCheckAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/GetWONoByToolNo", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> GetWONoByToolNoAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/ConnectionTest", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<bool> ConnectionTestAsync();

	[OperationContract(Action = "CyntecMES/MES_CarrierIn", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MES_CarrierInAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MES_CarrierIn_JustCheck", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MES_CarrierIn_JustCheckAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/MES_CheckEdcRawData", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> MES_CheckEdcRawDataAsync(string pParameter);

	[OperationContract(Action = "CyntecMES/WOBomQry", ReplyAction = "*")]
	[XmlSerializerFormat(SupportFaults = true)]
	Task<string> WOBomQryAsync(string pParameter);
}
