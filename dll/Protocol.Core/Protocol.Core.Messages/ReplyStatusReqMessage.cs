using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyStatusReq)]
public class ReplyStatusReqMessage : ReplyBase
{
	public int CurrentStatus { get; set; }

	public string AlarmCode { get; set; }
}
