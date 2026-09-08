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
public class Result_ComponentCheckIn_Cancel : Basic_Result
{
	private WOComponent[] _listWOComponentField;

	private WOComponent[] _listWOComponent_OldField;

	private WOComponentConfirm[] _listWOComponentConfirmField;

	private WOComponentConfirm[] _listWOComponentConfirm_OldField;

	[XmlArray(Order = 0)]
	public WOComponent[] _listWOComponent
	{
		get
		{
			return _listWOComponentField;
		}
		set
		{
			_listWOComponentField = value;
			RaisePropertyChanged("_listWOComponent");
		}
	}

	[XmlArray(Order = 1)]
	public WOComponent[] _listWOComponent_Old
	{
		get
		{
			return _listWOComponent_OldField;
		}
		set
		{
			_listWOComponent_OldField = value;
			RaisePropertyChanged("_listWOComponent_Old");
		}
	}

	[XmlArray(Order = 2)]
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

	[XmlArray(Order = 3)]
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
