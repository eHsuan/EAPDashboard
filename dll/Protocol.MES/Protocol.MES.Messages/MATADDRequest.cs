using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class MATADDRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string TowDListDt { get; set; }

	[JsonIgnore]
	public List<TowDListItem> TowDList => string.IsNullOrEmpty(TowDListDt) ? new List<TowDListItem>() : JsonConvert.DeserializeObject<List<TowDListItem>>(TowDListDt);

	public MATADDRequest()
	{
		base.TransactionName = MESTransactionName.MATADD;
	}
}
