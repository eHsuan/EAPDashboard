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
public class NGSet : INotifyPropertyChanged
{
	private string nGCodeField;

	private string nGNameField;

	[XmlElement(Order = 0)]
	public string NGCode
	{
		get
		{
			return nGCodeField;
		}
		set
		{
			nGCodeField = value;
			RaisePropertyChanged("NGCode");
		}
	}

	[XmlElement(Order = 1)]
	public string NGName
	{
		get
		{
			return nGNameField;
		}
		set
		{
			nGNameField = value;
			RaisePropertyChanged("NGName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
