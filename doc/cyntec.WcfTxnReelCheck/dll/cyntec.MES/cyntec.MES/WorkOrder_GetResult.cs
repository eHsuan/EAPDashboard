using System;
using System.Collections.Generic;
using System.Data;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WorkOrder_GetResult : TransactionResultBase
{
	public List<WorkOrder> _ItemList = new List<WorkOrder>();

	public DataSet ToDataSet()
	{
		return ToDataSet(null);
	}

	public DataSet ToDataSet(string dataSetName)
	{
		DataSet dataSet = null;
		dataSet = ((!string.IsNullOrWhiteSpace(dataSetName)) ? new DataSet(dataSetName) : new DataSet(GetType().Name));
		dataSet.Tables.Add(WorkOrder.ToDataTable(_ItemList, null));
		return dataSet;
	}
}
