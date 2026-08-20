using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class StdCheckItemsQueryRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public StdCheckItemsQueryRequest()
	{
		base.TransactionName = MESTransactionName.StdCheckItemsQuery;
	}
}
