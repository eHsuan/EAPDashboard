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
public class EqpErrorCode : INotifyPropertyChanged
{
	private string _MachNoField;

	private string _ErrorCodeField;

	private string _ErrorNameField;

	private string _ErrorKeyNameField;

	[XmlElement(Order = 0)]
	public string _MachNo
	{
		get
		{
			return _MachNoField;
		}
		set
		{
			_MachNoField = value;
			RaisePropertyChanged("_MachNo");
		}
	}

	[XmlElement(Order = 1)]
	public string _ErrorCode
	{
		get
		{
			return _ErrorCodeField;
		}
		set
		{
			_ErrorCodeField = value;
			RaisePropertyChanged("_ErrorCode");
		}
	}

	[XmlElement(Order = 2)]
	public string _ErrorName
	{
		get
		{
			return _ErrorNameField;
		}
		set
		{
			_ErrorNameField = value;
			RaisePropertyChanged("_ErrorName");
		}
	}

	[XmlElement(Order = 3)]
	public string _ErrorKeyName
	{
		get
		{
			return _ErrorKeyNameField;
		}
		set
		{
			_ErrorKeyNameField = value;
			RaisePropertyChanged("_ErrorKeyName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
