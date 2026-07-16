using Newtonsoft.Json;

namespace Protocol.Core.Messages;

public class SelectParameterItem
{
	[JsonProperty("Parameter")]
	public string Parameter { get; set; }
}
