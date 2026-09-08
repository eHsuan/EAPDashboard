using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_InputCode", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_InputCode : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string ChangeDateField;

	[OptionalField]
	private string ChangeUserField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private string DefaultValueField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private string InCheckField;

	[OptionalField]
	private string InputCodeField;

	[OptionalField]
	private string InputENameField;

	[OptionalField]
	private string InputNameField;

	[OptionalField]
	private string InputTypeField;

	[OptionalField]
	private string ListItemField;

	[OptionalField]
	private string LowerLimitField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string RequireField;

	[OptionalField]
	private int SortNoField;

	[OptionalField]
	private string UpperLimitField;

	[OptionalField]
	private string VerifyField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string unitField;

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
	public string ChangeDate
	{
		get
		{
			return ChangeDateField;
		}
		set
		{
			if ((object)ChangeDateField != value)
			{
				ChangeDateField = value;
				RaisePropertyChanged("ChangeDate");
			}
		}
	}

	[DataMember]
	public string ChangeUser
	{
		get
		{
			return ChangeUserField;
		}
		set
		{
			if ((object)ChangeUserField != value)
			{
				ChangeUserField = value;
				RaisePropertyChanged("ChangeUser");
			}
		}
	}

	[DataMember]
	public string CreateDate
	{
		get
		{
			return CreateDateField;
		}
		set
		{
			if ((object)CreateDateField != value)
			{
				CreateDateField = value;
				RaisePropertyChanged("CreateDate");
			}
		}
	}

	[DataMember]
	public string CreateUser
	{
		get
		{
			return CreateUserField;
		}
		set
		{
			if ((object)CreateUserField != value)
			{
				CreateUserField = value;
				RaisePropertyChanged("CreateUser");
			}
		}
	}

	[DataMember]
	public string DefaultValue
	{
		get
		{
			return DefaultValueField;
		}
		set
		{
			if ((object)DefaultValueField != value)
			{
				DefaultValueField = value;
				RaisePropertyChanged("DefaultValue");
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
	public string InCheck
	{
		get
		{
			return InCheckField;
		}
		set
		{
			if ((object)InCheckField != value)
			{
				InCheckField = value;
				RaisePropertyChanged("InCheck");
			}
		}
	}

	[DataMember]
	public string InputCode
	{
		get
		{
			return InputCodeField;
		}
		set
		{
			if ((object)InputCodeField != value)
			{
				InputCodeField = value;
				RaisePropertyChanged("InputCode");
			}
		}
	}

	[DataMember]
	public string InputEName
	{
		get
		{
			return InputENameField;
		}
		set
		{
			if ((object)InputENameField != value)
			{
				InputENameField = value;
				RaisePropertyChanged("InputEName");
			}
		}
	}

	[DataMember]
	public string InputName
	{
		get
		{
			return InputNameField;
		}
		set
		{
			if ((object)InputNameField != value)
			{
				InputNameField = value;
				RaisePropertyChanged("InputName");
			}
		}
	}

	[DataMember]
	public string InputType
	{
		get
		{
			return InputTypeField;
		}
		set
		{
			if ((object)InputTypeField != value)
			{
				InputTypeField = value;
				RaisePropertyChanged("InputType");
			}
		}
	}

	[DataMember]
	public string ListItem
	{
		get
		{
			return ListItemField;
		}
		set
		{
			if ((object)ListItemField != value)
			{
				ListItemField = value;
				RaisePropertyChanged("ListItem");
			}
		}
	}

	[DataMember]
	public string LowerLimit
	{
		get
		{
			return LowerLimitField;
		}
		set
		{
			if ((object)LowerLimitField != value)
			{
				LowerLimitField = value;
				RaisePropertyChanged("LowerLimit");
			}
		}
	}

	[DataMember]
	public string MatGroupNo
	{
		get
		{
			return MatGroupNoField;
		}
		set
		{
			if ((object)MatGroupNoField != value)
			{
				MatGroupNoField = value;
				RaisePropertyChanged("MatGroupNo");
			}
		}
	}

	[DataMember]
	public string PlantNo
	{
		get
		{
			return PlantNoField;
		}
		set
		{
			if ((object)PlantNoField != value)
			{
				PlantNoField = value;
				RaisePropertyChanged("PlantNo");
			}
		}
	}

	[DataMember]
	public string Require
	{
		get
		{
			return RequireField;
		}
		set
		{
			if ((object)RequireField != value)
			{
				RequireField = value;
				RaisePropertyChanged("Require");
			}
		}
	}

	[DataMember]
	public int SortNo
	{
		get
		{
			return SortNoField;
		}
		set
		{
			if (!SortNoField.Equals(value))
			{
				SortNoField = value;
				RaisePropertyChanged("SortNo");
			}
		}
	}

	[DataMember]
	public string UpperLimit
	{
		get
		{
			return UpperLimitField;
		}
		set
		{
			if ((object)UpperLimitField != value)
			{
				UpperLimitField = value;
				RaisePropertyChanged("UpperLimit");
			}
		}
	}

	[DataMember]
	public string Verify
	{
		get
		{
			return VerifyField;
		}
		set
		{
			if ((object)VerifyField != value)
			{
				VerifyField = value;
				RaisePropertyChanged("Verify");
			}
		}
	}

	[DataMember]
	public string WorkCenterNo
	{
		get
		{
			return WorkCenterNoField;
		}
		set
		{
			if ((object)WorkCenterNoField != value)
			{
				WorkCenterNoField = value;
				RaisePropertyChanged("WorkCenterNo");
			}
		}
	}

	[DataMember]
	public string unit
	{
		get
		{
			return unitField;
		}
		set
		{
			if ((object)unitField != value)
			{
				unitField = value;
				RaisePropertyChanged("unit");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
