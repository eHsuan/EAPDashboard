using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPPARTREFRESHRequest : MESBaseMessage
{
	public string UsageQty { get; set; }

	public string StatusCode { get; set; }

	public string PartTypeCode { get; set; }

	public string PartStartDate { get; set; }

	public string LimitQty { get; set; }

	public string UpdateStatus { get; set; }

	public EQPPARTREFRESHRequest()
	{
		base.TransactionName = MESTransactionName.EQPPARTREFRESH;
	}
}
