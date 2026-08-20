using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOMachineRecordRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public GetWOMachineRecordRequest()
	{
		base.TransactionName = MESTransactionName.GetWOMachineRecord;
	}
}
