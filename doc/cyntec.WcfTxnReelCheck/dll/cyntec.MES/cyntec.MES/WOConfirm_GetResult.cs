using System;
using System.Collections.Generic;
using System.Linq;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WOConfirm_GetResult : TransactionResultBase
{
	public List<WOConfirm> _ItemList = new List<WOConfirm>();

	internal WOConfirm m_CurrentConfirm;

	public WOConfirm _CurrentConfirm => m_CurrentConfirm;

	public WOConfirm ByStepNo(string workcenterNo, int currentFlowID)
	{
		IEnumerable<WOConfirm> source = from o in _ItemList
			where o.WorkCenterNo == workcenterNo && o.FlowID >= currentFlowID && !o.Canceled
			orderby o.FlowID descending
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WOConfirm ByStepNo(string workcenterNo)
	{
		if (_CurrentConfirm == null)
		{
			throw new Exception("工單可能已經完工無法取得當前WOConfirm");
		}
		return ByStepNo(workcenterNo, _CurrentConfirm.FlowID);
	}

	public WOConfirm GetDesignatedStep(string workCenterNo)
	{
		IEnumerable<WOConfirm> source = from o in _ItemList
			where o.WorkCenterNo == workCenterNo && !o.Canceled
			orderby o.FlowID descending
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WOConfirm ByflowID(int flowID)
	{
		IEnumerable<WOConfirm> source = _ItemList.Where((WOConfirm o) => o.FlowID == flowID && !o.Canceled);
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}
}
