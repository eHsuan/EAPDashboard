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
public class Result_WOParmQry : Basic_Result
{
	private Param[] _modelListField;

	[XmlArray(Order = 0)]
	public Param[] _modelList
	{
		get
		{
			return _modelListField;
		}
		set
		{
			_modelListField = value;
			RaisePropertyChanged("_modelList");
		}
	}
}
