using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UpdateReelStatusRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ReelID { get; set; }

	public string StatusCode { get; set; }

	public string WorkCenterTypeCode { get; set; }

	public string StartTime { get; set; }

	public string EndTime { get; set; }

	public string Remark { get; set; }

	public UpdateReelStatusRequest()
	{
		base.TransactionName = MESTransactionName.UpdateReelStatus;
	}
}
