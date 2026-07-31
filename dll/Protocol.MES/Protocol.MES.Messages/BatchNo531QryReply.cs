using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class BatchNo531QryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string Has531MatNo { get; set; }

	public string MainMatNo { get; set; }

	public string MainMatQty { get; set; }

	public string MainMatBatchNo { get; set; }

	public string SubMatNo { get; set; }

	public string SubMatQty { get; set; }

	public string SubMatBatchNo { get; set; }

	public string WOGroupID { get; set; }

	public string Batch531WOGroupDT { get; set; }

	[JsonIgnore]
	public List<Batch531WOGroupItem> Batch531WOGroupList => string.IsNullOrEmpty(Batch531WOGroupDT) ? new List<Batch531WOGroupItem>() : JsonConvert.DeserializeObject<List<Batch531WOGroupItem>>(Batch531WOGroupDT);

	public BatchNo531QryReply()
	{
		base.TransactionName = MESTransactionName.BatchNo531Qry;
	}
}
