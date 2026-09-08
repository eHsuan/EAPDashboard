using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EQP_TapingUILog_SaveCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class EQP_TapingUILog_SaveCondition : Condition
{
	[OptionalField]
	private EQP_TapingUILog TapingLogField;

	[DataMember]
	public EQP_TapingUILog TapingLog
	{
		get
		{
			return TapingLogField;
		}
		set
		{
			if (TapingLogField != value)
			{
				TapingLogField = value;
				RaisePropertyChanged("TapingLog");
			}
		}
	}
}
