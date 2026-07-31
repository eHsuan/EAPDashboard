using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class StdPartsQueryRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string StdPartsID { get; set; }

	public string Action { get; set; }

	public StdPartsQueryRequest()
	{
		base.TransactionName = MESTransactionName.StdPartsQuery;
	}
}
