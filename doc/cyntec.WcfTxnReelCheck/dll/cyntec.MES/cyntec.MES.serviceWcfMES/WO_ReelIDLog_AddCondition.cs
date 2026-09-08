using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelIDLog_AddCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelIDLog_AddCondition : Condition
{
	[OptionalField]
	private WO_ReelIDLog reelIDLogField;

	[DataMember]
	public WO_ReelIDLog reelIDLog
	{
		get
		{
			return reelIDLogField;
		}
		set
		{
			if (reelIDLogField != value)
			{
				reelIDLogField = value;
				RaisePropertyChanged("reelIDLog");
			}
		}
	}
}
