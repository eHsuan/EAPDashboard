using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKINRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentBatchIn { get; set; }

	public string EqpListDt { get; set; }

	public string InputListDt { get; set; }

	public string TraceMatBatchDt { get; set; }

	public string TOSDt { get; set; }

	public string WOCarrierDt { get; set; }

	public string CheckItemDt { get; set; }

	public string ParmListDt { get; set; }

	public string EqpItemDt { get; set; }

	[JsonIgnore]
	public List<EqpItem> EqpList
	{
		get
		{
			if (!string.IsNullOrEmpty(EqpListDt))
			{
				return JsonConvert.DeserializeObject<List<EqpItem>>(EqpListDt);
			}
			return new List<EqpItem>();
		}
	}

	[JsonIgnore]
	public List<InputItem> InputList
	{
		get
		{
			if (!string.IsNullOrEmpty(InputListDt))
			{
				return JsonConvert.DeserializeObject<List<InputItem>>(InputListDt);
			}
			return new List<InputItem>();
		}
	}

	[JsonIgnore]
	public List<InputItem> TraceMatBatchList
	{
		get
		{
			if (!string.IsNullOrEmpty(TraceMatBatchDt))
			{
				return JsonConvert.DeserializeObject<List<InputItem>>(TraceMatBatchDt);
			}
			return new List<InputItem>();
		}
	}

	[JsonIgnore]
	public List<TOSItem> TOSList
	{
		get
		{
			if (!string.IsNullOrEmpty(TOSDt))
			{
				return JsonConvert.DeserializeObject<List<TOSItem>>(TOSDt);
			}
			return new List<TOSItem>();
		}
	}

	[JsonIgnore]
	public List<CarrierItem> WOCarrierList
	{
		get
		{
			if (!string.IsNullOrEmpty(WOCarrierDt))
			{
				return JsonConvert.DeserializeObject<List<CarrierItem>>(WOCarrierDt);
			}
			return new List<CarrierItem>();
		}
	}

	[JsonIgnore]
	public List<CheckItem> CheckItemList
	{
		get
		{
			if (!string.IsNullOrEmpty(CheckItemDt))
			{
				return JsonConvert.DeserializeObject<List<CheckItem>>(CheckItemDt);
			}
			return new List<CheckItem>();
		}
	}

	[JsonIgnore]
	public List<ParmItem> ParmList
	{
		get
		{
			if (!string.IsNullOrEmpty(ParmListDt))
			{
				return JsonConvert.DeserializeObject<List<ParmItem>>(ParmListDt);
			}
			return new List<ParmItem>();
		}
	}

	[JsonIgnore]
	public List<EqpItem> EqpItemList
	{
		get
		{
			if (!string.IsNullOrEmpty(EqpItemDt))
			{
				return JsonConvert.DeserializeObject<List<EqpItem>>(EqpItemDt);
			}
			return new List<EqpItem>();
		}
	}

	public WOCHECKINRequest()
	{
		base.TransactionName = MESTransactionName.WOCHECKIN;
	}
}
