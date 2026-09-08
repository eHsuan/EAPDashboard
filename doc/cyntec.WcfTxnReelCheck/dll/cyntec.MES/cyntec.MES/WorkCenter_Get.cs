using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WorkCenter_Get : TransactionBase<WorkCenter_Get, WorkCenter_GetResult>
{
	public string _WorkCenterNo = "";

	public string _MatGroupNo = "";

	public string _MatNo = "";

	public string _MatClassCode = "";

	public string _WorkcenterTypeCode = "";

	public bool? _IsPlan;

	private double _dbAccumulation = 0.0;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
