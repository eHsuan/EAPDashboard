using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class Component_MakeUp_ChangeBackRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ComponentNo { get; set; }

	public Component_MakeUp_ChangeBackRequest()
	{
		base.TransactionName = MESTransactionName.Component_MakeUp_ChangeBack;
	}
}
