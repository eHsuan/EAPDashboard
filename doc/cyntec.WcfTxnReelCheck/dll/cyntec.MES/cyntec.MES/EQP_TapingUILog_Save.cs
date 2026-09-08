using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_TapingUILog_Save : TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>
{
	public EQP_TapingUILog _TapingLog;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
