using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Workorder_GetResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class Workorder_GetResult : TxnInfo
{
	[OptionalField]
	private Workorder[] ItemListField;

	[DataMember]
	public Workorder[] ItemList
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
