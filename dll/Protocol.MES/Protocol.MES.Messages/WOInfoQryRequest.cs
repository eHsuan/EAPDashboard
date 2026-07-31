using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOInfoQryRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public WOInfoQryRequest()
	{
		base.TransactionName = MESTransactionName.WOInfoQry;
	}
}
