using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TOS_GetToolListByWoRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public TOS_GetToolListByWoRequest()
	{
		base.TransactionName = MESTransactionName.TOS_GetToolListByWo;
	}
}
