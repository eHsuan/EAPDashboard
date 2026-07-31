using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class FAIMEASUREMENTRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string FIRSTARTICLData { get; set; }

	[JsonIgnore]
	public List<FIRSTARTICLDataListItem> FIRSTARTICLDataList => string.IsNullOrEmpty(FIRSTARTICLData) ? new List<FIRSTARTICLDataListItem>() : JsonConvert.DeserializeObject<List<FIRSTARTICLDataListItem>>(FIRSTARTICLData);

	public FAIMEASUREMENTRequest()
	{
		base.TransactionName = MESTransactionName.FAIMEASUREMENT;
	}
}
