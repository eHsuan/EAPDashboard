using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_GetResult : TransactionResultBase
{
	public List<WO_ReelID> _ItemList = new List<WO_ReelID>();

	public string _TapingNo
	{
		get
		{
			if (_ItemList.Count == 0)
			{
				return "";
			}
			return _ItemList[0].BatchNo;
		}
	}

	public string _TapingNoByMatNo(string matNo)
	{
		if (_ItemList.Count == 0)
		{
			return "";
		}
		IEnumerable<WO_ReelID> source = _ItemList.Where((WO_ReelID o) => o.MatNo.ToUpper() == matNo.ToUpper());
		if (source.Count() == 0)
		{
			return "";
		}
		return source.ElementAt(0).BatchNo;
	}

	public bool _IsLastReel(string reelID)
	{
		if (_ItemList.Count == 0)
		{
			return true;
		}
		IEnumerable<WO_ReelID> source = _ItemList.Where((WO_ReelID o) => o.ReelID == reelID && string.IsNullOrWhiteSpace(o.TPMachNo));
		if (source.Count() == 1)
		{
			return true;
		}
		source = _ItemList.OrderBy((WO_ReelID o) => o.TPStartTime);
		if (source.ElementAt(source.Count() - 1).ReelID.ToUpper().Trim() == reelID.ToUpper().Trim())
		{
			return true;
		}
		return false;
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
			foreach (WO_ReelID item in _ItemList)
			{
				item.ToDataTable(ref mainTable, null);
			}
			dataSet.Tables.Add(mainTable);
		}
		return dataSet;
	}
}
