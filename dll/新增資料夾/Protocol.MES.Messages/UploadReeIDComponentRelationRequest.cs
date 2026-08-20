using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UploadReeIDComponentRelationRequest : MESBaseMessage
{
	public string WONo { get; set; }

	public List<ReelIDComponentItem> Components { get; set; }

	public UploadReeIDComponentRelationRequest()
	{
		base.TransactionName = MESTransactionName.UploadReeIDComponentRelation;
	}
}
