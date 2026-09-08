using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Group_GetResult : TransactionResultBase
{
	public List<WorkOrder> _itemList = new List<WorkOrder>();
}
