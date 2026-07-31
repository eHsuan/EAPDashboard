using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TraceMatQueryRequest : MESBaseMessage
{
	public string GlueNo { get; set; }

	public TraceMatQueryRequest()
	{
		base.TransactionName = MESTransactionName.TraceMatQuery;
	}
}
