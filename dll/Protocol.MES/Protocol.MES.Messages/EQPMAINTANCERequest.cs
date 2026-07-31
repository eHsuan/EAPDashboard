using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMAINTANCERequest : MESBaseMessage
{
	public EQPMAINTANCERequest()
	{
		base.TransactionName = MESTransactionName.EQPMAINTANCE;
	}
}
