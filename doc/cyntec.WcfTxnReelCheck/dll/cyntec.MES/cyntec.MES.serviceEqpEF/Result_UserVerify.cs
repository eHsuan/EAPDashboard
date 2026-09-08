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
public class Result_UserVerify : Basic_Result
{
	private string _UserNameField;

	private string _NTDomainField;

	private string _NTAccountField;

	private string _EmailField;

	private string _EmpNoField;

	private string _EnableField;

	private string _ChgPassField;

	private string _RemarkField;

	private string _LockedField;

	private string _CheckCodeField;

	private string _CreateUserField;

	private string _UpdateUserField;

	private int? _BadPwdCountField;

	private DateTime? _PwdExpireDateField;

	private DateTime? _PwdSetDateField;

	private DateTime? _LastLogonField;

	private DateTime? _CreateDateField;

	private DateTime? _UpdateDateField;

	private DateTime? _CheckSetDateField;

	private int _ModbusErrorValueField;

	[XmlElement(Order = 0)]
	public string _UserName
	{
		get
		{
			return _UserNameField;
		}
		set
		{
			_UserNameField = value;
			RaisePropertyChanged("_UserName");
		}
	}

	[XmlElement(Order = 1)]
	public string _NTDomain
	{
		get
		{
			return _NTDomainField;
		}
		set
		{
			_NTDomainField = value;
			RaisePropertyChanged("_NTDomain");
		}
	}

	[XmlElement(Order = 2)]
	public string _NTAccount
	{
		get
		{
			return _NTAccountField;
		}
		set
		{
			_NTAccountField = value;
			RaisePropertyChanged("_NTAccount");
		}
	}

	[XmlElement(Order = 3)]
	public string _Email
	{
		get
		{
			return _EmailField;
		}
		set
		{
			_EmailField = value;
			RaisePropertyChanged("_Email");
		}
	}

	[XmlElement(Order = 4)]
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

	[XmlElement(Order = 5)]
	public string _Enable
	{
		get
		{
			return _EnableField;
		}
		set
		{
			_EnableField = value;
			RaisePropertyChanged("_Enable");
		}
	}

	[XmlElement(Order = 6)]
	public string _ChgPass
	{
		get
		{
			return _ChgPassField;
		}
		set
		{
			_ChgPassField = value;
			RaisePropertyChanged("_ChgPass");
		}
	}

	[XmlElement(Order = 7)]
	public string _Remark
	{
		get
		{
			return _RemarkField;
		}
		set
		{
			_RemarkField = value;
			RaisePropertyChanged("_Remark");
		}
	}

	[XmlElement(Order = 8)]
	public string _Locked
	{
		get
		{
			return _LockedField;
		}
		set
		{
			_LockedField = value;
			RaisePropertyChanged("_Locked");
		}
	}

	[XmlElement(Order = 9)]
	public string _CheckCode
	{
		get
		{
			return _CheckCodeField;
		}
		set
		{
			_CheckCodeField = value;
			RaisePropertyChanged("_CheckCode");
		}
	}

	[XmlElement(Order = 10)]
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

	[XmlElement(Order = 11)]
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

	[XmlElement(IsNullable = true, Order = 12)]
	public int? _BadPwdCount
	{
		get
		{
			return _BadPwdCountField;
		}
		set
		{
			_BadPwdCountField = value;
			RaisePropertyChanged("_BadPwdCount");
		}
	}

	[XmlElement(IsNullable = true, Order = 13)]
	public DateTime? _PwdExpireDate
	{
		get
		{
			return _PwdExpireDateField;
		}
		set
		{
			_PwdExpireDateField = value;
			RaisePropertyChanged("_PwdExpireDate");
		}
	}

	[XmlElement(IsNullable = true, Order = 14)]
	public DateTime? _PwdSetDate
	{
		get
		{
			return _PwdSetDateField;
		}
		set
		{
			_PwdSetDateField = value;
			RaisePropertyChanged("_PwdSetDate");
		}
	}

	[XmlElement(IsNullable = true, Order = 15)]
	public DateTime? _LastLogon
	{
		get
		{
			return _LastLogonField;
		}
		set
		{
			_LastLogonField = value;
			RaisePropertyChanged("_LastLogon");
		}
	}

	[XmlElement(IsNullable = true, Order = 16)]
	public DateTime? _CreateDate
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

	[XmlElement(IsNullable = true, Order = 17)]
	public DateTime? _UpdateDate
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

	[XmlElement(IsNullable = true, Order = 18)]
	public DateTime? _CheckSetDate
	{
		get
		{
			return _CheckSetDateField;
		}
		set
		{
			_CheckSetDateField = value;
			RaisePropertyChanged("_CheckSetDate");
		}
	}

	[XmlElement(Order = 19)]
	public int _ModbusErrorValue
	{
		get
		{
			return _ModbusErrorValueField;
		}
		set
		{
			_ModbusErrorValueField = value;
			RaisePropertyChanged("_ModbusErrorValue");
		}
	}
}
