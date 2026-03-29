using System.Collections.Generic;
using Protocol.Core.Base;
using Protocol.Core.Enums;

namespace Protocol.Core.Messages;

[CommandMapping(CommandType.ProcessDataReport)]
public class ProcessDataReportMessage : BaseMessage
{
	public string UserID { get; set; }

	public string MaterialID { get; set; }

	public string CassetteID { get; set; }

	public string WorkOrder { get; set; }

	public string RecipeID { get; set; }

	public string Result { get; set; }

	public double ProcessTime { get; set; }

	public List<ParameterValue> ProcessData { get; set; }
}
