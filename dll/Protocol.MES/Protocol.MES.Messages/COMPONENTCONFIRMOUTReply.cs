using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTCONFIRMOUTReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public COMPONENTCONFIRMOUTReply()
	{
		base.TransactionName = MESTransactionName.COMPONENTCONFIRMOUT;
	}
}
