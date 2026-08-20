using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOPARMQRYRequest : MESBaseMessage
{
	public string RefFieldCode { get; set; }

	public string WONO { get; set; }

	public WOPARMQRYRequest()
	{
		base.TransactionName = MESTransactionName.WOPARMQRY;
	}
}
