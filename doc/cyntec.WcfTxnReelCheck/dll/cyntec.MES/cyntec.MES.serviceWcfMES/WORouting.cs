using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WORouting", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WORouting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CalcNGField;

	[OptionalField]
	private string ChangeDateField;

	[OptionalField]
	private string ChangeUserField;

	[OptionalField]
	private bool ConfirmationField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private bool DeliveredField;

	[OptionalField]
	private bool EnableField;

	[OptionalField]
	private int FlowIDField;

	[OptionalField]
	private int IDField;

	[OptionalField]
	private bool IOEqualField;

	[OptionalField]
	private bool InCheckField;

	[OptionalField]
	private string InRefWorkCenterField;

	[OptionalField]
	private bool IsEquipmentField;

	[OptionalField]
	private bool IsMeasureField;

	[OptionalField]
	private bool IsMeterField;

	[OptionalField]
	private bool IsPackingField;

	[OptionalField]
	private bool IsPlanField;

	[OptionalField]
	private bool IsReworkField;

	[OptionalField]
	private bool IsTapingField;

	[OptionalField]
	private bool LineProductionField;

	[OptionalField]
	private string MeasureDataBaseField;

	[OptionalField]
	private bool RankField;

	[OptionalField]
	private string RefWC01Field;

	[OptionalField]
	private string SAPConfirmWorkCenterField;

	[OptionalField]
	private string SAPWorkCenterField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string WorkCenterNameField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string WorkcenterTypeCodeField;

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
	public string CalcNG
	{
		get
		{
			return CalcNGField;
		}
		set
		{
			if ((object)CalcNGField != value)
			{
				CalcNGField = value;
				RaisePropertyChanged("CalcNG");
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
	public bool Confirmation
	{
		get
		{
			return ConfirmationField;
		}
		set
		{
			if (!ConfirmationField.Equals(value))
			{
				ConfirmationField = value;
				RaisePropertyChanged("Confirmation");
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
	public bool Delivered
	{
		get
		{
			return DeliveredField;
		}
		set
		{
			if (!DeliveredField.Equals(value))
			{
				DeliveredField = value;
				RaisePropertyChanged("Delivered");
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
	public bool IOEqual
	{
		get
		{
			return IOEqualField;
		}
		set
		{
			if (!IOEqualField.Equals(value))
			{
				IOEqualField = value;
				RaisePropertyChanged("IOEqual");
			}
		}
	}

	[DataMember]
	public bool InCheck
	{
		get
		{
			return InCheckField;
		}
		set
		{
			if (!InCheckField.Equals(value))
			{
				InCheckField = value;
				RaisePropertyChanged("InCheck");
			}
		}
	}

	[DataMember]
	public string InRefWorkCenter
	{
		get
		{
			return InRefWorkCenterField;
		}
		set
		{
			if ((object)InRefWorkCenterField != value)
			{
				InRefWorkCenterField = value;
				RaisePropertyChanged("InRefWorkCenter");
			}
		}
	}

	[DataMember]
	public bool IsEquipment
	{
		get
		{
			return IsEquipmentField;
		}
		set
		{
			if (!IsEquipmentField.Equals(value))
			{
				IsEquipmentField = value;
				RaisePropertyChanged("IsEquipment");
			}
		}
	}

	[DataMember]
	public bool IsMeasure
	{
		get
		{
			return IsMeasureField;
		}
		set
		{
			if (!IsMeasureField.Equals(value))
			{
				IsMeasureField = value;
				RaisePropertyChanged("IsMeasure");
			}
		}
	}

	[DataMember]
	public bool IsMeter
	{
		get
		{
			return IsMeterField;
		}
		set
		{
			if (!IsMeterField.Equals(value))
			{
				IsMeterField = value;
				RaisePropertyChanged("IsMeter");
			}
		}
	}

	[DataMember]
	public bool IsPacking
	{
		get
		{
			return IsPackingField;
		}
		set
		{
			if (!IsPackingField.Equals(value))
			{
				IsPackingField = value;
				RaisePropertyChanged("IsPacking");
			}
		}
	}

	[DataMember]
	public bool IsPlan
	{
		get
		{
			return IsPlanField;
		}
		set
		{
			if (!IsPlanField.Equals(value))
			{
				IsPlanField = value;
				RaisePropertyChanged("IsPlan");
			}
		}
	}

	[DataMember]
	public bool IsRework
	{
		get
		{
			return IsReworkField;
		}
		set
		{
			if (!IsReworkField.Equals(value))
			{
				IsReworkField = value;
				RaisePropertyChanged("IsRework");
			}
		}
	}

	[DataMember]
	public bool IsTaping
	{
		get
		{
			return IsTapingField;
		}
		set
		{
			if (!IsTapingField.Equals(value))
			{
				IsTapingField = value;
				RaisePropertyChanged("IsTaping");
			}
		}
	}

	[DataMember]
	public bool LineProduction
	{
		get
		{
			return LineProductionField;
		}
		set
		{
			if (!LineProductionField.Equals(value))
			{
				LineProductionField = value;
				RaisePropertyChanged("LineProduction");
			}
		}
	}

	[DataMember]
	public string MeasureDataBase
	{
		get
		{
			return MeasureDataBaseField;
		}
		set
		{
			if ((object)MeasureDataBaseField != value)
			{
				MeasureDataBaseField = value;
				RaisePropertyChanged("MeasureDataBase");
			}
		}
	}

	[DataMember]
	public bool Rank
	{
		get
		{
			return RankField;
		}
		set
		{
			if (!RankField.Equals(value))
			{
				RankField = value;
				RaisePropertyChanged("Rank");
			}
		}
	}

	[DataMember]
	public string RefWC01
	{
		get
		{
			return RefWC01Field;
		}
		set
		{
			if ((object)RefWC01Field != value)
			{
				RefWC01Field = value;
				RaisePropertyChanged("RefWC01");
			}
		}
	}

	[DataMember]
	public string SAPConfirmWorkCenter
	{
		get
		{
			return SAPConfirmWorkCenterField;
		}
		set
		{
			if ((object)SAPConfirmWorkCenterField != value)
			{
				SAPConfirmWorkCenterField = value;
				RaisePropertyChanged("SAPConfirmWorkCenter");
			}
		}
	}

	[DataMember]
	public string SAPWorkCenter
	{
		get
		{
			return SAPWorkCenterField;
		}
		set
		{
			if ((object)SAPWorkCenterField != value)
			{
				SAPWorkCenterField = value;
				RaisePropertyChanged("SAPWorkCenter");
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
	public string WorkcenterTypeCode
	{
		get
		{
			return WorkcenterTypeCodeField;
		}
		set
		{
			if ((object)WorkcenterTypeCodeField != value)
			{
				WorkcenterTypeCodeField = value;
				RaisePropertyChanged("WorkcenterTypeCode");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
