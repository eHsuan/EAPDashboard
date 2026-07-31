using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class TraceMatCreateRequest : MESBaseMessage
{
	public string MatGroupNo { get; set; }

	public string UseType { get; set; }

	public string Classify { get; set; }

	public string Classify_Name { get; set; }

	public string GlueName { get; set; }

	public string SerialNo_1 { get; set; }

	public string IQC_QR_1 { get; set; }

	public string Qty_1 { get; set; }

	public string SerialNo_2 { get; set; }

	public string IQC_QR_2 { get; set; }

	public string Qty_2 { get; set; }

	public string SerialNo_3 { get; set; }

	public string IQC_QR_3 { get; set; }

	public string Qty_3 { get; set; }

	public string SerialNo_4 { get; set; }

	public string IQC_QR_4 { get; set; }

	public string Qty_4 { get; set; }

	public string SerialNo_5 { get; set; }

	public string IQC_QR_5 { get; set; }

	public string Qty_5 { get; set; }

	public string SerialNo_6 { get; set; }

	public string IQC_QR_6 { get; set; }

	public string Qty_6 { get; set; }

	public string DataRemark { get; set; }

	public string RequestQty { get; set; }

	public TraceMatCreateRequest()
	{
		base.TransactionName = MESTransactionName.TraceMatCreate;
	}
}
