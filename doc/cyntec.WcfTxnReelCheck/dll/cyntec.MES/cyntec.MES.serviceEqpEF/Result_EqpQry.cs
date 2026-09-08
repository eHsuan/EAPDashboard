using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Result_EqpQry : Basic_Result
{
	private Mach_Info[] _listMach_InfoField;

	[XmlArray(Order = 0)]
	public Mach_Info[] _listMach_Info
	{
		get
		{
			return _listMach_InfoField;
		}
		set
		{
			_listMach_InfoField = value;
			RaisePropertyChanged("_listMach_Info");
		}
	}
}
