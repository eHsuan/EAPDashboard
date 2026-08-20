using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetCheckTaskReply : MESBaseMessage
{
	public string code { get; set; }

	public string msg { get; set; }

	public List<GetCheckTaskDataItem> data { get; set; }

	public GetCheckTaskReply()
	{
		base.TransactionName = MESTransactionName.GetCheckTask;
	}
}
