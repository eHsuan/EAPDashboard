using System.Collections.Generic;

namespace cyntec.Base;

public class cyntecList
{
	public static void SetItem<T, N>(T key, N value, Dictionary<T, N> itemList)
	{
		if (itemList.ContainsKey(key))
		{
			itemList[key] = value;
		}
		else
		{
			itemList.Add(key, value);
		}
	}
}
