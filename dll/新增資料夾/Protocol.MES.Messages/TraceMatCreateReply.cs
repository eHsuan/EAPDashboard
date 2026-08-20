using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TraceMatCreateReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string GlueNo { get; set; }

	public string TraceMatDetailDt { get; set; }

	[JsonIgnore]
	public List<TraceMatDetailItem> TraceMatDetailList => string.IsNullOrEmpty(TraceMatDetailDt) ? new List<TraceMatDetailItem>() : JsonConvert.DeserializeObject<List<TraceMatDetailItem>>(TraceMatDetailDt);

	public TraceMatCreateReply()
	{
		base.TransactionName = MESTransactionName.TraceMatCreate;
	}
}
