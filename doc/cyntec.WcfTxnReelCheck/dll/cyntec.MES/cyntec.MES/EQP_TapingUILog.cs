using System;

namespace cyntec.MES;

[Serializable]
public class EQP_TapingUILog
{
	public string TxnID;

	public EQP_TapingUILogType LogType;

	public string UIName;

	public string UIVersion;

	public string WoNo;

	public string WorkcenterNo;

	public string EqpNo;

	public string ComputerName;

	public EQP_TapingUILogAction Action;

	public string Remark;

	public DateTime CreateDate;

	public string CreateUser;

	public int ConfirmId = 0;

	public int FlowID = 0;
}
