using System;
using System.Collections.Generic;
using System.Data;
using cyntec.Base;

namespace cyntec.MES;

[Serializable]
public class WorkCenter
{
	public string WorkCenterNo = "";

	public string WorkCenterName = "";

	public string PlantNo = "";

	public bool Enable;

	public DateTime CreateDate;

	public string CreateUser = "";

	public DateTime UpdateDate;

	public string UpdateUser = "";

	public string Note = "";

	public string MatGroupNo = "";

	public string WorkCenterGroupCode = "";

	public string WorkCenterTypeCode = "";

	public int TimeLimitMin = 0;

	public int TimeLimitMax = 0;

	public int CycleTime = 0;

	public string WOUnit = "";

	public bool GroupConfirm = false;

	public bool IsCarrier = false;

	public bool CheckInIsCarrier = false;

	public bool CheckOutIsCarrier = false;

	public string SortID = "";

	public void init(DataRow row)
	{
		WorkCenterNo = row["WorkCenterNo"].ToString();
		WorkCenterName = row["WorkCenterName"].ToString();
		MatGroupNo = row["MatGroupNo"].ToString();
		WorkCenterNo = row["WorkCenterNo"].ToString();
		WorkCenterName = row["WorkCenterName"].ToString();
		PlantNo = row["PlantNo"].ToString();
		Enable = cyntecByteConvert.ToBool(row["Enable"]);
		CreateDate = cyntecDateTime.ToDateTime(row["CreateDate"]);
		CreateUser = row["CreateUser"].ToString();
		UpdateDate = cyntecDateTime.ToDateTime(row["UpdateDate"]);
		UpdateUser = row["UpdateUser"].ToString();
		Note = row["Note"].ToString();
		MatGroupNo = row["MatGroupNo"].ToString();
		WorkCenterGroupCode = row["WorkCenterGroupCode"].ToString();
		WorkCenterTypeCode = row["WorkCenterTypeCode"].ToString();
		TimeLimitMin = int.Parse(row["TimeLimitMin"].ToString());
		TimeLimitMax = int.Parse(row["TimeLimitMax"].ToString());
		CycleTime = int.Parse(row["CycleTime"].ToString());
		WOUnit = row["WOUnit"].ToString();
		GroupConfirm = cyntecByteConvert.ToBool(row["GroupConfirm"]);
		IsCarrier = cyntecByteConvert.ToBool(row["IsCarrier"].ToString());
		CheckInIsCarrier = cyntecByteConvert.ToBool(row["CheckInIsCarrier"]);
		CheckOutIsCarrier = cyntecByteConvert.ToBool(row["CheckOutIsCarrier"]);
		SortID = row["SortID"].ToString();
	}

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["WorkCenterNo"] = WorkCenterNo;
		dataRow["WorkCenterName"] = WorkCenterName;
		dataRow["PlantNo"] = PlantNo;
		dataRow["Enable"] = Enable;
		dataRow["CreateDate"] = CreateDate;
		dataRow["CreateUser"] = CreateUser;
		dataRow["UpdateDate"] = UpdateDate;
		dataRow["UpdateUser"] = UpdateUser;
		dataRow["Note"] = Note;
		dataRow["MatGroupNo"] = MatGroupNo;
		dataRow["WorkCenterGroupCode"] = WorkCenterGroupCode;
		dataRow["WorkCenterTypeCode"] = WorkCenterTypeCode;
		dataRow["TimeLimitMin"] = TimeLimitMin;
		dataRow["TimeLimitMax"] = TimeLimitMax;
		dataRow["CycleTime"] = CycleTime;
		dataRow["WOUnit"] = WOUnit;
		dataRow["GroupConfirm"] = GroupConfirm;
		dataRow["IsCarrier"] = IsCarrier;
		dataRow["CheckInIsCarrier"] = CheckInIsCarrier;
		dataRow["CheckOutIsCarrier"] = CheckOutIsCarrier;
		dataRow["SortID"] = SortID;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("WorkCenterNo");
		dataTable.Columns.Add("WorkCenterName");
		dataTable.Columns.Add("PlantNo");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("UpdateDate");
		dataTable.Columns.Add("UpdateUser");
		dataTable.Columns.Add("Note");
		dataTable.Columns.Add("MatGroupNo");
		dataTable.Columns.Add("WorkCenterGroupCode");
		dataTable.Columns.Add("WorkCenterTypeCode");
		dataTable.Columns.Add("TimeLimitMin");
		dataTable.Columns.Add("TimeLimitMax");
		dataTable.Columns.Add("CycleTime");
		dataTable.Columns.Add("WOUnit");
		dataTable.Columns.Add("GroupConfirm");
		dataTable.Columns.Add("IsCarrier");
		dataTable.Columns.Add("CheckInIsCarrier");
		dataTable.Columns.Add("CheckOutIsCarrier");
		dataTable.Columns.Add("SortID");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WorkCenter> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WorkCenter item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
