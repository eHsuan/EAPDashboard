using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetLeaveWOListRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public GetLeaveWOListRequest()
	{
		base.TransactionName = MESTransactionName.GetLeaveWOList;
	}
}
