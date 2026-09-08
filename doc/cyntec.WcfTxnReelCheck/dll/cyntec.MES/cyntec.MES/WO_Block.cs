using System;

namespace cyntec.MES;

[Serializable]
public class WO_Block
{
	public int ID = 0;

	public string WONo = "";

	public string WorkCenterNo = "";

	public string BlockCode = "";

	public string BlockReason = "";

	public string DocNo = "";

	public bool Enable = false;

	public string Remark = "";

	public string CreateUser = "";

	public DateTime CreateDate;

	public string ChangeUser = "";

	public DateTime ChangeDate;
}
