using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpCheckRequest : MESBaseMessage
{
	public int ScheduleID { get; set; }

	public List<ParmItem> ParamList { get; set; }

	public EqpCheckRequest()
	{
		base.TransactionName = MESTransactionName.EqpCheck;
	}
}
