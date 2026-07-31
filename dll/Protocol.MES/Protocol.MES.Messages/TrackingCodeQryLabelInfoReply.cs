using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TrackingCodeQryLabelInfoReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentLabelList { get; set; }

	public TrackingCodeQryLabelInfoReply()
	{
		base.TransactionName = MESTransactionName.TrackingCodeQryLabelInfo;
	}
}
