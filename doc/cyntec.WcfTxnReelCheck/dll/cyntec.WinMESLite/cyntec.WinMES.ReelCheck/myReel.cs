using System;
using System.Collections.Generic;
using System.Text;
using cyntec.MES;

namespace cyntec.WinMES.ReelCheck;

public class myReel
{
	public string Rawdata = "";

	private WO_ReelID m_Reel = null;

	public DateTime ReelUpdateTime = DateTime.Now;

	public double ReelUpdateDelay = 3.0;

	public DateTime TapingUILogTime;

	public DateTime ReelIDScanTime;

	public Dictionary<string, string> m_BarocdeScanList = new Dictionary<string, string>();

	public bool IsBlock = false;

	public bool IsReelEnd = false;

	public bool IsfinishUpdate = false;

	public bool IsFirstOKUpdate = false;

	public bool IsNewReel = true;

	public bool IsWoFinish = false;

	public bool IsAnyUpdate = false;

	public DateTime StartTime = DateTime.Now;

	public DateTime EndTime = DateTime.Now;

	public string TapingUILogTxnID = "";

	public string Matno = "";

	public string TPNo = "";

	public myReel PreReel = null;

	public myReel NextReel = null;

	public WO_ReelID Reel
	{
		get
		{
			return m_Reel;
		}
		set
		{
			m_Reel = value;
		}
	}

	public bool IsTapingUILogTimeout
	{
		get
		{
			if (Reel != null)
			{
				return (DateTime.Now - TapingUILogTime).TotalMinutes > ReelUpdateDelay;
			}
			return false;
		}
	}

	public bool IsFinish => ReelIDCount > 0 && MatNoCount > 0 && TPNoCount > 0;

	public int MatNoCount
	{
		get
		{
			if (Reel != null)
			{
				return Reel.MatNoCount;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
		set
		{
			if (Reel != null)
			{
				Reel.MatNoCount = value;
				return;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
	}

	public int TPNoCount
	{
		get
		{
			if (Reel != null)
			{
				return Reel.TPNoCount;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
		set
		{
			if (Reel != null)
			{
				Reel.TPNoCount = value;
				return;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
	}

	public int ReelIDCount
	{
		get
		{
			if (Reel != null)
			{
				return Reel.ReelIDCount;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
		set
		{
			if (Reel != null)
			{
				Reel.ReelIDCount = value;
				return;
			}
			throw new Exception("WO_ReelID物件為Null");
		}
	}

	public void CountBarcode(string strBarcode)
	{
		if (strBarcode == Rawdata)
		{
			ReelIDCount++;
			ReelIDScanTime = DateTime.Now;
			IsAnyUpdate = true;
		}
		else if (strBarcode == Matno)
		{
			MatNoCount++;
			IsAnyUpdate = true;
		}
		else if (strBarcode == TPNo)
		{
			TPNoCount++;
			IsAnyUpdate = true;
		}
	}

	public void init(myReel oPreReelID)
	{
		if (oPreReelID != null)
		{
			oPreReelID.NextReel = this;
		}
		PreReel = oPreReelID;
	}

	public string ToRemark()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Reel掃描註記(V1)");
		if (Reel != null)
		{
			stringBuilder.Append(", ReelID,").Append(Reel.ReelID);
		}
		stringBuilder.Append(", 花費(分),").Append(Math.Round((DateTime.Now - StartTime).TotalMinutes, 3));
		stringBuilder.Append(", 二維條碼(ReelID),").Append(Rawdata).Append(", ReelID次數,")
			.Append(ReelIDCount);
		stringBuilder.Append(", 料號,").Append(Matno).Append(", 料號次數,")
			.Append(MatNoCount);
		stringBuilder.Append(", TPNo,").Append(TPNo).Append(", TPNo次數,")
			.Append(TPNoCount);
		if (ReelIDCount > 0 && MatNoCount > 0 && TPNoCount > 0)
		{
			stringBuilder.Append(", OK");
		}
		else
		{
			stringBuilder.Append(", WAITING");
		}
		return stringBuilder.ToString();
	}

	public string ToRemark(string remark)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Reel掃描註記(V1)");
		if (Reel != null)
		{
			stringBuilder.Append(", ReelID,").Append(Reel.ReelID);
		}
		stringBuilder.Append(", 花費(分),").Append(Math.Round((DateTime.Now - StartTime).TotalMinutes, 3));
		stringBuilder.Append(", 二維條碼(ReelID),").Append(Rawdata).Append(", ReelID次數,")
			.Append(ReelIDCount);
		stringBuilder.Append(", 料號,").Append(Matno).Append(", 料號次數,")
			.Append(MatNoCount);
		stringBuilder.Append(", TPNo,").Append(TPNo).Append(", TPNo次數,")
			.Append(TPNoCount);
		if (ReelIDCount > 0 && MatNoCount > 0 && TPNoCount > 0)
		{
			stringBuilder.Append(", OK");
		}
		else
		{
			stringBuilder.Append(", WAITING");
		}
		stringBuilder.Append(", " + remark);
		return stringBuilder.ToString();
	}
}
