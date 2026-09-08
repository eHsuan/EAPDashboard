using System;
using System.Collections.Generic;
using System.Data;
using cyntec.Tools;

namespace cyntec.WinMES.ReelCheck;

public class BarcodeCheckItem
{
	public bool Is2D = false;

	public string MES_ReelID = null;

	public string MES_TapingNo = null;

	public string MES_MatNo = null;

	public List<string> MES_ReelIDList = null;

	public string BC_ReelID = null;

	public string BC_TapingNo = null;

	public string BC_MatNo = null;

	public string DisplayBC = "";

	public string ExceptionMsg = "";

	public string RawData = "";

	public string ItemName = "";

	public string Description = "";

	public DateTime PreTime;

	public DateTime PostTime;

	public DateTime UpdateTime;

	public string CheckResult = "";

	public string Action = "";

	public int Count = 0;

	public static DataTable ToDataToable(IEnumerable<BarcodeCheckItem> itemList)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		cyntecDataTableGen val = new cyntecDataTableGen();
		val.TableName = "TEST1";
		foreach (BarcodeCheckItem item in itemList)
		{
			val.AddRow();
			val.AddItem("條碼", (object)item.DisplayBC, typeof(string));
			if (item.UpdateTime != DateTime.MinValue)
			{
				val.AddItem("上機時間", (object)item.UpdateTime.ToString("HH:mm:ss"), typeof(string));
			}
			else
			{
				val.AddItem("上機時間", (object)"", typeof(string));
			}
			val.AddItem("$OBJECT", (object)item, typeof(object));
		}
		return val.ToDataTable();
	}

	public bool IsOK()
	{
		if (MES_MatNo != null && BC_MatNo != null && MES_MatNo != BC_MatNo)
		{
			ExceptionMsg = "料號異常, MES," + MES_MatNo + ", BC," + BC_MatNo;
			return false;
		}
		if (MES_TapingNo != null && MES_TapingNo != null && MES_TapingNo != BC_TapingNo)
		{
			ExceptionMsg = "T/P No.異常, MES, " + MES_TapingNo + ", BC," + BC_TapingNo;
			return false;
		}
		if (MES_ReelIDList != null && BC_ReelID != null && !MES_ReelIDList.Contains(BC_ReelID))
		{
			ExceptionMsg = "ReelID異常, " + BC_TapingNo + "不在列表內";
			return false;
		}
		if (!string.IsNullOrWhiteSpace(ExceptionMsg))
		{
			return false;
		}
		return true;
	}
}
