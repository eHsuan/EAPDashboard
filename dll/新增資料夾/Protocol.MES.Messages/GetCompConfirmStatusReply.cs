using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetCompConfirmStatusReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ConfirmStatusCode { get; set; }

	public GetCompConfirmStatusReply()
	{
		base.TransactionName = MESTransactionName.GetCompConfirmStatus;
	}
}
