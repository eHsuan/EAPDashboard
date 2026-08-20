using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class CompLabelBatchCheckRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string LableCheckDT { get; set; }

	[JsonIgnore]
	public List<LableCheckItem> LableCheckList => string.IsNullOrEmpty(LableCheckDT) ? new List<LableCheckItem>() : JsonConvert.DeserializeObject<List<LableCheckItem>>(LableCheckDT);

	public CompLabelBatchCheckRequest()
	{
		base.TransactionName = MESTransactionName.CompLabelBatchCheck;
	}
}
