using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class Mach_Master
{
	public int MachID;

	public string MachNo;

	public string MachName;

	public string PlantNo;

	public string MachAlias;

	public string GroupName;

	public string ClassCode;

	public string SubClassCode;

	public string Spec;

	public string Model;

	public int Capacity;

	public double OutPut;

	public string CostCenter;

	public string WorkCenterNo;

	public string LineCode;

	public string StatusCode;

	public string AssetNo;

	public bool SingleAsset;

	public string LocationCode;

	public string PO;

	public string POItem;

	public string VendorCode;

	public string Remark;

	public string CreateUser;

	public DateTime CreateDate;

	public string UpdateUser;

	public DateTime UpdateDate;

	public string WoWorkcenter = "";

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("MachID");
		dataTable.Columns.Add("MachNo");
		dataTable.Columns.Add("MachName");
		dataTable.Columns.Add("PlantNo");
		dataTable.Columns.Add("MachAlias");
		dataTable.Columns.Add("GroupName");
		dataTable.Columns.Add("ClassCode");
		dataTable.Columns.Add("SubClassCode");
		dataTable.Columns.Add("Spec");
		dataTable.Columns.Add("Model");
		dataTable.Columns.Add("Capacity");
		dataTable.Columns.Add("OutPut");
		dataTable.Columns.Add("CostCenter");
		dataTable.Columns.Add("WorkCenterNo");
		dataTable.Columns.Add("LineCode");
		dataTable.Columns.Add("StatusCode");
		dataTable.Columns.Add("AssetNo");
		dataTable.Columns.Add("SingleAsset");
		dataTable.Columns.Add("LocationCode");
		dataTable.Columns.Add("PO");
		dataTable.Columns.Add("POItem");
		dataTable.Columns.Add("VendorCode");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("UpdateUser");
		dataTable.Columns.Add("UpdateDate");
		return dataTable;
	}

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["MachID"] = MachID;
		dataRow["MachNo"] = MachNo;
		dataRow["MachName"] = MachName;
		dataRow["PlantNo"] = PlantNo;
		dataRow["MachAlias"] = MachAlias;
		dataRow["GroupName"] = GroupName;
		dataRow["ClassCode"] = ClassCode;
		dataRow["SubClassCode"] = SubClassCode;
		dataRow["Spec"] = Spec;
		dataRow["Model"] = Model;
		dataRow["Capacity"] = Capacity;
		dataRow["OutPut"] = OutPut;
		dataRow["CostCenter"] = CostCenter;
		dataRow["WorkCenterNo"] = WorkCenterNo;
		dataRow["LineCode"] = LineCode;
		dataRow["StatusCode"] = StatusCode;
		dataRow["AssetNo"] = AssetNo;
		dataRow["SingleAsset"] = SingleAsset;
		dataRow["LocationCode"] = LocationCode;
		dataRow["PO"] = PO;
		dataRow["POItem"] = POItem;
		dataRow["VendorCode"] = VendorCode;
		dataRow["Remark"] = Remark;
		dataRow["CreateUser"] = CreateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["UpdateUser"] = UpdateUser;
		dataRow["UpdateDate"] = UpdateDate;
		return mainTable;
	}

	public static DataTable ToDataTable(IEnumerable<Mach_Master> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (Mach_Master item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
