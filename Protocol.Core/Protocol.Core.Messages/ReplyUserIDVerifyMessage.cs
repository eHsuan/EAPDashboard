using Protocol.Core.Base;
using Protocol.Core.Enums;
using Newtonsoft.Json;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ReplyUserID_Verify)]
public class ReplyUserIDVerifyMessage : ReplyBase
{
    public string UserID { get; set; }
    public string Barcode { get; set; }
}
