using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTCREATEPOSTCHECKRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string AllowNonBookingData { get; set; }

	public string ComponentListDT { get; set; }

	[JsonIgnore]
	public List<ComponentListItem> ComponentNoListList => string.IsNullOrEmpty(ComponentListDT) ? new List<ComponentListItem>() : JsonConvert.DeserializeObject<List<ComponentListItem>>(ComponentListDT);

	public COMPONENTCREATEPOSTCHECKRequest()
	{
		base.TransactionName = MESTransactionName.COMPONENTCREATEPOSTCHECK;
	}
}
