using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MatView_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MatView_GetCondition : Condition
{
	[OptionalField]
	private string MatNoField;

	[OptionalField]
	private string MatClassCodeField;

	[OptionalField]
	private string MatGroupNoField;

	[DataMember]
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

	[DataMember(Order = 1)]
	public string MatClassCode
	{
		get
		{
			return MatClassCodeField;
		}
		set
		{
			if ((object)MatClassCodeField != value)
			{
				MatClassCodeField = value;
				RaisePropertyChanged("MatClassCode");
			}
		}
	}

	[DataMember(Order = 2)]
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
}
