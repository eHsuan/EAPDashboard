using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class WO_ReelIDLog
{
	public string Parent_ReelID = "";

	public string ReelID = "";

	public string SMatNo = "";

	public string SBatchNo = "";

	public string MatNo = "";

	public string BatchNo = "";

	public string Big_ReelID = "";

	public string WONO = "";

	public string Status = "";

	public string Action = "";

	public string DNDOC = "";

	public string DNITEM = "";

	public string MMDOC = "";

	public string MMITEM = "";

	public string UserID = "";

	public DateTime CreateDate;

	public string CreateUser = "";

	public bool IsSAPLog = false;

	public int ConfirmID = 0;

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		dataRow["Parent_ReelID"] = Parent_ReelID;
		dataRow["ReelID"] = ReelID;
		dataRow["SMatNo"] = SMatNo;
		dataRow["SBatchNo"] = SBatchNo;
		dataRow["MatNo"] = MatNo;
		dataRow["BatchNo"] = BatchNo;
		dataRow["Big_ReelID"] = Big_ReelID;
		dataRow["WONO"] = WONO;
		dataRow["Status"] = Status;
		dataRow["Action"] = Action;
		dataRow["DNDOC"] = DNDOC;
		dataRow["DNITEM"] = DNITEM;
		dataRow["MMDOC"] = MMDOC;
		dataRow["MMITEM"] = MMITEM;
		dataRow["UserID"] = UserID;
		dataRow["CreateDate"] = CreateDate;
		dataRow["CreateUser"] = CreateUser;
		dataRow["IsSAPLog"] = IsSAPLog;
		dataRow["ConfirmID"] = ConfirmID;
		mainTable.Rows.Add(dataRow);
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("Parent_ReelID");
		dataTable.Columns.Add("ReelID");
		dataTable.Columns.Add("SMatNo");
		dataTable.Columns.Add("SBatchNo");
		dataTable.Columns.Add("MatNo");
		dataTable.Columns.Add("BatchNo");
		dataTable.Columns.Add("Big_ReelID");
		dataTable.Columns.Add("WONO");
		dataTable.Columns.Add("Status");
		dataTable.Columns.Add("Action");
		dataTable.Columns.Add("DNDOC");
		dataTable.Columns.Add("DNITEM");
		dataTable.Columns.Add("MMDOC");
		dataTable.Columns.Add("MMITEM");
		dataTable.Columns.Add("UserID");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("IsSAPLog");
		dataTable.Columns.Add("ConfirmID");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WO_ReelIDLog> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WO_ReelIDLog item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
