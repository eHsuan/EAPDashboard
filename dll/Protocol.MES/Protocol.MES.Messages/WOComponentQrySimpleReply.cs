using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class WOComponentQrySimpleReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string SerialNo { get; set; }

	public string RefSerialNo { get; set; }

	public string WONo { get; set; }

	public string RefWONO { get; set; }

	public string MatGroupNo { get; set; }

	public int FlowID { get; set; }

	public int CurrentQty { get; set; }

	public string WorkCenterNo { get; set; }

	public string CreateDate { get; set; }

	public string CreateUser { get; set; }

	public string ChangeDate { get; set; }

	public string ChangeUser { get; set; }

	public int InitQty { get; set; }

	public int InitNGQty { get; set; }

	public string NGWOComponentConfirmID { get; set; }

	public string NGWorkCenterNo { get; set; }

	public string NGCode { get; set; }

	public string NGName { get; set; }

	public string LabelID { get; set; }

	public string LabelContent_QR { get; set; }

	public string StatusCode { get; set; }

	public string StatusCodeActual { get; set; }

	public WOComponentQrySimpleReply()
	{
		base.TransactionName = MESTransactionName.WOComponentQrySimple;
	}
}
