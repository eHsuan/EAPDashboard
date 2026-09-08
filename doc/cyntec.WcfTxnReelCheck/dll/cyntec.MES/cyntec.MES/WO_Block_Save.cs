using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Block_Save : TransactionBase<WO_Block_Save, WO_Block_SaveResult>
{
	public List<WO_Block> _ItemList = new List<WO_Block>();

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
