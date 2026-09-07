
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using cyntec.Base;
using cyntec.MES;
using cyntec.Tools;
using cyntec.WCF;

namespace cyntec.Windows
{
    public partial class frmReelCheck : Form
    {
        // #1
        private cyntecLog m_Log = null;             //  log
        private cyntecXML m_Config = null;          //  xml設定檔
        private wcfServiceConnection m_txnConn;     //  API連線

        // #2 標籤防混物件
        private cyntec.WinMES.ReelCheck.ReelBarcodeCheck m_ReelBarcodeCheck = null;     // 標籤檢查物件

        // #2 API
        private WoInfo_GetResult m_WoInfo = null;                   // API
        private GetParameterResult m_Parameter = null;              // API
        private WO_ReelStatus_GetResult m_WoReelStatusInfo = null;  // API
        public WO_ReelID_GetResult m_woReelIdInfo = null;           // API
        public EQP_OP_CheckList_GetResult m_EqpOpCheckInfo = null;  // API
        private Mach_Master_GetResult m_MachInfo = null;            // API

        public double m_ReelUpdateDelay = 1;                // Reel資料刷新時間(分)
        private string m_machNo = "";     // 設定
        private string m_MatNo = "";      // 輸入
        private string m_TapingNo = "";   // 輸入
        private string m_UserID = "";     // 輸入
        private string m_WoNo = "";       // 輸入


        public frmReelCheck()
        {
            InitializeComponent();
        }

        // #1
        private void frmTest_Load(object sender, EventArgs e)
        {
            // #1 From load 時初始化l。
            init_LogWCF();
        }

        // #1
        public void init_LogWCF()
        {
            // 指定訊息語系
            cyntecCulture.Language = cyntecCulture.zh_TW;     // 指定全域變數     

            // 取設定檔
            string strXMLPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cyntec.ReelCheck.xml");
            if (cyntecFile.IsFileExist(strXMLPath) == false)
                throw new Exception($"設定檔案不存在,{strXMLPath}");
            cyntecXML oXML = new cyntecXML();
            oXML.Load(strXMLPath);
            m_Config = oXML;

            // 初始化log
            m_Log = new cyntecLog();
            m_Log.AppName = "ReelCheck";
            m_Log.MAX_LOG_SIZE = oXML.GetNode("LOG", "MAX_LOG_SIZE").ToInt32();
            m_Log.FILE_PATH = oXML.GetNode("LOG", "FILE_PATH").ToPathSlash();
            m_Log.IS_AUTO_PURGE = oXML.GetNode("LOG", "IS_AUTO_PURGE").ToBoolean();
            m_Log.IS_LOG_DEBUG = oXML.GetNode("LOG", "IS_LOG_DEBUG").ToBoolean();
            m_Log.IS_LOG_ERROR = oXML.GetNode("LOG", "IS_LOG_ERROR").ToBoolean();
            m_Log.IS_LOG_INFO = oXML.GetNode("LOG", "IS_LOG_INFO").ToBoolean();
            m_Log.IS_LOG_SQL = oXML.GetNode("LOG", "IS_LOG_SQL").ToBoolean();
            m_Log.IS_LOG_WARING = oXML.GetNode("LOG", "IS_LOG_WARING").ToBoolean();
            m_Log.KEEP_LOG_DAY = 30;
            m_Log.LogDebug("init, log");

            // 紀錄config檔案
            m_Log.LogDebug(strXMLPath + ", " + m_Config.ToXElement().ToString());

            // 初始化API連線
            m_Log.LogDebug("init, wcfServiceConnection");
            string clientUrl = oXML.GetNode("WCF", "CLIENT_URL").ToString();
            string serverUrl = oXML.GetNode("WCF", "SERVER_URL").ToString();
            m_txnConn = wcfServiceConnection.NewConnection(serverUrl, clientUrl, null, null);

            // 備用Server
            {
                cyntecXMLNode oNode = oXML.GetNodeTry("WCF", "SERVER_URL_LIST");
                if (oNode != null)
                {
                    List<string> urlList = oNode.ToStringList();
                    foreach (string strURL in urlList)
                    {
                        if (string.IsNullOrWhiteSpace(strURL))
                            continue;

                        m_txnConn.ServerUrlList.Add(strURL);
                    }
                }
            }
    
            m_txnConn.Client_Id = "ReelCheck @ " + Environment.MachineName;
            m_txnConn.Address = m_txnConn.Client_Id;
            m_txnConn.onError += new Base.HOnError(m_txnConn_onError);
            m_txnConn.onLog += new Base.HOnLog(m_txnConn_onLog);         
        }

        // #1
        void m_txnConn_onLog(object sender, string message)
        {
            m_Log.LogDebug("m_txnConn_onLog," + message);
        }

        // #1
        void m_txnConn_onError(object sender, string message)
        {
            m_Log.LogError("m_txnConn_onError," + message);

        }

        // #2
        public void init_ReelBarcodeCheck()
        {
            // #2-1, 新工單初始化

            // 先清空避免異常
            this.m_WoNo = this.m_WoNo.Trim().ToUpper();
            this.m_MachInfo = null;
            this.m_WoInfo = null;
            this.m_Parameter = null;
            this.m_WoReelStatusInfo = null;
            this.m_woReelIdInfo = null;

            // 關閉m_ReelBarcodeCheck
            if (m_ReelBarcodeCheck != null)
                m_ReelBarcodeCheck.Close();

            // API 取設備訊息
            {               
                Mach_Master_Get oTxn = new Mach_Master_Get();
                oTxn._MachNo = this.m_machNo;
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError)
                    throw new Exception(oTxn.TxnErrorMsg + ", 取設備資訊異常");
                else if (oTxn.Result._ItemList.Count == 0)
                    throw new Exception("無法取得設備資訊," + oTxn._MachNo);
       
                this.m_MachInfo = oTxn.Result;
            }

            // API 取設備作業檢查清單
            {
                EQP_OP_CheckList_Get oTxn = new EQP_OP_CheckList_Get();
                oTxn._MachNo = this.m_machNo;
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError)
                    throw new Exception("無法取得設備作業檢查清單, " + oTxn._MachNo + ", " + oTxn.TxnErrorMsg);
                this.m_EqpOpCheckInfo = oTxn.Result;
            }

            // API 取工單訊息          
            {
                WoInfo_Get oTxn = new WoInfo_Get();
                oTxn._WoNo = this.m_WoNo;
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError) 
                    throw new Exception(oTxn.TxnErrorMsg);
              
                this.m_WoInfo = oTxn.Result;
            }

            // API 取ReelId 列表          
            {
                WO_ReelID_Get oTxn = new WO_ReelID_Get();
                oTxn._WoNo = this.m_WoInfo._WO._ItemList[0].WONo;
                foreach (WorkOrder wo in this.m_WoInfo._WOGroup._itemList)
                    oTxn._GorupWoNoList.Add(wo.WONo);
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError)
                    throw new Exception(oTxn.TxnErrorMsg);

                if(oTxn.Result._ItemList.Count == 0)
                    throw new Exception("工單" + oTxn._WoNo + "無法取得ReelID列表(未綁定Taping)");       

                this.m_woReelIdInfo = oTxn.Result;

                // 顯示當前Reel訊息
                f_ShowReelInfo();
            }

            // API 取Reel狀態清單
            {
                WO_ReelStatus_Get oTxn = new WO_ReelStatus_Get();
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError)
                    throw new Exception(oTxn.TxnErrorMsg);
                m_WoReelStatusInfo = oTxn.Result;
            }

            // API 取工單參數
            {
                cyntec.MES.GetParameter oTxn = new GetParameter();
                oTxn._WoNo = m_WoInfo._WO._ItemList[0].WONo;
                oTxn._WorkcenterNo = m_WoInfo._WO._ItemList[0].CurrentWorkCenter;
                oTxn._Material = m_WoInfo._WO._ItemList[0].MatNo;
                oTxn.clientExecute(m_txnConn);
                if (oTxn.IsTxnError)
                    throw new Exception("oTxn.TxnErrorMsg");
                this.m_Parameter = oTxn.Result;
            }

            // 初始化ReelCheck標籤檢查物件
            m_ReelBarcodeCheck = new WinMES.ReelCheck.ReelBarcodeCheck();
            m_ReelBarcodeCheck.m_EqpOpCheckInfo = this.m_EqpOpCheckInfo;
            m_ReelBarcodeCheck.m_Is_Reel_Check = true;
            m_ReelBarcodeCheck.m_Is_Start = true;
            m_ReelBarcodeCheck.m_IsMainMat = true;
            m_ReelBarcodeCheck.m_Log = this.m_Log;
            m_ReelBarcodeCheck.m_MachInfo = this.m_MachInfo;
            m_ReelBarcodeCheck.m_machNo = this.m_machNo;
            m_ReelBarcodeCheck.m_MainMatNo = this.m_WoInfo._WO._ItemList[0].MatNo;
            m_ReelBarcodeCheck.m_MatNo = this.m_WoInfo._WO._ItemList[0].MatNo;
            m_ReelBarcodeCheck.m_Parameter = this.m_Parameter;
            m_ReelBarcodeCheck.m_ReelUpdateDelay = this.m_ReelUpdateDelay;
            m_ReelBarcodeCheck.m_Start = true;
            m_ReelBarcodeCheck.m_SubMatNo ="";
            m_ReelBarcodeCheck.m_TapingNo = this.m_WoInfo._WO._ItemList[0].BatchNo;
            m_ReelBarcodeCheck.m_txnConn = this.m_txnConn;
            m_ReelBarcodeCheck.m_UserID = this.m_UserID;
            m_ReelBarcodeCheck.m_WoInfo = this.m_WoInfo;
            m_ReelBarcodeCheck.m_WoNo = this.m_WoInfo._WO._ItemList[0].WONo;
            m_ReelBarcodeCheck.m_woReelIdInfo = this.m_woReelIdInfo;
            m_ReelBarcodeCheck.m_WoReelStatusInfo = this.m_WoReelStatusInfo;
            m_ReelBarcodeCheck.m_WorkcenterNo = this.m_WoInfo._WO._ItemList[0].CurrentWorkCenter;
            m_ReelBarcodeCheck.m_WoStartDate = DateTime.Now;
            m_ReelBarcodeCheck.LogPath = m_Config.GetNode("WOLOG", "FILE_PATH").ToPathSlash(); // System.IO.Path.Combine(Environment.CurrentDirectory, @"WOLog") + @"\";
            m_ReelBarcodeCheck.OnRefresh += new WinMES.ReelCheck.HOnRefresh(m_ReelBarcodeCheck_OnRefresh);
            m_ReelBarcodeCheck.LogSummaryBegin();
            m_ReelBarcodeCheck.Start();
        }

        // #2
        void m_ReelBarcodeCheck_OnRefresh(object oReel, object oReelBarcodeCheck)
        {
            // 標籤檢查事件
            #region 跨線程執行 (使用form's thread), 顯示Reel清單
            try
            {
                cyntec.WinMES.ReelCheck.ReelBarcodeCheck oMyCheck = oReelBarcodeCheck as cyntec.WinMES.ReelCheck.ReelBarcodeCheck;
                cyntec.WinMES.ReelCheck.myReel oMyReel = oReel as cyntec.WinMES.ReelCheck.myReel;                    

                // 正常刷新
                this.Invoke((HuiInvoke)(() =>
                {
                    f_ShowReelInfo();
                }));

                // #3 執行後. 根據事件傳入的物件判斷是否有異常
                if (oMyCheck.m_Is_BarcodeCheckOK)
                {
                    // 正常刷新
                    this.Invoke((HuiInvoke)(() =>
                    {
                        txtReel_Scan_No.Text = oMyCheck.InputBarcode;
                        label1.Visible = true;
                        label1.BackColor = Color.Green;
                        label1.Text = "正常";

                        if (oMyReel != null)
                        {
                            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                            ToolTip1.SetToolTip(label1, oMyReel.Reel.ReelID + ", " + cyntecDateTime.NowToMSSQL());
                        }

                        Hint(cyntecDateTime.NowToMSSQL() + ", 標籤檢查OK,");
                    }));
                }
                else
                {
                    string strErrMsg = oMyCheck.m_ReelCheckMsg;

                    // #3 若有異常根據user需求決定是否要停機
                    // 增加鎖機功能, 鎖機
                    // f_MachLock(strErrMsg);

                    // 跨線程執行 (使用form's thread)  使用form's thread
                    this.Invoke((HuiInvoke)(() =>
                    {
                        label1.Visible = true;
                        label1.BackColor = Color.Red;
                        label1.Text = "異常";
                        System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                        ToolTip1.SetToolTip(label1, strErrMsg);
                        // Hint("T/P No.掃描異常, 請重刷工單。");
                        //cyntecMsgBox.ShowErrorMsg("異常", "T/P No.掃描異常, 請重刷工單。" + msg);

                        Hint(cyntecDateTime.NowToMSSQL() + ", 標籤檢查異常," + strErrMsg);

                    }));
                }
            }
            catch (Exception ex)
            {
                m_Log.LogError("m_ReelCheckInput_onMessageArrive(), this.Invoke((HuiInvoke), " + ex.Message);
            }
            #endregion
   
        }

        public void f_ShowReelInfo()
        {  
            try
            {
                lock (dataGridView1)
                {        

                    if (m_ReelBarcodeCheck != null && m_ReelBarcodeCheck.m_woReelIdInfo != null)
                    {
                        this.m_woReelIdInfo = m_ReelBarcodeCheck.m_woReelIdInfo;
                    }

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ReelID",typeof(string));
                    dt.Columns.Add("上機時間", typeof(string));
                    dt.Columns.Add("完成時間", typeof(string));
                    dt.Columns.Add("Reel次數", typeof(string));
                    dt.Columns.Add("料號次數", typeof(string));
                    dt.Columns.Add("TPNO次數", typeof(string));

                    IEnumerable<WO_ReelID> tempList = from o in this.m_woReelIdInfo._ItemList orderby o.TPStartTime descending select o;

                    foreach (WO_ReelID o in tempList)
                    {
                        DataRow oRow = dt.NewRow();
                        dt.Rows.Add(oRow);

                        if (o.TPStatus != "11")
                            continue;

                        if (o.Status != "00" && o.Status != "11")
                            continue;

                        if (string.IsNullOrWhiteSpace(o.TPMachNo))
                            continue;

                        if (o.TPStartTime == DateTime.MinValue)
                            continue;

                        oRow["ReelID"]= o.ReelID;
                        oRow["上機時間"] = cyntecDateTime.ToMSSQL(o.TPStartTime).Substring(5, 11);
                        if (o.TPEndTime == DateTime.MinValue)
                            oRow["完成時間"] = "";                
                        else
                            oRow["完成時間"] = cyntecDateTime.ToMSSQL(o.TPEndTime).Substring(5, 11);

                        if (o.ReelIDCount == 0)
                        {
                            oRow["Reel次數"] = "-";
                            oRow["料號次數"] = "-";
                            oRow["TPNO次數"] = "-";
                        }
                        else
                        {
                            oRow["Reel次數"] = o.ReelIDCount.ToString();
                            oRow["料號次數"] = o.MatNoCount.ToString();
                            oRow["TPNO次數"] = o.TPNoCount.ToString();  
                        }                       
                    }
                    dataGridView1.DataSource = dt;
                    dataGridView1.Show();
                }
            }
            catch(Exception ex)
            {
                m_Log.LogError("f_ShowReelInfo,異常," + ex.Message + "," + ex.StackTrace);
            }
        }

        // #2
        private void btnWoQry_Click(object sender, EventArgs e)
        {
            // #2, 新工單初始化
            try
            {
                // 刷工單、工號、TapingNo、料號
                this.m_WoNo = txtWoNo.Text.Trim().ToUpper();
                this.m_machNo = txtMachNo.Text.Trim().ToUpper();
                this.m_UserID = txtUserID.Text.Trim().ToUpper();
                this.m_TapingNo = txtBatchNo.Text.Trim().ToUpper();
                this.m_MatNo = txtMatNo.Text.Trim().ToUpper();

                // 初始化標籤檢查
                init_ReelBarcodeCheck();

                Hint((sender as Button).Text + ",完成");
            }
            catch(Exception ex)
            { 
                HintError((sender as Button).Text + ",異常,+" + ex.Message);
            }
        }

        // #3
        private void btnReelBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_ReelBarcodeCheck == null)
                    throw new Exception("m_ReelBarcodeCheck尚未初始化");

                if (chkMatNo.Checked)
                {
                    m_Log.LogDebug("RS232收到條碼," + txtMatNo.Text);
                    m_ReelBarcodeCheck.m_ReelCheckInput_onMessageArrive(txtMatNo.Text);
                }

                if (chkTPNo.Checked)
                {
                    m_Log.LogDebug("RS232收到條碼," + txtBatchNo.Text);
                    m_ReelBarcodeCheck.m_ReelCheckInput_onMessageArrive(txtBatchNo.Text);
                }

                if (chk2D.Checked)
                {
                    m_Log.LogDebug("RS232收到條碼," + txtReelID.Text);
                    m_ReelBarcodeCheck.m_ReelCheckInput_onMessageArrive(txtReelID.Text);
                }
            }
            catch(Exception ex)
            {
                HintError((sender as TextBox).Text + ",異常," + ex.Message);
            }
        }       

        // #4
        private void btnWOFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_ReelBarcodeCheck != null)
                {
                    StringBuilder sb = new StringBuilder();
                    List<string> ErrorMsgList = m_ReelBarcodeCheck.WoFinish();  // #4 工單結束作業
                    foreach (string errMsg in ErrorMsgList)
                        sb.Append(errMsg).Append(Environment.NewLine);

                    if (ErrorMsgList.Count == 0)
                    {
                        Hint((sender as Button).Name + ", 完成");
                    }
                    else
                    {
                        HintError((sender as Button).Name + ",異常," + sb.ToString());
                    }
                }
                else
                {
                    HintError((sender as Button).Name + ",異常,m_ReelBarcodeCheck,未啟用");
                }
            }
            catch (Exception ex)
            {
                HintError((sender as Button).Name + ",異常," + ex.Message);
            }
        }

        private void Hint(string msg)
        {
            // 寫log
            m_Log.LogDebug(msg);

            // 畫面顯示
            this.Invoke((HuiInvoke)(() =>
            {
                txtMsg.Text = msg;
            }));
        }

        private void HintError(string msg)
        {
            // 寫log
            m_Log.LogError(msg);

            // 畫面顯示
            this.Invoke((HuiInvoke)(() =>
            {
                txtMsg.Text = msg;
            }));
        }

        // #5
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_ReelBarcodeCheck != null)
                    m_ReelBarcodeCheck.Close();     // #5 關閉標籤防混程式

                txtReel_Scan_No.Text = "";
                dataGridView1.DataSource = null;

                Hint((sender as Button).Text + ",完成");
            }
            catch (Exception ex)
            {
                HintError((sender as TextBox).Text + ",異常," + ex.Message);
            }
        }

        // #5
        private void frmReelCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_ReelBarcodeCheck != null)
                    m_ReelBarcodeCheck.Close();     // #5 關閉標籤防混程式
            }
            catch { }
        }
    }
}
