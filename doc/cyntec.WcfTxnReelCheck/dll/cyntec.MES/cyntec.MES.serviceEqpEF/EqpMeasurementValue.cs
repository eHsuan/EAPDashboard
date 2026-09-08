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
public class EqpMeasurementValue : INotifyPropertyChanged
{
	private long _IndexIDField;

	private string _SysIDField;

	private string _ComponentNoField;

	private string _WOComponentConfirmIDField;

	private int _WOConfirmIDField;

	private MeasurementType _MeasurementTypeField;

	private string _MeasurementNameField;

	private decimal _MeasurementValueField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	[XmlElement(Order = 0)]
	public long _IndexID
	{
		get
		{
			return _IndexIDField;
		}
		set
		{
			_IndexIDField = value;
			RaisePropertyChanged("_IndexID");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
	public string _ComponentNo
	{
		get
		{
			return _ComponentNoField;
		}
		set
		{
			_ComponentNoField = value;
			RaisePropertyChanged("_ComponentNo");
		}
	}

	[XmlElement(Order = 3)]
	public string _WOComponentConfirmID
	{
		get
		{
			return _WOComponentConfirmIDField;
		}
		set
		{
			_WOComponentConfirmIDField = value;
			RaisePropertyChanged("_WOComponentConfirmID");
		}
	}

	[XmlElement(Order = 4)]
	public int _WOConfirmID
	{
		get
		{
			return _WOConfirmIDField;
		}
		set
		{
			_WOConfirmIDField = value;
			RaisePropertyChanged("_WOConfirmID");
		}
	}

	[XmlElement(Order = 5)]
	public MeasurementType _MeasurementType
	{
		get
		{
			return _MeasurementTypeField;
		}
		set
		{
			_MeasurementTypeField = value;
			RaisePropertyChanged("_MeasurementType");
		}
	}

	[XmlElement(Order = 6)]
	public string _MeasurementName
	{
		get
		{
			return _MeasurementNameField;
		}
		set
		{
			_MeasurementNameField = value;
			RaisePropertyChanged("_MeasurementName");
		}
	}

	[XmlElement(Order = 7)]
	public decimal _MeasurementValue
	{
		get
		{
			return _MeasurementValueField;
		}
		set
		{
			_MeasurementValueField = value;
			RaisePropertyChanged("_MeasurementValue");
		}
	}

	[XmlElement(Order = 8)]
	public DateTime _CreateDate
	{
		get
		{
			return _CreateDateField;
		}
		set
		{
			_CreateDateField = value;
			RaisePropertyChanged("_CreateDate");
		}
	}

	[XmlElement(Order = 9)]
	public string _CreateUser
	{
		get
		{
			return _CreateUserField;
		}
		set
		{
			_CreateUserField = value;
			RaisePropertyChanged("_CreateUser");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
