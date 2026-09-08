using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_Mat_AddCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_Mat_AddCondition : Condition
{
	[OptionalField]
	private string EqpNoField;

	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string WONoField;

	[OptionalField]
	private MESKeyValue[] TowDListDtField;

	[DataMember]
	public string EqpNo
	{
		get
		{
			return EqpNoField;
		}
		set
		{
			if ((object)EqpNoField != value)
			{
				EqpNoField = value;
				RaisePropertyChanged("EqpNo");
			}
		}
	}

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
	public string WONo
	{
		get
		{
			return WONoField;
		}
		set
		{
			if ((object)WONoField != value)
			{
				WONoField = value;
				RaisePropertyChanged("WONo");
			}
		}
	}

	[DataMember(Order = 3)]
	public MESKeyValue[] TowDListDt
	{
		get
		{
			return TowDListDtField;
		}
		set
		{
			if (TowDListDtField != value)
			{
				TowDListDtField = value;
				RaisePropertyChanged("TowDListDt");
			}
		}
	}
}
