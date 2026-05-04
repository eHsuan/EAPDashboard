using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplySelectParameter)]
public class ReplySelectParameterMessage : ReplyBase
{
	public List<ParameterValue> Parameters { get; set; }
}
