using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class EQPMOULDQRYReply : MESBaseMessage
{
	public string UsageQty { get; set; }

	public string TotalUsageQty { get; set; }

	public string MaintainQuota { get; set; }

	public string LifeQuota { get; set; }

	public string MatNo { get; set; }

	public string ToolTypeCode { get; set; }

	public string CheckDate { get; set; }

	public string CheckQuota { get; set; }

	public string CreateDate { get; set; }

	public string CreateUser { get; set; }

	public string DefectQuota { get; set; }

	public string Enable { get; set; }

	public string EOL { get; set; }

	public string Expired { get; set; }

	public string ID { get; set; }

	public string Layer { get; set; }

	public string LifeType { get; set; }

	public string MachNo { get; set; }

	public string MaintainDate { get; set; }

	public string MatGroupNo { get; set; }

	public string NeedCheck { get; set; }

	public string NeedMaintain { get; set; }

	public string OnLineDate { get; set; }

	public string OperateNote { get; set; }

	public string Overed { get; set; }

	public string PONo { get; set; }

	public string Ratio { get; set; }

	public string Remark { get; set; }

	public string ScrapDate { get; set; }

	public string StatusCode { get; set; }

	public string ToolName { get; set; }

	public string ToolNo { get; set; }

	public string Unit { get; set; }

	public string UpdateDate { get; set; }

	public string UpdateUser { get; set; }

	public string ValidEndDate { get; set; }

	public string ValidStartDate { get; set; }

	public string VendorCode { get; set; }

	public string Version { get; set; }

	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public EQPMOULDQRYReply()
	{
		base.TransactionName = MESTransactionName.EQPMOULDQRY;
	}
}
