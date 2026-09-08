using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WoInfo_GetResult : TransactionResultBase
{
	public WorkOrder_GetResult _WO = new WorkOrder_GetResult();

	public WOConfirm_GetResult _WOConfirm = new WOConfirm_GetResult();

	public WorkCenter_GetResult _WOCenter = new WorkCenter_GetResult();

	public MatView_GetResult _MatView = new MatView_GetResult();

	public WO_Block_GetResult _WO_Block = new WO_Block_GetResult();

	public WOQueueTime_GetResult _WOQueueTime = new WOQueueTime_GetResult();

	public WORouting_GetResult _WOrouting = new WORouting_GetResult();

	public WO_Group_GetResult _WOGroup = new WO_Group_GetResult();

	public MatGroupNo_GetResult _MatGroupNo = new MatGroupNo_GetResult();
}
