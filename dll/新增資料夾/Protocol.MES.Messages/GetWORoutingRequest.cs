using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWORoutingRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public GetWORoutingRequest()
	{
		base.TransactionName = MESTransactionName.GetWORouting;
	}
}
