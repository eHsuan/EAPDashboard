using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.SelectParameter)]
public class SelectParameterMessage : BaseMessage
{
	[JsonProperty("Data")]
	public List<SelectParameterItem> Data { get; set; }
}
