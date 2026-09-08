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
public class Result_EDCPlanResultGet_FAI : Basic_Result
{
	private string _FAIPlan_ResultField;

	private string _FAIPlan_ErrorMessageField;

	[XmlElement(Order = 0)]
	public string _FAIPlan_Result
	{
		get
		{
			return _FAIPlan_ResultField;
		}
		set
		{
			_FAIPlan_ResultField = value;
			RaisePropertyChanged("_FAIPlan_Result");
		}
	}

	[XmlElement(Order = 1)]
	public string _FAIPlan_ErrorMessage
	{
		get
		{
			return _FAIPlan_ErrorMessageField;
		}
		set
		{
			_FAIPlan_ErrorMessageField = value;
			RaisePropertyChanged("_FAIPlan_ErrorMessage");
		}
	}
}
