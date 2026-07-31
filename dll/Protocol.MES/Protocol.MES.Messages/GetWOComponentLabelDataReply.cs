using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOComponentLabelDataReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ComponentLabelDT { get; set; }

	[JsonIgnore]
	public List<ComponentLabelItem> ComponentLabelList => string.IsNullOrEmpty(ComponentLabelDT) ? new List<ComponentLabelItem>() : JsonConvert.DeserializeObject<List<ComponentLabelItem>>(ComponentLabelDT);

	public GetWOComponentLabelDataReply()
	{
		base.TransactionName = MESTransactionName.GetWOComponentLabelData;
	}
}
