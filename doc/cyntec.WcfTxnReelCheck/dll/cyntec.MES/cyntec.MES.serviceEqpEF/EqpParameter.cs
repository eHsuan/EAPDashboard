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
public class EqpParameter : INotifyPropertyChanged
{
	private string _SysIDField;

	private string _GroupIDField;

	private int _Group_IndexField;

	private string _EqpNoField;

	private string _ParameterNameField;

	private string _ParameterValueField;

	private string _ParameterMinValueField;

	private string _ParameterMaxValueField;

	private DateTime _RefreshTimeField;

	[XmlElement(Order = 0)]
	public string _SysID
	{
		get
		{
			return _SysIDField;
		}
		set
		{
			_SysIDField = value;
			RaisePropertyChanged("_SysID");
		}
	}

	[XmlElement(Order = 1)]
	public string _GroupID
	{
		get
		{
			return _GroupIDField;
		}
		set
		{
			_GroupIDField = value;
			RaisePropertyChanged("_GroupID");
		}
	}

	[XmlElement(Order = 2)]
	public int _Group_Index
	{
		get
		{
			return _Group_IndexField;
		}
		set
		{
			_Group_IndexField = value;
			RaisePropertyChanged("_Group_Index");
		}
	}

	[XmlElement(Order = 3)]
	public string _EqpNo
	{
		get
		{
			return _EqpNoField;
		}
		set
		{
			_EqpNoField = value;
			RaisePropertyChanged("_EqpNo");
		}
	}

	[XmlElement(Order = 4)]
	public string _ParameterName
	{
		get
		{
			return _ParameterNameField;
		}
		set
		{
			_ParameterNameField = value;
			RaisePropertyChanged("_ParameterName");
		}
	}

	[XmlElement(Order = 5)]
	public string _ParameterValue
	{
		get
		{
			return _ParameterValueField;
		}
		set
		{
			_ParameterValueField = value;
			RaisePropertyChanged("_ParameterValue");
		}
	}

	[XmlElement(Order = 6)]
	public string _ParameterMinValue
	{
		get
		{
			return _ParameterMinValueField;
		}
		set
		{
			_ParameterMinValueField = value;
			RaisePropertyChanged("_ParameterMinValue");
		}
	}

	[XmlElement(Order = 7)]
	public string _ParameterMaxValue
	{
		get
		{
			return _ParameterMaxValueField;
		}
		set
		{
			_ParameterMaxValueField = value;
			RaisePropertyChanged("_ParameterMaxValue");
		}
	}

	[XmlElement(Order = 8)]
	public DateTime _RefreshTime
	{
		get
		{
			return _RefreshTimeField;
		}
		set
		{
			_RefreshTimeField = value;
			RaisePropertyChanged("_RefreshTime");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
