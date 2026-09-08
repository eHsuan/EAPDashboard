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
public class WOComponentInfo : WOComponent
{
	private string _NGWOComponentConfirmIDField;

	private string _NGWorkCenterNoField;

	private string _NGCodeField;

	private string _NGNameField;

	private string _LabelIDField;

	private string _LabelContent_QRField;

	[XmlElement(Order = 0)]
	public string _NGWOComponentConfirmID
	{
		get
		{
			return _NGWOComponentConfirmIDField;
		}
		set
		{
			_NGWOComponentConfirmIDField = value;
			RaisePropertyChanged("_NGWOComponentConfirmID");
		}
	}

	[XmlElement(Order = 1)]
	public string _NGWorkCenterNo
	{
		get
		{
			return _NGWorkCenterNoField;
		}
		set
		{
			_NGWorkCenterNoField = value;
			RaisePropertyChanged("_NGWorkCenterNo");
		}
	}

	[XmlElement(Order = 2)]
	public string _NGCode
	{
		get
		{
			return _NGCodeField;
		}
		set
		{
			_NGCodeField = value;
			RaisePropertyChanged("_NGCode");
		}
	}

	[XmlElement(Order = 3)]
	public string _NGName
	{
		get
		{
			return _NGNameField;
		}
		set
		{
			_NGNameField = value;
			RaisePropertyChanged("_NGName");
		}
	}

	[XmlElement(Order = 4)]
	public string _LabelID
	{
		get
		{
			return _LabelIDField;
		}
		set
		{
			_LabelIDField = value;
			RaisePropertyChanged("_LabelID");
		}
	}

	[XmlElement(Order = 5)]
	public string _LabelContent_QR
	{
		get
		{
			return _LabelContent_QRField;
		}
		set
		{
			_LabelContent_QRField = value;
			RaisePropertyChanged("_LabelContent_QR");
		}
	}
}
