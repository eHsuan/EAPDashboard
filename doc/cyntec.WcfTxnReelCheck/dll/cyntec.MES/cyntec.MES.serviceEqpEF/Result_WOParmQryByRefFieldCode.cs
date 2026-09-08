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
public class Result_WOParmQryByRefFieldCode : Basic_Result
{
	private ParamSet _resultItemField;

	[XmlElement(Order = 0)]
	public ParamSet _resultItem
	{
		get
		{
			return _resultItemField;
		}
		set
		{
			_resultItemField = value;
			RaisePropertyChanged("_resultItem");
		}
	}
}
