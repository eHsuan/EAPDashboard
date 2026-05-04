using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyTrackInReq)]
public class ReplyTrackInReqMessage : ReplyBase
{
	public string MaterialID { get; set; }

	public string CassetteID { get; set; }

	public string WorkOrder { get; set; }

	public string UserID { get; set; }

	public string ToolingID { get; set; }

	public string PanelID { get; set; }

	public List<string> PanelList { get; set; }
}
