using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentGetChildsReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentChildDt { get; set; }

	[JsonIgnore]
	public List<ComponentChildItem> ComponentChildList => string.IsNullOrEmpty(ComponentChildDt) ? new List<ComponentChildItem>() : JsonConvert.DeserializeObject<List<ComponentChildItem>>(ComponentChildDt);

	public ComponentGetChildsReply()
	{
		base.TransactionName = MESTransactionName.ComponentGetChilds;
	}
}
