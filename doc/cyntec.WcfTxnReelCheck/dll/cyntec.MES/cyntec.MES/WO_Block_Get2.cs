using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Block_Get2 : TransactionBase<WO_Block_Get2, WO_Block_Get2Result>
{
	public string _WONo = "";

	public bool _IsEnable = false;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
