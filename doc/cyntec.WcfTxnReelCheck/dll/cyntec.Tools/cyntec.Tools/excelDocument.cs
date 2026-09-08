using System;
using System.Data;
using System.Xml;
using cyntec.Base;

namespace cyntec.Tools;

public class excelDocument : cyntecXML
{
	public cyntecXMLNode xWorkbook;

	public cyntecXMLNode xExcelWorkbook;

	public cyntecXMLNode xStyles;

	public cyntecXMLNode xTable;

	public XmlAttribute ExpandedColumnCount;

	public XmlAttribute ExpandedRowCount;

	public XmlAttribute SheetName;

	public string sURL_SS;

	public excelDocument()
	{
		AppendChild(CreateProcessingInstruction("xml", "version=\"1.0\"  encoding=\"utf-8\""));
		AppendChild(CreateProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""));
		xWorkbook = CreateRoot("Workbook");
		xWorkbook.xElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
		XmlAttribute xmlAttribute = CreateAttribute("xmlns", "o", "http://www.w3.org/2000/xmlns/");
		string value = "urn:schemas-microsoft-com:office:office";
		xmlAttribute.Value = value;
		xWorkbook.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("xmlns", "x", "http://www.w3.org/2000/xmlns/");
		value = "urn:schemas-microsoft-com:office:excel";
		xmlAttribute.Value = value;
		xWorkbook.xElement.Attributes.Append(xmlAttribute);
		sURL_SS = "urn:schemas-microsoft-com:office:spreadsheet";
		xmlAttribute = CreateAttribute("xmlns", "ss", "http://www.w3.org/2000/xmlns/");
		value = sURL_SS;
		xmlAttribute.Value = value;
		xWorkbook.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("xmlns", "html", "http://www.w3.org/2000/xmlns/");
		value = "http://www.w3.org/TR/REC-html40";
		xmlAttribute.Value = value;
		xWorkbook.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode cyntecXMLNode2 = xWorkbook.AppendNode(this, "DocumentProperties", "");
		cyntecXMLNode2.xElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:office");
		cyntecXMLNode2.AppendNode(this, "Author", "CYNTEC CO., LTD.");
		cyntecXMLNode2.AppendNode(this, "LastAuthor", "CYNTEC CO., LTD.");
		cyntecXMLNode2.AppendNode(this, "Created", cyntecDateTime.NowToMSSQL());
		cyntecXMLNode2.AppendNode(this, "Version", "12.00");
		xExcelWorkbook = xWorkbook.AppendNode(this, "ExcelWorkbook", "");
		xExcelWorkbook.xElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:excel");
		xExcelWorkbook.AppendNode(this, "WindowHeight", "10005");
		xExcelWorkbook.AppendNode(this, "WindowWidth", "10005");
		xExcelWorkbook.AppendNode(this, "WindowTopX", "120");
		xExcelWorkbook.AppendNode(this, "WindowTopY", "135");
		xExcelWorkbook.AppendNode(this, "ProtectStructure", "False");
		xExcelWorkbook.AppendNode(this, "ProtectWindows", "False");
		xStyles = xWorkbook.AppendNode(this, "Styles", "");
		cyntecXMLNode cyntecXMLNode3 = xStyles.AppendNode(this, "Style", "");
		xmlAttribute = CreateAttribute("ss", "ID", sURL_SS);
		xmlAttribute.Value = "Default";
		cyntecXMLNode3.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("ss", "Name", sURL_SS);
		xmlAttribute.Value = "Normal";
		cyntecXMLNode3.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode cyntecXMLNode4 = cyntecXMLNode3.AppendNode(this, "Alignment", null);
		xmlAttribute = CreateAttribute("ss", "Vertical", sURL_SS);
		xmlAttribute.Value = "Bottom";
		cyntecXMLNode4.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode3.AppendNode(this, "Borders", null);
		cyntecXMLNode cyntecXMLNode5 = cyntecXMLNode3.AppendNode(this, "Font", null);
		xmlAttribute = CreateAttribute("ss", "FontName", sURL_SS);
		xmlAttribute.Value = "Arial";
		cyntecXMLNode5.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("ss", "Family", sURL_SS);
		xmlAttribute.Value = "Swiss";
		cyntecXMLNode5.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode3.AppendNode(this, "Interior", null);
		cyntecXMLNode3.AppendNode(this, "NumberFormat", null);
		cyntecXMLNode3.AppendNode(this, "Protection", null);
		cyntecXMLNode3 = xStyles.AppendNode(this, "Style", null);
		xmlAttribute = CreateAttribute("ss", "ID", sURL_SS);
		xmlAttribute.Value = "s62";
		cyntecXMLNode3.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode5 = cyntecXMLNode3.AppendNode(this, "Font", null);
		xmlAttribute = CreateAttribute("ss", "FontName", sURL_SS);
		xmlAttribute.Value = "Arial";
		cyntecXMLNode5.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("ss", "Family", sURL_SS);
		xmlAttribute.Value = "Swiss";
		cyntecXMLNode5.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("ss", "Bold", sURL_SS);
		xmlAttribute.Value = "1";
		cyntecXMLNode5.xElement.Attributes.Append(xmlAttribute);
		cyntecXMLNode cyntecXMLNode6 = xWorkbook.AppendNode(this, "Worksheet", null);
		SheetName = CreateAttribute("ss", "Name", sURL_SS);
		SheetName.Value = "Daily";
		cyntecXMLNode6.xElement.Attributes.Append(SheetName);
		xTable = cyntecXMLNode6.AppendNode(this, "Table", null);
		ExpandedColumnCount = CreateAttribute("ss", "ExpandedColumnCount", sURL_SS);
		ExpandedColumnCount.Value = "2";
		xTable.xElement.Attributes.Append(ExpandedColumnCount);
		ExpandedRowCount = CreateAttribute("ss", "ExpandedRowCount", sURL_SS);
		ExpandedRowCount.Value = "2";
		xTable.xElement.Attributes.Append(ExpandedRowCount);
		xmlAttribute = CreateAttribute("ss", "FullColumns", sURL_SS);
		xmlAttribute.Value = "1";
		xTable.xElement.Attributes.Append(xmlAttribute);
		xmlAttribute = CreateAttribute("ss", "FullRows", sURL_SS);
		xmlAttribute.Value = "1";
		xTable.xElement.Attributes.Append(xmlAttribute);
		xTable.xElement.SetAttribute("x:FullRows", "1");
		cyntecXMLNode cyntecXMLNode7 = cyntecXMLNode6.AppendNode(this, "WorksheetOptions", null);
		cyntecXMLNode7.xElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:excel");
		cyntecXMLNode cyntecXMLNode8 = cyntecXMLNode7.AppendNode(this, "Print", null);
		cyntecXMLNode cyntecXMLNode9 = cyntecXMLNode8.AppendNode(this, "ValidPrinterInfo", null);
		cyntecXMLNode cyntecXMLNode10 = cyntecXMLNode8.AppendNode(this, "PaperSizeIndex", "9");
		cyntecXMLNode cyntecXMLNode11 = cyntecXMLNode8.AppendNode(this, "HorizontalResolution", "1200");
		cyntecXMLNode cyntecXMLNode12 = cyntecXMLNode8.AppendNode(this, "VerticalResolution", "1200");
		cyntecXMLNode cyntecXMLNode13 = cyntecXMLNode7.AppendNode(this, "FreezePanes", null);
		cyntecXMLNode cyntecXMLNode14 = cyntecXMLNode7.AppendNode(this, "SplitHorizontal", "1");
		cyntecXMLNode cyntecXMLNode15 = cyntecXMLNode7.AppendNode(this, "TopRowBottomPane", "1");
		cyntecXMLNode cyntecXMLNode16 = cyntecXMLNode7.AppendNode(this, "ActivePane", "2");
		cyntecXMLNode cyntecXMLNode17 = cyntecXMLNode7.AppendNode(this, "Panes", null);
		cyntecXMLNode cyntecXMLNode18 = cyntecXMLNode17.AppendNode(this, "Pane", null);
		cyntecXMLNode cyntecXMLNode19 = cyntecXMLNode18.AppendNode(this, "Number", "3");
		cyntecXMLNode18 = cyntecXMLNode17.AppendNode(this, "Pane", null);
		cyntecXMLNode19 = cyntecXMLNode18.AppendNode(this, "Number", "2");
		cyntecXMLNode cyntecXMLNode20 = cyntecXMLNode18.AppendNode(this, "ActiveRow", "0");
		cyntecXMLNode cyntecXMLNode21 = cyntecXMLNode7.AppendNode(this, "ProtectObjects", "False");
		cyntecXMLNode cyntecXMLNode22 = cyntecXMLNode7.AppendNode(this, "ProtectScenarios", "False");
	}

	private string m_ConverType(string value, Type t)
	{
		if (value == "")
		{
			return "String";
		}
		if (t.Name == "Int32" || t.Name == "Double")
		{
			return "Number";
		}
		return "String";
	}

	public void AddTable(DataTable tb)
	{
		ExpandedColumnCount.Value = tb.Columns.Count.ToString();
		ExpandedRowCount.Value = (tb.Rows.Count + 1).ToString();
		SheetName.Value = "data";
		cyntecXMLNode cyntecXMLNode2 = xTable.AppendNode(this, "Row", null);
		foreach (DataColumn column in tb.Columns)
		{
			cyntecXMLNode cyntecXMLNode3 = cyntecXMLNode2.AppendNode(this, "Cell", null);
			XmlAttribute xmlAttribute = CreateAttribute("ss", "StyleID", sURL_SS);
			xmlAttribute.Value = "s62";
			cyntecXMLNode3.xElement.Attributes.Append(xmlAttribute);
			cyntecXMLNode cyntecXMLNode4 = cyntecXMLNode3.AppendNode(this, "Data", column.ColumnName);
			xmlAttribute = CreateAttribute("ss", "Type", sURL_SS);
			xmlAttribute.Value = "String";
			cyntecXMLNode4.xElement.Attributes.Append(xmlAttribute);
		}
		foreach (DataRow row in tb.Rows)
		{
			cyntecXMLNode2 = xTable.AppendNode(this, "Row", null);
			for (int i = 0; i < tb.Columns.Count; i++)
			{
				cyntecXMLNode cyntecXMLNode5 = cyntecXMLNode2.AppendNode(this, "Cell", null);
				cyntecXMLNode cyntecXMLNode6 = cyntecXMLNode5.AppendNode(this, "Data", row[i].ToString());
				XmlAttribute xmlAttribute2 = CreateAttribute("ss", "Type", sURL_SS);
				string value = (xmlAttribute2.Value = m_ConverType(row[i].ToString(), tb.Columns[i].DataType));
				xmlAttribute2.Value = value;
				cyntecXMLNode6.xElement.Attributes.Append(xmlAttribute2);
			}
		}
	}

	public static void Test()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateProcessingInstruction("xml", "version=\"1.0\"  encoding=\"utf-8\""));
		xmlDocument.AppendChild(xmlDocument.CreateProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""));
		XmlElement xmlElement = xmlDocument.CreateElement("Workbook");
		xmlElement.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
		XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("xmlns", "o", "http://www.w3.org/2000/xmlns/");
		string value = "urn:schemas-microsoft-com:office:office";
		xmlAttribute.Value = value;
		xmlElement.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("xmlns", "x", "http://www.w3.org/2000/xmlns/");
		value = "urn:schemas-microsoft-com:office:excel";
		xmlAttribute.Value = value;
		xmlElement.Attributes.Append(xmlAttribute);
		string text = "urn:schemas-microsoft-com:office:spreadsheet";
		xmlAttribute = xmlDocument.CreateAttribute("xmlns", "ss", "http://www.w3.org/2000/xmlns/");
		value = text;
		xmlAttribute.Value = value;
		xmlElement.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("xmlns", "html", "http://www.w3.org/2000/xmlns/");
		value = "http://www.w3.org/TR/REC-html40";
		xmlAttribute.Value = value;
		xmlElement.Attributes.Append(xmlAttribute);
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("DocumentProperties");
		xmlElement2.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:office");
		XmlElement xmlElement3 = xmlDocument.CreateElement("Author");
		xmlElement3.InnerText = "Stark Technology Inc.";
		xmlElement2.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("LastAuthor");
		xmlElement4.InnerText = "Stark Technology Inc.";
		xmlElement2.AppendChild(xmlElement4);
		XmlElement xmlElement5 = xmlDocument.CreateElement("Created");
		xmlElement5.InnerText = "2012-03-21T03:47:32Z";
		xmlElement2.AppendChild(xmlElement5);
		XmlElement xmlElement6 = xmlDocument.CreateElement("Version");
		xmlElement6.InnerText = "12.00";
		xmlElement2.AppendChild(xmlElement6);
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement7 = xmlDocument.CreateElement("ExcelWorkbook");
		xmlElement7.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:excel");
		XmlElement xmlElement8 = xmlDocument.CreateElement("WindowHeight");
		xmlElement8.InnerText = "10005";
		xmlElement7.AppendChild(xmlElement8);
		XmlElement xmlElement9 = xmlDocument.CreateElement("WindowWidth");
		xmlElement9.InnerText = "10005";
		xmlElement7.AppendChild(xmlElement9);
		XmlElement xmlElement10 = xmlDocument.CreateElement("WindowTopX");
		xmlElement10.InnerText = "120";
		xmlElement7.AppendChild(xmlElement10);
		XmlElement xmlElement11 = xmlDocument.CreateElement("WindowTopY");
		xmlElement11.InnerText = "135";
		xmlElement7.AppendChild(xmlElement11);
		XmlElement xmlElement12 = xmlDocument.CreateElement("ProtectStructure");
		xmlElement12.InnerText = "False";
		xmlElement7.AppendChild(xmlElement12);
		XmlElement xmlElement13 = xmlDocument.CreateElement("ProtectWindows");
		xmlElement13.InnerText = "False";
		xmlElement7.AppendChild(xmlElement13);
		xmlElement.AppendChild(xmlElement7);
		XmlElement xmlElement14 = xmlDocument.CreateElement("Styles");
		XmlElement xmlElement15 = xmlDocument.CreateElement("Style");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "ID", text);
		xmlAttribute.Value = "Default";
		xmlElement15.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Name", text);
		xmlAttribute.Value = "Normal";
		xmlElement15.Attributes.Append(xmlAttribute);
		XmlElement xmlElement16 = xmlDocument.CreateElement("Alignment");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Vertical", text);
		xmlAttribute.Value = "Bottom";
		xmlElement16.Attributes.Append(xmlAttribute);
		xmlElement15.AppendChild(xmlElement16);
		XmlElement newChild = xmlDocument.CreateElement("Borders");
		xmlElement15.AppendChild(newChild);
		XmlElement xmlElement17 = xmlDocument.CreateElement("Font");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "FontName", text);
		xmlAttribute.Value = "Arial";
		xmlElement17.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Family", text);
		xmlAttribute.Value = "Swiss";
		xmlElement17.Attributes.Append(xmlAttribute);
		xmlElement15.AppendChild(xmlElement17);
		XmlElement newChild2 = xmlDocument.CreateElement("Interior");
		xmlElement15.AppendChild(newChild2);
		XmlElement newChild3 = xmlDocument.CreateElement("NumberFormat");
		xmlElement15.AppendChild(newChild3);
		XmlElement newChild4 = xmlDocument.CreateElement("Protection");
		xmlElement15.AppendChild(newChild4);
		xmlElement14.AppendChild(xmlElement15);
		xmlElement15 = xmlDocument.CreateElement("Style");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "ID", text);
		xmlAttribute.Value = "s62";
		xmlElement15.Attributes.Append(xmlAttribute);
		xmlElement17 = xmlDocument.CreateElement("Font");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "FontName", text);
		xmlAttribute.Value = "Arial";
		xmlElement17.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Family", text);
		xmlAttribute.Value = "Swiss";
		xmlElement17.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Bold", text);
		xmlAttribute.Value = "1";
		xmlElement17.Attributes.Append(xmlAttribute);
		xmlElement15.AppendChild(xmlElement17);
		xmlElement14.AppendChild(xmlElement15);
		xmlElement.AppendChild(xmlElement14);
		XmlElement xmlElement18 = xmlDocument.CreateElement("Worksheet");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Name", text);
		xmlAttribute.Value = "Daily";
		xmlElement18.Attributes.Append(xmlAttribute);
		XmlElement xmlElement19 = xmlDocument.CreateElement("Table");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "ExpandedColumnCount", text);
		xmlAttribute.Value = "2";
		xmlElement19.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "ExpandedRowCount", text);
		xmlAttribute.Value = "2";
		xmlElement19.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "FullColumns", text);
		xmlAttribute.Value = "1";
		xmlElement19.Attributes.Append(xmlAttribute);
		xmlAttribute = xmlDocument.CreateAttribute("ss", "FullRows", text);
		xmlAttribute.Value = "1";
		xmlElement19.Attributes.Append(xmlAttribute);
		xmlElement19.SetAttribute("x:FullRows", "1");
		XmlElement xmlElement20 = xmlDocument.CreateElement("Row");
		XmlElement xmlElement21 = xmlDocument.CreateElement("Cell");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "StyleID", text);
		xmlAttribute.Value = "s62";
		xmlElement21.Attributes.Append(xmlAttribute);
		XmlElement xmlElement22 = xmlDocument.CreateElement("Data");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Type", text);
		xmlAttribute.Value = "String";
		xmlElement22.Attributes.Append(xmlAttribute);
		xmlElement22.InnerText = "FACTORY";
		xmlElement21.AppendChild(xmlElement22);
		xmlElement20.AppendChild(xmlElement21);
		xmlElement21 = xmlDocument.CreateElement("Cell");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "StyleID", text);
		xmlAttribute.Value = "s62";
		xmlElement21.Attributes.Append(xmlAttribute);
		xmlElement22 = xmlDocument.CreateElement("Data");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Type", text);
		xmlAttribute.Value = "String";
		xmlElement22.Attributes.Append(xmlAttribute);
		xmlElement22.InnerText = "LOT_ID";
		xmlElement21.AppendChild(xmlElement22);
		xmlElement20.AppendChild(xmlElement21);
		xmlElement19.AppendChild(xmlElement20);
		xmlElement20 = xmlDocument.CreateElement("Row");
		xmlElement21 = xmlDocument.CreateElement("Cell");
		xmlElement22 = xmlDocument.CreateElement("Data");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Type", text);
		xmlAttribute.Value = "String";
		xmlElement22.Attributes.Append(xmlAttribute);
		xmlElement22.InnerText = "BUMP_SCT3";
		xmlElement21.AppendChild(xmlElement22);
		xmlElement20.AppendChild(xmlElement21);
		xmlElement21 = xmlDocument.CreateElement("Cell");
		xmlElement22 = xmlDocument.CreateElement("Data");
		xmlAttribute = xmlDocument.CreateAttribute("ss", "Type", text);
		xmlAttribute.Value = "String";
		xmlElement22.Attributes.Append(xmlAttribute);
		xmlElement22.InnerText = "AP37H12.00";
		xmlElement21.AppendChild(xmlElement22);
		xmlElement20.AppendChild(xmlElement21);
		xmlElement19.AppendChild(xmlElement20);
		xmlElement18.AppendChild(xmlElement19);
		XmlElement xmlElement23 = xmlDocument.CreateElement("WorksheetOptions");
		xmlElement23.SetAttribute("xmlns", "urn:schemas-microsoft-com:office:excel");
		XmlElement xmlElement24 = xmlDocument.CreateElement("Print");
		XmlElement newChild5 = xmlDocument.CreateElement("ValidPrinterInfo");
		xmlElement24.AppendChild(newChild5);
		XmlElement xmlElement25 = xmlDocument.CreateElement("PaperSizeIndex");
		xmlElement25.InnerText = "9";
		xmlElement24.AppendChild(xmlElement25);
		XmlElement xmlElement26 = xmlDocument.CreateElement("HorizontalResolution");
		xmlElement26.InnerText = "1200";
		xmlElement24.AppendChild(xmlElement26);
		XmlElement xmlElement27 = xmlDocument.CreateElement("VerticalResolution");
		xmlElement27.InnerText = "1200";
		xmlElement24.AppendChild(xmlElement27);
		xmlElement23.AppendChild(xmlElement24);
		XmlElement newChild6 = xmlDocument.CreateElement("FreezePanes");
		xmlElement23.AppendChild(newChild6);
		XmlElement xmlElement28 = xmlDocument.CreateElement("SplitHorizontal");
		xmlElement28.InnerText = "1";
		xmlElement23.AppendChild(xmlElement28);
		XmlElement xmlElement29 = xmlDocument.CreateElement("TopRowBottomPane");
		xmlElement29.InnerText = "1";
		xmlElement23.AppendChild(xmlElement29);
		XmlElement xmlElement30 = xmlDocument.CreateElement("ActivePane");
		xmlElement30.InnerText = "2";
		xmlElement23.AppendChild(xmlElement30);
		XmlElement xmlElement31 = xmlDocument.CreateElement("Panes");
		XmlElement xmlElement32 = xmlDocument.CreateElement("Pane");
		XmlElement xmlElement33 = xmlDocument.CreateElement("Number");
		xmlElement33.InnerText = "3";
		xmlElement32.AppendChild(xmlElement33);
		xmlElement31.AppendChild(xmlElement32);
		xmlElement32 = xmlDocument.CreateElement("Pane");
		xmlElement33 = xmlDocument.CreateElement("Number");
		xmlElement33.InnerText = "2";
		xmlElement32.AppendChild(xmlElement33);
		XmlElement xmlElement34 = xmlDocument.CreateElement("ActiveRow");
		xmlElement34.InnerText = "0";
		xmlElement32.AppendChild(xmlElement34);
		xmlElement31.AppendChild(xmlElement32);
		xmlElement23.AppendChild(xmlElement31);
		XmlElement xmlElement35 = xmlDocument.CreateElement("ProtectObjects");
		xmlElement35.InnerText = "False";
		xmlElement23.AppendChild(xmlElement35);
		XmlElement xmlElement36 = xmlDocument.CreateElement("ProtectScenarios");
		xmlElement36.InnerText = "False";
		xmlElement23.AppendChild(xmlElement36);
		xmlElement18.AppendChild(xmlElement23);
		xmlElement.AppendChild(xmlElement18);
		xmlDocument.Save("C:\\Temp\\test3.xml");
	}
}
