using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UPLOADTESTFILERequest : MESBaseMessage
{
	public string WoNoDt { get; set; }

	public string ComponentNo { get; set; }

	public string TransferCode { get; set; }

	public string FileName { get; set; }

	public string File { get; set; }

	[JsonIgnore]
	public List<WoNoListItem> WoNoList => string.IsNullOrEmpty(WoNoDt) ? new List<WoNoListItem>() : JsonConvert.DeserializeObject<List<WoNoListItem>>(WoNoDt);

	public UPLOADTESTFILERequest()
	{
		base.TransactionName = MESTransactionName.UPLOADTESTFILE;
	}
}
