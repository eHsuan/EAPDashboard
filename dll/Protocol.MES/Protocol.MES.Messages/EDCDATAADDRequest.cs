using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EDCDATAADDRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string FlowID { get; set; }

	public string IsLast { get; set; }

	public string ISCOMPONENTMODE { get; set; }

	public string dtEDCRawData { get; set; }

	[JsonIgnore]
	public List<dtEDCRawDataItem> dtEDCRawDatalist => string.IsNullOrEmpty(dtEDCRawData) ? new List<dtEDCRawDataItem>() : JsonConvert.DeserializeObject<List<dtEDCRawDataItem>>(dtEDCRawData);

	public EDCDATAADDRequest()
	{
		base.TransactionName = MESTransactionName.EDCDATAADD;
	}
}
