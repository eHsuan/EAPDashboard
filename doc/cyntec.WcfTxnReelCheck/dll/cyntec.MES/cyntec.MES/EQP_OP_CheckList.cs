using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_CheckList
{
	public string SysID = "";

	public int MachID = 0;

	public string Operation = "";

	public string OperationDesc = "";

	public bool IsEnable = true;

	public DateTime UpdateDate;

	public string UpdateUser = "";

	public DateTime CreateDate;

	public string CreateUser = "";

	public EQP_OP_Record eqpOPRecord;

	public bool IsComplete
	{
		get
		{
			if (eqpOPRecord == null)
			{
				return false;
			}
			return true;
		}
	}

	public bool IsException
	{
		get
		{
			if (ToErrorMsg().Contains("量測數據上傳成功"))
			{
				return false;
			}
			return ToErrorMsg() != "";
		}
	}

	public string ToErrorMsg()
	{
		if (eqpOPRecord == null)
		{
			return Operation + "," + OperationDesc + ",未完成";
		}
		if (!string.IsNullOrWhiteSpace(eqpOPRecord.LastExceptionMsg))
		{
			return Operation + ", " + OperationDesc + "," + eqpOPRecord.LastExceptionMsg;
		}
		return "";
	}

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["SysID"] = SysID;
		dataRow["MachID"] = MachID;
		dataRow["Operation"] = Operation;
		dataRow["OperationDesc"] = OperationDesc;
		dataRow["IsEnable"] = IsEnable;
		dataRow["UpdateDate"] = UpdateDate;
		dataRow["UpdateUser"] = UpdateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["CreateUser"] = CreateUser;
		dataRow["IsComplete"] = IsComplete;
		dataRow["IsException"] = IsException;
		dataRow["ToErrorMsg"] = ToErrorMsg();
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("SysID");
		dataTable.Columns.Add("MachID");
		dataTable.Columns.Add("Operation");
		dataTable.Columns.Add("OperationDesc");
		dataTable.Columns.Add("IsEnable");
		dataTable.Columns.Add("UpdateDate");
		dataTable.Columns.Add("UpdateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("IsComplete");
		dataTable.Columns.Add("IsException");
		dataTable.Columns.Add("ToErrorMsg");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<EQP_OP_CheckList> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (EQP_OP_CheckList item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
