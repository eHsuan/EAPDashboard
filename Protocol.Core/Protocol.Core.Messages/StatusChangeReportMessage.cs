using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.StatusChangeReport)]
public class StatusChangeReportMessage : BaseMessage
{
	public int OldStatus { get; set; }

	public int NewStatus { get; set; }
}
