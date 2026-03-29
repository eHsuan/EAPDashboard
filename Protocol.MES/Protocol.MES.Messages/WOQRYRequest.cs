using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOQRYRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public WOQRYRequest()
	{
		base.TransactionName = MESTransactionName.WOQRY;
	}
}
