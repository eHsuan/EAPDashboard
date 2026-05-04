using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.BatchProcessDataReport)]
public class BatchProcessDataReportMessage : BaseMessage
{
    public string UserID { get; set; }
    public string CassetteID { get; set; }
    public string WorkOrder { get; set; }
    public string RecipeID { get; set; }
    public int BatchQty { get; set; }
    public List<BatchUnitInfo> UnitList { get; set; }
}
