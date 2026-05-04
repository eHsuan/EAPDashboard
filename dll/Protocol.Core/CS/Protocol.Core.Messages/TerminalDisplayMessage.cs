using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.TerminalDisplay)]
public class TerminalDisplayMessage : BaseMessage
{
	public string Message { get; set; }
}
