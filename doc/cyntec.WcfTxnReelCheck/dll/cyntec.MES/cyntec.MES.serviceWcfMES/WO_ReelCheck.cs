using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelCheck", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelCheck : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string ReelIDField;

	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string TPNOField;

	[OptionalField]
	private string EqpNoField;

	[OptionalField]
	private string ComputerNameField;

	[OptionalField]
	private int ReelIDCountField;

	[OptionalField]
	private int MatNoCountField;

	[OptionalField]
	private int TPNoCountField;

	[OptionalField]
	private string StartTimeField;

	[OptionalField]
	private string UpdateTimeField;

	[OptionalField]
	private string TapingUILogTxnIDField;

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

	[DataMember(Order = 2)]
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

	[DataMember(Order = 3)]
	public string TPNO
	{
		get
		{
			return TPNOField;
		}
		set
		{
			if ((object)TPNOField != value)
			{
				TPNOField = value;
				RaisePropertyChanged("TPNO");
			}
		}
	}

	[DataMember(Order = 4)]
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

	[DataMember(Order = 5)]
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

	[DataMember(Order = 6)]
	public int ReelIDCount
	{
		get
		{
			return ReelIDCountField;
		}
		set
		{
			if (!ReelIDCountField.Equals(value))
			{
				ReelIDCountField = value;
				RaisePropertyChanged("ReelIDCount");
			}
		}
	}

	[DataMember(Order = 7)]
	public int MatNoCount
	{
		get
		{
			return MatNoCountField;
		}
		set
		{
			if (!MatNoCountField.Equals(value))
			{
				MatNoCountField = value;
				RaisePropertyChanged("MatNoCount");
			}
		}
	}

	[DataMember(Order = 8)]
	public int TPNoCount
	{
		get
		{
			return TPNoCountField;
		}
		set
		{
			if (!TPNoCountField.Equals(value))
			{
				TPNoCountField = value;
				RaisePropertyChanged("TPNoCount");
			}
		}
	}

	[DataMember(Order = 9)]
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

	[DataMember(Order = 10)]
	public string UpdateTime
	{
		get
		{
			return UpdateTimeField;
		}
		set
		{
			if ((object)UpdateTimeField != value)
			{
				UpdateTimeField = value;
				RaisePropertyChanged("UpdateTime");
			}
		}
	}

	[DataMember(Order = 11)]
	public string TapingUILogTxnID
	{
		get
		{
			return TapingUILogTxnIDField;
		}
		set
		{
			if ((object)TapingUILogTxnIDField != value)
			{
				TapingUILogTxnIDField = value;
				RaisePropertyChanged("TapingUILogTxnID");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
