using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOMachineOutRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string Operator { get; set; }

	public string OutPut { get; set; }

	public string File { get; set; }

	public string InputListDt { get; set; }

	public string ParmListDt { get; set; }

	public string TOSDt { get; set; }

	public string CheckItemDt { get; set; }

	public string NGListDt { get; set; }

	[JsonIgnore]
	public List<InputItem> InputList => string.IsNullOrEmpty(InputListDt) ? new List<InputItem>() : JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);

	[JsonIgnore]
	public List<ParmItem> ParmList => string.IsNullOrEmpty(ParmListDt) ? new List<ParmItem>() : JsonConvert.DeserializeObject<List<ParmItem>>(ParmListDt);

	[JsonIgnore]
	public List<TOSItem> TOSList => string.IsNullOrEmpty(TOSDt) ? new List<TOSItem>() : JsonConvert.DeserializeObject<List<TOSItem>>(TOSDt);

	[JsonIgnore]
	public List<CheckItem> CheckItemList => string.IsNullOrEmpty(CheckItemDt) ? new List<CheckItem>() : JsonConvert.DeserializeObject<List<CheckItem>>(CheckItemDt);

	[JsonIgnore]
	public List<NGListItem> NGList => string.IsNullOrEmpty(NGListDt) ? new List<NGListItem>() : JsonConvert.DeserializeObject<List<NGListItem>>(NGListDt);

	public WOMachineOutRequest()
	{
		base.TransactionName = MESTransactionName.WOMachineOut;
	}
}
