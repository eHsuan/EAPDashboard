using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOComponentQrySimpleRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public WOComponentQrySimpleRequest()
	{
		base.TransactionName = MESTransactionName.WOComponentQrySimple;
	}
}
