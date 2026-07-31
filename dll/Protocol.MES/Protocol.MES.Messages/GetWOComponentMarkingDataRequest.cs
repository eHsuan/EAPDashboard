using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOComponentMarkingDataRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public GetWOComponentMarkingDataRequest()
	{
		base.TransactionName = MESTransactionName.GetWOComponentMarkingData;
	}
}
