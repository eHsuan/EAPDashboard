using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WOQueueTime", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WOQueueTime : IExtensibleDataObject, INotifyPropertyChanged
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
	private bool EnableField;

	[OptionalField]
	private string EndWorkcenterNoField;

	[OptionalField]
	private bool IsTimeOutField;

	[OptionalField]
	private double QuotaTimesField;

	[OptionalField]
	private DateTime ServerCurrentTimeField;

	[OptionalField]
	private string StartTimeField;

	[OptionalField]
	private string StartWorkcenterNoField;

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
	public string EndWorkcenterNo
	{
		get
		{
			return EndWorkcenterNoField;
		}
		set
		{
			if ((object)EndWorkcenterNoField != value)
			{
				EndWorkcenterNoField = value;
				RaisePropertyChanged("EndWorkcenterNo");
			}
		}
	}

	[DataMember]
	public bool IsTimeOut
	{
		get
		{
			return IsTimeOutField;
		}
		set
		{
			if (!IsTimeOutField.Equals(value))
			{
				IsTimeOutField = value;
				RaisePropertyChanged("IsTimeOut");
			}
		}
	}

	[DataMember]
	public double QuotaTimes
	{
		get
		{
			return QuotaTimesField;
		}
		set
		{
			if (!QuotaTimesField.Equals(value))
			{
				QuotaTimesField = value;
				RaisePropertyChanged("QuotaTimes");
			}
		}
	}

	[DataMember]
	public DateTime ServerCurrentTime
	{
		get
		{
			return ServerCurrentTimeField;
		}
		set
		{
			if (!ServerCurrentTimeField.Equals(value))
			{
				ServerCurrentTimeField = value;
				RaisePropertyChanged("ServerCurrentTime");
			}
		}
	}

	[DataMember]
	public string StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			if ((object)StartTimeField != value)
			{
				StartTimeField = value;
				RaisePropertyChanged("StartTime");
			}
		}
	}

	[DataMember]
	public string StartWorkcenterNo
	{
		get
		{
			return StartWorkcenterNoField;
		}
		set
		{
			if ((object)StartWorkcenterNoField != value)
			{
				StartWorkcenterNoField = value;
				RaisePropertyChanged("StartWorkcenterNo");
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
