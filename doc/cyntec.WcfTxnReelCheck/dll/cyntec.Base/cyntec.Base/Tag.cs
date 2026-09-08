using System;
using System.Collections.Generic;

namespace cyntec.Base;

public class Tag
{
	private List<string> m_Parm = null;

	private Dictionary<string, string> m_Dict = null;

	private string m_ID;

	public string ID => m_ID;

	public string this[int idx]
	{
		get
		{
			if (m_Parm == null)
			{
				return "";
			}
			if (m_Parm.Count <= idx)
			{
				return "";
			}
			return m_Parm[idx];
		}
	}

	public string this[string key]
	{
		get
		{
			if (m_Dict.ContainsKey(key))
			{
				return m_Dict[key.ToUpper()];
			}
			return "";
		}
	}

	public static Tag NewTag(object value)
	{
		if (value is Tag)
		{
			return value as Tag;
		}
		Tag tag = new Tag();
		tag.init(value);
		return tag;
	}

	public void init(object tag)
	{
		if (tag == null)
		{
			m_ID = "";
			return;
		}
		m_Parm = new List<string>();
		m_Dict = new Dictionary<string, string>();
		string[] array = tag.ToString().Split(new string[1] { ":" }, StringSplitOptions.None);
		m_ID = array[0].Trim();
		for (int i = 1; i < array.Length; i++)
		{
			if (!(array[i].Trim() == ""))
			{
				string[] array2 = array[i].Split(new string[1] { "@" }, StringSplitOptions.None);
				string text = array2[0].Trim();
				string text2 = text.Trim();
				if (array2.Length > 1)
				{
					text2 = array2[1];
				}
				m_Parm.Add(text2);
				m_Dict.Add(text.ToUpper(), text2);
			}
		}
	}
}
