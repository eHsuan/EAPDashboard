using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_Get_WCF : TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>
{
	public string _BatchNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
