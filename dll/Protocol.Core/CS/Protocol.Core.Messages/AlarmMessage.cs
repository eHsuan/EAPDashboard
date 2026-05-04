using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ALARM)]
public class AlarmMessage : BaseMessage
{
	public string AlarmStatus { get; set; }

	public string AlarmCode { get; set; }

	public string AlarmMsg { get; set; }

	public string AlarmType { get; set; }
}
