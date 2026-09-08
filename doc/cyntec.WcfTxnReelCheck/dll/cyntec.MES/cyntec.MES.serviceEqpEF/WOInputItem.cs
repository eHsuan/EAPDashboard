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
public class WOInputItem : INotifyPropertyChanged
{
	private string _inputCodeField;

	private string _inputValueField;

	private bool _isInsertedField;

	[XmlElement(Order = 0)]
	public string _inputCode
	{
		get
		{
			return _inputCodeField;
		}
		set
		{
			_inputCodeField = value;
			RaisePropertyChanged("_inputCode");
		}
	}

	[XmlElement(Order = 1)]
	public string _inputValue
	{
		get
		{
			return _inputValueField;
		}
		set
		{
			_inputValueField = value;
			RaisePropertyChanged("_inputValue");
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
