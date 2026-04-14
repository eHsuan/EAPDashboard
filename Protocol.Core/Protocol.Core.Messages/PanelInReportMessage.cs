using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.PanelInReport)]
public class PanelInReportMessage : BaseMessage
{
    public string CassetteID { get; set; }
    public string PanelID { get; set; }
    public string SlotNo { get; set; }
}
