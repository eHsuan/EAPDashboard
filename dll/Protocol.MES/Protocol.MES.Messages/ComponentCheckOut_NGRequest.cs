using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentCheckOut_NGRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ComponentNo { get; set; }

	public string NGCode { get; set; }

	public string NGQty { get; set; }

	public ComponentCheckOut_NGRequest()
	{
		base.TransactionName = MESTransactionName.ComponentCheckOut_NG;
	}
}
