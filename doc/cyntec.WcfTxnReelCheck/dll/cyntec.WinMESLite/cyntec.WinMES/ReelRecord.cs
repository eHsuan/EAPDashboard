using System;
using System.Collections.Generic;
using cyntec.Base;

namespace cyntec.WinMES;

public class ReelRecord
{
	public string ReelID = "";

	public DateTime LogTime;

	public string LogType = "";

	public string LabelContent = "";

	public decimal IntervalTime = default(decimal);

	public int ReelIDCount = 0;

	public int MatNoCount = 0;

	public int TPNoCount = 0;

	public bool IsComplete = false;

	public string LogTxnID = "";

	public string Remark = "";

	public bool IsError = false;

	public bool IsWO_ReelCheck_Save_OK = false;

	public bool IsWO_ReelIDLog_Add_OK = false;

	public bool IsWO_ReelID_Update_OK = false;

	public Dictionary<string, string> EqpOpRecordSaveList = new Dictionary<string, string>();

	public List<string> API_WO_ReelIDLog_Add_ExceptionList = new List<string>();

	public DateTime StartTime;

	public DateTime EndTime;

	public string MatNo = "";

	public string TPNo = "";

	public List<string> ErrorList = new List<string>();

	public DateTime APITime;

	public bool IsNewVersion = false;

	public bool IsTxnComplete => IsWO_ReelCheck_Save_OK && IsWO_ReelIDLog_Add_OK && IsWO_ReelID_Update_OK;

	public void Count(string msg, DateTime now)
	{
		if (msg == ReelID)
		{
			ReelIDCount++;
			EndTime = now;
		}
		else if (msg == MatNo)
		{
			MatNoCount++;
			EndTime = now;
		}
		else if (msg == TPNo)
		{
			TPNoCount++;
			EndTime = now;
		}
	}

	public void init(string line)
	{
		string[] array = line.Split(new string[1] { "," }, StringSplitOptions.None);
		if (array[0].ToString().StartsWith("#"))
		{
			return;
		}
		try
		{
			ReelID = array[0].ToString().Trim().Replace("\0", "");
		}
		catch
		{
			IsError = true;
		}
		try
		{
			LogTime = cyntecDateTime.ToDateTime(array[1]);
		}
		catch
		{
			IsError = true;
		}
		try
		{
			LogType = array[2].ToString();
		}
		catch
		{
			IsError = true;
		}
		try
		{
			LabelContent = array[3].ToString();
		}
		catch
		{
			IsError = true;
		}
		try
		{
			IntervalTime = decimal.Parse(array[4].ToString());
		}
		catch
		{
			IsError = true;
		}
		try
		{
			ReelIDCount = int.Parse(array[5].ToString());
		}
		catch
		{
			IsError = true;
		}
		try
		{
			MatNoCount = int.Parse(array[6].ToString());
		}
		catch
		{
			IsError = true;
		}
		try
		{
			TPNoCount = int.Parse(array[7].ToString());
		}
		catch
		{
			IsError = true;
		}
		try
		{
			IsComplete = cyntecByteConvert.ToBool((object)array[8].ToString());
		}
		catch
		{
			IsError = true;
		}
		try
		{
			LogTxnID = array[9].ToString();
		}
		catch
		{
			IsError = true;
		}
		try
		{
			Remark = array[10].ToString();
		}
		catch
		{
			IsError = true;
		}
	}

	public static Dictionary<string, ReelRecord> ReadReelRecord(string[] lines)
	{
		List<ReelRecord> list = new List<ReelRecord>();
		Dictionary<string, ReelRecord> dictionary = new Dictionary<string, ReelRecord>();
		bool isNewVersion = false;
		if (lines.Length != 0 && lines[0].Contains("%新版本%"))
		{
			isNewVersion = true;
		}
		foreach (string line in lines)
		{
			ReelRecord reelRecord = new ReelRecord();
			reelRecord.init(line);
			if (!reelRecord.IsError)
			{
				reelRecord.IsNewVersion = isNewVersion;
				list.Add(reelRecord);
			}
		}
		foreach (ReelRecord item in list)
		{
			if (string.IsNullOrWhiteSpace(item.ReelID))
			{
				continue;
			}
			if (!dictionary.ContainsKey(item.ReelID))
			{
				item.StartTime = item.LogTime;
				item.EndTime = item.LogTime;
				dictionary.Add(item.ReelID, item);
				continue;
			}
			ReelRecord reelRecord2 = dictionary[item.ReelID];
			if (item.LogType == "REELRECORD_REELID")
			{
				reelRecord2.ReelIDCount++;
				reelRecord2.EndTime = item.LogTime;
				reelRecord2.IsWO_ReelCheck_Save_OK = false;
				reelRecord2.IsWO_ReelID_Update_OK = false;
			}
			else if (item.LogType == "REELRECORD_MATNO")
			{
				reelRecord2.MatNoCount++;
				reelRecord2.EndTime = item.LogTime;
				reelRecord2.IsWO_ReelCheck_Save_OK = false;
				reelRecord2.IsWO_ReelID_Update_OK = false;
			}
			else if (item.LogType == "REELRECORD_TPNO")
			{
				reelRecord2.TPNoCount++;
				reelRecord2.EndTime = item.LogTime;
				reelRecord2.IsWO_ReelCheck_Save_OK = false;
				reelRecord2.IsWO_ReelID_Update_OK = false;
			}
			else if (item.LogType == "API_WO_ReelCheck_Save")
			{
				reelRecord2.IsWO_ReelCheck_Save_OK = true;
			}
			else if (item.LogType == "API_WO_ReelID_Update")
			{
				reelRecord2.IsWO_ReelID_Update_OK = true;
			}
			else if (item.LogType == "API_WO_ReelIDLog_Add")
			{
				reelRecord2.IsWO_ReelIDLog_Add_OK = true;
			}
			else if (item.LogType == "API_WO_ReelIDLog_Add_Exception_Error")
			{
				reelRecord2.API_WO_ReelIDLog_Add_ExceptionList.Add(item.LabelContent);
			}
			else if (item.LogType == "API_WO_ReelIDLog_Add_Exception")
			{
				reelRecord2.API_WO_ReelIDLog_Add_ExceptionList.Clear();
			}
			else if (item.LogType == "API_EQP_OP_Record_Save_Error")
			{
				if (!reelRecord2.EqpOpRecordSaveList.ContainsKey(item.LabelContent))
				{
					reelRecord2.EqpOpRecordSaveList.Add(item.LabelContent, item.Remark);
				}
			}
			else if (item.LogType == "API_EQP_OP_Record_Save")
			{
				if (reelRecord2.EqpOpRecordSaveList.ContainsKey(item.LabelContent))
				{
					reelRecord2.EqpOpRecordSaveList.Remove(item.LabelContent);
				}
			}
			else if ((item.LogType == "異常" || item.LogType.ToUpper() == "ERROR" || item.LogType.ToUpper() == "EXCEPTION") && item.Remark.Contains("NG-0110"))
			{
				reelRecord2.EndTime = item.LogTime;
				reelRecord2.IsWO_ReelCheck_Save_OK = true;
				reelRecord2.IsWO_ReelID_Update_OK = true;
				if (!reelRecord2.ErrorList.Contains("NG-0110"))
				{
					reelRecord2.ErrorList.Add("NG-0110");
				}
			}
		}
		return dictionary;
	}
}
