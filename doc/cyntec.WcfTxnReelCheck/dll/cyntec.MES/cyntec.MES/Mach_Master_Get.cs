using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class Mach_Master_Get : TransactionBase<Mach_Master_Get, Mach_Master_GetResult>
{
	public string _MatGroupNo = "";

	public string _MatClassCode = "";

	public string _MatNo = "";

	public string _WorkCenterNo = "";

	public bool _IsMachNoOnly = false;

	public string _MachNo = "";

	public int _MachID = 0;

	private double _dbAccumulation = 0.0;

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
