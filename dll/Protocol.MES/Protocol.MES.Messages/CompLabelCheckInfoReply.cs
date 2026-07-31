using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CompLabelCheckInfoReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentLabelCheckInfo { get; set; }

	public CompLabelCheckInfoReply()
	{
		base.TransactionName = MESTransactionName.CompLabelCheckInfo;
	}
}
