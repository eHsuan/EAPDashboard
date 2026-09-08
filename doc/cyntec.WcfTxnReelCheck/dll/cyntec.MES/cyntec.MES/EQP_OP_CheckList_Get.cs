using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_CheckList_Get : TransactionBase<EQP_OP_CheckList_Get, EQP_OP_CheckList_GetResult>
{
	public int _MachID = 0;

	public string _MachNo = "";

	public string _WoNo = "";

	public int _FlowID = 0;

	public string _WorkcenterNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
