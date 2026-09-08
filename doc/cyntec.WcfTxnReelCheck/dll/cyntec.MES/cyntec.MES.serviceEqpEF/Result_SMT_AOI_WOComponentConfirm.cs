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
public class Result_SMT_AOI_WOComponentConfirm : Basic_Result
{
	private WOComponent _model_oldField;

	private WOComponent _model_newField;

	private string _WOComponentConfirmIDField;

	private WOComponentConfirm _modelWOComponentConfirm_oriField;

	private WOComponentConfirm _modelWOComponentConfirm_curField;

	private WOComponentConfirmNGRecord[] _modelWOComponentConfirmNGRecordListField;

	[XmlElement(Order = 0)]
	public WOComponent _model_old
	{
		get
		{
			return _model_oldField;
		}
		set
		{
			_model_oldField = value;
			RaisePropertyChanged("_model_old");
		}
	}

	[XmlElement(Order = 1)]
	public WOComponent _model_new
	{
		get
		{
			return _model_newField;
		}
		set
		{
			_model_newField = value;
			RaisePropertyChanged("_model_new");
		}
	}

	[XmlElement(Order = 2)]
	public string _WOComponentConfirmID
	{
		get
		{
			return _WOComponentConfirmIDField;
		}
		set
		{
			_WOComponentConfirmIDField = value;
			RaisePropertyChanged("_WOComponentConfirmID");
		}
	}

	[XmlElement(Order = 3)]
	public WOComponentConfirm _modelWOComponentConfirm_ori
	{
		get
		{
			return _modelWOComponentConfirm_oriField;
		}
		set
		{
			_modelWOComponentConfirm_oriField = value;
			RaisePropertyChanged("_modelWOComponentConfirm_ori");
		}
	}

	[XmlElement(Order = 4)]
	public WOComponentConfirm _modelWOComponentConfirm_cur
	{
		get
		{
			return _modelWOComponentConfirm_curField;
		}
		set
		{
			_modelWOComponentConfirm_curField = value;
			RaisePropertyChanged("_modelWOComponentConfirm_cur");
		}
	}

	[XmlArray(Order = 5)]
	public WOComponentConfirmNGRecord[] _modelWOComponentConfirmNGRecordList
	{
		get
		{
			return _modelWOComponentConfirmNGRecordListField;
		}
		set
		{
			_modelWOComponentConfirmNGRecordListField = value;
			RaisePropertyChanged("_modelWOComponentConfirmNGRecordList");
		}
	}
}
