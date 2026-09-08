using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace cyntec.Base;

public static class cyntecMyListExtensions
{
	public static double Max(this List<double> values)
	{
		if (values.Count == 0)
		{
			return 0.0;
		}
		return values.ToArray().Max();
	}

	public static double Min(this List<double> values)
	{
		if (values.Count == 0)
		{
			return 0.0;
		}
		return values.ToArray().Min();
	}

	public static double Ca(this List<double> values, double USL, double LSL, double center)
	{
		double? num = null;
		double? num2 = null;
		double num3 = 0.0;
		if (USL != double.MinValue && USL != double.MaxValue)
		{
			num = USL;
		}
		if (LSL != double.MinValue && LSL != double.MaxValue)
		{
			num2 = LSL;
		}
		if (num.HasValue && num2.HasValue && num.Value < num2.Value)
		{
			string[] obj = new string[5] { "Ca計算異常, 上限(", null, null, null, null };
			double? num4 = num;
			obj[1] = num4.ToString();
			obj[2] = ")不可小於下限(";
			num4 = num2;
			obj[3] = num4.ToString();
			obj[4] = ")";
			throw new Exception(string.Concat(obj));
		}
		if (!num.HasValue && !num2.HasValue)
		{
			num = values.Max();
			num2 = values.Min();
			center = (num2 + (num - num2) / 2.0).Value;
		}
		double num5 = values.Mean();
		if (num.HasValue && num2.HasValue)
		{
			center = (num2 + (num - num2) / 2.0).Value;
			return (num5 - center) / ((num.Value - num2.Value) / 2.0);
		}
		if (num.HasValue && !num2.HasValue)
		{
			center = (values.Min() + (num - values.Min()) / 2.0).Value;
			return (num5 - center) / ((num.Value - center) / 2.0);
		}
		if (!num.HasValue && num2.HasValue)
		{
			center = (num2 + (values.Max() - num2) / 2.0).Value;
			return (num5 - center) / ((num2.Value - center) / 2.0);
		}
		throw new Exception("Ca計算異常");
	}

	public static double Cp(this List<double> values, double USL, double LSL)
	{
		double? num = null;
		double? num2 = null;
		double num3 = 0.0;
		if (USL != double.MinValue && USL != double.MaxValue)
		{
			num = USL;
		}
		if (LSL != double.MinValue && LSL != double.MaxValue)
		{
			num2 = LSL;
		}
		if (num.HasValue && num2.HasValue && num.Value < num2.Value)
		{
			string[] obj = new string[5] { "Cp計算異常, 上限(", null, null, null, null };
			double? num4 = num;
			obj[1] = num4.ToString();
			obj[2] = ")不可小於下限(";
			num4 = num2;
			obj[3] = num4.ToString();
			obj[4] = ")";
			throw new Exception(string.Concat(obj));
		}
		double num5 = values.StandardDeviation();
		if (!num.HasValue && !num2.HasValue)
		{
			num = values.Max();
			num2 = values.Min();
		}
		if (num.HasValue && num2.HasValue)
		{
			return (num.Value - num2.Value) / (6.0 * num5);
		}
		if (num.HasValue && !num2.HasValue)
		{
			double num6 = values.Mean();
			return (num.Value - num6) / (3.0 * num5);
		}
		if (!num.HasValue && num2.HasValue)
		{
			double num7 = values.Mean();
			return (num7 - num2.Value) / (3.0 * num5);
		}
		throw new Exception("Cp計算異常");
	}

	public static double Cpu(this List<double> values, double USL)
	{
		double num = values.Mean();
		double num2 = values.StandardDeviation();
		return (USL - num) / (3.0 * num2);
	}

	public static double Cpk(this List<double> values, double USL, double LSL, double Center)
	{
		if (m_IsDefine(USL) && m_IsDefine(LSL))
		{
			double num = values.StandardDeviation();
			double num2 = Math.Abs(values.Ca(USL, LSL, Center));
			double num3 = values.Cp(USL, LSL);
			return (1.0 - num2) * num3;
		}
		if (m_IsDefine(USL) || m_IsDefine(LSL))
		{
			double num4 = Math.Abs(values.Ca(USL, LSL, Center));
			double num5 = values.Cp(USL, LSL);
			return Math.Abs((1.0 - num4) * num5);
		}
		double uSL = values.Max();
		double lSL = values.Min();
		double num6 = values.Cp(uSL, LSL);
		double num7 = values.Cp(USL, lSL);
		if (num6 < num7)
		{
			return num6;
		}
		return num7;
	}

	private static bool m_IsDefine(double val)
	{
		return val != double.MaxValue && val != double.MinValue;
	}

	private static bool m_IsDefine(decimal val)
	{
		return val != decimal.MaxValue && val != decimal.MinValue;
	}

	public static double Mean(this List<double> values)
	{
		return (values.Count == 0) ? 0.0 : values.Mean(0, values.Count);
	}

	public static double Mean(this List<double> values, int start, int end)
	{
		double num = 0.0;
		for (int i = start; i < end; i++)
		{
			num += values[i];
		}
		return num / (double)(end - start);
	}

	public static double R(this List<double> values)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		if (values.Count > 0)
		{
			num2 = values[0];
			num3 = values[0];
		}
		foreach (double value in values)
		{
			if (value > num2)
			{
				num2 = value;
			}
			if (value < num3)
			{
				num3 = value;
			}
			num += num2 - num3;
		}
		if (values.Count > 0)
		{
			return num / (double)values.Count;
		}
		return 0.0;
	}

	public static double Variance(this List<double> values)
	{
		return values.Variance(values.Mean(), 0, values.Count);
	}

	public static double Variance(this List<double> values, double mean)
	{
		return values.Variance(mean, 0, values.Count);
	}

	public static double Variance(this List<double> values, double mean, int start, int end)
	{
		double num = 0.0;
		for (int i = start; i < end; i++)
		{
			num += Math.Pow(values[i] - mean, 2.0);
		}
		int num2 = end - start;
		if (start > 0)
		{
			num2--;
		}
		return num / (double)num2;
	}

	public static double StandardDeviation(this List<double> values)
	{
		return (values.Count == 0) ? 0.0 : values.StandardDeviation(0, values.Count);
	}

	public static double StandardDeviation(this List<double> values, int start, int end)
	{
		double mean = values.Mean(start, end);
		double d = values.Variance(mean, start, end);
		return Math.Sqrt(d);
	}

	public static List<List<T>> splitList<T>(this List<T> locations, int nSize = 30)
	{
		List<List<T>> list = new List<List<T>>();
		for (int i = 0; i < locations.Count; i += nSize)
		{
			list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
		}
		return list;
	}

	public static string ToSerializeXML<T>(this T value)
	{
		if (value == null)
		{
			return string.Empty;
		}
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringWriter stringWriter = new StringWriter();
			using XmlWriter xmlWriter = XmlWriter.Create(stringWriter);
			xmlSerializer.Serialize(xmlWriter, value);
			return stringWriter.ToString();
		}
		catch (Exception ex)
		{
			return "無法轉換," + ex.Message;
		}
	}

	public static decimal Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		decimal value = source.Variance(selector);
		return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(value)));
	}

	public static decimal Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
	{
		decimal avg = source.Select((TSource x) => selector(x)).Average();
		decimal num = source.Select((TSource x) => (selector(x) - avg) * (selector(x) - avg)).Sum();
		if (source.Count() <= 1)
		{
			return num;
		}
		return num / (decimal)(source.Count() - 1);
	}

	public static decimal Ca<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal USL, decimal LSL, decimal center)
	{
		decimal? num = null;
		decimal? num2 = null;
		if (USL != decimal.MinValue && USL != decimal.MaxValue)
		{
			num = USL;
		}
		if (LSL != decimal.MinValue && LSL != decimal.MaxValue)
		{
			num2 = LSL;
		}
		if (num.HasValue && num2.HasValue && num.Value < num2.Value)
		{
			string[] obj = new string[5] { "Ca計算異常, 上限(", null, null, null, null };
			decimal? num3 = num;
			obj[1] = num3.ToString();
			obj[2] = ")不可小於下限(";
			num3 = num2;
			obj[3] = num3.ToString();
			obj[4] = ")";
			throw new Exception(string.Concat(obj));
		}
		if (!num.HasValue && !num2.HasValue)
		{
			num = source.Select((TSource x) => selector(x)).Max();
			num2 = source.Select((TSource x) => selector(x)).Min();
			center = (num2 + (num - num2) / (decimal?)2).Value;
		}
		decimal num4 = source.Select((TSource x) => selector(x)).Average();
		if (num.HasValue && num2.HasValue)
		{
			center = (num2 + (num - num2) / (decimal?)2).Value;
			return (num4 - center) / ((num.Value - num2.Value) / 2m);
		}
		if (num.HasValue && !num2.HasValue)
		{
			decimal num5 = source.Select((TSource x) => selector(x)).Min();
			decimal value = num5;
			decimal? num6 = (num - (decimal?)num5) / (decimal?)2;
			center = ((decimal?)value + num6).Value;
			return (num4 - center) / ((num.Value - center) / 2m);
		}
		if (!num.HasValue && num2.HasValue)
		{
			decimal num7 = source.Select((TSource x) => selector(x)).Max();
			decimal? num3 = num2;
			decimal value2 = num7;
			decimal? num8 = num2;
			center = (num3 + ((decimal?)value2 - num8) / (decimal?)2).Value;
			return (num4 - center) / ((num2.Value - center) / 2m);
		}
		throw new Exception("Ca計算異常");
	}

	public static decimal Cp<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal USL, decimal LSL)
	{
		decimal? num = null;
		decimal? num2 = null;
		if (USL != decimal.MinValue && USL != decimal.MaxValue)
		{
			num = USL;
		}
		if (LSL != decimal.MinValue && LSL != decimal.MaxValue)
		{
			num2 = LSL;
		}
		if (num.HasValue && num2.HasValue && num.Value < num2.Value)
		{
			string[] obj = new string[5] { "Cp計算異常, 上限(", null, null, null, null };
			decimal? num3 = num;
			obj[1] = num3.ToString();
			obj[2] = ")不可小於下限(";
			num3 = num2;
			obj[3] = num3.ToString();
			obj[4] = ")";
			throw new Exception(string.Concat(obj));
		}
		decimal num4 = source.Stdev(selector);
		if (!num.HasValue && !num2.HasValue)
		{
			num = source.Select((TSource x) => selector(x)).Max();
			num2 = source.Select((TSource x) => selector(x)).Min();
		}
		if (num.HasValue && num2.HasValue)
		{
			return (num.Value - num2.Value) / (6m * num4);
		}
		if (num.HasValue && !num2.HasValue)
		{
			decimal num5 = source.Select((TSource x) => selector(x)).Average();
			return (num.Value - num5) / (3m * num4);
		}
		if (!num.HasValue && num2.HasValue)
		{
			decimal num6 = source.Select((TSource x) => selector(x)).Average();
			return (num6 - num2.Value) / (3m * num4);
		}
		throw new Exception("Cp計算異常");
	}

	public static decimal Cpk<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal USL, decimal LSL, decimal Center)
	{
		if (m_IsDefine(USL) && m_IsDefine(LSL))
		{
			decimal num = Math.Abs(source.Ca(selector, USL, LSL, Center));
			decimal num2 = source.Cp(selector, USL, LSL);
			return (1m - num) * num2;
		}
		if (m_IsDefine(USL) || m_IsDefine(LSL))
		{
			decimal num3 = Math.Abs(source.Ca(selector, USL, LSL, Center));
			decimal num4 = source.Cp(selector, USL, LSL);
			return Math.Abs((1m - num3) * num4);
		}
		decimal uSL = source.Select((TSource x) => selector(x)).Max();
		decimal lSL = source.Select((TSource x) => selector(x)).Min();
		decimal num5 = source.Cp(selector, uSL, LSL);
		decimal num6 = source.Cp(selector, USL, lSL);
		if (num5 < num6)
		{
			return num5;
		}
		return num6;
	}
}
