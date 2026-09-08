using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelCheck_SaveCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelCheck_SaveCondition : Condition
{
	[OptionalField]
	private WO_ReelCheck[] WoReelCheckListField;

	[DataMember]
	public WO_ReelCheck[] WoReelCheckList
	{
		get
		{
			return WoReelCheckListField;
		}
		set
		{
			if (WoReelCheckListField != value)
			{
				WoReelCheckListField = value;
				RaisePropertyChanged("WoReelCheckList");
			}
		}
	}
}
