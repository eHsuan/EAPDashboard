using Protocol.Core.Enums;

namespace Protocol.Core.Base;

public abstract class ReplyBase : BaseMessage
{
	public RtnResult RtnResult { get; set; }

	public string RtnMsg { get; set; }
}
