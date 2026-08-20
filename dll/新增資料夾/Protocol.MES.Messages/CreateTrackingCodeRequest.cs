using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CreateTrackingCodeRequest : MESBaseMessage
{
	public string WoNo { get; set; }

	public string ComponentList { get; set; }

	[JsonIgnore]
	public List<ComponentListItem> ComponentListList => string.IsNullOrEmpty(ComponentList) ? new List<ComponentListItem>() : JsonConvert.DeserializeObject<List<ComponentListItem>>(ComponentList);

	public CreateTrackingCodeRequest()
	{
		base.TransactionName = MESTransactionName.CreateTrackingCode;
	}
}
