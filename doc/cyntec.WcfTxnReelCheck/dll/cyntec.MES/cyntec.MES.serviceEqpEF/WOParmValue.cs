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
public class WOParmValue : INotifyPropertyChanged
{
	private bool _isInsertedField;

	private string _parmCodeField;

	private string _parmValueField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string _parmCode
	{
		get
		{
			return _parmCodeField;
		}
		set
		{
			_parmCodeField = value;
			RaisePropertyChanged("_parmCode");
		}
	}

	[XmlElement(Order = 2)]
	public string _parmValue
	{
		get
		{
			return _parmValueField;
		}
		set
		{
			_parmValueField = value;
			RaisePropertyChanged("_parmValue");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
