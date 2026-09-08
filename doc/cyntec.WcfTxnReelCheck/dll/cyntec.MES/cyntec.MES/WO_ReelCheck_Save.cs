using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelCheck_Save : TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>
{
	public List<WO_ReelCheck> _WoReelCheckList = new List<WO_ReelCheck>();

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
