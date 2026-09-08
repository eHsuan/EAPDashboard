using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MES_ParamItem_GetResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class MES_ParamItem_GetResult : TxnInfo
{
	[OptionalField]
	private MES_ParamItem[] FinalParamListField;

	[OptionalField]
	private MES_ParamItem[] OrginalParamListField;

	[DataMember]
	public MES_ParamItem[] FinalParamList
	{
		get
		{
			return FinalParamListField;
		}
		set
		{
			if (FinalParamListField != value)
			{
				FinalParamListField = value;
				RaisePropertyChanged("FinalParamList");
			}
		}
	}

	[DataMember]
	public MES_ParamItem[] OrginalParamList
	{
		get
		{
			return OrginalParamListField;
		}
		set
		{
			if (OrginalParamListField != value)
			{
				OrginalParamListField = value;
				RaisePropertyChanged("OrginalParamList");
			}
		}
	}
}
