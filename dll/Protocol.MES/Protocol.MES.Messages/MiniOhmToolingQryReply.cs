using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class MiniOhmToolingQryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string AmeterNo { get; set; }

	public string ToolingNo { get; set; }

	public string Qry1_ByMatNo_Complete { get; set; }

	public string Qry2_ByMatClassCode_Complete { get; set; }

	public MiniOhmToolingQryReply()
	{
		base.TransactionName = MESTransactionName.MiniOhmToolingQry;
	}
}
