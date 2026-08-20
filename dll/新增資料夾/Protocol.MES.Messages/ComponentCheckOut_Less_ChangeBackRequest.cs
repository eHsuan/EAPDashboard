using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentCheckOut_Less_ChangeBackRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public ComponentCheckOut_Less_ChangeBackRequest()
	{
		base.TransactionName = MESTransactionName.ComponentCheckOut_Less_ChangeBack;
	}
}
