using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetComponentLabelCheckListReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string CheckedComponentList { get; set; }

	public string UnCheckComponentList { get; set; }

	public GetComponentLabelCheckListReply()
	{
		base.TransactionName = MESTransactionName.GetComponentLabelCheckList;
	}
}
