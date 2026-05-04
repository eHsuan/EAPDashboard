using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.RtnTraceData)]
public class RtnTraceDataMessage : BaseMessage
{
	[JsonProperty("Status")]
	public string Status { get; set; }

	[JsonProperty("CycleTime")]
	public string CycleTime { get; set; }

	[JsonProperty("Availability")]
	public string Availability { get; set; }

	[JsonProperty("Parameter")]
	public List<Dictionary<string, double>> Parameter { get; set; }

	[JsonProperty("RecipeDATA")]
	public RecipeDataContent RecipeDATA { get; set; }
}
