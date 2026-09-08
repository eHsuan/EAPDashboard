using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WoInfo_Get : TransactionBase<WoInfo_Get, WoInfo_GetResult>
{
	private int Version = 2;

	public string _WoNo = "";

	public string _BatchNo = "";

	public bool _IsCheckWo = true;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
