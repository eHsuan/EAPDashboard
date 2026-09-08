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
public class Result_GetDecodeBarcode : Basic_Result
{
	private ReeIDInfo _modelField;

	[XmlElement(Order = 0)]
	public ReeIDInfo _model
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
