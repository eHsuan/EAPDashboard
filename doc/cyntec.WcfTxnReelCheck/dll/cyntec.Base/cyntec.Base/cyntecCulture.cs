using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace cyntec.Base;

public class cyntecCulture
{
	public DataTable dt;

	public DataTable dt_object;

	public DataSet ds;

	public string dllName = "";

	public string FormName = "";

	public static bool IsDebug = false;

	public static string Language = "zh-TW";

	public string strLanguage = "";

	public const string en_US = "en-US";

	public const string zh_TW = "zh-TW";

	public const string MSG_INFO = "MSG_INFO";

	public const string MSG_WORING = "MSG_WORING";

	public const string MSG_ERROR = "MSG_ERROR";

	public const string MSG_TEXT = "MSG_TEXT";

	public void AddRow(string TagID, string Culture, string Text)
	{
		AddRow(TagID, Culture, Text, "MSG_TEXT");
	}

	public void AddRow(string TagID, string Culture, string Text, string MsgType)
	{
		if (dt == null)
		{
			dt = new DataTable();
			dt.Columns.Add("TagID");
			dt.Columns.Add("Culture");
			dt.Columns.Add("Text");
			dt.Columns.Add("MsgType");
		}
		DataRow dataRow = dt.NewRow();
		dt.Rows.Add(dataRow);
		dataRow["TagID"] = TagID;
		dataRow["Culture"] = Culture;
		dataRow["Text"] = Text;
		dataRow["MsgType"] = MsgType.ToString().ToUpper();
	}

	public void AddRow(object obj, string Culture, string Text)
	{
		if (dt_object == null)
		{
			dt_object = new DataTable();
			dt_object.Columns.Add("obj", typeof(object));
			dt_object.Columns.Add("Culture");
			dt_object.Columns.Add("Text");
		}
		DataRow dataRow = dt_object.NewRow();
		dt_object.Rows.Add(dataRow);
		dataRow["obj"] = obj;
		dataRow["Culture"] = Culture;
		dataRow["Text"] = Text;
	}

	public void AddRowTable(string TableName, string TagID, string Culture, string Text)
	{
		DataTable dataTable = null;
		if (ds == null)
		{
			ds = new DataSet();
		}
		if (ds.Tables.Contains(TableName))
		{
			dataTable = ds.Tables[TableName];
		}
		if (dataTable == null)
		{
			dataTable = new DataTable(TableName);
			dataTable.Columns.Add("TagID");
			dataTable.Columns.Add("Culture");
			dataTable.Columns.Add("Text");
			dataTable.Columns.Add("MsgType");
			ds.Tables.Add(dataTable);
		}
		DataRow dataRow = dataTable.NewRow();
		dataTable.Rows.Add(dataRow);
		dataRow["TagID"] = TagID;
		dataRow["Culture"] = Culture;
		dataRow["Text"] = Text;
		dataRow["MsgType"] = "MSG_TEXT";
	}

	public void Display()
	{
		if (string.IsNullOrWhiteSpace(strLanguage))
		{
			strLanguage = Language;
		}
		if (dt_object == null)
		{
			throw new Exception("cyntecCulture, dt_object is null.");
		}
		foreach (DataRow row in dt_object.Rows)
		{
			object obj = row["obj"];
			if (obj != null && !(row["Culture"].ToString() != strLanguage))
			{
				if (obj is Control)
				{
					(obj as Control).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(Label))
				{
					(obj as Label).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(Button))
				{
					(obj as Button).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(ToolStripMenuItem))
				{
					(obj as ToolStripMenuItem).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(ToolStripStatusLabel))
				{
					(obj as ToolStripStatusLabel).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(CheckBox))
				{
					(obj as CheckBox).Text = row["Text"].ToString();
				}
				else if (obj is Form)
				{
					(obj as Form).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(RadioButton))
				{
					(obj as RadioButton).Text = row["Text"].ToString();
				}
				else if (obj.GetType() == typeof(TabPage))
				{
					(obj as TabPage).Text = row["Text"].ToString();
				}
				else
				{
					string name = obj.GetType().Name;
				}
			}
		}
	}

	public void DisplayGridView(DataGridView gv, string tableName)
	{
		if (string.IsNullOrWhiteSpace(strLanguage))
		{
			strLanguage = Language;
		}
		if (ds == null)
		{
			throw new Exception("cyntecCulture, ds is null.");
		}
		if (!ds.Tables.Contains(tableName))
		{
			throw new Exception("cyntecCulture, Table " + tableName + " is not exist.");
		}
		DataTable dataTable = ds.Tables[tableName];
		foreach (DataGridViewColumn column in gv.Columns)
		{
			if (column.Tag != null)
			{
				DataRow[] array = dataTable.Select("TagID='" + column.Tag?.ToString() + "' and Culture='" + strLanguage + "'");
				if (array.Length != 0)
				{
					column.HeaderText = array[0]["Text"].ToString();
				}
			}
		}
	}

	public string ToMSg(string TagID, params object[] items)
	{
		if (string.IsNullOrWhiteSpace(strLanguage))
		{
			strLanguage = Language;
		}
		if (dt == null)
		{
			throw new Exception("dt cannot be empty,cyntecCulture");
		}
		DataRow[] array = dt.Select("TagID='" + TagID + "' and Culture='" + strLanguage + "'");
		if (array.Length != 0)
		{
			if (items == null || items.Length == 0)
			{
				return array[0]["Text"].ToString();
			}
			return string.Format(array[0]["Text"].ToString(), items);
		}
		return TagID;
	}

	public Exception ToException(string TagID, params object[] items)
	{
		return new Exception(ToMSg(TagID, items));
	}

	public string ToType(string TagID)
	{
		if (string.IsNullOrWhiteSpace(strLanguage))
		{
			strLanguage = Language;
		}
		if (dt == null)
		{
			throw new Exception("dt cannot be empty,cyntecCulture");
		}
		DataRow[] array = dt.Select("TagID='" + TagID + "' and Culture='" + strLanguage + "'");
		if (array.Length != 0)
		{
			return array[0]["MsgType"].ToString();
		}
		return TagID;
	}

	private void TableToFile(string strPath, DataTable myTable, string FileName, string FormName)
	{
		if (IsDebug)
		{
			string tableName = myTable.TableName;
			myTable.TableName = FormName;
			string path = Path.Combine(strPath, FileName);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, myTable);
			byte[] array = memoryStream.ToArray();
			FileStream fileStream = File.Create(path);
			fileStream.Write(array, 0, array.Length);
			fileStream.Close();
			memoryStream.Close();
			myTable.TableName = tableName;
		}
	}

	public void dtToFile(string strPath, string FileName, string FormName)
	{
		TableToFile(strPath, dt, FileName, FormName);
	}

	public void dsToFile(string strPath, string FileName, string FormName)
	{
		if (!IsDebug)
		{
			return;
		}
		string[] array = FileName.Split(new string[1] { "." }, StringSplitOptions.None);
		string text = "";
		string text2 = "";
		if (array.Length > 1)
		{
			text2 = "." + array[array.Length - 1];
			text = FileName.Substring(0, FileName.Length - text2.Length);
		}
		else
		{
			text = FileName;
			text2 = "";
		}
		foreach (DataTable table in ds.Tables)
		{
			TableToFile(strPath, table, text + "." + table.TableName + text2, FormName + "." + table.TableName);
		}
	}

	public void dt_bject_ToFile(string strPath, string FileName, string FormName)
	{
		if (!IsDebug)
		{
			return;
		}
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("TagID");
		dataTable.Columns.Add("Culture");
		dataTable.Columns.Add("Text");
		dataTable.Columns.Add("MsgType");
		foreach (DataRow row in dt_object.Rows)
		{
			object obj = row["obj"];
			if (obj != null)
			{
				string text = "";
				text = ((obj is Control) ? (obj as Control).Name : ((obj.GetType() == typeof(Label)) ? (obj as Label).Name : ((obj.GetType() == typeof(Button)) ? (obj as Button).Name : ((obj.GetType() == typeof(ToolStripMenuItem)) ? (obj as ToolStripMenuItem).Name : ((obj.GetType() == typeof(ToolStripStatusLabel)) ? (obj as ToolStripStatusLabel).Name : ((obj.GetType() == typeof(CheckBox)) ? (obj as CheckBox).Name : ((obj is Form) ? (obj as Form).Name : ((obj.GetType() == typeof(RadioButton)) ? (obj as RadioButton).Name : ((!(obj.GetType() == typeof(TabPage))) ? obj.GetType().Name : (obj as TabPage).Name)))))))));
				DataRow dataRow2 = dataTable.NewRow();
				dataTable.Rows.Add(dataRow2);
				dataRow2["TagID"] = text;
				dataRow2["Culture"] = row["Culture"];
				dataRow2["Text"] = row["Text"];
			}
		}
		TableToFile(strPath, dataTable, FileName, FormName);
	}
}
