using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpCheckReply : MESBaseMessage
{
	public string code { get; set; }

	public string msg { get; set; }

	public object data { get; set; }

	public EqpCheckReply()
	{
		base.TransactionName = MESTransactionName.EqpCheck;
	}
}
