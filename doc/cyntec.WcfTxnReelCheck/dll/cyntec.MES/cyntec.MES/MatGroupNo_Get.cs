using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MatGroupNo_Get : TransactionBase<MatGroupNo_Get, MatGroupNo_GetResult>
{
	public string _MatGroupNo = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
