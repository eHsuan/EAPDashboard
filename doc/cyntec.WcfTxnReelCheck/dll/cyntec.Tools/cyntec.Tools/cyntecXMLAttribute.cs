using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace cyntec.Tools;

public class cyntecXMLAttribute
{
	protected XmlAttribute m_Attribute = null;

	public XmlAttribute xAttribute => m_Attribute;

	public cyntecXMLAttribute()
	{
		m_Attribute = null;
	}

	public cyntecXMLAttribute(XmlAttribute attr)
	{
		m_Attribute = attr;
	}

	public bool ToBoolean()
	{
		bool result = false;
		if (m_Attribute.Value == null)
		{
			return false;
		}
		bool.TryParse(m_Attribute.Value, out result);
		return result;
	}

	public T ToEnum<T>()
	{
		List<T> list = (from T v in Enum.GetValues(typeof(T))
			select (v)).ToList();
		foreach (T item in list)
		{
			if (m_Attribute.Value.ToUpper() == item.ToString().ToUpper())
			{
				return item;
			}
		}
		throw new Exception("Can not find the item of enum !");
	}
}
