using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOMachineInRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string EqpListDt { get; set; }

	public string InputListDt { get; set; }

	public string TraceMatBatchDt { get; set; }

	public string TOSDt { get; set; }

	public string WOCarrierDt { get; set; }

	public string CheckItemDt { get; set; }

	public string ParmListDt { get; set; }

	[JsonIgnore]
	public List<EqpItem> EqpList => string.IsNullOrEmpty(EqpListDt) ? new List<EqpItem>() : JsonConvert.DeserializeObject<List<EqpItem>>(EqpListDt);

	[JsonIgnore]
	public List<InputItem> InputList => string.IsNullOrEmpty(InputListDt) ? new List<InputItem>() : JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);

	[JsonIgnore]
	public List<TraceMatBatchItem> TraceMatBatchList => string.IsNullOrEmpty(TraceMatBatchDt) ? new List<TraceMatBatchItem>() : JsonConvert.DeserializeObject<List<TraceMatBatchItem>>(TraceMatBatchDt);

	[JsonIgnore]
	public List<TOSItem> TOSList => string.IsNullOrEmpty(TOSDt) ? new List<TOSItem>() : JsonConvert.DeserializeObject<List<TOSItem>>(TOSDt);

	[JsonIgnore]
	public List<CarrierItem> WOCarrierList => string.IsNullOrEmpty(WOCarrierDt) ? new List<CarrierItem>() : JsonConvert.DeserializeObject<List<CarrierItem>>(WOCarrierDt);

	[JsonIgnore]
	public List<CheckItem> CheckItemList => string.IsNullOrEmpty(CheckItemDt) ? new List<CheckItem>() : JsonConvert.DeserializeObject<List<CheckItem>>(CheckItemDt);

	[JsonIgnore]
	public List<ParmItem> ParmList => string.IsNullOrEmpty(ParmListDt) ? new List<ParmItem>() : JsonConvert.DeserializeObject<List<ParmItem>>(ParmListDt);

	public WOMachineInRequest()
	{
		base.TransactionName = MESTransactionName.WOMachineIn;
	}
}
