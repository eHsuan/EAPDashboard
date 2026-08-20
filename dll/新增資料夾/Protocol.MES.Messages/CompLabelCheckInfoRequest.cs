using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CompLabelCheckInfoRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public CompLabelCheckInfoRequest()
	{
		base.TransactionName = MESTransactionName.CompLabelCheckInfo;
	}
}
