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
public class WOCarrierItem : INotifyPropertyChanged
{
	private string _carrierNoField;

	private bool _isInsertedField;

	[XmlElement(Order = 0)]
	public string _carrierNo
	{
		get
		{
			return _carrierNoField;
		}
		set
		{
			_carrierNoField = value;
			RaisePropertyChanged("_carrierNo");
		}
	}

	[XmlElement(Order = 1)]
	public bool _isInserted
	{
		get
		{
			return _isInsertedField;
		}
		set
		{
			_isInsertedField = value;
			RaisePropertyChanged("_isInserted");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
