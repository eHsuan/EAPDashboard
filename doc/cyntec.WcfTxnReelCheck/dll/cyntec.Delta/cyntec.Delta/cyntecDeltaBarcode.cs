namespace cyntec.Delta;

public class cyntecDeltaBarcode
{
	public static string ToWoNo(string barcode, ref bool is_2D_Barcode)
	{
		string[] array = barcode.Split('{');
		is_2D_Barcode = false;
		if (array.Length != 9)
		{
			return barcode;
		}
		string result = array[4].ToString().Trim();
		string text = array[5].ToString().Trim();
		if (text == "96")
		{
			is_2D_Barcode = true;
			return result;
		}
		return barcode;
	}

	public static string ToStdPartsID(string barcode, ref bool is_2D_Barcode)
	{
		string[] array = barcode.Split('{');
		is_2D_Barcode = false;
		if (array.Length != 9)
		{
			return barcode;
		}
		return array[4].ToString().Trim();
	}

	public static void ToMat(string barcode)
	{
	}
}
