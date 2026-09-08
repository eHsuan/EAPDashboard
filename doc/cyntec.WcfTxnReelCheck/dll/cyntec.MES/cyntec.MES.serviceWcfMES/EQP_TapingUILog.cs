using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EQP_TapingUILog", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class EQP_TapingUILog : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int ConfirmIDField;

	[OptionalField]
	private string TxnIDField;

	[OptionalField]
	private int LogTypeField;

	[OptionalField]
	private string UINameField;

	[OptionalField]
	private string UIVersionField;

	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string WorkcenterNoField;

	[OptionalField]
	private string EqpNoField;

	[OptionalField]
	private string ComputerNameField;

	[OptionalField]
	private int ActionField;

	[OptionalField]
	private string RemarkField;

	[OptionalField]
	private string CreateDateField;

	[OptionalField]
	private string CreateUserField;

	[OptionalField]
	private int FlowIDField;

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

	[DataMember]
	public string TxnID
	{
		get
		{
			return TxnIDField;
		}
		set
		{
			if ((object)TxnIDField != value)
			{
				TxnIDField = value;
				RaisePropertyChanged("TxnID");
			}
		}
	}

	[DataMember(Order = 2)]
	public int LogType
	{
		get
		{
			return LogTypeField;
		}
		set
		{
			if (!LogTypeField.Equals(value))
			{
				LogTypeField = value;
				RaisePropertyChanged("LogType");
			}
		}
	}

	[DataMember(Order = 3)]
	public string UIName
	{
		get
		{
			return UINameField;
		}
		set
		{
			if ((object)UINameField != value)
			{
				UINameField = value;
				RaisePropertyChanged("UIName");
			}
		}
	}

	[DataMember(Order = 4)]
	public string UIVersion
	{
		get
		{
			return UIVersionField;
		}
		set
		{
			if ((object)UIVersionField != value)
			{
				UIVersionField = value;
				RaisePropertyChanged("UIVersion");
			}
		}
	}

	[DataMember(Order = 5)]
	public string WoNo
	{
		get
		{
			return WoNoField;
		}
		set
		{
			if ((object)WoNoField != value)
			{
				WoNoField = value;
				RaisePropertyChanged("WoNo");
			}
		}
	}

	[DataMember(Order = 6)]
	public string WorkcenterNo
	{
		get
		{
			return WorkcenterNoField;
		}
		set
		{
			if ((object)WorkcenterNoField != value)
			{
				WorkcenterNoField = value;
				RaisePropertyChanged("WorkcenterNo");
			}
		}
	}

	[DataMember(Order = 7)]
	public string EqpNo
	{
		get
		{
			return EqpNoField;
		}
		set
		{
			if ((object)EqpNoField != value)
			{
				EqpNoField = value;
				RaisePropertyChanged("EqpNo");
			}
		}
	}

	[DataMember(Order = 8)]
	public string ComputerName
	{
		get
		{
			return ComputerNameField;
		}
		set
		{
			if ((object)ComputerNameField != value)
			{
				ComputerNameField = value;
				RaisePropertyChanged("ComputerName");
			}
		}
	}

	[DataMember(Order = 9)]
	public int Action
	{
		get
		{
			return ActionField;
		}
		set
		{
			if (!ActionField.Equals(value))
			{
				ActionField = value;
				RaisePropertyChanged("Action");
			}
		}
	}

	[DataMember(Order = 10)]
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

	[DataMember(Order = 11)]
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

	[DataMember(Order = 12)]
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

	[DataMember(Order = 13)]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
