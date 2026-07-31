using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class StdCheckItemsQueryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public List<StdCheckItem> CheckItems { get; set; }

	public StdCheckItemsQueryReply()
	{
		base.TransactionName = MESTransactionName.StdCheckItemsQuery;
	}
}
