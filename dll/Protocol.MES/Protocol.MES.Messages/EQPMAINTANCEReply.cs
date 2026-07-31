using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMAINTANCEReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public EQPMAINTANCEReply()
	{
		base.TransactionName = MESTransactionName.EQPMAINTANCE;
	}
}
