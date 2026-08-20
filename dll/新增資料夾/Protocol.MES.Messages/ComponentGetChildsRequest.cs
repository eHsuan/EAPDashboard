using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentGetChildsRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public ComponentGetChildsRequest()
	{
		base.TransactionName = MESTransactionName.ComponentGetChilds;
	}
}
