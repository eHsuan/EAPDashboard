using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOReelIDListReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WOReelIDListDT { get; set; }

	[JsonIgnore]
	public List<WOReelIDItem> WOReelIDList => string.IsNullOrEmpty(WOReelIDListDT) ? new List<WOReelIDItem>() : JsonConvert.DeserializeObject<List<WOReelIDItem>>(WOReelIDListDT);

	public GetWOReelIDListReply()
	{
		base.TransactionName = MESTransactionName.GetWOReelIDList;
	}
}
