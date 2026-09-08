using System;
using System.Collections.Generic;
using System.Data;
using cyntec.Base;

namespace cyntec.MES;

[Serializable]
public class WorkOrder
{
	public string WONo = "";

	public string PlantNo = "";

	public string MatGroupNo = "";

	public string MatNo = "";

	public string OrderType = "";

	public string ProductStageCode;

	public string Priority = "";

	public string CustomerCode;

	public DateTime CustomerDeliveryTime;

	public string MasterWONo;

	public string RouteNo = "";

	public int TotalQty = 0;

	public string DeliverQty = "";

	public decimal ByProductQty;

	public int WIPQty = 0;

	public string LineCode = "";

	public string BatchNo = "";

	public int CurrentFlowID = 0;

	public string CurrentWorkCenter = "";

	public DateTime StartDate;

	public DateTime EndDate;

	public DateTime DeliverDate;

	public DateTime MatPostDate;

	public DateTime ProcessStartDate;

	public DateTime ProcessEndDate;

	public bool Blocked;

	public bool Deleted;

	public bool TECO;

	public bool Released;

	public bool Scraped;

	public bool Urgent;

	public string SplitCode;

	public bool NeedMerge;

	public bool WOStatusCode;

	public DateTime CloseDate;

	public string CloseCode;

	public string CloseReason;

	public string ReworkReason;

	public string Remark = "";

	public string SFC1010 = "";

	public string Remark2 = "";

	public string Remark3 = "";

	public string SortString = "";

	public DateTime LastConfirmDate;

	public string TraceCode;

	public string CreateUser = "";

	public DateTime CreateDate;

	public string ChangeUser = "";

	public DateTime ChangeDate;

	public DateTime SyncDate;

	public string ITNote = "";

	public string LabelMode = "";

	public void init(DataRow row)
	{
		WONo = row["WONo"].ToString();
		PlantNo = row["PlantNo"].ToString();
		MatGroupNo = row["MatGroupNo"].ToString();
		MatNo = row["MatNo"].ToString();
		OrderType = row["OrderType"].ToString();
		ProductStageCode = row["ProductStageCode"].ToString();
		Priority = row["Priority"].ToString();
		CustomerCode = row["CustomerCode"].ToString();
		CustomerDeliveryTime = cyntecDateTime.ToDateTime(row["CustomerDeliveryTime"]);
		MasterWONo = row["MasterWONo"].ToString();
		RouteNo = row["RouteNo"].ToString();
		TotalQty = Convert.ToInt32(row["TotalQty"]);
		DeliverQty = row["DeliverQty"].ToString();
		ByProductQty = Convert.ToDecimal(row["ByProductQty"].ToString());
		WIPQty = Convert.ToInt32(row["WIPQty"]);
		LineCode = row["LineCode"].ToString();
		BatchNo = row["BatchNo"].ToString();
		CurrentWorkCenter = row["CurrentWorkCenter"].ToString();
		CurrentFlowID = int.Parse(row["CurrentFlowID"].ToString());
		StartDate = cyntecDateTime.ToDateTime(row["StartDate"]);
		EndDate = cyntecDateTime.ToDateTime(row["EndDate"]);
		DeliverDate = cyntecDateTime.ToDateTime(row["DeliverDate"]);
		MatPostDate = cyntecDateTime.ToDateTime(row["MatPostDate"]);
		ProcessStartDate = cyntecDateTime.ToDateTime(row["ProcessStartDate"]);
		ProcessEndDate = cyntecDateTime.ToDateTime(row["ProcessEndDate"]);
		Blocked = cyntecByteConvert.ToBool(row["Blocked"]);
		Deleted = cyntecByteConvert.ToBool(row["Deleted"]);
		TECO = cyntecByteConvert.ToBool(row["TECO"]);
		Released = cyntecByteConvert.ToBool(row["Released"]);
		Scraped = cyntecByteConvert.ToBool(row["Scraped"]);
		Urgent = cyntecByteConvert.ToBool(row["Urgent"]);
		SplitCode = row["SplitCode"].ToString();
		NeedMerge = cyntecByteConvert.ToBool(row["NeedMerge"]);
		WOStatusCode = cyntecByteConvert.ToBool(row["WOStatusCode"]);
		CloseDate = cyntecDateTime.ToDateTime(row["CloseDate"]);
		CloseCode = row["CloseCode"].ToString();
		CloseReason = row["CloseReason"].ToString();
		ReworkReason = row["ReworkReason"].ToString();
		Remark = row["Remark"].ToString();
		SFC1010 = row["SFC1010"].ToString();
		Remark2 = row["Remark2"].ToString();
		Remark3 = row["Remark3"].ToString();
		SortString = row["SortString"].ToString();
		LastConfirmDate = cyntecDateTime.ToDateTime(row["LastConfirmDate"]);
		CreateUser = row["CreateUser"].ToString();
		CreateDate = cyntecDateTime.ToDateTime(row["CreateDate"]);
		ChangeUser = row["ChangeUser"].ToString();
		ChangeDate = cyntecDateTime.ToDateTime(row["ChangeDate"]);
		SyncDate = cyntecDateTime.ToDateTime(row["SyncDate"]);
		ITNote = row["ITNote"].ToString();
		LabelMode = row["LabelMode"].ToString();
	}

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		dataRow["WONo"] = WONo;
		dataRow["PlantNo"] = PlantNo;
		dataRow["MatGroupNo"] = MatGroupNo;
		dataRow["MatNo"] = MatNo;
		dataRow["OrderType"] = OrderType;
		dataRow["ProductStageCode"] = ProductStageCode;
		dataRow["Priority"] = Priority;
		dataRow["CustomerCode"] = CustomerCode;
		dataRow["CustomerDeliveryTime"] = CustomerDeliveryTime;
		dataRow["MasterWONo"] = MasterWONo;
		dataRow["RouteNo"] = RouteNo;
		dataRow["TotalQty"] = TotalQty;
		dataRow["DeliverQty"] = DeliverQty;
		dataRow["ByProductQty"] = ByProductQty;
		dataRow["WIPQty"] = WIPQty;
		dataRow["LineCode"] = LineCode;
		dataRow["BatchNo"] = BatchNo;
		dataRow["CurrentFlowID"] = CurrentFlowID;
		dataRow["CurrentWorkCenter"] = CurrentWorkCenter;
		dataRow["StartDate"] = StartDate;
		dataRow["EndDate"] = EndDate;
		dataRow["DeliverDate"] = DeliverDate;
		dataRow["MatPostDate"] = MatPostDate;
		dataRow["ProcessStartDate"] = ProcessStartDate;
		dataRow["ProcessEndDate"] = ProcessEndDate;
		dataRow["Blocked"] = Blocked;
		dataRow["Deleted"] = Deleted;
		dataRow["TECO"] = TECO;
		dataRow["Released"] = Released;
		dataRow["Scraped"] = Scraped;
		dataRow["Urgent"] = Urgent;
		dataRow["SplitCode"] = SplitCode;
		dataRow["NeedMerge"] = NeedMerge;
		dataRow["WOStatusCode"] = WOStatusCode;
		dataRow["CloseDate"] = CloseDate;
		dataRow["CloseCode"] = CloseCode;
		dataRow["CloseReason"] = CloseReason;
		dataRow["ReworkReason"] = ReworkReason;
		dataRow["Remark"] = Remark;
		dataRow["SFC1010"] = SFC1010;
		dataRow["Remark2"] = Remark2;
		dataRow["Remark3"] = Remark3;
		dataRow["SortString"] = SortString;
		dataRow["LastConfirmDate"] = LastConfirmDate;
		dataRow["TraceCode"] = TraceCode;
		dataRow["CreateUser"] = CreateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["ChangeUser"] = ChangeUser;
		dataRow["ChangeDate"] = ChangeDate;
		dataRow["SyncDate"] = SyncDate;
		dataRow["ITNote"] = ITNote;
		dataRow["LabelMode"] = LabelMode;
		mainTable.Rows.Add(dataRow);
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("WONo");
		dataTable.Columns.Add("PlantNo");
		dataTable.Columns.Add("MatGroupNo");
		dataTable.Columns.Add("MatNo");
		dataTable.Columns.Add("OrderType");
		dataTable.Columns.Add("ProductStageCode");
		dataTable.Columns.Add("Priority");
		dataTable.Columns.Add("CustomerCode");
		dataTable.Columns.Add("CustomerDeliveryTime");
		dataTable.Columns.Add("MasterWONo");
		dataTable.Columns.Add("RouteNo");
		dataTable.Columns.Add("TotalQty");
		dataTable.Columns.Add("DeliverQty");
		dataTable.Columns.Add("ByProductQty");
		dataTable.Columns.Add("WIPQty");
		dataTable.Columns.Add("LineCode");
		dataTable.Columns.Add("BatchNo");
		dataTable.Columns.Add("CurrentFlowID");
		dataTable.Columns.Add("CurrentWorkCenter");
		dataTable.Columns.Add("StartDate");
		dataTable.Columns.Add("EndDate");
		dataTable.Columns.Add("DeliverDate");
		dataTable.Columns.Add("MatPostDate");
		dataTable.Columns.Add("ProcessStartDate");
		dataTable.Columns.Add("ProcessEndDate");
		dataTable.Columns.Add("Blocked");
		dataTable.Columns.Add("Deleted");
		dataTable.Columns.Add("TECO");
		dataTable.Columns.Add("Released");
		dataTable.Columns.Add("Scraped");
		dataTable.Columns.Add("Urgent");
		dataTable.Columns.Add("SplitCode");
		dataTable.Columns.Add("NeedMerge");
		dataTable.Columns.Add("WOStatusCode");
		dataTable.Columns.Add("CloseDate");
		dataTable.Columns.Add("CloseCode");
		dataTable.Columns.Add("CloseReason");
		dataTable.Columns.Add("ReworkReason");
		dataTable.Columns.Add("Remark");
		dataTable.Columns.Add("SFC1010");
		dataTable.Columns.Add("Remark2");
		dataTable.Columns.Add("Remark3");
		dataTable.Columns.Add("SortString");
		dataTable.Columns.Add("LastConfirmDate");
		dataTable.Columns.Add("TraceCode");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("ChangeUser");
		dataTable.Columns.Add("ChangeDate");
		dataTable.Columns.Add("SyncDate");
		dataTable.Columns.Add("ITNote");
		dataTable.Columns.Add("LabelMode");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<WorkOrder> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (WorkOrder item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
