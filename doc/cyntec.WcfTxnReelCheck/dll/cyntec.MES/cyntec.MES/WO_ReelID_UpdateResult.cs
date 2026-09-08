using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelID_UpdateResult : TransactionResultBase
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
}
