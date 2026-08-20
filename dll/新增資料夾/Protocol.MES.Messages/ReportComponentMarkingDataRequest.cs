using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReportComponentMarkingDataRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ActionType { get; set; }

	public string IsLast { get; set; }

	public List<ComponentMarkingItem> ComponentMarkingDT { get; set; }

	public ReportComponentMarkingDataRequest()
	{
		base.TransactionName = MESTransactionName.ReportComponentMarkingData;
	}
}
