using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPSTATUSRequest : MESBaseMessage
{
	public string StatusCode { get; set; }

	public string CycleTime { get; set; }

	public string OutPutQty { get; set; }

	public string TotalOutPutQty { get; set; }

	public string Target_OutPutQty { get; set; }

	public string WONO { get; set; }

	public string ErrorCode { get; set; }

	public string Availability { get; set; }

	public string ParameterList { get; set; }

	public string EqpPartList { get; set; }

	[JsonIgnore]
	public List<ParameterItem> ParameterItems
	{
		get
		{
			if (!string.IsNullOrEmpty(ParameterList))
			{
				return JsonConvert.DeserializeObject<List<ParameterItem>>(ParameterList);
			}
			return new List<ParameterItem>();
		}
	}

	[JsonIgnore]
	public List<EqpPartItem> EqpParts
	{
		get
		{
			if (!string.IsNullOrEmpty(EqpPartList))
			{
				return JsonConvert.DeserializeObject<List<EqpPartItem>>(EqpPartList);
			}
			return new List<EqpPartItem>();
		}
	}

	public EQPSTATUSRequest()
	{
		base.TransactionName = MESTransactionName.EQPSTATUS;
	}
}
