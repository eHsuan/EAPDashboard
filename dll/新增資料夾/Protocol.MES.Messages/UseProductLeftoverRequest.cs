using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UseProductLeftoverRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ReelID { get; set; }

	public string LeftOverWONo { get; set; }

	public string LeftOverBatchNo { get; set; }

	public string AddQty { get; set; }

	public string GapQty { get; set; }

	public UseProductLeftoverRequest()
	{
		base.TransactionName = MESTransactionName.UseProductLeftover;
	}
}
