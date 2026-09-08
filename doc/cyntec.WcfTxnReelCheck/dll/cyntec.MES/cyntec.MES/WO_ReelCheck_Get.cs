using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelCheck_Get : TransactionBase<WO_ReelCheck_Get, WO_ReelCheck_GetResult>
{
	public string _WoNo = "";

	public string _TPNO = "";

	public string _MatNo = "";

	public string _ReelID = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
