using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSQLGen
{
	public string tableName = "";

	public string sql = "";

	public string where = "";

	public List<OleDbParameter> whereParamLsit = new List<OleDbParameter>();

	public Queue<cyntecSQLItem> itemList = new Queue<cyntecSQLItem>();

	public List<string> whereConditionList = new List<string>();

	public List<OleDbParameter> m_TempParamList = new List<OleDbParameter>();

	public List<OleDbParameter> oleDBParamList = new List<OleDbParameter>();

	~cyntecSQLGen()
	{
		itemList.Clear();
	}

	public void AddSQL(string data)
	{
		AddSQL(data, isNewLine: true);
	}

	public void AddSQL(string data, bool isNewLine)
	{
		if (isNewLine)
		{
			sql = sql + data + Environment.NewLine;
		}
		else
		{
			sql += data;
		}
	}

	public void AddCondition(string fieldName, string op, object value)
	{
		AddCondition(new cyntecSQLItem(fieldName, op, value));
	}

	public void AddCondition(string fieldName, string op, object value, ESQLValueType type)
	{
		AddCondition(new cyntecSQLItem(fieldName, op, value, type));
	}

	public void AddCondition(cyntecSQLItem condition)
	{
		whereConditionList.Add(condition.ToString() + Environment.NewLine);
	}

	public static string NewCondition(string fieldName, string op, object value)
	{
		if (value == null)
		{
			return fieldName + " " + op + " null";
		}
		return fieldName + " " + op + " '" + value?.ToString() + "'";
	}

	public static string NewCondition(string fieldName, string op, object value, ESQLValueType type)
	{
		if (value == null)
		{
			return fieldName + " " + op + " null";
		}
		if (type == ESQLValueType.NoQuote)
		{
			return fieldName + " " + op + " " + value;
		}
		return fieldName + " " + op + " '" + value?.ToString() + "'";
	}

	public string GetSQL()
	{
		if (whereConditionList == null)
		{
			return sql;
		}
		if (whereConditionList.Count == 0)
		{
			return sql;
		}
		return sql + " where " + GetCondition();
	}

	public string GetInList()
	{
		sql = "(";
		foreach (cyntecSQLItem item in itemList)
		{
			sql += item.name;
		}
		if (sql.Length > 0)
		{
			sql = sql.PadLeft(sql.Length - 1);
		}
		sql += ")";
		return sql;
	}

	public void AddItem(string name, object value)
	{
		AddItem(name, value, ESQLValueType.Normal);
	}

	public void Add2(string name, object value)
	{
		AddItem(name, AddParam(name, value));
		m_TempParamList.Add(new OleDbParameter(name, value));
	}

	public DataTable ToDataTable(ref DataTable dt)
	{
		if (dt == null)
		{
			dt = new DataTable(tableName);
			foreach (OleDbParameter tempParam in m_TempParamList)
			{
				if (tempParam.Value == null)
				{
					dt.Columns.Add(tempParam.ParameterName);
				}
				else
				{
					dt.Columns.Add(tempParam.ParameterName, tempParam.Value.GetType());
				}
			}
		}
		DataRow dataRow = dt.NewRow();
		dt.Rows.Add(dataRow);
		foreach (OleDbParameter tempParam2 in m_TempParamList)
		{
			dataRow[tempParam2.ParameterName] = tempParam2.Value;
		}
		return dt;
	}

	public void AddItem(string name, object value, ESQLValueType type)
	{
		cyntecSQLItem cyntecSQLItem2 = new cyntecSQLItem();
		cyntecSQLItem2.name = name;
		if (type == ESQLValueType.MS_DateTime && value.GetType().Name != typeof(DateTime).Name)
		{
			throw new Exception("cynTecSQLGen, 欄位 " + name + "必須為DateTime");
		}
		if (type == ESQLValueType.FLAG_YesNo && value.GetType().Name != typeof(bool).Name && value.GetType().Name != typeof(bool).Name)
		{
			throw new Exception("cynTecSQLGen, 欄位 " + name + "必須為bool");
		}
		if (value == null)
		{
			cyntecSQLItem2.value = null;
		}
		else
		{
			switch (type)
			{
			case ESQLValueType.MS_DateTime:
				cyntecSQLItem2.value = cyntecDateTime.ToMSSQL((DateTime)value);
				break;
			default:
				cyntecSQLItem2.value = value.ToString().Replace("'", "''");
				break;
			case ESQLValueType.NoQuote:
				cyntecSQLItem2.value = value.ToString();
				break;
			}
		}
		cyntecSQLItem2.valueType = type;
		itemList.Enqueue(cyntecSQLItem2);
	}

	public string ToUpdate()
	{
		sql = "update " + tableName + " set ";
		foreach (cyntecSQLItem item in itemList)
		{
			if (oleDBParamList.Count == 0)
			{
				sql = sql + item.ToString("=") + ",";
				continue;
			}
			sql = sql + item.name + "=" + item.value.ToString() + ",";
		}
		sql = sql.Substring(0, sql.Length - 1);
		if (where.Length > 0)
		{
			sql = sql + " where " + where;
		}
		else if (whereConditionList.Count > 0)
		{
			sql = sql + " where " + GetCondition();
		}
		else
		{
			sql += " where --強迫一定要加條件避免災難性的誤動作";
		}
		return sql;
	}

	public List<object> ToUpdate(bool IsBySQLParam)
	{
		oleDBParamList = new List<OleDbParameter>();
		sql = "update " + tableName + " set ";
		foreach (cyntecSQLItem item in itemList)
		{
			sql = sql + item.name + " = " + AddParam(item.name, item.value);
		}
		sql = sql.Substring(0, sql.Length - 1);
		if (where.Length > 0)
		{
			sql = sql + " where " + where;
		}
		else if (whereConditionList.Count > 0)
		{
			sql = sql + " where " + GetCondition();
		}
		else
		{
			sql += " where --強迫一定要加條件避免災難性的誤動作";
		}
		List<OleDbParameter> list = new List<OleDbParameter>();
		list.AddRange(oleDBParamList);
		list.AddRange(whereParamLsit);
		return new List<object> { sql, list };
	}

	public string GetUpdateSQLWithTS(string timeStamp)
	{
		ToUpdate();
		if (where.ToString().Length != 0)
		{
			sql = sql + " and modify_date='" + timeStamp + "'";
		}
		else
		{
			sql = sql + " where  modify_date='" + timeStamp + "'";
		}
		return sql;
	}

	public string ToInsertScript()
	{
		return "";
	}

	public string ToUpdateScript()
	{
		return "";
	}

	public OleDbParameter[] ToParameterList()
	{
		return null;
	}

	public string ToInsert()
	{
		string text = " (";
		string text2 = " VALUES (";
		sql = "";
		foreach (cyntecSQLItem item in itemList)
		{
			text = text + item.name + ",";
			text2 = ((oleDBParamList.Count != 0) ? (text2 + item.value.ToString() + ",") : (text2 + item.GetValue() + ","));
		}
		text = text.Substring(0, text.Length - 1) + ")";
		text2 = text2.Substring(0, text2.Length - 1) + ")";
		sql = "INSERT INTO " + tableName + text + text2;
		return sql;
	}

	public List<object> ToInsert(bool IsBySQLParam)
	{
		oleDBParamList = new List<OleDbParameter>();
		string text = " (";
		string text2 = " VALUES (";
		sql = "";
		foreach (cyntecSQLItem item in itemList)
		{
			text = text + item.name + ",";
			text2 = text2 + AddParam(item.name, item.GetValue()) + ",";
		}
		text = text.Substring(0, text.Length - 1) + ")";
		text2 = text2.Substring(0, text2.Length - 1) + ")";
		sql = "INSERT INTO " + tableName + text + text2;
		return new List<object> { sql, oleDBParamList };
	}

	public string GetInserSQLBySelect(string selectTableName)
	{
		string text = " (";
		string text2 = " select";
		foreach (cyntecSQLItem item in itemList)
		{
			text = text + item.name + ",";
			text2 = ((item.valueType != ESQLValueType.Quote) ? (text2 + text2 + item.value) : (text2 + "'" + item.value?.ToString() + "'"));
		}
		if (text.Length > 0)
		{
			text = text.PadLeft(text.Length) + ")";
		}
		if (text2.Length > 0)
		{
			text2 = text2.PadLeft(text2.Length) + " FROM " + selectTableName;
		}
		sql = " INSERT INTO " + tableName + text + text2;
		return sql;
	}

	public void Clear()
	{
		itemList = new Queue<cyntecSQLItem>();
		tableName = "";
		sql = "";
		where = "";
	}

	public string GetCondition()
	{
		string text = "";
		foreach (string whereCondition in whereConditionList)
		{
			text = text + whereCondition + " and ";
		}
		if (text.Length > 4)
		{
			text = text.Substring(0, text.Length - 4);
		}
		return text;
	}

	public string AddParam(string name, object value)
	{
		string text = name + "_" + (oleDBParamList.Count + 1);
		if (!name.StartsWith("@"))
		{
			text = "@" + text;
		}
		oleDBParamList.Add(new OleDbParameter(text, value));
		return text;
	}

	public string AddParam(string name, object value, OleDbType dataType)
	{
		string text = name + "_" + (oleDBParamList.Count + 1);
		if (!name.StartsWith("@"))
		{
			text = "@" + text;
		}
		OleDbParameter oleDbParameter = new OleDbParameter(text, dataType);
		oleDbParameter.Value = value;
		oleDBParamList.Add(oleDbParameter);
		return text;
	}

	public string AddWhereParam(string name, object value)
	{
		string text = name + "_" + whereParamLsit.Count + 1;
		if (!name.StartsWith("@"))
		{
			text = "@" + text;
		}
		whereParamLsit.Add(new OleDbParameter(text, value));
		return text;
	}
}
