using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class SMTWOBOMQRYRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public SMTWOBOMQRYRequest()
	{
		base.TransactionName = MESTransactionName.SMTWOBOMQRY;
	}
}
