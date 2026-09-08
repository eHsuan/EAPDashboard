using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MESSystemParameter_Get : TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>
{
	public MESSystemParameter _Condition = new MESSystemParameter();

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
