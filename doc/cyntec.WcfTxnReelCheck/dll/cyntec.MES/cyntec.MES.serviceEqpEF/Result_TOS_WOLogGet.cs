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
public class Result_TOS_WOLogGet : Basic_Result
{
	private DataTable dtTOS_WorkOrderLogField;

	private string _woNoField;

	private string _workCenterNoField;

	[XmlElement(Order = 0)]
	public DataTable dtTOS_WorkOrderLog
	{
		get
		{
			return dtTOS_WorkOrderLogField;
		}
		set
		{
			dtTOS_WorkOrderLogField = value;
			RaisePropertyChanged("dtTOS_WorkOrderLog");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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
}
