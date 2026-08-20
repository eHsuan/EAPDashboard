using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class SMTComponentConfirmRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentNo { get; set; }

	public string FlowID { get; set; }

	public string ComponentNGDt { get; set; }

	[JsonIgnore]
	public List<SMTComponentNGItem> ComponentNGList => string.IsNullOrEmpty(ComponentNGDt) ? new List<SMTComponentNGItem>() : JsonConvert.DeserializeObject<List<SMTComponentNGItem>>(ComponentNGDt);

	public SMTComponentConfirmRequest()
	{
		base.TransactionName = MESTransactionName.SMTComponentConfirm;
	}
}
