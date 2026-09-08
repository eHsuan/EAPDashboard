using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_Get : TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>
{
	public string _WoNo = "";

	public List<string> _GorupWoNoList = new List<string>();

	public string _ReelID = "";

	public string _StorBoxNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
