using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CARRIERINReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WONo { get; set; }

	public string WorkCenterNo { get; set; }

	public string FlowID { get; set; }

	public string MachNo { get; set; }

	public CARRIERINReply()
	{
		base.TransactionName = MESTransactionName.CARRIERIN;
	}
}
