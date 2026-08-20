using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMOULDONLINERequest : MESBaseMessage
{
	public string MouldID { get; set; }

	public EQPMOULDONLINERequest()
	{
		base.TransactionName = MESTransactionName.EQPMOULDONLINE;
	}
}
