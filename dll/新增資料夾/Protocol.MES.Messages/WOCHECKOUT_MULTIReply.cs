using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKOUT_MULTIReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public WOCHECKOUT_MULTIReply()
	{
		base.TransactionName = MESTransactionName.WOCHECKOUT_MULTI;
	}
}
