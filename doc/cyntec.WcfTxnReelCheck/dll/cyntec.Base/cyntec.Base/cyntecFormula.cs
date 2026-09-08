using System;
using System.Collections.Generic;

namespace cyntec.Base;

public class cyntecFormula
{
	public string Name = "";

	public string strValue = "";

	public Type valueType;

	public int Value_int
	{
		get
		{
			return int.Parse(strValue);
		}
		set
		{
			strValue = value.ToString();
		}
	}

	public decimal Value_decimal
	{
		get
		{
			return decimal.Parse(strValue);
		}
		set
		{
			strValue = value.ToString();
		}
	}

	public string Value_string
	{
		get
		{
			return strValue;
		}
		set
		{
			strValue = value;
		}
	}

	public cyntecFormula f_Add(cyntecFormula d2)
	{
		if (valueType != d2.valueType)
		{
			throw new Exception("資料類型不一致無法執行f_Add(cyntecFormula)");
		}
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		if (valueType == typeof(int))
		{
			cyntecFormula2.strValue = (Value_int + d2.Value_int).ToString();
		}
		if (valueType == typeof(decimal))
		{
			cyntecFormula2.strValue = (Value_decimal + d2.Value_decimal).ToString();
		}
		return cyntecFormula2;
	}

	public cyntecFormula f_Subtract(cyntecFormula d2)
	{
		if (valueType != d2.valueType)
		{
			throw new Exception("資料類型不一致無法執行f_Add(cyntecFormula)");
		}
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		if (valueType == typeof(int))
		{
			cyntecFormula2.strValue = (Value_int - d2.Value_int).ToString();
		}
		if (valueType == typeof(decimal))
		{
			cyntecFormula2.strValue = (Value_decimal - d2.Value_decimal).ToString();
		}
		return cyntecFormula2;
	}

	public cyntecFormula f_Multiply(cyntecFormula d2)
	{
		if (valueType != d2.valueType)
		{
			throw new Exception("資料類型不一致無法執行f_Add(cyntecFormula)");
		}
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		if (valueType == typeof(int))
		{
			cyntecFormula2.strValue = (Value_int * d2.Value_int).ToString();
		}
		if (valueType == typeof(decimal))
		{
			cyntecFormula2.strValue = (Value_decimal * d2.Value_decimal).ToString();
		}
		return cyntecFormula2;
	}

	public cyntecFormula f_Divide(cyntecFormula d2)
	{
		if (valueType != d2.valueType)
		{
			throw new Exception("資料類型不一致無法執行f_Add(cyntecFormula)");
		}
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		if (valueType == typeof(int))
		{
			cyntecFormula2.strValue = (Value_int / d2.Value_int).ToString();
		}
		if (valueType == typeof(decimal))
		{
			cyntecFormula2.strValue = (Value_decimal / d2.Value_decimal).ToString();
		}
		return cyntecFormula2;
	}

	public cyntecFormula f_Substring(int strartindex)
	{
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		cyntecFormula2.strValue = strValue.Substring(strartindex);
		return cyntecFormula2;
	}

	public cyntecFormula f_Substring(int strartindex, int length)
	{
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		cyntecFormula2.strValue = strValue.Substring(strartindex, length);
		return cyntecFormula2;
	}

	public cyntecFormula f_StringFormat(cyntecFormula[] paramList)
	{
		cyntecFormula cyntecFormula2 = new cyntecFormula();
		cyntecFormula2.valueType = valueType;
		List<object> list = new List<object>();
		foreach (cyntecFormula cyntecFormula3 in paramList)
		{
			list.Add(cyntecFormula3.strValue);
		}
		cyntecFormula2.strValue = string.Format(strValue, list.ToArray());
		return cyntecFormula2;
	}
}
