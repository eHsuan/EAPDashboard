using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MESUser_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MESUser_GetCondition : Condition
{
	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string WorkcenterNoField;

	[DataMember]
	public string UserID
	{
		get
		{
			return UserIDField;
		}
		set
		{
			if ((object)UserIDField != value)
			{
				UserIDField = value;
				RaisePropertyChanged("UserID");
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
