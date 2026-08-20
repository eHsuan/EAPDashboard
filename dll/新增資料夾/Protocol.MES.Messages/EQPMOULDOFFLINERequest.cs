using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMOULDOFFLINERequest : MESBaseMessage
{
	public string MouldID { get; set; }

	public string UsageQty { get; set; }

	public string Reason { get; set; }

	public EQPMOULDOFFLINERequest()
	{
		base.TransactionName = MESTransactionName.EQPMOULDOFFLINE;
	}
}
