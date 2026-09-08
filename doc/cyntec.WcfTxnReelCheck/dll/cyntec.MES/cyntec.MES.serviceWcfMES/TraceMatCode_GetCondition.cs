using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TraceMatCode_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class TraceMatCode_GetCondition : Condition
{
	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string WorkcenternoField;

	[OptionalField]
	private int FlowIDField;

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
	public string Workcenterno
	{
		get
		{
			return WorkcenternoField;
		}
		set
		{
			if ((object)WorkcenternoField != value)
			{
				WorkcenternoField = value;
				RaisePropertyChanged("Workcenterno");
			}
		}
	}

	[DataMember(Order = 2)]
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
}
