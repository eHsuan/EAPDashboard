using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MachineRecord", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MachineRecord : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int IDField;

	[OptionalField]
	private int MachIDField;

	[OptionalField]
	private string MachineNoField;

	[OptionalField]
	private string LineCodeField;

	[OptionalField]
	private string PlantNoField;

	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string WorkCenterNoField;

	[OptionalField]
	private string ConfirmWorkCenterField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string OutPutQtyField;

	[OptionalField]
	private string OperatorField;

	[OptionalField]
	private string IPQCField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private int FlowIDField;

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
	public int MachID
	{
		get
		{
			return MachIDField;
		}
		set
		{
			if (!MachIDField.Equals(value))
			{
				MachIDField = value;
				RaisePropertyChanged("MachID");
			}
		}
	}

	[DataMember]
	public string MachineNo
	{
		get
		{
			return MachineNoField;
		}
		set
		{
			if ((object)MachineNoField != value)
			{
				MachineNoField = value;
				RaisePropertyChanged("MachineNo");
			}
		}
	}

	[DataMember(Order = 3)]
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

	[DataMember(Order = 4)]
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

	[DataMember(Order = 5)]
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

	[DataMember(Order = 6)]
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

	[DataMember(Order = 7)]
	public string ConfirmWorkCenter
	{
		get
		{
			return ConfirmWorkCenterField;
		}
		set
		{
			if ((object)ConfirmWorkCenterField != value)
			{
				ConfirmWorkCenterField = value;
				RaisePropertyChanged("ConfirmWorkCenter");
			}
		}
	}

	[DataMember(Order = 8)]
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

	[DataMember(Order = 9)]
	public string OutPutQty
	{
		get
		{
			return OutPutQtyField;
		}
		set
		{
			if ((object)OutPutQtyField != value)
			{
				OutPutQtyField = value;
				RaisePropertyChanged("OutPutQty");
			}
		}
	}

	[DataMember(Order = 10)]
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

	[DataMember(Order = 11)]
	public string IPQC
	{
		get
		{
			return IPQCField;
		}
		set
		{
			if ((object)IPQCField != value)
			{
				IPQCField = value;
				RaisePropertyChanged("IPQC");
			}
		}
	}

	[DataMember(Order = 12)]
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

	[DataMember(Order = 13)]
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

	[DataMember(Order = 14)]
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

	[DataMember(Order = 15)]
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
