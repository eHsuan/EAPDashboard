using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CompLabelCheckRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string LabelID { get; set; }

	public string ComponentNo { get; set; }

	public string IsEqpAutoMode { get; set; }

	public CompLabelCheckRequest()
	{
		base.TransactionName = MESTransactionName.CompLabelCheck;
	}
}
