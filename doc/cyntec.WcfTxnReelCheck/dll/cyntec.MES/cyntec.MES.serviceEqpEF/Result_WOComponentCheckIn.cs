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
public class Result_WOComponentCheckIn : Basic_Result
{
	private WOComponent _model_oldField;

	private string _WOComponentConfirmIDField;

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
