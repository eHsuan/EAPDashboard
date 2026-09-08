using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MatView_Get : TransactionBase<MatView_Get, MatView_GetResult>
{
	public string _MatNo = "";

	public string _MatClassCode = "";

	public string _MatGroupNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
