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
public class Mach_Info : INotifyPropertyChanged
{
	private string _machNoField;

	private string _machAliasField;

	private string _workCenterNoField;

	private string _lineCodeField;

	[XmlElement(Order = 0)]
	public string _machNo
	{
		get
		{
			return _machNoField;
		}
		set
		{
			_machNoField = value;
			RaisePropertyChanged("_machNo");
		}
	}

	[XmlElement(Order = 1)]
	public string _machAlias
	{
		get
		{
			return _machAliasField;
		}
		set
		{
			_machAliasField = value;
			RaisePropertyChanged("_machAlias");
		}
	}

	[XmlElement(Order = 2)]
	public string _workCenterNo
	{
		get
		{
			return _workCenterNoField;
		}
		set
		{
			_workCenterNoField = value;
			RaisePropertyChanged("_workCenterNo");
		}
	}

	[XmlElement(Order = 3)]
	public string _lineCode
	{
		get
		{
			return _lineCodeField;
		}
		set
		{
			_lineCodeField = value;
			RaisePropertyChanged("_lineCode");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
