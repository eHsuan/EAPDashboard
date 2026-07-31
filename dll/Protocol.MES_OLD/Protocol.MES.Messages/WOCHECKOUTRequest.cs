using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOCHECKOUTRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string ComponentBatchOut { get; set; }

	public string Operator { get; set; }

	public string OutPut { get; set; }

	public string File { get; set; }

	public string Remark { get; set; }

	public string InputListDt { get; set; }

	public string ParmListDt { get; set; }

	public string TOSDt { get; set; }

	public string CheckItemDt { get; set; }

	public string NGListDt { get; set; }

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
	public List<NGListItem> NGList
	{
		get
		{
			if (!string.IsNullOrEmpty(NGListDt))
			{
				return JsonConvert.DeserializeObject<List<NGListItem>>(NGListDt);
			}
			return new List<NGListItem>();
		}
	}

	public WOCHECKOUTRequest()
	{
		base.TransactionName = MESTransactionName.WOCHECKOUT;
	}
}
