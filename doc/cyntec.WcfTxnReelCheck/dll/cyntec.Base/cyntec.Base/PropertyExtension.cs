using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace cyntec.Base;

public static class PropertyExtension
{
	public static string Right(this string s, int length)
	{
		length = Math.Max(length, 0);
		if (s.Length > length)
		{
			return s.Substring(s.Length - length, length);
		}
		return s;
	}

	public static string GetAssemblyName(this object p_object)
	{
		string[] array = p_object.GetType().Assembly.FullName.Split(new string[1] { "," }, StringSplitOptions.None);
		return array[0].Trim();
	}

	public static string GetAssemblyVersion(this object p_object)
	{
		string[] array = p_object.GetType().Assembly.FullName.Split(new string[1] { "," }, StringSplitOptions.None);
		array = array[1].Split(new string[1] { "=" }, StringSplitOptions.None);
		if (array.Length >= 2)
		{
			return array[1].Trim();
		}
		return "";
	}

	public static void SetPropertyValue(this object p_object, string p_propertyName, object value)
	{
		PropertyInfo property = p_object.GetType().GetProperty(p_propertyName);
		if (property == null)
		{
			p_object.SetFieldValue(p_propertyName, value);
		}
		else
		{
			property.SetValue(p_object, Convert.ChangeType(value, property.PropertyType), null);
		}
	}

	public static void SetFieldValue(this object p_object, string p_fieldName, object value)
	{
		FieldInfo field = p_object.GetType().GetField(p_fieldName);
		if (field == null)
		{
			throw new Exception("物件:" + p_object.ToString() + "  無法取得屬性" + p_fieldName);
		}
		field.SetValue(p_object, value);
	}

	public static object GetPropertyValue(this object p_object, string p_propertyName)
	{
		PropertyInfo property = p_object.GetType().GetProperty(p_propertyName);
		if (property == null)
		{
			return p_object.GetFieldValue(p_propertyName);
		}
		return property.GetValue(p_object, null);
	}

	public static object GetFieldValue(this object p_object, string p_fieldName)
	{
		FieldInfo field = p_object.GetType().GetField(p_fieldName);
		if (field == null)
		{
			throw new Exception("物件:" + p_object.ToString() + "  無法取得屬性" + p_fieldName);
		}
		return field.GetValue(p_object);
	}

	public static string ToFormatFieldValueString(this object p_object, List<string> fieldNameList)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string fieldName in fieldNameList)
		{
			stringBuilder.Append(fieldName).Append(" = ").Append(p_object.GetPropertyValue(fieldName))
				.Append(Environment.NewLine);
		}
		return stringBuilder.ToString();
	}

	public static string ToLogString(this Exception exception, string environmentStackTrace)
	{
		List<string> userStackTraceLines = GetUserStackTraceLines(environmentStackTrace);
		if (userStackTraceLines.Count == 1)
		{
			return userStackTraceLines[0];
		}
		if (userStackTraceLines.Count < 1)
		{
			return "";
		}
		userStackTraceLines.RemoveAt(0);
		List<string> stackTraceLines = GetStackTraceLines(exception.StackTrace);
		stackTraceLines.AddRange(userStackTraceLines);
		string text = string.Join(Environment.NewLine, stackTraceLines);
		return exception.Message + Environment.NewLine + text;
	}

	private static List<string> GetStackTraceLines(string stackTrace)
	{
		return stackTrace.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None).ToList();
	}

	private static List<string> GetUserStackTraceLines(string fullStackTrace)
	{
		List<string> list = new List<string>();
		Regex regex = new Regex("([^\\)]*\\)) in (.*):line (\\d)*$");
		List<string> stackTraceLines = GetStackTraceLines(fullStackTrace);
		foreach (string item in stackTraceLines)
		{
			if (regex.IsMatch(item))
			{
				list.Add(item);
			}
		}
		return list;
	}
}
