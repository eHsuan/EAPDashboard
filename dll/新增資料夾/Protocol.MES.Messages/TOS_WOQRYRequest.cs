using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TOS_WOQRYRequest : MESBaseMessage
{
	public string ToolNo { get; set; }

	public TOS_WOQRYRequest()
	{
		base.TransactionName = MESTransactionName.TOS_WOQRY;
	}
}
