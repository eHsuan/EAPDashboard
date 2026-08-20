using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class MiniOhmToolingQryRequest : MESBaseMessage
{
	public string MatgroupNo { get; set; }

	public string MatNo { get; set; }

	public MiniOhmToolingQryRequest()
	{
		base.TransactionName = MESTransactionName.MiniOhmToolingQry;
	}
}
