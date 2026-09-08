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
public class WOComponentWOChange : INotifyPropertyChanged
{
	private long _IndexIDField;

	private string _SerialNoField;

	private string _OldMatGroupNoField;

	private string _NewMatGroupNoField;

	private string _OldWONoField;

	private string _NewWONoField;

	private DateTime _CreateDateField;

	private string _CreateUserField;

	private DateTime _ChangeDateField;

	private string _ChangeUserField;

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
	public string _OldMatGroupNo
	{
		get
		{
			return _OldMatGroupNoField;
		}
		set
		{
			_OldMatGroupNoField = value;
			RaisePropertyChanged("_OldMatGroupNo");
		}
	}

	[XmlElement(Order = 3)]
	public string _NewMatGroupNo
	{
		get
		{
			return _NewMatGroupNoField;
		}
		set
		{
			_NewMatGroupNoField = value;
			RaisePropertyChanged("_NewMatGroupNo");
		}
	}

	[XmlElement(Order = 4)]
	public string _OldWONo
	{
		get
		{
			return _OldWONoField;
		}
		set
		{
			_OldWONoField = value;
			RaisePropertyChanged("_OldWONo");
		}
	}

	[XmlElement(Order = 5)]
	public string _NewWONo
	{
		get
		{
			return _NewWONoField;
		}
		set
		{
			_NewWONoField = value;
			RaisePropertyChanged("_NewWONo");
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

	[XmlElement(Order = 9)]
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
