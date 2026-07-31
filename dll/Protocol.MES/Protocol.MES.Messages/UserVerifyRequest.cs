using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UserVerifyRequest : MESBaseMessage
{
	public string User2DBarCode { get; set; }

	public string UserPassword { get; set; }

	public UserVerifyRequest()
	{
		base.TransactionName = MESTransactionName.UserVerify;
	}
}
