using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class USERQRYRequest : MESBaseMessage
{
	public USERQRYRequest()
	{
		base.TransactionName = MESTransactionName.USERQRY;
	}
}
