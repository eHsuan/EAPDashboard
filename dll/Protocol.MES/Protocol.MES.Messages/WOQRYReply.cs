using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;

namespace Protocol.MES.Messages;

public class WOQRYReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MatGroupNo { get; set; }

	public string LineCode { get; set; }

	public string MatNo { get; set; }

	public string WIPQty { get; set; }

	public string MatClassCode { get; set; }

	public string WorkCenterNo { get; set; }

	public string FlowID { get; set; }

	public string AlreadyInFlag { get; set; }

	public string SToPcs { get; set; }

	public string WToPcs { get; set; }

	public string ProductStage { get; set; }

	public string RefInputQty { get; set; }

	public string BatchNo { get; set; }

	public string WorkCenterName { get; set; }

	public string IsUrgent { get; set; }

	public string CustomerDeliveryTime { get; set; }

	public string NGCodeListDT { get; set; }

	public string InputCodeListDT { get; set; }

	public string InputParamListDT { get; set; }

	[JsonIgnore]
	public List<NGCodeItem> NGCodeList
	{
		get
		{
			return string.IsNullOrEmpty(NGCodeListDT) ? new List<NGCodeItem>() : JsonConvert.DeserializeObject<List<NGCodeItem>>(NGCodeListDT);
		}
		set
		{
			NGCodeListDT = JsonConvert.SerializeObject((object)value);
		}
	}

	[JsonIgnore]
	public List<InputCodeItem> InputCodeList
	{
		get
		{
			return string.IsNullOrEmpty(InputCodeListDT) ? new List<InputCodeItem>() : JsonConvert.DeserializeObject<List<InputCodeItem>>(InputCodeListDT);
		}
		set
		{
			InputCodeListDT = JsonConvert.SerializeObject((object)value);
		}
	}

	[JsonIgnore]
	public List<InputParamItem> InputParamList
	{
		get
		{
			return string.IsNullOrEmpty(InputParamListDT) ? new List<InputParamItem>() : JsonConvert.DeserializeObject<List<InputParamItem>>(InputParamListDT);
		}
		set
		{
			InputParamListDT = JsonConvert.SerializeObject((object)value);
		}
	}
}
