using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class mesWO_Block
{
	public int ID = 0;

	public string WONo = "";

	public string WorkCenterNo = "";

	public string BlockCode = "";

	public string BlockReason = "";

	public string DocNo = "";

	public bool Enable;

	public string Remark = "";

	public string CreateUser = "";

	public DateTime CreateDate;

	public string ChangeUser = "";

	public DateTime ChangeDate;

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["ID"] = ID;
		dataRow["WONo"] = WONo;
		dataRow["WorkCenterNo"] = WorkCenterNo;
		dataRow["BlockCode"] = BlockCode;
		dataRow["BlockReason"] = BlockReason;
		dataRow["DocNo"] = DocNo;
		dataRow["Enable"] = Enable;
		dataRow["Remark"] = Remark;
		dataRow["CreateUser"] = CreateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["ChangeUser"] = ChangeUser;
		dataRow["ChangeDate"] = ChangeDate;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("ID");
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("WorkCenterNo");
		dataTable.Columns.Add("BlockCode");
		dataTable.Columns.Add("BlockReason");
		dataTable.Columns.Add("DocNo");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("ChangeUser");
		dataTable.Columns.Add("ChangeDate");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<mesWO_Block> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (mesWO_Block item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
