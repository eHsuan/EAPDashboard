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
public class Result_TOS_WOQry : Basic_Result
{
	private string _woNoField;

	private string _workCenterNoField;

	private int _flowIDField;

	private string _machNoField;

	[XmlElement(Order = 0)]
	public string _woNo
	{
		get
		{
			return _woNoField;
		}
		set
		{
			_woNoField = value;
			RaisePropertyChanged("_woNo");
		}
	}

	[XmlElement(Order = 1)]
	public string _workCenterNo
	{
		get
		{
			return _workCenterNoField;
		}
		set
		{
			_workCenterNoField = value;
			RaisePropertyChanged("_workCenterNo");
		}
	}

	[XmlElement(Order = 2)]
	public int _flowID
	{
		get
		{
			return _flowIDField;
		}
		set
		{
			_flowIDField = value;
			RaisePropertyChanged("_flowID");
		}
	}

	[XmlElement(Order = 3)]
	public string _machNo
	{
		get
		{
			return _machNoField;
		}
		set
		{
			_machNoField = value;
			RaisePropertyChanged("_machNo");
		}
	}
}
