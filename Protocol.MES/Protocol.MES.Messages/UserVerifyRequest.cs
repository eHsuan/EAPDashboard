using Newtonsoft.Json;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class UserVerifyRequest : MESBaseMessage
{
    public string User2DBarCode { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string UserPassword { get; set; }

    public UserVerifyRequest()
    {
        base.TransactionName = MESTransactionName.UserVerify;
        base.UserID = null; // 設定為 null 以在序列化時忽略
        this.UserPassword = null;
    }
}
