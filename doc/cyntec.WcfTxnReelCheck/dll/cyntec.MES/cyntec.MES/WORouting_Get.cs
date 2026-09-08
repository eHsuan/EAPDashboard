using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WORouting_Get : TransactionBase<WORouting_Get, WORouting_GetResult>
{
	public string _WoNo = "";

	public WorkOrder _WO = null;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
