using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentNoQryTrackingCodeRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string CustType { get; set; }

	public string ComponentNoList { get; set; }

	[JsonIgnore]
	public List<ComponentListItem> ComponentNoListList => string.IsNullOrEmpty(ComponentNoList) ? new List<ComponentListItem>() : JsonConvert.DeserializeObject<List<ComponentListItem>>(ComponentNoList);

	public ComponentNoQryTrackingCodeRequest()
	{
		base.TransactionName = MESTransactionName.ComponentNoQryTrackingCode;
	}
}
