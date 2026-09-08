using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.serviceModbus;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "CyntecMES", ConfigurationName = "serviceModbus.ModBusSoap")]
public interface ModBusSoap
{
	[OperationContract(Action = "CyntecMES/SelectEQP_IPPCMAPPINGHOST", ReplyAction = "*")]
	SelectEQP_IPPCMAPPINGHOSTResponse SelectEQP_IPPCMAPPINGHOST(SelectEQP_IPPCMAPPINGHOSTRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_EQP_IPPCMAPPINGHOST", ReplyAction = "*")]
	InsertEQP_EQP_IPPCMAPPINGHOSTResponse InsertEQP_EQP_IPPCMAPPINGHOST(InsertEQP_EQP_IPPCMAPPINGHOSTRequest request);

	[OperationContract(Action = "CyntecMES/UpdateEQP_EQP_IPPCMAPPINGHOST", ReplyAction = "*")]
	UpdateEQP_EQP_IPPCMAPPINGHOSTResponse UpdateEQP_EQP_IPPCMAPPINGHOST(UpdateEQP_EQP_IPPCMAPPINGHOSTRequest request);

	[OperationContract(Action = "CyntecMES/DeleteEQP_EQP_IPPCMAPPINGHOST", ReplyAction = "*")]
	DeleteEQP_EQP_IPPCMAPPINGHOSTResponse DeleteEQP_EQP_IPPCMAPPINGHOST(DeleteEQP_EQP_IPPCMAPPINGHOSTRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_IPPCMAPPING", ReplyAction = "*")]
	SelectEQP_IPPCMAPPINGResponse SelectEQP_IPPCMAPPING(SelectEQP_IPPCMAPPINGRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_IPPCMAPPING_JOIN_MACH", ReplyAction = "*")]
	SelectEQP_IPPCMAPPING_JOIN_MACHResponse SelectEQP_IPPCMAPPING_JOIN_MACH(SelectEQP_IPPCMAPPING_JOIN_MACHRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_EQP_IPPCMAPPING", ReplyAction = "*")]
	InsertEQP_EQP_IPPCMAPPINGResponse InsertEQP_EQP_IPPCMAPPING(InsertEQP_EQP_IPPCMAPPINGRequest request);

	[OperationContract(Action = "CyntecMES/UpdateEQP_EQP_IPPCMAPPING", ReplyAction = "*")]
	UpdateEQP_EQP_IPPCMAPPINGResponse UpdateEQP_EQP_IPPCMAPPING(UpdateEQP_EQP_IPPCMAPPINGRequest request);

	[OperationContract(Action = "CyntecMES/DeleteEQP_EQP_IPPCMAPPING", ReplyAction = "*")]
	DeleteEQP_EQP_IPPCMAPPINGResponse DeleteEQP_EQP_IPPCMAPPING(DeleteEQP_EQP_IPPCMAPPINGRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_EQP_IPPCMAPPINGVar", ReplyAction = "*")]
	SelectEQP_EQP_IPPCMAPPINGVarResponse SelectEQP_EQP_IPPCMAPPINGVar(SelectEQP_EQP_IPPCMAPPINGVarRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_EQP_IPPCMAPPINGVar_Single", ReplyAction = "*")]
	SelectEQP_EQP_IPPCMAPPINGVar_SingleResponse SelectEQP_EQP_IPPCMAPPINGVar_Single(SelectEQP_EQP_IPPCMAPPINGVar_SingleRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_EQP_IPPCMAPPINGVar", ReplyAction = "*")]
	InsertEQP_EQP_IPPCMAPPINGVarResponse InsertEQP_EQP_IPPCMAPPINGVar(InsertEQP_EQP_IPPCMAPPINGVarRequest request);

	[OperationContract(Action = "CyntecMES/UpdateEQP_EQP_IPPCMAPPINGVar", ReplyAction = "*")]
	UpdateEQP_EQP_IPPCMAPPINGVarResponse UpdateEQP_EQP_IPPCMAPPINGVar(UpdateEQP_EQP_IPPCMAPPINGVarRequest request);

	[OperationContract(Action = "CyntecMES/DeleteEQP_EQP_IPPCMAPPINGVar", ReplyAction = "*")]
	DeleteEQP_EQP_IPPCMAPPINGVarResponse DeleteEQP_EQP_IPPCMAPPINGVar(DeleteEQP_EQP_IPPCMAPPINGVarRequest request);

	[OperationContract(Action = "CyntecMES/ChangeEQP_EQP_IPPCMAPPINGVarIndex", ReplyAction = "*")]
	ChangeEQP_EQP_IPPCMAPPINGVarIndexResponse ChangeEQP_EQP_IPPCMAPPINGVarIndex(ChangeEQP_EQP_IPPCMAPPINGVarIndexRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_EQP_IPPCMAPPINGPcCmd", ReplyAction = "*")]
	SelectEQP_EQP_IPPCMAPPINGPcCmdResponse SelectEQP_EQP_IPPCMAPPINGPcCmd(SelectEQP_EQP_IPPCMAPPINGPcCmdRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_EQP_IPPCMAPPINGPcCmd", ReplyAction = "*")]
	InsertEQP_EQP_IPPCMAPPINGPcCmdResponse InsertEQP_EQP_IPPCMAPPINGPcCmd(InsertEQP_EQP_IPPCMAPPINGPcCmdRequest request);

	[OperationContract(Action = "CyntecMES/UpdateEQP_EQP_IPPCMAPPINGPcCmd", ReplyAction = "*")]
	UpdateEQP_EQP_IPPCMAPPINGPcCmdResponse UpdateEQP_EQP_IPPCMAPPINGPcCmd(UpdateEQP_EQP_IPPCMAPPINGPcCmdRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_EQP_IPPCMAPPINGAPIList", ReplyAction = "*")]
	SelectEQP_EQP_IPPCMAPPINGAPIListResponse SelectEQP_EQP_IPPCMAPPINGAPIList(SelectEQP_EQP_IPPCMAPPINGAPIListRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_EQP_IPPCMAPPINGAPIList_P", ReplyAction = "*")]
	SelectEQP_EQP_IPPCMAPPINGAPIList_PResponse SelectEQP_EQP_IPPCMAPPINGAPIList_P(SelectEQP_EQP_IPPCMAPPINGAPIList_PRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_EQP_IPPCMAPPINGAPIList", ReplyAction = "*")]
	InsertEQP_EQP_IPPCMAPPINGAPIListResponse InsertEQP_EQP_IPPCMAPPINGAPIList(InsertEQP_EQP_IPPCMAPPINGAPIListRequest request);

	[OperationContract(Action = "CyntecMES/UpdateEQP_EQP_IPPCMAPPINGAPIList", ReplyAction = "*")]
	UpdateEQP_EQP_IPPCMAPPINGAPIListResponse UpdateEQP_EQP_IPPCMAPPINGAPIList(UpdateEQP_EQP_IPPCMAPPINGAPIListRequest request);

	[OperationContract(Action = "CyntecMES/DeleteEQP_EQP_IPPCMAPPINGAPIList", ReplyAction = "*")]
	DeleteEQP_EQP_IPPCMAPPINGAPIListResponse DeleteEQP_EQP_IPPCMAPPINGAPIList(DeleteEQP_EQP_IPPCMAPPINGAPIListRequest request);

	[OperationContract(Action = "CyntecMES/SelectEQP_IPPCMAPPINGHOSTUser", ReplyAction = "*")]
	SelectEQP_IPPCMAPPINGHOSTUserResponse SelectEQP_IPPCMAPPINGHOSTUser(SelectEQP_IPPCMAPPINGHOSTUserRequest request);

	[OperationContract(Action = "CyntecMES/InsertEQP_IPPCMAPPINGHOSTUser", ReplyAction = "*")]
	InsertEQP_IPPCMAPPINGHOSTUserResponse InsertEQP_IPPCMAPPINGHOSTUser(InsertEQP_IPPCMAPPINGHOSTUserRequest request);

	[OperationContract(Action = "CyntecMES/DeleteEQP_IPPCMAPPINGHOSTUser", ReplyAction = "*")]
	DeleteEQP_IPPCMAPPINGHOSTUserResponse DeleteEQP_IPPCMAPPINGHOSTUser(DeleteEQP_IPPCMAPPINGHOSTUserRequest request);
}
