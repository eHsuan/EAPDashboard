using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WorkCenter_GetResult : TransactionResultBase
{
	public List<WorkCenter> _ItemList = new List<WorkCenter>();
}
