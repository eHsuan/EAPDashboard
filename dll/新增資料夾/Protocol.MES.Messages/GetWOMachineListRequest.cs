using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOMachineListRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public GetWOMachineListRequest()
	{
		base.TransactionName = MESTransactionName.GetWOMachineList;
	}
}
