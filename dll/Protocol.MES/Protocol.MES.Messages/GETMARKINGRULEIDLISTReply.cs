using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GETMARKINGRULEIDLISTReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string TargetType { get; set; }

	public string RuleCode { get; set; }

	public string RuleName { get; set; }

	public string RuleDesc { get; set; }

	public string RuleSide { get; set; }

	public string RuleSideUnit { get; set; }

	public string RuleSizeX { get; set; }

	public string RuleSizeY { get; set; }

	public string TwoDimKind { get; set; }

	public string SubIDListDT { get; set; }

	public GETMARKINGRULEIDLISTReply()
	{
		base.TransactionName = MESTransactionName.GETMARKINGRULEIDLIST;
	}
}
