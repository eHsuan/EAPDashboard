using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOParmQryNotByEqpNoReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ParamValue { get; set; }

	public string UpperValue { get; set; }

	public string LowerValue { get; set; }

	public string Unit { get; set; }

	public WOParmQryNotByEqpNoReply()
	{
		base.TransactionName = MESTransactionName.WOParmQryNotByEqpNo;
	}
}
