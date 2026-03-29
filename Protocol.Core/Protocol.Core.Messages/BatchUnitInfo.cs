using System.Collections.Generic;

namespace Protocol.Core.Messages;

public class BatchUnitInfo
{
    public string MaterialID { get; set; }
    public string Result { get; set; }
    public string ProcessTime { get; set; }
    public string ProcessTimestamp { get; set; }
    public List<ParameterValue> ProcessData { get; set; }
}
