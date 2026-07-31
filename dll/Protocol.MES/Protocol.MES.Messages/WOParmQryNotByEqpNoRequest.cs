using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOParmQryNotByEqpNoRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string RefFieldCode { get; set; }

	public string Key { get; set; }

	public string Password { get; set; }

	public WOParmQryNotByEqpNoRequest()
	{
		base.TransactionName = MESTransactionName.WOParmQryNotByEqpNo;
	}
}
