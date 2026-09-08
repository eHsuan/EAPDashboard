using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class WOConfirm
{
	public int ID = 0;

	public string WONo = "";

	public int FlowID = 0;

	public string WorkCenterNo = "";

	public int InputQty = 0;

	public int OutPutQty = 0;

	public DateTime StartDate;

	public DateTime EndDate;

	public bool Canceled;

	public string Finished;

	public string Remark = "";

	public string HSF = "";

	public string Operator = "";

	public DateTime CreateDate;

	public string CreateUser;

	public DateTime ChangeDate;

	public string ChangeUser = "";

	public int NGQty = 0;

	public int RefInputQty = 0;

	public int RefOutputQty = 0;

	public string InUser = "";

	public DateTime InTime;

	public bool IsCheckIn => InTime != DateTime.MinValue;

	public bool IsInTimeNG(DateTime dt)
	{
		DateTime dateTime = Convert.ToDateTime(InTime.ToString("yyyy-MM-dd HH:mm:ss"));
		DateTime dateTime2 = Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm:ss"));
		return dateTime != dateTime2;
	}

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		dataRow["ID"] = ID;
		dataRow["WONo"] = WONo;
		dataRow["FlowID"] = FlowID;
		dataRow["WorkCenterNo"] = WorkCenterNo;
		dataRow["InputQty"] = InputQty;
		dataRow["OutPutQty"] = OutPutQty;
		dataRow["StartDate"] = StartDate;
		dataRow["EndDate"] = EndDate;
		dataRow["Canceled"] = Canceled;
		dataRow["Finished"] = Finished;
		dataRow["Remark"] = Remark;
		dataRow["HSF"] = HSF;
		dataRow["Operator"] = Operator;
		dataRow["CreateDate"] = CreateDate;
		dataRow["CreateUser"] = CreateUser;
		dataRow["ChangeDate"] = ChangeDate;
		dataRow["ChangeUser"] = ChangeUser;
		dataRow["NGQty"] = NGQty;
		dataRow["RefInputQty"] = RefInputQty;
		dataRow["RefOutputQty"] = RefOutputQty;
		dataRow["InUser"] = InUser;
		dataRow["InTime"] = InTime;
		mainTable.Rows.Add(dataRow);
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("ID");
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("FlowID");
		dataTable.Columns.Add("WorkCenterNo");
		dataTable.Columns.Add("InputQty");
		dataTable.Columns.Add("OutPutQty");
		dataTable.Columns.Add("StartDate");
		dataTable.Columns.Add("EndDate");
		dataTable.Columns.Add("Canceled");
		dataTable.Columns.Add("Finished");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("HSF");
		dataTable.Columns.Add("Operator");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("ChangeDate");
		dataTable.Columns.Add("ChangeUser");
		dataTable.Columns.Add("NGQty");
		dataTable.Columns.Add("RefInputQty");
		dataTable.Columns.Add("RefOutputQty");
		dataTable.Columns.Add("InUser");
		dataTable.Columns.Add("InTime");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WOConfirm> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WOConfirm item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
