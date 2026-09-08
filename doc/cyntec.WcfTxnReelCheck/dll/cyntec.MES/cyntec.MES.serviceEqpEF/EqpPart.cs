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
public class EqpPart : INotifyPropertyChanged
{
	private string _EqpNoField;

	private string _PartTypeCodeField;

	private DateTime _PartStartDateField;

	private string _StatusCodeField;

	private decimal _UsageQtyField;

	private decimal _LimitQtyField;

	private DateTime _ModifyDateField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string _PartTypeCode
	{
		get
		{
			return _PartTypeCodeField;
		}
		set
		{
			_PartTypeCodeField = value;
			RaisePropertyChanged("_PartTypeCode");
		}
	}

	[XmlElement(Order = 2)]
	public DateTime _PartStartDate
	{
		get
		{
			return _PartStartDateField;
		}
		set
		{
			_PartStartDateField = value;
			RaisePropertyChanged("_PartStartDate");
		}
	}

	[XmlElement(Order = 3)]
	public string _StatusCode
	{
		get
		{
			return _StatusCodeField;
		}
		set
		{
			_StatusCodeField = value;
			RaisePropertyChanged("_StatusCode");
		}
	}

	[XmlElement(Order = 4)]
	public decimal _UsageQty
	{
		get
		{
			return _UsageQtyField;
		}
		set
		{
			_UsageQtyField = value;
			RaisePropertyChanged("_UsageQty");
		}
	}

	[XmlElement(Order = 5)]
	public decimal _LimitQty
	{
		get
		{
			return _LimitQtyField;
		}
		set
		{
			_LimitQtyField = value;
			RaisePropertyChanged("_LimitQty");
		}
	}

	[XmlElement(Order = 6)]
	public DateTime _ModifyDate
	{
		get
		{
			return _ModifyDateField;
		}
		set
		{
			_ModifyDateField = value;
			RaisePropertyChanged("_ModifyDate");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
