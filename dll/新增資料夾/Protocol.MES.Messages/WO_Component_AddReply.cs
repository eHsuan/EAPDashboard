using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WO_Component_AddReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public WO_Component_AddReply()
	{
		base.TransactionName = MESTransactionName.WO_Component_Add;
	}
}
