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
public class Input_EDC_RawData : INotifyPropertyChanged
{
	private string componentNoField;

	private string fieldCodeField;

	private double valueField;

	[XmlElement(Order = 0)]
	public string componentNo
	{
		get
		{
			return componentNoField;
		}
		set
		{
			componentNoField = value;
			RaisePropertyChanged("componentNo");
		}
	}

	[XmlElement(Order = 1)]
	public string fieldCode
	{
		get
		{
			return fieldCodeField;
		}
		set
		{
			fieldCodeField = value;
			RaisePropertyChanged("fieldCode");
		}
	}

	[XmlElement(Order = 2)]
	public double value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("value");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
