using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_InputCode_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_InputCode_GetCondition : Condition
{
	[OptionalField]
	private string MatGroupNoField;

	[OptionalField]
	private string WorkcenterNoField;

	[DataMember]
	public string MatGroupNo
	{
		get
		{
			return MatGroupNoField;
		}
		set
		{
			if ((object)MatGroupNoField != value)
			{
				MatGroupNoField = value;
				RaisePropertyChanged("MatGroupNo");
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
}
