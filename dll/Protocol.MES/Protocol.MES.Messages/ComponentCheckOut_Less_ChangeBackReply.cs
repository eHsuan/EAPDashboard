using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentCheckOut_Less_ChangeBackReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WONo { get; set; }

	public ComponentCheckOut_Less_ChangeBackReply()
	{
		base.TransactionName = MESTransactionName.ComponentCheckOut_Less_ChangeBack;
	}
}
