using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyTraceDataMessage)]
public class ReplyTraceDataMessage : BaseMessage
{
	public int Status { get; set; }

	public double CycleTime { get; set; }

	public int Availability { get; set; }

	public Dictionary<string, double> Parameters { get; set; }

	public RecipeInfo Recipe { get; set; }
}
