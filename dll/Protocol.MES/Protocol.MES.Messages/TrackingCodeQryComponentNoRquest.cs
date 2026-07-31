using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TrackingCodeQryComponentNoRquest : MESBaseMessage
{
	public string WONO { get; set; }

	public string CustType { get; set; }

	public string TrackingCodeList { get; set; }

	[JsonIgnore]
	public List<TrackingCodeListItem> TrackingCodeListList => string.IsNullOrEmpty(TrackingCodeList) ? new List<TrackingCodeListItem>() : JsonConvert.DeserializeObject<List<TrackingCodeListItem>>(TrackingCodeList);

	public TrackingCodeQryComponentNoRquest()
	{
		base.TransactionName = MESTransactionName.TrackingCodeQryComponentNo;
	}
}
