using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MESSystemParameter_GetResult : TransactionResultBase
{
	public List<MESSystemParameter> _ItemList = new List<MESSystemParameter>();
}
