using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.MES;

[Serializable]
public class MatGroupNoC
{
	public string MatGroupNo;

	public string MatGroupName;

	public string PlantNo;

	public int Yield;

	public string ProductGroupCode;

	public bool IsAutomobile;

	public bool EnableMachBarcode;

	public string Property1;

	public string Property2;

	public string Property3;

	public string Property4;

	public string Property5;

	public string Property6;

	public string Property7;

	public string Property8;

	public string Property9;

	public string Property10;

	public string Property11;

	public string Property12;

	public string Property13;

	public string Property14;

	public string Property15;

	public string Property16;

	public string Property17;

	public string Property18;

	public string Property19;

	public string Property20;

	public string Property21;

	public string Property22;

	public string Property23;

	public string Property24;

	public string Property25;

	public string Property26;

	public string Property27;

	public string Property28;

	public string Property29;

	public string Property30;

	public string Property31;

	public string Property32;

	public string Property33;

	public string Property34;

	public string Property35;

	public string Property36;

	public string Property37;

	public string Property38;

	public string Property39;

	public int SortID;

	public bool Enable;

	public string CreateUser;

	public DateTime CreateDate;

	public string UpdateUser;

	public DateTime UpdateDate;

	public DataTable ToDataTable(ref DataTable mainTable, string tableName)
	{
		if (mainTable == null)
		{
			mainTable = NewDataTable(tableName);
		}
		DataRow dataRow = mainTable.NewRow();
		mainTable.Rows.Add(dataRow);
		dataRow["MatGroupNo"] = MatGroupNo;
		dataRow["MatGroupName"] = MatGroupName;
		dataRow["PlantNo"] = PlantNo;
		dataRow["Yield"] = Yield;
		dataRow["ProductGroupCode"] = ProductGroupCode;
		dataRow["IsAutomobile"] = IsAutomobile;
		dataRow["EnableMachBarcode"] = EnableMachBarcode;
		dataRow["Property1"] = Property1;
		dataRow["Property2"] = Property2;
		dataRow["Property3"] = Property3;
		dataRow["Property4"] = Property4;
		dataRow["Property5"] = Property5;
		dataRow["Property6"] = Property6;
		dataRow["Property7"] = Property7;
		dataRow["Property8"] = Property8;
		dataRow["Property9"] = Property9;
		dataRow["Property10"] = Property10;
		dataRow["Property11"] = Property11;
		dataRow["Property12"] = Property12;
		dataRow["Property13"] = Property13;
		dataRow["Property14"] = Property14;
		dataRow["Property15"] = Property15;
		dataRow["Property16"] = Property16;
		dataRow["Property17"] = Property17;
		dataRow["Property18"] = Property18;
		dataRow["Property19"] = Property19;
		dataRow["Property20"] = Property20;
		dataRow["Property21"] = Property21;
		dataRow["Property22"] = Property22;
		dataRow["Property23"] = Property23;
		dataRow["Property24"] = Property24;
		dataRow["Property25"] = Property25;
		dataRow["Property26"] = Property26;
		dataRow["Property27"] = Property27;
		dataRow["Property28"] = Property28;
		dataRow["Property29"] = Property29;
		dataRow["Property30"] = Property30;
		dataRow["Property31"] = Property31;
		dataRow["Property32"] = Property32;
		dataRow["Property33"] = Property33;
		dataRow["Property34"] = Property34;
		dataRow["Property35"] = Property35;
		dataRow["Property36"] = Property36;
		dataRow["Property37"] = Property37;
		dataRow["Property38"] = Property38;
		dataRow["Property39"] = Property39;
		dataRow["SortID"] = SortID;
		dataRow["Enable"] = Enable;
		dataRow["CreateUser"] = CreateUser;
		dataRow["CreateDate"] = CreateDate;
		dataRow["UpdateUser"] = UpdateUser;
		dataRow["UpdateDate"] = UpdateDate;
		return mainTable;
	}

	public static DataTable NewDataTable(string tableName)
	{
		DataTable dataTable = null;
		dataTable = ((!string.IsNullOrWhiteSpace(tableName)) ? new DataTable(tableName) : new DataTable(typeof(Mach_Master).Name));
		dataTable.Columns.Add("MatGroupNo");
		dataTable.Columns.Add("MatGroupName");
		dataTable.Columns.Add("PlantNo");
		dataTable.Columns.Add("Yield");
		dataTable.Columns.Add("ProductGroupCode");
		dataTable.Columns.Add("IsAutomobile");
		dataTable.Columns.Add("EnableMachBarcode");
		dataTable.Columns.Add("Property1");
		dataTable.Columns.Add("Property2");
		dataTable.Columns.Add("Property3");
		dataTable.Columns.Add("Property4");
		dataTable.Columns.Add("Property5");
		dataTable.Columns.Add("Property6");
		dataTable.Columns.Add("Property7");
		dataTable.Columns.Add("Property8");
		dataTable.Columns.Add("Property9");
		dataTable.Columns.Add("Property10");
		dataTable.Columns.Add("Property11");
		dataTable.Columns.Add("Property12");
		dataTable.Columns.Add("Property13");
		dataTable.Columns.Add("Property14");
		dataTable.Columns.Add("Property15");
		dataTable.Columns.Add("Property16");
		dataTable.Columns.Add("Property17");
		dataTable.Columns.Add("Property18");
		dataTable.Columns.Add("Property19");
		dataTable.Columns.Add("Property20");
		dataTable.Columns.Add("Property21");
		dataTable.Columns.Add("Property22");
		dataTable.Columns.Add("Property23");
		dataTable.Columns.Add("Property24");
		dataTable.Columns.Add("Property25");
		dataTable.Columns.Add("Property26");
		dataTable.Columns.Add("Property27");
		dataTable.Columns.Add("Property28");
		dataTable.Columns.Add("Property29");
		dataTable.Columns.Add("Property29");
		dataTable.Columns.Add("Property30");
		dataTable.Columns.Add("Property31");
		dataTable.Columns.Add("Property32");
		dataTable.Columns.Add("Property33");
		dataTable.Columns.Add("Property34");
		dataTable.Columns.Add("Property35");
		dataTable.Columns.Add("Property36");
		dataTable.Columns.Add("Property37");
		dataTable.Columns.Add("Property38");
		dataTable.Columns.Add("Property39");
		dataTable.Columns.Add("SortID");
		dataTable.Columns.Add("Enable");
		dataTable.Columns.Add("CreateUser");
		dataTable.Columns.Add("CreateDate");
		dataTable.Columns.Add("UpdateUser");
		dataTable.Columns.Add("UpdateDate");
		return dataTable;
	}

	public static DataTable ToDataTable(IEnumerable<MatGroupNoC> itemList, string tableName)
	{
		DataTable mainTable = NewDataTable(tableName);
		foreach (MatGroupNoC item in itemList)
		{
			item.ToDataTable(ref mainTable, tableName);
		}
		return mainTable;
	}
}
