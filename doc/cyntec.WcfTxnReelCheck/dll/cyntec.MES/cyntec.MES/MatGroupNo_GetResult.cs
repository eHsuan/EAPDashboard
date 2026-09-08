using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MatGroupNo_GetResult : TransactionResultBase
{
	public List<MatGroupNoC> _ItemList = new List<MatGroupNoC>();
}
