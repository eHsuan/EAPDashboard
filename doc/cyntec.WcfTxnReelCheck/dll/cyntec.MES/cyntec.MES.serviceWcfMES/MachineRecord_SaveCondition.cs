using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MachineRecord_SaveCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MachineRecord_SaveCondition : Condition
{
	[OptionalField]
	private MachineRecord[] ItemListField;

	[DataMember]
	public MachineRecord[] ItemList
	{
		get
		{
			return ItemListField;
		}
		set
		{
			if (ItemListField != value)
			{
				ItemListField = value;
				RaisePropertyChanged("ItemList");
			}
		}
	}
}
