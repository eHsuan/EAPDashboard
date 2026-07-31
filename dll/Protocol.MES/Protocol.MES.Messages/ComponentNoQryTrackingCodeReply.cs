using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentNoQryTrackingCodeReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string TrackingCodeList { get; set; }

	public ComponentNoQryTrackingCodeReply()
	{
		base.TransactionName = MESTransactionName.ComponentNoQryTrackingCode;
	}
}
