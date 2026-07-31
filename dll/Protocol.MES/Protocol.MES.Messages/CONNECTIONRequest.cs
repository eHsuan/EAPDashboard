using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CONNECTIONRequest : MESBaseMessage
{
	public CONNECTIONRequest()
	{
		base.TransactionName = MESTransactionName.CONNECTION;
	}
}
