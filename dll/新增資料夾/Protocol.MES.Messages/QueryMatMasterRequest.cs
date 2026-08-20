using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class QueryMatMasterRequest : MESBaseMessage
{
	public string MatNo { get; set; }

	public QueryMatMasterRequest()
	{
		base.TransactionName = MESTransactionName.QueryMatMaster;
	}
}
