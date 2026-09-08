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
public class EqpStatus : INotifyPropertyChanged
{
	private string _EqpNoField;

	private string _StatusCodeField;

	private decimal? _OutPutQtyField;

	private decimal? _Target_OutPutQtyField;

	private string _CycleTimeField;

	private string _WONOField;

	private string _EmpNoField;

	private string _ErrorCodeField;

	private string _AvailabilityField;

	private DateTime _RefreshTimeField;

	private decimal? _TotalOutPutQtyField;

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

	[XmlElement(IsNullable = true, Order = 2)]
	public decimal? _OutPutQty
	{
		get
		{
			return _OutPutQtyField;
		}
		set
		{
			_OutPutQtyField = value;
			RaisePropertyChanged("_OutPutQty");
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public decimal? _Target_OutPutQty
	{
		get
		{
			return _Target_OutPutQtyField;
		}
		set
		{
			_Target_OutPutQtyField = value;
			RaisePropertyChanged("_Target_OutPutQty");
		}
	}

	[XmlElement(Order = 4)]
	public string _CycleTime
	{
		get
		{
			return _CycleTimeField;
		}
		set
		{
			_CycleTimeField = value;
			RaisePropertyChanged("_CycleTime");
		}
	}

	[XmlElement(Order = 5)]
	public string _WONO
	{
		get
		{
			return _WONOField;
		}
		set
		{
			_WONOField = value;
			RaisePropertyChanged("_WONO");
		}
	}

	[XmlElement(Order = 6)]
	public string _EmpNo
	{
		get
		{
			return _EmpNoField;
		}
		set
		{
			_EmpNoField = value;
			RaisePropertyChanged("_EmpNo");
		}
	}

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
	public string _Availability
	{
		get
		{
			return _AvailabilityField;
		}
		set
		{
			_AvailabilityField = value;
			RaisePropertyChanged("_Availability");
		}
	}

	[XmlElement(Order = 9)]
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

	[XmlElement(IsNullable = true, Order = 10)]
	public decimal? _TotalOutPutQty
	{
		get
		{
			return _TotalOutPutQtyField;
		}
		set
		{
			_TotalOutPutQtyField = value;
			RaisePropertyChanged("_TotalOutPutQty");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
