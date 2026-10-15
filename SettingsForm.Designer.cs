namespace EAPDashboard
{
    partial class SettingsForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.pnlSystem = new System.Windows.Forms.Panel();
            this.numEqpStatusInterval = new System.Windows.Forms.NumericUpDown();
            this.lblEqpStatusInterval = new System.Windows.Forms.Label();
            this.numHeartbeat = new System.Windows.Forms.NumericUpDown();
            this.lblHeartbeat = new System.Windows.Forms.Label();
            this.txtControllerTopic = new System.Windows.Forms.TextBox();
            this.lblControllerTopic = new System.Windows.Forms.Label();
            this.numMqttPort = new System.Windows.Forms.NumericUpDown();
            this.lblMqttPort = new System.Windows.Forms.Label();
            this.txtMqttHost = new System.Windows.Forms.TextBox();
            this.lblMqttHost = new System.Windows.Forms.Label();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.txtExitPassword = new System.Windows.Forms.TextBox();
            this.lblExitPassword = new System.Windows.Forms.Label();
            this.txtEapId = new System.Windows.Forms.TextBox();
            this.lblEapId = new System.Windows.Forms.Label();
            this.txtDictionaryDir = new System.Windows.Forms.TextBox();
            this.lblDictionaryDir = new System.Windows.Forms.Label();
            this.txtMqttTopicPrefix = new System.Windows.Forms.TextBox();
            this.lblMqttTopicPrefix = new System.Windows.Forms.Label();
            this.tabMes = new System.Windows.Forms.TabPage();
            this.pnlMes = new System.Windows.Forms.Panel();
            this.grpReelCheck = new System.Windows.Forms.GroupBox();
            this.numReelTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblReelTimeout = new System.Windows.Forms.Label();
            this.txtReelClientUrl = new System.Windows.Forms.TextBox();
            this.lblReelClientUrl = new System.Windows.Forms.Label();
            this.txtReelTestUrl = new System.Windows.Forms.TextBox();
            this.lblReelTestUrl = new System.Windows.Forms.Label();
            this.txtReelProdUrl = new System.Windows.Forms.TextBox();
            this.lblReelProdUrl = new System.Windows.Forms.Label();
            this.cboReelEnv = new System.Windows.Forms.ComboBox();
            this.lblReelEnv = new System.Windows.Forms.Label();
            this.grpMes = new System.Windows.Forms.GroupBox();
            this.chkCassetteBinding = new System.Windows.Forms.CheckBox();
            this.txtMesTestUrl = new System.Windows.Forms.TextBox();
            this.lblMesTestUrl = new System.Windows.Forms.Label();
            this.txtMesProdUrl = new System.Windows.Forms.TextBox();
            this.lblMesProdUrl = new System.Windows.Forms.Label();
            this.cboMesEnv = new System.Windows.Forms.ComboBox();
            this.lblMesEnv = new System.Windows.Forms.Label();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.pnlDatabase = new System.Windows.Forms.Panel();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.chkLogAreYouThere = new System.Windows.Forms.CheckBox();
            this.numLogMaxSize = new System.Windows.Forms.NumericUpDown();
            this.lblLogMaxSize = new System.Windows.Forms.Label();
            this.numLogCompressDays = new System.Windows.Forms.NumericUpDown();
            this.lblLogCompressDays = new System.Windows.Forms.Label();
            this.numLogRetentionDays = new System.Windows.Forms.NumericUpDown();
            this.lblLogRetentionDays = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.grpPostgres = new System.Windows.Forms.GroupBox();
            this.numPgRetentionDays = new System.Windows.Forms.NumericUpDown();
            this.lblPgRetentionDays = new System.Windows.Forms.Label();
            this.txtPgConnStr = new System.Windows.Forms.TextBox();
            this.lblPgConnStr = new System.Windows.Forms.Label();
            this.chkPgEnabled = new System.Windows.Forms.CheckBox();
            this.tabEquipments = new System.Windows.Forms.TabPage();
            this.dgvEquipments = new System.Windows.Forms.DataGridView();
            this.colEqActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEqId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqProtocol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colEqStatusType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colEqIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqDeviceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqAutoReport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEqReelCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEqTrackOutValidation = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlEqToolbar = new System.Windows.Forms.Panel();
            this.btnDeleteEq = new System.Windows.Forms.Button();
            this.btnAddEq = new System.Windows.Forms.Button();
            this.tabLines = new System.Windows.Forms.TabPage();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colLineActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBatchCapacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStation1Eqs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStation2Eqs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLinesToolbar = new System.Windows.Forms.Panel();
            this.lblSmtWebPort = new System.Windows.Forms.Label();
            this.numSmtWebPort = new System.Windows.Forms.NumericUpDown();
            this.chkEnableSmtWeb = new System.Windows.Forms.CheckBox();
            this.btnDeleteLine = new System.Windows.Forms.Button();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.tabJsonPreview = new System.Windows.Forms.TabPage();
            this.txtJsonPreview = new System.Windows.Forms.TextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblFootnote = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabSystem.SuspendLayout();
            this.pnlSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqpStatusInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMqttPort)).BeginInit();
            this.tabMes.SuspendLayout();
            this.pnlMes.SuspendLayout();
            this.grpReelCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReelTimeout)).BeginInit();
            this.grpMes.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.pnlDatabase.SuspendLayout();
            this.grpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLogMaxSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogCompressDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogRetentionDays)).BeginInit();
            this.grpPostgres.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPgRetentionDays)).BeginInit();
            this.tabEquipments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipments)).BeginInit();
            this.pnlEqToolbar.SuspendLayout();
            this.tabLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.pnlLinesToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSmtWebPort)).BeginInit();
            this.tabJsonPreview.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSystem);
            this.tabControl.Controls.Add(this.tabMes);
            this.tabControl.Controls.Add(this.tabDatabase);
            this.tabControl.Controls.Add(this.tabEquipments);
            this.tabControl.Controls.Add(this.tabLines);
            this.tabControl.Controls.Add(this.tabJsonPreview);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F);
            this.tabControl.ItemSize = new System.Drawing.Size(140, 30);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1220, 660);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabSystem.Controls.Add(this.pnlSystem);
            this.tabSystem.Location = new System.Drawing.Point(4, 34);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(15);
            this.tabSystem.Size = new System.Drawing.Size(952, 532);
            this.tabSystem.TabIndex = 0;
            this.tabSystem.Text = " ⚙️ 系統與通訊 ";
            // 
            // pnlSystem
            // 
            this.pnlSystem.AutoScroll = true;
            this.pnlSystem.Controls.Add(this.txtMqttTopicPrefix);
            this.pnlSystem.Controls.Add(this.lblMqttTopicPrefix);
            this.pnlSystem.Controls.Add(this.txtDictionaryDir);
            this.pnlSystem.Controls.Add(this.lblDictionaryDir);
            this.pnlSystem.Controls.Add(this.numEqpStatusInterval);
            this.pnlSystem.Controls.Add(this.lblEqpStatusInterval);
            this.pnlSystem.Controls.Add(this.numHeartbeat);
            this.pnlSystem.Controls.Add(this.lblHeartbeat);
            this.pnlSystem.Controls.Add(this.txtControllerTopic);
            this.pnlSystem.Controls.Add(this.lblControllerTopic);
            this.pnlSystem.Controls.Add(this.numMqttPort);
            this.pnlSystem.Controls.Add(this.lblMqttPort);
            this.pnlSystem.Controls.Add(this.txtMqttHost);
            this.pnlSystem.Controls.Add(this.lblMqttHost);
            this.pnlSystem.Controls.Add(this.chkMinimizeToTray);
            this.pnlSystem.Controls.Add(this.txtExitPassword);
            this.pnlSystem.Controls.Add(this.lblExitPassword);
            this.pnlSystem.Controls.Add(this.txtEapId);
            this.pnlSystem.Controls.Add(this.lblEapId);
            this.pnlSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSystem.Location = new System.Drawing.Point(15, 15);
            this.pnlSystem.Name = "pnlSystem";
            this.pnlSystem.Size = new System.Drawing.Size(922, 502);
            this.pnlSystem.TabIndex = 0;
            // 
            // numEqpStatusInterval
            // 
            this.numEqpStatusInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numEqpStatusInterval.ForeColor = System.Drawing.Color.White;
            this.numEqpStatusInterval.Location = new System.Drawing.Point(220, 275);
            this.numEqpStatusInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numEqpStatusInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numEqpStatusInterval.Name = "numEqpStatusInterval";
            this.numEqpStatusInterval.Size = new System.Drawing.Size(120, 24);
            this.numEqpStatusInterval.TabIndex = 14;
            this.numEqpStatusInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // lblEqpStatusInterval
            // 
            this.lblEqpStatusInterval.AutoSize = true;
            this.lblEqpStatusInterval.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblEqpStatusInterval.Location = new System.Drawing.Point(25, 277);
            this.lblEqpStatusInterval.Name = "lblEqpStatusInterval";
            this.lblEqpStatusInterval.Size = new System.Drawing.Size(171, 17);
            this.lblEqpStatusInterval.TabIndex = 13;
            this.lblEqpStatusInterval.Text = "機台狀態上報週期 (秒):";
            // 
            // numHeartbeat
            // 
            this.numHeartbeat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numHeartbeat.ForeColor = System.Drawing.Color.White;
            this.numHeartbeat.Location = new System.Drawing.Point(220, 235);
            this.numHeartbeat.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numHeartbeat.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numHeartbeat.Name = "numHeartbeat";
            this.numHeartbeat.Size = new System.Drawing.Size(120, 24);
            this.numHeartbeat.TabIndex = 12;
            this.numHeartbeat.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblHeartbeat
            // 
            this.lblHeartbeat.AutoSize = true;
            this.lblHeartbeat.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHeartbeat.Location = new System.Drawing.Point(25, 237);
            this.lblHeartbeat.Name = "lblHeartbeat";
            this.lblHeartbeat.Size = new System.Drawing.Size(145, 17);
            this.lblHeartbeat.TabIndex = 11;
            this.lblHeartbeat.Text = "心跳存活發送間隔 (秒):";
            // 
            // txtControllerTopic
            // 
            this.txtControllerTopic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtControllerTopic.ForeColor = System.Drawing.Color.White;
            this.txtControllerTopic.Location = new System.Drawing.Point(220, 195);
            this.txtControllerTopic.Name = "txtControllerTopic";
            this.txtControllerTopic.Size = new System.Drawing.Size(400, 24);
            this.txtControllerTopic.TabIndex = 10;
            // 
            // lblControllerTopic
            // 
            this.lblControllerTopic.AutoSize = true;
            this.lblControllerTopic.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblControllerTopic.Location = new System.Drawing.Point(25, 198);
            this.lblControllerTopic.Name = "lblControllerTopic";
            this.lblControllerTopic.Size = new System.Drawing.Size(155, 17);
            this.lblControllerTopic.TabIndex = 9;
            this.lblControllerTopic.Text = "Controller 廣播 Topic:";
            // 
            // numMqttPort
            // 
            this.numMqttPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numMqttPort.ForeColor = System.Drawing.Color.White;
            this.numMqttPort.Location = new System.Drawing.Point(220, 155);
            this.numMqttPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numMqttPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMqttPort.Name = "numMqttPort";
            this.numMqttPort.Size = new System.Drawing.Size(120, 24);
            this.numMqttPort.TabIndex = 8;
            this.numMqttPort.Value = new decimal(new int[] {
            1883,
            0,
            0,
            0});
            // 
            // lblMqttPort
            // 
            this.lblMqttPort.AutoSize = true;
            this.lblMqttPort.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblMqttPort.Location = new System.Drawing.Point(25, 157);
            this.lblMqttPort.Name = "lblMqttPort";
            this.lblMqttPort.Size = new System.Drawing.Size(148, 17);
            this.lblMqttPort.TabIndex = 7;
            this.lblMqttPort.Text = "MQTT Broker 埠號 (Port):";
            // 
            // txtMqttHost
            // 
            this.txtMqttHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtMqttHost.ForeColor = System.Drawing.Color.White;
            this.txtMqttHost.Location = new System.Drawing.Point(220, 115);
            this.txtMqttHost.Name = "txtMqttHost";
            this.txtMqttHost.Size = new System.Drawing.Size(260, 24);
            this.txtMqttHost.TabIndex = 6;
            // 
            // lblMqttHost
            // 
            this.lblMqttHost.AutoSize = true;
            this.lblMqttHost.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblMqttHost.Location = new System.Drawing.Point(25, 118);
            this.lblMqttHost.Name = "lblMqttHost";
            this.lblMqttHost.Size = new System.Drawing.Size(169, 17);
            this.lblMqttHost.TabIndex = 5;
            this.lblMqttHost.Text = "MQTT Broker 位址 (Host):";
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.ForeColor = System.Drawing.Color.Gainsboro;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(220, 325);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(184, 21);
            this.chkMinimizeToTray.TabIndex = 4;
            this.chkMinimizeToTray.Text = "最小化時縮小至系統工作列";
            this.chkMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // txtDictionaryDir
            // 
            this.txtDictionaryDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtDictionaryDir.ForeColor = System.Drawing.Color.White;
            this.txtDictionaryDir.Location = new System.Drawing.Point(220, 360);
            this.txtDictionaryDir.Name = "txtDictionaryDir";
            this.txtDictionaryDir.Size = new System.Drawing.Size(260, 24);
            this.txtDictionaryDir.TabIndex = 15;
            this.txtDictionaryDir.Text = "Dictionaries";
            // 
            // lblDictionaryDir
            // 
            this.lblDictionaryDir.AutoSize = true;
            this.lblDictionaryDir.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblDictionaryDir.Location = new System.Drawing.Point(25, 363);
            this.lblDictionaryDir.Name = "lblDictionaryDir";
            this.lblDictionaryDir.Size = new System.Drawing.Size(185, 17);
            this.lblDictionaryDir.TabIndex = 16;
            this.lblDictionaryDir.Text = "字典檔根目錄 (Dictionary Dir):";
            // 
            // txtMqttTopicPrefix
            // 
            this.txtMqttTopicPrefix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtMqttTopicPrefix.ForeColor = System.Drawing.Color.White;
            this.txtMqttTopicPrefix.Location = new System.Drawing.Point(220, 400);
            this.txtMqttTopicPrefix.Name = "txtMqttTopicPrefix";
            this.txtMqttTopicPrefix.Size = new System.Drawing.Size(400, 24);
            this.txtMqttTopicPrefix.TabIndex = 17;
            this.txtMqttTopicPrefix.Text = "TN2/YELLOW/machine";
            // 
            // lblMqttTopicPrefix
            // 
            this.lblMqttTopicPrefix.AutoSize = true;
            this.lblMqttTopicPrefix.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblMqttTopicPrefix.Location = new System.Drawing.Point(25, 403);
            this.lblMqttTopicPrefix.Name = "lblMqttTopicPrefix";
            this.lblMqttTopicPrefix.Size = new System.Drawing.Size(185, 17);
            this.lblMqttTopicPrefix.TabIndex = 18;
            this.lblMqttTopicPrefix.Text = "MQTT 機台主題前綴 (Topic Prefix):";
            // 
            // txtExitPassword
            // 
            this.txtExitPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtExitPassword.ForeColor = System.Drawing.Color.White;
            this.txtExitPassword.Location = new System.Drawing.Point(220, 65);
            this.txtExitPassword.Name = "txtExitPassword";
            this.txtExitPassword.Size = new System.Drawing.Size(260, 24);
            this.txtExitPassword.TabIndex = 3;
            // 
            // lblExitPassword
            // 
            this.lblExitPassword.AutoSize = true;
            this.lblExitPassword.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblExitPassword.Location = new System.Drawing.Point(25, 68);
            this.lblExitPassword.Name = "lblExitPassword";
            this.lblExitPassword.Size = new System.Drawing.Size(119, 17);
            this.lblExitPassword.TabIndex = 2;
            this.lblExitPassword.Text = "系統關閉保護密碼:";
            // 
            // txtEapId
            // 
            this.txtEapId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtEapId.ForeColor = System.Drawing.Color.White;
            this.txtEapId.Location = new System.Drawing.Point(220, 25);
            this.txtEapId.Name = "txtEapId";
            this.txtEapId.Size = new System.Drawing.Size(260, 24);
            this.txtEapId.TabIndex = 1;
            // 
            // lblEapId
            // 
            this.lblEapId.AutoSize = true;
            this.lblEapId.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblEapId.Location = new System.Drawing.Point(25, 28);
            this.lblEapId.Name = "lblEapId";
            this.lblEapId.Size = new System.Drawing.Size(73, 17);
            this.lblEapId.TabIndex = 0;
            this.lblEapId.Text = "EAP 站別編號:";
            // 
            // tabMes
            // 
            this.tabMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabMes.Controls.Add(this.pnlMes);
            this.tabMes.Location = new System.Drawing.Point(4, 34);
            this.tabMes.Name = "tabMes";
            this.tabMes.Padding = new System.Windows.Forms.Padding(15);
            this.tabMes.Size = new System.Drawing.Size(952, 532);
            this.tabMes.TabIndex = 1;
            this.tabMes.Text = " 🏭 MES 與料捲 ";
            // 
            // pnlMes
            // 
            this.pnlMes.AutoScroll = true;
            this.pnlMes.Controls.Add(this.grpReelCheck);
            this.pnlMes.Controls.Add(this.grpMes);
            this.pnlMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMes.Location = new System.Drawing.Point(15, 15);
            this.pnlMes.Name = "pnlMes";
            this.pnlMes.Size = new System.Drawing.Size(922, 502);
            this.pnlMes.TabIndex = 0;
            // 
            // grpReelCheck
            // 
            this.grpReelCheck.Controls.Add(this.numReelTimeout);
            this.grpReelCheck.Controls.Add(this.lblReelTimeout);
            this.grpReelCheck.Controls.Add(this.txtReelClientUrl);
            this.grpReelCheck.Controls.Add(this.lblReelClientUrl);
            this.grpReelCheck.Controls.Add(this.txtReelTestUrl);
            this.grpReelCheck.Controls.Add(this.lblReelTestUrl);
            this.grpReelCheck.Controls.Add(this.txtReelProdUrl);
            this.grpReelCheck.Controls.Add(this.lblReelProdUrl);
            this.grpReelCheck.Controls.Add(this.cboReelEnv);
            this.grpReelCheck.Controls.Add(this.lblReelEnv);
            this.grpReelCheck.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpReelCheck.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpReelCheck.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpReelCheck.Location = new System.Drawing.Point(0, 200);
            this.grpReelCheck.Name = "grpReelCheck";
            this.grpReelCheck.Padding = new System.Windows.Forms.Padding(15);
            this.grpReelCheck.Size = new System.Drawing.Size(922, 230);
            this.grpReelCheck.TabIndex = 1;
            this.grpReelCheck.TabStop = false;
            this.grpReelCheck.Text = " 📦 料捲防混料設定 (ReelCheck WCF) ";
            // 
            // numReelTimeout
            // 
            this.numReelTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numReelTimeout.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.numReelTimeout.ForeColor = System.Drawing.Color.White;
            this.numReelTimeout.Location = new System.Drawing.Point(180, 185);
            this.numReelTimeout.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numReelTimeout.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numReelTimeout.Name = "numReelTimeout";
            this.numReelTimeout.Size = new System.Drawing.Size(120, 23);
            this.numReelTimeout.TabIndex = 9;
            this.numReelTimeout.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblReelTimeout
            // 
            this.lblReelTimeout.AutoSize = true;
            this.lblReelTimeout.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblReelTimeout.Location = new System.Drawing.Point(25, 187);
            this.lblReelTimeout.Name = "lblReelTimeout";
            this.lblReelTimeout.Size = new System.Drawing.Size(117, 15);
            this.lblReelTimeout.TabIndex = 8;
            this.lblReelTimeout.Text = "連線逾時秒數 (Sec):";
            // 
            // txtReelClientUrl
            // 
            this.txtReelClientUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtReelClientUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtReelClientUrl.ForeColor = System.Drawing.Color.White;
            this.txtReelClientUrl.Location = new System.Drawing.Point(180, 145);
            this.txtReelClientUrl.Name = "txtReelClientUrl";
            this.txtReelClientUrl.Size = new System.Drawing.Size(550, 23);
            this.txtReelClientUrl.TabIndex = 7;
            // 
            // lblReelClientUrl
            // 
            this.lblReelClientUrl.AutoSize = true;
            this.lblReelClientUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblReelClientUrl.Location = new System.Drawing.Point(25, 148);
            this.lblReelClientUrl.Name = "lblReelClientUrl";
            this.lblReelClientUrl.Size = new System.Drawing.Size(121, 15);
            this.lblReelClientUrl.TabIndex = 6;
            this.lblReelClientUrl.Text = "本地服務端點 (Client):";
            // 
            // txtReelTestUrl
            // 
            this.txtReelTestUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtReelTestUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtReelTestUrl.ForeColor = System.Drawing.Color.White;
            this.txtReelTestUrl.Location = new System.Drawing.Point(180, 105);
            this.txtReelTestUrl.Name = "txtReelTestUrl";
            this.txtReelTestUrl.Size = new System.Drawing.Size(550, 23);
            this.txtReelTestUrl.TabIndex = 5;
            // 
            // lblReelTestUrl
            // 
            this.lblReelTestUrl.AutoSize = true;
            this.lblReelTestUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblReelTestUrl.Location = new System.Drawing.Point(25, 108);
            this.lblReelTestUrl.Name = "lblReelTestUrl";
            this.lblReelTestUrl.Size = new System.Drawing.Size(128, 15);
            this.lblReelTestUrl.TabIndex = 4;
            this.lblReelTestUrl.Text = "測試環境網址 (TestUrl):";
            // 
            // txtReelProdUrl
            // 
            this.txtReelProdUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtReelProdUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtReelProdUrl.ForeColor = System.Drawing.Color.White;
            this.txtReelProdUrl.Location = new System.Drawing.Point(180, 65);
            this.txtReelProdUrl.Name = "txtReelProdUrl";
            this.txtReelProdUrl.Size = new System.Drawing.Size(550, 23);
            this.txtReelProdUrl.TabIndex = 3;
            // 
            // lblReelProdUrl
            // 
            this.lblReelProdUrl.AutoSize = true;
            this.lblReelProdUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblReelProdUrl.Location = new System.Drawing.Point(25, 68);
            this.lblReelProdUrl.Name = "lblReelProdUrl";
            this.lblReelProdUrl.Size = new System.Drawing.Size(147, 15);
            this.lblReelProdUrl.TabIndex = 2;
            this.lblReelProdUrl.Text = "正式環境網址 (ProdUrl):";
            // 
            // cboReelEnv
            // 
            this.cboReelEnv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.cboReelEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReelEnv.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.cboReelEnv.ForeColor = System.Drawing.Color.White;
            this.cboReelEnv.FormattingEnabled = true;
            this.cboReelEnv.Items.AddRange(new object[] {
            "Test",
            "Production"});
            this.cboReelEnv.Location = new System.Drawing.Point(180, 25);
            this.cboReelEnv.Name = "cboReelEnv";
            this.cboReelEnv.Size = new System.Drawing.Size(150, 23);
            this.cboReelEnv.TabIndex = 1;
            // 
            // lblReelEnv
            // 
            this.lblReelEnv.AutoSize = true;
            this.lblReelEnv.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblReelEnv.Location = new System.Drawing.Point(25, 28);
            this.lblReelEnv.Name = "lblReelEnv";
            this.lblReelEnv.Size = new System.Drawing.Size(107, 15);
            this.lblReelEnv.TabIndex = 0;
            this.lblReelEnv.Text = "目前生效環境切換:";
            // 
            // grpMes
            // 
            this.grpMes.Controls.Add(this.chkCassetteBinding);
            this.grpMes.Controls.Add(this.txtMesTestUrl);
            this.grpMes.Controls.Add(this.lblMesTestUrl);
            this.grpMes.Controls.Add(this.txtMesProdUrl);
            this.grpMes.Controls.Add(this.lblMesProdUrl);
            this.grpMes.Controls.Add(this.cboMesEnv);
            this.grpMes.Controls.Add(this.lblMesEnv);
            this.grpMes.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMes.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpMes.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpMes.Location = new System.Drawing.Point(0, 0);
            this.grpMes.Name = "grpMes";
            this.grpMes.Padding = new System.Windows.Forms.Padding(15);
            this.grpMes.Size = new System.Drawing.Size(922, 200);
            this.grpMes.TabIndex = 0;
            this.grpMes.TabStop = false;
            this.grpMes.Text = " 🏭 工廠 MES 報工設定 (WebService) ";
            // 
            // chkCassetteBinding
            // 
            this.chkCassetteBinding.AutoSize = true;
            this.chkCassetteBinding.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.chkCassetteBinding.Location = new System.Drawing.Point(180, 155);
            this.chkCassetteBinding.Name = "chkCassetteBinding";
            this.chkCassetteBinding.Size = new System.Drawing.Size(194, 19);
            this.chkCassetteBinding.TabIndex = 6;
            this.chkCassetteBinding.Text = "啟用全域卡匣自動綁定檢核機制";
            this.chkCassetteBinding.UseVisualStyleBackColor = true;
            // 
            // txtMesTestUrl
            // 
            this.txtMesTestUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtMesTestUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtMesTestUrl.ForeColor = System.Drawing.Color.White;
            this.txtMesTestUrl.Location = new System.Drawing.Point(180, 105);
            this.txtMesTestUrl.Name = "txtMesTestUrl";
            this.txtMesTestUrl.Size = new System.Drawing.Size(550, 23);
            this.txtMesTestUrl.TabIndex = 5;
            // 
            // lblMesTestUrl
            // 
            this.lblMesTestUrl.AutoSize = true;
            this.lblMesTestUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblMesTestUrl.Location = new System.Drawing.Point(25, 108);
            this.lblMesTestUrl.Name = "lblMesTestUrl";
            this.lblMesTestUrl.Size = new System.Drawing.Size(128, 15);
            this.lblMesTestUrl.TabIndex = 4;
            this.lblMesTestUrl.Text = "測試環境網址 (TestUrl):";
            // 
            // txtMesProdUrl
            // 
            this.txtMesProdUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtMesProdUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtMesProdUrl.ForeColor = System.Drawing.Color.White;
            this.txtMesProdUrl.Location = new System.Drawing.Point(180, 65);
            this.txtMesProdUrl.Name = "txtMesProdUrl";
            this.txtMesProdUrl.Size = new System.Drawing.Size(550, 23);
            this.txtMesProdUrl.TabIndex = 3;
            // 
            // lblMesProdUrl
            // 
            this.lblMesProdUrl.AutoSize = true;
            this.lblMesProdUrl.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblMesProdUrl.Location = new System.Drawing.Point(25, 68);
            this.lblMesProdUrl.Name = "lblMesProdUrl";
            this.lblMesProdUrl.Size = new System.Drawing.Size(147, 15);
            this.lblMesProdUrl.TabIndex = 2;
            this.lblMesProdUrl.Text = "正式環境網址 (ProdUrl):";
            // 
            // cboMesEnv
            // 
            this.cboMesEnv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.cboMesEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMesEnv.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.cboMesEnv.ForeColor = System.Drawing.Color.White;
            this.cboMesEnv.FormattingEnabled = true;
            this.cboMesEnv.Items.AddRange(new object[] {
            "Test",
            "Production"});
            this.cboMesEnv.Location = new System.Drawing.Point(180, 25);
            this.cboMesEnv.Name = "cboMesEnv";
            this.cboMesEnv.Size = new System.Drawing.Size(150, 23);
            this.cboMesEnv.TabIndex = 1;
            // 
            // lblMesEnv
            // 
            this.lblMesEnv.AutoSize = true;
            this.lblMesEnv.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblMesEnv.Location = new System.Drawing.Point(25, 28);
            this.lblMesEnv.Name = "lblMesEnv";
            this.lblMesEnv.Size = new System.Drawing.Size(107, 15);
            this.lblMesEnv.TabIndex = 0;
            this.lblMesEnv.Text = "目前生效環境切換:";
            // 
            // tabDatabase
            // 
            this.tabDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabDatabase.Controls.Add(this.pnlDatabase);
            this.tabDatabase.Location = new System.Drawing.Point(4, 34);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Padding = new System.Windows.Forms.Padding(15);
            this.tabDatabase.Size = new System.Drawing.Size(952, 532);
            this.tabDatabase.TabIndex = 2;
            this.tabDatabase.Text = " 🗄️ 資料庫與日誌 ";
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.AutoScroll = true;
            this.pnlDatabase.Controls.Add(this.grpLog);
            this.pnlDatabase.Controls.Add(this.grpPostgres);
            this.pnlDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDatabase.Location = new System.Drawing.Point(15, 15);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(922, 502);
            this.pnlDatabase.TabIndex = 0;
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.chkLogAreYouThere);
            this.grpLog.Controls.Add(this.numLogMaxSize);
            this.grpLog.Controls.Add(this.lblLogMaxSize);
            this.grpLog.Controls.Add(this.numLogCompressDays);
            this.grpLog.Controls.Add(this.lblLogCompressDays);
            this.grpLog.Controls.Add(this.numLogRetentionDays);
            this.grpLog.Controls.Add(this.lblLogRetentionDays);
            this.grpLog.Controls.Add(this.chkLogEnabled);
            this.grpLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLog.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpLog.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpLog.Location = new System.Drawing.Point(0, 160);
            this.grpLog.Name = "grpLog";
            this.grpLog.Padding = new System.Windows.Forms.Padding(15);
            this.grpLog.Size = new System.Drawing.Size(922, 210);
            this.grpLog.TabIndex = 1;
            this.grpLog.TabStop = false;
            this.grpLog.Text = " 📜 系統日誌與存活交握維運 (Log Maintenance) ";
            // 
            // chkLogAreYouThere
            // 
            this.chkLogAreYouThere.AutoSize = true;
            this.chkLogAreYouThere.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.chkLogAreYouThere.Location = new System.Drawing.Point(25, 165);
            this.chkLogAreYouThere.Name = "chkLogAreYouThere";
            this.chkLogAreYouThere.Size = new System.Drawing.Size(325, 19);
            this.chkLogAreYouThere.TabIndex = 7;
            this.chkLogAreYouThere.Text = "記錄雙向 AreYouThere / IamHere 存活交握日誌 (除錯用)";
            this.chkLogAreYouThere.UseVisualStyleBackColor = true;
            // 
            // numLogMaxSize
            // 
            this.numLogMaxSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numLogMaxSize.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.numLogMaxSize.ForeColor = System.Drawing.Color.White;
            this.numLogMaxSize.Location = new System.Drawing.Point(200, 125);
            this.numLogMaxSize.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numLogMaxSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLogMaxSize.Name = "numLogMaxSize";
            this.numLogMaxSize.Size = new System.Drawing.Size(120, 23);
            this.numLogMaxSize.TabIndex = 6;
            this.numLogMaxSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblLogMaxSize
            // 
            this.lblLogMaxSize.AutoSize = true;
            this.lblLogMaxSize.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblLogMaxSize.Location = new System.Drawing.Point(25, 128);
            this.lblLogMaxSize.Name = "lblLogMaxSize";
            this.lblLogMaxSize.Size = new System.Drawing.Size(139, 15);
            this.lblLogMaxSize.TabIndex = 5;
            this.lblLogMaxSize.Text = "單一日誌大小上限 (MB):";
            // 
            // numLogCompressDays
            // 
            this.numLogCompressDays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numLogCompressDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.numLogCompressDays.ForeColor = System.Drawing.Color.White;
            this.numLogCompressDays.Location = new System.Drawing.Point(200, 90);
            this.numLogCompressDays.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numLogCompressDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLogCompressDays.Name = "numLogCompressDays";
            this.numLogCompressDays.Size = new System.Drawing.Size(120, 23);
            this.numLogCompressDays.TabIndex = 4;
            this.numLogCompressDays.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblLogCompressDays
            // 
            this.lblLogCompressDays.AutoSize = true;
            this.lblLogCompressDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblLogCompressDays.Location = new System.Drawing.Point(25, 93);
            this.lblLogCompressDays.Name = "lblLogCompressDays";
            this.lblLogCompressDays.Size = new System.Drawing.Size(117, 15);
            this.lblLogCompressDays.TabIndex = 3;
            this.lblLogCompressDays.Text = "幾天後自動壓縮 (.gz):";
            // 
            // numLogRetentionDays
            // 
            this.numLogRetentionDays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numLogRetentionDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.numLogRetentionDays.ForeColor = System.Drawing.Color.White;
            this.numLogRetentionDays.Location = new System.Drawing.Point(200, 55);
            this.numLogRetentionDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numLogRetentionDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLogRetentionDays.Name = "numLogRetentionDays";
            this.numLogRetentionDays.Size = new System.Drawing.Size(120, 23);
            this.numLogRetentionDays.TabIndex = 2;
            this.numLogRetentionDays.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblLogRetentionDays
            // 
            this.lblLogRetentionDays.AutoSize = true;
            this.lblLogRetentionDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblLogRetentionDays.Location = new System.Drawing.Point(25, 58);
            this.lblLogRetentionDays.Name = "lblLogRetentionDays";
            this.lblLogRetentionDays.Size = new System.Drawing.Size(117, 15);
            this.lblLogRetentionDays.TabIndex = 1;
            this.lblLogRetentionDays.Text = "歷史日誌保留天數:";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.chkLogEnabled.Location = new System.Drawing.Point(25, 25);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(170, 19);
            this.chkLogEnabled.TabIndex = 0;
            this.chkLogEnabled.Text = "啟用日誌檔案自動維運服務";
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // grpPostgres
            // 
            this.grpPostgres.Controls.Add(this.numPgRetentionDays);
            this.grpPostgres.Controls.Add(this.lblPgRetentionDays);
            this.grpPostgres.Controls.Add(this.txtPgConnStr);
            this.grpPostgres.Controls.Add(this.lblPgConnStr);
            this.grpPostgres.Controls.Add(this.chkPgEnabled);
            this.grpPostgres.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPostgres.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpPostgres.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpPostgres.Location = new System.Drawing.Point(0, 0);
            this.grpPostgres.Name = "grpPostgres";
            this.grpPostgres.Padding = new System.Windows.Forms.Padding(15);
            this.grpPostgres.Size = new System.Drawing.Size(922, 160);
            this.grpPostgres.TabIndex = 0;
            this.grpPostgres.TabStop = false;
            this.grpPostgres.Text = " 🐘 PostgreSQL 本地歷史資料庫 (Local Data) ";
            // 
            // numPgRetentionDays
            // 
            this.numPgRetentionDays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.numPgRetentionDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.numPgRetentionDays.ForeColor = System.Drawing.Color.White;
            this.numPgRetentionDays.Location = new System.Drawing.Point(200, 110);
            this.numPgRetentionDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numPgRetentionDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPgRetentionDays.Name = "numPgRetentionDays";
            this.numPgRetentionDays.Size = new System.Drawing.Size(120, 23);
            this.numPgRetentionDays.TabIndex = 4;
            this.numPgRetentionDays.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // lblPgRetentionDays
            // 
            this.lblPgRetentionDays.AutoSize = true;
            this.lblPgRetentionDays.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblPgRetentionDays.Location = new System.Drawing.Point(25, 112);
            this.lblPgRetentionDays.Name = "lblPgRetentionDays";
            this.lblPgRetentionDays.Size = new System.Drawing.Size(129, 15);
            this.lblPgRetentionDays.TabIndex = 3;
            this.lblPgRetentionDays.Text = "分區日誌保留天數 (天):";
            // 
            // txtPgConnStr
            // 
            this.txtPgConnStr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(50)))));
            this.txtPgConnStr.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.txtPgConnStr.ForeColor = System.Drawing.Color.White;
            this.txtPgConnStr.Location = new System.Drawing.Point(200, 65);
            this.txtPgConnStr.Name = "txtPgConnStr";
            this.txtPgConnStr.Size = new System.Drawing.Size(650, 23);
            this.txtPgConnStr.TabIndex = 2;
            // 
            // lblPgConnStr
            // 
            this.lblPgConnStr.AutoSize = true;
            this.lblPgConnStr.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblPgConnStr.Location = new System.Drawing.Point(25, 68);
            this.lblPgConnStr.Name = "lblPgConnStr";
            this.lblPgConnStr.Size = new System.Drawing.Size(135, 15);
            this.lblPgConnStr.TabIndex = 1;
            this.lblPgConnStr.Text = "PostgreSQL 連線字串:";
            // 
            // chkPgEnabled
            // 
            this.chkPgEnabled.AutoSize = true;
            this.chkPgEnabled.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.chkPgEnabled.Location = new System.Drawing.Point(25, 28);
            this.chkPgEnabled.Name = "chkPgEnabled";
            this.chkPgEnabled.Size = new System.Drawing.Size(206, 19);
            this.chkPgEnabled.TabIndex = 0;
            this.chkPgEnabled.Text = "啟用 PostgreSQL 本地即時資料庫";
            this.chkPgEnabled.UseVisualStyleBackColor = true;
            // 
            // tabEquipments
            // 
            this.tabEquipments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabEquipments.Controls.Add(this.dgvEquipments);
            this.tabEquipments.Controls.Add(this.pnlEqToolbar);
            this.tabEquipments.Location = new System.Drawing.Point(4, 34);
            this.tabEquipments.Name = "tabEquipments";
            this.tabEquipments.Padding = new System.Windows.Forms.Padding(10);
            this.tabEquipments.Size = new System.Drawing.Size(952, 532);
            this.tabEquipments.TabIndex = 3;
            this.tabEquipments.Text = " 🖥️ 機台設備管理 ";
            // 
            // dgvEquipments
            // 
            this.dgvEquipments.AllowUserToAddRows = false;
            this.dgvEquipments.AllowUserToDeleteRows = false;
            this.dgvEquipments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEquipments.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvEquipments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvEquipments.ColumnHeadersHeight = 32;
            this.dgvEquipments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEqActive,
            this.colEqId,
            this.colEqName,
            this.colEqProtocol,
            this.colEqStatusType,
            this.colEqIp,
            this.colEqPort,
            this.colEqDeviceId,
            this.colEqAutoReport,
            this.colEqReelCheck,
            this.colEqTrackOutValidation});
            this.dgvEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEquipments.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.dgvEquipments.Location = new System.Drawing.Point(10, 52);
            this.dgvEquipments.Name = "dgvEquipments";
            this.dgvEquipments.RowHeadersVisible = false;
            this.dgvEquipments.RowTemplate.Height = 26;
            this.dgvEquipments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipments.Size = new System.Drawing.Size(1192, 560);
            this.dgvEquipments.TabIndex = 1;
            // 
            // colEqActive
            // 
            this.colEqActive.FillWeight = 16F;
            this.colEqActive.HeaderText = "啟用";
            this.colEqActive.MinimumWidth = 55;
            this.colEqActive.Name = "colEqActive";
            // 
            // colEqId
            // 
            this.colEqId.FillWeight = 32F;
            this.colEqId.HeaderText = "設備編號 (ID)";
            this.colEqId.MinimumWidth = 125;
            this.colEqId.Name = "colEqId";
            // 
            // colEqName
            // 
            this.colEqName.FillWeight = 36F;
            this.colEqName.HeaderText = "設備名稱";
            this.colEqName.MinimumWidth = 135;
            this.colEqName.Name = "colEqName";
            // 
            // colEqProtocol
            // 
            this.colEqProtocol.FillWeight = 24F;
            this.colEqProtocol.HeaderText = "通訊協定";
            this.colEqProtocol.Items.AddRange(new object[] {
            "MQTT",
            "SECS"});
            this.colEqProtocol.MinimumWidth = 95;
            this.colEqProtocol.Name = "colEqProtocol";
            // 
            // colEqStatusType
            // 
            this.colEqStatusType.FillWeight = 24F;
            this.colEqStatusType.HeaderText = "狀態格式";
            this.colEqStatusType.Items.AddRange(new object[] {
            "Cyntec",
            "Delta"});
            this.colEqStatusType.MinimumWidth = 95;
            this.colEqStatusType.Name = "colEqStatusType";
            // 
            // colEqIp
            // 
            this.colEqIp.FillWeight = 32F;
            this.colEqIp.HeaderText = "SECS IP";
            this.colEqIp.MinimumWidth = 125;
            this.colEqIp.Name = "colEqIp";
            // 
            // colEqPort
            // 
            this.colEqPort.FillWeight = 22F;
            this.colEqPort.HeaderText = "SECS 埠號";
            this.colEqPort.MinimumWidth = 90;
            this.colEqPort.Name = "colEqPort";
            // 
            // colEqDeviceId
            // 
            this.colEqDeviceId.FillWeight = 22F;
            this.colEqDeviceId.HeaderText = "Device ID";
            this.colEqDeviceId.MinimumWidth = 90;
            this.colEqDeviceId.Name = "colEqDeviceId";
            // 
            // colEqAutoReport
            // 
            this.colEqAutoReport.FillWeight = 22F;
            this.colEqAutoReport.HeaderText = "自動上報";
            this.colEqAutoReport.MinimumWidth = 90;
            this.colEqAutoReport.Name = "colEqAutoReport";
            // 
            // colEqReelCheck
            // 
            this.colEqReelCheck.FillWeight = 26F;
            this.colEqReelCheck.HeaderText = "料捲防混料";
            this.colEqReelCheck.MinimumWidth = 105;
            this.colEqReelCheck.Name = "colEqReelCheck";
            // 
            // colEqTrackOutValidation
            // 
            this.colEqTrackOutValidation.FillWeight = 38F;
            this.colEqTrackOutValidation.HeaderText = "出站結算/NG檢核";
            this.colEqTrackOutValidation.MinimumWidth = 145;
            this.colEqTrackOutValidation.Name = "colEqTrackOutValidation";
            // 
            // pnlEqToolbar
            // 
            this.pnlEqToolbar.Controls.Add(this.btnDeleteEq);
            this.pnlEqToolbar.Controls.Add(this.btnAddEq);
            this.pnlEqToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEqToolbar.Location = new System.Drawing.Point(10, 10);
            this.pnlEqToolbar.Name = "pnlEqToolbar";
            this.pnlEqToolbar.Size = new System.Drawing.Size(932, 42);
            this.pnlEqToolbar.TabIndex = 0;
            // 
            // btnDeleteEq
            // 
            this.btnDeleteEq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnDeleteEq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteEq.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.btnDeleteEq.ForeColor = System.Drawing.Color.White;
            this.btnDeleteEq.Location = new System.Drawing.Point(135, 5);
            this.btnDeleteEq.Name = "btnDeleteEq";
            this.btnDeleteEq.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteEq.TabIndex = 1;
            this.btnDeleteEq.Text = "➖ 刪除選取設備";
            this.btnDeleteEq.UseVisualStyleBackColor = false;
            this.btnDeleteEq.Click += new System.EventHandler(this.BtnDeleteEq_Click);
            // 
            // btnAddEq
            // 
            this.btnAddEq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(100)))), ((int)(((byte)(65)))));
            this.btnAddEq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEq.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.btnAddEq.ForeColor = System.Drawing.Color.White;
            this.btnAddEq.Location = new System.Drawing.Point(5, 5);
            this.btnAddEq.Name = "btnAddEq";
            this.btnAddEq.Size = new System.Drawing.Size(120, 30);
            this.btnAddEq.TabIndex = 0;
            this.btnAddEq.Text = "➕ 新增設備項目";
            this.btnAddEq.UseVisualStyleBackColor = false;
            this.btnAddEq.Click += new System.EventHandler(this.BtnAddEq_Click);
            // 
            // tabLines
            // 
            this.tabLines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabLines.Controls.Add(this.dgvLines);
            this.tabLines.Controls.Add(this.pnlLinesToolbar);
            this.tabLines.Location = new System.Drawing.Point(4, 34);
            this.tabLines.Name = "tabLines";
            this.tabLines.Padding = new System.Windows.Forms.Padding(10);
            this.tabLines.Size = new System.Drawing.Size(952, 532);
            this.tabLines.TabIndex = 4;
            this.tabLines.Text = " 🏭 產線設定 (Line) ";
            // 
            // dgvLines
            // 
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.AllowUserToDeleteRows = false;
            this.dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLineActive,
            this.colLineId,
            this.colLineName,
            this.colLineType,
            this.colBatchCapacity,
            this.colStation1Eqs,
            this.colStation2Eqs});
            this.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLines.Location = new System.Drawing.Point(10, 52);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.RowHeadersVisible = false;
            this.dgvLines.RowTemplate.Height = 28;
            this.dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLines.Size = new System.Drawing.Size(932, 470);
            this.dgvLines.TabIndex = 1;
            // 
            // colLineActive
            // 
            this.colLineActive.FillWeight = 50F;
            this.colLineActive.HeaderText = "啟用";
            this.colLineActive.Name = "colLineActive";
            // 
            // colLineId
            // 
            this.colLineId.FillWeight = 80F;
            this.colLineId.HeaderText = "線別代號";
            this.colLineId.Name = "colLineId";
            // 
            // colLineName
            // 
            this.colLineName.FillWeight = 110F;
            this.colLineName.HeaderText = "線別名稱";
            this.colLineName.Name = "colLineName";
            // 
            // colLineType
            // 
            this.colLineType.FillWeight = 70F;
            this.colLineType.HeaderText = "線別類型";
            this.colLineType.Items.AddRange(new object[] { "SMT", "Normal" });
            this.colLineType.Name = "colLineType";
            // 
            // colBatchCapacity
            // 
            this.colBatchCapacity.FillWeight = 70F;
            this.colBatchCapacity.HeaderText = "滿批上限";
            this.colBatchCapacity.Name = "colBatchCapacity";
            // 
            // colStation1Eqs
            // 
            this.colStation1Eqs.FillWeight = 130F;
            this.colStation1Eqs.HeaderText = "第一站設備清單(逗號分隔)";
            this.colStation1Eqs.Name = "colStation1Eqs";
            // 
            // colStation2Eqs
            // 
            this.colStation2Eqs.FillWeight = 130F;
            this.colStation2Eqs.HeaderText = "第二站設備清單(逗號分隔)";
            this.colStation2Eqs.Name = "colStation2Eqs";
            // 
            // pnlLinesToolbar
            // 
            this.pnlLinesToolbar.Controls.Add(this.lblSmtWebPort);
            this.pnlLinesToolbar.Controls.Add(this.numSmtWebPort);
            this.pnlLinesToolbar.Controls.Add(this.chkEnableSmtWeb);
            this.pnlLinesToolbar.Controls.Add(this.btnDeleteLine);
            this.pnlLinesToolbar.Controls.Add(this.btnAddLine);
            this.pnlLinesToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLinesToolbar.Location = new System.Drawing.Point(10, 10);
            this.pnlLinesToolbar.Name = "pnlLinesToolbar";
            this.pnlLinesToolbar.Size = new System.Drawing.Size(932, 42);
            this.pnlLinesToolbar.TabIndex = 0;
            // 
            // btnAddLine
            // 
            this.btnAddLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(100)))), ((int)(((byte)(65)))));
            this.btnAddLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLine.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.btnAddLine.ForeColor = System.Drawing.Color.White;
            this.btnAddLine.Location = new System.Drawing.Point(5, 5);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(120, 30);
            this.btnAddLine.TabIndex = 0;
            this.btnAddLine.Text = "➕ 新增 SMT 線別";
            this.btnAddLine.UseVisualStyleBackColor = false;
            this.btnAddLine.Click += new System.EventHandler(this.BtnAddLine_Click);
            // 
            // btnDeleteLine
            // 
            this.btnDeleteLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnDeleteLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteLine.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.btnDeleteLine.ForeColor = System.Drawing.Color.White;
            this.btnDeleteLine.Location = new System.Drawing.Point(135, 5);
            this.btnDeleteLine.Name = "btnDeleteLine";
            this.btnDeleteLine.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteLine.TabIndex = 1;
            this.btnDeleteLine.Text = "➖ 刪除選取線別";
            this.btnDeleteLine.UseVisualStyleBackColor = false;
            this.btnDeleteLine.Click += new System.EventHandler(this.BtnDeleteLine_Click);
            // 
            // chkEnableSmtWeb
            // 
            this.chkEnableSmtWeb.AutoSize = true;
            this.chkEnableSmtWeb.Checked = true;
            this.chkEnableSmtWeb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableSmtWeb.ForeColor = System.Drawing.Color.White;
            this.chkEnableSmtWeb.Location = new System.Drawing.Point(280, 10);
            this.chkEnableSmtWeb.Name = "chkEnableSmtWeb";
            this.chkEnableSmtWeb.Size = new System.Drawing.Size(164, 21);
            this.chkEnableSmtWeb.TabIndex = 2;
            this.chkEnableSmtWeb.Text = "啟用 SMT Web 監控中心";
            this.chkEnableSmtWeb.UseVisualStyleBackColor = true;
            // 
            // lblSmtWebPort
            // 
            this.lblSmtWebPort.AutoSize = true;
            this.lblSmtWebPort.ForeColor = System.Drawing.Color.White;
            this.lblSmtWebPort.Location = new System.Drawing.Point(460, 11);
            this.lblSmtWebPort.Name = "lblSmtWebPort";
            this.lblSmtWebPort.Size = new System.Drawing.Size(117, 17);
            this.lblSmtWebPort.TabIndex = 3;
            this.lblSmtWebPort.Text = "Web 伺服器通訊埠:";
            // 
            // numSmtWebPort
            // 
            this.numSmtWebPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(52)))));
            this.numSmtWebPort.ForeColor = System.Drawing.Color.White;
            this.numSmtWebPort.Location = new System.Drawing.Point(585, 9);
            this.numSmtWebPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numSmtWebPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            this.numSmtWebPort.Name = "numSmtWebPort";
            this.numSmtWebPort.Size = new System.Drawing.Size(80, 24);
            this.numSmtWebPort.TabIndex = 4;
            this.numSmtWebPort.Value = new decimal(new int[] { 8088, 0, 0, 0 });
            // 
            // tabJsonPreview
            // 
            this.tabJsonPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.tabJsonPreview.Controls.Add(this.txtJsonPreview);
            this.tabJsonPreview.Location = new System.Drawing.Point(4, 34);
            this.tabJsonPreview.Name = "tabJsonPreview";
            this.tabJsonPreview.Padding = new System.Windows.Forms.Padding(10);
            this.tabJsonPreview.Size = new System.Drawing.Size(952, 532);
            this.tabJsonPreview.TabIndex = 4;
            this.tabJsonPreview.Text = " 📄 即時 JSON 預覽 ";
            // 
            // txtJsonPreview
            // 
            this.txtJsonPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.txtJsonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJsonPreview.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.txtJsonPreview.ForeColor = System.Drawing.Color.LightCyan;
            this.txtJsonPreview.Location = new System.Drawing.Point(10, 10);
            this.txtJsonPreview.Multiline = true;
            this.txtJsonPreview.Name = "txtJsonPreview";
            this.txtJsonPreview.ReadOnly = true;
            this.txtJsonPreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJsonPreview.Size = new System.Drawing.Size(932, 512);
            this.txtJsonPreview.TabIndex = 0;
            this.txtJsonPreview.WordWrap = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pnlBottom.Controls.Add(this.lblFootnote);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 660);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1220, 60);
            this.pnlBottom.TabIndex = 1;
            // 
            // lblFootnote
            // 
            this.lblFootnote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFootnote.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblFootnote.ForeColor = System.Drawing.Color.DarkGray;
            this.lblFootnote.Location = new System.Drawing.Point(15, 12);
            this.lblFootnote.Name = "lblFootnote";
            this.lblFootnote.Size = new System.Drawing.Size(940, 36);
            this.lblFootnote.TabIndex = 2;
            this.lblFootnote.Text = "💡 提示：全域參數修改完一律需重啟生效；僅修改機台設定可無須重啟熱套用，未變動機台維持連線。";
            this.lblFootnote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(128)))), ((int)(((byte)(75)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(965, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 36);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 儲存並套用";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1110, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 36);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "關閉";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1220, 720);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EAP 系統設定管理與熱套用";
            this.tabControl.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.pnlSystem.ResumeLayout(false);
            this.pnlSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqpStatusInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMqttPort)).EndInit();
            this.tabMes.ResumeLayout(false);
            this.pnlMes.ResumeLayout(false);
            this.grpReelCheck.ResumeLayout(false);
            this.grpReelCheck.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReelTimeout)).EndInit();
            this.grpMes.ResumeLayout(false);
            this.grpMes.PerformLayout();
            this.tabDatabase.ResumeLayout(false);
            this.pnlDatabase.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLogMaxSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogCompressDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLogRetentionDays)).EndInit();
            this.grpPostgres.ResumeLayout(false);
            this.grpPostgres.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPgRetentionDays)).EndInit();
            this.tabEquipments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipments)).EndInit();
            this.pnlEqToolbar.ResumeLayout(false);
            this.tabJsonPreview.ResumeLayout(false);
            this.tabJsonPreview.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.Panel pnlSystem;
        private System.Windows.Forms.TextBox txtEapId;
        private System.Windows.Forms.Label lblEapId;
        private System.Windows.Forms.TextBox txtExitPassword;
        private System.Windows.Forms.Label lblExitPassword;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.TextBox txtMqttHost;
        private System.Windows.Forms.Label lblMqttHost;
        private System.Windows.Forms.NumericUpDown numMqttPort;
        private System.Windows.Forms.Label lblMqttPort;
        private System.Windows.Forms.TextBox txtControllerTopic;
        private System.Windows.Forms.Label lblControllerTopic;
        private System.Windows.Forms.NumericUpDown numHeartbeat;
        private System.Windows.Forms.Label lblHeartbeat;
        private System.Windows.Forms.NumericUpDown numEqpStatusInterval;
        private System.Windows.Forms.Label lblEqpStatusInterval;
        private System.Windows.Forms.TextBox txtDictionaryDir;
        private System.Windows.Forms.Label lblDictionaryDir;
        private System.Windows.Forms.TextBox txtMqttTopicPrefix;
        private System.Windows.Forms.Label lblMqttTopicPrefix;
        private System.Windows.Forms.TabPage tabMes;
        private System.Windows.Forms.Panel pnlMes;
        private System.Windows.Forms.GroupBox grpMes;
        private System.Windows.Forms.ComboBox cboMesEnv;
        private System.Windows.Forms.Label lblMesEnv;
        private System.Windows.Forms.TextBox txtMesProdUrl;
        private System.Windows.Forms.Label lblMesProdUrl;
        private System.Windows.Forms.TextBox txtMesTestUrl;
        private System.Windows.Forms.Label lblMesTestUrl;
        private System.Windows.Forms.CheckBox chkCassetteBinding;
        private System.Windows.Forms.GroupBox grpReelCheck;
        private System.Windows.Forms.ComboBox cboReelEnv;
        private System.Windows.Forms.Label lblReelEnv;
        private System.Windows.Forms.TextBox txtReelProdUrl;
        private System.Windows.Forms.Label lblReelProdUrl;
        private System.Windows.Forms.TextBox txtReelTestUrl;
        private System.Windows.Forms.Label lblReelTestUrl;
        private System.Windows.Forms.TextBox txtReelClientUrl;
        private System.Windows.Forms.Label lblReelClientUrl;
        private System.Windows.Forms.NumericUpDown numReelTimeout;
        private System.Windows.Forms.Label lblReelTimeout;
        private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.Panel pnlDatabase;
        private System.Windows.Forms.GroupBox grpPostgres;
        private System.Windows.Forms.CheckBox chkPgEnabled;
        private System.Windows.Forms.TextBox txtPgConnStr;
        private System.Windows.Forms.Label lblPgConnStr;
        private System.Windows.Forms.NumericUpDown numPgRetentionDays;
        private System.Windows.Forms.Label lblPgRetentionDays;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.NumericUpDown numLogRetentionDays;
        private System.Windows.Forms.Label lblLogRetentionDays;
        private System.Windows.Forms.NumericUpDown numLogCompressDays;
        private System.Windows.Forms.Label lblLogCompressDays;
        private System.Windows.Forms.NumericUpDown numLogMaxSize;
        private System.Windows.Forms.Label lblLogMaxSize;
        private System.Windows.Forms.CheckBox chkLogAreYouThere;
        private System.Windows.Forms.TabPage tabEquipments;
        private System.Windows.Forms.DataGridView dgvEquipments;
        private System.Windows.Forms.Panel pnlEqToolbar;
        private System.Windows.Forms.Button btnAddEq;
        private System.Windows.Forms.Button btnDeleteEq;
        private System.Windows.Forms.TabPage tabJsonPreview;
        private System.Windows.Forms.TextBox txtJsonPreview;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblFootnote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEqActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colEqProtocol;
        private System.Windows.Forms.DataGridViewComboBoxColumn colEqStatusType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqIp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqDeviceId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEqAutoReport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEqReelCheck;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEqTrackOutValidation;
        private System.Windows.Forms.TabPage tabLines;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colLineActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colLineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchCapacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStation1Eqs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStation2Eqs;
        private System.Windows.Forms.Panel pnlLinesToolbar;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnDeleteLine;
        private System.Windows.Forms.Label lblSmtWebPort;
        private System.Windows.Forms.NumericUpDown numSmtWebPort;
        private System.Windows.Forms.CheckBox chkEnableSmtWeb;
    }
}
