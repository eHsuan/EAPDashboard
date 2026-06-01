using System.Collections.Generic;
using Newtonsoft.Json;

namespace Protocol.Core.Messages;

public class RecipeDataContent
{
	[JsonProperty("RecipeID")]
	public string RecipeID { get; set; }

	[JsonProperty("RecipeParameter")]
	public Dictionary<string, double> RecipeParameter { get; set; }
}
