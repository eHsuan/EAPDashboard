using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.EditParameter)]
public class EditParameterMessage : BaseMessage
{
	public List<ParameterValue> Parameters { get; set; }
}
