using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AccountUser", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class AccountUser : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string ChgPassField;

	[OptionalField]
	private string EmpNoField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private string NTAccountField;

	[OptionalField]
	private string NTDomainField;

	[OptionalField]
	private string PasswordField;

	[OptionalField]
	private string[] PermissionSringListField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string UserNameField;

	[OptionalField]
	private string[] UserRoleListField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string ChgPass
	{
		get
		{
			return ChgPassField;
		}
		set
		{
			if ((object)ChgPassField != value)
			{
				ChgPassField = value;
				RaisePropertyChanged("ChgPass");
			}
		}
	}

	[DataMember]
	public string EmpNo
	{
		get
		{
			return EmpNoField;
		}
		set
		{
			if ((object)EmpNoField != value)
			{
				EmpNoField = value;
				RaisePropertyChanged("EmpNo");
			}
		}
	}

	[DataMember]
	public bool Enable
	{
		get
		{
			return EnableField;
		}
		set
		{
			if (!EnableField.Equals(value))
			{
				EnableField = value;
				RaisePropertyChanged("Enable");
			}
		}
	}

	[DataMember]
	public string NTAccount
	{
		get
		{
			return NTAccountField;
		}
		set
		{
			if ((object)NTAccountField != value)
			{
				NTAccountField = value;
				RaisePropertyChanged("NTAccount");
			}
		}
	}

	[DataMember]
	public string NTDomain
	{
		get
		{
			return NTDomainField;
		}
		set
		{
			if ((object)NTDomainField != value)
			{
				NTDomainField = value;
				RaisePropertyChanged("NTDomain");
			}
		}
	}

	[DataMember]
	public string Password
	{
		get
		{
			return PasswordField;
		}
		set
		{
			if ((object)PasswordField != value)
			{
				PasswordField = value;
				RaisePropertyChanged("Password");
			}
		}
	}

	[DataMember]
	public string[] PermissionSringList
	{
		get
		{
			return PermissionSringListField;
		}
		set
		{
			if (PermissionSringListField != value)
			{
				PermissionSringListField = value;
				RaisePropertyChanged("PermissionSringList");
			}
		}
	}

	[DataMember]
	public string Remark
	{
		get
		{
			return RemarkField;
		}
		set
		{
			if ((object)RemarkField != value)
			{
				RemarkField = value;
				RaisePropertyChanged("Remark");
			}
		}
	}

	[DataMember]
	public string UserID
	{
		get
		{
			return UserIDField;
		}
		set
		{
			if ((object)UserIDField != value)
			{
				UserIDField = value;
				RaisePropertyChanged("UserID");
			}
		}
	}

	[DataMember]
	public string UserName
	{
		get
		{
			return UserNameField;
		}
		set
		{
			if ((object)UserNameField != value)
			{
				UserNameField = value;
				RaisePropertyChanged("UserName");
			}
		}
	}

	[DataMember]
	public string[] UserRoleList
	{
		get
		{
			return UserRoleListField;
		}
		set
		{
			if (UserRoleListField != value)
			{
				UserRoleListField = value;
				RaisePropertyChanged("UserRoleList");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
