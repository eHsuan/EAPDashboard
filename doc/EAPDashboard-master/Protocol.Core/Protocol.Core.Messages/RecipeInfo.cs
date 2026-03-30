using System.Collections.Generic;

namespace Protocol.Core.Messages;

public class RecipeInfo
{
	public string RecipeID { get; set; }

	public Dictionary<string, double> Parameters { get; set; }
}
