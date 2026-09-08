using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cyntec.Tools;

public class cyntecJSON
{
	public static string DictionaryToJSON(Dictionary<string, string> itemList, string colNameKey, string colNameValue)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add(colNameKey, typeof(string));
		dataTable.Columns.Add(colNameValue, typeof(string));
		foreach (KeyValuePair<string, string> item in itemList)
		{
			dataTable.Rows.Add(item.Key, item.Value);
		}
		return JsonConvert.SerializeObject((object)dataTable);
	}

	public static void ItemValueSet(Dictionary<string, object> obj, string pKey, object oValue)
	{
		string key = pKey.ToUpper();
		if (obj.ContainsKey(key))
		{
			obj[key] = oValue;
		}
		else
		{
			obj.Add(key, oValue);
		}
	}

	public static T ItemValueGet<T>(Dictionary<string, object> obj, string pKey)
	{
		string key = pKey.ToUpper();
		if (!obj.ContainsKey(key))
		{
			return default(T);
		}
		object obj2 = obj[key];
		if (obj2 == null)
		{
			return default(T);
		}
		if (typeof(T) == obj2.GetType())
		{
			return (T)obj2;
		}
		if (typeof(T) == typeof(string))
		{
			return (T)Convert.ChangeType(obj2.ToString(), typeof(T));
		}
		if (obj2.GetType() == typeof(JArray) || obj2.GetType() == typeof(JObject))
		{
			return JsonConvert.DeserializeObject<T>(obj2.ToString());
		}
		return (T)Convert.ChangeType(obj2, typeof(T));
	}

	public static T ItemValueGetX<T>(Dictionary<string, object> obj, string pKey)
	{
		if (!obj.ContainsKey(pKey))
		{
			return default(T);
		}
		object obj2 = obj[pKey];
		if (obj2 == null)
		{
			return default(T);
		}
		if (typeof(T) == obj2.GetType())
		{
			return (T)obj2;
		}
		if (typeof(T) == typeof(string))
		{
			return (T)Convert.ChangeType(obj2.ToString(), typeof(T));
		}
		if (obj2.GetType() == typeof(JArray) || obj2.GetType() == typeof(JObject))
		{
			return JsonConvert.DeserializeObject<T>(obj2.ToString());
		}
		return (T)Convert.ChangeType(obj2, typeof(T));
	}

	public static T ToObject<T>(string JsonData)
	{
		return JsonConvert.DeserializeObject<T>(JsonData);
	}

	public static string ToString(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}

	public static DataTable ToDataTable(JArray oArray)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		DataTable dataTable = null;
		JArray val = null;
		int num = 0;
		foreach (JObject item in oArray)
		{
			JObject val2 = item;
			if (val == null)
			{
				val = new JArray();
			}
			num++;
			val.Add((JToken)(object)val2);
			if (num < 1000 && (object)val2 != ((JToken)oArray).Last)
			{
				continue;
			}
			if (dataTable == null)
			{
				dataTable = ToObject<DataTable>(((object)val).ToString());
			}
			else
			{
				DataTable dataTable2 = ToObject<DataTable>(((object)val).ToString());
				foreach (DataRow row in dataTable2.Rows)
				{
					dataTable.ImportRow(row);
				}
			}
			num = 0;
			val = null;
		}
		return dataTable;
	}
}
