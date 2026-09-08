using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_BOM_GetCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_BOM_GetCondition : Condition
{
	[OptionalField]
	private string WoNoField;

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
}
