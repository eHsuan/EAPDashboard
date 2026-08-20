using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class SMTRawMatSetNGReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public SMTRawMatSetNGReply()
	{
		base.TransactionName = MESTransactionName.SMTRawMatSetNG;
	}
}
