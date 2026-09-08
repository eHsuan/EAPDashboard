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
public class EqpCheckItem : INotifyPropertyChanged
{
	private string _itemNameField;

	private string _itemValueField;

	[XmlElement(Order = 0)]
	public string _itemName
	{
		get
		{
			return _itemNameField;
		}
		set
		{
			_itemNameField = value;
			RaisePropertyChanged("_itemName");
		}
	}

	[XmlElement(Order = 1)]
	public string _itemValue
	{
		get
		{
			return _itemValueField;
		}
		set
		{
			_itemValueField = value;
			RaisePropertyChanged("_itemValue");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
