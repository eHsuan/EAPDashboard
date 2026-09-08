using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Block_SaveResult : TransactionResultBase
{
	public List<WO_Block> _itemList = new List<WO_Block>();
}
