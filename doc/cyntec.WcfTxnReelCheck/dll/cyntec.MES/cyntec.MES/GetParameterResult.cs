using System;
using System.Collections.Generic;
using cyntec.WCF;

namespace cyntec.MES;

[Serializable]
public class GetParameterResult : TransactionResultBase
{
	public List<ParameterItem> _itemList = new List<ParameterItem>();

	public List<ParameterItem> _OriginalParamList = new List<ParameterItem>();

	public ParameterItem GetResultParameterItemByCode(string itemName)
	{
		foreach (ParameterItem item in _itemList)
		{
			if (item.ParamCode.ToUpper() == itemName.ToUpper())
			{
				return item;
			}
		}
		return null;
	}

	public ParameterItem GetResultParameterItem(string itemName)
	{
		foreach (ParameterItem item in _itemList)
		{
			if (item.ParamName.ToUpper() == itemName.ToUpper())
			{
				return item;
			}
		}
		return null;
	}

	public ParameterItem GetParameterItemByRefieldCode(string RefieldCode)
	{
		foreach (ParameterItem item in _itemList)
		{
			if (item.RefFieldCode.ToUpper() == RefieldCode.ToUpper())
			{
				return item;
			}
		}
		return null;
	}
}
