using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class WORouting
{
	public int ID = 0;

	public string WONo = "";

	public string WorkCenterNo = "";

	public int FlowID = 0;

	public string SAPWorkCenter = "";

	public string SAPConfirmWorkCenter = "";

	public bool Confirmation = false;

	public string RefWC01 = "";

	public bool Rank = false;

	public bool Delivered = false;

	public bool IsTaping = false;

	public bool IsPacking = false;

	public bool IOEqual = false;

	public bool IsMeasure = false;

	public string MeasureDataBase = "";

	public string CalcNG = "";

	public bool Enable = false;

	public DateTime CreateDate;

	public string CreateUser = "";

	public DateTime ChangeDate;

	public string ChangeUser = "";

	public bool InCheck = false;

	public string InRefWorkCenter = "";

	public bool IsRework = false;

	public bool LineProduction = false;

	public string WorkcenterTypeCode = "";

	public string WorkCenterName = "";

	public bool IsPlan = false;

	public bool IsMeter = false;

	public bool IsEquipment = false;

	[NonSerialized]
	public WOConfirm oWoConfirm = null;

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
		dataRow["FlowID"] = FlowID;
		dataRow["SAPWorkCenter"] = SAPWorkCenter;
		dataRow["SAPConfirmWorkCenter"] = SAPConfirmWorkCenter;
		dataRow["Confirmation"] = Confirmation;
		dataRow["RefWC01"] = RefWC01;
		dataRow["Rank"] = Rank;
		dataRow["Delivered"] = Delivered;
		dataRow["IsTaping"] = IsTaping;
		dataRow["IsPacking"] = IsPacking;
		dataRow["IOEqual"] = IOEqual;
		dataRow["IsMeasure"] = IsMeasure;
		dataRow["MeasureDataBase"] = MeasureDataBase;
		dataRow["CalcNG"] = CalcNG;
		dataRow["Enable"] = Enable;
		dataRow["CreateDate"] = CreateDate;
		dataRow["CreateUser"] = CreateUser;
		dataRow["ChangeDate"] = ChangeDate;
		dataRow["ChangeUser"] = ChangeUser;
		dataRow["InCheck"] = InCheck;
		dataRow["InRefWorkCenter"] = InRefWorkCenter;
		dataRow["IsRework"] = IsRework;
		dataRow["LineProduction"] = LineProduction;
		dataRow["WorkcenterTypeCode"] = WorkcenterTypeCode;
		dataRow["WorkCenterName"] = WorkCenterName;
		dataRow["IsPlan"] = IsPlan;
		dataRow["IsMeter"] = IsMeter;
		dataRow["IsEquipment"] = IsEquipment;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("ID");
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("WorkCenterNo");
		dataTable.Columns.Add("FlowID");
		dataTable.Columns.Add("SAPWorkCenter");
		dataTable.Columns.Add("SAPConfirmWorkCenter");
		dataTable.Columns.Add("Confirmation");
		dataTable.Columns.Add("RefWC01");
		dataTable.Columns.Add("Rank");
		dataTable.Columns.Add("Delivered");
		dataTable.Columns.Add("IsTaping");
		dataTable.Columns.Add("IsPacking");
		dataTable.Columns.Add("IOEqual");
		dataTable.Columns.Add("IsMeasure");
		dataTable.Columns.Add("MeasureDataBase");
		dataTable.Columns.Add("CalcNG");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("ChangeDate");
		dataTable.Columns.Add("ChangeUser");
		dataTable.Columns.Add("InCheck");
		dataTable.Columns.Add("InRefWorkCenter");
		dataTable.Columns.Add("IsRework");
		dataTable.Columns.Add("LineProduction");
		dataTable.Columns.Add("WorkcenterTypeCode");
		dataTable.Columns.Add("WorkCenterName");
		dataTable.Columns.Add("IsPlan");
		dataTable.Columns.Add("IsMeter");
		dataTable.Columns.Add("IsEquipment");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WORouting> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WORouting item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}

	public static void BindWoConfirm(List<WORouting> myRoutingList, List<WOConfirm> fromConfirmList)
	{
		foreach (WORouting myRouting in myRoutingList)
		{
		}
	}
}
