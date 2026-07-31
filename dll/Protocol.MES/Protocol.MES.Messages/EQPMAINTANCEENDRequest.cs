using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMAINTANCEENDRequest : MESBaseMessage
{
	public EQPMAINTANCEENDRequest()
	{
		base.TransactionName = MESTransactionName.EQPMAINTANCEEND;
	}
}
