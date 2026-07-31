using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TrackingCodeCheckReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentList { get; set; }

	public TrackingCodeCheckReply()
	{
		base.TransactionName = MESTransactionName.TrackingCodeCheck;
	}
}
