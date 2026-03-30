using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.PP_SELECT)]
public class PPSelectMessage : BaseMessage
{
	public string RecipeID { get; set; }
}
