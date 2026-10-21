namespace MESSimulator
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControlMain = new System.Windows.Forms.TabControl();
            tabMes = new System.Windows.Forms.TabPage();
            tabBroker = new System.Windows.Forms.TabPage();
            tabEap = new System.Windows.Forms.TabPage();

            // --- Tab 1: MES WebService 模擬器 ---
            panelTop = new System.Windows.Forms.Panel();
            lblHandshakeResponses = new System.Windows.Forms.Label();
            chkUserVerifyPass = new System.Windows.Forms.CheckBox();
            chkWOQRYPass = new System.Windows.Forms.CheckBox();
            chkWOCHECKINPass = new System.Windows.Forms.CheckBox();
            chkWOCHECKOUTPass = new System.Windows.Forms.CheckBox();
            chkEQPSTATUSPass = new System.Windows.Forms.CheckBox();
            chkRecipeMismatch = new System.Windows.Forms.CheckBox();
            chkEnableNGCodeList = new System.Windows.Forms.CheckBox();
            chkSimError0049 = new System.Windows.Forms.CheckBox();
            lblSimWoQryResultCode = new System.Windows.Forms.Label();
            txtSimWoQryResultCode = new System.Windows.Forms.TextBox();
            lblSimWoQryHint = new System.Windows.Forms.Label();
            lblWipQty = new System.Windows.Forms.Label();
            txtWipQty = new System.Windows.Forms.TextBox();
            lblInRequireInputCode = new System.Windows.Forms.Label();
            txtInRequireInputCode = new System.Windows.Forms.TextBox();
            lblOutRequireInputCode = new System.Windows.Forms.Label();
            txtOutRequireInputCode = new System.Windows.Forms.TextBox();
            lblRecipeId = new System.Windows.Forms.Label();
            txtRecipeId = new System.Windows.Forms.TextBox();
            btnStop = new System.Windows.Forms.Button();
            btnStart = new System.Windows.Forms.Button();
            btnExportMesLog = new System.Windows.Forms.Button();
            btnClearMesLog = new System.Windows.Forms.Button();
            txtPort = new System.Windows.Forms.TextBox();
            lblPort = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            splitContainer = new System.Windows.Forms.SplitContainer();
            grpRequest = new System.Windows.Forms.GroupBox();
            txtRequest = new System.Windows.Forms.TextBox();
            grpLog = new System.Windows.Forms.GroupBox();
            txtLog = new System.Windows.Forms.TextBox();

            // --- Tab 2: MQTT Broker 監控 ---
            panelBrokerTop = new System.Windows.Forms.Panel();
            lblBrokerStatus = new System.Windows.Forms.Label();
            lblBrokerPort = new System.Windows.Forms.Label();
            txtBrokerPort = new System.Windows.Forms.TextBox();
            btnBrokerStart = new System.Windows.Forms.Button();
            btnBrokerStop = new System.Windows.Forms.Button();
            lblClientCount = new System.Windows.Forms.Label();
            btnClearBrokerLog = new System.Windows.Forms.Button();
            splitBroker = new System.Windows.Forms.SplitContainer();
            grpClients = new System.Windows.Forms.GroupBox();
            dgvClients = new System.Windows.Forms.DataGridView();
            grpBrokerLog = new System.Windows.Forms.GroupBox();
            txtBrokerLog = new System.Windows.Forms.TextBox();

            // --- Tab 3: EAP 模擬器 (設備商測試) ---
            panelEapTop = new System.Windows.Forms.Panel();
            lblEapStatus = new System.Windows.Forms.Label();
            btnEapStart = new System.Windows.Forms.Button();
            btnEapStop = new System.Windows.Forms.Button();
            btnSelectAllAutoReply = new System.Windows.Forms.Button();
            btnUnselectAllAutoReply = new System.Windows.Forms.Button();
            btnResetForceFail = new System.Windows.Forms.Button();
            btnExportEapLog = new System.Windows.Forms.Button();
            btnClearEapLog = new System.Windows.Forms.Button();
            lblTargetTopic = new System.Windows.Forms.Label();
            txtTargetCmdTopic = new System.Windows.Forms.TextBox();
            btnSendAreYouThere = new System.Windows.Forms.Button();
            btnSendStatusReq = new System.Windows.Forms.Button();
            btnSendTimeCalibrate = new System.Windows.Forms.Button();
            lblTraceInterval = new System.Windows.Forms.Label();
            txtTraceInterval = new System.Windows.Forms.TextBox();
            lblTraceParams = new System.Windows.Forms.Label();
            txtTraceParams = new System.Windows.Forms.TextBox();
            btnSendInitTraceData = new System.Windows.Forms.Button();
            lblRemoteCmd = new System.Windows.Forms.Label();
            cboRemoteCmdType = new System.Windows.Forms.ComboBox();
            lblRemoteRecipe = new System.Windows.Forms.Label();
            txtRemoteRecipe = new System.Windows.Forms.TextBox();
            btnSendRemoteCmd = new System.Windows.Forms.Button();
            lblTerminalMsg = new System.Windows.Forms.Label();
            txtTerminalMsg = new System.Windows.Forms.TextBox();
            btnSendTerminalMsg = new System.Windows.Forms.Button();
            splitEap = new System.Windows.Forms.SplitContainer();
            grpEapRules = new System.Windows.Forms.GroupBox();
            dgvEapRules = new System.Windows.Forms.DataGridView();
            grpEapLog = new System.Windows.Forms.GroupBox();
            txtEapLog = new System.Windows.Forms.TextBox();

            tabControlMain.SuspendLayout();
            tabMes.SuspendLayout();
            tabBroker.SuspendLayout();
            tabEap.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            grpRequest.SuspendLayout();
            grpLog.SuspendLayout();
            panelBrokerTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBroker).BeginInit();
            splitBroker.Panel1.SuspendLayout();
            splitBroker.Panel2.SuspendLayout();
            splitBroker.SuspendLayout();
            grpClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
            grpBrokerLog.SuspendLayout();
            panelEapTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitEap).BeginInit();
            splitEap.Panel1.SuspendLayout();
            splitEap.Panel2.SuspendLayout();
            splitEap.SuspendLayout();
            grpEapRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEapRules).BeginInit();
            grpEapLog.SuspendLayout();
            SuspendLayout();

            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabMes);
            tabControlMain.Controls.Add(tabBroker);
            tabControlMain.Controls.Add(tabEap);
            tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlMain.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Regular);
            tabControlMain.Location = new System.Drawing.Point(0, 0);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new System.Drawing.Size(1360, 911);
            tabControlMain.TabIndex = 0;

            // 
            // tabMes
            // 
            tabMes.Controls.Add(splitContainer);
            tabMes.Controls.Add(panelTop);
            tabMes.Location = new System.Drawing.Point(4, 29);
            tabMes.Name = "tabMes";
            tabMes.Padding = new System.Windows.Forms.Padding(3);
            tabMes.Size = new System.Drawing.Size(1352, 878);
            tabMes.TabIndex = 0;
            tabMes.Text = "🌐 MES WebService 模擬器";
            tabMes.UseVisualStyleBackColor = true;

            // 
            // panelTop (Tab 1)
            // 
            panelTop.Controls.Add(lblHandshakeResponses);
            panelTop.Controls.Add(chkUserVerifyPass);
            panelTop.Controls.Add(chkWOQRYPass);
            panelTop.Controls.Add(chkWOCHECKINPass);
            panelTop.Controls.Add(chkWOCHECKOUTPass);
            panelTop.Controls.Add(chkEQPSTATUSPass);
            panelTop.Controls.Add(chkRecipeMismatch);
            panelTop.Controls.Add(chkEnableNGCodeList);
            panelTop.Controls.Add(chkSimError0049);
            panelTop.Controls.Add(lblWipQty);
            panelTop.Controls.Add(txtWipQty);
            panelTop.Controls.Add(lblInRequireInputCode);
            panelTop.Controls.Add(txtInRequireInputCode);
            panelTop.Controls.Add(lblOutRequireInputCode);
            panelTop.Controls.Add(txtOutRequireInputCode);
            panelTop.Controls.Add(lblRecipeId);
            panelTop.Controls.Add(txtRecipeId);
            panelTop.Controls.Add(btnExportMesLog);
            panelTop.Controls.Add(btnClearMesLog);
            panelTop.Controls.Add(btnStop);
            panelTop.Controls.Add(btnStart);
            panelTop.Controls.Add(txtPort);
            panelTop.Controls.Add(lblSimWoQryResultCode);
            panelTop.Controls.Add(txtSimWoQryResultCode);
            panelTop.Controls.Add(lblSimWoQryHint);
            panelTop.Controls.Add(lblPort);
            panelTop.Controls.Add(lblStatus);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(3, 3);
            panelTop.Margin = new System.Windows.Forms.Padding(4);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1346, 112);
            panelTop.TabIndex = 0;
            // 
            // lblHandshakeResponses
            // 
            lblHandshakeResponses.AutoSize = true;
            lblHandshakeResponses.Location = new System.Drawing.Point(15, 52);
            lblHandshakeResponses.Name = "lblHandshakeResponses";
            lblHandshakeResponses.Size = new System.Drawing.Size(98, 15);
            lblHandshakeResponses.TabIndex = 9;
            lblHandshakeResponses.Text = "交握回復 (PASS):";
            // 
            // chkUserVerifyPass
            // 
            chkUserVerifyPass.AutoSize = true;
            chkUserVerifyPass.Checked = true;
            chkUserVerifyPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkUserVerifyPass.Location = new System.Drawing.Point(124, 50);
            chkUserVerifyPass.Name = "chkUserVerifyPass";
            chkUserVerifyPass.Size = new System.Drawing.Size(83, 19);
            chkUserVerifyPass.TabIndex = 10;
            chkUserVerifyPass.Text = "UserVerify";
            chkUserVerifyPass.UseVisualStyleBackColor = true;
            // 
            // chkWOQRYPass
            // 
            chkWOQRYPass.AutoSize = true;
            chkWOQRYPass.Checked = true;
            chkWOQRYPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOQRYPass.Location = new System.Drawing.Point(212, 50);
            chkWOQRYPass.Name = "chkWOQRYPass";
            chkWOQRYPass.Size = new System.Drawing.Size(73, 19);
            chkWOQRYPass.TabIndex = 11;
            chkWOQRYPass.Text = "WOQRY";
            chkWOQRYPass.UseVisualStyleBackColor = true;
            // 
            // chkWOCHECKINPass
            // 
            chkWOCHECKINPass.AutoSize = true;
            chkWOCHECKINPass.Checked = true;
            chkWOCHECKINPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOCHECKINPass.Location = new System.Drawing.Point(290, 50);
            chkWOCHECKINPass.Name = "chkWOCHECKINPass";
            chkWOCHECKINPass.Size = new System.Drawing.Size(100, 19);
            chkWOCHECKINPass.TabIndex = 12;
            chkWOCHECKINPass.Text = "WOCHECKIN";
            chkWOCHECKINPass.UseVisualStyleBackColor = true;
            // 
            // chkWOCHECKOUTPass
            // 
            chkWOCHECKOUTPass.AutoSize = true;
            chkWOCHECKOUTPass.Checked = true;
            chkWOCHECKOUTPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOCHECKOUTPass.Location = new System.Drawing.Point(398, 50);
            chkWOCHECKOUTPass.Name = "chkWOCHECKOUTPass";
            chkWOCHECKOUTPass.Size = new System.Drawing.Size(113, 19);
            chkWOCHECKOUTPass.TabIndex = 13;
            chkWOCHECKOUTPass.Text = "WOCHECKOUT";
            chkWOCHECKOUTPass.UseVisualStyleBackColor = true;
            // 
            // chkEQPSTATUSPass
            // 
            chkEQPSTATUSPass.AutoSize = true;
            chkEQPSTATUSPass.Checked = true;
            chkEQPSTATUSPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkEQPSTATUSPass.Location = new System.Drawing.Point(518, 50);
            chkEQPSTATUSPass.Name = "chkEQPSTATUSPass";
            chkEQPSTATUSPass.Size = new System.Drawing.Size(95, 19);
            chkEQPSTATUSPass.TabIndex = 14;
            chkEQPSTATUSPass.Text = "EQPSTATUS";
            chkEQPSTATUSPass.UseVisualStyleBackColor = true;
            // 
            // chkRecipeMismatch
            // 
            chkRecipeMismatch.AutoSize = true;
            chkRecipeMismatch.Location = new System.Drawing.Point(620, 50);
            chkRecipeMismatch.Name = "chkRecipeMismatch";
            chkRecipeMismatch.Size = new System.Drawing.Size(200, 19);
            chkRecipeMismatch.TabIndex = 15;
            chkRecipeMismatch.Text = "工單 Recipe 不一致 (模擬 Fail)";
            chkRecipeMismatch.UseVisualStyleBackColor = true;
            // 
            // chkEnableNGCodeList
            // 
            chkEnableNGCodeList.AutoSize = true;
            chkEnableNGCodeList.Location = new System.Drawing.Point(825, 50);
            chkEnableNGCodeList.Name = "chkEnableNGCodeList";
            chkEnableNGCodeList.Size = new System.Drawing.Size(164, 19);
            chkEnableNGCodeList.TabIndex = 16;
            chkEnableNGCodeList.Text = "啟用 NGCodeListDT 模擬";
            chkEnableNGCodeList.UseVisualStyleBackColor = true;
            // 
            // chkSimError0049
            // 
            chkSimError0049.AutoSize = true;
            chkSimError0049.Location = new System.Drawing.Point(995, 50);
            chkSimError0049.Name = "chkSimError0049";
            chkSimError0049.Size = new System.Drawing.Size(185, 19);
            chkSimError0049.TabIndex = 19;
            chkSimError0049.Text = "模擬 ERROR0049 (站點不合)";
            chkSimError0049.UseVisualStyleBackColor = true;
            // 
            // lblSimWoQryResultCode
            // 
            lblSimWoQryResultCode.AutoSize = true;
            lblSimWoQryResultCode.Location = new System.Drawing.Point(15, 82);
            lblSimWoQryResultCode.Name = "lblSimWoQryResultCode";
            lblSimWoQryResultCode.Size = new System.Drawing.Size(253, 15);
            lblSimWoQryResultCode.TabIndex = 20;
            lblSimWoQryResultCode.Text = "模擬 TrackIn WOQRY 失敗代碼 (ResultCode):";
            // 
            // txtSimWoQryResultCode
            // 
            txtSimWoQryResultCode.Location = new System.Drawing.Point(278, 78);
            txtSimWoQryResultCode.Name = "txtSimWoQryResultCode";
            txtSimWoQryResultCode.Size = new System.Drawing.Size(150, 23);
            txtSimWoQryResultCode.TabIndex = 21;
            // 
            // lblSimWoQryHint
            // 
            lblSimWoQryHint.AutoSize = true;
            lblSimWoQryHint.ForeColor = System.Drawing.Color.DimGray;
            lblSimWoQryHint.Location = new System.Drawing.Point(440, 82);
            lblSimWoQryHint.Name = "lblSimWoQryHint";
            lblSimWoQryHint.Size = new System.Drawing.Size(515, 15);
            lblSimWoQryHint.TabIndex = 22;
            lblSimWoQryHint.Text = "(若有設定代碼，TrackIn 工單查詢將直接回傳 FAIL 並帶上此 ResultCode；留空則正常回覆)";
            // 
            // lblWipQty
            // 
            lblWipQty.AutoSize = true;
            lblWipQty.Location = new System.Drawing.Point(1180, 52);
            lblWipQty.Name = "lblWipQty";
            lblWipQty.Size = new System.Drawing.Size(90, 15);
            lblWipQty.TabIndex = 17;
            lblWipQty.Text = "投入數 (QTY):";
            // 
            // txtWipQty
            // 
            txtWipQty.Location = new System.Drawing.Point(1272, 48);
            txtWipQty.Name = "txtWipQty";
            txtWipQty.Size = new System.Drawing.Size(65, 23);
            txtWipQty.TabIndex = 18;
            txtWipQty.Text = "1000";
            // 
            // lblInRequireInputCode
            // 
            lblInRequireInputCode.AutoSize = true;
            lblInRequireInputCode.Location = new System.Drawing.Point(408, 22);
            lblInRequireInputCode.Name = "lblInRequireInputCode";
            lblInRequireInputCode.Size = new System.Drawing.Size(113, 15);
            lblInRequireInputCode.TabIndex = 5;
            lblInRequireInputCode.Text = "進站 Require Input:";
            // 
            // txtInRequireInputCode
            // 
            txtInRequireInputCode.Location = new System.Drawing.Point(537, 18);
            txtInRequireInputCode.Name = "txtInRequireInputCode";
            txtInRequireInputCode.Size = new System.Drawing.Size(116, 23);
            txtInRequireInputCode.TabIndex = 6;
            // 
            // lblOutRequireInputCode
            // 
            lblOutRequireInputCode.AutoSize = true;
            lblOutRequireInputCode.Location = new System.Drawing.Point(671, 22);
            lblOutRequireInputCode.Name = "lblOutRequireInputCode";
            lblOutRequireInputCode.Size = new System.Drawing.Size(113, 15);
            lblOutRequireInputCode.TabIndex = 7;
            lblOutRequireInputCode.Text = "出站 Require Input:";
            // 
            // txtOutRequireInputCode
            // 
            txtOutRequireInputCode.Location = new System.Drawing.Point(799, 18);
            txtOutRequireInputCode.Name = "txtOutRequireInputCode";
            txtOutRequireInputCode.Size = new System.Drawing.Size(116, 23);
            txtOutRequireInputCode.TabIndex = 8;
            // 
            // lblRecipeId
            // 
            lblRecipeId.AutoSize = true;
            lblRecipeId.Location = new System.Drawing.Point(930, 22);
            lblRecipeId.Name = "lblRecipeId";
            lblRecipeId.Size = new System.Drawing.Size(113, 15);
            lblRecipeId.TabIndex = 9;
            lblRecipeId.Text = "PP_SELECT Recipe:";
            // 
            // txtRecipeId
            // 
            txtRecipeId.Location = new System.Drawing.Point(1055, 18);
            txtRecipeId.Name = "txtRecipeId";
            txtRecipeId.Size = new System.Drawing.Size(116, 23);
            txtRecipeId.TabIndex = 10;
            txtRecipeId.Text = "11";
            // 
            // btnExportMesLog
            // 
            btnExportMesLog.Location = new System.Drawing.Point(1182, 12);
            btnExportMesLog.Name = "btnExportMesLog";
            btnExportMesLog.Size = new System.Drawing.Size(155, 30);
            btnExportMesLog.TabIndex = 23;
            btnExportMesLog.Text = "💾 輸出 MES Log";
            btnExportMesLog.UseVisualStyleBackColor = true;
            btnExportMesLog.Click += BtnExportMesLog_Click;
            // 
            // btnClearMesLog
            // 
            btnClearMesLog.Location = new System.Drawing.Point(1182, 74);
            btnClearMesLog.Name = "btnClearMesLog";
            btnClearMesLog.Size = new System.Drawing.Size(155, 28);
            btnClearMesLog.TabIndex = 24;
            btnClearMesLog.Text = "清除 MES 日誌";
            btnClearMesLog.UseVisualStyleBackColor = true;
            btnClearMesLog.Click += BtnClearMesLog_Click;
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new System.Drawing.Point(303, 9);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(88, 34);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += BtnStop_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(208, 9);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(88, 34);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += BtnStart_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(110, 15);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(85, 23);
            txtPort.TabIndex = 2;
            txtPort.Text = "8070";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new System.Drawing.Point(15, 22);
            lblPort.Name = "lblPort";
            lblPort.Size = new System.Drawing.Size(85, 15);
            lblPort.TabIndex = 1;
            lblPort.Text = "SOAP Port:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            lblStatus.Location = new System.Drawing.Point(15, 2);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(115, 16);
            lblStatus.TabIndex = 19;
            lblStatus.Text = "● Server Stopped";
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(3, 115);
            splitContainer.Name = "splitContainer";
            splitContainer.Panel1.Controls.Add(grpRequest);
            splitContainer.Panel2.Controls.Add(grpLog);
            splitContainer.Size = new System.Drawing.Size(1346, 760);
            splitContainer.SplitterDistance = 650;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 1;
            // 
            // grpRequest
            // 
            grpRequest.Controls.Add(txtRequest);
            grpRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            grpRequest.Location = new System.Drawing.Point(0, 0);
            grpRequest.Name = "grpRequest";
            grpRequest.Padding = new System.Windows.Forms.Padding(4);
            grpRequest.Size = new System.Drawing.Size(650, 760);
            grpRequest.TabIndex = 0;
            grpRequest.TabStop = false;
            grpRequest.Text = "Last SOAP Request";
            // 
            // txtRequest
            // 
            txtRequest.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            txtRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            txtRequest.Font = new System.Drawing.Font("Consolas", 9F);
            txtRequest.ForeColor = System.Drawing.Color.LightBlue;
            txtRequest.Location = new System.Drawing.Point(4, 22);
            txtRequest.Multiline = true;
            txtRequest.Name = "txtRequest";
            txtRequest.ReadOnly = true;
            txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtRequest.Size = new System.Drawing.Size(642, 734);
            txtRequest.TabIndex = 0;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpLog.Location = new System.Drawing.Point(0, 0);
            grpLog.Name = "grpLog";
            grpLog.Padding = new System.Windows.Forms.Padding(4);
            grpLog.Size = new System.Drawing.Size(691, 760);
            grpLog.TabIndex = 0;
            grpLog.TabStop = false;
            grpLog.Text = "MES Communication Log";
            // 
            // txtLog
            // 
            txtLog.BackColor = System.Drawing.Color.Black;
            txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            txtLog.ForeColor = System.Drawing.Color.Lime;
            txtLog.Location = new System.Drawing.Point(4, 22);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtLog.Size = new System.Drawing.Size(683, 734);
            txtLog.TabIndex = 0;

            // =========================================================================
            // Tab 2: MQTT Broker 監控 (Client 與訂閱 Topic 監看頁面)
            // =========================================================================
            tabBroker.Controls.Add(splitBroker);
            tabBroker.Controls.Add(panelBrokerTop);
            tabBroker.Location = new System.Drawing.Point(4, 29);
            tabBroker.Name = "tabBroker";
            tabBroker.Padding = new System.Windows.Forms.Padding(3);
            tabBroker.Size = new System.Drawing.Size(1352, 878);
            tabBroker.TabIndex = 1;
            tabBroker.Text = "📡 內建 MQTT Broker 監控 (Client / Topic)";
            tabBroker.UseVisualStyleBackColor = true;

            // panelBrokerTop
            panelBrokerTop.Controls.Add(lblBrokerStatus);
            panelBrokerTop.Controls.Add(lblBrokerPort);
            panelBrokerTop.Controls.Add(txtBrokerPort);
            panelBrokerTop.Controls.Add(btnBrokerStart);
            panelBrokerTop.Controls.Add(btnBrokerStop);
            panelBrokerTop.Controls.Add(lblClientCount);
            panelBrokerTop.Controls.Add(btnClearBrokerLog);
            panelBrokerTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelBrokerTop.Location = new System.Drawing.Point(3, 3);
            panelBrokerTop.Name = "panelBrokerTop";
            panelBrokerTop.Size = new System.Drawing.Size(1346, 52);
            panelBrokerTop.TabIndex = 0;

            lblBrokerStatus.AutoSize = true;
            lblBrokerStatus.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            lblBrokerStatus.ForeColor = System.Drawing.Color.Red;
            lblBrokerStatus.Location = new System.Drawing.Point(15, 16);
            lblBrokerStatus.Name = "lblBrokerStatus";
            lblBrokerStatus.Size = new System.Drawing.Size(150, 18);
            lblBrokerStatus.Text = "● Broker Stopped";

            lblBrokerPort.AutoSize = true;
            lblBrokerPort.Location = new System.Drawing.Point(185, 17);
            lblBrokerPort.Name = "lblBrokerPort";
            lblBrokerPort.Size = new System.Drawing.Size(80, 17);
            lblBrokerPort.Text = "Broker Port:";

            txtBrokerPort.Location = new System.Drawing.Point(272, 13);
            txtBrokerPort.Name = "txtBrokerPort";
            txtBrokerPort.Size = new System.Drawing.Size(75, 24);
            txtBrokerPort.Text = "1883";

            btnBrokerStart.Location = new System.Drawing.Point(360, 10);
            btnBrokerStart.Name = "btnBrokerStart";
            btnBrokerStart.Size = new System.Drawing.Size(100, 32);
            btnBrokerStart.Text = "啟動 Broker";
            btnBrokerStart.UseVisualStyleBackColor = true;
            btnBrokerStart.Click += BtnBrokerStart_Click;

            btnBrokerStop.Enabled = false;
            btnBrokerStop.Location = new System.Drawing.Point(468, 10);
            btnBrokerStop.Name = "btnBrokerStop";
            btnBrokerStop.Size = new System.Drawing.Size(100, 32);
            btnBrokerStop.Text = "停止 Broker";
            btnBrokerStop.UseVisualStyleBackColor = true;
            btnBrokerStop.Click += BtnBrokerStop_Click;

            lblClientCount.AutoSize = true;
            lblClientCount.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            lblClientCount.ForeColor = System.Drawing.Color.DarkSlateBlue;
            lblClientCount.Location = new System.Drawing.Point(590, 16);
            lblClientCount.Name = "lblClientCount";
            lblClientCount.Size = new System.Drawing.Size(160, 18);
            lblClientCount.Text = "目前連線 Client 數: 0";

            btnClearBrokerLog.Location = new System.Drawing.Point(1220, 10);
            btnClearBrokerLog.Name = "btnClearBrokerLog";
            btnClearBrokerLog.Size = new System.Drawing.Size(110, 32);
            btnClearBrokerLog.Text = "清除日誌";
            btnClearBrokerLog.UseVisualStyleBackColor = true;
            btnClearBrokerLog.Click += BtnClearBrokerLog_Click;

            // splitBroker
            splitBroker.Dock = System.Windows.Forms.DockStyle.Fill;
            splitBroker.Location = new System.Drawing.Point(3, 55);
            splitBroker.Name = "splitBroker";
            splitBroker.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitBroker.Panel1.Controls.Add(grpClients);
            splitBroker.Panel2.Controls.Add(grpBrokerLog);
            splitBroker.Size = new System.Drawing.Size(1346, 820);
            splitBroker.SplitterDistance = 480;
            splitBroker.SplitterWidth = 5;

            grpClients.Controls.Add(dgvClients);
            grpClients.Dock = System.Windows.Forms.DockStyle.Fill;
            grpClients.Location = new System.Drawing.Point(0, 0);
            grpClients.Name = "grpClients";
            grpClients.Padding = new System.Windows.Forms.Padding(5);
            grpClients.Size = new System.Drawing.Size(1346, 480);
            grpClients.TabStop = false;
            grpClients.Text = "目前已連線之 MQTT Clients 與訂閱 Topic 清單 (即時更新)";

            dgvClients.AllowUserToAddRows = false;
            dgvClients.AllowUserToDeleteRows = false;
            dgvClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvClients.BackgroundColor = System.Drawing.Color.White;
            dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvClients.Location = new System.Drawing.Point(5, 23);
            dgvClients.Name = "dgvClients";
            dgvClients.ReadOnly = true;
            dgvClients.RowHeadersVisible = false;
            dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvClients.Size = new System.Drawing.Size(1336, 452);

            grpBrokerLog.Controls.Add(txtBrokerLog);
            grpBrokerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpBrokerLog.Location = new System.Drawing.Point(0, 0);
            grpBrokerLog.Name = "grpBrokerLog";
            grpBrokerLog.Padding = new System.Windows.Forms.Padding(5);
            grpBrokerLog.Size = new System.Drawing.Size(1346, 335);
            grpBrokerLog.TabStop = false;
            grpBrokerLog.Text = "MQTT Broker 連線 / 斷線 / 訂閱事件日誌";

            txtBrokerLog.BackColor = System.Drawing.Color.FromArgb(20, 25, 30);
            txtBrokerLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtBrokerLog.Font = new System.Drawing.Font("Consolas", 9.5F);
            txtBrokerLog.ForeColor = System.Drawing.Color.Cyan;
            txtBrokerLog.Location = new System.Drawing.Point(5, 23);
            txtBrokerLog.Multiline = true;
            txtBrokerLog.Name = "txtBrokerLog";
            txtBrokerLog.ReadOnly = true;
            txtBrokerLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtBrokerLog.Size = new System.Drawing.Size(1336, 307);

            // =========================================================================
            // Tab 3: EAP 模擬器 (設備商測試)
            // =========================================================================
            tabEap.Controls.Add(splitEap);
            tabEap.Controls.Add(panelEapTop);
            tabEap.Location = new System.Drawing.Point(4, 29);
            tabEap.Name = "tabEap";
            tabEap.Padding = new System.Windows.Forms.Padding(3);
            tabEap.Size = new System.Drawing.Size(1352, 878);
            tabEap.TabIndex = 2;
            tabEap.Text = "🤖 EAP 模擬器 (設備商測試專用)";
            tabEap.UseVisualStyleBackColor = true;

            // panelEapTop
            panelEapTop.Controls.Add(lblEapStatus);
            panelEapTop.Controls.Add(btnEapStart);
            panelEapTop.Controls.Add(btnEapStop);
            panelEapTop.Controls.Add(btnSelectAllAutoReply);
            panelEapTop.Controls.Add(btnUnselectAllAutoReply);
            panelEapTop.Controls.Add(btnResetForceFail);
            panelEapTop.Controls.Add(btnExportEapLog);
            panelEapTop.Controls.Add(btnClearEapLog);
            panelEapTop.Controls.Add(lblTargetTopic);
            panelEapTop.Controls.Add(txtTargetCmdTopic);
            panelEapTop.Controls.Add(btnSendAreYouThere);
            panelEapTop.Controls.Add(btnSendStatusReq);
            panelEapTop.Controls.Add(btnSendTimeCalibrate);
            panelEapTop.Controls.Add(lblTraceInterval);
            panelEapTop.Controls.Add(txtTraceInterval);
            panelEapTop.Controls.Add(lblTraceParams);
            panelEapTop.Controls.Add(txtTraceParams);
            panelEapTop.Controls.Add(btnSendInitTraceData);
            panelEapTop.Controls.Add(lblRemoteCmd);
            panelEapTop.Controls.Add(cboRemoteCmdType);
            panelEapTop.Controls.Add(lblRemoteRecipe);
            panelEapTop.Controls.Add(txtRemoteRecipe);
            panelEapTop.Controls.Add(btnSendRemoteCmd);
            panelEapTop.Controls.Add(lblTerminalMsg);
            panelEapTop.Controls.Add(txtTerminalMsg);
            panelEapTop.Controls.Add(btnSendTerminalMsg);
            panelEapTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelEapTop.Location = new System.Drawing.Point(3, 3);
            panelEapTop.Name = "panelEapTop";
            panelEapTop.Size = new System.Drawing.Size(1346, 126);
            panelEapTop.TabIndex = 0;

            // Row 1: EAP Simulator Control & Batch Rule Toggle
            lblEapStatus.AutoSize = true;
            lblEapStatus.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            lblEapStatus.ForeColor = System.Drawing.Color.Red;
            lblEapStatus.Location = new System.Drawing.Point(12, 13);
            lblEapStatus.Name = "lblEapStatus";
            lblEapStatus.Size = new System.Drawing.Size(145, 18);
            lblEapStatus.Text = "● EAP Sim Stopped";

            btnEapStart.Location = new System.Drawing.Point(175, 7);
            btnEapStart.Name = "btnEapStart";
            btnEapStart.Size = new System.Drawing.Size(95, 30);
            btnEapStart.Text = "啟動 EAP";
            btnEapStart.UseVisualStyleBackColor = true;
            btnEapStart.Click += BtnEapStart_Click;

            btnEapStop.Enabled = false;
            btnEapStop.Location = new System.Drawing.Point(276, 7);
            btnEapStop.Name = "btnEapStop";
            btnEapStop.Size = new System.Drawing.Size(95, 30);
            btnEapStop.Text = "停止 EAP";
            btnEapStop.UseVisualStyleBackColor = true;
            btnEapStop.Click += BtnEapStop_Click;

            btnSelectAllAutoReply.Location = new System.Drawing.Point(390, 7);
            btnSelectAllAutoReply.Name = "btnSelectAllAutoReply";
            btnSelectAllAutoReply.Size = new System.Drawing.Size(120, 30);
            btnSelectAllAutoReply.Text = "☑ 全選自動回復";
            btnSelectAllAutoReply.UseVisualStyleBackColor = true;
            btnSelectAllAutoReply.Click += BtnSelectAllAutoReply_Click;

            btnUnselectAllAutoReply.Location = new System.Drawing.Point(516, 7);
            btnUnselectAllAutoReply.Name = "btnUnselectAllAutoReply";
            btnUnselectAllAutoReply.Size = new System.Drawing.Size(130, 30);
            btnUnselectAllAutoReply.Text = "☐ 全部取消自動回復";
            btnUnselectAllAutoReply.UseVisualStyleBackColor = true;
            btnUnselectAllAutoReply.Click += BtnUnselectAllAutoReply_Click;

            btnResetForceFail.Location = new System.Drawing.Point(652, 7);
            btnResetForceFail.Name = "btnResetForceFail";
            btnResetForceFail.Size = new System.Drawing.Size(135, 30);
            btnResetForceFail.Text = "重設全部為 PASS";
            btnResetForceFail.UseVisualStyleBackColor = true;
            btnResetForceFail.Click += BtnResetForceFail_Click;

            btnExportEapLog.Location = new System.Drawing.Point(1078, 7);
            btnExportEapLog.Name = "btnExportEapLog";
            btnExportEapLog.Size = new System.Drawing.Size(136, 30);
            btnExportEapLog.Text = "💾 輸出 EAP Log";
            btnExportEapLog.UseVisualStyleBackColor = true;
            btnExportEapLog.Click += BtnExportEapLog_Click;

            btnClearEapLog.Location = new System.Drawing.Point(1220, 7);
            btnClearEapLog.Name = "btnClearEapLog";
            btnClearEapLog.Size = new System.Drawing.Size(110, 30);
            btnClearEapLog.Text = "清除 EAP 日誌";
            btnClearEapLog.UseVisualStyleBackColor = true;
            btnClearEapLog.Click += BtnClearEapLog_Click;

            // Row 2: Target Cmd Topic (Default: TEST01) + Basic Probes + InitTraceData
            lblTargetTopic.AutoSize = true;
            lblTargetTopic.Location = new System.Drawing.Point(12, 51);
            lblTargetTopic.Name = "lblTargetTopic";
            lblTargetTopic.Size = new System.Drawing.Size(165, 17);
            lblTargetTopic.Text = "目標 Cmd Topic (TEST01):";

            txtTargetCmdTopic.Location = new System.Drawing.Point(180, 47);
            txtTargetCmdTopic.Name = "txtTargetCmdTopic";
            txtTargetCmdTopic.Size = new System.Drawing.Size(340, 24);
            txtTargetCmdTopic.Text = "TN2/YELLOW/machine/TEST01/Command";

            btnSendAreYouThere.Location = new System.Drawing.Point(528, 44);
            btnSendAreYouThere.Name = "btnSendAreYouThere";
            btnSendAreYouThere.Size = new System.Drawing.Size(128, 30);
            btnSendAreYouThere.Text = "發送 AreYouThere";
            btnSendAreYouThere.UseVisualStyleBackColor = true;
            btnSendAreYouThere.Click += BtnSendAreYouThere_Click;

            btnSendStatusReq.Location = new System.Drawing.Point(662, 44);
            btnSendStatusReq.Name = "btnSendStatusReq";
            btnSendStatusReq.Size = new System.Drawing.Size(116, 30);
            btnSendStatusReq.Text = "發送 StatusReq";
            btnSendStatusReq.UseVisualStyleBackColor = true;
            btnSendStatusReq.Click += BtnSendStatusReq_Click;

            btnSendTimeCalibrate.Location = new System.Drawing.Point(784, 44);
            btnSendTimeCalibrate.Name = "btnSendTimeCalibrate";
            btnSendTimeCalibrate.Size = new System.Drawing.Size(132, 30);
            btnSendTimeCalibrate.Text = "發送 TimeCalibrate";
            btnSendTimeCalibrate.UseVisualStyleBackColor = true;
            btnSendTimeCalibrate.Click += BtnSendTimeCalibrate_Click;

            lblTraceInterval.AutoSize = true;
            lblTraceInterval.Location = new System.Drawing.Point(926, 51);
            lblTraceInterval.Name = "lblTraceInterval";
            lblTraceInterval.Size = new System.Drawing.Size(95, 17);
            lblTraceInterval.Text = "Trace週期(Timer):";

            txtTraceInterval.Location = new System.Drawing.Point(1025, 47);
            txtTraceInterval.Name = "txtTraceInterval";
            txtTraceInterval.Size = new System.Drawing.Size(50, 24);
            txtTraceInterval.Text = "5";

            lblTraceParams.AutoSize = true;
            lblTraceParams.Location = new System.Drawing.Point(1082, 51);
            lblTraceParams.Name = "lblTraceParams";
            lblTraceParams.Size = new System.Drawing.Size(55, 17);
            lblTraceParams.Text = "Enable:";

            txtTraceParams.Location = new System.Drawing.Point(1138, 47);
            txtTraceParams.Name = "txtTraceParams";
            txtTraceParams.Size = new System.Drawing.Size(55, 24);
            txtTraceParams.Text = "true";

            btnSendInitTraceData.Location = new System.Drawing.Point(1200, 44);
            btnSendInitTraceData.Name = "btnSendInitTraceData";
            btnSendInitTraceData.Size = new System.Drawing.Size(130, 30);
            btnSendInitTraceData.Text = "發送 InitTraceData";
            btnSendInitTraceData.UseVisualStyleBackColor = true;
            btnSendInitTraceData.Click += BtnSendInitTraceData_Click;

            // Row 3: RemoteCMD (with clear dropdown & RecipeID label) + TerminalDisplay (with clear label)
            lblRemoteCmd.AutoSize = true;
            lblRemoteCmd.Location = new System.Drawing.Point(12, 91);
            lblRemoteCmd.Name = "lblRemoteCmd";
            lblRemoteCmd.Size = new System.Drawing.Size(145, 17);
            lblRemoteCmd.Text = "遠端控制 (RemoteCMD):";

            cboRemoteCmdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboRemoteCmdType.Items.AddRange(new object[] {
                "PP_SELECT",
                "START",
                "STOP",
                "PAUSE",
                "RESUME",
                "ABORT"
            });
            cboRemoteCmdType.Location = new System.Drawing.Point(162, 87);
            cboRemoteCmdType.Name = "cboRemoteCmdType";
            cboRemoteCmdType.Size = new System.Drawing.Size(115, 25);

            lblRemoteRecipe.AutoSize = true;
            lblRemoteRecipe.Location = new System.Drawing.Point(290, 91);
            lblRemoteRecipe.Name = "lblRemoteRecipe";
            lblRemoteRecipe.Size = new System.Drawing.Size(135, 17);
            lblRemoteRecipe.Text = "PP_SELECT RecipeID:";

            txtRemoteRecipe.Location = new System.Drawing.Point(428, 87);
            txtRemoteRecipe.Name = "txtRemoteRecipe";
            txtRemoteRecipe.PlaceholderText = "RecipeID";
            txtRemoteRecipe.Size = new System.Drawing.Size(100, 24);
            txtRemoteRecipe.Text = "11";

            btnSendRemoteCmd.Location = new System.Drawing.Point(536, 84);
            btnSendRemoteCmd.Name = "btnSendRemoteCmd";
            btnSendRemoteCmd.Size = new System.Drawing.Size(130, 30);
            btnSendRemoteCmd.Text = "發送 RemoteCMD";
            btnSendRemoteCmd.UseVisualStyleBackColor = true;
            btnSendRemoteCmd.Click += BtnSendRemoteCmd_Click;

            lblTerminalMsg.AutoSize = true;
            lblTerminalMsg.Location = new System.Drawing.Point(690, 91);
            lblTerminalMsg.Name = "lblTerminalMsg";
            lblTerminalMsg.Size = new System.Drawing.Size(185, 17);
            lblTerminalMsg.Text = "機台螢幕訊息 (TerminalDisplay):";

            txtTerminalMsg.Location = new System.Drawing.Point(882, 87);
            txtTerminalMsg.Name = "txtTerminalMsg";
            txtTerminalMsg.PlaceholderText = "請輸入要顯示於機台螢幕的文字";
            txtTerminalMsg.Size = new System.Drawing.Size(295, 24);
            txtTerminalMsg.Text = "EAP TEST MESSAGE";

            btnSendTerminalMsg.Location = new System.Drawing.Point(1185, 84);
            btnSendTerminalMsg.Name = "btnSendTerminalMsg";
            btnSendTerminalMsg.Size = new System.Drawing.Size(145, 30);
            btnSendTerminalMsg.Text = "發送 TerminalDisplay";
            btnSendTerminalMsg.UseVisualStyleBackColor = true;
            btnSendTerminalMsg.Click += BtnSendTerminalMsg_Click;

            // splitEap
            splitEap.Dock = System.Windows.Forms.DockStyle.Fill;
            splitEap.Location = new System.Drawing.Point(3, 129);
            splitEap.Name = "splitEap";
            splitEap.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitEap.Panel1.Controls.Add(grpEapRules);
            splitEap.Panel2.Controls.Add(grpEapLog);
            splitEap.Size = new System.Drawing.Size(1346, 746);
            splitEap.SplitterDistance = 400;
            splitEap.SplitterWidth = 5;

            grpEapRules.Controls.Add(dgvEapRules);
            grpEapRules.Dock = System.Windows.Forms.DockStyle.Fill;
            grpEapRules.Location = new System.Drawing.Point(0, 0);
            grpEapRules.Name = "grpEapRules";
            grpEapRules.Padding = new System.Windows.Forms.Padding(5);
            grpEapRules.Size = new System.Drawing.Size(1346, 420);
            grpEapRules.TabStop = false;
            grpEapRules.Text = "EAP 一般邏輯清單 — 可勾選「自動回復 (AutoReply)」與「強制失敗 (Force FAIL)」(已排除防混料、NGCODE上報、自動結帳、線別功能)";

            dgvEapRules.AllowUserToAddRows = false;
            dgvEapRules.AllowUserToDeleteRows = false;
            dgvEapRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvEapRules.BackgroundColor = System.Drawing.Color.White;
            dgvEapRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEapRules.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvEapRules.Location = new System.Drawing.Point(5, 23);
            dgvEapRules.Name = "dgvEapRules";
            dgvEapRules.RowHeadersVisible = false;
            dgvEapRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvEapRules.Size = new System.Drawing.Size(1336, 392);

            grpEapLog.Controls.Add(txtEapLog);
            grpEapLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpEapLog.Location = new System.Drawing.Point(0, 0);
            grpEapLog.Name = "grpEapLog";
            grpEapLog.Padding = new System.Windows.Forms.Padding(5);
            grpEapLog.Size = new System.Drawing.Size(1346, 355);
            grpEapLog.TabStop = false;
            grpEapLog.Text = "EAP 模擬器通訊與邏輯執行日誌 (JSON 收發詳情)";

            txtEapLog.BackColor = System.Drawing.Color.FromArgb(15, 20, 15);
            txtEapLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtEapLog.Font = new System.Drawing.Font("Consolas", 9.5F);
            txtEapLog.ForeColor = System.Drawing.Color.LightGreen;
            txtEapLog.Location = new System.Drawing.Point(5, 23);
            txtEapLog.Multiline = true;
            txtEapLog.Name = "txtEapLog";
            txtEapLog.ReadOnly = true;
            txtEapLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtEapLog.Size = new System.Drawing.Size(1336, 327);

            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1360, 911);
            Controls.Add(tabControlMain);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainForm";
            Text = "MES & EAP Integrated Simulator v2.0 (Built-in MQTT Broker)";
            Load += MainForm_Load;
            FormClosing += MainForm_FormClosing;

            tabControlMain.ResumeLayout(false);
            tabMes.ResumeLayout(false);
            tabBroker.ResumeLayout(false);
            tabEap.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            grpRequest.ResumeLayout(false);
            grpRequest.PerformLayout();
            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();

            panelBrokerTop.ResumeLayout(false);
            panelBrokerTop.PerformLayout();
            splitBroker.Panel1.ResumeLayout(false);
            splitBroker.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBroker).EndInit();
            splitBroker.ResumeLayout(false);
            grpClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
            grpBrokerLog.ResumeLayout(false);
            grpBrokerLog.PerformLayout();

            panelEapTop.ResumeLayout(false);
            panelEapTop.PerformLayout();
            splitEap.Panel1.ResumeLayout(false);
            splitEap.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitEap).EndInit();
            splitEap.ResumeLayout(false);
            grpEapRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEapRules).EndInit();
            grpEapLog.ResumeLayout(false);
            grpEapLog.PerformLayout();

            ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabMes;
        private System.Windows.Forms.TabPage tabBroker;
        private System.Windows.Forms.TabPage tabEap;

        // Tab 1 controls
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblInRequireInputCode;
        private System.Windows.Forms.TextBox txtInRequireInputCode;
        private System.Windows.Forms.Label lblOutRequireInputCode;
        private System.Windows.Forms.TextBox txtOutRequireInputCode;
        private System.Windows.Forms.Label lblRecipeId;
        private System.Windows.Forms.TextBox txtRecipeId;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExportMesLog;
        private System.Windows.Forms.Button btnClearMesLog;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox grpRequest;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblHandshakeResponses;
        private System.Windows.Forms.CheckBox chkUserVerifyPass;
        private System.Windows.Forms.CheckBox chkWOQRYPass;
        private System.Windows.Forms.CheckBox chkWOCHECKINPass;
        private System.Windows.Forms.CheckBox chkWOCHECKOUTPass;
        private System.Windows.Forms.CheckBox chkEQPSTATUSPass;
        private System.Windows.Forms.CheckBox chkRecipeMismatch;
        private System.Windows.Forms.CheckBox chkEnableNGCodeList;
        private System.Windows.Forms.CheckBox chkSimError0049;
        private System.Windows.Forms.Label lblWipQty;
        private System.Windows.Forms.TextBox txtWipQty;
        private System.Windows.Forms.Label lblSimWoQryResultCode;
        private System.Windows.Forms.TextBox txtSimWoQryResultCode;
        private System.Windows.Forms.Label lblSimWoQryHint;

        // Tab 2 controls
        private System.Windows.Forms.Panel panelBrokerTop;
        private System.Windows.Forms.Label lblBrokerStatus;
        private System.Windows.Forms.Label lblBrokerPort;
        private System.Windows.Forms.TextBox txtBrokerPort;
        private System.Windows.Forms.Button btnBrokerStart;
        private System.Windows.Forms.Button btnBrokerStop;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.Button btnClearBrokerLog;
        private System.Windows.Forms.SplitContainer splitBroker;
        private System.Windows.Forms.GroupBox grpClients;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.GroupBox grpBrokerLog;
        private System.Windows.Forms.TextBox txtBrokerLog;

        // Tab 3 controls
        private System.Windows.Forms.Panel panelEapTop;
        private System.Windows.Forms.Label lblEapStatus;
        private System.Windows.Forms.Button btnEapStart;
        private System.Windows.Forms.Button btnEapStop;
        private System.Windows.Forms.Button btnSelectAllAutoReply;
        private System.Windows.Forms.Button btnUnselectAllAutoReply;
        private System.Windows.Forms.Button btnResetForceFail;
        private System.Windows.Forms.Button btnExportEapLog;
        private System.Windows.Forms.Button btnClearEapLog;
        private System.Windows.Forms.Label lblTargetTopic;
        private System.Windows.Forms.TextBox txtTargetCmdTopic;
        private System.Windows.Forms.Button btnSendAreYouThere;
        private System.Windows.Forms.Button btnSendStatusReq;
        private System.Windows.Forms.Button btnSendTimeCalibrate;
        private System.Windows.Forms.Label lblTraceInterval;
        private System.Windows.Forms.TextBox txtTraceInterval;
        private System.Windows.Forms.Label lblTraceParams;
        private System.Windows.Forms.TextBox txtTraceParams;
        private System.Windows.Forms.Button btnSendInitTraceData;
        private System.Windows.Forms.Label lblRemoteCmd;
        private System.Windows.Forms.ComboBox cboRemoteCmdType;
        private System.Windows.Forms.Label lblRemoteRecipe;
        private System.Windows.Forms.TextBox txtRemoteRecipe;
        private System.Windows.Forms.Button btnSendRemoteCmd;
        private System.Windows.Forms.Label lblTerminalMsg;
        private System.Windows.Forms.TextBox txtTerminalMsg;
        private System.Windows.Forms.Button btnSendTerminalMsg;
        private System.Windows.Forms.SplitContainer splitEap;
        private System.Windows.Forms.GroupBox grpEapRules;
        private System.Windows.Forms.DataGridView dgvEapRules;
        private System.Windows.Forms.GroupBox grpEapLog;
        private System.Windows.Forms.TextBox txtEapLog;
    }
}
