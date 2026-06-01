using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyRemoteCMD)]
public class ReplyRemoteCMDMessage : ReplyBase
{
	public RemoteCommandType RemoteCMDType { get; set; }
}
