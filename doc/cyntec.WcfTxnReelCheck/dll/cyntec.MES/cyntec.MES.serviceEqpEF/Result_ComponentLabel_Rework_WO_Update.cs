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
public class Result_ComponentLabel_Rework_WO_Update : Basic_Result
{
	private WOComponentLabel _model_WOComponentLabel_OldField;

	[XmlElement(Order = 0)]
	public WOComponentLabel _model_WOComponentLabel_Old
	{
		get
		{
			return _model_WOComponentLabel_OldField;
		}
		set
		{
			_model_WOComponentLabel_OldField = value;
			RaisePropertyChanged("_model_WOComponentLabel_Old");
		}
	}
}
