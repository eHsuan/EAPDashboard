using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CONNECTIONReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public CONNECTIONReply()
	{
		base.TransactionName = MESTransactionName.CONNECTION;
	}
}
