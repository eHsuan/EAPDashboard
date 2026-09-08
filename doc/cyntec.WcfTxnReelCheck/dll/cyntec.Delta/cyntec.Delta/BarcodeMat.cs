using System;

namespace cyntec.Delta;

public class BarcodeMat
{
	public string PN;

	public string Qty;

	public string Unit;

	public string Vendor;

	public string BatchNo;

	public string TypeCode;

	public string PO;

	public string InvNo;

	public string SerialNo;

	public string Plant;

	public string DC;

	public void Init(string BarCode)
	{
		string[] array = BarCode.Split(new string[1] { "{" }, StringSplitOptions.None);
		if (array.Length != 0)
		{
			PN = array[0];
		}
		if (array.Length > 1)
		{
			Qty = array[1];
		}
		if (array.Length > 2)
		{
			Unit = array[2];
		}
		if (array.Length > 3)
		{
			Vendor = array[3];
		}
		if (array.Length > 4)
		{
			BatchNo = array[4];
		}
		if (array.Length > 5)
		{
			TypeCode = array[5];
		}
		if (array.Length > 6)
		{
			PO = array[6];
		}
		if (array.Length > 7)
		{
			InvNo = array[7];
		}
		if (array.Length > 8)
		{
			SerialNo = array[8];
		}
		if (array.Length > 9)
		{
			Plant = array[9];
		}
		if (array.Length > 10)
		{
			DC = array[10];
		}
	}
}
