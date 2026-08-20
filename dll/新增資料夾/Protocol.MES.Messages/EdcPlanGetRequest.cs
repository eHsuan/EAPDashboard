using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EdcPlanGetRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string FlowID { get; set; }

	public string MatNo { get; set; }

	public EdcPlanGetRequest()
	{
		base.TransactionName = MESTransactionName.EdcPlanGet;
	}
}
