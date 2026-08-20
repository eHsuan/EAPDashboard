using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentNGRequest : MESBaseMessage
{
	public string ComponentNo { get; set; }

	public string dtNGList { get; set; }

	[JsonIgnore]
	public List<dtNGListItem> dtNGListList => string.IsNullOrEmpty(dtNGList) ? new List<dtNGListItem>() : JsonConvert.DeserializeObject<List<dtNGListItem>>(dtNGList);

	public ComponentNGRequest()
	{
		base.TransactionName = MESTransactionName.ComponentNG;
	}
}
