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
public class Result_WOComponentLabel_GetModel : Basic_Result
{
	private int _resultCountField;

	private WOComponentLabel _modelField;

	[XmlElement(Order = 0)]
	public int _resultCount
	{
		get
		{
			return _resultCountField;
		}
		set
		{
			_resultCountField = value;
			RaisePropertyChanged("_resultCount");
		}
	}

	[XmlElement(Order = 1)]
	public WOComponentLabel _model
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
