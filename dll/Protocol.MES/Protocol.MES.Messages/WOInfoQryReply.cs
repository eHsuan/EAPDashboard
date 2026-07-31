using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOInfoQryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MatGroupNo { get; set; }

	public string LineCode { get; set; }

	public string MatNo { get; set; }

	public string WIPQty { get; set; }

	public string TatalQty { get; set; }

	public string BatchNo { get; set; }

	public string CreateDate { get; set; }

	public string CurrentWorkCenterNo { get; set; }

	public string CurrentFlowID { get; set; }

	public string IsUrgent { get; set; }

	public string CustomerDeliveryTime { get; set; }

	public WOInfoQryReply()
	{
		base.TransactionName = MESTransactionName.WOInfoQry;
	}
}
