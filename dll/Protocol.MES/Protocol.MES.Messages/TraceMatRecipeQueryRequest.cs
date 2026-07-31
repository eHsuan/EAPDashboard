using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TraceMatRecipeQueryRequest : MESBaseMessage
{
	public string MatGroupNo { get; set; }

	public string UseType { get; set; }

	public string Classify_Name { get; set; }

	public string Classify { get; set; }

	public string GlueName { get; set; }

	public string IsQryRecipeHeadOnly { get; set; }

	public TraceMatRecipeQueryRequest()
	{
		base.TransactionName = MESTransactionName.TraceMatRecipeQuery;
	}
}
