using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class EQP_OP_CheckList_GetResult : TransactionResultBase
{
	public List<EQP_OP_CheckList> _ItemList = new List<EQP_OP_CheckList>();

	public bool IsContains(EQP_OP_OperationEnum op)
	{
		return IsContains(op.ToString());
	}

	public bool IsContains(string op)
	{
		IEnumerable<EQP_OP_CheckList> source = _ItemList.Where((EQP_OP_CheckList o) => o.Operation.Trim().ToUpper() == op.Trim().ToUpper());
		return source.Count() > 0;
	}

	public DataSet ToDataSet()
	{
		return ToDataSet(null);
	}

	public DataSet ToDataSet(string dataSetName)
	{
		DataSet dataSet = null;
		dataSet = ((!string.IsNullOrWhiteSpace(dataSetName)) ? new DataSet(dataSetName) : new DataSet(GetType().Name));
		DataTable mainTable = null;
		if (_ItemList != null)
		{
			foreach (EQP_OP_CheckList item in _ItemList)
			{
				item.ToDataTable(ref mainTable, null);
			}
			dataSet.Tables.Add(mainTable);
		}
		return dataSet;
	}
}
