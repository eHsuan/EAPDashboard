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
public class Result_ComponentCheckIn_QReturn : Basic_Result
{
	private WOComponentConfirm[] _listWOComponentConfirmField;

	private WOComponentConfirm[] _listWOComponentConfirm_OldField;

	[XmlArray(Order = 0)]
	public WOComponentConfirm[] _listWOComponentConfirm
	{
		get
		{
			return _listWOComponentConfirmField;
		}
		set
		{
			_listWOComponentConfirmField = value;
			RaisePropertyChanged("_listWOComponentConfirm");
		}
	}

	[XmlArray(Order = 1)]
	public WOComponentConfirm[] _listWOComponentConfirm_Old
	{
		get
		{
			return _listWOComponentConfirm_OldField;
		}
		set
		{
			_listWOComponentConfirm_OldField = value;
			RaisePropertyChanged("_listWOComponentConfirm_Old");
		}
	}
}
