using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WorkOrderComponentBoundRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public List<ComponentRefItem> Components { get; set; }

	public WorkOrderComponentBoundRequest()
	{
		base.TransactionName = MESTransactionName.WorkOrderComponentBound;
	}
}
