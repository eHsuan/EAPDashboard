using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WO_CheckInCondition", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WO_CheckInCondition : Condition
{
	[OptionalField]
	private string WONoField;

	[OptionalField]
	private string UserIDField;

	[OptionalField]
	private string EqpNoField;

	[OptionalField]
	private MESKeyValue[] InputListDtField;

	[OptionalField]
	private MESKeyValue[] TraceMatBatchDtField;

	[OptionalField]
	private MESKeyValue[] WOCarrierDtField;

	[OptionalField]
	private MESKeyValue[] ParmListDtField;

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

	[DataMember(Order = 1)]
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

	[DataMember(Order = 2)]
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

	[DataMember(Order = 3)]
	public MESKeyValue[] InputListDt
	{
		get
		{
			return InputListDtField;
		}
		set
		{
			if (InputListDtField != value)
			{
				InputListDtField = value;
				RaisePropertyChanged("InputListDt");
			}
		}
	}

	[DataMember(Order = 4)]
	public MESKeyValue[] TraceMatBatchDt
	{
		get
		{
			return TraceMatBatchDtField;
		}
		set
		{
			if (TraceMatBatchDtField != value)
			{
				TraceMatBatchDtField = value;
				RaisePropertyChanged("TraceMatBatchDt");
			}
		}
	}

	[DataMember(Order = 5)]
	public MESKeyValue[] WOCarrierDt
	{
		get
		{
			return WOCarrierDtField;
		}
		set
		{
			if (WOCarrierDtField != value)
			{
				WOCarrierDtField = value;
				RaisePropertyChanged("WOCarrierDt");
			}
		}
	}

	[DataMember(Order = 6)]
	public MESKeyValue[] ParmListDt
	{
		get
		{
			return ParmListDtField;
		}
		set
		{
			if (ParmListDtField != value)
			{
				ParmListDtField = value;
				RaisePropertyChanged("ParmListDt");
			}
		}
	}
}
