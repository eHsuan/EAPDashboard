using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentLabelCheckCompleteRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public ComponentLabelCheckCompleteRequest()
	{
		base.TransactionName = MESTransactionName.ComponentLabelCheckComplete;
	}
}
