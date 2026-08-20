using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTRELATIONRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentRelationDt { get; set; }

	[JsonIgnore]
	public List<ComponentRelationDtListItem> ComponentRelationDtList => string.IsNullOrEmpty(ComponentRelationDt) ? new List<ComponentRelationDtListItem>() : JsonConvert.DeserializeObject<List<ComponentRelationDtListItem>>(ComponentRelationDt);

	public COMPONENTRELATIONRequest()
	{
		base.TransactionName = MESTransactionName.COMPONENTRELATION;
	}
}
