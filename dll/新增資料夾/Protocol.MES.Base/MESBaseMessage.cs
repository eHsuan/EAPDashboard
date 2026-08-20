using System;
using Protocol.MES.Enums;

namespace Protocol.MES.Base;

public abstract class MESBaseMessage
{
	public string DATE { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");

	public string EqpNo { get; set; }

	public string UserID { get; set; }

	public MESTransactionName TransactionName { get; set; }
}
