using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetCompConfirmStatusRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentNo { get; set; }

	public string FlowID { get; set; }

	public GetCompConfirmStatusRequest()
	{
		base.TransactionName = MESTransactionName.GetCompConfirmStatus;
	}
}
