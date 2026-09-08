using System;
using System.Collections.Generic;
using System.Data;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class Mach_Master_GetResult : TransactionResultBase
{
	public List<Mach_Master> _ItemList = new List<Mach_Master>();

	public DataSet ToDataSet()
	{
		return ToDataSet(null);
	}

	public DataSet ToDataSet(string dataSetName)
	{
		DataSet dataSet = null;
		dataSet = ((!string.IsNullOrWhiteSpace(dataSetName)) ? new DataSet(dataSetName) : new DataSet(GetType().Name));
		DataTable mainTable = Mach_Master.NewDataTable(null);
		if (_ItemList != null && _ItemList.Count > 0)
		{
			foreach (Mach_Master item in _ItemList)
			{
				item.ToDataTable(ref mainTable, null);
			}
		}
		dataSet.Tables.Add(mainTable);
		return dataSet;
	}
}
