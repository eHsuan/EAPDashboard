using System;
using System.Data.OleDb;
using cyntec.Base;

namespace cyntec.Tools;

public class cyntecSQLItem
{
	public string name = "";

	public object value = "";

	public string sql = "";

	public ESQLValueType valueType = ESQLValueType.Normal;

	public ESQLConditionType conditionType = ESQLConditionType.AND;

	private string _op = "=";

	public string op
	{
		get
		{
			return _op;
		}
		set
		{
			switch (value)
			{
			}
			_op = value;
		}
	}

	public string ToCondition()
	{
		if (op == "" && name == "")
		{
			return value?.ToString() + " " + conditionType;
		}
		if (valueType == ESQLValueType.NoQuote)
		{
			return name + " " + op + " " + value?.ToString() + " " + conditionType;
		}
		return name + " " + op + "'" + value?.ToString() + "' " + conditionType.ToString() + " ";
	}

	public static string ToCondition(string name, string op, object value)
	{
		return ToCondition(name, op, value, ESQLValueType.Quote);
	}

	public static string ToCondition(string name, string op, object value, ESQLValueType type)
	{
		switch (op)
		{
		default:
			if (type == ESQLValueType.NoQuote)
			{
				return name + " " + op + " " + value;
			}
			return name + " " + op + " '" + value?.ToString() + "'";
		}
	}

	public cyntecSQLItem()
	{
	}

	public cyntecSQLItem(string columnName, object columnValue)
	{
		name = columnName;
		value = columnValue;
	}

	public cyntecSQLItem(string columnName, string myOP, object columnValue)
	{
		name = columnName;
		value = columnValue;
		op = myOP;
	}

	public cyntecSQLItem(object columnValue)
	{
		name = "";
		op = "";
		value = columnValue;
	}

	public cyntecSQLItem(string columnName, string myOP, object columnValue, ESQLValueType type)
	{
		name = columnName;
		value = columnValue;
		op = myOP;
		valueType = type;
	}

	public string GetValue()
	{
		if (op == "" && name == "")
		{
			return value.ToString();
		}
		if (value == null)
		{
			return "null";
		}
		if (valueType == ESQLValueType.UniqueID && value.ToString() == "")
		{
			return "null";
		}
		if (valueType == ESQLValueType.NoQuote)
		{
			return value.ToString();
		}
		if (valueType == ESQLValueType.MS_DateTime)
		{
			if (Convert.ToDateTime(value) >= DateTime.MaxValue.AddDays(-1.0) || Convert.ToDateTime(value) <= DateTime.MinValue.AddDays(1.0))
			{
				return "null";
			}
			return "'" + cyntecDateTime.ToMSSQL(Convert.ToDateTime(value)) + "'";
		}
		if (valueType == ESQLValueType.FLAG_YesNo)
		{
			return "'" + cyntecByteConvert.ToFlag(Convert.ToBoolean(value)) + "'";
		}
		if (valueType == ESQLValueType.NVarchar)
		{
			return "N'" + value?.ToString() + "'";
		}
		return "'" + value?.ToString() + "'";
	}

	public override string ToString()
	{
		return name + " " + op + GetValue();
	}

	public OleDbParameter ToOleDbParameter()
	{
		return new OleDbParameter("@" + name, GetValue());
	}

	public string ToString(string myOP)
	{
		return name + " " + myOP + GetValue();
	}
}
