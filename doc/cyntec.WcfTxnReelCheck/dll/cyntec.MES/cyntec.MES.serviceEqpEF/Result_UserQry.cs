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
public class Result_UserQry : Basic_Result
{
	private string _userNameField;

	[XmlElement(Order = 0)]
	public string _userName
	{
		get
		{
			return _userNameField;
		}
		set
		{
			_userNameField = value;
			RaisePropertyChanged("_userName");
		}
	}
}
