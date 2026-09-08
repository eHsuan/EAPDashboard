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
public class TOSItem : INotifyPropertyChanged
{
	private string toolNoField;

	private string usageQtyField;

	private string totalUsageQtyField;

	[XmlElement(Order = 0)]
	public string ToolNo
	{
		get
		{
			return toolNoField;
		}
		set
		{
			toolNoField = value;
			RaisePropertyChanged("ToolNo");
		}
	}

	[XmlElement(Order = 1)]
	public string UsageQty
	{
		get
		{
			return usageQtyField;
		}
		set
		{
			usageQtyField = value;
			RaisePropertyChanged("UsageQty");
		}
	}

	[XmlElement(Order = 2)]
	public string TotalUsageQty
	{
		get
		{
			return totalUsageQtyField;
		}
		set
		{
			totalUsageQtyField = value;
			RaisePropertyChanged("TotalUsageQty");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
