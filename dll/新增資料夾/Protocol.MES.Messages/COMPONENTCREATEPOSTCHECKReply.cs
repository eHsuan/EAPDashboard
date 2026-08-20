using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTCREATEPOSTCHECKReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public COMPONENTCREATEPOSTCHECKReply()
	{
		base.TransactionName = MESTransactionName.COMPONENTCREATEPOSTCHECK;
	}
}
