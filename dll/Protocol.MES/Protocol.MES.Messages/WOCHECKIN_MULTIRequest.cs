using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKIN_MULTIRequest : MESBaseMessage
{
	public string WoNoDt { get; set; }

	public string IsGroupedEqp { get; set; }

	public string InputListDt { get; set; }

	public string TraceMatBatchDt { get; set; }

	public string WOCarrierDt { get; set; }

	public string ParmListDt { get; set; }

	[JsonIgnore]
	public List<WoNoListItem> WoNoList => string.IsNullOrEmpty(WoNoDt) ? new List<WoNoListItem>() : JsonConvert.DeserializeObject<List<WoNoListItem>>(WoNoDt);

	[JsonIgnore]
	public List<InputItem> InputList => string.IsNullOrEmpty(InputListDt) ? new List<InputItem>() : JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);

	[JsonIgnore]
	public List<TraceMatBatchItem> TraceMatBatchList => string.IsNullOrEmpty(TraceMatBatchDt) ? new List<TraceMatBatchItem>() : JsonConvert.DeserializeObject<List<TraceMatBatchItem>>(TraceMatBatchDt);

	[JsonIgnore]
	public List<CarrierItem> WOCarrierList => string.IsNullOrEmpty(WOCarrierDt) ? new List<CarrierItem>() : JsonConvert.DeserializeObject<List<CarrierItem>>(WOCarrierDt);

	[JsonIgnore]
	public List<ParmItem> ParmList => string.IsNullOrEmpty(ParmListDt) ? new List<ParmItem>() : JsonConvert.DeserializeObject<List<ParmItem>>(ParmListDt);

	public WOCHECKIN_MULTIRequest()
	{
		base.TransactionName = MESTransactionName.WOCHECKIN_MULTI;
	}
}
