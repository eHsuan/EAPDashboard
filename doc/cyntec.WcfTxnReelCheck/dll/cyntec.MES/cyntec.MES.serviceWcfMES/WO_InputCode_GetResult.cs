using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_InputCode_GetResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_InputCode_GetResult : TxnInfo
{
	[OptionalField]
	private WO_InputCode[] ItemListField;

	[DataMember]
	public WO_InputCode[] ItemList
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
