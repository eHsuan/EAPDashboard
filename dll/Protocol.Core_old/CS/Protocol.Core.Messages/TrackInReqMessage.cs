using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.TrackInReq)]
public class TrackInReqMessage : BaseMessage
{
	public string MaterialID { get; set; }

	public string CassetteID { get; set; }

	public string WorkOrder { get; set; }

	public string UserID { get; set; }

	public string ToolingID { get; set; }

	public string PanelID { get; set; }

	public int Qty { get; set; }
}
