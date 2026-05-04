using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.RemoteCMD)]
public class RemoteCMDMessage : BaseMessage
{
	public RemoteCommandType RemoteCMDType { get; set; }

	public string RecipeID { get; set; }
}
