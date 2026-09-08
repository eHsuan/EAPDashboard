using System;
using System.Collections.Generic;
using System.Data;

namespace cyntec.Tools;

public class cyntecCsvFile
{
	public static DataTable ToDataTable(string str, string tableName)
	{
		DataTable dataTable = new DataTable(tableName);
		dataTable.Columns.Add("KEY");
		dataTable.Columns.Add("VALUE");
		string[] array = str.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split(',');
			string text2 = "";
			string value = "";
			if (array3.Length != 0)
			{
				text2 = array3[0];
			}
			if (array3.Length > 1)
			{
				value = array3[1];
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = text2.ToUpper();
				dataRow[1] = value;
				dataTable.Rows.Add(dataRow);
			}
		}
		return dataTable;
	}

	public static DataTable ToDataTable(string str, string tableName, List<string> ColumnNameList)
	{
		DataTable dataTable = new DataTable(tableName);
		foreach (string ColumnName in ColumnNameList)
		{
			dataTable.Columns.Add(ColumnName);
		}
		string[] array = str.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				string[] array3 = text.Split(',');
				if (array3.Length != dataTable.Columns.Count)
				{
					throw new Exception("cyntecCsvFile.ToDataTable(), CSV檔案的欄位數與預期的不一樣");
				}
				DataRow dataRow = dataTable.NewRow();
				for (int j = 0; j < array3.Length; j++)
				{
					dataRow[j] = array3[j];
				}
				dataTable.Rows.Add(dataRow);
			}
		}
		return dataTable;
	}
}
