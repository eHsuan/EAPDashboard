using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class COMPONENTQUERYReply : MESBaseMessage
{
	public string MatGroupNo { get; set; }

	public string LineCode { get; set; }

	public string WONO { get; set; }

	public string MatNo { get; set; }

	public string WIPQty { get; set; }

	public string MatClassCode { get; set; }

	public string WorkCenterNo { get; set; }

	public string AlreadyInFlag { get; set; }

	public string WorkCenterName { get; set; }

	public string FlowID { get; set; }

	public string WToPCS { get; set; }

	public string SToPCS { get; set; }

	public string NGCodeListDT { get; set; }

	public string InputCodeListDT { get; set; }

	public string InputParamListDT { get; set; }

	public string TraceMatBatchDt { get; set; }

	public string WOCarrierDt { get; set; }

	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	[JsonIgnore]
	public List<WoNoListItem> NGCodeListDTList => string.IsNullOrEmpty(NGCodeListDT) ? new List<WoNoListItem>() : JsonConvert.DeserializeObject<List<WoNoListItem>>(NGCodeListDT);

	[JsonIgnore]
	public List<InputCodeItem> InputCodeListDTList => string.IsNullOrEmpty(InputCodeListDT) ? new List<InputCodeItem>() : JsonConvert.DeserializeObject<List<InputCodeItem>>(InputCodeListDT);

	[JsonIgnore]
	public List<InputParamItem> InputParamListDTList => string.IsNullOrEmpty(InputParamListDT) ? new List<InputParamItem>() : JsonConvert.DeserializeObject<List<InputParamItem>>(InputParamListDT);

	[JsonIgnore]
	public List<TraceMatBatchItem> TraceMatBatchList => string.IsNullOrEmpty(TraceMatBatchDt) ? new List<TraceMatBatchItem>() : JsonConvert.DeserializeObject<List<TraceMatBatchItem>>(TraceMatBatchDt);

	[JsonIgnore]
	public List<CarrierItem> WOCarrierList => string.IsNullOrEmpty(WOCarrierDt) ? new List<CarrierItem>() : JsonConvert.DeserializeObject<List<CarrierItem>>(WOCarrierDt);

	public COMPONENTQUERYReply()
	{
		base.TransactionName = MESTransactionName.COMPONENTQUERY;
	}
}
