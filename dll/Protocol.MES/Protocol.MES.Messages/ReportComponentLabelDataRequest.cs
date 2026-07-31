using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReportComponentLabelDataRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ActionType { get; set; }

	public string Remark { get; set; }

	public string LPRemark { get; set; }

	public List<LabelContentItem> LabelContentDT { get; set; }

	public ReportComponentLabelDataRequest()
	{
		base.TransactionName = MESTransactionName.ReportComponentLabelData;
	}
}
