using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MES_ParamItem_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MES_ParamItem_GetCondition : Condition
{
	[OptionalField]
	private string MaterialField;

	[OptionalField]
	private string StartDateField;

	[OptionalField]
	private string WoNoField;

	[OptionalField]
	private string WorkcenterNoField;

	[DataMember]
	public string Material
	{
		get
		{
			return MaterialField;
		}
		set
		{
			if ((object)MaterialField != value)
			{
				MaterialField = value;
				RaisePropertyChanged("Material");
			}
		}
	}

	[DataMember]
	public string StartDate
	{
		get
		{
			return StartDateField;
		}
		set
		{
			if ((object)StartDateField != value)
			{
				StartDateField = value;
				RaisePropertyChanged("StartDate");
			}
		}
	}

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
}
