using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelUpdateTPReelIDReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public ReelUpdateTPReelIDReply()
	{
		base.TransactionName = MESTransactionName.ReelUpdateTPReelID;
	}
}
