using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ReelGetDecodeBarcodeRequest : MESBaseMessage
{
	public string EncodeBarcode { get; set; }

	public ReelGetDecodeBarcodeRequest()
	{
		base.TransactionName = MESTransactionName.ReelGetDecodeBarcode;
	}
}
