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
public class Result_WO_Component_Add_Web : Basic_Result
{
	private WOComponent _modelWOComponentField;

	private WOComponent _modelWOComponent_OldField;

	private string _WOComponentConfirmIDField;

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
	public WOComponent _modelWOComponent_Old
	{
		get
		{
			return _modelWOComponent_OldField;
		}
		set
		{
			_modelWOComponent_OldField = value;
			RaisePropertyChanged("_modelWOComponent_Old");
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
}
