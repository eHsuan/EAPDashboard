using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WOQueueTime_Get : TransactionBase<WOQueueTime_Get, WOQueueTime_GetResult>
{
	public string _WONo = "";

	public int _WorkCenterFlowId = 0;

	public bool _IsEnable = false;

	public WorkOrder _WO = null;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
