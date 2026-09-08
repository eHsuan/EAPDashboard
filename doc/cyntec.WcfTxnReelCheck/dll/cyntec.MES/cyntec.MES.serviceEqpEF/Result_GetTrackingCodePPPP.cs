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
public class Result_GetTrackingCodePPPP : Basic_Result
{
	private string _PPPPField;

	[XmlElement(Order = 0)]
	public string _PPPP
	{
		get
		{
			return _PPPPField;
		}
		set
		{
			_PPPPField = value;
			RaisePropertyChanged("_PPPP");
		}
	}
}
