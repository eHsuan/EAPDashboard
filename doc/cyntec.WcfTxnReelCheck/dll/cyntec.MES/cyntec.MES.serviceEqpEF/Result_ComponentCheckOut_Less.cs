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
public class Result_ComponentCheckOut_Less : Basic_Result
{
	private WOComponent _model_WOComponentField;

	private WOComponent _model_WOComponent_OldField;

	private WOComponentConfirm _model_WOComponentConfirmField;

	private WOComponentConfirm _model_WOComponentConfirm_OldField;

	[XmlElement(Order = 0)]
	public WOComponent _model_WOComponent
	{
		get
		{
			return _model_WOComponentField;
		}
		set
		{
			_model_WOComponentField = value;
			RaisePropertyChanged("_model_WOComponent");
		}
	}

	[XmlElement(Order = 1)]
	public WOComponent _model_WOComponent_Old
	{
		get
		{
			return _model_WOComponent_OldField;
		}
		set
		{
			_model_WOComponent_OldField = value;
			RaisePropertyChanged("_model_WOComponent_Old");
		}
	}

	[XmlElement(Order = 2)]
	public WOComponentConfirm _model_WOComponentConfirm
	{
		get
		{
			return _model_WOComponentConfirmField;
		}
		set
		{
			_model_WOComponentConfirmField = value;
			RaisePropertyChanged("_model_WOComponentConfirm");
		}
	}

	[XmlElement(Order = 3)]
	public WOComponentConfirm _model_WOComponentConfirm_Old
	{
		get
		{
			return _model_WOComponentConfirm_OldField;
		}
		set
		{
			_model_WOComponentConfirm_OldField = value;
			RaisePropertyChanged("_model_WOComponentConfirm_Old");
		}
	}
}
