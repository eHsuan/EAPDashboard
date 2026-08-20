using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOBOMQRYReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string BomListDT { get; set; }

	public WOBOMQRYReply()
	{
		base.TransactionName = MESTransactionName.WOBOMQRY;
	}
}
