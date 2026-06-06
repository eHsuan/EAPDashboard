using System.Collections.Generic;
using Newtonsoft.Json;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.EventReport)]
public class EventReportMessage : BaseMessage
{
	[JsonProperty("EventID")]
	public string EventID { get; set; }

	[JsonProperty("EventName")]
	public string EventName { get; set; }

	[JsonProperty("Data")]
	public List<EventItem> Data { get; set; }
}
