using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[Flags]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[XmlType(Namespace = "CyntecMES")]
public enum SqlCompareOptions
{
	None = 1,
	IgnoreCase = 2,
	IgnoreNonSpace = 4,
	IgnoreKanaType = 8,
	IgnoreWidth = 0x10,
	BinarySort = 0x20,
	BinarySort2 = 0x40
}
