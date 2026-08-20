using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelGetReelByIDRequest : MESBaseMessage
{
	public string ReelID { get; set; }

	public ReelGetReelByIDRequest()
	{
		base.TransactionName = MESTransactionName.ReelGetReelByID;
	}
}
