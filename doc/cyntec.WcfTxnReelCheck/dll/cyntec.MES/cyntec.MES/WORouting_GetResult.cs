using System;
using System.Collections.Generic;
using System.Linq;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WORouting_GetResult : TransactionResultBase
{
	public List<WORouting> _ItemList = new List<WORouting>();

	public WorkOrder _WO = null;

	public DateTime _AccessTime = DateTime.Now;

	public WORouting ByStepNo(string workcenterNo)
	{
		IEnumerable<WORouting> source = from o in _ItemList
			where o.WorkCenterNo == workcenterNo && o.Enable
			orderby o.FlowID descending
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WORouting ByStepNo(string workcenterNo, int currentFlowID)
	{
		IEnumerable<WORouting> source = from o in _ItemList
			where o.WorkCenterNo == workcenterNo && o.Enable && o.FlowID >= currentFlowID
			orderby o.FlowID descending
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WORouting PreStep(int currentFlowID)
	{
		IEnumerable<WORouting> source = from o in _ItemList
			where o.FlowID < currentFlowID && o.Enable
			orderby o.FlowID descending
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WORouting NextStep(int currentFlowID)
	{
		IEnumerable<WORouting> source = from o in _ItemList
			where o.FlowID > currentFlowID && o.Enable
			orderby o.FlowID
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WORouting ByFlowID(int flowID)
	{
		IEnumerable<WORouting> source = _ItemList.Where((WORouting o) => o.FlowID == flowID && o.Enable);
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		return null;
	}

	public WORouting FQCStep(int currentFlowID)
	{
		IOrderedEnumerable<WORouting> source = from o in _ItemList
			where o.FlowID > currentFlowID && o.WorkcenterTypeCode == "QF" && o.Enable
			orderby o.FlowID
			select o;
		if (source.Count() == 0)
		{
			return null;
		}
		return source.ElementAt(0);
	}

	public WORouting TapingStep(int currentFlowID)
	{
		IOrderedEnumerable<WORouting> source = from o in _ItemList
			where o.FlowID >= currentFlowID && o.WorkcenterTypeCode == "TP" && o.Enable
			orderby o.FlowID
			select o;
		if (source.Count() == 0)
		{
			return null;
		}
		return source.ElementAt(0);
	}

	public List<string> ToWorkcenterNoList()
	{
		List<string> list = new List<string>();
		foreach (WORouting item in _ItemList)
		{
			list.Add(item.WorkCenterNo);
		}
		return list;
	}

	public WORouting GetDesignatedStep(string stepName)
	{
		IEnumerable<WORouting> source = from o in _ItemList
			where o.WorkcenterTypeCode == stepName && o.Enable
			orderby o.FlowID
			select o;
		if (source.Count() > 0)
		{
			return source.ElementAt(source.Count() - 1);
		}
		return null;
	}
}
