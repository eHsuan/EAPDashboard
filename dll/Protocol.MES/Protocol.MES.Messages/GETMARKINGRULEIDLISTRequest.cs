using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GETMARKINGRULEIDLISTRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentNo { get; set; }

	public string TargetType { get; set; }

	public string RequestQty { get; set; }

	public string UpdateSeqNo { get; set; }

	public string AutoGenerateCompID { get; set; }

	public string AutoCompConfirmIn { get; set; }

	public string AutoCompConfirmmrOut { get; set; }

	public GETMARKINGRULEIDLISTRequest()
	{
		base.TransactionName = MESTransactionName.GETMARKINGRULEIDLIST;
	}
}
