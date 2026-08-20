using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TrackingCodeQryLabelInfoRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string TrackingCodeList { get; set; }

	[JsonIgnore]
	public List<TrackingCodeListItem> TrackingCodeListList => string.IsNullOrEmpty(TrackingCodeList) ? new List<TrackingCodeListItem>() : JsonConvert.DeserializeObject<List<TrackingCodeListItem>>(TrackingCodeList);

	public TrackingCodeQryLabelInfoRequest()
	{
		base.TransactionName = MESTransactionName.TrackingCodeQryLabelInfo;
	}
}
