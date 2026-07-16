using Newtonsoft.Json;

namespace Protocol.Core.Messages;

public class EditParameterItem
{
	[JsonProperty("Parameter")]
	public string Parameter { get; set; }

	[JsonProperty("Value")]
	public double Value { get; set; }
}
