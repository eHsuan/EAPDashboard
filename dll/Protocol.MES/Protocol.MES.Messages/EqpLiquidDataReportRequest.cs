using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EqpLiquidDataReportRequest : MESBaseMessage
{
	public string JobCode { get; set; }

	public string JobName { get; set; }

	public string AllowValidateionNGReport { get; set; }

	public string AllowOverwriteExistedData { get; set; }

	public string EqpLiquidItemDT { get; set; }

	[JsonIgnore]
	public List<EqpLiquidItem> EqpLiquidItemList => string.IsNullOrEmpty(EqpLiquidItemDT) ? new List<EqpLiquidItem>() : JsonConvert.DeserializeObject<List<EqpLiquidItem>>(EqpLiquidItemDT);

	public EqpLiquidDataReportRequest()
	{
		base.TransactionName = MESTransactionName.EqpLiquidDataReport;
	}
}
