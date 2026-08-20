using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelUpdateTPReelIDRequest : MESBaseMessage
{
	public string ReelID { get; set; }

	public string StatusCode { get; set; }

	public string StatusRemark { get; set; }

	public string StartTime { get; set; }

	public string EndTime { get; set; }

	public string LogKeyword { get; set; }

	public ReelUpdateTPReelIDRequest()
	{
		base.TransactionName = MESTransactionName.ReelUpdateTPReelID;
	}
}
