using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelCheck_GetResult : TransactionResultBase
{
	public List<WO_ReelCheck> _ItemList = new List<WO_ReelCheck>();
}
