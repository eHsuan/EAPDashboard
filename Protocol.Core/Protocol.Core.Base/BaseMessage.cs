using Newtonsoft.Json;
using Protocol.Core.Enums;

namespace Protocol.Core.Base;

public abstract class BaseMessage
{
	[JsonProperty("CMD")]
	public CommandType CMD { get; set; }

	[JsonProperty("DATE")]
	public string Date { get; set; }

	public string Machine { get; set; }
}
