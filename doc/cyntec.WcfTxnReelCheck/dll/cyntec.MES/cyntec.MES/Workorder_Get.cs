using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class Workorder_Get : TransactionBase<Workorder_Get, WorkOrder_GetResult>
{
	public string _WoNo = "";

	public string _BatchNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
