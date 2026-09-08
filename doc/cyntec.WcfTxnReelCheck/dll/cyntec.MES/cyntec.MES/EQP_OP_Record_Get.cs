using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Record_Get : TransactionBase<EQP_OP_Record_Get, EQP_OP_Record_GetResult>
{
	public string _SysID = "";

	public string _WoNo = "";

	public int _FlowID = 0;

	public int _MachID = 0;

	public string _Operation = "";

	public string _MatNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
