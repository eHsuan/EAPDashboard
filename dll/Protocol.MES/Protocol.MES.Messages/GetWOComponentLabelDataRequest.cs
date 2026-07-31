using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOComponentLabelDataRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string ComponentNo { get; set; }

	public string IsActive { get; set; }

	public string LabelID { get; set; }

	public GetWOComponentLabelDataRequest()
	{
		base.TransactionName = MESTransactionName.GetWOComponentLabelData;
	}
}
