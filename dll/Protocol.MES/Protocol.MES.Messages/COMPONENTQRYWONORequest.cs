using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTQRYWONORequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public COMPONENTQRYWONORequest()
	{
		base.TransactionName = MESTransactionName.COMPONENTQRYWONO;
	}
}
