using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class WOQueueTime
{
	public string WONo = "";

	public string StartWorkcenterNo = "";

	public string EndWorkcenterNo = "";

	public DateTime StartTime;

	public double QuotaTimes = 0.0;

	public bool Enable = true;

	public DateTime ServerCurrentTime;

	public string CreateUser = "";

	public DateTime CreateDate;

	public string ChangeUser = "";

	public DateTime ChangeDate;

	public bool IsTimeOut = false;

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["WONo"] = WONo;
		dataRow["StartWorkcenterNo"] = StartWorkcenterNo;
		dataRow["EndWorkcenterNo"] = EndWorkcenterNo;
		dataRow["StartTime"] = StartTime;
		dataRow["QuotaTimes"] = QuotaTimes;
		dataRow["Enable"] = Enable;
		dataRow["ServerCurrentTime"] = ServerCurrentTime;
		dataRow["CreateUser"] = CreateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["ChangeUser"] = ChangeUser;
		dataRow["ChangeDate"] = ChangeDate;
		dataRow["IsTimeOut"] = IsTimeOut;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("StartWorkcenterNo");
		dataTable.Columns.Add("EndWorkcenterNo");
		dataTable.Columns.Add("StartTime");
		dataTable.Columns.Add("QuotaTimes");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("ServerCurrentTime");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("ChangeUser");
		dataTable.Columns.Add("ChangeDate");
		dataTable.Columns.Add("IsTimeOut");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WOQueueTime> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WOQueueTime item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
