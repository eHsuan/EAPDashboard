using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWORoutingReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WORoutingDT { get; set; }

	[JsonIgnore]
	public List<WORoutingItem> WORoutingList => string.IsNullOrEmpty(WORoutingDT) ? new List<WORoutingItem>() : JsonConvert.DeserializeObject<List<WORoutingItem>>(WORoutingDT);

	public GetWORoutingReply()
	{
		base.TransactionName = MESTransactionName.GetWORouting;
	}
}
