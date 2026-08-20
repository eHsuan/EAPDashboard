using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOComponentMarkingDataReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentMarkingDT { get; set; }

	[JsonIgnore]
	public List<ComponentMarkingRecordItem> ComponentMarkingList => string.IsNullOrEmpty(ComponentMarkingDT) ? new List<ComponentMarkingRecordItem>() : JsonConvert.DeserializeObject<List<ComponentMarkingRecordItem>>(ComponentMarkingDT);

	public GetWOComponentMarkingDataReply()
	{
		base.TransactionName = MESTransactionName.GetWOComponentMarkingData;
	}
}
