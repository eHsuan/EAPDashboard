using System;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_Update : TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>
{
	public string _reelID = "";

	public string _statusCode = "";

	public string _statusRemark = "";

	public string _machNo = "";

	public DateTime _startTime;

	public DateTime _endTime;

	public string _UpdateUser = "";

	public override IwcfMessage serverExecute()
	{
		return null;
	}
}
