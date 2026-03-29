using System;
using Newtonsoft.Json;
using Protocol.MES.Enums;

namespace Protocol.MES.Base;

public abstract class MESBaseMessage
{
    public MESTransactionName TransactionName { get; set; }
    public string EqpNo { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string UserID { get; set; }
    
    // 對齊實測範例的格式: 2026-03-26 12:24:14.868
    public string ModifyDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    
    // 範例中的固定值或識別碼
    public string MacID { get; set; } = "12345";
    
    // 對齊實測範例格式: 2026/03/26 12:23:19.556
    public string ReceivedTime { get; set; } = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
    
    public string ProcessedTotalSeconds { get; set; } = "0.0";
}
