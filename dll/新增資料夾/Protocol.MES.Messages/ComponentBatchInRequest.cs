using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentBatchInRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public bool AutoCheckIn { get; set; }

	public List<ComponentRefItem> Components { get; set; }

	public List<WOInputItem> WOInputItemDt { get; set; }

	public List<TraceMatBatchItem> TraceMatBatchDt { get; set; }

	public List<TOSItem> TOSDt { get; set; }

	public List<CarrierItem> WOCarrierDt { get; set; }

	public List<CheckItem> CheckItemDt { get; set; }

	public List<ParmItem> ParmListDt { get; set; }

	public List<EqpItem> EqpItemDt { get; set; }

	public ComponentBatchInRequest()
	{
		base.TransactionName = MESTransactionName.ComponentBatchIn;
	}
}
