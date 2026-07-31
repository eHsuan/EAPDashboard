using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpWOInputAddRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public string FlowID { get; set; }

	public string InputListDt { get; set; }

	[JsonIgnore]
	public List<InputItem> InputList => string.IsNullOrEmpty(InputListDt) ? new List<InputItem>() : JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);

	public EqpWOInputAddRequest()
	{
		base.TransactionName = MESTransactionName.EqpWOInputAdd;
	}
}
