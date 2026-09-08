using System;
using System.Collections.Generic;

namespace cyntec.Base;

public class cyntecDateTime
{
	private DateTime m_dtData = DateTime.Now;

	public static string Now()
	{
		return ToString(DateTime.Now);
	}

	public static DateTime ToDateTime(object rowField)
	{
		if (string.IsNullOrWhiteSpace(rowField.ToString()))
		{
			return DateTime.MinValue;
		}
		if (rowField.GetType() == typeof(string) || rowField.GetType() == typeof(string))
		{
			return ToDateTime(rowField.ToString());
		}
		return (DateTime)rowField;
	}

	public static DateTime ToDateTime(string time)
	{
		if (string.IsNullOrWhiteSpace(time))
		{
			return DateTime.MinValue;
		}
		return Convert.ToDateTime(time);
	}

	public static string ToString(DateTime time)
	{
		return ToMSSQL(time);
	}

	public static string ToStandString(DateTime time)
	{
		return time.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
	}

	public override string ToString()
	{
		return ToString(m_dtData);
	}

	public static string NowToMSSQL()
	{
		return ToMSSQL(DateTime.Now);
	}

	public static string NowToStandString()
	{
		return ToStandString(DateTime.Now);
	}

	public static string ToMSSQL(DateTime t)
	{
		return t.ToString("yyyy-MM-dd HH:mm:ss.fff");
	}

	public static DateTime ToNextTime(DateTime t, List<string> timeList)
	{
		string text = t.ToString("yyyy-MM-dd");
		string text2 = "";
		foreach (string time in timeList)
		{
			DateTime dateTime = Convert.ToDateTime(text + " " + time.Trim());
			if (t < dateTime)
			{
				return dateTime;
			}
		}
		text2 = timeList[0];
		t = t.AddDays(1.0);
		text = t.ToString("yyyy-MM-dd");
		return Convert.ToDateTime(text + " " + text2.Trim());
	}

	public static bool IsDateTimeOK(string s)
	{
		DateTime result;
		return DateTime.TryParse(s, out result);
	}

	public TimeSpan ToTimeSpan()
	{
		return DateTime.Now - m_dtData;
	}
}
