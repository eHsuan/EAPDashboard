using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class StdPartsResultUploadRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string StdPartsID { get; set; }

	public string Reason { get; set; }

	public string Comments { get; set; }

	public List<StdPartsResultItem> StdPartsItems { get; set; }

	public StdPartsResultUploadRequest()
	{
		base.TransactionName = MESTransactionName.StdPartsResultUpload;
	}
}
