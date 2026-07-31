using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UploadCMKRequest : MESBaseMessage
{
	public string ParamCode { get; set; }

	public string MatNo { get; set; }

	public string UpperValue { get; set; }

	public string LowValue { get; set; }

	public string ActionCategory { get; set; }

	public string Remark { get; set; }

	public string Data { get; set; }

	[JsonIgnore]
	public List<double> DataList => string.IsNullOrEmpty(Data) ? new List<double>() : JsonConvert.DeserializeObject<List<double>>(Data);

	public UploadCMKRequest()
	{
		base.TransactionName = MESTransactionName.UploadCMK;
	}
}
