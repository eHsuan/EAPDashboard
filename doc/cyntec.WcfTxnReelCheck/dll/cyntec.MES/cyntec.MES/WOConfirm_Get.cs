using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WOConfirm_Get : TransactionBase<WOConfirm_Get, WOConfirm_GetResult>
{
	public string _WoNo = "";

	public WorkOrder _WO = null;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
