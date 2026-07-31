using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EdcPlanGetReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string EdcSpecItemDt { get; set; }

	public string EdcPlanDiagramDt { get; set; }

	[JsonIgnore]
	public List<EdcSpecItem> EdcSpecItemList => string.IsNullOrEmpty(EdcSpecItemDt) ? new List<EdcSpecItem>() : JsonConvert.DeserializeObject<List<EdcSpecItem>>(EdcSpecItemDt);

	[JsonIgnore]
	public List<EdcPlanDiagramItem> EdcPlanDiagramList => string.IsNullOrEmpty(EdcPlanDiagramDt) ? new List<EdcPlanDiagramItem>() : JsonConvert.DeserializeObject<List<EdcPlanDiagramItem>>(EdcPlanDiagramDt);

	public EdcPlanGetReply()
	{
		base.TransactionName = MESTransactionName.EdcPlanGet;
	}
}
