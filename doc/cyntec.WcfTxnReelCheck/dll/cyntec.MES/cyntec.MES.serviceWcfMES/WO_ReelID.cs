using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelID", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelID : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string AreaField;

	[OptionalField]
	private string BatchNoField;

	[OptionalField]
	private string FQCRemarkField;

	[OptionalField]
	private string FQCStatusField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string PCField;

	[OptionalField]
	private decimal QtyField;

	[OptionalField]
	private string RePrtField;

	[OptionalField]
	private string ReelIDField;

	[OptionalField]
	private string RefReelIDField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string SernoField;

	[OptionalField]
	private string StatusField;

	[OptionalField]
	private string TCRemarkField;

	[OptionalField]
	private string TCStatusField;

	[OptionalField]
	private string TPEndTimeField;

	[OptionalField]
	private string TPMachNoField;

	[OptionalField]
	private string TPRemarkField;

	[OptionalField]
	private string TPStartTimeField;

	[OptionalField]
	private string TPStatusField;

	[OptionalField]
	private string UpdateDateField;

	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string WONoField;

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
	public string Area
	{
		get
		{
			return AreaField;
		}
		set
		{
			if ((object)AreaField != value)
			{
				AreaField = value;
				RaisePropertyChanged("Area");
			}
		}
	}

	[DataMember]
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

	[DataMember]
	public string FQCRemark
	{
		get
		{
			return FQCRemarkField;
		}
		set
		{
			if ((object)FQCRemarkField != value)
			{
				FQCRemarkField = value;
				RaisePropertyChanged("FQCRemark");
			}
		}
	}

	[DataMember]
	public string FQCStatus
	{
		get
		{
			return FQCStatusField;
		}
		set
		{
			if ((object)FQCStatusField != value)
			{
				FQCStatusField = value;
				RaisePropertyChanged("FQCStatus");
			}
		}
	}

	[DataMember]
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

	[DataMember]
	public string PC
	{
		get
		{
			return PCField;
		}
		set
		{
			if ((object)PCField != value)
			{
				PCField = value;
				RaisePropertyChanged("PC");
			}
		}
	}

	[DataMember]
	public decimal Qty
	{
		get
		{
			return QtyField;
		}
		set
		{
			if (!QtyField.Equals(value))
			{
				QtyField = value;
				RaisePropertyChanged("Qty");
			}
		}
	}

	[DataMember]
	public string RePrt
	{
		get
		{
			return RePrtField;
		}
		set
		{
			if ((object)RePrtField != value)
			{
				RePrtField = value;
				RaisePropertyChanged("RePrt");
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
	public string RefReelID
	{
		get
		{
			return RefReelIDField;
		}
		set
		{
			if ((object)RefReelIDField != value)
			{
				RefReelIDField = value;
				RaisePropertyChanged("RefReelID");
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
	public string Serno
	{
		get
		{
			return SernoField;
		}
		set
		{
			if ((object)SernoField != value)
			{
				SernoField = value;
				RaisePropertyChanged("Serno");
			}
		}
	}

	[DataMember]
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

	[DataMember]
	public string TCRemark
	{
		get
		{
			return TCRemarkField;
		}
		set
		{
			if ((object)TCRemarkField != value)
			{
				TCRemarkField = value;
				RaisePropertyChanged("TCRemark");
			}
		}
	}

	[DataMember]
	public string TCStatus
	{
		get
		{
			return TCStatusField;
		}
		set
		{
			if ((object)TCStatusField != value)
			{
				TCStatusField = value;
				RaisePropertyChanged("TCStatus");
			}
		}
	}

	[DataMember]
	public string TPEndTime
	{
		get
		{
			return TPEndTimeField;
		}
		set
		{
			if ((object)TPEndTimeField != value)
			{
				TPEndTimeField = value;
				RaisePropertyChanged("TPEndTime");
			}
		}
	}

	[DataMember]
	public string TPMachNo
	{
		get
		{
			return TPMachNoField;
		}
		set
		{
			if ((object)TPMachNoField != value)
			{
				TPMachNoField = value;
				RaisePropertyChanged("TPMachNo");
			}
		}
	}

	[DataMember]
	public string TPRemark
	{
		get
		{
			return TPRemarkField;
		}
		set
		{
			if ((object)TPRemarkField != value)
			{
				TPRemarkField = value;
				RaisePropertyChanged("TPRemark");
			}
		}
	}

	[DataMember]
	public string TPStartTime
	{
		get
		{
			return TPStartTimeField;
		}
		set
		{
			if ((object)TPStartTimeField != value)
			{
				TPStartTimeField = value;
				RaisePropertyChanged("TPStartTime");
			}
		}
	}

	[DataMember]
	public string TPStatus
	{
		get
		{
			return TPStatusField;
		}
		set
		{
			if ((object)TPStatusField != value)
			{
				TPStatusField = value;
				RaisePropertyChanged("TPStatus");
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
