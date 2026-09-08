using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace cyntec.MES.serviceWcfMES;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class MESClient : ClientBase<IMES>, IMES
{
	public MESClient()
	{
	}

	public MESClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public MESClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MESClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MESClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Workorder_GetResult Workorder_Get(Workorder_GetCondition condition)
	{
		return base.Channel.Workorder_Get(condition);
	}

	public AccountUser_GetResult AccountUser_Get(AccountUser_GetCondition condition)
	{
		return base.Channel.AccountUser_Get(condition);
	}

	public MESUser_GetResult MESUser_Get(MESUser_GetCondition condition)
	{
		return base.Channel.MESUser_Get(condition);
	}

	public WO_Leave_GetResult WO_Leave_Get(WO_Leave_GetCondition condition)
	{
		return base.Channel.WO_Leave_Get(condition);
	}

	public WO_ReelIDLog_AddResult WO_ReelIDLog_Add(WO_ReelIDLog_AddCondition condition)
	{
		return base.Channel.WO_ReelIDLog_Add(condition);
	}

	public EQP_TapingUILog_SaveResult EQP_TapingUILog_Save(EQP_TapingUILog_SaveCondition condition)
	{
		return base.Channel.EQP_TapingUILog_Save(condition);
	}

	public EQP_TapingUILog_GetResult EQP_TapingUILog_Get(EQP_TapingUILog_GetCondition condition)
	{
		return base.Channel.EQP_TapingUILog_Get(condition);
	}

	public WoInfo_GetResult WoInfo_Get(WoInfo_GetCondition condition)
	{
		return base.Channel.WoInfo_Get(condition);
	}

	public WO_BOM_GetResult WO_BOM_Get(WO_BOM_GetCondition condition)
	{
		return base.Channel.WO_BOM_Get(condition);
	}

	public WO_ReelID_GetResult WO_ReelID_Get(WO_ReelID_GetCondition condition)
	{
		return base.Channel.WO_ReelID_Get(condition);
	}

	public MatView_GetResult MatView_Get(MatView_GetCondition condition)
	{
		return base.Channel.MatView_Get(condition);
	}

	public MES_ParamItem_GetResult MES_ParamItem_Get(MES_ParamItem_GetCondition condition)
	{
		return base.Channel.MES_ParamItem_Get(condition);
	}

	public MachineRecord_SaveResult MachineRecord_Save(MachineRecord_SaveCondition condition)
	{
		return base.Channel.MachineRecord_Save(condition);
	}

	public WO_CheckInResult WO_CheckIn(WO_CheckInCondition condition)
	{
		return base.Channel.WO_CheckIn(condition);
	}

	public WO_InputCode_GetResult WO_InputCode_Get(WO_InputCode_GetCondition condition)
	{
		return base.Channel.WO_InputCode_Get(condition);
	}

	public WO_Mat_AddResult WO_Mat_Add(WO_Mat_AddCondition condition)
	{
		return base.Channel.WO_Mat_Add(condition);
	}

	public TraceMatCode_GetResult TraceMatCode_Get(TraceMatCode_GetCondition condition)
	{
		return base.Channel.TraceMatCode_Get(condition);
	}

	public WO_ReelCheck_SaveResult WO_ReelCheck_Save(WO_ReelCheck_SaveCondition condition)
	{
		return base.Channel.WO_ReelCheck_Save(condition);
	}

	public WO_ReelCheck_GetResult WO_ReelCheck_Get(WO_ReelCheck_GetCondition condition)
	{
		return base.Channel.WO_ReelCheck_Get(condition);
	}

	public WO_ReelID_Get_WCF_TEMPResult WO_ReelID_Get_WCF_TEMP(WO_ReelID_Get_WCF_TEMPCondition condition)
	{
		return base.Channel.WO_ReelID_Get_WCF_TEMP(condition);
	}

	public WO_ReelID_Update_WCF_TEMPResult WO_ReelID_Update_WCF_TEMP(WO_ReelID_Update_WCF_TEMPCondition condition)
	{
		return base.Channel.WO_ReelID_Update_WCF_TEMP(condition);
	}
}
