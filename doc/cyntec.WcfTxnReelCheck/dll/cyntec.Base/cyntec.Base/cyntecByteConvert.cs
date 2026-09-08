using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace cyntec.Base;

public static class cyntecByteConvert
{
	public static string ToNarrowString(string str)
	{
		try
		{
			return new string(str.Select(delegate(char c)
			{
				if (c == '\u3000')
				{
					return ' ';
				}
				return (c >= '！' && c <= '～') ? ((char)(c - 65248)) : c;
			}).ToArray());
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("vbstrconv.wide"))
			{
				throw new Exception("異常," + ex.Message + "，請檢查作業系統的語言/地區，確保它的地區是台灣，語言是繁體中文");
			}
			throw ex;
		}
	}

	public static byte[] ObjectToByteArray(object obj)
	{
		if (obj == null)
		{
			return null;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, obj);
		return memoryStream.ToArray();
	}

	public static object ByteArrayToObject(byte[] arrBytes)
	{
		string text = "";
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			memoryStream.Write(arrBytes, 0, arrBytes.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return binaryFormatter.Deserialize(memoryStream);
		}
		catch (Exception ex)
		{
			text = ex.Message;
		}
		return null;
	}

	public static object ByteArrayToObject2(byte[] arrBytes)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			memoryStream.Write(arrBytes, 0, arrBytes.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return binaryFormatter.Deserialize(memoryStream);
		}
		catch (Exception ex)
		{
			if (arrBytes != null)
			{
				throw new Exception("ByteArrayToObject2, " + arrBytes.Length + ", " + ex.Message);
			}
			throw new Exception("ByteArrayToObject2, " + ex.Message);
		}
	}

	public static string AssemblyName(object obj)
	{
		string[] array = obj.GetType().Assembly.FullName.Split(new string[1] { "," }, StringSplitOptions.None);
		if (array.Length >= 2)
		{
			return array[0] + ", " + array[1];
		}
		return obj.GetType().Assembly.FullName;
	}

	public static bool IsNumber(string tmp)
	{
		double result = 0.0;
		return double.TryParse(tmp, out result);
	}

	public static bool ToBool(object data)
	{
		if (data.ToString().ToUpper() == "Y")
		{
			return true;
		}
		if (data.ToString().ToUpper() == "TRUE")
		{
			return true;
		}
		return false;
	}

	public static string ToFlag(bool data)
	{
		if (data)
		{
			return "Y";
		}
		return "N";
	}

	public static byte[] StringToByteArray(string hex)
	{
		return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
	}

	public static string TempleteToString(string templeteText, Dictionary<string, string> paramList, string regexKeyword)
	{
		return Regex.Replace(templeteText, regexKeyword, delegate(Match match)
		{
			KeyValuePair<string, string> keyValuePair = paramList.OfType<KeyValuePair<string, string>>().FirstOrDefault((KeyValuePair<string, string> a) => a.Key.ToUpper() == match.Groups[1].Value.ToUpper());
			return keyValuePair.Equals(default(KeyValuePair<string, string>)) ? match.ToString() : keyValuePair.Value;
		});
	}
}
