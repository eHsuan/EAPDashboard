using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_TapingUILog_SaveResult : TransactionResultBase
{
	public string _TxnID;
}
