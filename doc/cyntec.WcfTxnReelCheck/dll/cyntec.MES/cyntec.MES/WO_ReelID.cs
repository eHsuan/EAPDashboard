using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID
{
	public string ReelID = "";

	public string Area = "";

	public string WONo = "";

	public string MatNo = "";

	public string BatchNo = "";

	public string Serno = "";

	public decimal Qty = default(decimal);

	public string RePrt = "";

	public string Status = "";

	public string RefReelID = "";

	public string Remark = "";

	public DateTime UpdateDate;

	public string UserID = "";

	public string PC = "";

	public string FQCRemark = "";

	public string FQCStatus = "";

	public string TCRemark = "";

	public string TCStatus = "";

	public DateTime TPEndTime;

	public string TPMachNo = "";

	public string TPRemark = "";

	public DateTime TPStartTime;

	public string TPStatus = "";

	public int ReelIDCount = 0;

	public int MatNoCount = 0;

	public int TPNoCount = 0;

	public string StorBoxNo = "";

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		dataRow["ReelID"] = ReelID;
		dataRow["Area"] = Area;
		dataRow["WONo"] = WONo;
		dataRow["MatNo"] = MatNo;
		dataRow["BatchNo"] = BatchNo;
		dataRow["Serno"] = Serno;
		dataRow["Qty"] = Qty;
		dataRow["RePrt"] = RePrt;
		dataRow["Status"] = Status;
		dataRow["RefReelID"] = RefReelID;
		dataRow["Remark"] = Remark;
		dataRow["UpdateDate"] = UpdateDate;
		dataRow["UserID"] = UserID;
		dataRow["PC"] = PC;
		dataRow["FQCRemark"] = FQCRemark;
		dataRow["FQCStatus"] = FQCStatus;
		dataRow["TCRemark"] = TCRemark;
		dataRow["TCStatus"] = TCStatus;
		dataRow["TPEndTime"] = TPEndTime;
		dataRow["TPMachNo"] = TPMachNo;
		dataRow["TPRemark"] = TPRemark;
		dataRow["TPStartTime"] = TPStartTime;
		dataRow["TPStatus"] = TPStatus;
		mainTable.Rows.Add(dataRow);
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("ReelID");
		dataTable.Columns.Add("Area");
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("MatNo");
		dataTable.Columns.Add("BatchNo");
		dataTable.Columns.Add("Serno");
		dataTable.Columns.Add("Qty");
		dataTable.Columns.Add("RePrt");
		dataTable.Columns.Add("Status");
		dataTable.Columns.Add("RefReelID");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("UpdateDate");
		dataTable.Columns.Add("UserID");
		dataTable.Columns.Add("PC");
		dataTable.Columns.Add("FQCRemark");
		dataTable.Columns.Add("FQCStatus");
		dataTable.Columns.Add("TCRemark");
		dataTable.Columns.Add("TCStatus");
		dataTable.Columns.Add("TPEndTime");
		dataTable.Columns.Add("TPMachNo");
		dataTable.Columns.Add("TPRemark");
		dataTable.Columns.Add("TPStartTime");
		dataTable.Columns.Add("TPStatus");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WO_ReelID> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WO_ReelID item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
