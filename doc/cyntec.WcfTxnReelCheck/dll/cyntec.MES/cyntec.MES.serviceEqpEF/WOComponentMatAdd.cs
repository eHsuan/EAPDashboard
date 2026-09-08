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
public class WOComponentMatAdd : INotifyPropertyChanged
{
	private string _SerialNoField;

	private string _WONoField;

	private string _RefWONOField;

	private string _MatGroupNoField;

	private int _FlowIDField;

	private int _CurrentQtyField;

	private string _WorkCenterNoField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime _ChangeDateField;

	private string _ChangeUserField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
	public string _RefWONO
	{
		get
		{
			return _RefWONOField;
		}
		set
		{
			_RefWONOField = value;
			RaisePropertyChanged("_RefWONO");
		}
	}

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public int _CurrentQty
	{
		get
		{
			return _CurrentQtyField;
		}
		set
		{
			_CurrentQtyField = value;
			RaisePropertyChanged("_CurrentQty");
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

	[XmlElement(Order = 8)]
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

	[XmlElement(Order = 9)]
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

	[XmlElement(Order = 10)]
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
