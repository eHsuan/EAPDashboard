using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace cyntec.MES.serviceWcfMES;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WoInfo_GetResult", Namespace = "http://schemas.datacontract.org/2004/07/WcfEntrance")]
public class WoInfo_GetResult : TxnInfo
{
	[OptionalField]
	private MatGroupNo[] MatGroupNoListField;

	[OptionalField]
	private MatView[] MatViewListField;

	[OptionalField]
	private WO_Block[] WOBlockListField;

	[OptionalField]
	private WOQueueTime[] WOQueueTimeListField;

	[OptionalField]
	private WORouting[] WORoutingListField;

	[OptionalField]
	private Workorder[] WO_GroupListField;

	[OptionalField]
	private WOConfirm[] WoConfirmListField;

	[OptionalField]
	private Workorder[] WoListField;

	[OptionalField]
	private Workcenter[] WocenterListField;

	[DataMember]
	public MatGroupNo[] MatGroupNoList
	{
		get
		{
			return MatGroupNoListField;
		}
		set
		{
			if (MatGroupNoListField != value)
			{
				MatGroupNoListField = value;
				RaisePropertyChanged("MatGroupNoList");
			}
		}
	}

	[DataMember]
	public MatView[] MatViewList
	{
		get
		{
			return MatViewListField;
		}
		set
		{
			if (MatViewListField != value)
			{
				MatViewListField = value;
				RaisePropertyChanged("MatViewList");
			}
		}
	}

	[DataMember]
	public WO_Block[] WOBlockList
	{
		get
		{
			return WOBlockListField;
		}
		set
		{
			if (WOBlockListField != value)
			{
				WOBlockListField = value;
				RaisePropertyChanged("WOBlockList");
			}
		}
	}

	[DataMember]
	public WOQueueTime[] WOQueueTimeList
	{
		get
		{
			return WOQueueTimeListField;
		}
		set
		{
			if (WOQueueTimeListField != value)
			{
				WOQueueTimeListField = value;
				RaisePropertyChanged("WOQueueTimeList");
			}
		}
	}

	[DataMember]
	public WORouting[] WORoutingList
	{
		get
		{
			return WORoutingListField;
		}
		set
		{
			if (WORoutingListField != value)
			{
				WORoutingListField = value;
				RaisePropertyChanged("WORoutingList");
			}
		}
	}

	[DataMember]
	public Workorder[] WO_GroupList
	{
		get
		{
			return WO_GroupListField;
		}
		set
		{
			if (WO_GroupListField != value)
			{
				WO_GroupListField = value;
				RaisePropertyChanged("WO_GroupList");
			}
		}
	}

	[DataMember]
	public WOConfirm[] WoConfirmList
	{
		get
		{
			return WoConfirmListField;
		}
		set
		{
			if (WoConfirmListField != value)
			{
				WoConfirmListField = value;
				RaisePropertyChanged("WoConfirmList");
			}
		}
	}

	[DataMember]
	public Workorder[] WoList
	{
		get
		{
			return WoListField;
		}
		set
		{
			if (WoListField != value)
			{
				WoListField = value;
				RaisePropertyChanged("WoList");
			}
		}
	}

	[DataMember]
	public Workcenter[] WocenterList
	{
		get
		{
			return WocenterListField;
		}
		set
		{
			if (WocenterListField != value)
			{
				WocenterListField = value;
				RaisePropertyChanged("WocenterList");
			}
		}
	}
}
