using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_ReelID_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_ReelID_GetCondition : Condition
{
	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string[] GorupWoNoListField;

	[OptionalField]
	private string ReelIDField;

	[OptionalField]
	private string BatchNoField;

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
	public string[] GorupWoNoList
	{
		get
		{
			return GorupWoNoListField;
		}
		set
		{
			if (GorupWoNoListField != value)
			{
				GorupWoNoListField = value;
				RaisePropertyChanged("GorupWoNoList");
			}
		}
	}

	[DataMember(Order = 2)]
	public string ReelID
	{
		get
		{
			return ReelIDField;
		}
		set
		{
			if ((object)ReelIDField != value)
			{
				ReelIDField = value;
				RaisePropertyChanged("ReelID");
			}
		}
	}

	[DataMember(Order = 3)]
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
}
