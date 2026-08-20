using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WO_Component_AddRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentNo { get; set; }

	public string FlowID { get; set; }

	public string Qty { get; set; }

	public string InitQty { get; set; }

	public string InitNgQty { get; set; }

	public WO_Component_AddRequest()
	{
		base.TransactionName = MESTransactionName.WO_Component_Add;
	}
}
