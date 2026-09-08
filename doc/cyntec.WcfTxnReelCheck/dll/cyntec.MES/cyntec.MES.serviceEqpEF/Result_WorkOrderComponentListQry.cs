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
public class Result_WorkOrderComponentListQry : Basic_Result
{
	private WOComponentInfo[] _listWOComponentInfoField;

	[XmlArray(Order = 0)]
	public WOComponentInfo[] _listWOComponentInfo
	{
		get
		{
			return _listWOComponentInfoField;
		}
		set
		{
			_listWOComponentInfoField = value;
			RaisePropertyChanged("_listWOComponentInfo");
		}
	}
}
