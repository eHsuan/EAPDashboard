using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.SelectParameter)]
public class SelectParameterMessage : BaseMessage
{
	public List<string> Parameters { get; set; }
}
