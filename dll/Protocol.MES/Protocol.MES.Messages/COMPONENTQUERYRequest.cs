using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTQUERYRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public COMPONENTQUERYRequest()
	{
		base.TransactionName = MESTransactionName.COMPONENTQUERY;
	}
}
