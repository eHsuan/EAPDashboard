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
public class Result_TOS_MouldQry : Basic_Result
{
	private CUS_TOS_Master _modelField;

	[XmlElement(Order = 0)]
	public CUS_TOS_Master _model
	{
		get
		{
			return _modelField;
		}
		set
		{
			_modelField = value;
			RaisePropertyChanged("_model");
		}
	}
}
