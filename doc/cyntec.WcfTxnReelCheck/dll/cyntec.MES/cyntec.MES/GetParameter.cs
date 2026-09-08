using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class GetParameter : TransactionBase<GetParameter, GetParameterResult>
{
	public string _Material = "";

	public string _WorkcenterNo = "";

	public string _WoNo = "";

	public DateTime? _StartDate;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
