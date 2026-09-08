using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Operation_GetResult : TransactionResultBase
{
	public List<EQP_OP_Operation> _ItemList = new List<EQP_OP_Operation>();
}
