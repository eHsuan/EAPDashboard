using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMOULDQRYRequest : MESBaseMessage
{
	public string MouldID { get; set; }

	public EQPMOULDQRYRequest()
	{
		base.TransactionName = MESTransactionName.EQPMOULDQRY;
	}
}
