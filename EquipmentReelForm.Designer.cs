namespace EAPDashboard;

partial class EquipmentReelForm
{
    private System.ComponentModel.IContainer components = null;

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
    private System.Windows.Forms.Button _btnFinishWorkOrder;
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
    private System.Windows.Forms.Label _lblDecodedReelId;
    private System.Windows.Forms.Label _lblStatusDetail;
    private System.Windows.Forms.Label _lblStatusText;
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
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        _mainLayout = new System.Windows.Forms.TableLayoutPanel();
        _grpWo = new System.Windows.Forms.GroupBox();
        _woPanel = new System.Windows.Forms.FlowLayoutPanel();
        _lblMachineNo = new System.Windows.Forms.Label();
        _txtMachineNo = new System.Windows.Forms.TextBox();
        _lblWoNo = new System.Windows.Forms.Label();
        _txtWoNo = new System.Windows.Forms.TextBox();
        _lblUserId = new System.Windows.Forms.Label();
        _txtUserId = new System.Windows.Forms.TextBox();
        _btnInitWorkOrder = new System.Windows.Forms.Button();
        _btnFinishWorkOrder = new System.Windows.Forms.Button();
        _chkSimulateOffline = new System.Windows.Forms.CheckBox();
        _woDetailPanel = new System.Windows.Forms.FlowLayoutPanel();
        _lblMatNo = new System.Windows.Forms.Label();
        _txtMatNo = new System.Windows.Forms.TextBox();
        _lblBatchNo = new System.Windows.Forms.Label();
        _txtBatchNo = new System.Windows.Forms.TextBox();
        _grpScan = new System.Windows.Forms.GroupBox();
        _scanLayout = new System.Windows.Forms.TableLayoutPanel();
        _scanLeft = new System.Windows.Forms.FlowLayoutPanel();
        _lblScanPrompt = new System.Windows.Forms.Label();
        _inputRow = new System.Windows.Forms.FlowLayoutPanel();
        _txtBarcodeScan = new System.Windows.Forms.TextBox();
        _btnSubmitScan = new System.Windows.Forms.Button();
        _testButtonsRow = new System.Windows.Forms.FlowLayoutPanel();
        _btnTestScanMatNo = new System.Windows.Forms.Button();
        _btnTestScanBatchNo = new System.Windows.Forms.Button();
        _btnTestScanLegalReel = new System.Windows.Forms.Button();
        _btnTestScanIllegalReel = new System.Windows.Forms.Button();
        _pnlStatusLight = new System.Windows.Forms.Panel();
        _lblDecodedReelId = new System.Windows.Forms.Label();
        _lblStatusDetail = new System.Windows.Forms.Label();
        _lblStatusText = new System.Windows.Forms.Label();
        _grpGrid = new System.Windows.Forms.GroupBox();
        _dgvReels = new System.Windows.Forms.DataGridView();
        _colReelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colMachNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colReelScanCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colMatScanCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _colTPScanCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        _grpLog = new System.Windows.Forms.GroupBox();
        _rtbLog = new System.Windows.Forms.RichTextBox();
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
        _mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        _mainLayout.Controls.Add(_grpWo, 0, 0);
        _mainLayout.Controls.Add(_grpScan, 0, 1);
        _mainLayout.Controls.Add(_grpGrid, 0, 2);
        _mainLayout.Controls.Add(_grpLog, 0, 3);
        _mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        _mainLayout.Location = new System.Drawing.Point(0, 0);
        _mainLayout.Name = "_mainLayout";
        _mainLayout.RowCount = 4;
        _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
        _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
        _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
        _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
        _mainLayout.Size = new System.Drawing.Size(960, 720);
        _mainLayout.TabIndex = 0;
        // 
        // _grpWo
        // 
        _grpWo.Controls.Add(_woPanel);
        _grpWo.Controls.Add(_woDetailPanel);
        _grpWo.Dock = System.Windows.Forms.DockStyle.Fill;
        _grpWo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        _grpWo.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
        _grpWo.Location = new System.Drawing.Point(3, 3);
        _grpWo.Name = "_grpWo";
        _grpWo.Size = new System.Drawing.Size(954, 109);
        _grpWo.TabIndex = 0;
        _grpWo.TabStop = false;
        _grpWo.Text = "工單與機台資訊 (Work Order Info)";
        // 
        // _woPanel
        // 
        _woPanel.Controls.Add(_lblMachineNo);
        _woPanel.Controls.Add(_txtMachineNo);
        _woPanel.Controls.Add(_lblWoNo);
        _woPanel.Controls.Add(_txtWoNo);
        _woPanel.Controls.Add(_lblUserId);
        _woPanel.Controls.Add(_txtUserId);
        _woPanel.Controls.Add(_btnInitWorkOrder);
        _woPanel.Controls.Add(_btnFinishWorkOrder);
        _woPanel.Controls.Add(_chkSimulateOffline);
        _woPanel.Dock = System.Windows.Forms.DockStyle.Top;
        _woPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
        _woPanel.ForeColor = System.Drawing.SystemColors.ControlText;
        _woPanel.Location = new System.Drawing.Point(3, 19);
        _woPanel.Name = "_woPanel";
        _woPanel.Padding = new System.Windows.Forms.Padding(4);
        _woPanel.Size = new System.Drawing.Size(948, 42);
        _woPanel.TabIndex = 0;
        // 
        // _lblMachineNo
        // 
        _lblMachineNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _lblMachineNo.AutoSize = true;
        _lblMachineNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblMachineNo.Location = new System.Drawing.Point(7, 11);
        _lblMachineNo.Name = "_lblMachineNo";
        _lblMachineNo.Size = new System.Drawing.Size(46, 15);
        _lblMachineNo.TabIndex = 0;
        _lblMachineNo.Text = "機台號:";
        // 
        // _txtMachineNo
        // 
        _txtMachineNo.Location = new System.Drawing.Point(59, 7);
        _txtMachineNo.Name = "_txtMachineNo";
        _txtMachineNo.ReadOnly = true;
        _txtMachineNo.Size = new System.Drawing.Size(90, 23);
        _txtMachineNo.TabIndex = 1;
        // 
        // _lblWoNo
        // 
        _lblWoNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _lblWoNo.AutoSize = true;
        _lblWoNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblWoNo.Location = new System.Drawing.Point(155, 11);
        _lblWoNo.Name = "_lblWoNo";
        _lblWoNo.Size = new System.Drawing.Size(59, 15);
        _lblWoNo.TabIndex = 2;
        _lblWoNo.Text = "工單號碼:";
        // 
        // _txtWoNo
        // 
        _txtWoNo.Location = new System.Drawing.Point(220, 7);
        _txtWoNo.Name = "_txtWoNo";
        _txtWoNo.Size = new System.Drawing.Size(140, 23);
        _txtWoNo.TabIndex = 3;
        // 
        // _lblUserId
        // 
        _lblUserId.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _lblUserId.AutoSize = true;
        _lblUserId.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblUserId.Location = new System.Drawing.Point(366, 11);
        _lblUserId.Name = "_lblUserId";
        _lblUserId.Size = new System.Drawing.Size(59, 15);
        _lblUserId.TabIndex = 4;
        _lblUserId.Text = "人員工號:";
        // 
        // _txtUserId
        // 
        _txtUserId.Location = new System.Drawing.Point(431, 7);
        _txtUserId.Name = "_txtUserId";
        _txtUserId.Size = new System.Drawing.Size(90, 23);
        _txtUserId.TabIndex = 5;
        _txtUserId.Text = "OP-01";
        // 
        // _btnInitWorkOrder
        // 
        _btnInitWorkOrder.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
        _btnInitWorkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnInitWorkOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _btnInitWorkOrder.ForeColor = System.Drawing.Color.White;
        _btnInitWorkOrder.Location = new System.Drawing.Point(527, 6);
        _btnInitWorkOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        _btnInitWorkOrder.Name = "_btnInitWorkOrder";
        _btnInitWorkOrder.Size = new System.Drawing.Size(125, 26);
        _btnInitWorkOrder.TabIndex = 6;
        _btnInitWorkOrder.Text = "載入 / 初始化工單";
        _btnInitWorkOrder.UseVisualStyleBackColor = false;
        // 
        // _btnFinishWorkOrder
        // 
        _btnFinishWorkOrder.BackColor = System.Drawing.Color.FromArgb(142, 68, 173);
        _btnFinishWorkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnFinishWorkOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _btnFinishWorkOrder.ForeColor = System.Drawing.Color.White;
        _btnFinishWorkOrder.Location = new System.Drawing.Point(658, 6);
        _btnFinishWorkOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        _btnFinishWorkOrder.Name = "_btnFinishWorkOrder";
        _btnFinishWorkOrder.Size = new System.Drawing.Size(105, 26);
        _btnFinishWorkOrder.TabIndex = 7;
        _btnFinishWorkOrder.Text = "🏁 工單結束";
        _btnFinishWorkOrder.UseVisualStyleBackColor = false;
        // 
        // _chkSimulateOffline
        // 
        _chkSimulateOffline.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _chkSimulateOffline.AutoSize = true;
        _chkSimulateOffline.Location = new System.Drawing.Point(771, 9);
        _chkSimulateOffline.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
        _chkSimulateOffline.Name = "_chkSimulateOffline";
        _chkSimulateOffline.Size = new System.Drawing.Size(98, 19);
        _chkSimulateOffline.TabIndex = 8;
        _chkSimulateOffline.Text = "離線測試模式";
        _chkSimulateOffline.UseVisualStyleBackColor = true;
        // 
        // _woDetailPanel
        // 
        _woDetailPanel.Controls.Add(_lblMatNo);
        _woDetailPanel.Controls.Add(_txtMatNo);
        _woDetailPanel.Controls.Add(_lblBatchNo);
        _woDetailPanel.Controls.Add(_txtBatchNo);
        _woDetailPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
        _woDetailPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
        _woDetailPanel.ForeColor = System.Drawing.SystemColors.ControlText;
        _woDetailPanel.Location = new System.Drawing.Point(3, 67);
        _woDetailPanel.Name = "_woDetailPanel";
        _woDetailPanel.Padding = new System.Windows.Forms.Padding(4);
        _woDetailPanel.Size = new System.Drawing.Size(948, 39);
        _woDetailPanel.TabIndex = 1;
        // 
        // _lblMatNo
        // 
        _lblMatNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _lblMatNo.AutoSize = true;
        _lblMatNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblMatNo.Location = new System.Drawing.Point(7, 10);
        _lblMatNo.Name = "_lblMatNo";
        _lblMatNo.Size = new System.Drawing.Size(59, 15);
        _lblMatNo.TabIndex = 0;
        _lblMatNo.Text = "主料料號:";
        // 
        // _txtMatNo
        // 
        _txtMatNo.BackColor = System.Drawing.Color.WhiteSmoke;
        _txtMatNo.Location = new System.Drawing.Point(72, 7);
        _txtMatNo.Name = "_txtMatNo";
        _txtMatNo.ReadOnly = true;
        _txtMatNo.Size = new System.Drawing.Size(180, 23);
        _txtMatNo.TabIndex = 1;
        // 
        // _lblBatchNo
        // 
        _lblBatchNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
        _lblBatchNo.AutoSize = true;
        _lblBatchNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblBatchNo.Location = new System.Drawing.Point(258, 10);
        _lblBatchNo.Name = "_lblBatchNo";
        _lblBatchNo.Size = new System.Drawing.Size(83, 15);
        _lblBatchNo.TabIndex = 2;
        _lblBatchNo.Text = "工單/捲料批號:";
        // 
        // _txtBatchNo
        // 
        _txtBatchNo.BackColor = System.Drawing.Color.WhiteSmoke;
        _txtBatchNo.Location = new System.Drawing.Point(347, 7);
        _txtBatchNo.Name = "_txtBatchNo";
        _txtBatchNo.ReadOnly = true;
        _txtBatchNo.Size = new System.Drawing.Size(180, 23);
        _txtBatchNo.TabIndex = 3;
        // 
        // _grpScan
        // 
        _grpScan.Controls.Add(_scanLayout);
        _grpScan.Dock = System.Windows.Forms.DockStyle.Fill;
        _grpScan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        _grpScan.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
        _grpScan.Location = new System.Drawing.Point(3, 118);
        _grpScan.Name = "_grpScan";
        _grpScan.Size = new System.Drawing.Size(954, 134);
        _grpScan.TabIndex = 1;
        _grpScan.TabStop = false;
        _grpScan.Text = "條碼掃描與即時防呆判定 (Barcode Verification)";
        // 
        // _scanLayout
        // 
        _scanLayout.ColumnCount = 2;
        _scanLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
        _scanLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
        _scanLayout.Controls.Add(_scanLeft, 0, 0);
        _scanLayout.Controls.Add(_pnlStatusLight, 1, 0);
        _scanLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        _scanLayout.Location = new System.Drawing.Point(3, 19);
        _scanLayout.Name = "_scanLayout";
        _scanLayout.RowCount = 1;
        _scanLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        _scanLayout.Size = new System.Drawing.Size(948, 112);
        _scanLayout.TabIndex = 0;
        // 
        // _scanLeft
        // 
        _scanLeft.Controls.Add(_lblScanPrompt);
        _scanLeft.Controls.Add(_inputRow);
        _scanLeft.Controls.Add(_testButtonsRow);
        _scanLeft.Dock = System.Windows.Forms.DockStyle.Fill;
        _scanLeft.Font = new System.Drawing.Font("Segoe UI", 9F);
        _scanLeft.ForeColor = System.Drawing.SystemColors.ControlText;
        _scanLeft.Location = new System.Drawing.Point(3, 3);
        _scanLeft.Name = "_scanLeft";
        _scanLeft.Size = new System.Drawing.Size(562, 106);
        _scanLeft.TabIndex = 0;
        // 
        // _lblScanPrompt
        // 
        _lblScanPrompt.AutoSize = true;
        _lblScanPrompt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _lblScanPrompt.Location = new System.Drawing.Point(3, 0);
        _lblScanPrompt.Name = "_lblScanPrompt";
        _lblScanPrompt.Size = new System.Drawing.Size(262, 15);
        _lblScanPrompt.TabIndex = 0;
        _lblScanPrompt.Text = "請掃描條碼 (支援 1D 料號/批號 或 2D Cyntec Reel):";
        // 
        // _inputRow
        // 
        _inputRow.Controls.Add(_txtBarcodeScan);
        _inputRow.Controls.Add(_btnSubmitScan);
        _inputRow.Location = new System.Drawing.Point(3, 18);
        _inputRow.Name = "_inputRow";
        _inputRow.Size = new System.Drawing.Size(550, 36);
        _inputRow.TabIndex = 1;
        // 
        // _txtBarcodeScan
        // 
        _txtBarcodeScan.Font = new System.Drawing.Font("Consolas", 11F);
        _txtBarcodeScan.Location = new System.Drawing.Point(3, 3);
        _txtBarcodeScan.Name = "_txtBarcodeScan";
        _txtBarcodeScan.Size = new System.Drawing.Size(430, 25);
        _txtBarcodeScan.TabIndex = 0;
        // 
        // _btnSubmitScan
        // 
        _btnSubmitScan.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
        _btnSubmitScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        _btnSubmitScan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        _btnSubmitScan.ForeColor = System.Drawing.Color.White;
        _btnSubmitScan.Location = new System.Drawing.Point(439, 2);
        _btnSubmitScan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        _btnSubmitScan.Name = "_btnSubmitScan";
        _btnSubmitScan.Size = new System.Drawing.Size(85, 27);
        _btnSubmitScan.TabIndex = 1;
        _btnSubmitScan.Text = "檢驗 (Scan)";
        _btnSubmitScan.UseVisualStyleBackColor = false;
        // 
        // _testButtonsRow
        // 
        _testButtonsRow.Controls.Add(_btnTestScanMatNo);
        _testButtonsRow.Controls.Add(_btnTestScanBatchNo);
        _testButtonsRow.Controls.Add(_btnTestScanLegalReel);
        _testButtonsRow.Controls.Add(_btnTestScanIllegalReel);
        _testButtonsRow.Location = new System.Drawing.Point(3, 60);
        _testButtonsRow.Name = "_testButtonsRow";
        _testButtonsRow.Size = new System.Drawing.Size(550, 32);
        _testButtonsRow.TabIndex = 2;
        // 
        // _btnTestScanMatNo
        // 
        _btnTestScanMatNo.Font = new System.Drawing.Font("Segoe UI", 8F);
        _btnTestScanMatNo.Location = new System.Drawing.Point(2, 2);
        _btnTestScanMatNo.Margin = new System.Windows.Forms.Padding(2);
        _btnTestScanMatNo.Name = "_btnTestScanMatNo";
        _btnTestScanMatNo.Size = new System.Drawing.Size(95, 26);
        _btnTestScanMatNo.TabIndex = 0;
        _btnTestScanMatNo.Text = "填入主料號";
        _btnTestScanMatNo.UseVisualStyleBackColor = true;
        // 
        // _btnTestScanBatchNo
        // 
        _btnTestScanBatchNo.Font = new System.Drawing.Font("Segoe UI", 8F);
        _btnTestScanBatchNo.Location = new System.Drawing.Point(101, 2);
        _btnTestScanBatchNo.Margin = new System.Windows.Forms.Padding(2);
        _btnTestScanBatchNo.Name = "_btnTestScanBatchNo";
        _btnTestScanBatchNo.Size = new System.Drawing.Size(95, 26);
        _btnTestScanBatchNo.TabIndex = 1;
        _btnTestScanBatchNo.Text = "填入主批號";
        _btnTestScanBatchNo.UseVisualStyleBackColor = true;
        // 
        // _btnTestScanLegalReel
        // 
        _btnTestScanLegalReel.Font = new System.Drawing.Font("Segoe UI", 8F);
        _btnTestScanLegalReel.Location = new System.Drawing.Point(200, 2);
        _btnTestScanLegalReel.Margin = new System.Windows.Forms.Padding(2);
        _btnTestScanLegalReel.Name = "_btnTestScanLegalReel";
        _btnTestScanLegalReel.Size = new System.Drawing.Size(105, 26);
        _btnTestScanLegalReel.TabIndex = 2;
        _btnTestScanLegalReel.Text = "填入合法 2D Reel";
        _btnTestScanLegalReel.UseVisualStyleBackColor = true;
        // 
        // _btnTestScanIllegalReel
        // 
        _btnTestScanIllegalReel.Font = new System.Drawing.Font("Segoe UI", 8F);
        _btnTestScanIllegalReel.ForeColor = System.Drawing.Color.Maroon;
        _btnTestScanIllegalReel.Location = new System.Drawing.Point(309, 2);
        _btnTestScanIllegalReel.Margin = new System.Windows.Forms.Padding(2);
        _btnTestScanIllegalReel.Name = "_btnTestScanIllegalReel";
        _btnTestScanIllegalReel.Size = new System.Drawing.Size(115, 26);
        _btnTestScanIllegalReel.TabIndex = 3;
        _btnTestScanIllegalReel.Text = "填入非法 2D Reel";
        _btnTestScanIllegalReel.UseVisualStyleBackColor = true;
        // 
        // _pnlStatusLight
        // 
        _pnlStatusLight.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
        _pnlStatusLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        _pnlStatusLight.Controls.Add(_lblDecodedReelId);
        _pnlStatusLight.Controls.Add(_lblStatusDetail);
        _pnlStatusLight.Controls.Add(_lblStatusText);
        _pnlStatusLight.Dock = System.Windows.Forms.DockStyle.Fill;
        _pnlStatusLight.Location = new System.Drawing.Point(571, 3);
        _pnlStatusLight.Name = "_pnlStatusLight";
        _pnlStatusLight.Padding = new System.Windows.Forms.Padding(8);
        _pnlStatusLight.Size = new System.Drawing.Size(374, 106);
        _pnlStatusLight.TabIndex = 1;
        // 
        // _lblDecodedReelId
        // 
        _lblDecodedReelId.Dock = System.Windows.Forms.DockStyle.Bottom;
        _lblDecodedReelId.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
        _lblDecodedReelId.ForeColor = System.Drawing.Color.White;
        _lblDecodedReelId.Location = new System.Drawing.Point(8, 80);
        _lblDecodedReelId.Name = "_lblDecodedReelId";
        _lblDecodedReelId.Size = new System.Drawing.Size(356, 16);
        _lblDecodedReelId.TabIndex = 2;
        _lblDecodedReelId.Text = "還原 Reel ID: ---";
        // 
        // _lblStatusDetail
        // 
        _lblStatusDetail.Dock = System.Windows.Forms.DockStyle.Fill;
        _lblStatusDetail.Font = new System.Drawing.Font("Segoe UI", 9F);
        _lblStatusDetail.ForeColor = System.Drawing.Color.White;
        _lblStatusDetail.Location = new System.Drawing.Point(8, 38);
        _lblStatusDetail.Name = "_lblStatusDetail";
        _lblStatusDetail.Size = new System.Drawing.Size(356, 58);
        _lblStatusDetail.TabIndex = 1;
        _lblStatusDetail.Text = "尚未開始掃描";
        // 
        // _lblStatusText
        // 
        _lblStatusText.Dock = System.Windows.Forms.DockStyle.Top;
        _lblStatusText.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold);
        _lblStatusText.ForeColor = System.Drawing.Color.White;
        _lblStatusText.Location = new System.Drawing.Point(8, 8);
        _lblStatusText.Name = "_lblStatusText";
        _lblStatusText.Size = new System.Drawing.Size(356, 30);
        _lblStatusText.TabIndex = 0;
        _lblStatusText.Text = "IDLE";
        // 
        // _grpGrid
        // 
        _grpGrid.Controls.Add(_dgvReels);
        _grpGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        _grpGrid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        _grpGrid.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
        _grpGrid.Location = new System.Drawing.Point(3, 258);
        _grpGrid.Name = "_grpGrid";
        _grpGrid.Size = new System.Drawing.Size(954, 253);
        _grpGrid.TabIndex = 2;
        _grpGrid.TabStop = false;
        _grpGrid.Text = "目前設備綁定 Reel 清單即時狀態 (Reel Status List)";
        // 
        // _dgvReels
        // 
        _dgvReels.AllowUserToAddRows = false;
        _dgvReels.AllowUserToDeleteRows = false;
        _dgvReels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        _dgvReels.BackgroundColor = System.Drawing.Color.White;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        _dgvReels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        _dgvReels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _dgvReels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            _colReelId,
            _colStatus,
            _colMachNo,
            _colStartTime,
            _colReelScanCount,
            _colMatScanCount,
            _colTPScanCount
        });
        _dgvReels.Dock = System.Windows.Forms.DockStyle.Fill;
        _dgvReels.Font = new System.Drawing.Font("Segoe UI", 9F);
        _dgvReels.ForeColor = System.Drawing.Color.Black;
        _dgvReels.Location = new System.Drawing.Point(3, 19);
        _dgvReels.Name = "_dgvReels";
        _dgvReels.ReadOnly = true;
        _dgvReels.RowHeadersWidth = 30;
        _dgvReels.RowTemplate.Height = 25;
        _dgvReels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        _dgvReels.Size = new System.Drawing.Size(948, 231);
        _dgvReels.TabIndex = 0;
        // 
        // _colReelId
        // 
        _colReelId.FillWeight = 120F;
        _colReelId.HeaderText = "Reel ID";
        _colReelId.Name = "_colReelId";
        _colReelId.ReadOnly = true;
        // 
        // _colStatus
        // 
        _colStatus.FillWeight = 80F;
        _colStatus.HeaderText = "狀態 (Status)";
        _colStatus.Name = "_colStatus";
        _colStatus.ReadOnly = true;
        // 
        // _colMachNo
        // 
        _colMachNo.FillWeight = 80F;
        _colMachNo.HeaderText = "綁定機台";
        _colMachNo.Name = "_colMachNo";
        _colMachNo.ReadOnly = true;
        // 
        // _colStartTime
        // 
        _colStartTime.FillWeight = 90F;
        _colStartTime.HeaderText = "上機時間";
        _colStartTime.Name = "_colStartTime";
        _colStartTime.ReadOnly = true;
        // 
        // _colReelScanCount
        // 
        _colReelScanCount.FillWeight = 60F;
        _colReelScanCount.HeaderText = "2D次數";
        _colReelScanCount.Name = "_colReelScanCount";
        _colReelScanCount.ReadOnly = true;
        // 
        // _colMatScanCount
        // 
        _colMatScanCount.FillWeight = 60F;
        _colMatScanCount.HeaderText = "料號防呆";
        _colMatScanCount.Name = "_colMatScanCount";
        _colMatScanCount.ReadOnly = true;
        // 
        // _colTPScanCount
        // 
        _colTPScanCount.FillWeight = 60F;
        _colTPScanCount.HeaderText = "批號防呆";
        _colTPScanCount.Name = "_colTPScanCount";
        _colTPScanCount.ReadOnly = true;
        // 
        // _grpLog
        // 
        _grpLog.Controls.Add(_rtbLog);
        _grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
        _grpLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        _grpLog.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
        _grpLog.Location = new System.Drawing.Point(3, 517);
        _grpLog.Name = "_grpLog";
        _grpLog.Size = new System.Drawing.Size(954, 200);
        _grpLog.TabIndex = 3;
        _grpLog.TabStop = false;
        _grpLog.Text = "防混料即時日誌歷程 (Log Events)";
        // 
        // _rtbLog
        // 
        _rtbLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        _rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
        _rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
        _rtbLog.Font = new System.Drawing.Font("Consolas", 9.5F);
        _rtbLog.ForeColor = System.Drawing.Color.Gainsboro;
        _rtbLog.Location = new System.Drawing.Point(3, 19);
        _rtbLog.Name = "_rtbLog";
        _rtbLog.ReadOnly = true;
        _rtbLog.Size = new System.Drawing.Size(948, 178);
        _rtbLog.TabIndex = 0;
        _rtbLog.Text = "";
        // 
        // EquipmentReelForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
        ClientSize = new System.Drawing.Size(960, 720);
        Controls.Add(_mainLayout);
        Font = new System.Drawing.Font("Segoe UI", 9F);
        MinimumSize = new System.Drawing.Size(800, 600);
        Name = "EquipmentReelForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "TP 防混料監控管理";
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
}
