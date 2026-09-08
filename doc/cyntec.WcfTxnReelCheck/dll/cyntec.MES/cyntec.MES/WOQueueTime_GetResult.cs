using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WOQueueTime_GetResult : TransactionResultBase
{
	public List<WOQueueTime> _itemList = new List<WOQueueTime>();
}
