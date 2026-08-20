using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKIN_MULTIReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public WOCHECKIN_MULTIReply()
	{
		base.TransactionName = MESTransactionName.WOCHECKIN_MULTI;
	}
}
