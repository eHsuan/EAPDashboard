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
public class CheckItem : INotifyPropertyChanged
{
	private string paramNameField;

	private string paramValueField;

	[XmlElement(Order = 0)]
	public string ParamName
	{
		get
		{
			return paramNameField;
		}
		set
		{
			paramNameField = value;
			RaisePropertyChanged("ParamName");
		}
	}

	[XmlElement(Order = 1)]
	public string ParamValue
	{
		get
		{
			return paramValueField;
		}
		set
		{
			paramValueField = value;
			RaisePropertyChanged("ParamValue");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
