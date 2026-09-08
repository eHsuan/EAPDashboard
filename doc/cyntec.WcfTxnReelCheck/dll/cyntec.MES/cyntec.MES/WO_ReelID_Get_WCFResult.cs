using System;
using System.Collections.Generic;
using System.Data;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_Get_WCFResult : TransactionResultBase
{
	public List<WO_ReelID> _itemList = new List<WO_ReelID>();

	public DataSet ToDataSet()
	{
		return ToDataSet(null);
	}

	public DataSet ToDataSet(string dataSetName)
	{
		DataSet dataSet = null;
		dataSet = ((!string.IsNullOrWhiteSpace(dataSetName)) ? new DataSet(dataSetName) : new DataSet(GetType().Name));
		DataTable mainTable = null;
		if (_itemList != null)
		{
			foreach (WO_ReelID item in _itemList)
			{
				item.ToDataTable(ref mainTable, null);
			}
			dataSet.Tables.Add(mainTable);
		}
		return dataSet;
	}
}
