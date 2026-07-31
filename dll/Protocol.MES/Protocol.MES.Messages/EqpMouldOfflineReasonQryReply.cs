using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpMouldOfflineReasonQryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string TOSOfflineReasonDt { get; set; }

	[JsonIgnore]
	public List<TOSOfflineReasonItem> TOSOfflineReasonList => string.IsNullOrEmpty(TOSOfflineReasonDt) ? new List<TOSOfflineReasonItem>() : JsonConvert.DeserializeObject<List<TOSOfflineReasonItem>>(TOSOfflineReasonDt);

	public EqpMouldOfflineReasonQryReply()
	{
		base.TransactionName = MESTransactionName.EqpMouldOfflineReasonQry;
	}
}
