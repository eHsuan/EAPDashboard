using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTQRYWONOReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WONo { get; set; }

	public COMPONENTQRYWONOReply()
	{
		base.TransactionName = MESTransactionName.COMPONENTQRYWONO;
	}
}
