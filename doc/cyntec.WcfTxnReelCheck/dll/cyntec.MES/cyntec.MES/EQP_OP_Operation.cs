using System;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_Operation
{
	public string Operation;

	public string Description;

	public bool IsEnable;

	public DateTime CreateDate;

	public string CreateUser;

	public DateTime ChangeDate;

	public string ChangeUser;
}
