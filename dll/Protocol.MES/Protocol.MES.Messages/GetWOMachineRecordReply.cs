using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetWOMachineRecordReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MachineRecordDT { get; set; }

	[JsonIgnore]
	public List<MachineRecordItem> MachineRecordList => string.IsNullOrEmpty(MachineRecordDT) ? new List<MachineRecordItem>() : JsonConvert.DeserializeObject<List<MachineRecordItem>>(MachineRecordDT);

	public GetWOMachineRecordReply()
	{
		base.TransactionName = MESTransactionName.GetWOMachineRecord;
	}
}
