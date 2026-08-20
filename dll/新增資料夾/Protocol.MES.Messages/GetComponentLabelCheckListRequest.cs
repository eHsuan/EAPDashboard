using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetComponentLabelCheckListRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public GetComponentLabelCheckListRequest()
	{
		base.TransactionName = MESTransactionName.GetComponentLabelCheckList;
	}
}
