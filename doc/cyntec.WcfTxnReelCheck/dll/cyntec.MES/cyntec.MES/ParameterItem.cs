using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using cyntec.Base;

namespace cyntec.MES;

[Serializable]
public class ParameterItem
{
	public static cyntecCulture m_Culture = ToCulture("ParameterItem");

	private const string MSG01 = "MSG01";

	private const string MSG02 = "MSG02";

	private const string MSG03 = "MSG03";

	private const string MSG04 = "MSG04";

	public string ID = "";

	public string ParamCode;

	public string ParamName;

	public string WorkCenterNo;

	public string RefFieldCode;

	public string MatNo;

	public string ParamValue;

	public string Unit;

	public string Enable;

	public string UpperLimit;

	public string LowerLimit;

	public string UPLimitMode;

	public string LOLimitMode;

	public string SourceType;

	public string StatusCode;

	public string Remark;

	public bool IsWoParam;

	public bool IsSystem;

	public bool IsInitError = false;

	public string ErrorMsg;

	public decimal UnitValue
	{
		get
		{
			Unit = Unit.Trim();
			if ("Ω" == Unit.Trim())
			{
				return 1m;
			}
			if ("mΩ" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -3.0);
			}
			if ("mV" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -3.0);
			}
			if ("mA" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -3.0);
			}
			if ("MΩ" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, 6.0);
			}
			if ("KHz".ToUpper() == Unit.Trim().ToUpper())
			{
				return (decimal)Math.Pow(10.0, 3.0);
			}
			if ("μH" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -6.0);
			}
			if ("uH" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -6.0);
			}
			if ("nH" == Unit)
			{
				return (decimal)Math.Pow(10.0, -9.0);
			}
			if ("%" == Unit.Trim())
			{
				return (decimal)Math.Pow(10.0, -2.0);
			}
			if ("mm" == Unit.Trim())
			{
				return Convert.ToDecimal(Math.Pow(10.0, -3.0));
			}
			if ("um" == Unit.Trim())
			{
				return Convert.ToDecimal(Math.Pow(10.0, -6.0));
			}
			if ("S" == Unit.Trim().ToUpper())
			{
				return Convert.ToDecimal(Math.Pow(10.0, 0.0));
			}
			if ("MPA" == Unit.Trim().ToUpper())
			{
				return Convert.ToDecimal(Math.Pow(10.0, 0.0));
			}
			return 1m;
		}
	}

	private static cyntecCulture ToCulture(string strTableName)
	{
		if (m_Culture != null)
		{
			return m_Culture;
		}
		m_Culture = new cyntecCulture();
		m_Culture.AddRow("MSG01", "en-US", "Parameter exception, ParamCode ={0} , ParamName = {1}, UpperLimit = {2}.");
		m_Culture.AddRow("MSG01", "zh-TW", "參數異常, ParamCode ={0}, ParamName = {1}, UpperLimit = {2}.");
		m_Culture.AddRow("MSG02", "en-US", "Parameter exception, ParamCode ={0} , ParamName = {1}, LowerLimit = {2}.");
		m_Culture.AddRow("MSG02", "zh-TW", "參數異常, ParamCode ={0}, ParamName = {1}, LowerLimit = {2}.");
		m_Culture.AddRow("MSG03", "en-US", "MatNo={0}, WorkcenterNo={1}, Param={2}({3}) lower limit value is blank.");
		m_Culture.AddRow("MSG03", "zh-TW", "料號={0}, 製程={1}, 參數={2}({3})下限值空白。");
		m_Culture.AddRow("MSG04", "en-US", "MatNo={0}, WorkcenterNo={1}, Param={2}({3}) upper limit value is blank.");
		m_Culture.AddRow("MSG04", "zh-TW", "料號={0}, 製程={1}, 參數={2}({3})上限值空白。");
		m_Culture.dt.TableName = strTableName;
		return m_Culture;
	}

	public decimal ToUpperLimitValue()
	{
		decimal result = default(decimal);
		if (!decimal.TryParse(UpperLimit, out result))
		{
			throw m_Culture.ToException("MSG01", ParamCode, ParamName, UpperLimit);
		}
		return result;
	}

	public decimal ToLowerLimitValue()
	{
		decimal result = default(decimal);
		if (!decimal.TryParse(LowerLimit, out result))
		{
			throw m_Culture.ToException("MSG01", ParamCode, ParamName, LowerLimit);
		}
		return result;
	}

	public ParameterItem()
	{
		ParamCode = "";
		ParamName = "";
		WorkCenterNo = "";
		RefFieldCode = "";
		MatNo = "";
		ParamValue = "";
		Unit = "";
		Enable = "";
		UpperLimit = "";
		LowerLimit = "";
		UPLimitMode = "";
		LOLimitMode = "";
		SourceType = "0";
		Remark = "";
		IsWoParam = false;
		IsSystem = false;
	}

	public static bool IsExist(ParameterItem checkItem, List<ParameterItem> itemList)
	{
		foreach (ParameterItem item in itemList)
		{
			if (item.ParamCode == checkItem.ParamCode)
			{
				return true;
			}
		}
		return false;
	}

	private string m_GetRowItem(DataRow row, string itemName)
	{
		try
		{
			return row[itemName].ToString();
		}
		catch
		{
			return "";
		}
	}

	public virtual void Init(DataRow row)
	{
		ParamCode = row["ParamCode"].ToString();
		ParamName = row["ParamName"].ToString();
		WorkCenterNo = row["WorkCenterNo"].ToString();
		RefFieldCode = row["RefFieldCode"].ToString();
		MatNo = row["MatNo"].ToString();
		ParamValue = row["ParamValue"].ToString();
		Unit = row["Unit"].ToString();
		try
		{
		}
		catch
		{
		}
		try
		{
			UpperLimit = row["UpperValue"].ToString();
			LowerLimit = row["LowerValue"].ToString();
		}
		catch
		{
		}
		Remark = m_GetRowItem(row, "MatParamRemark");
		ID = m_GetRowItem(row, "ID");
		Enable = m_GetRowItem(row, "Enable");
		LOLimitMode = m_GetRowItem(row, "LOLimitMode");
		UPLimitMode = m_GetRowItem(row, "UPLimitMode");
		SourceType = m_GetRowItem(row, "SourceType");
	}

	public virtual void Init(DataTable dt, DataRow row)
	{
		ParamCode = row["ParamCode"].ToString();
		ParamName = row["ParamName"].ToString();
		WorkCenterNo = row["WorkCenterNo"].ToString();
		RefFieldCode = row["RefFieldCode"].ToString();
		MatNo = row["MatNo"].ToString();
		ParamValue = row["ParamValue"].ToString();
		Unit = row["Unit"].ToString();
		if (dt.Columns.Contains("UpperValue"))
		{
			UpperLimit = row["UpperValue"].ToString();
		}
		if (dt.Columns.Contains("LowerValue"))
		{
			LowerLimit = row["LowerValue"].ToString();
		}
		if (dt.Columns.Contains("MatParamRemark"))
		{
			Remark = row["MatParamRemark"].ToString();
		}
		if (dt.Columns.Contains("ID"))
		{
			ID = row["ID"].ToString();
		}
		if (dt.Columns.Contains("Enable"))
		{
			Enable = row["Enable"].ToString();
		}
		if (dt.Columns.Contains("LOLimitMode"))
		{
			LOLimitMode = row["LOLimitMode"].ToString();
		}
		if (dt.Columns.Contains("UPLimitMode"))
		{
			UPLimitMode = row["UPLimitMode"].ToString();
		}
		if (dt.Columns.Contains("SourceType"))
		{
			SourceType = row["SourceType"].ToString();
		}
		if (dt.Columns.Contains("StatusCode"))
		{
			StatusCode = row["SourceType"].ToString();
		}
	}

	public virtual string Verify()
	{
		string text = "";
		if (LOLimitMode == "1" && LowerLimit == "")
		{
			text = text + m_Culture.ToMSg("MSG03", MatNo, WorkCenterNo, ParamName, ParamCode) + Environment.NewLine;
		}
		if (UPLimitMode == "1" && UpperLimit == "")
		{
			text = text + m_Culture.ToMSg("MSG04", MatNo, WorkCenterNo, ParamName, ParamCode) + Environment.NewLine;
		}
		return text;
	}

	public virtual bool IsMaxNg(string data)
	{
		if (data == "")
		{
			return false;
		}
		if (UPLimitMode == "0")
		{
			return false;
		}
		if (UPLimitMode == "2" && string.IsNullOrWhiteSpace(UpperLimit))
		{
			return false;
		}
		double result = 0.0;
		double result2 = 0.0;
		double.TryParse(data, out result);
		double.TryParse(UpperLimit, out result2);
		return result > result2;
	}

	public virtual bool IsMinNg(string data)
	{
		if (data == "")
		{
			return false;
		}
		if (LOLimitMode == "0")
		{
			return false;
		}
		if (LOLimitMode == "2" && string.IsNullOrWhiteSpace(LowerLimit))
		{
			return false;
		}
		double result = 0.0;
		double result2 = 0.0;
		double.TryParse(data, out result);
		double.TryParse(LOLimitMode, out result2);
		return result < result2;
	}

	public virtual bool IsNg(string data)
	{
		if (IsMaxNg(data))
		{
			return true;
		}
		if (IsMinNg(data))
		{
			return true;
		}
		return false;
	}

	public decimal ToNormalValueUpper()
	{
		return decimal.Parse(UpperLimit) * UnitValue;
	}

	public decimal ToNormalValueLower()
	{
		return decimal.Parse(LowerLimit) * UnitValue;
	}

	public decimal ToNormalValueParam()
	{
		return decimal.Parse(ParamValue) * UnitValue;
	}

	public string ToLogString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("param").Append(",").Append(ParamCode)
			.Append(",")
			.Append(ParamName)
			.Append(",")
			.Append(RefFieldCode)
			.Append(",")
			.Append("規格(上限,下限,值)")
			.Append(",")
			.Append(UpperLimit)
			.Append(",")
			.Append(LowerLimit)
			.Append(",")
			.Append(ParamValue)
			.Append(",")
			.Append("單位")
			.Append(",")
			.Append(Unit)
			.Append(",")
			.Append(UnitValue);
		return stringBuilder.ToString();
	}

	public static ParameterItem ToOne(string refFieldCode, IEnumerable<ParameterItem> itemList)
	{
		IEnumerable<ParameterItem> source = itemList.Where((ParameterItem o) => o.RefFieldCode.ToUpper() == refFieldCode.ToUpper());
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}
}
