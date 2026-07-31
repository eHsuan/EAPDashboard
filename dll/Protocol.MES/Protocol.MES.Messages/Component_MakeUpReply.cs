using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class Component_MakeUpReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WONo { get; set; }

	public Component_MakeUpReply()
	{
		base.TransactionName = MESTransactionName.Component_MakeUp;
	}
}
