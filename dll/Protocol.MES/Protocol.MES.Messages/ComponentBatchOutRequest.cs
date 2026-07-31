using System.Collections.Generic;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace Protocol.MES.Messages;

public class ComponentBatchOutRequest : MESBaseMessage
{
	public string WONO { get; set; }

	public string Remark { get; set; }

	public string RefWO { get; set; }

	public string RefWOGroupID { get; set; }

	public bool AutoCheckOut { get; set; }

	public List<ComponentRefItem> Components { get; set; }

	public List<WOInputItem> WOInputItemDt { get; set; }

	public List<TOSItem> TOSDt { get; set; }

	public List<CheckItem> CheckItemDt { get; set; }

	public List<ParmItem> ParmListDt { get; set; }

	public ComponentBatchOutRequest()
	{
		base.TransactionName = MESTransactionName.ComponentBatchOut;
	}
}
