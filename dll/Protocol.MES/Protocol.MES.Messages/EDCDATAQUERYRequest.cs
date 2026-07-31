using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EDCDATAQUERYRequest : MESBaseMessage
{
	public string WoNo { get; set; }

	public string IsAvailable { get; set; }

	public string FlowID { get; set; }

	public string WorkcenterNo { get; set; }

	public string EdcRawdataSummarySysID { get; set; }

	public string EdcPlanSysID { get; set; }

	public string ComponentID { get; set; }

	public string[] RefFieldCodeList { get; set; }

	public EDCDATAQUERYRequest()
	{
		base.TransactionName = MESTransactionName.EDCDATAQUERY;
	}
}
