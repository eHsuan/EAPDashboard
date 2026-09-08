using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_Block_Get : TransactionBase<WO_Block_Get, WO_Block_GetResult>
{
	public string _WONo = "";

	public bool _IsEnable = false;

	private double _dbAccumulation = 0.0;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
