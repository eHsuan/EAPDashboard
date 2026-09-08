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
public class WOComponentConfirm : INotifyPropertyChanged
{
	private string _WOComponentConfirmIDField;

	private string _SerialNoField;

	private string _MatGroupNoField;

	private string _CarrierSNField;

	private string _WONoField;

	private int _FlowIDField;

	private string _WorkCenterNoField;

	private string _MachNoField;

	private int _InQtyField;

	private DateTime _InTimeField;

	private string _InUserField;

	private int? _OutQtyField;

	private int? _NGQtyField;

	private DateTime? _OutTimeField;

	private string _OutUserField;

	private string _NGCodeField;

	private int _ConfirmIDField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime _ChangeDateField;

	private string _ChangeUserField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
	public string _SerialNo
	{
		get
		{
			return _SerialNoField;
		}
		set
		{
			_SerialNoField = value;
			RaisePropertyChanged("_SerialNo");
		}
	}

	[XmlElement(Order = 2)]
	public string _MatGroupNo
	{
		get
		{
			return _MatGroupNoField;
		}
		set
		{
			_MatGroupNoField = value;
			RaisePropertyChanged("_MatGroupNo");
		}
	}

	[XmlElement(Order = 3)]
	public string _CarrierSN
	{
		get
		{
			return _CarrierSNField;
		}
		set
		{
			_CarrierSNField = value;
			RaisePropertyChanged("_CarrierSN");
		}
	}

	[XmlElement(Order = 4)]
	public string _WONo
	{
		get
		{
			return _WONoField;
		}
		set
		{
			_WONoField = value;
			RaisePropertyChanged("_WONo");
		}
	}

	[XmlElement(Order = 5)]
	public int _FlowID
	{
		get
		{
			return _FlowIDField;
		}
		set
		{
			_FlowIDField = value;
			RaisePropertyChanged("_FlowID");
		}
	}

	[XmlElement(Order = 6)]
	public string _WorkCenterNo
	{
		get
		{
			return _WorkCenterNoField;
		}
		set
		{
			_WorkCenterNoField = value;
			RaisePropertyChanged("_WorkCenterNo");
		}
	}

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
	public int _InQty
	{
		get
		{
			return _InQtyField;
		}
		set
		{
			_InQtyField = value;
			RaisePropertyChanged("_InQty");
		}
	}

	[XmlElement(Order = 9)]
	public DateTime _InTime
	{
		get
		{
			return _InTimeField;
		}
		set
		{
			_InTimeField = value;
			RaisePropertyChanged("_InTime");
		}
	}

	[XmlElement(Order = 10)]
	public string _InUser
	{
		get
		{
			return _InUserField;
		}
		set
		{
			_InUserField = value;
			RaisePropertyChanged("_InUser");
		}
	}

	[XmlElement(IsNullable = true, Order = 11)]
	public int? _OutQty
	{
		get
		{
			return _OutQtyField;
		}
		set
		{
			_OutQtyField = value;
			RaisePropertyChanged("_OutQty");
		}
	}

	[XmlElement(IsNullable = true, Order = 12)]
	public int? _NGQty
	{
		get
		{
			return _NGQtyField;
		}
		set
		{
			_NGQtyField = value;
			RaisePropertyChanged("_NGQty");
		}
	}

	[XmlElement(IsNullable = true, Order = 13)]
	public DateTime? _OutTime
	{
		get
		{
			return _OutTimeField;
		}
		set
		{
			_OutTimeField = value;
			RaisePropertyChanged("_OutTime");
		}
	}

	[XmlElement(Order = 14)]
	public string _OutUser
	{
		get
		{
			return _OutUserField;
		}
		set
		{
			_OutUserField = value;
			RaisePropertyChanged("_OutUser");
		}
	}

	[XmlElement(Order = 15)]
	public string _NGCode
	{
		get
		{
			return _NGCodeField;
		}
		set
		{
			_NGCodeField = value;
			RaisePropertyChanged("_NGCode");
		}
	}

	[XmlElement(Order = 16)]
	public int _ConfirmID
	{
		get
		{
			return _ConfirmIDField;
		}
		set
		{
			_ConfirmIDField = value;
			RaisePropertyChanged("_ConfirmID");
		}
	}

	[XmlElement(Order = 17)]
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

	[XmlElement(Order = 18)]
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

	[XmlElement(Order = 19)]
	public DateTime _ChangeDate
	{
		get
		{
			return _ChangeDateField;
		}
		set
		{
			_ChangeDateField = value;
			RaisePropertyChanged("_ChangeDate");
		}
	}

	[XmlElement(Order = 20)]
	public string _ChangeUser
	{
		get
		{
			return _ChangeUserField;
		}
		set
		{
			_ChangeUserField = value;
			RaisePropertyChanged("_ChangeUser");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
