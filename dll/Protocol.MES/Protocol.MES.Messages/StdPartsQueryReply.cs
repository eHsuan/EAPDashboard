using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class StdPartsQueryReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string StdPartsID { get; set; }

	public string StdPartsType { get; set; }

	public string CategoryCode { get; set; }

	public string CategoryName { get; set; }

	public string PlantNo { get; set; }

	public string LineCode { get; set; }

	public string MatGroupNo { get; set; }

	public string MatNo { get; set; }

	public string WorkCenterNo { get; set; }

	public int NoticeDay { get; set; }

	public string PublishDate { get; set; }

	public string Remark { get; set; }

	public int SafeQty { get; set; }

	public string SendDate { get; set; }

	public string StdPartsAlias { get; set; }

	public string ValidDate { get; set; }

	public int ValidPeriod { get; set; }

	public List<StdPartsSpecItem> StdPartsItems { get; set; }

	public StdPartsQueryReply()
	{
		base.TransactionName = MESTransactionName.StdPartsQuery;
	}
}
