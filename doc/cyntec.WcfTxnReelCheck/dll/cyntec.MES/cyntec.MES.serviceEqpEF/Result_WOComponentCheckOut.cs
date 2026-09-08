using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cyntec.MES.serviceEqpEF;

[Serializable]
[GeneratedCode("System.Xml", "4.8.9032.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CyntecMES")]
public class Result_WOComponentCheckOut : Basic_Result
{
	private int _wooutqtyField;

	private int _wongqtyField;

	private bool _needConfirmField;

	private WOComponent _model_WOComponent_oldField;

	private WOComponentConfirm _model_WOComponentConfirm_oldField;

	private DataSet _dicSPCDataField;

	[XmlElement(Order = 0)]
	public int _wooutqty
	{
		get
		{
			return _wooutqtyField;
		}
		set
		{
			_wooutqtyField = value;
			RaisePropertyChanged("_wooutqty");
		}
	}

	[XmlElement(Order = 1)]
	public int _wongqty
	{
		get
		{
			return _wongqtyField;
		}
		set
		{
			_wongqtyField = value;
			RaisePropertyChanged("_wongqty");
		}
	}

	[XmlElement(Order = 2)]
	public bool _needConfirm
	{
		get
		{
			return _needConfirmField;
		}
		set
		{
			_needConfirmField = value;
			RaisePropertyChanged("_needConfirm");
		}
	}

	[XmlElement(Order = 3)]
	public WOComponent _model_WOComponent_old
	{
		get
		{
			return _model_WOComponent_oldField;
		}
		set
		{
			_model_WOComponent_oldField = value;
			RaisePropertyChanged("_model_WOComponent_old");
		}
	}

	[XmlElement(Order = 4)]
	public WOComponentConfirm _model_WOComponentConfirm_old
	{
		get
		{
			return _model_WOComponentConfirm_oldField;
		}
		set
		{
			_model_WOComponentConfirm_oldField = value;
			RaisePropertyChanged("_model_WOComponentConfirm_old");
		}
	}

	[XmlElement(Order = 5)]
	public DataSet _dicSPCData
	{
		get
		{
			return _dicSPCDataField;
		}
		set
		{
			_dicSPCDataField = value;
			RaisePropertyChanged("_dicSPCData");
		}
	}
}
