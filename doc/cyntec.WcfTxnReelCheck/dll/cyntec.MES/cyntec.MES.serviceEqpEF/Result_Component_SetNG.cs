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
public class Result_Component_SetNG : Basic_Result
{
	private WOComponent _modelWOComponentField;

	private WOComponent _modelWOComponent_oldField;

	private WOComponentConfirm _modelWOComponentConfirmField;

	private WOComponentConfirm _modelWOComponentConfirm_oldField;

	private WOComponentConfirmNGRecord _modelWOComponentConfirmNGRecordField;

	private WOComponentConfirmNGRecord _modelWOComponentConfirmNGRecord_oldField;

	private bool _haveNGRecordField;

	private bool _haveConfirmRecordField;

	[XmlElement(Order = 0)]
	public WOComponent _modelWOComponent
	{
		get
		{
			return _modelWOComponentField;
		}
		set
		{
			_modelWOComponentField = value;
			RaisePropertyChanged("_modelWOComponent");
		}
	}

	[XmlElement(Order = 1)]
	public WOComponent _modelWOComponent_old
	{
		get
		{
			return _modelWOComponent_oldField;
		}
		set
		{
			_modelWOComponent_oldField = value;
			RaisePropertyChanged("_modelWOComponent_old");
		}
	}

	[XmlElement(Order = 2)]
	public WOComponentConfirm _modelWOComponentConfirm
	{
		get
		{
			return _modelWOComponentConfirmField;
		}
		set
		{
			_modelWOComponentConfirmField = value;
			RaisePropertyChanged("_modelWOComponentConfirm");
		}
	}

	[XmlElement(Order = 3)]
	public WOComponentConfirm _modelWOComponentConfirm_old
	{
		get
		{
			return _modelWOComponentConfirm_oldField;
		}
		set
		{
			_modelWOComponentConfirm_oldField = value;
			RaisePropertyChanged("_modelWOComponentConfirm_old");
		}
	}

	[XmlElement(Order = 4)]
	public WOComponentConfirmNGRecord _modelWOComponentConfirmNGRecord
	{
		get
		{
			return _modelWOComponentConfirmNGRecordField;
		}
		set
		{
			_modelWOComponentConfirmNGRecordField = value;
			RaisePropertyChanged("_modelWOComponentConfirmNGRecord");
		}
	}

	[XmlElement(Order = 5)]
	public WOComponentConfirmNGRecord _modelWOComponentConfirmNGRecord_old
	{
		get
		{
			return _modelWOComponentConfirmNGRecord_oldField;
		}
		set
		{
			_modelWOComponentConfirmNGRecord_oldField = value;
			RaisePropertyChanged("_modelWOComponentConfirmNGRecord_old");
		}
	}

	[XmlElement(Order = 6)]
	public bool _haveNGRecord
	{
		get
		{
			return _haveNGRecordField;
		}
		set
		{
			_haveNGRecordField = value;
			RaisePropertyChanged("_haveNGRecord");
		}
	}

	[XmlElement(Order = 7)]
	public bool _haveConfirmRecord
	{
		get
		{
			return _haveConfirmRecordField;
		}
		set
		{
			_haveConfirmRecordField = value;
			RaisePropertyChanged("_haveConfirmRecord");
		}
	}
}
