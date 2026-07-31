using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTCONFIRMINRequest : MESBaseMessage
{
	public string Operator { get; set; }

	public string MachNo { get; set; }

	public string FlowID { get; set; }

	public string ComponentNo { get; set; }

	public string WONO { get; set; }

	public COMPONENTCONFIRMINRequest()
	{
		base.TransactionName = MESTransactionName.COMPONENTCONFIRMIN;
	}
}
