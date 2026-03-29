using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyStatusReq)]
public class ReplyStatusReqMessage : ReplyBase
{
	public string CurrentStatus { get; set; }
	public string RecipeID { get; set; }
}
