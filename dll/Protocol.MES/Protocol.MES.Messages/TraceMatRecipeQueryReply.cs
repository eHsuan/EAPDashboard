using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TraceMatRecipeQueryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MatRecipeHeadDt { get; set; }

	public string MatRecipeBodyDt { get; set; }

	[JsonIgnore]
	public List<MatRecipeHeadItem> MatRecipeHeadList => string.IsNullOrEmpty(MatRecipeHeadDt) ? new List<MatRecipeHeadItem>() : JsonConvert.DeserializeObject<List<MatRecipeHeadItem>>(MatRecipeHeadDt);

	[JsonIgnore]
	public List<MatRecipeBodyItem> MatRecipeBodyList => string.IsNullOrEmpty(MatRecipeBodyDt) ? new List<MatRecipeBodyItem>() : JsonConvert.DeserializeObject<List<MatRecipeBodyItem>>(MatRecipeBodyDt);

	public TraceMatRecipeQueryReply()
	{
		base.TransactionName = MESTransactionName.TraceMatRecipeQuery;
	}
}
