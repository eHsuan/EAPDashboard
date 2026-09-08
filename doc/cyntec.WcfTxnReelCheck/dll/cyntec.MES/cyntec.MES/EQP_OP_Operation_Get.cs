using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Operation_Get : TransactionBase<EQP_OP_Operation_Get, EQP_OP_Operation_GetResult>
{
	public string _Operation = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
