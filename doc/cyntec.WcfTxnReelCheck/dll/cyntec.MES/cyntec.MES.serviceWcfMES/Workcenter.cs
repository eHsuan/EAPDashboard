using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Workcenter", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class Workcenter : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool CheckInIsCarrierField;

	[OptionalField]
	private bool CheckOutIsCarrierField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private int CycleTimeField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private bool GroupConfirmField;

	[OptionalField]
	private bool IsCarrierField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string NoteField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string SortIDField;

	[OptionalField]
	private int TimeLimitMaxField;

	[OptionalField]
	private int TimeLimitMinField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UpdateUserField;

	[OptionalField]
	private string WOUnitField;

	[OptionalField]
	private string WorkCenterGroupCodeField;

	[OptionalField]
	private string WorkCenterNameField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string WorkCenterTypeCodeField;

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
	public bool CheckInIsCarrier
	{
		get
		{
			return CheckInIsCarrierField;
		}
		set
		{
			if (!CheckInIsCarrierField.Equals(value))
			{
				CheckInIsCarrierField = value;
				RaisePropertyChanged("CheckInIsCarrier");
			}
		}
	}

	[DataMember]
	public bool CheckOutIsCarrier
	{
		get
		{
			return CheckOutIsCarrierField;
		}
		set
		{
			if (!CheckOutIsCarrierField.Equals(value))
			{
				CheckOutIsCarrierField = value;
				RaisePropertyChanged("CheckOutIsCarrier");
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
	public int CycleTime
	{
		get
		{
			return CycleTimeField;
		}
		set
		{
			if (!CycleTimeField.Equals(value))
			{
				CycleTimeField = value;
				RaisePropertyChanged("CycleTime");
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
	public bool GroupConfirm
	{
		get
		{
			return GroupConfirmField;
		}
		set
		{
			if (!GroupConfirmField.Equals(value))
			{
				GroupConfirmField = value;
				RaisePropertyChanged("GroupConfirm");
			}
		}
	}

	[DataMember]
	public bool IsCarrier
	{
		get
		{
			return IsCarrierField;
		}
		set
		{
			if (!IsCarrierField.Equals(value))
			{
				IsCarrierField = value;
				RaisePropertyChanged("IsCarrier");
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
	public string Note
	{
		get
		{
			return NoteField;
		}
		set
		{
			if ((object)NoteField != value)
			{
				NoteField = value;
				RaisePropertyChanged("Note");
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
	public string SortID
	{
		get
		{
			return SortIDField;
		}
		set
		{
			if ((object)SortIDField != value)
			{
				SortIDField = value;
				RaisePropertyChanged("SortID");
			}
		}
	}

	[DataMember]
	public int TimeLimitMax
	{
		get
		{
			return TimeLimitMaxField;
		}
		set
		{
			if (!TimeLimitMaxField.Equals(value))
			{
				TimeLimitMaxField = value;
				RaisePropertyChanged("TimeLimitMax");
			}
		}
	}

	[DataMember]
	public int TimeLimitMin
	{
		get
		{
			return TimeLimitMinField;
		}
		set
		{
			if (!TimeLimitMinField.Equals(value))
			{
				TimeLimitMinField = value;
				RaisePropertyChanged("TimeLimitMin");
			}
		}
	}

	[DataMember]
	public string UpdateDate
	{
		get
		{
			return UpdateDateField;
		}
		set
		{
			if ((object)UpdateDateField != value)
			{
				UpdateDateField = value;
				RaisePropertyChanged("UpdateDate");
			}
		}
	}

	[DataMember]
	public string UpdateUser
	{
		get
		{
			return UpdateUserField;
		}
		set
		{
			if ((object)UpdateUserField != value)
			{
				UpdateUserField = value;
				RaisePropertyChanged("UpdateUser");
			}
		}
	}

	[DataMember]
	public string WOUnit
	{
		get
		{
			return WOUnitField;
		}
		set
		{
			if ((object)WOUnitField != value)
			{
				WOUnitField = value;
				RaisePropertyChanged("WOUnit");
			}
		}
	}

	[DataMember]
	public string WorkCenterGroupCode
	{
		get
		{
			return WorkCenterGroupCodeField;
		}
		set
		{
			if ((object)WorkCenterGroupCodeField != value)
			{
				WorkCenterGroupCodeField = value;
				RaisePropertyChanged("WorkCenterGroupCode");
			}
		}
	}

	[DataMember]
	public string WorkCenterName
	{
		get
		{
			return WorkCenterNameField;
		}
		set
		{
			if ((object)WorkCenterNameField != value)
			{
				WorkCenterNameField = value;
				RaisePropertyChanged("WorkCenterName");
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
	public string WorkCenterTypeCode
	{
		get
		{
			return WorkCenterTypeCodeField;
		}
		set
		{
			if ((object)WorkCenterTypeCodeField != value)
			{
				WorkCenterTypeCodeField = value;
				RaisePropertyChanged("WorkCenterTypeCode");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
