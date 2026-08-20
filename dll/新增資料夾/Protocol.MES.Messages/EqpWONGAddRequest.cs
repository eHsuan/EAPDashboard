using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpWONGAddRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string FlowID { get; set; }

	public string NGListDt { get; set; }

	[JsonIgnore]
	public List<NGListItem> NGList => string.IsNullOrEmpty(NGListDt) ? new List<NGListItem>() : JsonConvert.DeserializeObject<List<NGListItem>>(NGListDt);

	public EqpWONGAddRequest()
	{
		base.TransactionName = MESTransactionName.EqpWONGAdd;
	}
}
