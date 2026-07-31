using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKOUT_MULTIRequest : MESBaseMessage
{
	public string WoNoDt { get; set; }

	public string IsGroupedEqp { get; set; }

	public string Operator { get; set; }

	public string OutPut { get; set; }

	public string File { get; set; }

	public string InputListDt { get; set; }

	public string ParmListDt { get; set; }

	public string NGListDt { get; set; }

	[JsonIgnore]
	public List<WoNoListItem> WoNoList => string.IsNullOrEmpty(WoNoDt) ? new List<WoNoListItem>() : JsonConvert.DeserializeObject<List<WoNoListItem>>(WoNoDt);

	[JsonIgnore]
	public List<InputItem> InputList => string.IsNullOrEmpty(InputListDt) ? new List<InputItem>() : JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);

	[JsonIgnore]
	public List<ParmItem> ParmList => string.IsNullOrEmpty(ParmListDt) ? new List<ParmItem>() : JsonConvert.DeserializeObject<List<ParmItem>>(ParmListDt);

	[JsonIgnore]
	public List<MULTINGListItem> NGList => string.IsNullOrEmpty(NGListDt) ? new List<MULTINGListItem>() : JsonConvert.DeserializeObject<List<MULTINGListItem>>(NGListDt);

	public WOCHECKOUT_MULTIRequest()
	{
		base.TransactionName = MESTransactionName.WOCHECKOUT_MULTI;
	}
}
