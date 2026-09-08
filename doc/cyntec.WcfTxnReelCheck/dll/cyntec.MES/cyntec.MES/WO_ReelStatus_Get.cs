using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelStatus_Get : TransactionBase<WO_ReelStatus_Get, WO_ReelStatus_GetResult>
{
	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
