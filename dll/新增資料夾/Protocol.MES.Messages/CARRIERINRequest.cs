using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CARRIERINRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string FlowID { get; set; }

	public string IsCycle { get; set; }

	public string ToolNo { get; set; }

	public CARRIERINRequest()
	{
		base.TransactionName = MESTransactionName.CARRIERIN;
	}
}
