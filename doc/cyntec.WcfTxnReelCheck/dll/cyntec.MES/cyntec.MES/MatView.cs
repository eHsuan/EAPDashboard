using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class MatView
{
	public string MatGroupNo = "";

	public string MatNo = "";

	public string PlantNo = "";

	public string MatClassCode = "";

	public string ValClassNo = "";

	public int UnitQty = 0;

	public string Remark = "";

	public bool Enable;

	public string UpdateUser = "";

	public DateTime UpdateDate;

	public decimal ReelTapeMinTime = default(decimal);

	public string MarkingFormat = "";

	public string Packing = "";

	public int BoxUnitQty = 0;

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["MatGroupNo"] = MatGroupNo;
		dataRow["MatNo"] = MatNo;
		dataRow["PlantNo"] = PlantNo;
		dataRow["MatClassCode"] = MatClassCode;
		dataRow["ValClassNo"] = ValClassNo;
		dataRow["UnitQty"] = UnitQty;
		dataRow["Remark"] = Remark;
		dataRow["Enable"] = Enable;
		dataRow["UpdateUser"] = UpdateUser;
		dataRow["UpdateDate"] = UpdateDate;
		dataRow["ReelTapeMinTime"] = ReelTapeMinTime;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("MatGroupNo");
		dataTable.Columns.Add("MatNo");
		dataTable.Columns.Add("PlantNo");
		dataTable.Columns.Add("MatClassCode");
		dataTable.Columns.Add("ValClassNo");
		dataTable.Columns.Add("UnitQty");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("UpdateUser");
		dataTable.Columns.Add("UpdateDate");
		dataTable.Columns.Add("ReelTapeMinTime");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<MatView> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (MatView item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
