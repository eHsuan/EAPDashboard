using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WoInfo_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WoInfo_GetCondition : Condition
{
	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string BatchNoField;

	[OptionalField]
	private bool IsCheckWoField;

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

	[DataMember(Order = 1)]
	public string BatchNo
	{
		get
		{
			return BatchNoField;
		}
		set
		{
			if ((object)BatchNoField != value)
			{
				BatchNoField = value;
				RaisePropertyChanged("BatchNo");
			}
		}
	}

	[DataMember(Order = 2)]
	public bool IsCheckWo
	{
		get
		{
			return IsCheckWoField;
		}
		set
		{
			if (!IsCheckWoField.Equals(value))
			{
				IsCheckWoField = value;
				RaisePropertyChanged("IsCheckWo");
			}
		}
	}
}
