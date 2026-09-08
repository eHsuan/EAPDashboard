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
public class EqpDynamicNGRecord : INotifyPropertyChanged
{
	private int _IDField;

	private string _MachNoField;

	private string _WONOField;

	private int _WOConfirmIDField;

	private string _NGCodeField;

	private decimal _NGQtyField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime _UpdateDateField;

	private string _UpdateUserField;

	[XmlElement(Order = 0)]
	public int _ID
	{
		get
		{
			return _IDField;
		}
		set
		{
			_IDField = value;
			RaisePropertyChanged("_ID");
		}
	}

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public decimal _NGQty
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
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

	[XmlElement(Order = 8)]
	public DateTime _UpdateDate
	{
		get
		{
			return _UpdateDateField;
		}
		set
		{
			_UpdateDateField = value;
			RaisePropertyChanged("_UpdateDate");
		}
	}

	[XmlElement(Order = 9)]
	public string _UpdateUser
	{
		get
		{
			return _UpdateUserField;
		}
		set
		{
			_UpdateUserField = value;
			RaisePropertyChanged("_UpdateUser");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
