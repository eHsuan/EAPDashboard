using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class WO_ReelStatus_GetResult : TransactionResultBase
{
	public List<WO_ReelStatus> _ItemList = new List<WO_ReelStatus>();

	public string ToCodeName(string code)
	{
		foreach (WO_ReelStatus item in _ItemList)
		{
			if (item.Code == code)
			{
				return item.CodeName;
			}
		}
		return "";
	}
}
