using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelAddLogRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string Parent_ReelID { get; set; }

	public string ReelID { get; set; }

	public string SMatNo { get; set; }

	public string SBatchNo { get; set; }

	public string MatNo { get; set; }

	public string BatchNo { get; set; }

	public string Big_ReelID { get; set; }

	public string Status { get; set; }

	public string Action { get; set; }

	public ReelAddLogRequest()
	{
		base.TransactionName = MESTransactionName.ReelAddLog;
	}
}
