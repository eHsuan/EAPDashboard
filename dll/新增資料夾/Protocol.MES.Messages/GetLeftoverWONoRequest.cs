using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetLeftoverWONoRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ReelID { get; set; }

	public GetLeftoverWONoRequest()
	{
		base.TransactionName = MESTransactionName.GetLeftoverWONo;
	}
}
