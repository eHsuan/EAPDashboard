using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Condition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
[KnownType(typeof(AccountUser_GetCondition))]
[KnownType(typeof(MESUser_GetCondition))]
[KnownType(typeof(WO_Leave_GetCondition))]
[KnownType(typeof(WO_ReelIDLog_AddCondition))]
[KnownType(typeof(EQP_TapingUILog_SaveCondition))]
[KnownType(typeof(EQP_TapingUILog_GetCondition))]
[KnownType(typeof(WoInfo_GetCondition))]
[KnownType(typeof(WO_BOM_GetCondition))]
[KnownType(typeof(WO_ReelID_GetCondition))]
[KnownType(typeof(MatView_GetCondition))]
[KnownType(typeof(MES_ParamItem_GetCondition))]
[KnownType(typeof(MachineRecord_SaveCondition))]
[KnownType(typeof(WO_CheckInCondition))]
[KnownType(typeof(WO_InputCode_GetCondition))]
[KnownType(typeof(WO_Mat_AddCondition))]
[KnownType(typeof(TraceMatCode_GetCondition))]
[KnownType(typeof(WO_ReelCheck_SaveCondition))]
[KnownType(typeof(WO_ReelCheck_GetCondition))]
[KnownType(typeof(WO_ReelID_Get_WCF_TEMPCondition))]
[KnownType(typeof(WO_ReelID_Update_WCF_TEMPCondition))]
[KnownType(typeof(Workorder_GetCondition))]
public class Condition : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string _ClientNameField;

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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
