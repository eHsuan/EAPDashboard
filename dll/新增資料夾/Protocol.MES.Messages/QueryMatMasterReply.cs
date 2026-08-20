using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class QueryMatMasterReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ClassCode { get; set; }

	public string Description { get; set; }

	public QueryMatMasterReply()
	{
		base.TransactionName = MESTransactionName.QueryMatMaster;
	}
}
