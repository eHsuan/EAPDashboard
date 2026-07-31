using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOReelIDListRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public GetWOReelIDListRequest()
	{
		base.TransactionName = MESTransactionName.GetWOReelIDList;
	}
}
