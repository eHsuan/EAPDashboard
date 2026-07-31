using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOBOMQRYRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public WOBOMQRYRequest()
	{
		base.TransactionName = MESTransactionName.WOBOMQRY;
	}
}
