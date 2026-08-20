using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOMachineListReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MachineRecordDT { get; set; }

	[JsonIgnore]
	public List<WOMachineListItem> MachineRecordList => string.IsNullOrEmpty(MachineRecordDT) ? new List<WOMachineListItem>() : JsonConvert.DeserializeObject<List<WOMachineListItem>>(MachineRecordDT);

	public GetWOMachineListReply()
	{
		base.TransactionName = MESTransactionName.GetWOMachineList;
	}
}
