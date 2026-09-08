using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Block_GetResult : TransactionResultBase
{
	public List<mesWO_Block> _itemList = new List<mesWO_Block>();
}
