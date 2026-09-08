using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelIDLog", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelIDLog : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string Parent_ReelIDField;

	[OptionalField]
	private string ReelIDField;

	[OptionalField]
	private string SMatNoField;

	[OptionalField]
	private string SBatchNoField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string BatchNoField;

	[OptionalField]
	private string Big_ReelIDField;

	[OptionalField]
	private string WONOField;

	[OptionalField]
	private string StatusField;

	[OptionalField]
	private string ActionField;

	[OptionalField]
	private string DNDOCField;

	[OptionalField]
	private string DNITEMField;

	[OptionalField]
	private string MMDOCField;

	[OptionalField]
	private string MMITEMField;

	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private bool IsSAPLogField;

	[OptionalField]
	private int ConfirmIDField;

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
	public string Parent_ReelID
	{
		get
		{
			return Parent_ReelIDField;
		}
		set
		{
			if ((object)Parent_ReelIDField != value)
			{
				Parent_ReelIDField = value;
				RaisePropertyChanged("Parent_ReelID");
			}
		}
	}

	[DataMember]
	public string ReelID
	{
		get
		{
			return ReelIDField;
		}
		set
		{
			if ((object)ReelIDField != value)
			{
				ReelIDField = value;
				RaisePropertyChanged("ReelID");
			}
		}
	}

	[DataMember]
	public string SMatNo
	{
		get
		{
			return SMatNoField;
		}
		set
		{
			if ((object)SMatNoField != value)
			{
				SMatNoField = value;
				RaisePropertyChanged("SMatNo");
			}
		}
	}

	[DataMember(Order = 3)]
	public string SBatchNo
	{
		get
		{
			return SBatchNoField;
		}
		set
		{
			if ((object)SBatchNoField != value)
			{
				SBatchNoField = value;
				RaisePropertyChanged("SBatchNo");
			}
		}
	}

	[DataMember(Order = 4)]
	public string MatNo
	{
		get
		{
			return MatNoField;
		}
		set
		{
			if ((object)MatNoField != value)
			{
				MatNoField = value;
				RaisePropertyChanged("MatNo");
			}
		}
	}

	[DataMember(Order = 5)]
	public string BatchNo
	{
		get
		{
			return BatchNoField;
		}
		set
		{
			if ((object)BatchNoField != value)
			{
				BatchNoField = value;
				RaisePropertyChanged("BatchNo");
			}
		}
	}

	[DataMember(Order = 6)]
	public string Big_ReelID
	{
		get
		{
			return Big_ReelIDField;
		}
		set
		{
			if ((object)Big_ReelIDField != value)
			{
				Big_ReelIDField = value;
				RaisePropertyChanged("Big_ReelID");
			}
		}
	}

	[DataMember(Order = 7)]
	public string WONO
	{
		get
		{
			return WONOField;
		}
		set
		{
			if ((object)WONOField != value)
			{
				WONOField = value;
				RaisePropertyChanged("WONO");
			}
		}
	}

	[DataMember(Order = 8)]
	public string Status
	{
		get
		{
			return StatusField;
		}
		set
		{
			if ((object)StatusField != value)
			{
				StatusField = value;
				RaisePropertyChanged("Status");
			}
		}
	}

	[DataMember(Order = 9)]
	public string Action
	{
		get
		{
			return ActionField;
		}
		set
		{
			if ((object)ActionField != value)
			{
				ActionField = value;
				RaisePropertyChanged("Action");
			}
		}
	}

	[DataMember(Order = 10)]
	public string DNDOC
	{
		get
		{
			return DNDOCField;
		}
		set
		{
			if ((object)DNDOCField != value)
			{
				DNDOCField = value;
				RaisePropertyChanged("DNDOC");
			}
		}
	}

	[DataMember(Order = 11)]
	public string DNITEM
	{
		get
		{
			return DNITEMField;
		}
		set
		{
			if ((object)DNITEMField != value)
			{
				DNITEMField = value;
				RaisePropertyChanged("DNITEM");
			}
		}
	}

	[DataMember(Order = 12)]
	public string MMDOC
	{
		get
		{
			return MMDOCField;
		}
		set
		{
			if ((object)MMDOCField != value)
			{
				MMDOCField = value;
				RaisePropertyChanged("MMDOC");
			}
		}
	}

	[DataMember(Order = 13)]
	public string MMITEM
	{
		get
		{
			return MMITEMField;
		}
		set
		{
			if ((object)MMITEMField != value)
			{
				MMITEMField = value;
				RaisePropertyChanged("MMITEM");
			}
		}
	}

	[DataMember(Order = 14)]
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

	[DataMember(Order = 15)]
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

	[DataMember(Order = 16)]
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

	[DataMember(Order = 17)]
	public bool IsSAPLog
	{
		get
		{
			return IsSAPLogField;
		}
		set
		{
			if (!IsSAPLogField.Equals(value))
			{
				IsSAPLogField = value;
				RaisePropertyChanged("IsSAPLog");
			}
		}
	}

	[DataMember(Order = 18)]
	public int ConfirmID
	{
		get
		{
			return ConfirmIDField;
		}
		set
		{
			if (!ConfirmIDField.Equals(value))
			{
				ConfirmIDField = value;
				RaisePropertyChanged("ConfirmID");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
