namespace EAP.ReelCheck.App;

partial class MainForm
{
    /// <summary>
    /// 設計工具所需的變數。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 清除任何使用中的資源。
    /// </summary>
    /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form 設計工具產生的程式碼

    /// <summary>
    /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
    /// 這個方法的內容。
    /// </summary>
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        _mainLayout = new TableLayoutPanel();
        _grpWo = new GroupBox();
        _woPanel = new FlowLayoutPanel();
        _lblMachineNo = new Label();
        _txtMachineNo = new TextBox();
        _lblWoNo = new Label();
        _txtWoNo = new TextBox();
        _lblUserId = new Label();
        _txtUserId = new TextBox();
        _btnInitWorkOrder = new Button();
        _chkSimulateOffline = new CheckBox();
        _woDetailPanel = new FlowLayoutPanel();
        _lblMatNo = new Label();
        _txtMatNo = new TextBox();
        _lblBatchNo = new Label();
        _txtBatchNo = new TextBox();
        _grpScan = new GroupBox();
        _scanLayout = new TableLayoutPanel();
        _scanLeft = new FlowLayoutPanel();
        _lblScanPrompt = new Label();
        _inputRow = new FlowLayoutPanel();
        _txtBarcodeScan = new TextBox();
        _btnSubmitScan = new Button();
        _testButtonsRow = new FlowLayoutPanel();
        _btnTestScanMatNo = new Button();
        _btnTestScanBatchNo = new Button();
        _btnTestScanLegalReel = new Button();
        _btnTestScanIllegalReel = new Button();
        _pnlStatusLight = new Panel();
        _lblDecodedReelId = new Label();
        _lblStatusDetail = new Label();
        _lblStatusText = new Label();
        _grpGrid = new GroupBox();
        _dgvReels = new DataGridView();
        _colReelId = new DataGridViewTextBoxColumn();
        _colStatus = new DataGridViewTextBoxColumn();
        _colMachNo = new DataGridViewTextBoxColumn();
        _colStartTime = new DataGridViewTextBoxColumn();
        _colReelScanCount = new DataGridViewTextBoxColumn();
        _colMatScanCount = new DataGridViewTextBoxColumn();
        _colTPScanCount = new DataGridViewTextBoxColumn();
        _grpLog = new GroupBox();
        _rtbLog = new RichTextBox();
        _mainLayout.SuspendLayout();
        _grpWo.SuspendLayout();
        _woPanel.SuspendLayout();
        _woDetailPanel.SuspendLayout();
        _grpScan.SuspendLayout();
        _scanLayout.SuspendLayout();
        _scanLeft.SuspendLayout();
        _inputRow.SuspendLayout();
        _testButtonsRow.SuspendLayout();
        _pnlStatusLight.SuspendLayout();
        _grpGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_dgvReels).BeginInit();
        _grpLog.SuspendLayout();
        SuspendLayout();
        // 
        // _mainLayout
        // 
        _mainLayout.ColumnCount = 1;
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _mainLayout.Controls.Add(_grpWo, 0, 0);
        _mainLayout.Controls.Add(_grpScan, 0, 1);
        _mainLayout.Controls.Add(_grpGrid, 0, 2);
        _mainLayout.Controls.Add(_grpLog, 0, 3);
        _mainLayout.Dock = DockStyle.Fill;
        _mainLayout.Location = new Point(0, 0);
        _mainLayout.Name = "_mainLayout";
        _mainLayout.Padding = new Padding(12);
        _mainLayout.RowCount = 4;
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 175F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        _mainLayout.Size = new Size(1200, 850);
        _mainLayout.TabIndex = 0;
        // 
        // _grpWo
        // 
        _grpWo.Controls.Add(_woPanel);
        _grpWo.Dock = DockStyle.Fill;
        _grpWo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _grpWo.Location = new Point(15, 15);
        _grpWo.Name = "_grpWo";
        _grpWo.Size = new Size(1170, 104);
        _grpWo.TabIndex = 0;
        _grpWo.TabStop = false;
        _grpWo.Text = "🏭 工單與機台作業設定";
        // 
        // _woPanel
        // 
        _woPanel.AutoScroll = true;
        _woPanel.Controls.Add(_lblMachineNo);
        _woPanel.Controls.Add(_txtMachineNo);
        _woPanel.Controls.Add(_lblWoNo);
        _woPanel.Controls.Add(_txtWoNo);
        _woPanel.Controls.Add(_lblUserId);
        _woPanel.Controls.Add(_txtUserId);
        _woPanel.Controls.Add(_btnInitWorkOrder);
        _woPanel.Controls.Add(_chkSimulateOffline);
        _woPanel.Controls.Add(_woDetailPanel);
        _woPanel.Dock = DockStyle.Fill;
        _woPanel.Font = new Font("Segoe UI", 9F);
        _woPanel.Location = new Point(3, 25);
        _woPanel.Name = "_woPanel";
        _woPanel.Padding = new Padding(8);
        _woPanel.Size = new Size(1164, 76);
        _woPanel.TabIndex = 0;
        // 
        // _lblMachineNo
        // 
        _lblMachineNo.AutoSize = true;
        _lblMachineNo.Location = new Point(8, 16);
        _lblMachineNo.Margin = new Padding(0, 8, 4, 0);
        _lblMachineNo.Name = "_lblMachineNo";
        _lblMachineNo.Size = new Size(76, 20);
        _lblMachineNo.TabIndex = 0;
        _lblMachineNo.Text = "機台編號:";
        // 
        // _txtMachineNo
        // 
        _txtMachineNo.Location = new Point(91, 11);
        _txtMachineNo.Name = "_txtMachineNo";
        _txtMachineNo.Size = new Size(90, 27);
        _txtMachineNo.TabIndex = 1;
        _txtMachineNo.Text = "TP0870";
        // 
        // _lblWoNo
        // 
        _lblWoNo.AutoSize = true;
        _lblWoNo.Location = new Point(194, 16);
        _lblWoNo.Margin = new Padding(10, 8, 4, 0);
        _lblWoNo.Name = "_lblWoNo";
        _lblWoNo.Size = new Size(76, 20);
        _lblWoNo.TabIndex = 2;
        _lblWoNo.Text = "工單號碼:";
        // 
        // _txtWoNo
        // 
        _txtWoNo.Location = new Point(277, 11);
        _txtWoNo.Name = "_txtWoNo";
        _txtWoNo.Size = new Size(130, 27);
        _txtWoNo.TabIndex = 3;
        _txtWoNo.Text = "071226000031";
        // 
        // _lblUserId
        // 
        _lblUserId.AutoSize = true;
        _lblUserId.Location = new Point(420, 16);
        _lblUserId.Margin = new Padding(10, 8, 4, 0);
        _lblUserId.Name = "_lblUserId";
        _lblUserId.Size = new Size(76, 20);
        _lblUserId.TabIndex = 4;
        _lblUserId.Text = "作業人員:";
        // 
        // _txtUserId
        // 
        _txtUserId.Location = new Point(503, 11);
        _txtUserId.Name = "_txtUserId";
        _txtUserId.Size = new Size(90, 27);
        _txtUserId.TabIndex = 5;
        _txtUserId.Text = "736984";
        // 
        // _btnInitWorkOrder
        // 
        _btnInitWorkOrder.BackColor = Color.FromArgb(41, 128, 185);
        _btnInitWorkOrder.FlatStyle = FlatStyle.Flat;
        _btnInitWorkOrder.ForeColor = Color.White;
        _btnInitWorkOrder.Location = new Point(599, 11);
        _btnInitWorkOrder.Name = "_btnInitWorkOrder";
        _btnInitWorkOrder.Size = new Size(110, 30);
        _btnInitWorkOrder.TabIndex = 6;
        _btnInitWorkOrder.Text = "🚀 開工初始化";
        _btnInitWorkOrder.UseVisualStyleBackColor = false;
        // 
        // _chkSimulateOffline
        // 
        _chkSimulateOffline.AutoSize = true;
        _chkSimulateOffline.Checked = true;
        _chkSimulateOffline.CheckState = CheckState.Checked;
        _chkSimulateOffline.Location = new Point(727, 14);
        _chkSimulateOffline.Margin = new Padding(15, 6, 0, 0);
        _chkSimulateOffline.Name = "_chkSimulateOffline";
        _chkSimulateOffline.Size = new Size(206, 24);
        _chkSimulateOffline.TabIndex = 7;
        _chkSimulateOffline.Text = "離線模擬模式 (免接 MES)";
        _chkSimulateOffline.UseVisualStyleBackColor = true;
        // 
        // _woDetailPanel
        // 
        _woDetailPanel.Controls.Add(_lblMatNo);
        _woDetailPanel.Controls.Add(_txtMatNo);
        _woDetailPanel.Controls.Add(_lblBatchNo);
        _woDetailPanel.Controls.Add(_txtBatchNo);
        _woDetailPanel.Location = new Point(8, 48);
        _woDetailPanel.Margin = new Padding(0, 4, 0, 0);
        _woDetailPanel.Name = "_woDetailPanel";
        _woDetailPanel.Size = new Size(1100, 35);
        _woDetailPanel.TabIndex = 8;
        // 
        // _lblMatNo
        // 
        _lblMatNo.AutoSize = true;
        _lblMatNo.Location = new Point(0, 6);
        _lblMatNo.Margin = new Padding(0, 6, 4, 0);
        _lblMatNo.Name = "_lblMatNo";
        _lblMatNo.Size = new Size(136, 20);
        _lblMatNo.TabIndex = 0;
        _lblMatNo.Text = "工單料號 (MatNo):";
        // 
        // _txtMatNo
        // 
        _txtMatNo.BackColor = Color.FromArgb(235, 235, 235);
        _txtMatNo.Location = new Point(143, 3);
        _txtMatNo.Name = "_txtMatNo";
        _txtMatNo.ReadOnly = true;
        _txtMatNo.Size = new Size(150, 27);
        _txtMatNo.TabIndex = 1;
        // 
        // _lblBatchNo
        // 
        _lblBatchNo.AutoSize = true;
        _lblBatchNo.Location = new Point(316, 6);
        _lblBatchNo.Margin = new Padding(20, 6, 4, 0);
        _lblBatchNo.Name = "_lblBatchNo";
        _lblBatchNo.Size = new Size(147, 20);
        _lblBatchNo.TabIndex = 2;
        _lblBatchNo.Text = "工單批號 (BatchNo):";
        // 
        // _txtBatchNo
        // 
        _txtBatchNo.BackColor = Color.FromArgb(235, 235, 235);
        _txtBatchNo.Location = new Point(470, 3);
        _txtBatchNo.Name = "_txtBatchNo";
        _txtBatchNo.ReadOnly = true;
        _txtBatchNo.Size = new Size(150, 27);
        _txtBatchNo.TabIndex = 3;
        // 
        // _grpScan
        // 
        _grpScan.Controls.Add(_scanLayout);
        _grpScan.Dock = DockStyle.Fill;
        _grpScan.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _grpScan.Location = new Point(15, 125);
        _grpScan.Name = "_grpScan";
        _grpScan.Size = new Size(1170, 169);
        _grpScan.TabIndex = 1;
        _grpScan.TabStop = false;
        _grpScan.Text = "🔍 條碼掃描與防混檢驗";
        // 
        // _scanLayout
        // 
        _scanLayout.ColumnCount = 2;
        _scanLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        _scanLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        _scanLayout.Controls.Add(_scanLeft, 0, 0);
        _scanLayout.Controls.Add(_pnlStatusLight, 1, 0);
        _scanLayout.Dock = DockStyle.Fill;
        _scanLayout.Location = new Point(3, 25);
        _scanLayout.Name = "_scanLayout";
        _scanLayout.RowCount = 1;
        _scanLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _scanLayout.Size = new Size(1164, 141);
        _scanLayout.TabIndex = 0;
        // 
        // _scanLeft
        // 
        _scanLeft.Controls.Add(_lblScanPrompt);
        _scanLeft.Controls.Add(_inputRow);
        _scanLeft.Controls.Add(_testButtonsRow);
        _scanLeft.Dock = DockStyle.Fill;
        _scanLeft.Location = new Point(3, 3);
        _scanLeft.Name = "_scanLeft";
        _scanLeft.Padding = new Padding(6);
        _scanLeft.Size = new Size(634, 135);
        _scanLeft.TabIndex = 0;
        // 
        // _lblScanPrompt
        // 
        _lblScanPrompt.AutoSize = true;
        _lblScanPrompt.Font = new Font("Segoe UI", 9F);
        _lblScanPrompt.Location = new Point(9, 6);
        _lblScanPrompt.Name = "_lblScanPrompt";
        _lblScanPrompt.Size = new Size(311, 20);
        _lblScanPrompt.TabIndex = 0;
        _lblScanPrompt.Text = "請掃入條碼 (1D 料號/批號 或 2D Reel 標籤):";
        // 
        // _inputRow
        // 
        _inputRow.Controls.Add(_txtBarcodeScan);
        _inputRow.Controls.Add(_btnSubmitScan);
        _inputRow.Location = new Point(9, 29);
        _inputRow.Name = "_inputRow";
        _inputRow.Size = new Size(550, 35);
        _inputRow.TabIndex = 1;
        // 
        // _txtBarcodeScan
        // 
        _txtBarcodeScan.Font = new Font("Consolas", 11F);
        _txtBarcodeScan.Location = new Point(3, 3);
        _txtBarcodeScan.Name = "_txtBarcodeScan";
        _txtBarcodeScan.Size = new Size(400, 29);
        _txtBarcodeScan.TabIndex = 0;
        _txtBarcodeScan.Text = "{{{{{86{{{1ODKESE9QQQQEQQQS";
        // 
        // _btnSubmitScan
        // 
        _btnSubmitScan.BackColor = Color.FromArgb(46, 204, 113);
        _btnSubmitScan.FlatStyle = FlatStyle.Flat;
        _btnSubmitScan.ForeColor = Color.White;
        _btnSubmitScan.Location = new Point(409, 3);
        _btnSubmitScan.Name = "_btnSubmitScan";
        _btnSubmitScan.Size = new Size(80, 28);
        _btnSubmitScan.TabIndex = 1;
        _btnSubmitScan.Text = "驗證 ⏎";
        _btnSubmitScan.UseVisualStyleBackColor = false;
        // 
        // _testButtonsRow
        // 
        _testButtonsRow.Controls.Add(_btnTestScanMatNo);
        _testButtonsRow.Controls.Add(_btnTestScanBatchNo);
        _testButtonsRow.Controls.Add(_btnTestScanLegalReel);
        _testButtonsRow.Controls.Add(_btnTestScanIllegalReel);
        _testButtonsRow.Location = new Point(6, 71);
        _testButtonsRow.Margin = new Padding(0, 4, 0, 0);
        _testButtonsRow.Name = "_testButtonsRow";
        _testButtonsRow.Size = new Size(600, 38);
        _testButtonsRow.TabIndex = 2;
        // 
        // _btnTestScanMatNo
        // 
        _btnTestScanMatNo.Location = new Point(3, 3);
        _btnTestScanMatNo.Name = "_btnTestScanMatNo";
        _btnTestScanMatNo.Size = new Size(100, 30);
        _btnTestScanMatNo.TabIndex = 0;
        _btnTestScanMatNo.Text = "刷 1D 料號";
        _btnTestScanMatNo.UseVisualStyleBackColor = true;
        // 
        // _btnTestScanBatchNo
        // 
        _btnTestScanBatchNo.Location = new Point(109, 3);
        _btnTestScanBatchNo.Name = "_btnTestScanBatchNo";
        _btnTestScanBatchNo.Size = new Size(100, 30);
        _btnTestScanBatchNo.TabIndex = 1;
        _btnTestScanBatchNo.Text = "刷 1D 批號";
        _btnTestScanBatchNo.UseVisualStyleBackColor = true;
        // 
        // _btnTestScanLegalReel
        // 
        _btnTestScanLegalReel.BackColor = Color.FromArgb(230, 247, 235);
        _btnTestScanLegalReel.Location = new Point(215, 3);
        _btnTestScanLegalReel.Name = "_btnTestScanLegalReel";
        _btnTestScanLegalReel.Size = new Size(115, 30);
        _btnTestScanLegalReel.TabIndex = 2;
        _btnTestScanLegalReel.Text = "刷 合法 Reel";
        _btnTestScanLegalReel.UseVisualStyleBackColor = false;
        // 
        // _btnTestScanIllegalReel
        // 
        _btnTestScanIllegalReel.BackColor = Color.FromArgb(255, 235, 235);
        _btnTestScanIllegalReel.ForeColor = Color.DarkRed;
        _btnTestScanIllegalReel.Location = new Point(336, 3);
        _btnTestScanIllegalReel.Name = "_btnTestScanIllegalReel";
        _btnTestScanIllegalReel.Size = new Size(185, 30);
        _btnTestScanIllegalReel.TabIndex = 3;
        _btnTestScanIllegalReel.Text = "刷 跨工單 Reel (NG-0401)";
        _btnTestScanIllegalReel.UseVisualStyleBackColor = false;
        // 
        // _pnlStatusLight
        // 
        _pnlStatusLight.BackColor = Color.FromArgb(230, 230, 230);
        _pnlStatusLight.Controls.Add(_lblDecodedReelId);
        _pnlStatusLight.Controls.Add(_lblStatusDetail);
        _pnlStatusLight.Controls.Add(_lblStatusText);
        _pnlStatusLight.Dock = DockStyle.Fill;
        _pnlStatusLight.Location = new Point(643, 3);
        _pnlStatusLight.Name = "_pnlStatusLight";
        _pnlStatusLight.Padding = new Padding(10);
        _pnlStatusLight.Size = new Size(518, 135);
        _pnlStatusLight.TabIndex = 1;
        // 
        // _lblDecodedReelId
        // 
        _lblDecodedReelId.Dock = DockStyle.Top;
        _lblDecodedReelId.Font = new Font("Consolas", 10.5F, FontStyle.Bold);
        _lblDecodedReelId.ForeColor = Color.FromArgb(40, 40, 40);
        _lblDecodedReelId.Location = new Point(10, 70);
        _lblDecodedReelId.Name = "_lblDecodedReelId";
        _lblDecodedReelId.Size = new Size(498, 25);
        _lblDecodedReelId.TabIndex = 2;
        _lblDecodedReelId.Text = "還原 Reel ID: ---";
        _lblDecodedReelId.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _lblStatusDetail
        // 
        _lblStatusDetail.Dock = DockStyle.Top;
        _lblStatusDetail.Font = new Font("Segoe UI", 10F);
        _lblStatusDetail.ForeColor = Color.FromArgb(60, 60, 60);
        _lblStatusDetail.Location = new Point(10, 45);
        _lblStatusDetail.Name = "_lblStatusDetail";
        _lblStatusDetail.Size = new Size(498, 25);
        _lblStatusDetail.TabIndex = 1;
        _lblStatusDetail.Text = "請點擊 [開工初始化] 載入工單";
        _lblStatusDetail.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _lblStatusText
        // 
        _lblStatusText.Dock = DockStyle.Top;
        _lblStatusText.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        _lblStatusText.ForeColor = Color.DimGray;
        _lblStatusText.Location = new Point(10, 10);
        _lblStatusText.Name = "_lblStatusText";
        _lblStatusText.Size = new Size(498, 35);
        _lblStatusText.TabIndex = 0;
        _lblStatusText.Text = "STANDBY 等待開工";
        _lblStatusText.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _grpGrid
        // 
        _grpGrid.Controls.Add(_dgvReels);
        _grpGrid.Dock = DockStyle.Fill;
        _grpGrid.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _grpGrid.Location = new Point(15, 300);
        _grpGrid.Name = "_grpGrid";
        _grpGrid.Size = new Size(1170, 291);
        _grpGrid.TabIndex = 2;
        _grpGrid.TabStop = false;
        _grpGrid.Text = "📋 工單料捲 (Reel) 狀態清單";
        // 
        // _dgvReels
        // 
        _dgvReels.AllowUserToAddRows = false;
        _dgvReels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _dgvReels.BackgroundColor = Color.White;
        _dgvReels.BorderStyle = BorderStyle.None;
        _dgvReels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _dgvReels.Columns.AddRange(new DataGridViewColumn[] { _colReelId, _colStatus, _colMachNo, _colStartTime, _colReelScanCount, _colMatScanCount, _colTPScanCount });
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Window;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
        _dgvReels.DefaultCellStyle = dataGridViewCellStyle1;
        _dgvReels.Dock = DockStyle.Fill;
        _dgvReels.Location = new Point(3, 25);
        _dgvReels.Name = "_dgvReels";
        _dgvReels.ReadOnly = true;
        _dgvReels.RowHeadersVisible = false;
        _dgvReels.RowHeadersWidth = 51;
        _dgvReels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvReels.Size = new Size(1164, 263);
        _dgvReels.TabIndex = 0;
        // 
        // _colReelId
        // 
        _colReelId.HeaderText = "Reel ID (料捲編號)";
        _colReelId.MinimumWidth = 6;
        _colReelId.Name = "_colReelId";
        _colReelId.ReadOnly = true;
        // 
        // _colStatus
        // 
        _colStatus.HeaderText = "狀態";
        _colStatus.MinimumWidth = 6;
        _colStatus.Name = "_colStatus";
        _colStatus.ReadOnly = true;
        // 
        // _colMachNo
        // 
        _colMachNo.HeaderText = "綁定機台";
        _colMachNo.MinimumWidth = 6;
        _colMachNo.Name = "_colMachNo";
        _colMachNo.ReadOnly = true;
        // 
        // _colStartTime
        // 
        _colStartTime.HeaderText = "上機起始時間";
        _colStartTime.MinimumWidth = 6;
        _colStartTime.Name = "_colStartTime";
        _colStartTime.ReadOnly = true;
        // 
        // _colReelScanCount
        // 
        _colReelScanCount.HeaderText = "Reel 刷次";
        _colReelScanCount.MinimumWidth = 6;
        _colReelScanCount.Name = "_colReelScanCount";
        _colReelScanCount.ReadOnly = true;
        // 
        // _colMatScanCount
        // 
        _colMatScanCount.HeaderText = "料號刷次";
        _colMatScanCount.MinimumWidth = 6;
        _colMatScanCount.Name = "_colMatScanCount";
        _colMatScanCount.ReadOnly = true;
        // 
        // _colTPScanCount
        // 
        _colTPScanCount.HeaderText = "批號刷次";
        _colTPScanCount.MinimumWidth = 6;
        _colTPScanCount.Name = "_colTPScanCount";
        _colTPScanCount.ReadOnly = true;
        // 
        // _grpLog
        // 
        _grpLog.Controls.Add(_rtbLog);
        _grpLog.Dock = DockStyle.Fill;
        _grpLog.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _grpLog.Location = new Point(15, 597);
        _grpLog.Name = "_grpLog";
        _grpLog.Size = new Size(1170, 238);
        _grpLog.TabIndex = 3;
        _grpLog.TabStop = false;
        _grpLog.Text = "📜 即時操作與通訊歷程 (Log)";
        // 
        // _rtbLog
        // 
        _rtbLog.BackColor = Color.FromArgb(30, 30, 30);
        _rtbLog.BorderStyle = BorderStyle.None;
        _rtbLog.Dock = DockStyle.Fill;
        _rtbLog.Font = new Font("Consolas", 9F);
        _rtbLog.ForeColor = Color.FromArgb(220, 220, 220);
        _rtbLog.Location = new Point(3, 25);
        _rtbLog.Name = "_rtbLog";
        _rtbLog.ReadOnly = true;
        _rtbLog.Size = new Size(1164, 210);
        _rtbLog.TabIndex = 0;
        _rtbLog.Text = "";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1200, 850);
        Controls.Add(_mainLayout);
        Font = new Font("Segoe UI", 9.5F);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EAP 標籤防混檢驗展示系統 (EAP.ReelCheck Demo)";
        _mainLayout.ResumeLayout(false);
        _grpWo.ResumeLayout(false);
        _woPanel.ResumeLayout(false);
        _woPanel.PerformLayout();
        _woDetailPanel.ResumeLayout(false);
        _woDetailPanel.PerformLayout();
        _grpScan.ResumeLayout(false);
        _scanLayout.ResumeLayout(false);
        _scanLeft.ResumeLayout(false);
        _scanLeft.PerformLayout();
        _inputRow.ResumeLayout(false);
        _inputRow.PerformLayout();
        _testButtonsRow.ResumeLayout(false);
        _pnlStatusLight.ResumeLayout(false);
        _grpGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_dgvReels).EndInit();
        _grpLog.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel _mainLayout;
    private System.Windows.Forms.GroupBox _grpWo;
    private System.Windows.Forms.FlowLayoutPanel _woPanel;
    private System.Windows.Forms.Label _lblMachineNo;
    private System.Windows.Forms.TextBox _txtMachineNo;
    private System.Windows.Forms.Label _lblWoNo;
    private System.Windows.Forms.TextBox _txtWoNo;
    private System.Windows.Forms.Label _lblUserId;
    private System.Windows.Forms.TextBox _txtUserId;
    private System.Windows.Forms.Button _btnInitWorkOrder;
    private System.Windows.Forms.CheckBox _chkSimulateOffline;
    private System.Windows.Forms.FlowLayoutPanel _woDetailPanel;
    private System.Windows.Forms.Label _lblMatNo;
    private System.Windows.Forms.TextBox _txtMatNo;
    private System.Windows.Forms.Label _lblBatchNo;
    private System.Windows.Forms.TextBox _txtBatchNo;
    private System.Windows.Forms.GroupBox _grpScan;
    private System.Windows.Forms.TableLayoutPanel _scanLayout;
    private System.Windows.Forms.FlowLayoutPanel _scanLeft;
    private System.Windows.Forms.Label _lblScanPrompt;
    private System.Windows.Forms.FlowLayoutPanel _inputRow;
    private System.Windows.Forms.TextBox _txtBarcodeScan;
    private System.Windows.Forms.Button _btnSubmitScan;
    private System.Windows.Forms.FlowLayoutPanel _testButtonsRow;
    private System.Windows.Forms.Button _btnTestScanMatNo;
    private System.Windows.Forms.Button _btnTestScanBatchNo;
    private System.Windows.Forms.Button _btnTestScanLegalReel;
    private System.Windows.Forms.Button _btnTestScanIllegalReel;
    private System.Windows.Forms.Panel _pnlStatusLight;
    private System.Windows.Forms.Label _lblStatusText;
    private System.Windows.Forms.Label _lblStatusDetail;
    private System.Windows.Forms.Label _lblDecodedReelId;
    private System.Windows.Forms.GroupBox _grpGrid;
    private System.Windows.Forms.DataGridView _dgvReels;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colReelId;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colMachNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colStartTime;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colReelScanCount;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colMatScanCount;
    private System.Windows.Forms.DataGridViewTextBoxColumn _colTPScanCount;
    private System.Windows.Forms.GroupBox _grpLog;
    private System.Windows.Forms.RichTextBox _rtbLog;
}
