using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.Tools;

public class cyntecDataTableGen
{
	public string TableName = "NeedToModify";

	private Dictionary<string, DataColumn> columnDictionary = new Dictionary<string, DataColumn>();

	private List<string> PkStringList = new List<string>();

	private List<Dictionary<string, object>> valueList = new List<Dictionary<string, object>>();

	public void AddRow()
	{
		valueList.Add(new Dictionary<string, object>());
	}

	public void AddRow(Dictionary<string, object> row)
	{
		valueList.Add(row);
	}

	public void AddColumn(string columnID, Type dataType)
	{
		string text = columnID.ToUpper();
		if (!columnDictionary.ContainsKey(text))
		{
			columnDictionary.Add(text, new DataColumn(text, dataType));
		}
	}

	public void AddPK(string columnID)
	{
		PkStringList.Add(columnID);
	}

	public void AddItem(string columnID, object value, Type dataType)
	{
		string text = "";
		text = ((!columnID.EndsWith("!")) ? columnID.ToUpper() : columnID.Substring(0, columnID.Length - 1));
		if (!columnDictionary.ContainsKey(text))
		{
			columnDictionary.Add(text, new DataColumn(text, dataType));
		}
		if (valueList.Count == 0)
		{
			valueList.Add(new Dictionary<string, object>());
		}
		int index = valueList.Count - 1;
		if (valueList[index].ContainsKey(text))
		{
			valueList[index][text] = value;
		}
		else
		{
			valueList[index].Add(text, value);
		}
	}

	public DataTable ToDataTable()
	{
		DataTable dataTable = new DataTable();
		dataTable.TableName = TableName;
		List<DataColumn> list = new List<DataColumn>();
		foreach (string key in columnDictionary.Keys)
		{
			dataTable.Columns.Add(new DataColumn(columnDictionary[key].ColumnName, columnDictionary[key].DataType));
		}
		foreach (string pkString in PkStringList)
		{
			list.Add(dataTable.Columns[pkString]);
		}
		dataTable.PrimaryKey = list.ToArray();
		foreach (Dictionary<string, object> value in valueList)
		{
			List<object> list2 = new List<object>();
			foreach (string key2 in columnDictionary.Keys)
			{
				list2.Add(value[key2]);
			}
			dataTable.Rows.Add(list2.ToArray());
		}
		return dataTable;
	}
}
