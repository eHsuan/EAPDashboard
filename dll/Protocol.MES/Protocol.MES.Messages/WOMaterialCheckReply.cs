using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOMaterialCheckReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MaterialMatchFlag { get; set; }

	public string MatchFlag { get; set; }

	public List<MaterialCheckItem> UnMatchMaterialDt { get; set; }

	public WOMaterialCheckReply()
	{
		base.TransactionName = MESTransactionName.WOMaterialCheck;
	}
}
