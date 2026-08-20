using System.Collections.Generic;

namespace Protocol.MES.Messages;

public class GetCheckTaskDataItem
{
	public int ScheduleID;

	public string EqpNo;

	public string EqpName;

	public string JobCode;

	public string JobName;

	public string FreqCode;

	public string PlanDate;

	public string StartDate;

	public string EndDate;

	public string ExeStartTime;

	public string ExeEndTime;

	public string CheckUser;

	public string CheckResult;

	public string CheckRemark;

	public List<JobItemItem> JobItem;
}
