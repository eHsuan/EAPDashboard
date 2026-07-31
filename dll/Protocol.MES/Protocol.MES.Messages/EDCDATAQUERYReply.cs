using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EDCDATAQUERYReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WoNo { get; set; }

	public string RawDataList { get; set; }

	public EDCDATAQUERYReply()
	{
		base.TransactionName = MESTransactionName.EDCDATAQUERY;
	}
}
