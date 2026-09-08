using System;
using System.Collections.Generic;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Record
{
	public string SysID = Guid.NewGuid().ToString();

	public string MatGroupNo = "";

	public string MatNo = "";

	public string WoNo = "";

	public string ReelID = "";

	public string WorkcenterNo = "";

	public int FlowID = 0;

	public int ConfirmID = 0;

	public int MachID = 0;

	public string MachNo = "";

	public string Operation = "";

	public string Instruction = "";

	public DateTime ValidDate;

	public DateTime CreateDate;

	public string CreateUser = "";

	public List<ParameterItem> paramList = new List<ParameterItem>();

	public bool IsException = false;

	public string LastExceptionMsg = "";

	public string ExceptionWoNo = "";

	public string ExceptionType = "";
}
