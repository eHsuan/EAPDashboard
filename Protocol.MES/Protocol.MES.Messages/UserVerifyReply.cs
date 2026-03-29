using Protocol.MES.Base;

namespace Protocol.MES.Messages;

public class UserVerifyReply : MESBaseMessage
{
    public string Result { get; set; }
    public string ResultCode { get; set; }
    public string Message { get; set; }
    public string UserID { get; set; }
    public string UserName { get; set; }
}
