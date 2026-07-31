using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class USERQRYReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string UserName { get; set; }

	public USERQRYReply()
	{
		base.TransactionName = MESTransactionName.USERQRY;
	}
}
