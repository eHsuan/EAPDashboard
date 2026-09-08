using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_BOM_GetResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_BOM_GetResult : TxnInfo
{
	[OptionalField]
	private WO_BOM[] ItemListField;

	[OptionalField]
	private WO_BOM To531Field;

	[DataMember]
	public WO_BOM[] ItemList
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

	[DataMember]
	public WO_BOM To531
	{
		get
		{
			return To531Field;
		}
		set
		{
			if (To531Field != value)
			{
				To531Field = value;
				RaisePropertyChanged("To531");
			}
		}
	}
}
