using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentCheckOut_NGReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public ComponentCheckOut_NGReply()
	{
		base.TransactionName = MESTransactionName.ComponentCheckOut_NG;
	}
}
