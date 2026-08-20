using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentCheckOut_LessRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public ComponentCheckOut_LessRequest()
	{
		base.TransactionName = MESTransactionName.ComponentCheckOut_Less;
	}
}
