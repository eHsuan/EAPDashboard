using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelIDLog_Add : TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>
{
	public WO_ReelIDLog _Condition = new WO_ReelIDLog();

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
