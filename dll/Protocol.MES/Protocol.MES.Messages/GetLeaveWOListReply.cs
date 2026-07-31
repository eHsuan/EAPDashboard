using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetLeaveWOListReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string WOListDT { get; set; }

	[JsonIgnore]
	public List<LeaveWOItem> WOList => string.IsNullOrEmpty(WOListDT) ? new List<LeaveWOItem>() : JsonConvert.DeserializeObject<List<LeaveWOItem>>(WOListDT);

	public GetLeaveWOListReply()
	{
		base.TransactionName = MESTransactionName.GetLeaveWOList;
	}
}
