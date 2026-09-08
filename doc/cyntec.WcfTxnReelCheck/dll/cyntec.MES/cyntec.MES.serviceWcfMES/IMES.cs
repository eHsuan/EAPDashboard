using System.CodeDom.Compiler;
using System.ServiceModel;

namespace cyntec.MES.serviceWcfMES;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "serviceWcfMES.IMES")]
public interface IMES
{
	[OperationContract(Action = "http://tempuri.org/IMES/Workorder_Get", ReplyAction = "http://tempuri.org/IMES/Workorder_GetResponse")]
	Workorder_GetResult Workorder_Get(Workorder_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/AccountUser_Get", ReplyAction = "http://tempuri.org/IMES/AccountUser_GetResponse")]
	AccountUser_GetResult AccountUser_Get(AccountUser_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/MESUser_Get", ReplyAction = "http://tempuri.org/IMES/MESUser_GetResponse")]
	MESUser_GetResult MESUser_Get(MESUser_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_Leave_Get", ReplyAction = "http://tempuri.org/IMES/WO_Leave_GetResponse")]
	WO_Leave_GetResult WO_Leave_Get(WO_Leave_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelIDLog_Add", ReplyAction = "http://tempuri.org/IMES/WO_ReelIDLog_AddResponse")]
	WO_ReelIDLog_AddResult WO_ReelIDLog_Add(WO_ReelIDLog_AddCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/EQP_TapingUILog_Save", ReplyAction = "http://tempuri.org/IMES/EQP_TapingUILog_SaveResponse")]
	EQP_TapingUILog_SaveResult EQP_TapingUILog_Save(EQP_TapingUILog_SaveCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/EQP_TapingUILog_Get", ReplyAction = "http://tempuri.org/IMES/EQP_TapingUILog_GetResponse")]
	EQP_TapingUILog_GetResult EQP_TapingUILog_Get(EQP_TapingUILog_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WoInfo_Get", ReplyAction = "http://tempuri.org/IMES/WoInfo_GetResponse")]
	WoInfo_GetResult WoInfo_Get(WoInfo_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_BOM_Get", ReplyAction = "http://tempuri.org/IMES/WO_BOM_GetResponse")]
	WO_BOM_GetResult WO_BOM_Get(WO_BOM_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelID_Get", ReplyAction = "http://tempuri.org/IMES/WO_ReelID_GetResponse")]
	WO_ReelID_GetResult WO_ReelID_Get(WO_ReelID_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/MatView_Get", ReplyAction = "http://tempuri.org/IMES/MatView_GetResponse")]
	MatView_GetResult MatView_Get(MatView_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/MES_ParamItem_Get", ReplyAction = "http://tempuri.org/IMES/MES_ParamItem_GetResponse")]
	MES_ParamItem_GetResult MES_ParamItem_Get(MES_ParamItem_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/MachineRecord_Save", ReplyAction = "http://tempuri.org/IMES/MachineRecord_SaveResponse")]
	MachineRecord_SaveResult MachineRecord_Save(MachineRecord_SaveCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_CheckIn", ReplyAction = "http://tempuri.org/IMES/WO_CheckInResponse")]
	WO_CheckInResult WO_CheckIn(WO_CheckInCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_InputCode_Get", ReplyAction = "http://tempuri.org/IMES/WO_InputCode_GetResponse")]
	WO_InputCode_GetResult WO_InputCode_Get(WO_InputCode_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_Mat_Add", ReplyAction = "http://tempuri.org/IMES/WO_Mat_AddResponse")]
	WO_Mat_AddResult WO_Mat_Add(WO_Mat_AddCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/TraceMatCode_Get", ReplyAction = "http://tempuri.org/IMES/TraceMatCode_GetResponse")]
	TraceMatCode_GetResult TraceMatCode_Get(TraceMatCode_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelCheck_Save", ReplyAction = "http://tempuri.org/IMES/WO_ReelCheck_SaveResponse")]
	WO_ReelCheck_SaveResult WO_ReelCheck_Save(WO_ReelCheck_SaveCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelCheck_Get", ReplyAction = "http://tempuri.org/IMES/WO_ReelCheck_GetResponse")]
	WO_ReelCheck_GetResult WO_ReelCheck_Get(WO_ReelCheck_GetCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelID_Get_WCF_TEMP", ReplyAction = "http://tempuri.org/IMES/WO_ReelID_Get_WCF_TEMPResponse")]
	WO_ReelID_Get_WCF_TEMPResult WO_ReelID_Get_WCF_TEMP(WO_ReelID_Get_WCF_TEMPCondition condition);

	[OperationContract(Action = "http://tempuri.org/IMES/WO_ReelID_Update_WCF_TEMP", ReplyAction = "http://tempuri.org/IMES/WO_ReelID_Update_WCF_TEMPResponse")]
	WO_ReelID_Update_WCF_TEMPResult WO_ReelID_Update_WCF_TEMP(WO_ReelID_Update_WCF_TEMPCondition condition);
}
