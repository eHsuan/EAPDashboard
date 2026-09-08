using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TxnInfo", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
[KnownType(typeof(AccountUser_GetResult))]
[KnownType(typeof(MESUser_GetResult))]
[KnownType(typeof(WO_Leave_GetResult))]
[KnownType(typeof(WO_ReelIDLog_AddResult))]
[KnownType(typeof(EQP_TapingUILog_SaveResult))]
[KnownType(typeof(EQP_TapingUILog_GetResult))]
[KnownType(typeof(WoInfo_GetResult))]
[KnownType(typeof(WO_BOM_GetResult))]
[KnownType(typeof(WO_ReelID_GetResult))]
[KnownType(typeof(MatView_GetResult))]
[KnownType(typeof(MES_ParamItem_GetResult))]
[KnownType(typeof(MachineRecord_SaveResult))]
[KnownType(typeof(WO_CheckInResult))]
[KnownType(typeof(WO_InputCode_GetResult))]
[KnownType(typeof(WO_Mat_AddResult))]
[KnownType(typeof(TraceMatCode_GetResult))]
[KnownType(typeof(WO_ReelCheck_SaveResult))]
[KnownType(typeof(WO_ReelCheck_GetResult))]
[KnownType(typeof(WO_ReelID_Get_WCF_TEMPResult))]
[KnownType(typeof(WO_ReelID_Update_WCF_TEMPResult))]
[KnownType(typeof(Workorder_GetResult))]
public class TxnInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string _ClientNameField;

	[OptionalField]
	private bool _IsTxnErrorField;

	[OptionalField]
	private string _TxnErrorMsgField;

	[OptionalField]
	private string _TxnIDField;

	[OptionalField]
	private string _TxnSpendField;

	[OptionalField]
	private string _TxnTimeField;

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
	public string _ClientName
	{
		get
		{
			return _ClientNameField;
		}
		set
		{
			if ((object)_ClientNameField != value)
			{
				_ClientNameField = value;
				RaisePropertyChanged("_ClientName");
			}
		}
	}

	[DataMember]
	public bool _IsTxnError
	{
		get
		{
			return _IsTxnErrorField;
		}
		set
		{
			if (!_IsTxnErrorField.Equals(value))
			{
				_IsTxnErrorField = value;
				RaisePropertyChanged("_IsTxnError");
			}
		}
	}

	[DataMember]
	public string _TxnErrorMsg
	{
		get
		{
			return _TxnErrorMsgField;
		}
		set
		{
			if ((object)_TxnErrorMsgField != value)
			{
				_TxnErrorMsgField = value;
				RaisePropertyChanged("_TxnErrorMsg");
			}
		}
	}

	[DataMember]
	public string _TxnID
	{
		get
		{
			return _TxnIDField;
		}
		set
		{
			if ((object)_TxnIDField != value)
			{
				_TxnIDField = value;
				RaisePropertyChanged("_TxnID");
			}
		}
	}

	[DataMember]
	public string _TxnSpend
	{
		get
		{
			return _TxnSpendField;
		}
		set
		{
			if ((object)_TxnSpendField != value)
			{
				_TxnSpendField = value;
				RaisePropertyChanged("_TxnSpend");
			}
		}
	}

	[DataMember]
	public string _TxnTime
	{
		get
		{
			return _TxnTimeField;
		}
		set
		{
			if ((object)_TxnTimeField != value)
			{
				_TxnTimeField = value;
				RaisePropertyChanged("_TxnTime");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
