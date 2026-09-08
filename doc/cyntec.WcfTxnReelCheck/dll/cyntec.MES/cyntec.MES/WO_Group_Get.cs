using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Group_Get : TransactionBase<WO_Group_Get, WO_Group_GetResult>
{
	public string _WoNo = "";

	public WorkOrder _WO = null;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
