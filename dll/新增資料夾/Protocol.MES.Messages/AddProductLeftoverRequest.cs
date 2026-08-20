using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class AddProductLeftoverRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string Qty { get; set; }

	public string ReelID { get; set; }

	public string Loc { get; set; }

	public AddProductLeftoverRequest()
	{
		base.TransactionName = MESTransactionName.AddProductLeftover;
	}
}
