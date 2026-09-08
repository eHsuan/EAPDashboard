using System;

namespace cyntec.MES;

[Serializable]
public class WO_ReelCheck
{
	public string ReelID = "";

	public string WoNo = "";

	public string MatNo = "";

	public string TPNO = "";

	public string EqpNo = "";

	public string ComputerName = "";

	public int ReelIDCount = 0;

	public int MatNoCount = 0;

	public int TPNoCount = 0;

	public DateTime StartTime;

	public DateTime UpdateTime;

	public string TapingUILogTxnID = "";
}
