using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using cyntec.Base;
using cyntec.Delta;
using cyntec.MES;
using cyntec.Tools;
using cyntec.WCF;

namespace cyntec.WinMES.ReelCheck;

public class ReelBarcodeCheck
{
	private cyntecCulture m_Culture = null;

	private const string MSG001 = "MSG001";

	private const string MSG002 = "MSG002";

	private const string MSG003 = "MSG003";

	private const string MSG004 = "MSG004";

	private const string MSG005 = "MSG005";

	private const string MSG006 = "MSG006";

	private const string MSG007 = "MSG007";

	private const string MSG008 = "MSG008";

	private const string MSG009 = "MSG009";

	private const string MSG010 = "MSG010";

	private const string MSG011 = "MSG011";

	private const string MSG012 = "MSG012";

	private const string MSG013 = "MSG013";

	private const string MSG014 = "MSG014";

	private const string MSG015 = "MSG015";

	private const string MSG016 = "MSG016";

	private const string MSG017 = "MSG017";

	private const string MSG018 = "MSG018";

	private const string MSG019 = "MSG019";

	private const string MSG020 = "MSG020";

	private const string MSG021 = "MSG021";

	private const string MSG022 = "MSG022";

	private const string MSG023 = "MSG023";

	private const string MSG024 = "MSG024";

	private const string MSG025 = "MSG025";

	private const string MSG026 = "MSG026";

	private const string MSG027 = "MSG027";

	private const string MSG028 = "MSG028";

	private const string MSG029 = "MSG029";

	private const string MSG030 = "MSG030";

	private const string MSG031 = "MSG031";

	private const string MSG032 = "MSG032";

	private const string MSG033 = "MSG033";

	private const string MSG034 = "MSG034";

	private const string MSG035 = "MSG035";

	private const string MSG036 = "MSG036";

	private const string MSG037 = "MSG037";

	private const string MSG038 = "MSG038";

	private const string MSG039 = "MSG039";

	private const string MSG040 = "MSG040";

	private const string MSG041 = "MSG041";

	private const string MSG042 = "MSG042";

	private const string MSG043 = "MSG043";

	private const string MSG044 = "MSG044";

	private const string MSG045 = "MSG045";

	private const string MSG046 = "MSG046";

	private const string MSG047 = "MSG047";

	private const string MSG048 = "MSG048";

	private const string MSG049 = "MSG049";

	private const string MSG050 = "MSG050";

	private const string MSG051 = "MSG051";

	private const string MSG052 = "MSG052";

	private const string MSG053 = "MSG053";

	private const string MSG054 = "MSG054";

	private const string MSG055 = "MSG055";

	private const string MSG056 = "MSG056";

	private const string MSG057 = "MSG057";

	private const string MSG058 = "MSG058";

	private const string MSG059 = "MSG059";

	private const string MSG060 = "MSG060";

	private const string strDESAPHA = "-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private const string strSOURCEAPHA = "-QXERPWS89AKCMNT56OF2HIYBLDV4U37GZ01J";

	public cyntecLog m_Log = null;

	public cyntecLogFunction m_LogFunction = new cyntecLogFunction();

	public wcfServiceConnection m_txnConn = null;

	public string InputBarcode = "";

	public bool m_Start = false;

	public DateTime m_WoStartDate;

	public WoInfo_GetResult m_WoInfo = null;

	public GetParameterResult m_Parameter = null;

	public WO_ReelStatus_GetResult m_WoReelStatusInfo = null;

	public WO_ReelID_GetResult m_woReelIdInfo = null;

	public EQP_OP_CheckList_GetResult m_EqpOpCheckInfo = null;

	public double m_ReelUpdateDelay = 3.0;

	public bool m_Is_Reel_Check = false;

	public bool m_Is_Start = false;

	public bool m_Is_BarcodeCheckOK = true;

	public string m_ReelCheckMsg_KeepLast = "";

	private string m_ReelCheckMsg_Private = "";

	public string m_ReelCheckErrorType = "";

	public string m_ExceptionWoNo = "";

	public Dictionary<string, myReel> m_InputBarcodeList = new Dictionary<string, myReel>();

	public Dictionary<string, string> m_InputBarcodeExceptionList = new Dictionary<string, string>();

	public myReel m_LastInputBarcode = null;

	public int m_iReelCount = 0;

	public string m_UserID = "";

	public string m_machNo = "";

	public string m_WorkcenterNo = "";

	public string m_WoNo = "";

	public string m_MatNo = "";

	public string m_MainMatNo = "";

	public string m_SubMatNo = "";

	public string m_TapingNo = "";

	public bool m_IsMainMat = true;

	public Mach_Master_GetResult m_MachInfo = null;

	public string m_BarcodeType = "0";

	public string LogPath = "";

	private cyntecThread m_ReelUpdate = null;

	public int iMessageCount = 0;

	public int m_MatNoCountMin = 1;

	public int m_TPNoCountMin = 1;

	public bool m_IsCheckConfirmDate = true;

	public bool isWJ = false;

	private DataTable m_PingErrorDT;

	private cyntecFile m_FileLogReel = null;

	private cyntecFile m_FileLogSummary = null;

	private bool m_Ism_ReelCheckInputProcess = false;

	private ulong l_onMessageArrive = 0uL;

	private ulong l_onMessageBuffer = 0uL;

	public Queue<BarcodeRecord> m_BarcodeQueue = new Queue<BarcodeRecord>();

	public bool IsBuzy = false;

	public cyntecDateTime myTime;

	private List<ReelRecord> m_ReelRecordList = new List<ReelRecord>();

	public string strReelRecordRefreshErrorMsg = "";

	public DateTime TimeReelUpdate_onProcess;

	public DateTime StartTime = DateTime.Now;

	public ulong ReelUpdate_onProcessCount = 0uL;

	public string m_ReelCheckMsg
	{
		get
		{
			return m_ReelCheckMsg_Private;
		}
		set
		{
			m_ReelCheckMsg_Private = value;
			if (string.IsNullOrWhiteSpace(m_ReelCheckMsg_KeepLast))
			{
				m_ReelCheckMsg_KeepLast = value;
			}
		}
	}

	public bool IsReelUpdate_onProcessOK => ReelUpdate_onProcessCount != 0;

	public string FileNameReel => m_WoNo + "_LOGREEL.txt";

	public string FileNameSummary => m_WoNo + "_LOGSUMMARY.txt";

	public event HOnRefresh OnRefresh;

	public ReelBarcodeCheck()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		if (m_Culture == null)
		{
			m_Culture = new cyntecCulture();
			m_Culture.AddRow("MSG001", "en-US", "Reel Check (NG-0101), the current process for main material, main MatNo: {0}, but the scanned in is the auxiliary MatNo barcode, the auxiliary MatNo: {1}");
			m_Culture.AddRow("MSG001", "zh-TW", "上機異常(NG-0101), 當前作業是主料, 主料號:{0}, 但掃進來的是副料條碼, 副料號:{1}");
			m_Culture.AddRow("MSG002", "en-US", "Reel Check (NG-0102), the current process for auxiliary material, auxiliary MatNo: {0}, but the barcode scanned is the main MatNo, main MatNo: {1}");
			m_Culture.AddRow("MSG002", "zh-TW", "上機異常(NG-0102), 當前作業是副料, 副料號: {0}, 但掃進的是主料條碼, 主料號: {1}");
			m_Culture.AddRow("MSG003", "en-US", "Reel Check (NG-0103), the current process for main material, T/P NO.:{0}, but the barcode scanned in is the auxiliary material, T/P No.:{1}");
			m_Culture.AddRow("MSG003", "zh-TW", "上機異常(NG-0103), 當前作業是主料, T/P NO.:{0}, 但掃進來的是副料條碼, T/P No.:{1}");
			m_Culture.AddRow("MSG004", "en-US", "Reel Check (NG-0104), the current process for auxiliary material, T/P No.:{0}, but the barcode scanned in is the main material, T/p No:{1}");
			m_Culture.AddRow("MSG004", "zh-TW", "上機異常(NG-0104), 當前作業是副料, T/P No.:{0}, 但掃進來的是主料條碼, T/p No:{1}");
			m_Culture.AddRow("MSG005", "en-US", "Reel Check (NG-0105), current WO: {0}, but the scanned T/P No. barcode: {1}, belongs to another Wo: {2};");
			m_Culture.AddRow("MSG005", "zh-TW", "上機異常(NG-0105), 當前作業工單:{0}, 但掃進來的T/P No.條碼:{1}, 屬於另一個工單:{2};");
			m_Culture.AddRow("MSG006", "en-US", "Reel Check (NG-0106), current WO: {0}, but the scanned material number barcode: {1}, belongs to another WO (unknown)");
			m_Culture.AddRow("MSG006", "zh-TW", "上機異常(NG-0106), 當前作業工單:{0}, 但掃進來的料號條碼:{1}, 屬於另一個工單(未知)");
			m_Culture.AddRow("MSG007", "en-US", "Reel Check (NG-0107), current WO: {0}, but the scanned barcode: {1} cannot be recognized.");
			m_Culture.AddRow("MSG007", "zh-TW", "上機異常(NG-0107), 當前作業工單:{0}, 但掃進來的條碼:{1}無法識別。");
			m_Culture.AddRow("MSG008", "en-US", "WO Status Check (NG-0202), the WO has been comfirmed (ConfrimDate is inconsistent with the time when the WO was refreshed), entry time: {0}, modified time: {1}");
			m_Culture.AddRow("MSG008", "zh-TW", "工單狀態異動(NG-0202), 工單已被過帳(ConfrimDate與刷工單時不一致), 進站時間:{0}, 被更動時間:{1}");
			m_Culture.AddRow("MSG009", "en-US", "WO Status Check (NG-0203), work order exit status changes: Work order has exited the site.");
			m_Culture.AddRow("MSG009", "zh-TW", "工單狀態變動(NG-0203), 工單出站狀態改變: 工單已出站");
			m_Culture.AddRow("MSG010", "en-US", "Reel Check (NG-0110), the Reel ID ({0}) of the 2D barcode has been loaded on EQP.");
			m_Culture.AddRow("MSG010", "zh-TW", "上機異常(NG-0110), 2D條碼之Reel ID ({0})已經重複上機台");
			m_Culture.AddRow("MSG011", "en-US", "WO Status Check (NG-0201), the WO may have been deleted 'Confirm'.");
			m_Culture.AddRow("MSG011", "zh-TW", "工單狀態異動(NG-0201), 工單可能已被刪Confirm。");
			m_Culture.AddRow("MSG012", "en-US", "WO Status Check (NG-0202), the WO has been posted (ConfrimDate is inconsistent with the time when the WO was refreshed), entry time: {0}, modified time: {1}");
			m_Culture.AddRow("MSG012", "zh-TW", "工單狀態異動(NG-0202), 工單已被過帳(ConfrimDate與刷工單時不一致), 進站時間:{0}, 被更動時間: {1}。");
			m_Culture.AddRow("MSG013", "en-US", "No Used.");
			m_Culture.AddRow("MSG013", "zh-TW", "No Used.");
			m_Culture.AddRow("MSG014", "en-US", "Barcode Check (NG-0303), unable to obtain barcode from WO_REELID table: {0} information, maybe (1) DB changes, (2) abnormality when printing labels");
			m_Culture.AddRow("MSG014", "zh-TW", "條碼狀態異動(NG-0303), 無法從WO_REELID表格取得條碼: {0} 資訊, 可能(1)DB有異動, (2)列印標籤時異常。");
			m_Culture.AddRow("MSG015", "en-US", "Wo Check (NG-0401), the current work order ({0}), but the Reel({2}) of other work orders({1}) was scanned.");
			m_Culture.AddRow("MSG015", "zh-TW", "與工單不匹配(NG-0401), 當前工單({0}), 但掃到其他工單({1})的Reel({2})。");
			m_Culture.AddRow("MSG016", "en-US", "Reel Check (NG-0108), Reel({0}) expected status: 11 {1}, but the current status has been manually changed to: {2}.");
			m_Culture.AddRow("MSG016", "zh-TW", "上機異常(NG-0108), Reel({0})預期狀態:11 {1}, 但當前狀態已被手動改為:{2}。");
			m_Culture.AddRow("MSG017", "en-US", "Reel Check (NG-0109), 2D barcode ({0}), the expected operating machine is: {1}, but it has been loaded to another machine: {2}.");
			m_Culture.AddRow("MSG017", "zh-TW", "上機異常(NG-0109), 2D條碼({0}), 預期的作業機台是:{1}, 但已經上到別的機台:{2}。");
			m_Culture.AddRow("MSG018", "en-US", "Boarcode Status (NG-0304), 2D barcode ({0}), the expected material number is: {1}, but the material number defined in the WO_REELID table is: {2}.");
			m_Culture.AddRow("MSG018", "zh-TW", "條碼狀態異常(NG-0304), 2D條碼({0}), 預期的料號是:{1},但在WO_REELID表格定義的料號是:{2}。");
			m_Culture.AddRow("MSG019", "en-US", "Barcode Status (NG-0305), 2D barcode ({0}), the expected T/P No. is: {1}, but the T/P No. defined in the WO_REELID table is: {2}.");
			m_Culture.AddRow("MSG019", "zh-TW", "條碼狀態異常(NG-0305), 2D條碼({0}), 預期的T/P No.是:{1}, 但在WO_REELID表格定義的T/P No.是:{2}。");
			m_Culture.AddRow("MSG020", "en-US", "Reel Check (NG-0110), the Reel ID ({0}) of the 2D barcode has been loaded onto the EQP.");
			m_Culture.AddRow("MSG020", "zh-TW", "上機異常(NG-0110), 2D條碼之Reel ID({0})已經重複上機台。");
			m_Culture.AddRow("MSG021", "en-US", "WO status (NG-0202), the work order may have been deleted Confirm.");
			m_Culture.AddRow("MSG021", "zh-TW", "工單狀態異動(NG-0202), 工單可能已被刪Confirm。");
			m_Culture.AddRow("MSG022", "en-US", "WO status changed (NG-0202), the WO has been posted (ConfrimDate is inconsistent with the time when the WO was refreshed), entry time: {0}, modified time: {1};.");
			m_Culture.AddRow("MSG022", "zh-TW", "工單狀態異動(NG-0202), 工單已被過帳(ConfrimDate與刷工單時不一致), 進站時間:{0}, 被更動時間:{1}。");
			m_Culture.AddRow("MSG023", "en-US", "Abnormal barcode content");
			m_Culture.AddRow("MSG023", "zh-TW", "條碼內容異常");
			m_Culture.AddRow("MSG024", "en-US", "The barcode content is abnormal (NG-9801), and the REEL scanner reports an exception (the barcode content contains ERROR). The content is, ");
			m_Culture.AddRow("MSG024", "zh-TW", "條碼內容異常(NG-9801), REEL掃描槍報異常(條碼內容包含ERROR), 內容是, ");
			m_Culture.AddRow("MSG025", "en-US", "Loading exception");
			m_Culture.AddRow("MSG025", "zh-TW", "上機異常");
			m_Culture.AddRow("MSG026", "en-US", "Barcode status changes (NG-0301), unable to obtain barcode: {0} information from the WO_REELID table, there may be changes in the DB");
			m_Culture.AddRow("MSG026", "zh-TW", "條碼狀態異動(NG-0301), 無法從WO_REELID表格取得條碼:{0}資訊, 可能DB有異動");
			m_Culture.AddRow("MSG027", "en-US", "Barcode status changes (NG-0302), the barcode T/P status is expected to be: 11({0}), but the barcode status has changed to:");
			m_Culture.AddRow("MSG027", "zh-TW", "條碼狀態異動(NG-0302), 條碼T/P狀態預期是:11({0}), 但條碼狀態已經變成:");
			m_Culture.AddRow("MSG028", "en-US", "Does not match the WO");
			m_Culture.AddRow("MSG028", "zh-TW", "與工單不匹配");
			m_Culture.AddRow("MSG029", "en-US", "Does not match the work order (NG-0401B), The Reel({1}) of the problem WO ({0}) was found in the operation of the WO ({2})");
			m_Culture.AddRow("MSG029", "zh-TW", "與工單不匹配(NG-0401B), 問題工單({0}的Reel({1})在工單({2})的作業當中發現");
			m_Culture.AddRow("MSG030", "en-US", "NoUsed");
			m_Culture.AddRow("MSG030", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG031", "en-US", "Abnormal barcode status");
			m_Culture.AddRow("MSG031", "zh-TW", "條碼狀態異常");
			m_Culture.AddRow("MSG032", "en-US", "REEL tag scan count check");
			m_Culture.AddRow("MSG032", "zh-TW", "REEL標籤掃描次數檢查");
			m_Culture.AddRow("MSG033", "en-US", "REEL tag scanning count check (switch ON starts checking)");
			m_Culture.AddRow("MSG033", "zh-TW", "REEL標籤掃描次數檢查(開關ON開始檢查)");
			m_Culture.AddRow("MSG034", "en-US", "NoUsed");
			m_Culture.AddRow("MSG034", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG035", "en-US", "Loading Exception (NG-0113), the operation time of Reel[{0}][{1}] is less than the minimum operation time of Reel (minutes)[{2}]");
			m_Culture.AddRow("MSG035", "zh-TW", "上機異常(NG-0113), Reel[0]之作業時間[{1}]小於Reel最小作業時間(分)[{2}]");
			m_Culture.AddRow("MSG036", "en-US", "Reel operating time (minimum operating time)");
			m_Culture.AddRow("MSG036", "zh-TW", "Reel作業時間 (最小作業時間)");
			m_Culture.AddRow("MSG037", "en-US", "NoUsed");
			m_Culture.AddRow("MSG037", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG038", "en-US", "NoUsed");
			m_Culture.AddRow("MSG038", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG039", "en-US", "NoUsed");
			m_Culture.AddRow("MSG039", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG040", "en-US", "NoUsed");
			m_Culture.AddRow("MSG040", "zh-TW", "NoUsed");
			m_Culture.AddRow("MSG041", "en-US", "Loading Exception (NG-0111), Reel ID of 2D barcode [{0}], the number of times this reel scans large labels (material number barcodes) [{1}] is less than the set number [{2}]");
			m_Culture.AddRow("MSG041", "zh-TW", "上機異常(NG-0111), 2D條碼之Reel ID[{0}],這個Reel掃到大標籤(料號條碼)次數[{1}]小於設定次數 [{2}]");
			m_Culture.AddRow("MSG042", "en-US", "Loading Exception (NG-0112), Reel ID of 2D barcode [{0}], the number of large tags (T/P No. barcode) scanned by this reel [{1}] is less than the set number [{2}]");
			m_Culture.AddRow("MSG042", "zh-TW", "上機異常(NG-0112), 2D條碼之Reel ID[{0}],這個Reel掃到大標籤(T/P No.條碼)次數[{1}]小於設定次數 [{2}]");
			m_Culture.AddRow("MSG043", "en-US", "The UserID is blank, the event content is [{0}], and the UserID blank is changed to sys to avoid errors.");
			m_Culture.AddRow("MSG043", "zh-TW", "UserID為空白,事件內容為[{0}],UserID空白改為sys,以避免錯誤一直出現");
			m_Culture.AddRow("MSG044", "en-US", "The reel [{0}] has not been generated when the work order is posted, but it is obtained when querying again.");
			m_Culture.AddRow("MSG044", "zh-TW", "刷工單的時候 reel [{0}]還沒生成,再查詢時得到。");
			m_Culture.AddRow("MSG045", "en-US", "WO Status");
			m_Culture.AddRow("MSG045", "zh-TW", "工單狀態異常");
			m_Culture.AddRow("MSG046", "en-US", "Barcode status change");
			m_Culture.AddRow("MSG046", "zh-TW", "條碼狀態異動");
		}
	}

	private string f_ReelIDDecode(string Barcode)
	{
		string text = "";
		string[] array = Barcode.Split(new string[1] { "{" }, StringSplitOptions.None);
		if (array.Length != 0)
		{
			text = array[array.Length - 1];
		}
		List<char> list = new List<char>();
		for (int i = 1; i < text.Length; i++)
		{
			int num = "-QXERPWS89AKCMNT56OF2HIYBLDV4U37GZ01J".IndexOf(text[i]);
			if (num >= 0)
			{
				list.Add("-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[num]);
			}
		}
		return new string(list.ToArray());
	}

	public string f_ReelIDEncryption(string reelID)
	{
		List<char> list = new List<char>();
		for (int i = 1; i < reelID.Length; i++)
		{
			int num = "-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(reelID[i]);
			if (num >= 0)
			{
				list.Add("-QXERPWS89AKCMNT56OF2HIYBLDV4U37GZ01J"[num]);
			}
		}
		return new string(list.ToArray());
	}

	private WO_ReelID f_ToMEsReel(string reelid)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		IEnumerable<WO_ReelID> source = m_woReelIdInfo._ItemList.Where((WO_ReelID o) => o.ReelID == reelid);
		if (source.Count() > 0)
		{
			return source.ElementAt(0);
		}
		WO_ReelID_Get val = new WO_ReelID_Get();
		val._ReelID = reelid;
		cyntecDateTime val2 = new cyntecDateTime();
		((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).clientExecute(m_txnConn);
		m_Log.LogDebug($"m_ReelCheckInput_f_ToMEsReel,process(WO_ReelID_Get),<{l_onMessageArrive}>,spend(s),{val2.ToTimeSpan().TotalSeconds}");
		if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).IsTxnError)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ToMEsReel,Exception(WO_ReelID_Get),<{l_onMessageArrive}>,{((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).TxnErrorMsg}");
		}
		else if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).Result._ItemList.Count > 0 && m_WoNo == ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).Result._ItemList[0].WONo)
		{
			string arg = m_Culture.ToMSg("MSG044", new object[1] { reelid });
			m_Log.LogDebug($"m_ReelCheckInput_f_ToMEsReel,process,<{l_onMessageArrive}>,{arg}");
			return ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val).Result._ItemList[0];
		}
		return null;
	}

	private void f_InitFileLogReel()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		string text = Path.Combine(LogPath, FileNameReel);
		if (m_FileLogReel != null && m_FileLogReel.FilePath.ToUpper() != text.ToUpper())
		{
			m_FileLogReel.Close();
			m_FileLogReel = null;
		}
		if (m_FileLogReel == null)
		{
			m_FileLogReel = new cyntecFile();
			m_FileLogReel.oEncoding = Encoding.UTF8;
			m_FileLogReel.FilePath = text;
		}
	}

	private void f_InitFileLogSummary()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		string text = Path.Combine(LogPath, FileNameSummary);
		if (m_FileLogSummary != null && m_FileLogSummary.FilePath.ToUpper() != text.ToUpper())
		{
			m_FileLogSummary.Close();
			m_FileLogSummary = null;
		}
		if (m_FileLogSummary == null)
		{
			m_FileLogSummary = new cyntecFile();
			m_FileLogSummary.oEncoding = Encoding.UTF8;
			m_FileLogSummary.FilePath = text;
		}
	}

	public void Start()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_Start(),start");
			f_InitFileLogReel();
			f_InitFileLogSummary();
			m_PingErrorDT = new DataTable();
			m_PingErrorDT.Columns.Add("時間", typeof(string));
			m_PingErrorDT.Columns.Add("次數", typeof(int));
			m_LogFunction.init(m_Log, "REELCHECK;");
			m_ReelUpdate = new cyntecThread();
			m_ReelUpdate.onProcessX += new ThreadProcessXEvent(m_ReelUpdate_onProcess);
			m_ReelUpdate.onError += new ThreadErrorEvent(m_ReelUpdate_onError);
			m_ReelUpdate.SleepTime = 10;
			m_ReelUpdate.Priority = ThreadPriority.Highest;
			m_ReelUpdate.Start();
			m_PingServrer();
			MESSystemParameter_Get val = new MESSystemParameter_Get();
			val._Condition.ParameterName = "EQP_REELCHECK_COUNT";
			val._Condition.Value = m_machNo;
			((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).clientExecute(m_txnConn);
			if (((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).IsTxnError)
			{
				throw new Exception("Fetch EQP_REELCHECK_COUNT Exception, " + ((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).TxnErrorMsg);
			}
			if (((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).Result._ItemList.Count > 0)
			{
				int result = 0;
				if (int.TryParse(((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).Result._ItemList[0].Item001, out result))
				{
					m_MatNoCountMin = result;
				}
				if (int.TryParse(((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).Result._ItemList[0].Item002, out result))
				{
					m_TPNoCountMin = result;
				}
				if (int.TryParse(((TransactionBase<MESSystemParameter_Get, MESSystemParameter_GetResult>)(object)val).Result._ItemList[0].Item003, out result))
				{
					m_ReelUpdateDelay = result;
				}
			}
			string[] lines = m_FileLogReel.ReadLines();
			m_ReelRecordList = ReelRecord.ReadReelRecord(lines).Values.ToList();
			foreach (ReelRecord reelRecord in m_ReelRecordList)
			{
				reelRecord.MatNo = m_MatNo;
				reelRecord.TPNo = m_TapingNo;
			}
			cyntecThread.Sleep(100);
			if (ReelUpdate_onProcessCount == 0)
			{
				throw new Exception("The thread ReelUpdate_onProcess is not active. Please restart");
			}
			m_Log.LogDebug("m_ReelCheckInput_Start(), thread check OK");
		}
		catch (Exception ex)
		{
			m_Log.LogDebug(string.Format("m_ReelCheckInput_Start(),exception,{0}", ex.Message + ", " + ex.StackTrace));
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_Start(),End");
		}
	}

	public void Close()
	{
		try
		{
			m_Log.LogDebug("ReelBarcodeCheck.Close(), start");
			if (IsBuzy)
			{
				m_Log.LogDebug("ReelBarcodeCheck.Close(), barcode is checking");
			}
			if (m_BarcodeQueue != null && m_BarcodeQueue.Count > 0)
			{
				m_Log.LogDebug("ReelBarcodeCheck.Close(), There are still unprocessed items in m_BarcodeQueue, the total number of items," + m_BarcodeQueue.Count);
			}
			m_BarcodeQueue.Clear();
			if (m_ReelUpdate != null)
			{
				m_Log.LogDebug("ReelBarcodeCheck.Close(),m_ReelUpdate.Stop()");
				m_ReelUpdate.Stop();
				cyntecThread.Sleep(100);
				m_Log.LogDebug("ReelBarcodeCheck.Close(),m_ReelUpdate.Abort()");
				m_ReelUpdate.Abort();
			}
			if (m_FileLogReel != null)
			{
				m_Log.LogDebug("ReelBarcodeCheck.Close(),m_FileLogReel.Close()");
				m_FileLogReel.Close();
			}
			if (m_FileLogSummary != null)
			{
				m_Log.LogDebug("ReelBarcodeCheck.Close(),m_FileLogSummary.Close()");
				m_FileLogSummary.Close();
			}
		}
		catch (Exception ex)
		{
			m_Log.LogError("ReelBarcodeCheck.Close() exception," + ex.Message);
		}
		finally
		{
			m_Log.LogDebug("ReelBarcodeCheck.Close(), End");
		}
	}

	public void m_ReelCheckInput_onMessageArrive(string msg, cyntecDateTime myTime)
	{
		m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,receive,<{l_onMessageBuffer}>,{msg},spend{myTime.ToTimeSpan().TotalSeconds}");
		m_ReelCheckInput_onMessageArrive(msg);
	}

	public void m_ReelCheckInput_onMessageArrive(string msg)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Expected O, but got Unknown
		l_onMessageBuffer++;
		string text = "";
		string text2 = "";
		if (m_LastInputBarcode != null)
		{
			text = m_LastInputBarcode.Reel.ReelID;
			text2 = m_LastInputBarcode.Rawdata;
		}
		cyntecDateTime val = new cyntecDateTime();
		string text3 = "";
		m_Is_BarcodeCheckOK = true;
		m_ReelCheckErrorType = "";
		m_ExceptionWoNo = "";
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,Start,<{l_onMessageBuffer}>,{msg}");
			if (!m_Start)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process(If it is not started, Reel will not be checked.),<{l_onMessageBuffer}>,{msg}");
				return;
			}
			text3 = (InputBarcode = msg.TrimEnd('\n', '\r').Replace(Environment.NewLine, ""));
			if (!m_Is_Start)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process(The EQP is not started, so this barcode input is ignored.),<{l_onMessageBuffer}>,{msg}");
				return;
			}
			if (m_WoInfo._MatGroupNo._ItemList[0].Property20 == "N")
			{
				string remark = "Reel Check (OK-00.05), MatGroupNo " + m_WoInfo._MatGroupNo._ItemList[0].MatGroupNo + ", The Barcode Check switch (Property20) is not turned on, and this barcode input is ignored., " + text3;
				LogReel(m_LastInputBarcode, "REMARK", text3, remark);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process(The Barcode Check switch (Property20) is not turned on, and this barcode input is ignored.),<{l_onMessageBuffer}>,{msg}");
				return;
			}
			if (!m_Is_Reel_Check)
			{
				string remark2 = "Reel Check(OK-00.06), Reel scanning check is turned off, so this barcode input is ignored, " + text3;
				LogReel(m_LastInputBarcode, "REMARK", text3, remark2);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process (Reel scanning check is turned off, so this barcode input is ignored),<{l_onMessageBuffer}>,{msg}");
				return;
			}
			if (m_WoInfo._WO._ItemList[0].OrderType == "ZE01")
			{
				string remark3 = "Reel Check(OK-00.07), engineering WO(ZE01), ignored, " + text3;
				LogReel(m_LastInputBarcode, "REMARK", text3, remark3);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process(Engineering WO (ZE01) is ignored),<{l_onMessageBuffer}>,{msg}");
				return;
			}
			BarcodeRecord barcodeRecord = new BarcodeRecord();
			barcodeRecord.Barocde = msg;
			barcodeRecord.ReciecTime = DateTime.Now;
			barcodeRecord.SeqNo = l_onMessageBuffer;
			if (text3.StartsWith("ERROR"))
			{
				m_Log.LogDebug(string.Format("m_ReelCheckInput_onMessageArrive buffer,process,<{0}>,{1}", l_onMessageBuffer, "The barcode begins with ERROR and is handed over to the process for processing.," + text3));
				m_BarcodeQueue.Enqueue(barcodeRecord);
				return;
			}
			if (text3.Contains(":") && text3.Contains("%"))
			{
				string remark4 = "Abnormal barcode content (NG-9802), " + text3;
				LogReel(m_LastInputBarcode, "REMARK", text3, remark4);
				string text4 = text3;
				string[] array = text3.Split(new string[1] { ":" }, StringSplitOptions.None);
				text3 = array[0];
				remark4 = "Abnormal barcode content (NG-9802), " + text4 + ", The program attempts to fix, " + text3;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{remark4}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, remark4);
			}
			if (text3.Contains("?"))
			{
				string text5 = "Abnormal barcode content (NG-9803), " + text3;
				string text6 = text3;
				try
				{
					string[] array2 = text3.Split(new string[1] { "?" }, StringSplitOptions.None);
					if (array2[0] != "")
					{
						text3 = array2[0];
					}
					else if (array2[1] != "")
					{
						text3 = array2[1];
					}
					text5 = "Abnormal barcode content (NG-9803), " + text6 + ", The program attempts to fix, " + text3;
				}
				catch (Exception ex)
				{
					text5 = "Abnormal barcode content (NG-9803), " + text6 + ", The program's attempt to repair failed," + ex.Message;
				}
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text5}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text5);
			}
			if (IsNumAndEnCh(text3))
			{
				string text7 = "Barcode Check (NG-9901), Barcode content is pure number (ignore it), " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text7}");
				LogReel(m_LastInputBarcode, " Abnormal_Barcode_Adjust", text3, text7);
				return;
			}
			if (text3.Contains('+') || text3.Contains('.') || text3.Contains('_') || text3.StartsWith("!") || text3.Contains("%"))
			{
				string text8 = "Barcode Check (NG-9902), Barcode content contains special characters (ignore it), " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text8}");
				LogReel(m_LastInputBarcode, " Abnormal_Barcode_Adjust", text3, text8);
				return;
			}
			if (text3.Length < 9)
			{
				string text9 = "Barcode Check(NG-9903), Barcode content length is less than 9 (ignore it), " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text9}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text9);
				return;
			}
			if (text3.Contains(m_MatNo) && text3.Contains(m_TapingNo) && text3.Length >= m_MatNo.Length + m_TapingNo.Length)
			{
				string text10 = "Barcode Check (NG-9904), The Barcode contains both the MatNo and TP No. It may be a problem with the scanner or the manufacturer's software (ignore it), " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text10}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text10);
				return;
			}
			if ((text3.StartsWith(m_MatNo) && !text3.EndsWith(m_MatNo)) || (!text3.StartsWith(m_MatNo) && text3.EndsWith(m_MatNo)))
			{
				string text11 = "Barcode Check (NG-9905), The MatNo barcode is mixed with other barcodes, ignore this problem., " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text11}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text11);
				return;
			}
			if ((text3.StartsWith(m_TapingNo) && !text3.EndsWith(m_TapingNo)) || (!text3.StartsWith(m_TapingNo) && text3.EndsWith(m_TapingNo)))
			{
				string text12 = "Barcode Check (NG-9906), The TapingNo barcode is mixed with other barcodes, ignore this problem., " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text12}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text12);
				return;
			}
			Barcode2D88 val2 = new Barcode2D88();
			val2.Init(text3);
			if (!IsMahchRele(text3) && !val2.IsBarcode2D)
			{
				string text13 = "Barcode Check(NG-9905), The scanned label content is not numbers + letters and is not a QR code. It does not conform to the current MatNo and TP No (numbers + letters) format. It may be a scanner abnormality (ignore it), " + msg;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,process,<{l_onMessageBuffer}>,{text13}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text3, text13);
			}
			else
			{
				f_ReelRecord(text3, barcodeRecord.ReciecTime);
				m_BarcodeQueue.Enqueue(barcodeRecord);
			}
		}
		catch (Exception ex2)
		{
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,Exception,<{l_onMessageBuffer}>,{ex2.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive buffer,End,<{l_onMessageBuffer}>,spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	public void m_ReelCheckInput_onMessageArrive(BarcodeRecord oMsgBarcode)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_3293: Unknown result type (might be due to invalid IL or missing references)
		//IL_3296: Invalid comparison between Unknown and I4
		//IL_35d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_35df: Expected O, but got Unknown
		//IL_332e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3331: Invalid comparison between Unknown and I4
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddc: Expected O, but got Unknown
		//IL_0de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dec: Expected O, but got Unknown
		//IL_10b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Expected O, but got Unknown
		//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbe: Expected O, but got Unknown
		//IL_1ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Expected O, but got Unknown
		//IL_1ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1add: Expected O, but got Unknown
		//IL_1414: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Expected O, but got Unknown
		//IL_1428: Unknown result type (might be due to invalid IL or missing references)
		//IL_142f: Expected O, but got Unknown
		//IL_1cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cff: Expected O, but got Unknown
		//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0f: Expected O, but got Unknown
		//IL_15d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15da: Expected O, but got Unknown
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f8: Expected O, but got Unknown
		//IL_1d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2431: Unknown result type (might be due to invalid IL or missing references)
		//IL_2438: Expected O, but got Unknown
		//IL_2445: Unknown result type (might be due to invalid IL or missing references)
		//IL_244c: Expected O, but got Unknown
		//IL_25b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b7: Expected O, but got Unknown
		//IL_2742: Unknown result type (might be due to invalid IL or missing references)
		//IL_2749: Expected O, but got Unknown
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2846: Expected O, but got Unknown
		//IL_2846: Unknown result type (might be due to invalid IL or missing references)
		//IL_284d: Expected O, but got Unknown
		//IL_2947: Unknown result type (might be due to invalid IL or missing references)
		//IL_294e: Expected O, but got Unknown
		//IL_2f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4a: Expected O, but got Unknown
		//IL_2fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc6: Expected O, but got Unknown
		//IL_30fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3103: Expected O, but got Unknown
		if (oMsgBarcode == null)
		{
			return;
		}
		string barocde = oMsgBarcode.Barocde;
		DateTime reciecTime = oMsgBarcode.ReciecTime;
		l_onMessageArrive = oMsgBarcode.SeqNo;
		m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,Start,<{l_onMessageArrive}>,{barocde},Buffer Count,{m_BarcodeQueue.Count},Time,{cyntecDateTime.ToMSSQL(reciecTime)}");
		string text = "Step-0";
		string strReelID = "";
		string strReelIDEncode = "";
		if (m_LastInputBarcode != null)
		{
			strReelID = m_LastInputBarcode.Reel.ReelID;
			strReelIDEncode = m_LastInputBarcode.Rawdata;
		}
		myTime = new cyntecDateTime();
		if (m_Ism_ReelCheckInputProcess)
		{
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,END (The last Process is working),<{l_onMessageArrive}>,spend(s),{myTime.ToTimeSpan()},{barocde}");
			return;
		}
		m_Ism_ReelCheckInputProcess = true;
		string text2 = "";
		EQP_OP_OperationEnum val = (EQP_OP_OperationEnum)2;
		m_Is_BarcodeCheckOK = true;
		m_ReelCheckErrorType = "";
		m_ExceptionWoNo = "";
		try
		{
			IsBuzy = true;
			if (!m_Start)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process, process (If it is not started, Reel will not be checked.),<{l_onMessageArrive}>,{barocde}");
				return;
			}
			text2 = (InputBarcode = barocde.TrimEnd('\n', '\r').Replace(Environment.NewLine, ""));
			iMessageCount++;
			if (!m_Is_Start)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process(The EQP is not started, so this barcode input is ignored.),<{l_onMessageArrive}>,{barocde}");
				return;
			}
			if (m_WoInfo._MatGroupNo._ItemList[0].Property20 == "N")
			{
				string remark = "Barcode Check (OK-00.05), MatGroupNo, " + m_WoInfo._MatGroupNo._ItemList[0].MatGroupNo + ", The barcode-check switch (Property20) is not turned on, and this barcode input is ignored., " + text2;
				LogReel(m_LastInputBarcode, "REMARK", text2, remark);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process, process (The barcode-check switch (Property20) is not turned on, and this barcode input is ignored.),<{l_onMessageArrive}>,{barocde}");
				return;
			}
			if (!m_Is_Reel_Check)
			{
				string remark2 = "Barcode Check (OK-00.06), Reel scanning check is turned off, so this barcode input is ignored, " + text2;
				LogReel(m_LastInputBarcode, "REMARK", text2, remark2);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process (Reel scanning check is turned off, so this barcode input is ignored),<{l_onMessageArrive}>,{barocde}");
				return;
			}
			if (m_WoInfo._WO._ItemList[0].OrderType == "ZE01")
			{
				string remark3 = "Barcode Check (OK-00.07), Engineering Wo (ZE01) is ignored, " + text2;
				LogReel(m_LastInputBarcode, "REMARK", text2, remark3);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process (Engineering work orders (ZE01) are ignored),<{l_onMessageArrive}>,{barocde}");
				return;
			}
			if (text2.StartsWith("ERROR"))
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG023", new object[0]);
				throw new Exception(m_Culture.ToMSg("MSG024", new object[0]) + text2);
			}
			if (text2.Contains(":") && text2.Contains("%"))
			{
				string remark4 = "Abnormal barcode content (NG-9802),The barcode content contains: %, ready to try to repair it, " + text2;
				LogReel(m_LastInputBarcode, "REMARK", text2, remark4);
				string text3 = text2;
				string[] array = text2.Split(new string[1] { ":" }, StringSplitOptions.None);
				text2 = array[0];
				remark4 = "Abnormal barcode content (NG-9802),The barcode content contains: %," + text3 + ", The program attempts to fix, " + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{remark4}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, remark4);
			}
			if (text2.Contains("?"))
			{
				string text4 = "Abnormal barcode content (NG-9803),The barcode content contains ?, try to repair it, " + text2;
				string text5 = text2;
				try
				{
					string[] array2 = text2.Split(new string[1] { "?" }, StringSplitOptions.None);
					if (array2[0] != "")
					{
						text2 = array2[0];
					}
					else if (array2[1] != "")
					{
						text2 = array2[1];
					}
					text4 = "Abnormal barcode content (NG-9803), The barcode content contains ?, " + text5 + ", try to repair it, " + text2;
				}
				catch (Exception ex)
				{
					text4 = "Abnormal barcode content (NG-9803), The barcode content contains ?， " + text5 + ", The program's attempt to repair failed," + ex.Message;
				}
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text4}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, text4);
			}
			if (IsNumAndEnCh(text2))
			{
				string text6 = "Barcode Check (NG-9901), Barcode content is pure number (ignore it), " + barocde;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text6}");
				LogReel(m_LastInputBarcode, " Abnormal_Barcode_Adjust", text2, text6);
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-9901", text2, "Barcode content is pure number (ignore it)");
				return;
			}
			if (text2.Contains('+') || text2.Contains('.') || text2.Contains('_') || text2.StartsWith("!") || text2.Contains("%"))
			{
				string text7 = "Barcode Check (NG-9902), Barcode content contains special characters (ignore it), " + barocde;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text7}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, text7);
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-9902", text2, "Barcode content contains special characters (ignore it)");
				return;
			}
			if (text2.Length < 9)
			{
				string text8 = "Barcode Check (NG-9903), Barcode content length is less than 9 (ignore it), " + barocde;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text8}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, text8);
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-9903", text2, "Barcode content length is less than 9 (ignore it)");
				return;
			}
			if (text2.Contains(m_MatNo) && text2.Contains(m_TapingNo) && text2.Length >= m_MatNo.Length + m_TapingNo.Length)
			{
				string text9 = "Barcode Check (NG-9904), The Barcode content contains both the MatNo and TP No. It may be a problem with the scanner or the manufacturer's software (ignore it), " + barocde;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text9}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, text9);
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-9904", text2, "The Barcode content contains both the MatNo and TP No. It may be a problem with the scanner or the manufacturer's software (ignore it)");
				return;
			}
			Barcode2D88 val2 = new Barcode2D88();
			val2.Init(text2);
			if (!IsMahchRele(text2) && !val2.IsBarcode2D)
			{
				string text10 = "Barcode Check (NG-9905), The scanned Barcode content is not numbers + letters and is not a QR code. It does not conform to the current MatNo and TP No (numbers + letters) format. It may be a scanner abnormality (ignore it), " + barocde;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{text10}");
				LogReel(m_LastInputBarcode, "Abnormal_Barcode_Adjust", text2, text10);
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-9905", text2, "The scanned Barcode content is not numbers + letters and is not a QR code. It does not conform to the current MatNo and TP No (numbers + letters) format. It may be a scanner abnormality (ignore it)");
				return;
			}
			ReelRecord reelRecord = null;
			if (m_LastInputBarcode != null)
			{
				reelRecord = f_ToReelRecord(m_LastInputBarcode.Reel.ReelID);
				if (reelRecord != null)
				{
					m_LastInputBarcode.MatNoCount = reelRecord.MatNoCount;
					m_LastInputBarcode.TPNoCount = reelRecord.TPNoCount;
					m_LastInputBarcode.ReelIDCount = reelRecord.ReelIDCount;
					m_LastInputBarcode.Reel.TPStartTime = reelRecord.StartTime;
					m_LastInputBarcode.ReelIDScanTime = reelRecord.StartTime;
				}
				if (text2 == m_MatNo)
				{
					LogReel(m_LastInputBarcode, "MatNo_Input", text2, "");
					m_BarcodeType = "1";
				}
				else if (text2 == m_TapingNo)
				{
					LogReel(m_LastInputBarcode, "TPNO_Input", text2, "");
					m_BarcodeType = "2";
				}
				if (m_LastInputBarcode.IsFinish && !m_LastInputBarcode.IsfinishUpdate)
				{
					f_ReelUpdate(m_LastInputBarcode);
					if (this.OnRefresh != null)
					{
						this.OnRefresh(m_LastInputBarcode, this);
					}
					m_LastInputBarcode.IsfinishUpdate = true;
				}
			}
			if (m_IsMainMat && text2 == m_MainMatNo)
			{
				string arg = "Barcode Check (OK - 01), ReelCheck scans the main MatNo," + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg}");
				return;
			}
			if (!m_IsMainMat && text2 == m_SubMatNo)
			{
				string arg2 = "Barcode Check(OK-02), ReelCheck scans the sub-MatNo," + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg2}");
				return;
			}
			if (m_IsMainMat && text2 == m_SubMatNo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG001", new object[2] { m_MainMatNo, text2 });
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (!m_IsMainMat && text2 == m_MainMatNo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG002", new object[2] { m_SubMatNo, text2 });
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (isWJ && text2.Trim().ToUpper() == m_TapingNo.Trim().ToUpper())
			{
				string arg3 = "Barcode Check (OK-03), ReelCheck recive the TapingNo of main MatNo," + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg3}");
				return;
			}
			if (m_IsMainMat && m_woReelIdInfo._TapingNoByMatNo(m_MainMatNo) == text2)
			{
				string arg4 = "Barcode Check (OK-03), ReelCheck recive the TapingNo of main MatNo," + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg4}");
				return;
			}
			if (!m_IsMainMat && m_woReelIdInfo._TapingNoByMatNo(m_SubMatNo) == text2)
			{
				string arg5 = "Barcode Check (OK-04), ReelCheck scanned the TapingNo of sub-MatNo.," + text2;
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg5}");
				return;
			}
			if (m_IsMainMat && m_woReelIdInfo._TapingNoByMatNo(m_SubMatNo) == text2)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG003", new object[2]
				{
					m_woReelIdInfo._TapingNoByMatNo(m_MainMatNo),
					text2
				});
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (!m_IsMainMat && m_woReelIdInfo._TapingNoByMatNo(m_SubMatNo) == text2)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG004", new object[2]
				{
					m_woReelIdInfo._TapingNoByMatNo(m_SubMatNo),
					text2
				});
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (!val2.IsBarcode2D)
			{
				if (!m_InputBarcodeExceptionList.ContainsKey(text2))
				{
					m_PingServrer();
					WO_ReelID_Get_WCF val3 = new WO_ReelID_Get_WCF();
					val3._BatchNo = text2;
					cyntecDateTime val4 = new cyntecDateTime();
					((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_ReelID_Get_WCF),<{l_onMessageArrive}>,spend(s),{val4.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).IsTxnError)
					{
						throw new Exception(((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).TxnErrorMsg);
					}
					if (((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).Result._itemList.Count > 0)
					{
						m_Is_BarcodeCheckOK = false;
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
						m_ExceptionWoNo = ((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).Result._itemList[0].WONo;
						m_ReelCheckMsg = m_Culture.ToMSg("MSG005", new object[3]
						{
							m_WoInfo._WO._ItemList[0].WONo,
							text2,
							((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).Result._itemList[0].WONo
						});
						f_BlockWo(((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).Result._itemList[0].WONo, "C2", m_Culture.ToMSg("MSG005", new object[3]
						{
							m_WoInfo._WO._ItemList[0].WONo,
							text2,
							((TransactionBase<WO_ReelID_Get_WCF, WO_ReelID_Get_WCFResult>)(object)val3).Result._itemList[0].WONo
						}));
						throw new Exception(m_ReelCheckMsg);
					}
				}
				if (!m_InputBarcodeExceptionList.ContainsKey(text2))
				{
					m_PingServrer();
					MatView_Get val5 = new MatView_Get();
					val5._MatNo = text2;
					cyntecDateTime val6 = new cyntecDateTime();
					((TransactionBase<MatView_Get, MatView_GetResult>)(object)val5).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (MatView_Get),<{l_onMessageArrive}>,spend(s),{val6.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<MatView_Get, MatView_GetResult>)(object)val5).IsTxnError)
					{
						throw new Exception(((TransactionBase<MatView_Get, MatView_GetResult>)(object)val5).TxnErrorMsg);
					}
					if (((TransactionBase<MatView_Get, MatView_GetResult>)(object)val5).Result._itemList.Count > 0)
					{
						m_Is_BarcodeCheckOK = false;
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
						m_ReelCheckMsg = m_Culture.ToMSg("MSG006", new object[2]
						{
							m_WoInfo._WO._ItemList[0].WONo,
							text2
						});
						f_BlockWo();
						throw new Exception(m_ReelCheckMsg);
					}
				}
				val = (EQP_OP_OperationEnum)8;
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG007", new object[2]
				{
					m_WoInfo._WO._ItemList[0].WONo,
					text2
				});
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-0107", text2, "Barcode cannot be recognized");
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			text = "Step-1";
			if (m_InputBarcodeList.ContainsKey(text2) && (reciecTime - m_InputBarcodeList[text2].Reel.TPEndTime).TotalMinutes < m_ReelUpdateDelay)
			{
				m_BarcodeType = "3";
				LogReel(m_LastInputBarcode, "REELID_Input(" + m_LastInputBarcode.Reel.ReelID + ")", text2, "Less than " + m_ReelUpdateDelay + " minutes");
				string arg6 = "Barcode Check (OK-05), Not more than " + m_ReelUpdateDelay + " minutes, ignored, " + text2 + ", " + (reciecTime - m_InputBarcodeList[text2].Reel.TPEndTime).TotalSeconds.ToString("000.0");
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,process,<{l_onMessageArrive}>,{arg6}");
				return;
			}
			if (m_InputBarcodeList.ContainsKey(text2))
			{
				text = "Step-21";
				m_BarcodeType = "3";
				m_LastInputBarcode = m_InputBarcodeList[text2];
				m_InputBarcodeList[text2].Reel.UpdateDate = reciecTime;
				m_InputBarcodeList[text2].Reel.TPEndTime = reciecTime;
				reelRecord = f_ToReelRecord(m_InputBarcodeList[text2].Reel.ReelID);
				if (reelRecord != null)
				{
					m_InputBarcodeList[text2].MatNoCount = reelRecord.MatNoCount;
					m_InputBarcodeList[text2].TPNoCount = reelRecord.TPNoCount;
					m_InputBarcodeList[text2].ReelIDCount = reelRecord.ReelIDCount;
					m_InputBarcodeList[text2].Reel.TPStartTime = reelRecord.StartTime;
				}
				LogReel(m_LastInputBarcode, "REELID_Input(" + m_LastInputBarcode.Reel.ReelID + ") ", text2, "");
				if ((reciecTime - m_InputBarcodeList[text2].ReelUpdateTime).TotalMinutes > m_ReelUpdateDelay)
				{
					text = "Step-22";
					m_PingServrer();
					string text11 = m_WoInfo._WOrouting.GetDesignatedStep("TP").WorkCenterNo.ToString();
					WOConfirm_Get val7 = new WOConfirm_Get();
					val7._WoNo = m_WoNo;
					cyntecDateTime val8 = new cyntecDateTime();
					((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process(WOConfirm_Get),<{l_onMessageArrive}>,spend(s),{val8.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).IsTxnError)
					{
						throw new Exception(((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).TxnErrorMsg);
					}
					if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).Result.GetDesignatedStep(text11).IsInTimeNG(m_WoInfo._WOConfirm.GetDesignatedStep(text11).InTime))
					{
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
						m_Is_BarcodeCheckOK = false;
						m_ReelCheckMsg = m_Culture.ToMSg("MSG008", new object[2]
						{
							cyntecDateTime.ToMSSQL(m_WoInfo._WOConfirm.GetDesignatedStep(text11).InTime),
							cyntecDateTime.ToMSSQL(((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).Result.GetDesignatedStep(text11).InTime)
						});
						f_BlockWo();
						throw new Exception(m_ReelCheckMsg);
					}
					if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val7).Result.GetDesignatedStep(text11).Finished == "Y")
					{
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
						m_Is_BarcodeCheckOK = false;
						m_ReelCheckMsg = m_Culture.ToMSg("MSG009", new object[0]);
						f_BlockWo();
						throw new Exception(m_ReelCheckMsg);
					}
					text = "Step-23";
					m_PingServrer();
					WO_ReelID_Get val9 = new WO_ReelID_Get();
					val9._ReelID = m_LastInputBarcode.Reel.ReelID;
					cyntecDateTime val10 = new cyntecDateTime();
					((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process(WO_ReelID_Get),<{l_onMessageArrive}>,spend(s),{val10.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).IsTxnError)
					{
						throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).TxnErrorMsg);
					}
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList.Count == 0)
					{
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG046", new object[0]);
						m_Is_BarcodeCheckOK = false;
						Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-0301", text2, "WO_ReelID data may be removed");
						throw new Exception(m_Culture.ToMSg("MSG026", new object[1] { val9._ReelID }));
					}
					f_logReelList(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList);
					text = "Step-24";
					if (m_WoReelStatusInfo == null)
					{
						m_Log.LogError("Step-24,Error 1, m_WoReelStatusInfo is null");
					}
					if (val9 == null)
					{
						m_Log.LogError("Step-24,Error 2, oReelIDGet is null");
					}
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result == null)
					{
						m_Log.LogError("Step-24,Error 3, oReelIDGet.Result is null");
					}
					if (!string.IsNullOrWhiteSpace(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList[0].TPStatus) && (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList[0].TPStatus != "11" || ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList[0].TPMachNo != m_machNo))
					{
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG046", new object[0]);
						m_Is_BarcodeCheckOK = false;
						text = "Step-24,2";
						string text12 = m_Culture.ToMSg("MSG027", new object[1] { m_WoReelStatusInfo.ToCodeName("11") });
						text = "Step-24,3";
						text12 += ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList[0].TPStatus;
						text = "Step-24,4";
						text12 = text12 + "(" + m_WoReelStatusInfo.ToCodeName(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val9).Result._ItemList[0].TPStatus) + ")";
						text = "Step-24,5";
						throw new Exception(text12);
					}
					text = "Step-25";
					if (m_woReelIdInfo != null && !m_woReelIdInfo._IsLastReel(m_LastInputBarcode.Reel.ReelID))
					{
						m_Is_BarcodeCheckOK = false;
						m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
						m_ReelCheckMsg = m_Culture.ToMSg("MSG010", new object[1] { m_LastInputBarcode.Reel.ReelID });
						if (!m_InputBarcodeExceptionList.ContainsKey(text2))
						{
							f_LogReelInfo(m_LastInputBarcode.Reel);
						}
						f_BlockWo();
						throw new Exception(m_ReelCheckMsg);
					}
					text = "Step-26";
					f_ReelUpdate(m_LastInputBarcode);
					text = "Step-27";
					if (this.OnRefresh != null)
					{
						this.OnRefresh(m_LastInputBarcode, this);
					}
					text = "Step-28";
				}
				m_InputBarcodeList[text2].Reel.UpdateDate = reciecTime;
				m_InputBarcodeList[text2].Reel.TPEndTime = reciecTime;
				text = "Step-29";
				reelRecord = f_ToReelRecord(m_InputBarcodeList[text2].Reel.ReelID);
				if (reelRecord != null)
				{
					m_InputBarcodeList[text2].MatNoCount = reelRecord.MatNoCount;
					m_InputBarcodeList[text2].TPNoCount = reelRecord.TPNoCount;
					m_InputBarcodeList[text2].ReelIDCount = reelRecord.ReelIDCount;
					m_InputBarcodeList[text2].Reel.TPStartTime = reelRecord.StartTime;
					m_InputBarcodeList[text2].Reel.TPEndTime = reelRecord.EndTime;
				}
				return;
			}
			text = "Step-31";
			m_PingServrer();
			string text13 = m_WoInfo._WOrouting.GetDesignatedStep("TP").WorkCenterNo.ToString();
			WOConfirm_Get val11 = new WOConfirm_Get();
			val11._WoNo = m_WoNo;
			cyntecDateTime val12 = new cyntecDateTime();
			((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).clientExecute(m_txnConn);
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WOConfirm_Get),<{l_onMessageArrive}>,spend(s),{val12.ToTimeSpan().TotalSeconds}");
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).IsTxnError)
			{
				throw new Exception(((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).TxnErrorMsg);
			}
			text = "Step-32";
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).Result.GetDesignatedStep(text13) == null)
			{
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckMsg = m_Culture.ToMSg("MSG011", new object[0]);
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).Result.GetDesignatedStep(text13).IsInTimeNG(m_WoInfo._WOConfirm.GetDesignatedStep(text13).InTime))
			{
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckMsg = m_Culture.ToMSg("MSG012", new object[2]
				{
					cyntecDateTime.ToMSSQL(m_WoInfo._WOConfirm.GetDesignatedStep(text13).InTime),
					cyntecDateTime.ToMSSQL(((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).Result.GetDesignatedStep(text13).InTime)
				});
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			text = "Step-33";
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val11).Result.GetDesignatedStep(text13).Finished == "Y")
			{
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckMsg = m_Culture.ToMSg("MSG009", new object[0]);
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			text = "Step-34";
			m_PingServrer();
			WO_ReelID_Get val13 = new WO_ReelID_Get();
			val13._ReelID = text2;
			cyntecDateTime val14 = new cyntecDateTime();
			((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).clientExecute(m_txnConn);
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_ReelID_Get),<{l_onMessageArrive}>,spend(s),{val14.ToTimeSpan().TotalSeconds}");
			if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).IsTxnError)
			{
				throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).TxnErrorMsg);
			}
			if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).Result._ItemList.Count == 0)
			{
				val = (EQP_OP_OperationEnum)8;
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG046", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG014", new object[1] { val13._ReelID });
				Wo_ExceptionLog("ReelID", strReelID, strReelIDEncode, "NG-0301", text2, "QR code cannot be recognized");
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			f_logReelList(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).Result._ItemList);
			text = "Step-35";
			myReel oMyReel = new myReel();
			oMyReel.Rawdata = text2;
			oMyReel.Reel = ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val13).Result._ItemList[0];
			oMyReel.Matno = m_MatNo;
			oMyReel.TPNo = m_TapingNo;
			oMyReel.ReelUpdateDelay = m_ReelUpdateDelay;
			if (m_LastInputBarcode != null)
			{
				m_LastInputBarcode.NextReel = oMyReel;
			}
			oMyReel.PreReel = m_LastInputBarcode;
			text = "Step-36";
			reelRecord = f_ToReelRecord(oMyReel.Reel.ReelID);
			if (reelRecord != null)
			{
				oMyReel.Reel.TPStartTime = reelRecord.StartTime;
			}
			text = "Step-37";
			bool flag = false;
			if (string.IsNullOrWhiteSpace(oMyReel.Reel.TPStatus))
			{
				flag = true;
			}
			text = "Step-38";
			if (oMyReel.Reel.WONo != m_WoInfo._WO._ItemList[0].WONo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG028", new object[0]);
				m_ExceptionWoNo = oMyReel.Reel.WONo;
				m_ReelCheckMsg = m_Culture.ToMSg("MSG015", new object[3]
				{
					m_WoNo,
					oMyReel.Reel.WONo,
					oMyReel.Reel.ReelID
				});
				f_BlockWo(oMyReel.Reel.WONo, "C2", m_Culture.ToMSg("MSG029", new object[3]
				{
					oMyReel.Reel.WONo,
					oMyReel.Reel.ReelID,
					m_WoNo
				}));
				oMyReel.IsBlock = true;
				throw new Exception(m_ReelCheckMsg);
			}
			if (!string.IsNullOrWhiteSpace(oMyReel.Reel.TPStatus) && !oMyReel.Reel.TPStatus.Equals("00") && !oMyReel.Reel.TPStatus.Equals("11"))
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG016", new object[3]
				{
					oMyReel.Reel.ReelID,
					m_WoReelStatusInfo.ToCodeName("11"),
					oMyReel.Reel.TPStatus + m_WoReelStatusInfo.ToCodeName(oMyReel.Reel.TPStatus)
				});
				f_BlockWo();
				oMyReel.IsBlock = true;
				throw new Exception(m_ReelCheckMsg);
			}
			if (!string.IsNullOrWhiteSpace(oMyReel.Reel.TPMachNo) && oMyReel.Reel.TPMachNo != m_machNo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG017", new object[3]
				{
					oMyReel.Reel.ReelID,
					m_machNo,
					oMyReel.Reel.TPMachNo + "(" + cyntecDateTime.ToMSSQL(oMyReel.Reel.TPEndTime) + ")"
				});
				f_BlockWo();
				oMyReel.IsBlock = true;
				throw new Exception(m_ReelCheckMsg);
			}
			if (oMyReel.Reel.MatNo != m_MatNo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG031", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG018", new object[3]
				{
					oMyReel.Reel.ReelID,
					m_MatNo,
					oMyReel.Reel.MatNo
				});
				f_BlockWo();
				oMyReel.IsBlock = true;
				throw new Exception(m_ReelCheckMsg);
			}
			if (oMyReel.Reel.BatchNo != m_TapingNo)
			{
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG031", new object[0]);
				m_ReelCheckMsg = m_Culture.ToMSg("MSG019", new object[3]
				{
					oMyReel.Reel.ReelID,
					m_TapingNo,
					oMyReel.Reel.BatchNo
				});
				f_BlockWo();
				oMyReel.IsBlock = true;
				throw new Exception(m_ReelCheckMsg);
			}
			if (m_woReelIdInfo != null && !m_woReelIdInfo._IsLastReel(oMyReel.Reel.ReelID))
			{
				m_PingServrer();
				WO_ReelID_Get val15 = new WO_ReelID_Get();
				val15._WoNo = m_WoNo;
				cyntecDateTime val16 = new cyntecDateTime();
				((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val15).clientExecute(m_txnConn);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_ReelID_Get),<{l_onMessageArrive}>,spend (s),{val16.ToTimeSpan().TotalSeconds}");
				if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val15).IsTxnError)
				{
					throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val15).TxnErrorMsg);
				}
				m_woReelIdInfo = ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val15).Result;
				if (m_woReelIdInfo != null && !m_woReelIdInfo._IsLastReel(oMyReel.Reel.ReelID))
				{
					m_Is_BarcodeCheckOK = false;
					m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
					m_ReelCheckMsg = m_Culture.ToMSg("MSG020", new object[1] { oMyReel.Reel.ReelID });
					if (!m_InputBarcodeExceptionList.ContainsKey(text2))
					{
						f_LogReelInfo(oMyReel.Reel);
					}
					f_BlockWo();
					oMyReel.IsBlock = true;
					throw new Exception(m_ReelCheckMsg);
				}
			}
			text = "Step-39";
			m_BarcodeType = "3";
			if (flag)
			{
				m_PingServrer();
				WO_ReelIDLog_Add val17 = new WO_ReelIDLog_Add();
				val17._Condition.Action = "00005";
				val17._Condition.BatchNo = m_TapingNo;
				val17._Condition.Big_ReelID = oMyReel.Reel.ReelID;
				val17._Condition.ConfirmID = m_WoInfo._WOConfirm._CurrentConfirm.ID;
				val17._Condition.CreateDate = reciecTime;
				val17._Condition.CreateUser = m_UserID;
				val17._Condition.DNDOC = "";
				val17._Condition.DNITEM = "";
				val17._Condition.IsSAPLog = false;
				val17._Condition.MatNo = oMyReel.Reel.MatNo;
				val17._Condition.MMDOC = "";
				val17._Condition.MMITEM = "";
				val17._Condition.Parent_ReelID = "";
				val17._Condition.ReelID = oMyReel.Reel.ReelID;
				val17._Condition.SBatchNo = oMyReel.Reel.BatchNo;
				val17._Condition.SMatNo = oMyReel.Reel.MatNo;
				val17._Condition.Status = "11";
				val17._Condition.UserID = m_UserID;
				val17._Condition.WONO = m_WoNo;
				cyntecDateTime val18 = new cyntecDateTime();
				((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val17).clientExecute(m_txnConn);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_ReelIDLog_Add),<{l_onMessageArrive}>,spend (s),{val18.ToTimeSpan().TotalSeconds}");
				if (((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val17).IsTxnError)
				{
					throw new Exception(((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val17).TxnErrorMsg);
				}
				m_Log.LogDebug("The first scan comes in ReelIDLogAdd ");
				LogReel(oMyReel, "API_WO_ReelIDLog_Add", oMyReel.Rawdata, "");
			}
			text = "Step-3A";
			if (flag && m_WoInfo._MatView._itemList[0].Packing != "86" && !string.IsNullOrWhiteSpace(m_WoInfo._MatView._itemList[0].Packing))
			{
				WO_Packing_Save val19 = new WO_Packing_Save();
				WO_Packing val20 = new WO_Packing();
				val20.BatchNo = m_WoInfo._WO._ItemList[0].BatchNo;
				val20.CreateUser = m_UserID;
				val20.Enable = true;
				val20.FlowID = m_WoInfo._WOConfirm.ByStepNo(m_WorkcenterNo).FlowID;
				val20.LabelNumber_CheckIn = "";
				val20.LabelNumber_CheckOut = oMyReel.Reel.ReelID;
				val20.MatGroupNo = m_WoInfo._WO._ItemList[0].MatGroupNo;
				val20.MatNo = m_MatNo;
				val20.PackType_CheckIn = "";
				val20.PackType_CheckOut = "86";
				val20.UpdateUser = m_UserID;
				val20.WONo = m_WoNo;
				val20.WorkCenterNo = m_WorkcenterNo;
				val19._ItemList.Add(val20);
				cyntecDateTime val21 = new cyntecDateTime();
				((TransactionBase<WO_Packing_Save, WO_Packing_SaveResult>)(object)val19).clientExecute(m_txnConn);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_Packing_Save),<{l_onMessageArrive}>,spend (s),{val21.ToTimeSpan().TotalSeconds}");
				if (((TransactionBase<WO_Packing_Save, WO_Packing_SaveResult>)(object)val19).IsTxnError)
				{
					throw new Exception(((TransactionBase<WO_Packing_Save, WO_Packing_SaveResult>)(object)val19).TxnErrorMsg);
				}
			}
			if (m_LastInputBarcode != null && m_LastInputBarcode.Reel.ReelID != oMyReel.Reel.ReelID)
			{
				string arg7 = "Check the previous Reel";
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg7}");
				m_LastInputBarcode.TapingUILogTime = DateTime.MaxValue;
				m_LastInputBarcode.Reel.TPEndTime = reciecTime;
				string text14 = f_IsLastReelOK(m_LastInputBarcode);
				if (!string.IsNullOrWhiteSpace(text14))
				{
					m_Is_BarcodeCheckOK = false;
					m_ReelCheckErrorType = m_Culture.ToMSg("MSG032", new object[0]);
					m_ReelCheckMsg = text14;
					f_EqpOpRecordSave((EQP_OP_OperationEnum)2, m_MatNo, m_Culture.ToMSg("MSG033", new object[0]), null, IsException: true, m_ReelCheckMsg, m_ExceptionWoNo, m_ReelCheckErrorType);
				}
				f_ReelUpdate(m_LastInputBarcode);
				reelRecord = f_ToReelRecord(oMyReel.Reel.ReelID);
				if (reelRecord == null)
				{
					throw new Exception("f_ToReelRecord() exception, cannot fetch ReelID, " + oMyReel.Reel.ReelID);
				}
				oMyReel.MatNoCount = reelRecord.MatNoCount;
				oMyReel.TPNoCount = reelRecord.TPNoCount;
				oMyReel.ReelIDCount = reelRecord.ReelIDCount;
				oMyReel.Reel.TPStartTime = reelRecord.StartTime;
				oMyReel.Reel.TPEndTime = reelRecord.EndTime;
				f_ReelUpdate(oMyReel);
			}
			else
			{
				string arg8 = "Do not check the previous Reel";
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg8}");
				reelRecord = f_ToReelRecord(oMyReel.Reel.ReelID);
				if (reelRecord == null)
				{
					throw new Exception("f_ToReelRecord() exception, Unable to get ReelID, " + oMyReel.Reel.ReelID);
				}
				oMyReel.MatNoCount = reelRecord.MatNoCount;
				oMyReel.TPNoCount = reelRecord.TPNoCount;
				oMyReel.ReelIDCount = reelRecord.ReelIDCount;
				oMyReel.Reel.TPStartTime = reelRecord.StartTime;
				oMyReel.Reel.TPEndTime = reelRecord.EndTime;
				f_ReelUpdate(oMyReel);
			}
			text = "Step-3B";
			if (m_LastInputBarcode != null && m_LastInputBarcode.Reel.ReelID != oMyReel.Reel.ReelID)
			{
				decimal num = Convert.ToDecimal((m_LastInputBarcode.Reel.TPEndTime - m_LastInputBarcode.Reel.TPStartTime).TotalMinutes);
				string arg9 = "Last Reel job time ，" + num + "minutes, set time," + m_WoInfo._MatView._itemList[0].ReelTapeMinTime + "minutes, Is EQP C3 enabled?  " + m_EqpOpCheckInfo.IsContains((EQP_OP_OperationEnum)3);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg9}");
				if (num < m_WoInfo._MatView._itemList[0].ReelTapeMinTime && m_EqpOpCheckInfo.IsContains((EQP_OP_OperationEnum)3))
				{
					arg9 = "Abnormality on reel boarding (NG-0113), Reel operation time is less than the set value, start time:" + cyntecDateTime.ToMSSQL(m_LastInputBarcode.Reel.TPStartTime) + "，End Time:" + cyntecDateTime.ToMSSQL(m_LastInputBarcode.Reel.TPEndTime);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg9}");
					m_Is_BarcodeCheckOK = false;
					string text15 = m_Culture.ToMSg("MSG035", new object[3]
					{
						m_LastInputBarcode.Reel.ReelID,
						Math.Round(num, 3),
						m_WoInfo._MatView._itemList[0].ReelTapeMinTime
					});
					m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
					m_ReelCheckMsg = text15;
					f_EqpOpRecordSave((EQP_OP_OperationEnum)3, m_MatNo, m_Culture.ToMSg("MSG036", new object[0]), null, IsException: true, text15, "", m_ReelCheckErrorType);
					f_WOReelIDLogException(m_LastInputBarcode, (EQP_OP_OperationEnum)3, m_Culture.ToMSg("MSG036", new object[0]), text15);
				}
				else
				{
					arg9 = "Reel operation time is not less than the set value";
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process,<{l_onMessageArrive}>,{arg9}");
				}
				m_LastInputBarcode = oMyReel;
			}
			text = "Step-3C";
			m_PingServrer();
			WO_ReelID_Get val22 = new WO_ReelID_Get();
			val22._WoNo = m_WoInfo._WO._ItemList[0].WONo;
			foreach (WorkOrder item in m_WoInfo._WOGroup._itemList)
			{
				val22._GorupWoNoList.Add(item.WONo);
			}
			cyntecDateTime val23 = new cyntecDateTime();
			((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).clientExecute(m_txnConn);
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (WO_ReelID_Get),<{l_onMessageArrive}>,spend (s),{val23.ToTimeSpan().TotalSeconds}");
			if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).IsTxnError)
			{
				throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).TxnErrorMsg);
			}
			m_woReelIdInfo = ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).Result;
			IEnumerable<WO_ReelID> source = ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).Result._ItemList.Where((WO_ReelID o) => o.TPStatus == "11" && o.TPMachNo == m_machNo && o.TPStartTime != DateTime.MinValue);
			m_iReelCount = source.Count();
			source = ((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val22).Result._ItemList.Where((WO_ReelID o) => o.ReelID == oMyReel.Reel.ReelID);
			if (source.Count() > 0)
			{
				oMyReel.Reel = source.ElementAt(0);
			}
			text = "Step-3D";
			m_LastInputBarcode = oMyReel;
			if (!m_InputBarcodeList.ContainsKey(text2))
			{
				m_InputBarcodeList.Add(text2, oMyReel);
			}
			text = "Step-3E";
			if (this.OnRefresh != null)
			{
				cyntecDateTime val24 = new cyntecDateTime();
				this.OnRefresh(m_LastInputBarcode, this);
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process (OnRefresh),<{l_onMessageArrive}>,spend (s),{val24.ToTimeSpan().TotalSeconds}");
			}
			text = "Step-3F";
		}
		catch (Exception ex2)
		{
			string text16 = "";
			if (ex2.Message.Contains("並未將物件參考設定為物件的執行個體") || ex2.Message.Contains("Object reference not set to an instance of an object"))
			{
				StackTrace stackTrace = new StackTrace(ex2, fNeedFileInfo: true);
				text16 = stackTrace.ToString();
				m_Log.LogDebug(string.Format("m_ReelCheckInput_onMessageArrive process,exception,<{0}>,{1}", l_onMessageArrive, text + "," + text2 + "," + ex2.Message + "," + text16));
			}
			else
			{
				m_Log.LogDebug(string.Format("m_ReelCheckInput_onMessageArrive process,exception,<{0}>,{1}", l_onMessageArrive, text + "," + text2 + "," + ex2.Message));
			}
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step1,<{l_onMessageArrive}>");
			LogReel(m_LastInputBarcode, "ERROR", text2, ex2.Message);
			try
			{
				if ((int)val == 2)
				{
					if (!m_InputBarcodeExceptionList.ContainsKey(text2) && !m_Is_BarcodeCheckOK)
					{
						m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step2,<{l_onMessageArrive}>");
						f_EqpOpRecordSave((EQP_OP_OperationEnum)2, m_MatNo, "Check Reel Barcode", null, IsException: true, ex2.Message, m_ExceptionWoNo, m_ReelCheckErrorType);
						f_WOReelIDLogException(m_LastInputBarcode, (EQP_OP_OperationEnum)2, "Check Reel Barcode", ex2.Message);
					}
				}
				else if ((int)val == 3)
				{
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,Exception-Step3,<{l_onMessageArrive}>");
					f_EqpOpRecordSave((EQP_OP_OperationEnum)3, m_MatNo, m_Culture.ToMSg("MSG036", new object[0]), null, IsException: true, ex2.Message, m_ExceptionWoNo, m_ReelCheckErrorType);
					f_WOReelIDLogException(m_LastInputBarcode, (EQP_OP_OperationEnum)3, m_Culture.ToMSg("MSG036", new object[0]), ex2.Message);
				}
			}
			catch (Exception ex3)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step4,<{l_onMessageArrive}>,{ex3.Message}");
			}
			try
			{
				if (!m_InputBarcodeExceptionList.ContainsKey(text2))
				{
					if (m_Is_BarcodeCheckOK)
					{
						m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step5,<{l_onMessageArrive}>");
						f_TapingUILog((EQP_TapingUILogType)98, (EQP_TapingUILogAction)2, ex2.Message);
					}
					else
					{
						m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step6,<{l_onMessageArrive}>");
						f_TapingUILog((EQP_TapingUILogType)97, (EQP_TapingUILogAction)2, ex2.Message);
					}
				}
			}
			catch (Exception ex4)
			{
				m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step7,<{l_onMessageArrive}>,{ex4.Message}");
			}
			if (!m_InputBarcodeExceptionList.ContainsKey(text2))
			{
				m_InputBarcodeExceptionList.Add(text2, text2);
				string message = ex2.Message;
				message += ", (Current Process:";
				message = ((!m_IsMainMat) ? (message + m_SubMatNo + "/" + m_woReelIdInfo._TapingNoByMatNo(m_SubMatNo)) : (message + m_MainMatNo + "/" + m_woReelIdInfo._TapingNoByMatNo(m_MainMatNo)));
				message += ")";
				if (!string.IsNullOrWhiteSpace(text16))
				{
					message = message + ", " + text16;
				}
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
				m_ReelCheckMsg = message;
				if (this.OnRefresh != null)
				{
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,exception-Step8,<{l_onMessageArrive}>");
					cyntecDateTime val25 = new cyntecDateTime();
					this.OnRefresh(m_LastInputBarcode, this);
					m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive,process(OnRefresh),<{l_onMessageArrive}>,spend(s),{val25.ToTimeSpan().TotalSeconds}");
				}
			}
		}
		finally
		{
			IsBuzy = false;
			m_Ism_ReelCheckInputProcess = false;
			if (m_LastInputBarcode != null)
			{
				if (!m_LastInputBarcode.m_BarocdeScanList.ContainsKey(text2) && text2 != m_LastInputBarcode.Rawdata)
				{
					if (text2 == m_MainMatNo)
					{
						m_LastInputBarcode.m_BarocdeScanList.Add(text2, "1.主料號," + text2);
					}
					else if (text2 == m_SubMatNo)
					{
						m_LastInputBarcode.m_BarocdeScanList.Add(text2, "2.副料號," + text2);
					}
					else if (text2 == m_TapingNo)
					{
						m_LastInputBarcode.m_BarocdeScanList.Add(text2, "3.TPNo.," + text2);
					}
					else
					{
						m_LastInputBarcode.m_BarocdeScanList.Add(text2, "其他(異常), " + text2);
					}
				}
				if (m_LastInputBarcode != null && !m_LastInputBarcode.IsTapingUILogTimeout)
				{
				}
			}
			m_Log.LogDebug($"m_ReelCheckInput_onMessageArrive process,END,<{l_onMessageArrive}>,Spend(s),{myTime.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_logReelList(List<WO_ReelID> woReelList)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ReelID,Status,TPStatus,TpStartTime,TPEndTim").Append(Environment.NewLine);
			foreach (WO_ReelID woReel in woReelList)
			{
				stringBuilder.Append(woReel.ReelID).Append(",").Append(woReel.Status)
					.Append(",")
					.Append(woReel.TPStatus)
					.Append(",")
					.Append(cyntecDateTime.ToMSSQL(woReel.TPStartTime))
					.Append(",")
					.Append(cyntecDateTime.ToMSSQL(woReel.TPEndTime));
			}
			m_Log.LogDebug($"m_ReelCheckInput_f_logReelList,Recoed,<{l_onMessageArrive}>,{stringBuilder.ToString()}");
		}
		catch
		{
		}
	}

	public myReel f_ToMyReel(ReelRecord oRecord)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		myReel myReel2 = new myReel();
		myReel2.Reel = new WO_ReelID();
		myReel2.Reel.ReelID = oRecord.ReelID;
		myReel2.ReelIDScanTime = DateTime.Now;
		myReel2.ReelIDCount = oRecord.ReelIDCount;
		myReel2.MatNoCount = oRecord.MatNoCount;
		myReel2.TPNoCount = oRecord.TPNoCount;
		myReel2.StartTime = oRecord.StartTime;
		return myReel2;
	}

	public ReelRecord f_ToReelRecord(string ReelID)
	{
		foreach (ReelRecord reelRecord in m_ReelRecordList)
		{
			if (reelRecord.ReelID == ReelID)
			{
				return reelRecord;
			}
		}
		return null;
	}

	public void f_ReelRecord(string msg, DateTime now)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		if (msg == m_MatNo)
		{
			if (m_ReelRecordList.Count != 0)
			{
				m_ReelRecordList.Last().Count(msg, now);
				myReel oMyReel = f_ToMyReel(m_ReelRecordList.Last());
				LogReel(oMyReel, "REELRECORD_MATNO", msg, "");
			}
			return;
		}
		if (msg == m_TapingNo)
		{
			if (m_ReelRecordList.Count != 0)
			{
				m_ReelRecordList.Last().Count(msg, now);
				myReel oMyReel2 = f_ToMyReel(m_ReelRecordList.Last());
				LogReel(oMyReel2, "REELRECORD_TPNO", msg, "");
			}
			return;
		}
		Barcode2D88 val = new Barcode2D88();
		val.Init(msg);
		if (!val.IsBarcode2D)
		{
			return;
		}
		string text = f_ReelIDDecode(msg);
		WO_ReelID val2 = f_ToMEsReel(text);
		if (val2 == null)
		{
			return;
		}
		ReelRecord reelRecord = f_ToReelRecord(text);
		if (reelRecord == null)
		{
			reelRecord = new ReelRecord();
			reelRecord.ReelID = text;
			reelRecord.MatNo = m_MatNo;
			reelRecord.TPNo = m_TapingNo;
			reelRecord.StartTime = now;
			reelRecord.EndTime = now;
			reelRecord.Count(text, now);
			if (m_ReelRecordList.Count > 0)
			{
				m_ReelRecordList.Last().EndTime = now;
			}
			WO_ReelID val3 = f_ToMEsReel(text);
			if (val3 != null && val3.TPStartTime != DateTime.MinValue)
			{
				reelRecord.StartTime = val3.TPStartTime;
			}
			if (val3 != null)
			{
				reelRecord.ReelIDCount = val3.ReelIDCount + 1;
				reelRecord.MatNoCount = val3.MatNoCount;
				reelRecord.TPNoCount = val3.TPNoCount;
			}
			m_ReelRecordList.Add(reelRecord);
			myReel oMyReel3 = f_ToMyReel(reelRecord);
			LogReel(oMyReel3, "REELRECORD_REELID", msg, "");
		}
		else
		{
			reelRecord.Count(text, now);
			myReel oMyReel4 = f_ToMyReel(reelRecord);
			LogReel(oMyReel4, "REELRECORD_REELID", msg, "");
		}
	}

	public void f_ReelRecordRefresh()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,Start,<{l_onMessageArrive}>");
			foreach (ReelRecord reelRecord in m_ReelRecordList)
			{
				myReel myReel2 = f_ToMyReel(reelRecord);
				if (reelRecord.ErrorList.Contains("NG-0110"))
				{
					LogReel(myReel2, "REMARK", myReel2.Rawdata, "because Reel check exception, no re-update reel status");
					continue;
				}
				if (!reelRecord.IsWO_ReelID_Update_OK)
				{
					m_PingServrer();
					WO_ReelID_Get val2 = new WO_ReelID_Get();
					val2._ReelID = reelRecord.ReelID;
					cyntecDateTime val3 = new cyntecDateTime();
					((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,過程(WO_ReelID_Get),<{l_onMessageArrive}>,spend (s),{val3.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).IsTxnError)
					{
						throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).TxnErrorMsg);
					}
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList.Count > 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].ReelID).Append(",Status,").Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].Status)
							.Append(",TPStatus,")
							.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].TPStatus)
							.Append(",TCStatus,")
							.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].TCStatus);
						m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (WO_ReelID_Get),<{l_onMessageArrive}>,{stringBuilder.ToString()}");
					}
					if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList.Count > 0 && "00|11".Contains(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].Status))
					{
						m_PingServrer();
						WO_ReelID_Update val4 = new WO_ReelID_Update();
						val4._reelID = reelRecord.ReelID;
						val4._UpdateUser = m_UserID;
						val4._statusCode = "11";
						val4._startTime = reelRecord.StartTime;
						val4._endTime = reelRecord.EndTime;
						val4._machNo = m_machNo;
						cyntecDateTime val5 = new cyntecDateTime();
						((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val4).clientExecute(m_txnConn);
						m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (WO_ReelID_Update),<{l_onMessageArrive}>,spend (s),{val5.ToTimeSpan().TotalSeconds}");
						if (((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val4).IsTxnError)
						{
							throw new Exception(((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val4).TxnErrorMsg);
						}
					}
					LogReel(myReel2, "API_WO_ReelID_Update", myReel2.Rawdata, "UPDATE");
					reelRecord.IsWO_ReelID_Update_OK = true;
				}
				if (!reelRecord.IsWO_ReelIDLog_Add_OK)
				{
					m_PingServrer();
					WO_ReelIDLog_Add val6 = new WO_ReelIDLog_Add();
					val6._Condition.Action = "00005";
					val6._Condition.BatchNo = m_TapingNo;
					val6._Condition.Big_ReelID = myReel2.Reel.ReelID;
					val6._Condition.ConfirmID = m_WoInfo._WOConfirm._CurrentConfirm.ID;
					val6._Condition.CreateDate = myReel2.StartTime;
					val6._Condition.CreateUser = m_UserID;
					val6._Condition.DNDOC = "";
					val6._Condition.DNITEM = "";
					val6._Condition.IsSAPLog = false;
					val6._Condition.MatNo = myReel2.Reel.MatNo;
					val6._Condition.MMDOC = "";
					val6._Condition.MMITEM = "";
					val6._Condition.Parent_ReelID = "";
					val6._Condition.ReelID = myReel2.Reel.ReelID;
					val6._Condition.SBatchNo = myReel2.Reel.BatchNo;
					val6._Condition.SMatNo = myReel2.Reel.MatNo;
					val6._Condition.Status = "11";
					val6._Condition.UserID = m_UserID;
					val6._Condition.WONO = m_WoNo;
					cyntecDateTime val7 = new cyntecDateTime();
					((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val6).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (WO_ReelIDLog_Add),<{l_onMessageArrive}>,spend (s),{val7.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val6).IsTxnError)
					{
						throw new Exception(((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val6).TxnErrorMsg);
					}
					LogReel(myReel2, "API_WO_ReelIDLog_Add", myReel2.Rawdata, "UPDATE");
					reelRecord.IsWO_ReelIDLog_Add_OK = true;
				}
				if (!reelRecord.IsWO_ReelCheck_Save_OK)
				{
					m_PingServrer();
					WO_ReelCheck_Save val8 = new WO_ReelCheck_Save();
					WO_ReelCheck val9 = new WO_ReelCheck();
					val9.ComputerName = Environment.MachineName;
					val9.EqpNo = m_machNo;
					val9.MatNo = m_MatNo;
					val9.MatNoCount = myReel2.Reel.MatNoCount;
					val9.ReelID = myReel2.Reel.ReelID;
					val9.ReelIDCount = myReel2.Reel.ReelIDCount;
					val9.StartTime = myReel2.StartTime;
					val9.TPNO = m_TapingNo;
					val9.TPNoCount = myReel2.Reel.TPNoCount;
					val9.UpdateTime = DateTime.Now;
					val9.WoNo = m_WoNo;
					val9.TapingUILogTxnID = myReel2.TapingUILogTxnID;
					val8._WoReelCheckList.Add(val9);
					cyntecDateTime val10 = new cyntecDateTime();
					((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val8).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (WO_ReelCheck_Save),<{l_onMessageArrive}>,spend (s),{val10.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val8).IsTxnError)
					{
						throw new Exception(((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val8).TxnErrorMsg);
					}
					LogReel(myReel2, "API_WO_ReelCheck_Save", myReel2.Rawdata, "UPDATE");
					reelRecord.IsWO_ReelCheck_Save_OK = true;
				}
				foreach (string aPI_WO_ReelIDLog_Add_Exception in reelRecord.API_WO_ReelIDLog_Add_ExceptionList)
				{
					WO_ReelIDLog_Add val11 = TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>.ToObject(Convert.FromBase64String(aPI_WO_ReelIDLog_Add_Exception));
					if (val11 != null)
					{
						cyntecDateTime val12 = new cyntecDateTime();
						((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val11).clientExecute(m_txnConn);
						m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (WO_ReelIDLog_Add),<{l_onMessageArrive}>,spend (s),{val12.ToTimeSpan().TotalSeconds}");
						if (((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val11).IsTxnError)
						{
							throw new Exception(((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val11).TxnErrorMsg);
						}
						LogReel(myReel2, "API_WO_ReelIDLog_Add_Exception", "", "UPDATE");
					}
				}
				foreach (string value in reelRecord.EqpOpRecordSaveList.Values)
				{
					EQP_OP_Record_Save val13 = TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>.ToObject(Convert.FromBase64String(value));
					if (val13 == null)
					{
						continue;
					}
					string barcode = "";
					foreach (EQP_OP_Record eqpOpRecord in val13._EqpOpRecordList)
					{
						eqpOpRecord.SysID = Guid.NewGuid().ToString();
						barcode = eqpOpRecord.Instruction;
					}
					cyntecDateTime val14 = new cyntecDateTime();
					((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val13).clientExecute(m_txnConn);
					m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,process (EQP_OP_Record_Save),<{l_onMessageArrive}>,spend (s),{val14.ToTimeSpan().TotalSeconds}");
					if (((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val13).IsTxnError)
					{
						throw new Exception(((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val13).TxnErrorMsg);
					}
					LogReel(myReel2, "API_EQP_OP_Record_Save", barcode, "UPDATE");
				}
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,exception 1,<{l_onMessageArrive}>,spend(s),{val.ToTimeSpan().TotalSeconds},{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,END,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
		foreach (ReelRecord reelRecord2 in m_ReelRecordList)
		{
			if (!reelRecord2.IsTxnComplete)
			{
				strReelRecordRefreshErrorMsg = "Some Reels have not been updated yet";
				m_Log.LogDebug($"m_ReelCheckInput_f_ReelRecordRefresh,exception 2,<{l_onMessageArrive}>,{strReelRecordRefreshErrorMsg}");
				break;
			}
		}
	}

	public List<string> WoFinish()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		List<string> result = new List<string>();
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			if (m_LastInputBarcode != null)
			{
				ReelRecord reelRecord = f_ToReelRecord(m_LastInputBarcode.Reel.ReelID);
				if (reelRecord != null)
				{
					m_LastInputBarcode.MatNoCount = reelRecord.MatNoCount;
					m_LastInputBarcode.TPNoCount = reelRecord.TPNoCount;
					m_LastInputBarcode.ReelIDCount = reelRecord.ReelIDCount;
					m_LastInputBarcode.Reel.TPStartTime = reelRecord.StartTime;
					m_LastInputBarcode.Reel.TPEndTime = reelRecord.EndTime;
				}
				m_LastInputBarcode.TapingUILogTime = DateTime.MaxValue;
				m_LastInputBarcode.IsWoFinish = true;
				m_PingServrer();
				WO_ReelID_Get val2 = new WO_ReelID_Get();
				val2._ReelID = m_LastInputBarcode.Reel.ReelID;
				cyntecDateTime val3 = new cyntecDateTime();
				((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).clientExecute(m_txnConn);
				m_Log.LogDebug($"m_ReelCheckInput_WoFinish,process (WO_ReelID_Get),<{l_onMessageArrive}>,spend (s),{val3.ToTimeSpan().TotalSeconds}");
				if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).IsTxnError)
				{
					throw new Exception(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).TxnErrorMsg);
				}
				if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].ReelID).Append(",Status,").Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].Status)
						.Append(",TPStatus,")
						.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].TPStatus)
						.Append(",TCStatus,")
						.Append(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].TCStatus);
					m_Log.LogDebug($"m_ReelCheckInput_WoFinish,process (WO_ReelID_Get),<{l_onMessageArrive}>,{stringBuilder.ToString()}");
				}
				if (((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList.Count > 0 && "00|11".Contains(((TransactionBase<WO_ReelID_Get, WO_ReelID_GetResult>)(object)val2).Result._ItemList[0].Status) && f_IsWOStatusOK() == "OK")
				{
					f_ReelUpdate(m_LastInputBarcode);
				}
			}
			string[] lines = m_FileLogReel.ReadLines();
			m_ReelRecordList = ReelRecord.ReadReelRecord(lines).Values.ToList();
			if (m_ReelRecordList.Count > 0 && m_ReelRecordList[0].IsNewVersion)
			{
				f_ReelRecordRefresh();
			}
			else
			{
				string arg = "Old version records do not re-update Reel times to avoid exceptions.";
				m_Log.LogDebug($"m_ReelCheckInput_WoFinish,process,<{l_onMessageArrive}>,{arg}");
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_WoFinish,exception,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_WoFinish,END,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
		return result;
	}

	private string f_IsLastReelOK(myReel oMyReel)
	{
		string text = "";
		if (oMyReel.MatNoCount < m_MatNoCountMin)
		{
			text = m_Culture.ToMSg("MSG041", new object[3]
			{
				oMyReel.Reel.ReelID,
				oMyReel.MatNoCount,
				m_MatNoCountMin
			});
			m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
			m_ReelCheckMsg = text;
			f_BlockWo();
			oMyReel.IsBlock = true;
			f_TapingUILog((EQP_TapingUILogType)97, (EQP_TapingUILogAction)2, text);
		}
		else if (oMyReel.TPNoCount < m_TPNoCountMin)
		{
			text = m_Culture.ToMSg("MSG042", new object[3]
			{
				oMyReel.Reel.ReelID,
				oMyReel.TPNoCount,
				m_TPNoCountMin
			});
			m_ReelCheckErrorType = m_Culture.ToMSg("MSG025", new object[0]);
			m_ReelCheckMsg = text;
			f_BlockWo();
			oMyReel.IsBlock = true;
			f_TapingUILog((EQP_TapingUILogType)97, (EQP_TapingUILogAction)2, text);
		}
		return text;
	}

	private void f_BlockWo(string WoNo, string BlockCode, string BlockReason)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		WorkOrder_GetResult val = null;
		cyntecDateTime val2 = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo1,Start,<{l_onMessageArrive}>");
			m_PingServrer();
			Workorder_Get val3 = new Workorder_Get();
			val3._WoNo = WoNo;
			((TransactionBase<Workorder_Get, WorkOrder_GetResult>)(object)val3).clientExecute(m_txnConn);
			if (((TransactionBase<Workorder_Get, WorkOrder_GetResult>)(object)val3).IsTxnError)
			{
				throw new Exception(((TransactionBase<Workorder_Get, WorkOrder_GetResult>)(object)val3).TxnErrorMsg);
			}
			val = ((TransactionBase<Workorder_Get, WorkOrder_GetResult>)(object)val3).Result;
			if (val._ItemList.Count == 0)
			{
				throw new Exception("Unable to obtain WO information," + WoNo);
			}
			m_PingServrer();
			WO_Block_Save val4 = new WO_Block_Save();
			WO_Block val5 = new WO_Block();
			val5.WONo = val._ItemList[0].WONo;
			val5.WorkCenterNo = val._ItemList[0].CurrentWorkCenter;
			val5.BlockCode = BlockCode;
			val5.BlockReason = BlockReason;
			val5.Remark = "";
			val5.Enable = true;
			val5.CreateUser = "SYSTEM";
			val5.ChangeUser = m_UserID;
			val4._ItemList.Add(val5);
			((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val4).clientExecute(m_txnConn);
			if (((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val4).IsTxnError)
			{
				throw new Exception(((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val4).TxnErrorMsg);
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo1,Exception,<{l_onMessageArrive}>,{ex.Message}");
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo1,End,<{l_onMessageArrive}>,Spend(s),{val2.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_BlockWo(WorkOrder wo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo2,Start,<{l_onMessageArrive}>");
			m_PingServrer();
			WO_Block_Save val2 = new WO_Block_Save();
			WO_Block val3 = new WO_Block();
			val3.WONo = wo.WONo;
			val3.WorkCenterNo = wo.CurrentWorkCenter;
			val3.BlockCode = "A6";
			if (m_ReelCheckMsg.Length > 500)
			{
				val3.BlockReason = m_ReelCheckMsg.Substring(0, 500);
			}
			else
			{
				val3.BlockReason = m_ReelCheckMsg;
			}
			val3.Remark = "";
			val3.Enable = true;
			val3.CreateUser = "SYSTEM";
			val3.ChangeUser = m_UserID;
			val2._ItemList.Add(val3);
			((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val2).IsTxnError)
			{
				throw new Exception(((TransactionBase<WO_Block_Save, WO_Block_SaveResult>)(object)val2).TxnErrorMsg);
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo2,Exception,<{l_onMessageArrive}>,{ex.Message}");
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_BlockWo2,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_LogReelInfo(WO_ReelID reelID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		string arg = "";
		try
		{
			if (m_woReelIdInfo == null)
			{
				arg = "reelID是null";
				return;
			}
			if (m_woReelIdInfo._ItemList.Count == 0)
			{
				arg = "m_woReelIdInfo 0 筆未上機";
				return;
			}
			IEnumerable<WO_ReelID> source = m_woReelIdInfo._ItemList.Where((WO_ReelID o) => o.ReelID == reelID.ReelID && string.IsNullOrWhiteSpace(o.TPMachNo));
			if (source.Count() == 1)
			{
				arg = "m_woReelIdInfo TPMachNo 空值(未上機)";
				return;
			}
			source = m_woReelIdInfo._ItemList.OrderBy((WO_ReelID o) => o.TPStartTime);
			if (source.ElementAt(source.Count() - 1).ReelID.ToUpper().Trim() == reelID.ReelID.ToUpper().Trim())
			{
				arg = "m_woReelIdInfo 上機 最後一筆";
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("m_woReelIdInfo, Loading").Append(Environment.NewLine);
			stringBuilder.Append("DB, WO_ReelID Record,").Append(Environment.NewLine);
			stringBuilder.Append("ReelID=").Append(reelID.ReelID).Append(Environment.NewLine);
			stringBuilder.Append("Status").Append(reelID.Status).Append(Environment.NewLine);
			stringBuilder.Append("TPMachNo=").Append(reelID.TPMachNo).Append(Environment.NewLine);
			stringBuilder.Append("TPStartTime=").Append(cyntecDateTime.ToMSSQL(reelID.TPStartTime)).Append(Environment.NewLine);
			stringBuilder.Append("TPEndTime=").Append(cyntecDateTime.ToMSSQL(reelID.TPEndTime)).Append(Environment.NewLine);
			stringBuilder.Append("TPStatus=").Append(reelID.TPStatus).Append(Environment.NewLine);
			stringBuilder.Append("UpdateDate=").Append(cyntecDateTime.ToMSSQL(reelID.UpdateDate)).Append(Environment.NewLine);
			stringBuilder.Append("UpdateUser=").Append(reelID.UserID).Append(Environment.NewLine);
			source = m_woReelIdInfo._ItemList.Where((WO_ReelID o) => o.ReelID.ToUpper().Trim() == reelID.ReelID.ToUpper().Trim());
			if (source.Count() == 0)
			{
				stringBuilder.Append("The PC does not have this Reel record").Append(Environment.NewLine);
			}
			else
			{
				stringBuilder.Append("Records uploaded to the PC").Append(Environment.NewLine);
				stringBuilder.Append("ReelID=").Append(source.ElementAt(0).ReelID).Append(Environment.NewLine);
				stringBuilder.Append("Status").Append(source.ElementAt(0).Status).Append(Environment.NewLine);
				stringBuilder.Append("TPMachNo=").Append(source.ElementAt(0).TPMachNo).Append(Environment.NewLine);
				stringBuilder.Append("TPStartTime=").Append(cyntecDateTime.ToMSSQL(source.ElementAt(0).TPStartTime)).Append(Environment.NewLine);
				stringBuilder.Append("TPEndTime=").Append(cyntecDateTime.ToMSSQL(source.ElementAt(0).TPEndTime)).Append(Environment.NewLine);
				stringBuilder.Append("TPStatus=").Append(source.ElementAt(0).TPStatus).Append(Environment.NewLine);
				stringBuilder.Append("UpdateDate=").Append(cyntecDateTime.ToMSSQL(source.ElementAt(0).UpdateDate)).Append(Environment.NewLine);
				stringBuilder.Append("UpdateUser=").Append(source.ElementAt(0).UserID).Append(Environment.NewLine);
			}
			m_Log.LogDebug($"m_ReelCheckInput_f_LogReelInfo,Process,<{l_onMessageArrive}>,{stringBuilder.ToString()}");
			f_TapingUILog((EQP_TapingUILogType)99, (EQP_TapingUILogAction)2, stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_LogReelInfo,Exception,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_LogReelInfo,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds},{arg}");
		}
	}

	private void f_BlockWo()
	{
	}

	private void f_TapingUILog(EQP_TapingUILogType xLogType, EQP_TapingUILogAction xAction, string xRemark)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog1,Start,<{l_onMessageArrive}>");
			m_PingServrer();
			EQP_TapingUILog_Save val2 = new EQP_TapingUILog_Save();
			val2._TapingLog = new EQP_TapingUILog();
			val2._TapingLog.WoNo = m_WoNo;
			val2._TapingLog.WorkcenterNo = m_WorkcenterNo;
			val2._TapingLog.UIName = PropertyExtension.GetAssemblyName((object)this);
			val2._TapingLog.UIVersion = PropertyExtension.GetAssemblyVersion((object)this);
			val2._TapingLog.LogType = xLogType;
			val2._TapingLog.ComputerName = Environment.MachineName;
			val2._TapingLog.CreateUser = m_UserID;
			val2._TapingLog.EqpNo = m_machNo;
			val2._TapingLog.Action = xAction;
			val2._TapingLog.Remark = xRemark;
			val2._TapingLog.CreateUser = m_UserID;
			((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).IsTxnError)
			{
				string arg = "Transaction Exception, " + ((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).TxnErrorMsg;
				m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog1,Exception 1,<{l_onMessageArrive}>,spend (s),{arg}");
			}
			else
			{
				string arg2 = "Transaction Log, " + ((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).Result._TxnID + ", " + val2._TapingLog.Remark;
				m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog1,process,<{l_onMessageArrive}>,{arg2}");
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog1,Exception 2,<{l_onMessageArrive}>,{ex.Message}");
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog1,Start,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_TapingUILog(EQP_TapingUILogType xLogType, EQP_TapingUILogAction xAction, myReel oReel, params object[] remarks)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		cyntecDateTime val = new cyntecDateTime();
		if (!string.IsNullOrWhiteSpace(oReel.TapingUILogTxnID))
		{
			return;
		}
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog2,Start,<{l_onMessageArrive}>");
			m_PingServrer();
			EQP_TapingUILog_Save val2 = new EQP_TapingUILog_Save();
			val2._TapingLog = new EQP_TapingUILog();
			val2._TapingLog.WoNo = m_WoNo;
			val2._TapingLog.WorkcenterNo = m_WorkcenterNo;
			val2._TapingLog.UIName = PropertyExtension.GetAssemblyName((object)this);
			val2._TapingLog.UIVersion = PropertyExtension.GetAssemblyVersion((object)this);
			val2._TapingLog.LogType = xLogType;
			val2._TapingLog.ComputerName = Environment.MachineName;
			val2._TapingLog.CreateUser = m_UserID;
			val2._TapingLog.EqpNo = m_machNo;
			val2._TapingLog.Action = xAction;
			val2._TapingLog.Remark = oReel.ToRemark();
			if (remarks != null)
			{
				foreach (object obj in remarks)
				{
					EQP_TapingUILog tapingLog = val2._TapingLog;
					tapingLog.Remark = tapingLog.Remark + " ," + obj.ToString();
				}
			}
			val2._TapingLog.TxnID = oReel.TapingUILogTxnID;
			val2._TapingLog.CreateUser = m_UserID;
			((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).IsTxnError)
			{
				string arg = "Transaction Exception, " + ((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).TxnErrorMsg;
				m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog2,Exception 1,<{l_onMessageArrive}>,{arg}");
			}
			else
			{
				string text = "Transaction Log, " + ((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).Result._TxnID + ", " + val2._TapingLog.Remark;
				oReel.TapingUILogTxnID = ((TransactionBase<EQP_TapingUILog_Save, EQP_TapingUILog_SaveResult>)(object)val2).Result._TxnID;
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog2,Exception 2,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_TapingUILog2,Start,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_WOReelIDLogException(myReel oMyReel, EQP_OP_OperationEnum op, string instrustion, string LastExceptionMsg)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (oMyReel == null)
		{
			return;
		}
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_WOReelIDLogException,Start,<{l_onMessageArrive}>");
			m_PingServrer();
			WO_ReelIDLog_Add val2 = new WO_ReelIDLog_Add();
			val2._Condition.Action = "00026";
			val2._Condition.BatchNo = m_TapingNo;
			val2._Condition.Big_ReelID = oMyReel.Reel.ReelID;
			val2._Condition.ConfirmID = m_WoInfo._WOConfirm._CurrentConfirm.ID;
			val2._Condition.CreateDate = DateTime.Now;
			val2._Condition.CreateUser = m_UserID;
			val2._Condition.DNDOC = "";
			val2._Condition.DNITEM = "";
			val2._Condition.IsSAPLog = false;
			val2._Condition.MatNo = oMyReel.Reel.MatNo;
			val2._Condition.MMDOC = "";
			val2._Condition.MMITEM = "";
			val2._Condition.Parent_ReelID = "";
			val2._Condition.ReelID = oMyReel.Reel.ReelID;
			val2._Condition.SBatchNo = oMyReel.Reel.BatchNo;
			val2._Condition.SMatNo = oMyReel.Reel.MatNo;
			val2._Condition.Status = "11";
			val2._Condition.UserID = m_UserID;
			val2._Condition.WONO = m_WoNo;
			((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val2).IsTxnError)
			{
				LogReel(oMyReel, "API_WO_ReelIDLog_Add_Exception_Error", Convert.ToBase64String(((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val2).ToByte()), ((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val2).TxnErrorMsg);
				throw new Exception(((TransactionBase<WO_ReelIDLog_Add, WO_ReelIDLog_AddResult>)(object)val2).TxnErrorMsg);
			}
			LogReel(oMyReel, "API_WO_ReelIDLog_Add_Exception", oMyReel.Rawdata, "");
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_WOReelIDLogException,Exception,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_WOReelIDLogException,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private unsafe void f_EqpOpRecordSave(EQP_OP_OperationEnum op, string MatNo, string instrustion, List<ParameterItem> paramList, bool IsException, string LastExceptionMsg)
	{
		f_EqpOpRecordSave(((object)(*(EQP_OP_OperationEnum*)(&op))/*cast due to .constrained prefix*/).ToString(), MatNo, instrustion, paramList, IsException, LastExceptionMsg, "", "");
	}

	private unsafe void f_EqpOpRecordSave(EQP_OP_OperationEnum op, string MatNo, string instrustion, List<ParameterItem> paramList, bool IsException, string LastExceptionMsg, string ExceptionWoNo, string ExceptionType)
	{
		f_EqpOpRecordSave(((object)(*(EQP_OP_OperationEnum*)(&op))/*cast due to .constrained prefix*/).ToString(), MatNo, instrustion, paramList, IsException, LastExceptionMsg, ExceptionWoNo, ExceptionType);
	}

	private void f_EqpOpRecordSave(string op, string MatNo, string instrustion, List<ParameterItem> paramList, bool IsException, string LastExceptionMsg, string ExceptionWoNo, string ExceptionType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			if (m_MachInfo == null)
			{
				throw new Exception("EQP information not obtained");
			}
			m_Log.LogDebug($"m_ReelCheckInput_f_EqpOpRecordSave1,Start,<{l_onMessageArrive}>");
			EQP_OP_Record val2 = new EQP_OP_Record();
			val2.ConfirmID = m_WoInfo._WOConfirm._CurrentConfirm.ID;
			if (string.IsNullOrWhiteSpace(m_UserID))
			{
				val2.CreateUser = "sys";
				string text = m_Culture.ToMSg("MSG043", new object[1] { instrustion });
				m_Log.LogError(text);
				f_TapingUILog((EQP_TapingUILogType)98, (EQP_TapingUILogAction)2, text);
			}
			else
			{
				val2.CreateUser = m_UserID;
			}
			val2.ValidDate = DateTime.MaxValue;
			val2.FlowID = m_WoInfo._WOConfirm._CurrentConfirm.FlowID;
			val2.Instruction = instrustion;
			val2.MachID = m_MachInfo._ItemList[0].MachID;
			val2.MatGroupNo = m_WoInfo._WO._ItemList[0].MatGroupNo;
			val2.Operation = op;
			val2.MatNo = MatNo;
			val2.WoNo = m_WoInfo._WO._ItemList[0].WONo;
			val2.WorkcenterNo = m_WoInfo._WO._ItemList[0].CurrentWorkCenter;
			val2.IsException = IsException;
			val2.ExceptionWoNo = ExceptionWoNo;
			val2.ExceptionType = ExceptionType;
			if (LastExceptionMsg.Length > 500)
			{
				val2.LastExceptionMsg = LastExceptionMsg.Substring(0, 500);
			}
			else
			{
				val2.LastExceptionMsg = LastExceptionMsg;
			}
			if (m_LastInputBarcode != null)
			{
				val2.ReelID = m_LastInputBarcode.Reel.ReelID;
			}
			if (paramList != null)
			{
				val2.paramList.AddRange(paramList);
			}
			m_PingServrer();
			EQP_OP_Record_Save val3 = new EQP_OP_Record_Save();
			val3._EqpOpRecordList.Add(val2);
			((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val3).clientExecute(m_txnConn);
			if (((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val3).IsTxnError)
			{
				if (IsException && m_LastInputBarcode != null && m_LastInputBarcode.Reel != null)
				{
					LogReel(m_LastInputBarcode, "API_EQP_OP_Record_Save_Error", instrustion, Convert.ToBase64String(((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val3).ToByte()));
				}
				throw new Exception(((TransactionBase<EQP_OP_Record_Save, EQP_OP_Record_SaveResult>)(object)val3).TxnErrorMsg);
			}
			if (IsException && m_LastInputBarcode != null && m_LastInputBarcode.Reel != null)
			{
				LogReel(m_LastInputBarcode, "API_EQP_OP_Record_Save", instrustion, "");
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_EqpOpRecordSave1,Exception,<{l_onMessageArrive}>,{ex.Message}");
			throw new Exception("EqpOpRecordSave Exception, " + ex.Message);
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_EqpOpRecordSave1,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_ReelUpdate(myReel oMyReel)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelUpdate,Start,<{l_onMessageArrive}>");
			oMyReel.IsAnyUpdate = false;
			oMyReel.ReelUpdateTime = DateTime.Now;
			m_PingServrer();
			WO_ReelID_Update val2 = new WO_ReelID_Update();
			val2._reelID = oMyReel.Reel.ReelID;
			val2._UpdateUser = m_UserID;
			val2._statusCode = "11";
			val2._startTime = oMyReel.Reel.TPStartTime;
			val2._endTime = oMyReel.Reel.TPEndTime;
			val2._machNo = m_machNo;
			((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).IsTxnError)
			{
				LogReel(oMyReel, "API_WO_ReelID_Update_EXCEPTION", oMyReel.Rawdata, "TXN_ERROR," + ((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).msgTxnID);
				m_Log.LogDebug(string.Format("m_ReelCheckInput_f_ReelUpdate,Exception 1,<{0}>,{1}", l_onMessageArrive, "API_WO_ReelID_Update_EXCEPTION," + ((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).TxnErrorMsg));
			}
			else if (!string.IsNullOrWhiteSpace(((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).TxnErrorMsg))
			{
				LogReel(oMyReel, "API_WO_ReelID_Update_TIMEOUT", oMyReel.Rawdata, ((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).TxnErrorMsg);
				m_Log.LogDebug(string.Format("m_ReelCheckInput_f_ReelUpdate,Exception 2,<{0}>,{1}", l_onMessageArrive, "API_WO_ReelID_Update_TIMEOUT"));
			}
			else
			{
				LogReel(oMyReel, "API_WO_ReelID_Update", oMyReel.Rawdata, "");
				m_Log.LogDebug(string.Format("m_ReelCheckInput_f_ReelUpdate,Process,<{0}>,{1}", l_onMessageArrive, "API_WO_ReelID_Update, " + oMyReel.Rawdata));
			}
			if (oMyReel.IsWoFinish)
			{
				f_TapingUILog((EQP_TapingUILogType)1, (EQP_TapingUILogAction)8, oMyReel, "WO Finish");
			}
			else
			{
				f_TapingUILog((EQP_TapingUILogType)1, (EQP_TapingUILogAction)8, oMyReel);
			}
			m_PingServrer();
			WO_ReelCheck_Save val3 = new WO_ReelCheck_Save();
			WO_ReelCheck val4 = new WO_ReelCheck();
			val4.ComputerName = Environment.MachineName;
			val4.EqpNo = m_machNo;
			val4.MatNo = m_MatNo;
			val4.MatNoCount = oMyReel.Reel.MatNoCount;
			val4.ReelID = oMyReel.Reel.ReelID;
			val4.ReelIDCount = oMyReel.Reel.ReelIDCount;
			val4.StartTime = oMyReel.Reel.TPStartTime;
			val4.TPNO = m_TapingNo;
			val4.TPNoCount = oMyReel.Reel.TPNoCount;
			val4.UpdateTime = DateTime.Now;
			val4.WoNo = m_WoNo;
			val4.TapingUILogTxnID = oMyReel.TapingUILogTxnID;
			val3._WoReelCheckList.Add(val4);
			((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val3).clientExecute(m_txnConn);
			if (((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val3).IsTxnError)
			{
				LogReel(oMyReel, "API_WO_ReelCheck_Save_EXCEPTION", oMyReel.Rawdata, "TXN_ERROR," + ((TransactionBase<WO_ReelID_Update, WO_ReelID_UpdateResult>)(object)val2).msgTxnID);
				m_Log.LogDebug(string.Format("m_ReelCheckInput_f_ReelUpdate,Exception 3,<{0}>,{1}", l_onMessageArrive, "API_WO_ReelCheck_Save_EXCEPTION," + ((TransactionBase<WO_ReelCheck_Save, WO_ReelCheck_SaveeResult>)(object)val3).TxnErrorMsg));
			}
			else
			{
				LogReel(oMyReel, "API_WO_ReelCheck_Save", oMyReel.Rawdata, "");
				m_Log.LogDebug(string.Format("m_ReelCheckInput_f_ReelUpdate,Process,<{0}>,{1}", l_onMessageArrive, "API_WO_ReelCheck_Save," + oMyReel.Rawdata));
			}
			if (oMyReel.TapingUILogTime == DateTime.MaxValue)
			{
				LogSummaryReel(oMyReel);
				LogReel(oMyReel, "Reel_END(" + oMyReel.Reel.ReelID + ")", oMyReel.Rawdata, "");
			}
			if (oMyReel.IsNewReel)
			{
				oMyReel.IsNewReel = false;
				LogReel(oMyReel, "Reel_CHANGE(" + oMyReel.Reel.ReelID + ")", oMyReel.Rawdata, "");
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelUpdate,Exception 4,<{l_onMessageArrive}>,{ex.Message}");
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_ReelUpdate,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void m_PingServrer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			if (m_LogFunction == null || m_txnConn == null)
			{
				return;
			}
			m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,Start,<{l_onMessageArrive}>");
			List<string> list = new List<string>();
			list.Add(m_txnConn.ServerUrl);
			if (m_txnConn.ServerUrlList != null)
			{
				list.AddRange(m_txnConn.ServerUrlList);
			}
			Ping ping = new Ping();
			PingOptions pingOptions = new PingOptions();
			pingOptions.DontFragment = true;
			string s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			int timeout = 120;
			string text = "Step-0";
			foreach (string item in list)
			{
				try
				{
					if (!item.ToUpper().StartsWith("HTTP"))
					{
						text = "Step-1";
						string[] array = item.Split(new string[1] { "//" }, StringSplitOptions.None);
						text = "Step-2";
						if (array.Length < 2)
						{
							break;
						}
						text = "Step-3";
						array = array[1].Split(new string[1] { ":" }, StringSplitOptions.None);
						text = "Step-4";
						string text2 = array[0];
						text = "Step-5";
						DateTime now = DateTime.Now;
						PingReply pingReply = ping.Send(text2, timeout, bytes, pingOptions);
						text = "Step-6";
						if (pingReply != null)
						{
							m_PingInfoUpdate();
							string arg = "Ping Server," + text2 + "," + pingReply.RoundtripTime;
							m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,Process,<{l_onMessageArrive}>,{arg}");
						}
						else
						{
							f_PingErrorLog();
							string arg2 = "Ping Server Exception (return null)," + text2 + "," + (DateTime.Now - now).TotalMilliseconds;
							m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,Exception 1,<{l_onMessageArrive}>,{arg2}");
						}
						text = "Step-7";
					}
				}
				catch (Exception ex)
				{
					f_PingErrorLog();
					string arg3 = text + "," + item + ", " + ex.Message;
					m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,Exception 2,<{l_onMessageArrive}>,{arg3}");
				}
			}
		}
		catch (Exception ex2)
		{
			f_PingErrorLog();
			m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,Exception 3,<{l_onMessageArrive}>,{ex2.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_m_PingServrer,End,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void m_ReelUpdate_onProcess(object sender)
	{
		bool flag = false;
		try
		{
			if ((DateTime.Now - TimeReelUpdate_onProcess).TotalSeconds > 10.0)
			{
				flag = true;
				TimeReelUpdate_onProcess = DateTime.Now;
			}
			if (ReelUpdate_onProcessCount < ulong.MaxValue)
			{
				ReelUpdate_onProcessCount++;
			}
			else
			{
				ReelUpdate_onProcessCount = 1uL;
			}
			cyntecThread val = (cyntecThread)((sender is cyntecThread) ? sender : null);
			if (string.IsNullOrWhiteSpace(val.Description))
			{
				val.Description = cyntecDateTime.NowToMSSQL();
			}
			if (flag)
			{
				m_Log.LogDebug($"m_ReelUpdate_onProcess Execute,<{m_BarcodeQueue.Count}>,count,{ReelUpdate_onProcessCount},Name,{val.Name},Create Time,{val.Description}");
			}
			if (m_BarcodeQueue.Count <= 0)
			{
				return;
			}
			if (m_txnConn.IsExecuting)
			{
				if (flag)
				{
					TimeReelUpdate_onProcess = DateTime.Now;
					m_Log.LogDebug($"WCF Txn is being executed, ReelUpdate_onProcess is skipping this time, <{l_onMessageArrive}>, number of caches, {m_BarcodeQueue.Count}");
				}
			}
			else
			{
				BarcodeRecord barcodeRecord = m_BarcodeQueue.Dequeue();
				if (barcodeRecord != null)
				{
					m_ReelCheckInput_onMessageArrive(barcodeRecord);
				}
			}
		}
		catch (Exception ex)
		{
			try
			{
				m_Log.LogError("m_ReelUpdate_onProcess Exception," + ex.Message);
			}
			catch
			{
			}
		}
		finally
		{
			if (flag)
			{
				m_Log.LogDebug($"m_ReelUpdate_onProcess End,<{m_BarcodeQueue.Count}>,Count,{ReelUpdate_onProcessCount}");
			}
		}
	}

	private void m_ReelUpdate_onError(string errorMsg)
	{
		m_Log.LogDebug($"m_ReelCheckInput_m_ReelUpdate_onError,Exception,<{l_onMessageArrive}>,{errorMsg}");
	}

	public bool IsNumAndEnCh(string input)
	{
		string pattern = "^[0-9]+$";
		Regex regex = new Regex(pattern);
		return regex.IsMatch(input);
	}

	public bool IsContainsLetter(string input)
	{
		if (Regex.Matches(input, "[a-zA-Z]").Count > 0)
		{
			return true;
		}
		return false;
	}

	public bool IsContainsLetterAndNum(string str)
	{
		if (Regex.IsMatch(str, "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9a-zA-Z]+$"))
		{
			return true;
		}
		return false;
	}

	public bool IsMahchRele(string str)
	{
		bool result = true;
		foreach (char c in str)
		{
			if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '-')
			{
				result = false;
			}
		}
		return result;
	}

	public void LogSummaryBegin()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryBegin,開始,<{l_onMessageArrive}>,");
			if (!cyntecFile.IsDirExist(LogPath))
			{
				cyntecFile.CreateDir(LogPath);
			}
			string text = Path.Combine(LogPath, FileNameSummary);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("工單號碼,").Append(m_WoNo).Append(Environment.NewLine);
			stringBuilder.Append("電腦名稱,").Append(Environment.MachineName).Append(Environment.NewLine);
			stringBuilder.Append("設備編號,").Append(m_machNo).Append(Environment.NewLine);
			stringBuilder.Append("料號,").Append(m_MatNo).Append(Environment.NewLine);
			stringBuilder.Append("TPNO,").Append(m_TapingNo).Append(Environment.NewLine);
			stringBuilder.Append("刷工單時間,").Append(cyntecDateTime.ToMSSQL(m_WoStartDate)).Append(Environment.NewLine);
			stringBuilder.Append("#參數,#paramCode,#RefFieldCode,#ParamName,#中心,#上限,#下限").Append(Environment.NewLine);
			foreach (ParameterItem item in m_Parameter._itemList)
			{
				stringBuilder.Append("參數,").Append(item.ParamCode).Append(",")
					.Append(item.RefFieldCode)
					.Append(",")
					.Append(item.ParamName)
					.Append(",")
					.Append(item.ParamValue)
					.Append(",")
					.Append(item.UpperLimit)
					.Append(",")
					.Append(item.LowerLimit)
					.Append(Environment.NewLine);
			}
			stringBuilder.Append(">>> 產品特別屬性 <<<" + m_WoInfo._WO._ItemList[0].MatGroupNo).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property1\t剩料一次補完 ," + m_WoInfo._MatGroupNo._ItemList[0].Property1).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property2\t剩料允許卷數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property2).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property3\t剩料有效天數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property3).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property4\t剩料需紀錄儲位 ," + m_WoInfo._MatGroupNo._ItemList[0].Property4).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property5\t補料需顯示粉種 ," + m_WoInfo._MatGroupNo._ItemList[0].Property5).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property6\t補料允許替代料 ," + m_WoInfo._MatGroupNo._ItemList[0].Property6).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property7\t補料允許誤差% ," + m_WoInfo._MatGroupNo._ItemList[0].Property7).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property8\t補料允許卷數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property8).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property9\t補料允許卷數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property9).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property10\t檢查T/P No ," + m_WoInfo._MatGroupNo._ItemList[0].Property10).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property11\tT/P No刷二維碼 ," + m_WoInfo._MatGroupNo._ItemList[0].Property11).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property12\t補料刷二維碼檢查有效期 ," + m_WoInfo._MatGroupNo._ItemList[0].Property12).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property13\t補料允許批數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property13).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property14\t副產品補料允許批數 ," + m_WoInfo._MatGroupNo._ItemList[0].Property14).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property15\t使用補剩料工具 ," + m_WoInfo._MatGroupNo._ItemList[0].Property15).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property16\tT/C有剩料 ," + m_WoInfo._MatGroupNo._ItemList[0].Property16).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property17\tTP成卷產出 ," + m_WoInfo._MatGroupNo._ItemList[0].Property17).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property18\t預留 ," + m_WoInfo._MatGroupNo._ItemList[0].Property18).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property19\t預留 ," + m_WoInfo._MatGroupNo._ItemList[0].Property19).Append(Environment.NewLine);
			stringBuilder.Append("MatGroupNo Property20\t預留 ," + m_WoInfo._MatGroupNo._ItemList[0].Property20).Append(Environment.NewLine);
			stringBuilder.Append(">>> 料號特別屬性 <<<" + m_WoInfo._WO._ItemList[0].MatNo).Append(Environment.NewLine);
			stringBuilder.Append("MatView ReelTapeMinTime\tReel最少作業時間(分) ," + m_WoInfo._MatView._itemList[0].ReelTapeMinTime).Append(Environment.NewLine);
			stringBuilder.Append(">>> 設備檢查項目 <<<" + m_machNo).Append(Environment.NewLine);
			foreach (EQP_OP_CheckList item2 in m_EqpOpCheckInfo._ItemList)
			{
				stringBuilder.Append(item2.Operation + ", " + item2.OperationDesc).Append(Environment.NewLine);
			}
			f_InitFileLogSummary();
			m_FileLogSummary.Write(stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryBegin,異常,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryBegin,結束,<{l_onMessageArrive}>,花費(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	public void LogSummaryReel(myReel oMyReel)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (oMyReel == null)
		{
			return;
		}
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryReel,開始,<{l_onMessageArrive}>");
			if (!cyntecFile.IsDirExist(LogPath))
			{
				cyntecFile.CreateDir(LogPath);
			}
			string text = Path.Combine(LogPath, FileNameSummary);
			StringBuilder stringBuilder = new StringBuilder();
			if (oMyReel.ReelIDCount > 0 && oMyReel.MatNoCount > 0 && oMyReel.TPNoCount > 0)
			{
				stringBuilder.Append("Reel標籤檢查OK,");
			}
			else
			{
				stringBuilder.Append("Reel標籤檢查NG,");
			}
			stringBuilder.Append(oMyReel.TapingUILogTxnID).Append(",").Append(oMyReel.ToRemark())
				.Append(Environment.NewLine);
			f_InitFileLogSummary();
			m_FileLogSummary.Write(stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryReel,異常,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogSummaryReel,結束,<{l_onMessageArrive}>,花費(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	public void LogReel(myReel oMyReel, string logType, string barcode, string remark)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogReel,開始,<{l_onMessageArrive}>");
			string text = Path.Combine(LogPath, FileNameReel);
			if (!cyntecFile.IsDirExist(LogPath))
			{
				cyntecFile.CreateDir(LogPath);
			}
			StringBuilder stringBuilder = null;
			if (!cyntecFile.IsFileExist(text))
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append("#當前ReelID").Append(",").Append("#紀錄時間")
					.Append(",")
					.Append("#Log類型")
					.Append(",")
					.Append("#條碼內容")
					.Append(",")
					.Append("#距離ReelID時間(秒)")
					.Append(",")
					.Append("#ReelID累計")
					.Append(",")
					.Append("#料號累計")
					.Append(",")
					.Append("#TPNO累計")
					.Append(",")
					.Append("#是否完成")
					.Append(",")
					.Append("#LogTxnID")
					.Append(",")
					.Append("#備註")
					.Append(",")
					.Append("%新版本%")
					.Append(Environment.NewLine);
				m_FileLogReel.Write(stringBuilder.ToString());
			}
			if (oMyReel != null)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append(oMyReel.Reel.ReelID).Append(",").Append(cyntecDateTime.NowToMSSQL())
					.Append(",")
					.Append(logType)
					.Append(",")
					.Append(barcode)
					.Append(",")
					.Append((DateTime.Now - oMyReel.ReelIDScanTime).TotalSeconds)
					.Append(",")
					.Append(oMyReel.ReelIDCount)
					.Append(",")
					.Append(oMyReel.MatNoCount)
					.Append(",")
					.Append(oMyReel.TPNoCount)
					.Append(",")
					.Append(cyntecByteConvert.ToFlag(oMyReel.IsFinish))
					.Append(",")
					.Append(oMyReel.TapingUILogTxnID)
					.Append(",")
					.Append(remark)
					.Append(Environment.NewLine);
				m_FileLogReel.Write(stringBuilder.ToString());
			}
			else
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append(barcode).Append(",").Append(cyntecDateTime.NowToMSSQL())
					.Append(",")
					.Append(logType)
					.Append(",")
					.Append(barcode)
					.Append(",")
					.Append(",")
					.Append(",")
					.Append(",")
					.Append(",")
					.Append(",")
					.Append(",")
					.Append(remark)
					.Append(Environment.NewLine);
				m_FileLogReel.Write(stringBuilder.ToString());
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_LogReel,結束,<{l_onMessageArrive}>,花費(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	public void StreamWrite(string strFilePath, string remark, Encoding oEncoding)
	{
		FileStream fileStream = null;
		fileStream = ((!cyntecFile.IsFileExist(strFilePath)) ? new FileStream(strFilePath, FileMode.Create, FileAccess.Write, FileShare.None) : new FileStream(strFilePath, FileMode.Append, FileAccess.Write, FileShare.None));
		StreamWriter streamWriter = new StreamWriter(fileStream, oEncoding);
		streamWriter.Write(remark);
		streamWriter.Flush();
		streamWriter.Close();
	}

	private bool Wo_ExceptionLog(string strLogType, string strReelID, string strReelIDEncode, string strErrCode, string strBarCode, string strRemark)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_Wo_ExceptionLog,Exception,<{l_onMessageArrive}>,{ex.Message}");
			return false;
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_Wo_ExceptionLog,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
		return true;
	}

	private void m_PingInfoUpdate()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			if (m_PingErrorDT.Rows.Count == 0)
			{
				return;
			}
			DateTime dateTime = cyntecDateTime.ToDateTime(m_PingErrorDT.Rows[0]["時間"]);
			if ((DateTime.Now - dateTime).TotalMinutes < 10.0)
			{
				return;
			}
		}
		catch
		{
		}
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_m_PingInfoUpdate,Start,<{l_onMessageArrive}>");
			int num = 1;
			DateTime dateTime2 = cyntecDateTime.ToDateTime(m_PingErrorDT.Rows[0]["時間"]);
			DateTime dateTime3 = dateTime2;
			if (m_PingErrorDT.Rows.Count > 1)
			{
				dateTime3 = cyntecDateTime.ToDateTime(m_PingErrorDT.Rows[1]["時間"]);
				num = int.Parse(m_PingErrorDT.Rows[1]["次數"].ToString());
			}
			string strReelID = "";
			if (m_LastInputBarcode != null)
			{
				strReelID = m_LastInputBarcode.Reel.ReelID;
			}
			if (Wo_ExceptionLog("Ping", strReelID, "", "", "", "Exception Start Time:" + cyntecDateTime.ToMSSQL(dateTime2) + ",Exception End Time:" + cyntecDateTime.ToMSSQL(dateTime3) + "Exception duration," + (dateTime3 - dateTime2).TotalMinutes + "minutes,Cumulative number of exceptions:" + num))
			{
				m_PingErrorDT.Rows.Clear();
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_m_PingInfoUpdate,Exception,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_m_PingInfoUpdate,End,<{l_onMessageArrive}>,Spend(s),{val.ToTimeSpan().TotalSeconds}");
		}
	}

	private void f_PingErrorLog()
	{
		if (m_PingErrorDT != null)
		{
			if (m_PingErrorDT.Rows.Count == 0)
			{
				DataRow dataRow = m_PingErrorDT.NewRow();
				dataRow["時間"] = cyntecDateTime.NowToMSSQL();
				dataRow["次數"] = 1;
				m_PingErrorDT.Rows.Add(dataRow);
			}
			else if (m_PingErrorDT.Rows.Count == 1)
			{
				DataRow dataRow2 = m_PingErrorDT.NewRow();
				dataRow2["時間"] = cyntecDateTime.NowToMSSQL();
				dataRow2["次數"] = 2;
				m_PingErrorDT.Rows.Add(dataRow2);
			}
			else if (m_PingErrorDT.Rows.Count >= 2)
			{
				int num = int.Parse(m_PingErrorDT.Rows[1]["次數"].ToString()) + 1;
				m_PingErrorDT.Rows[1]["時間"] = cyntecDateTime.NowToMSSQL();
				m_PingErrorDT.Rows[1]["次數"] = num;
			}
		}
	}

	private string f_IsWOStatusOK()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		cyntecDateTime val = new cyntecDateTime();
		try
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_IsWOStatusOK,Start,<{l_onMessageArrive}>");
			string text = m_WoInfo._WOrouting.GetDesignatedStep("TP").WorkCenterNo.ToString();
			WOConfirm_Get val2 = new WOConfirm_Get();
			val2._WoNo = m_WoNo;
			((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).clientExecute(m_txnConn);
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).IsTxnError)
			{
				return ((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).TxnErrorMsg;
			}
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).Result.GetDesignatedStep(text) == null)
			{
				m_ReelCheckErrorType = m_Culture.ToMSg("MSG045", new object[0]);
				m_Is_BarcodeCheckOK = false;
				m_ReelCheckMsg = m_Culture.ToMSg("MSG021", new object[0]);
				f_BlockWo();
				throw new Exception(m_ReelCheckMsg);
			}
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).Result.GetDesignatedStep(text).IsInTimeNG(m_WoInfo._WOConfirm.GetDesignatedStep(text).InTime))
			{
				return m_Culture.ToMSg("MSG022", new object[2]
				{
					cyntecDateTime.ToMSSQL(m_WoInfo._WOConfirm.GetDesignatedStep(text).InTime),
					cyntecDateTime.ToMSSQL(((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).Result.GetDesignatedStep(text).InTime)
				});
			}
			if (((TransactionBase<WOConfirm_Get, WOConfirm_GetResult>)(object)val2).Result.GetDesignatedStep(text).Finished == "Y")
			{
				return m_Culture.ToMSg("MSG009", new object[0]);
			}
		}
		catch (Exception ex)
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_IsWOStatusOK,Exception,<{l_onMessageArrive}>,{ex.Message}");
		}
		finally
		{
			m_Log.LogDebug($"m_ReelCheckInput_f_IsWOStatusOK,End,<{l_onMessageArrive}>,Spend (s),{val.ToTimeSpan().TotalSeconds}");
		}
		return "OK";
	}
}
