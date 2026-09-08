using System;

namespace cyntec.Delta;

public class Barcode2D88
{
	public string MatNo = "";

	public string TapingNo = "";

	public string ReelID = "";

	public string TypeCode = "";

	public int Version
	{
		get
		{
			int result = 0;
			if (MatNo.Length > 0)
			{
				int.TryParse(MatNo.Substring(0, 1), out result);
			}
			return result;
		}
	}

	public bool IsBarcode2D
	{
		get
		{
			if (TypeCode.Trim().Length == 2)
			{
				int result = 0;
				return int.TryParse(TypeCode, out result);
			}
			return false;
		}
	}

	public void Init(string BarCode)
	{
		string[] array = BarCode.Split(new string[1] { "{" }, StringSplitOptions.None);
		if (array.Length != 0)
		{
			MatNo = array[0];
		}
		if (array.Length > 4)
		{
			TapingNo = array[4];
		}
		if (array.Length > 5)
		{
			TypeCode = array[5];
		}
		if (array.Length > 8)
		{
			ReelID = array[8];
		}
	}
}
