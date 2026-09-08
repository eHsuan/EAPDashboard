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
public class MatItem : INotifyPropertyChanged
{
	private string _matCodeField;

	private string _matValueField;

	private bool _isInsertedField;

	[XmlElement(Order = 0)]
	public string _matCode
	{
		get
		{
			return _matCodeField;
		}
		set
		{
			_matCodeField = value;
			RaisePropertyChanged("_matCode");
		}
	}

	[XmlElement(Order = 1)]
	public string _matValue
	{
		get
		{
			return _matValueField;
		}
		set
		{
			_matValueField = value;
			RaisePropertyChanged("_matValue");
		}
	}

	[XmlElement(Order = 2)]
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
