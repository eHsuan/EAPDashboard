using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WOConfirm", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WOConfirm : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool CanceledField;

	[OptionalField]
	private string ChangeDateField;

	[OptionalField]
	private string ChangeUserField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private string EndDateField;

	[OptionalField]
	private string FinishedField;

	[OptionalField]
	private int FlowIDField;

	[OptionalField]
	private string HSFField;

	[OptionalField]
	private int IDField;

	[OptionalField]
	private string InTimeField;

	[OptionalField]
	private string InUserField;

	[OptionalField]
	private int InputQtyField;

	[OptionalField]
	private bool IsCheckInField;

	[OptionalField]
	private int NGQtyField;

	[OptionalField]
	private string OperatorField;

	[OptionalField]
	private int OutPutQtyField;

	[OptionalField]
	private int RefInputQtyField;

	[OptionalField]
	private int RefOutputQtyField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string StartDateField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string WorkCenterNoField;

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
	public bool Canceled
	{
		get
		{
			return CanceledField;
		}
		set
		{
			if (!CanceledField.Equals(value))
			{
				CanceledField = value;
				RaisePropertyChanged("Canceled");
			}
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
	public string EndDate
	{
		get
		{
			return EndDateField;
		}
		set
		{
			if ((object)EndDateField != value)
			{
				EndDateField = value;
				RaisePropertyChanged("EndDate");
			}
		}
	}

	[DataMember]
	public string Finished
	{
		get
		{
			return FinishedField;
		}
		set
		{
			if ((object)FinishedField != value)
			{
				FinishedField = value;
				RaisePropertyChanged("Finished");
			}
		}
	}

	[DataMember]
	public int FlowID
	{
		get
		{
			return FlowIDField;
		}
		set
		{
			if (!FlowIDField.Equals(value))
			{
				FlowIDField = value;
				RaisePropertyChanged("FlowID");
			}
		}
	}

	[DataMember]
	public string HSF
	{
		get
		{
			return HSFField;
		}
		set
		{
			if ((object)HSFField != value)
			{
				HSFField = value;
				RaisePropertyChanged("HSF");
			}
		}
	}

	[DataMember]
	public int ID
	{
		get
		{
			return IDField;
		}
		set
		{
			if (!IDField.Equals(value))
			{
				IDField = value;
				RaisePropertyChanged("ID");
			}
		}
	}

	[DataMember]
	public string InTime
	{
		get
		{
			return InTimeField;
		}
		set
		{
			if ((object)InTimeField != value)
			{
				InTimeField = value;
				RaisePropertyChanged("InTime");
			}
		}
	}

	[DataMember]
	public string InUser
	{
		get
		{
			return InUserField;
		}
		set
		{
			if ((object)InUserField != value)
			{
				InUserField = value;
				RaisePropertyChanged("InUser");
			}
		}
	}

	[DataMember]
	public int InputQty
	{
		get
		{
			return InputQtyField;
		}
		set
		{
			if (!InputQtyField.Equals(value))
			{
				InputQtyField = value;
				RaisePropertyChanged("InputQty");
			}
		}
	}

	[DataMember]
	public bool IsCheckIn
	{
		get
		{
			return IsCheckInField;
		}
		set
		{
			if (!IsCheckInField.Equals(value))
			{
				IsCheckInField = value;
				RaisePropertyChanged("IsCheckIn");
			}
		}
	}

	[DataMember]
	public int NGQty
	{
		get
		{
			return NGQtyField;
		}
		set
		{
			if (!NGQtyField.Equals(value))
			{
				NGQtyField = value;
				RaisePropertyChanged("NGQty");
			}
		}
	}

	[DataMember]
	public string Operator
	{
		get
		{
			return OperatorField;
		}
		set
		{
			if ((object)OperatorField != value)
			{
				OperatorField = value;
				RaisePropertyChanged("Operator");
			}
		}
	}

	[DataMember]
	public int OutPutQty
	{
		get
		{
			return OutPutQtyField;
		}
		set
		{
			if (!OutPutQtyField.Equals(value))
			{
				OutPutQtyField = value;
				RaisePropertyChanged("OutPutQty");
			}
		}
	}

	[DataMember]
	public int RefInputQty
	{
		get
		{
			return RefInputQtyField;
		}
		set
		{
			if (!RefInputQtyField.Equals(value))
			{
				RefInputQtyField = value;
				RaisePropertyChanged("RefInputQty");
			}
		}
	}

	[DataMember]
	public int RefOutputQty
	{
		get
		{
			return RefOutputQtyField;
		}
		set
		{
			if (!RefOutputQtyField.Equals(value))
			{
				RefOutputQtyField = value;
				RaisePropertyChanged("RefOutputQty");
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
	public string StartDate
	{
		get
		{
			return StartDateField;
		}
		set
		{
			if ((object)StartDateField != value)
			{
				StartDateField = value;
				RaisePropertyChanged("StartDate");
			}
		}
	}

	[DataMember]
	public string WONo
	{
		get
		{
			return WONoField;
		}
		set
		{
			if ((object)WONoField != value)
			{
				WONoField = value;
				RaisePropertyChanged("WONo");
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
