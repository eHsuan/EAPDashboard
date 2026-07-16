using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.InitTraceData)]
public class InitTraceDataMessage : BaseMessage
{
	public int Timer { get; set; }

	public bool Enable { get; set; }
}
