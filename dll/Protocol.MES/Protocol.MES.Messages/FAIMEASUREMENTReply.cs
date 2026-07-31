using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class FAIMEASUREMENTReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public FAIMEASUREMENTReply()
	{
		base.TransactionName = MESTransactionName.FAIMEASUREMENT;
	}
}
