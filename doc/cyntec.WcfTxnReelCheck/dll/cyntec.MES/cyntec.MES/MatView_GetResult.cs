using System;
using System.Collections.Generic;
using System.Data;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class MatView_GetResult : TransactionResultBase
{
	public List<MatView> _itemList = new List<MatView>();

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
			foreach (MatView item in _itemList)
			{
				item.ToDataTable(ref mainTable, "_ItemList");
			}
			dataSet.Tables.Add(mainTable);
		}
		return dataSet;
	}
}
