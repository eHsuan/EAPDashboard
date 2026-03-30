using Protocol.Core.Base;
using Protocol.Core.Enums;
using Newtonsoft.Json;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.UserID_Verify)]
public class UserIDVerifyMessage : BaseMessage
{
    public string UserID { get; set; }
    public string Barcode { get; set; }
}
