using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelGetDecodeBarcodeReply : MESBaseMessage
{
	public string Result { get; set; }

	public string ResultCode { get; set; }

	public string Message { get; set; }

	public string MatNo { get; set; }

	public string Batch { get; set; }

	public string ReelID { get; set; }

	public ReelGetDecodeBarcodeReply()
	{
		base.TransactionName = MESTransactionName.ReelGetDecodeBarcode;
	}
}
