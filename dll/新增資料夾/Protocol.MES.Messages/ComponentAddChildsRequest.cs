using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentAddChildsRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentChildDt { get; set; }

	[JsonIgnore]
	public List<ComponentChildItem> ComponentChildList => string.IsNullOrEmpty(ComponentChildDt) ? new List<ComponentChildItem>() : JsonConvert.DeserializeObject<List<ComponentChildItem>>(ComponentChildDt);

	public ComponentAddChildsRequest()
	{
		base.TransactionName = MESTransactionName.ComponentAddChilds;
	}
}
