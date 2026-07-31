using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GETWOCOMPONENTLISTRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public GETWOCOMPONENTLISTRequest()
	{
		base.TransactionName = MESTransactionName.GETWOCOMPONENTLIST;
	}
}
