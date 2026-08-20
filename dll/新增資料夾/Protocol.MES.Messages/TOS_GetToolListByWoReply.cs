using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TOS_GetToolListByWoReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ToolListDT { get; set; }

	[JsonIgnore]
	public List<ToolListItem> ToolList => string.IsNullOrEmpty(ToolListDT) ? new List<ToolListItem>() : JsonConvert.DeserializeObject<List<ToolListItem>>(ToolListDT);

	public TOS_GetToolListByWoReply()
	{
		base.TransactionName = MESTransactionName.TOS_GetToolListByWo;
	}
}
