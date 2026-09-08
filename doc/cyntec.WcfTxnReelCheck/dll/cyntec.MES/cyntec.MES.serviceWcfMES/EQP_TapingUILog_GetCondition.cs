using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EQP_TapingUILog_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class EQP_TapingUILog_GetCondition : Condition
{
	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string WorkcenterNoField;

	[OptionalField]
	private string EqpNoField;

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

	[DataMember]
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

	[DataMember(Order = 2)]
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
}
