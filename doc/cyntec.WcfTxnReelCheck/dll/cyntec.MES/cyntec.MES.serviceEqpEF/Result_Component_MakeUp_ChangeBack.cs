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
public class Result_Component_MakeUp_ChangeBack : Basic_Result
{
	private WOComponent _model_WOComponentField;

	private WOComponent _model_WOComponent_OldField;

	private WOComponentLabel _model_WOComponentLabelField;

	private WOComponentLabel _model_WOComponentLabel_OldField;

	private WOComponentConfirm _model_WOComponentConfirm_OldWONOField;

	private WOComponentConfirm _model_WOComponentConfirm_NewWONOField;

	private WOComponentMatAdd _model_WOComponentMatAdd_OldField;

	private WOComponentWOChange _model_WOComponentWOChange_OldField;

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
	public WOComponentLabel _model_WOComponentLabel
	{
		get
		{
			return _model_WOComponentLabelField;
		}
		set
		{
			_model_WOComponentLabelField = value;
			RaisePropertyChanged("_model_WOComponentLabel");
		}
	}

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
	public WOComponentConfirm _model_WOComponentConfirm_OldWONO
	{
		get
		{
			return _model_WOComponentConfirm_OldWONOField;
		}
		set
		{
			_model_WOComponentConfirm_OldWONOField = value;
			RaisePropertyChanged("_model_WOComponentConfirm_OldWONO");
		}
	}

	[XmlElement(Order = 5)]
	public WOComponentConfirm _model_WOComponentConfirm_NewWONO
	{
		get
		{
			return _model_WOComponentConfirm_NewWONOField;
		}
		set
		{
			_model_WOComponentConfirm_NewWONOField = value;
			RaisePropertyChanged("_model_WOComponentConfirm_NewWONO");
		}
	}

	[XmlElement(Order = 6)]
	public WOComponentMatAdd _model_WOComponentMatAdd_Old
	{
		get
		{
			return _model_WOComponentMatAdd_OldField;
		}
		set
		{
			_model_WOComponentMatAdd_OldField = value;
			RaisePropertyChanged("_model_WOComponentMatAdd_Old");
		}
	}

	[XmlElement(Order = 7)]
	public WOComponentWOChange _model_WOComponentWOChange_Old
	{
		get
		{
			return _model_WOComponentWOChange_OldField;
		}
		set
		{
			_model_WOComponentWOChange_OldField = value;
			RaisePropertyChanged("_model_WOComponentWOChange_Old");
		}
	}
}
