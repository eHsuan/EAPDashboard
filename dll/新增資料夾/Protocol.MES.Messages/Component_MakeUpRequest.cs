using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class Component_MakeUpRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ComponentNo { get; set; }

	public Component_MakeUpRequest()
	{
		base.TransactionName = MESTransactionName.Component_MakeUp;
	}
}
