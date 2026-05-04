using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplySelectParameter)]
public class ReplySelectParameterMessage : ReplyBase
{
	[JsonProperty("Data")]
	public List<ParameterResultItem> Data { get; set; }
}
