using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpPublic;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[XmlType(Namespace = "CyntecMES")]
public enum DataRowVersion
{
	Original,
	Current,
	Proposed,
	Default
}
