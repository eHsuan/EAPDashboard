using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetCheckTaskRequest : MESBaseMessage
{
	public string JobType { get; set; }

	public GetCheckTaskRequest()
	{
		base.TransactionName = MESTransactionName.GetCheckTask;
	}
}
