using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TrackingCodeCheckRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string CustType { get; set; }

	public string ComponentList { get; set; }

	[JsonIgnore]
	public List<ComponentCheckListItem> ComponentCheckListList => string.IsNullOrEmpty(ComponentList) ? new List<ComponentCheckListItem>() : JsonConvert.DeserializeObject<List<ComponentCheckListItem>>(ComponentList);

	public TrackingCodeCheckRequest()
	{
		base.TransactionName = MESTransactionName.TrackingCodeCheck;
	}
}
