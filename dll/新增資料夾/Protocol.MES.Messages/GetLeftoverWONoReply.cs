using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetLeftoverWONoReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string Qty { get; set; }

	public string WONO { get; set; }

	public string BachNo { get; set; }

	public string Loc { get; set; }

	public GetLeftoverWONoReply()
	{
		base.TransactionName = MESTransactionName.GetLeftoverWONo;
	}
}
