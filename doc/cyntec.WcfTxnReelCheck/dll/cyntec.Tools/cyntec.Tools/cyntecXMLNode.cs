using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace cyntec.Tools;

public class cyntecXMLNode
{
	public XmlNamespaceManager xmlm;

	protected XmlNode m_node = null;

	public object Tag { get; set; }

	public XmlElement xElement => m_node as XmlElement;

	public cyntecXMLNode this[string idx]
	{
		get
		{
			if (m_node == null)
			{
				return null;
			}
			cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(xElement.SelectSingleNode(idx, xmlm));
			cyntecXMLNode2.xmlm = xmlm;
			return cyntecXMLNode2;
		}
	}

	public XmlNode xNode => m_node;

	public cyntecXMLNode()
	{
		m_node = null;
	}

	public cyntecXMLNode(XmlNode node)
	{
		m_node = node;
	}

	public override string ToString()
	{
		if (m_node == null)
		{
			return "";
		}
		return m_node.InnerText;
	}

	public string ToString(params object[] items)
	{
		if (m_node == null)
		{
			return "";
		}
		string text = m_node.InnerText;
		for (int i = 0; i < items.Length && i + 1 < items.Length; i += 2)
		{
			text = text.Replace(items[i].ToString(), items[i + 1].ToString());
		}
		return text;
	}

	public string ToPath()
	{
		if (m_node == null)
		{
			return "";
		}
		string text = m_node.InnerText.Trim();
		if (text.Length < 1)
		{
			return text;
		}
		if (text.Trim().Substring(0, 1) == ".")
		{
			text = AppDomain.CurrentDomain.BaseDirectory + text.Substring(2);
		}
		else if (text.Trim().ToUpper().StartsWith("@DESKTOP"))
		{
			text = text.ToUpper().Replace("@DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
		}
		return text;
	}

	public string ToPathSlash()
	{
		string text = ToPath();
		if (!text.EndsWith("\\"))
		{
			text += "\\";
		}
		return text;
	}

	public long ToLong()
	{
		long result = 0L;
		if (m_node == null)
		{
			return 0L;
		}
		long.TryParse(m_node.InnerText, out result);
		return result;
	}

	public int ToInt()
	{
		int result = 0;
		if (m_node == null)
		{
			return 0;
		}
		if (int.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public short ToInt16()
	{
		short result = 0;
		if (m_node == null)
		{
			return 0;
		}
		if (short.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt16(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public int ToInt32()
	{
		int result = 0;
		if (m_node == null)
		{
			return 0;
		}
		if (int.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public ushort ToUInt16()
	{
		ushort result = 0;
		if (m_node == null)
		{
			return 0;
		}
		if (ushort.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToUInt16(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public uint ToUInt32()
	{
		uint result = 0u;
		if (m_node == null)
		{
			return 0u;
		}
		if (uint.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToUInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public ulong ToUInt64()
	{
		ulong result = 0uL;
		if (m_node == null)
		{
			return 0uL;
		}
		if (ulong.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToUInt64(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public float ToFloat()
	{
		float result = 0f;
		if (m_node == null)
		{
			return 0f;
		}
		if (float.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public double ToDouble()
	{
		double result = 0.0;
		if (m_node == null)
		{
			return 0.0;
		}
		if (double.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public decimal ToDecimal()
	{
		decimal result = default(decimal);
		if (m_node == null)
		{
			return 0m;
		}
		if (decimal.TryParse(m_node.InnerText, out result))
		{
			return result;
		}
		if (m_node.InnerText.ToUpper().StartsWith("0X"))
		{
			try
			{
				result = Convert.ToInt32(m_node.InnerText, 16);
			}
			catch
			{
			}
		}
		return result;
	}

	public bool ToBoolean()
	{
		bool result = false;
		if (m_node == null)
		{
			return false;
		}
		bool.TryParse(m_node.InnerText, out result);
		return result;
	}

	public byte[] ToIP()
	{
		byte[] array = new byte[0];
		string[] array2 = m_node.InnerText.Trim().Split(new string[1] { "." }, StringSplitOptions.None);
		if (array2.Length != 4)
		{
			throw new Exception("無法轉IP, " + m_node.InnerText);
		}
		try
		{
			array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = byte.Parse(array2[i]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception("無法IP, " + m_node.InnerText + ", " + ex.Message);
		}
		return array;
	}

	public cyntecXMLNode[] ToList()
	{
		List<cyntecXMLNode> list = new List<cyntecXMLNode>();
		if (m_node == null)
		{
			return list.ToArray();
		}
		foreach (XmlNode childNode in m_node.ChildNodes)
		{
			if (childNode.NodeType != XmlNodeType.Comment)
			{
				cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(childNode);
				cyntecXMLNode2.xmlm = xmlm;
				list.Add(cyntecXMLNode2);
			}
		}
		return list.ToArray();
	}

	public int ToListCount()
	{
		if (m_node == null)
		{
			return 0;
		}
		return m_node.ChildNodes.Count;
	}

	public cyntecXMLNode[] ToList(string nodeName)
	{
		List<cyntecXMLNode> list = new List<cyntecXMLNode>();
		for (XmlNode xmlNode = m_node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.Name == nodeName)
			{
				cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
				cyntecXMLNode2.xmlm = xmlm;
				list.Add(cyntecXMLNode2);
			}
		}
		return list.ToArray();
	}

	public List<string> ToStringList()
	{
		List<string> list = new List<string>();
		if (m_node == null)
		{
			return list;
		}
		for (int i = 0; i < m_node.ChildNodes.Count; i++)
		{
			if (m_node.ChildNodes[i].NodeType != XmlNodeType.Comment)
			{
				list.Add(m_node.ChildNodes[i].InnerText);
			}
		}
		return list;
	}

	public DateTime ToDateTime()
	{
		return Convert.ToDateTime(m_node.InnerText);
	}

	public void FillNode(object rawData)
	{
		if (rawData.GetType().Name == "DataTable")
		{
			FillNodeInRecord(m_node, rawData);
		}
	}

	public static void FillNodeInRecord(XmlNode nodeParent, object rawData)
	{
		DataTable dataTable = rawData as DataTable;
		for (int i = 0; i < dataTable.Columns.Count; i++)
		{
			XmlNode xmlNode = nodeParent.SelectSingleNode(dataTable.Columns[i].ColumnName);
			if (xmlNode != null)
			{
				xmlNode.InnerText = dataTable.Rows[0][i].ToString();
			}
		}
	}

	public void AppendNode(cyntecXMLNode node)
	{
		m_node.AppendChild(node.xNode);
	}

	public void AppendNode(cyntecXML doc, DataTable dt, DataRow row)
	{
		AppendNodeInRecordSet(doc, m_node, dt, row);
	}

	public cyntecXMLNode AppendNodePreFix(cyntecXML doc, string NodeName, string NodeText, params string[] prefix)
	{
		XmlNode xmlNode = null;
		if (prefix == null || prefix.Length == 0)
		{
			xmlNode = doc.CreateNode(XmlNodeType.Element, NodeName, "");
		}
		else if (xmlm != null && xmlm.HasNamespace(prefix[0]))
		{
			xmlNode = doc.CreateNode(XmlNodeType.Element, NodeName, xmlm.LookupNamespace(prefix[0]));
		}
		if (NodeText != null)
		{
			xmlNode.InnerText = NodeText;
		}
		m_node.AppendChild(xmlNode);
		cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
		cyntecXMLNode2.xmlm = xmlm;
		return cyntecXMLNode2;
	}

	public cyntecXMLNode AppendNode(cyntecXML doc, string NodeName, string NodeText)
	{
		return AppendNodePreFix(doc, NodeName, NodeText);
	}

	public XmlAttribute AppendAttr(cyntecXML doc, string attrName, string attrValue)
	{
		XmlAttribute xmlAttribute = doc.CreateAttribute(attrName);
		xmlAttribute.Value = attrValue;
		xElement.SetAttributeNode(xmlAttribute);
		return xmlAttribute;
	}

	public static void AppendNodeInRecordSet(cyntecXML doc, XmlNode nodeParent, DataTable dt, DataRow row)
	{
		doc.AppendNodeInRecordSet(nodeParent, dt, row);
	}

	public void Clear()
	{
		xNode.RemoveAll();
	}

	public cyntecXMLNode GetNodeByAttr(string AttrName, string AttrValue)
	{
		cyntecXMLNode cyntecXMLNode2 = null;
		XmlNode xmlNode = null;
		if (m_node == null)
		{
			throw new Exception("this node is null");
		}
		foreach (XmlNode childNode in m_node.ChildNodes)
		{
			foreach (XmlAttribute attribute in childNode.Attributes)
			{
				if (attribute.Name == AttrName && attribute.Value == AttrValue)
				{
					xmlNode = childNode;
					break;
				}
			}
			if (xmlNode != null)
			{
				break;
			}
		}
		if (xmlNode == null)
		{
			throw new Exception("can not find the xml item, AttrName= " + AttrName + ", AttrValue=" + AttrValue);
		}
		cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
		cyntecXMLNode2.xmlm = xmlm;
		return cyntecXMLNode2;
	}

	public cyntecXMLNode GetNode(params object[] items)
	{
		if (m_node == null)
		{
			throw new Exception("this node is null");
		}
		XmlNode xmlNode = m_node;
		for (int i = 0; i < items.Length; i++)
		{
			xmlNode = xmlNode[items[i].ToString()];
			if (xmlNode == null)
			{
				throw new Exception("can not find the xml item " + items[i].ToString());
			}
		}
		cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
		cyntecXMLNode2.xmlm = xmlm;
		return cyntecXMLNode2;
	}

	public cyntecXMLNode GetNodeTry(params object[] items)
	{
		if (m_node == null)
		{
			return null;
		}
		XmlNode xmlNode = m_node;
		for (int i = 0; i < items.Length; i++)
		{
			try
			{
				xmlNode = xmlNode.SelectSingleNode(items[i].ToString(), xmlm);
			}
			catch (Exception ex)
			{
				throw new Exception("The node id could be invalid, " + ex.Message);
			}
			if (xmlNode == null)
			{
				return null;
			}
		}
		cyntecXMLNode cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
		cyntecXMLNode2.xmlm = xmlm;
		return cyntecXMLNode2;
	}

	public cyntecXMLAttribute GetAttribute(string id)
	{
		for (int i = 0; i < xNode.Attributes.Count; i++)
		{
			if (id.ToUpper() == xNode.Attributes[i].Name.ToUpper())
			{
				return new cyntecXMLAttribute(xNode.Attributes[i]);
			}
		}
		return null;
	}

	public T ToEnum<T>()
	{
		List<T> list = (from T v in Enum.GetValues(typeof(T))
			select (v)).ToList();
		foreach (T item in list)
		{
			if (m_node.InnerText.ToUpper().Trim() == item.ToString().ToUpper())
			{
				return item;
			}
		}
		throw new Exception("Can not find the item of enum !");
	}

	public void SetValue(string value)
	{
		xNode.InnerText = value;
	}

	public void SetValue(object obj)
	{
		xNode.InnerText = obj.ToString();
	}
}
