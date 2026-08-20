using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelWOReelCheckSaveRequest : MESBaseMessage
{
	public string ReelCheckDt { get; set; }

	[JsonIgnore]
	public List<ReelCheckDtItem> ReelCheckDtList => string.IsNullOrEmpty(ReelCheckDt) ? new List<ReelCheckDtItem>() : JsonConvert.DeserializeObject<List<ReelCheckDtItem>>(ReelCheckDt);

	public ReelWOReelCheckSaveRequest()
	{
		base.TransactionName = MESTransactionName.ReelWOReelCheckSave;
	}
}
