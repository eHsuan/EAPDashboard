using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[XmlType(Namespace = "http://CyntecMES")]
public enum action
{
	INSERT,
	UPDATE,
	DELETE
}
