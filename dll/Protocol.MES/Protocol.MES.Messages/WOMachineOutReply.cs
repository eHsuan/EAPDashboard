using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOMachineOutReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public WOMachineOutReply()
	{
		base.TransactionName = MESTransactionName.WOMachineOut;
	}
}
