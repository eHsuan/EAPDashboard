using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class GetTestProgramRequest : MESBaseMessage
{
	public string MatGroupNo { get; set; }

	public string DeltaMatNo { get; set; }

	public GetTestProgramRequest()
	{
		base.TransactionName = MESTransactionName.GetTestProgram;
	}
}
