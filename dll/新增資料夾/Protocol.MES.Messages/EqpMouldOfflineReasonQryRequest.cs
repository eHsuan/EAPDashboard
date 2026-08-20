using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpMouldOfflineReasonQryRequest : MESBaseMessage
{
	public string MouldID { get; set; }

	public EqpMouldOfflineReasonQryRequest()
	{
		base.TransactionName = MESTransactionName.EqpMouldOfflineReasonQry;
	}
}
