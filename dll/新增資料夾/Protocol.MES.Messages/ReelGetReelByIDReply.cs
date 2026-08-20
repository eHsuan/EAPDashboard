using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelGetReelByIDReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string ReelID { get; set; }

	public string Area { get; set; }

	public string WONo { get; set; }

	public string MatNo { get; set; }

	public string BatchNo { get; set; }

	public string Serno { get; set; }

	public string Qty { get; set; }

	public string RePrt { get; set; }

	public string SLabel { get; set; }

	public string Status { get; set; }

	public string RefReelID { get; set; }

	public string Remark { get; set; }

	public string CreateDate { get; set; }

	public new string UserID { get; set; }

	public string PC { get; set; }

	public string SAPStatus { get; set; }

	public string SAPRemark { get; set; }

	public string TPStatus { get; set; }

	public string TPRemark { get; set; }

	public string TPMachNo { get; set; }

	public string TPStartTime { get; set; }

	public string TPEndTime { get; set; }

	public string TCStatus { get; set; }

	public string TCRemark { get; set; }

	public string FQCStatus { get; set; }

	public string FQCRemark { get; set; }

	public string UpdateUser { get; set; }

	public string UpdateDate { get; set; }

	public ReelGetReelByIDReply()
	{
		base.TransactionName = MESTransactionName.ReelGetReelByID;
	}
}
