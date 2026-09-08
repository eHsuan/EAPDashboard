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
public class WOComponentConfirmNGRecord : INotifyPropertyChanged
{
	private string _WOComponentConfirmIDField;

	private string _NGCodeField;

	private int _NGQtyField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime _ChangeDateField;

	private string _ChangeUserField;

	private string _NGNameField;

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

	[XmlElement(Order = 2)]
	public int _NGQty
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

	[XmlElement(Order = 3)]
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

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
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

	[XmlElement(Order = 6)]
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

	[XmlElement(Order = 7)]
	public string _NGName
	{
		get
		{
			return _NGNameField;
		}
		set
		{
			_NGNameField = value;
			RaisePropertyChanged("_NGName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
