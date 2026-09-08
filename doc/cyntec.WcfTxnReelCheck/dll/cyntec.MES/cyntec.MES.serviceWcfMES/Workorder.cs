using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Workorder", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class Workorder : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string BatchNoField;

	[OptionalField]
	private bool BlockedField;

	[OptionalField]
	private decimal ByProductQtyField;

	[OptionalField]
	private string ChangeDateField;

	[OptionalField]
	private string ChangeUserField;

	[OptionalField]
	private string CloseCodeField;

	[OptionalField]
	private string CloseDateField;

	[OptionalField]
	private string CloseReasonField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private int CurrentFlowIDField;

	[OptionalField]
	private string CurrentWorkCenterField;

	[OptionalField]
	private string CustomerCodeField;

	[OptionalField]
	private string CustomerDeliveryTimeField;

	[OptionalField]
	private bool DeletedField;

	[OptionalField]
	private string DeliverDateField;

	[OptionalField]
	private string DeliverQtyField;

	[OptionalField]
	private string EndDateField;

	[OptionalField]
	private string ITNoteField;

	[OptionalField]
	private string LabelModeField;

	[OptionalField]
	private string LastConfirmDateField;

	[OptionalField]
	private string LineCodeField;

	[OptionalField]
	private string MasterWONoField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string MatPostDateField;

	[OptionalField]
	private bool NeedMergeField;

	[OptionalField]
	private string OrderTypeField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string PriorityField;

	[OptionalField]
	private string ProcessEndDateField;

	[OptionalField]
	private string ProcessStartDateField;

	[OptionalField]
	private string ProductStageCodeField;

	[OptionalField]
	private bool ReleasedField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string Remark2Field;

	[OptionalField]
	private string Remark3Field;

	[OptionalField]
	private string ReworkReasonField;

	[OptionalField]
	private string RouteNoField;

	[OptionalField]
	private string SFC1010Field;

	[OptionalField]
	private bool ScrapedField;

	[OptionalField]
	private string SortStringField;

	[OptionalField]
	private string SplitCodeField;

	[OptionalField]
	private string StartDateField;

	[OptionalField]
	private string SyncDateField;

	[OptionalField]
	private bool TECOField;

	[OptionalField]
	private int TotalQtyField;

	[OptionalField]
	private string TraceCodeField;

	[OptionalField]
	private bool UrgentField;

	[OptionalField]
	private int WIPQtyField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private bool WOStatusCodeField;

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
	public bool Blocked
	{
		get
		{
			return BlockedField;
		}
		set
		{
			if (!BlockedField.Equals(value))
			{
				BlockedField = value;
				RaisePropertyChanged("Blocked");
			}
		}
	}

	[DataMember]
	public decimal ByProductQty
	{
		get
		{
			return ByProductQtyField;
		}
		set
		{
			if (!ByProductQtyField.Equals(value))
			{
				ByProductQtyField = value;
				RaisePropertyChanged("ByProductQty");
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
	public string CloseCode
	{
		get
		{
			return CloseCodeField;
		}
		set
		{
			if ((object)CloseCodeField != value)
			{
				CloseCodeField = value;
				RaisePropertyChanged("CloseCode");
			}
		}
	}

	[DataMember]
	public string CloseDate
	{
		get
		{
			return CloseDateField;
		}
		set
		{
			if ((object)CloseDateField != value)
			{
				CloseDateField = value;
				RaisePropertyChanged("CloseDate");
			}
		}
	}

	[DataMember]
	public string CloseReason
	{
		get
		{
			return CloseReasonField;
		}
		set
		{
			if ((object)CloseReasonField != value)
			{
				CloseReasonField = value;
				RaisePropertyChanged("CloseReason");
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
	public int CurrentFlowID
	{
		get
		{
			return CurrentFlowIDField;
		}
		set
		{
			if (!CurrentFlowIDField.Equals(value))
			{
				CurrentFlowIDField = value;
				RaisePropertyChanged("CurrentFlowID");
			}
		}
	}

	[DataMember]
	public string CurrentWorkCenter
	{
		get
		{
			return CurrentWorkCenterField;
		}
		set
		{
			if ((object)CurrentWorkCenterField != value)
			{
				CurrentWorkCenterField = value;
				RaisePropertyChanged("CurrentWorkCenter");
			}
		}
	}

	[DataMember]
	public string CustomerCode
	{
		get
		{
			return CustomerCodeField;
		}
		set
		{
			if ((object)CustomerCodeField != value)
			{
				CustomerCodeField = value;
				RaisePropertyChanged("CustomerCode");
			}
		}
	}

	[DataMember]
	public string CustomerDeliveryTime
	{
		get
		{
			return CustomerDeliveryTimeField;
		}
		set
		{
			if ((object)CustomerDeliveryTimeField != value)
			{
				CustomerDeliveryTimeField = value;
				RaisePropertyChanged("CustomerDeliveryTime");
			}
		}
	}

	[DataMember]
	public bool Deleted
	{
		get
		{
			return DeletedField;
		}
		set
		{
			if (!DeletedField.Equals(value))
			{
				DeletedField = value;
				RaisePropertyChanged("Deleted");
			}
		}
	}

	[DataMember]
	public string DeliverDate
	{
		get
		{
			return DeliverDateField;
		}
		set
		{
			if ((object)DeliverDateField != value)
			{
				DeliverDateField = value;
				RaisePropertyChanged("DeliverDate");
			}
		}
	}

	[DataMember]
	public string DeliverQty
	{
		get
		{
			return DeliverQtyField;
		}
		set
		{
			if ((object)DeliverQtyField != value)
			{
				DeliverQtyField = value;
				RaisePropertyChanged("DeliverQty");
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
	public string ITNote
	{
		get
		{
			return ITNoteField;
		}
		set
		{
			if ((object)ITNoteField != value)
			{
				ITNoteField = value;
				RaisePropertyChanged("ITNote");
			}
		}
	}

	[DataMember]
	public string LabelMode
	{
		get
		{
			return LabelModeField;
		}
		set
		{
			if ((object)LabelModeField != value)
			{
				LabelModeField = value;
				RaisePropertyChanged("LabelMode");
			}
		}
	}

	[DataMember]
	public string LastConfirmDate
	{
		get
		{
			return LastConfirmDateField;
		}
		set
		{
			if ((object)LastConfirmDateField != value)
			{
				LastConfirmDateField = value;
				RaisePropertyChanged("LastConfirmDate");
			}
		}
	}

	[DataMember]
	public string LineCode
	{
		get
		{
			return LineCodeField;
		}
		set
		{
			if ((object)LineCodeField != value)
			{
				LineCodeField = value;
				RaisePropertyChanged("LineCode");
			}
		}
	}

	[DataMember]
	public string MasterWONo
	{
		get
		{
			return MasterWONoField;
		}
		set
		{
			if ((object)MasterWONoField != value)
			{
				MasterWONoField = value;
				RaisePropertyChanged("MasterWONo");
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
	public string MatPostDate
	{
		get
		{
			return MatPostDateField;
		}
		set
		{
			if ((object)MatPostDateField != value)
			{
				MatPostDateField = value;
				RaisePropertyChanged("MatPostDate");
			}
		}
	}

	[DataMember]
	public bool NeedMerge
	{
		get
		{
			return NeedMergeField;
		}
		set
		{
			if (!NeedMergeField.Equals(value))
			{
				NeedMergeField = value;
				RaisePropertyChanged("NeedMerge");
			}
		}
	}

	[DataMember]
	public string OrderType
	{
		get
		{
			return OrderTypeField;
		}
		set
		{
			if ((object)OrderTypeField != value)
			{
				OrderTypeField = value;
				RaisePropertyChanged("OrderType");
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
	public string Priority
	{
		get
		{
			return PriorityField;
		}
		set
		{
			if ((object)PriorityField != value)
			{
				PriorityField = value;
				RaisePropertyChanged("Priority");
			}
		}
	}

	[DataMember]
	public string ProcessEndDate
	{
		get
		{
			return ProcessEndDateField;
		}
		set
		{
			if ((object)ProcessEndDateField != value)
			{
				ProcessEndDateField = value;
				RaisePropertyChanged("ProcessEndDate");
			}
		}
	}

	[DataMember]
	public string ProcessStartDate
	{
		get
		{
			return ProcessStartDateField;
		}
		set
		{
			if ((object)ProcessStartDateField != value)
			{
				ProcessStartDateField = value;
				RaisePropertyChanged("ProcessStartDate");
			}
		}
	}

	[DataMember]
	public string ProductStageCode
	{
		get
		{
			return ProductStageCodeField;
		}
		set
		{
			if ((object)ProductStageCodeField != value)
			{
				ProductStageCodeField = value;
				RaisePropertyChanged("ProductStageCode");
			}
		}
	}

	[DataMember]
	public bool Released
	{
		get
		{
			return ReleasedField;
		}
		set
		{
			if (!ReleasedField.Equals(value))
			{
				ReleasedField = value;
				RaisePropertyChanged("Released");
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
	public string Remark2
	{
		get
		{
			return Remark2Field;
		}
		set
		{
			if ((object)Remark2Field != value)
			{
				Remark2Field = value;
				RaisePropertyChanged("Remark2");
			}
		}
	}

	[DataMember]
	public string Remark3
	{
		get
		{
			return Remark3Field;
		}
		set
		{
			if ((object)Remark3Field != value)
			{
				Remark3Field = value;
				RaisePropertyChanged("Remark3");
			}
		}
	}

	[DataMember]
	public string ReworkReason
	{
		get
		{
			return ReworkReasonField;
		}
		set
		{
			if ((object)ReworkReasonField != value)
			{
				ReworkReasonField = value;
				RaisePropertyChanged("ReworkReason");
			}
		}
	}

	[DataMember]
	public string RouteNo
	{
		get
		{
			return RouteNoField;
		}
		set
		{
			if ((object)RouteNoField != value)
			{
				RouteNoField = value;
				RaisePropertyChanged("RouteNo");
			}
		}
	}

	[DataMember]
	public string SFC1010
	{
		get
		{
			return SFC1010Field;
		}
		set
		{
			if ((object)SFC1010Field != value)
			{
				SFC1010Field = value;
				RaisePropertyChanged("SFC1010");
			}
		}
	}

	[DataMember]
	public bool Scraped
	{
		get
		{
			return ScrapedField;
		}
		set
		{
			if (!ScrapedField.Equals(value))
			{
				ScrapedField = value;
				RaisePropertyChanged("Scraped");
			}
		}
	}

	[DataMember]
	public string SortString
	{
		get
		{
			return SortStringField;
		}
		set
		{
			if ((object)SortStringField != value)
			{
				SortStringField = value;
				RaisePropertyChanged("SortString");
			}
		}
	}

	[DataMember]
	public string SplitCode
	{
		get
		{
			return SplitCodeField;
		}
		set
		{
			if ((object)SplitCodeField != value)
			{
				SplitCodeField = value;
				RaisePropertyChanged("SplitCode");
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
	public string SyncDate
	{
		get
		{
			return SyncDateField;
		}
		set
		{
			if ((object)SyncDateField != value)
			{
				SyncDateField = value;
				RaisePropertyChanged("SyncDate");
			}
		}
	}

	[DataMember]
	public bool TECO
	{
		get
		{
			return TECOField;
		}
		set
		{
			if (!TECOField.Equals(value))
			{
				TECOField = value;
				RaisePropertyChanged("TECO");
			}
		}
	}

	[DataMember]
	public int TotalQty
	{
		get
		{
			return TotalQtyField;
		}
		set
		{
			if (!TotalQtyField.Equals(value))
			{
				TotalQtyField = value;
				RaisePropertyChanged("TotalQty");
			}
		}
	}

	[DataMember]
	public string TraceCode
	{
		get
		{
			return TraceCodeField;
		}
		set
		{
			if ((object)TraceCodeField != value)
			{
				TraceCodeField = value;
				RaisePropertyChanged("TraceCode");
			}
		}
	}

	[DataMember]
	public bool Urgent
	{
		get
		{
			return UrgentField;
		}
		set
		{
			if (!UrgentField.Equals(value))
			{
				UrgentField = value;
				RaisePropertyChanged("Urgent");
			}
		}
	}

	[DataMember]
	public int WIPQty
	{
		get
		{
			return WIPQtyField;
		}
		set
		{
			if (!WIPQtyField.Equals(value))
			{
				WIPQtyField = value;
				RaisePropertyChanged("WIPQty");
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
	public bool WOStatusCode
	{
		get
		{
			return WOStatusCodeField;
		}
		set
		{
			if (!WOStatusCodeField.Equals(value))
			{
				WOStatusCodeField = value;
				RaisePropertyChanged("WOStatusCode");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
