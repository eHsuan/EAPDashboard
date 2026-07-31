using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class BatchNo531QryRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string BatchNo { get; set; }

	public BatchNo531QryRequest()
	{
		base.TransactionName = MESTransactionName.BatchNo531Qry;
	}
}
