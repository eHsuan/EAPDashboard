using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GETWOCOMPONENTLISTReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WOComponentListDT { get; set; }

	public GETWOCOMPONENTLISTReply()
	{
		base.TransactionName = MESTransactionName.GETWOCOMPONENTLIST;
	}
}
