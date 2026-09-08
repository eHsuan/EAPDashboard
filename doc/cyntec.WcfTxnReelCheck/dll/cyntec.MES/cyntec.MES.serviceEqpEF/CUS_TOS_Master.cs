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
public class CUS_TOS_Master : TOS_Master
{
	private decimal _usageQtyField;

	private decimal _totalUsageQtyField;

	[XmlElement(Order = 0)]
	public decimal _usageQty
	{
		get
		{
			return _usageQtyField;
		}
		set
		{
			_usageQtyField = value;
			RaisePropertyChanged("_usageQty");
		}
	}

	[XmlElement(Order = 1)]
	public decimal _totalUsageQty
	{
		get
		{
			return _totalUsageQtyField;
		}
		set
		{
			_totalUsageQtyField = value;
			RaisePropertyChanged("_totalUsageQty");
		}
	}
}
