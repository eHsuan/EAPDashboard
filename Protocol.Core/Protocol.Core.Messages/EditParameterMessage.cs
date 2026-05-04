using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.EditParameter)]
public class EditParameterMessage : BaseMessage
{
	[JsonProperty("Data")]
	public List<EditParameterItem> Data { get; set; }
}
