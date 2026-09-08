using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace cyntec.Tools;

public class cyntecXML : XmlDocument
{
	private XmlNamespaceManager m_xmlm;

	public string RootTag = "ROOT";

	public XmlNamespaceManager xmlm
	{
		get
		{
			if (m_xmlm == null)
			{
				m_xmlm = new XmlNamespaceManager(base.NameTable);
			}
			return m_xmlm;
		}
	}

	public cyntecXMLNode rootNode
	{
		get
		{
			XmlNode node = null;
			XmlNodeList childNodes = ChildNodes;
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes[i].Name == RootTag)
				{
					node = childNodes[i];
				}
			}
			return new cyntecXMLNode(node);
		}
	}

	public override void Load(string filename)
	{
		base.Load(filename);
		RootTag = base.DocumentElement.Name;
	}

	public cyntecXMLNode GetNode(params object[] items)
	{
		XmlNode xmlNode = this[RootTag];
		if (xmlNode == null)
		{
			throw new Exception("can not find the xml item: " + RootTag);
		}
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

	public List<string> GetNodeList(params object[] items)
	{
		XmlNode xmlNode = this[RootTag];
		if (xmlNode == null)
		{
			throw new Exception("can not find the xml item: " + RootTag);
		}
		List<string> list = new List<string>();
		for (int i = 0; i < items.Length; i++)
		{
			xmlNode = xmlNode[items[i].ToString()];
			if (xmlNode == null)
			{
				throw new Exception("can not find the xml item " + items[i].ToString());
			}
			foreach (XmlNode item in xmlNode)
			{
				list.Add(item.InnerText);
			}
		}
		return list;
	}

	public cyntecXMLNode GetNodeX(params object[] items)
	{
		XmlNode xmlNode = FirstChild;
		if (xmlNode == null)
		{
			throw new Exception("can not find the xml item: " + RootTag);
		}
		for (int i = 0; i < items.Length; i++)
		{
			xmlNode = xmlNode[items[i].ToString()];
			if (xmlNode == null)
			{
				throw new Exception("can not find the xml item " + items[i].ToString());
			}
		}
		return new cyntecXMLNode(xmlNode);
	}

	public cyntecXMLNode GetNodeTry(params object[] items)
	{
		cyntecXMLNode cyntecXMLNode2 = null;
		XmlNode xmlNode = null;
		try
		{
			xmlNode = SelectSingleNode(RootTag, m_xmlm);
			if (xmlNode == null)
			{
				return null;
			}
			for (int i = 0; i < items.Length; i++)
			{
				xmlNode = xmlNode.SelectSingleNode(items[i].ToString(), m_xmlm);
				if (xmlNode == null)
				{
					return null;
				}
			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			return null;
		}
		cyntecXMLNode2 = new cyntecXMLNode(xmlNode);
		cyntecXMLNode2.xmlm = xmlm;
		return cyntecXMLNode2;
	}

	public cyntecXMLNode CreateNode(string NodeName, string NodeText)
	{
		return new cyntecXMLNode(CreateNode(XmlNodeType.Element, NodeName, ""));
	}

	public cyntecXMLNode CreateNode2(string NodeName, string NodeText, string prefix)
	{
		if (xmlm != null && xmlm.HasNamespace(prefix))
		{
			return new cyntecXMLNode(CreateNode(XmlNodeType.Element, NodeName, xmlm.LookupNamespace(prefix)));
		}
		return new cyntecXMLNode(CreateNode(XmlNodeType.Element, NodeName, ""));
	}

	public static string[] GetFileList(string filePath, string searchKey)
	{
		return cyntecFile.GetFileList(filePath, searchKey);
	}

	public void AppendNodeInRecordSet(XmlNode nodeParent, DataTable dt, DataRow Row)
	{
		for (int i = 0; i < dt.Columns.Count; i++)
		{
			if (!dt.Columns[i].ColumnName.StartsWith("$"))
			{
				XmlNode xmlNode = CreateNode(XmlNodeType.Element, dt.Columns[i].ColumnName, "");
				xmlNode.InnerText = Row[i].ToString();
				nodeParent.AppendChild(xmlNode);
			}
		}
	}

	public cyntecXMLNode CreateRoot(string rootName)
	{
		return CreateRoot(rootName, isDeclaration: false);
	}

	public cyntecXMLNode CreateRoot(string rootName, bool isDeclaration)
	{
		XmlElement xmlElement = CreateElement(rootName);
		if (isDeclaration)
		{
			XmlDeclaration newChild = CreateXmlDeclaration("1.0", "utf-8", null);
			InsertBefore(newChild, base.DocumentElement);
		}
		AppendChild(xmlElement);
		return new cyntecXMLNode(xmlElement);
	}

	public override string ToString()
	{
		return OuterXml;
	}

	public XElement ToXElement()
	{
		return XElement.Parse(OuterXml);
	}
}
