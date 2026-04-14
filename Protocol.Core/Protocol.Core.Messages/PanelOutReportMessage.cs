using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.PanelOutReport)]
public class PanelOutReportMessage : BaseMessage
{
    public string CassetteID { get; set; }
    public string PanelID { get; set; }
    public string SlotNo { get; set; }
    public string Result { get; set; }
    public string NGCode { get; set; }
}
