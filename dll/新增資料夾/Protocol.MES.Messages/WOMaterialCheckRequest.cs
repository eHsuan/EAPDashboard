using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOMaterialCheckRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public List<MaterialCheckItem> Materials { get; set; }

	public WOMaterialCheckRequest()
	{
		base.TransactionName = MESTransactionName.WOMaterialCheck;
	}
}
