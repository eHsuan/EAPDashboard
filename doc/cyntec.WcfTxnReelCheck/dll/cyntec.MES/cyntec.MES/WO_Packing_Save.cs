using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Packing_Save : TransactionBase<WO_Packing_Save, WO_Packing_SaveResult>
{
	public List<WO_Packing> _ItemList = new List<WO_Packing>();

	private double _dbAccumulation = 0.0;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
