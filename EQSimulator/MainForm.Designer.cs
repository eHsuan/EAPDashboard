namespace EQSimulator
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblBroker = new System.Windows.Forms.Label();
            this._txtBroker = new System.Windows.Forms.TextBox();
            this.lblEqId = new System.Windows.Forms.Label();
            this._txtEqId = new System.Windows.Forms.TextBox();
            this.lblBaseTopic = new System.Windows.Forms.Label();
            this._txtTopic = new System.Windows.Forms.TextBox();
            this._btnConnect = new System.Windows.Forms.Button();
            this.grpParams = new System.Windows.Forms.GroupBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this._txtBarcode = new System.Windows.Forms.TextBox();
            this.lblQty = new System.Windows.Forms.Label();
            this._txtQty = new System.Windows.Forms.TextBox();
            this.lblToolingId = new System.Windows.Forms.Label();
            this._txtToolingId = new System.Windows.Forms.TextBox();
            this.lblCassetteId = new System.Windows.Forms.Label();
            this._txtCassetteId = new System.Windows.Forms.TextBox();
            this.lblMaterialId = new System.Windows.Forms.Label();
            this._txtMaterialId = new System.Windows.Forms.TextBox();
            this.lblWorkOrder = new System.Windows.Forms.Label();
            this._txtWorkOrder = new System.Windows.Forms.TextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this._txtUserId = new System.Windows.Forms.TextBox();
            this._grpActions = new System.Windows.Forms.GroupBox();
            this.flowActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnTrackIn = new System.Windows.Forms.Button();
            this.btnTrackOut = new System.Windows.Forms.Button();
            this.btnPanelIn = new System.Windows.Forms.Button();
            this.btnPanelOut = new System.Windows.Forms.Button();
            this.btnRtnTraceData = new System.Windows.Forms.Button();
            this.btnAlarm = new System.Windows.Forms.Button();
            this.pnlStatusReport = new System.Windows.Forms.Panel();
            this._cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnReportStatus = new System.Windows.Forms.Button();
            this._txtLog = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            this.grpParams.SuspendLayout();
            this._grpActions.SuspendLayout();
            this.flowActions.SuspendLayout();
            this.pnlStatusReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblBroker);
            this.panelTop.Controls.Add(this._txtBroker);
            this.panelTop.Controls.Add(this.lblEqId);
            this.panelTop.Controls.Add(this._txtEqId);
            this.panelTop.Controls.Add(this.lblBaseTopic);
            this.panelTop.Controls.Add(this._txtTopic);
            this.panelTop.Controls.Add(this._btnConnect);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10);
            this.panelTop.Size = new System.Drawing.Size(784, 80);
            this.panelTop.TabIndex = 0;
            // 
            // lblBroker
            // 
            this.lblBroker.Location = new System.Drawing.Point(10, 15);
            this.lblBroker.Name = "lblBroker";
            this.lblBroker.Size = new System.Drawing.Size(50, 23);
            this.lblBroker.TabIndex = 0;
            this.lblBroker.Text = "Broker:";
            // 
            // _txtBroker
            // 
            this._txtBroker.Location = new System.Drawing.Point(60, 12);
            this._txtBroker.Name = "_txtBroker";
            this._txtBroker.Size = new System.Drawing.Size(120, 22);
            this._txtBroker.TabIndex = 1;
            this._txtBroker.Text = "127.0.0.1";
            // 
            // lblEqId
            // 
            this.lblEqId.Location = new System.Drawing.Point(200, 15);
            this.lblEqId.Name = "lblEqId";
            this.lblEqId.Size = new System.Drawing.Size(50, 23);
            this.lblEqId.TabIndex = 2;
            this.lblEqId.Text = "EQ ID:";
            // 
            // _txtEqId
            // 
            this._txtEqId.Location = new System.Drawing.Point(250, 12);
            this._txtEqId.Name = "_txtEqId";
            this._txtEqId.Size = new System.Drawing.Size(100, 22);
            this._txtEqId.TabIndex = 3;
            this._txtEqId.Text = "ZZ0304";
            // 
            // lblBaseTopic
            // 
            this.lblBaseTopic.Location = new System.Drawing.Point(10, 45);
            this.lblBaseTopic.Name = "lblBaseTopic";
            this.lblBaseTopic.Size = new System.Drawing.Size(80, 23);
            this.lblBaseTopic.TabIndex = 4;
            this.lblBaseTopic.Text = "Base Topic:";
            // 
            // _txtTopic
            // 
            this._txtTopic.Location = new System.Drawing.Point(90, 42);
            this._txtTopic.Name = "_txtTopic";
            this._txtTopic.Size = new System.Drawing.Size(260, 22);
            this._txtTopic.TabIndex = 5;
            this._txtTopic.Text = "TN2/Floor2/Yellow/ZZ0304/";
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(370, 10);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(100, 55);
            this._btnConnect.TabIndex = 6;
            this._btnConnect.Text = "Connect";
            this._btnConnect.UseVisualStyleBackColor = true;
            this._btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // grpParams
            // 
            this.grpParams.Controls.Add(this.lblBarcode);
            this.grpParams.Controls.Add(this._txtBarcode);
            this.grpParams.Controls.Add(this.lblQty);
            this.grpParams.Controls.Add(this._txtQty);
            this.grpParams.Controls.Add(this.lblToolingId);
            this.grpParams.Controls.Add(this._txtToolingId);
            this.grpParams.Controls.Add(this.lblCassetteId);
            this.grpParams.Controls.Add(this._txtCassetteId);
            this.grpParams.Controls.Add(this.lblMaterialId);
            this.grpParams.Controls.Add(this._txtMaterialId);
            this.grpParams.Controls.Add(this.lblWorkOrder);
            this.grpParams.Controls.Add(this._txtWorkOrder);
            this.grpParams.Controls.Add(this.lblUserId);
            this.grpParams.Controls.Add(this._txtUserId);
            this.grpParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpParams.Location = new System.Drawing.Point(0, 80);
            this.grpParams.Name = "grpParams";
            this.grpParams.Size = new System.Drawing.Size(784, 130);
            this.grpParams.TabIndex = 1;
            this.grpParams.TabStop = false;
            this.grpParams.Text = "Request Parameters";
            // 
            // lblBarcode
            // 
            this.lblBarcode.Location = new System.Drawing.Point(180, 20);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(60, 23);
            this.lblBarcode.TabIndex = 13;
            this.lblBarcode.Text = "Barcode:";
            // 
            // _txtBarcode
            // 
            this._txtBarcode.Location = new System.Drawing.Point(240, 17);
            this._txtBarcode.Name = "_txtBarcode";
            this._txtBarcode.Size = new System.Drawing.Size(340, 22);
            this._txtBarcode.TabIndex = 12;
            this._txtBarcode.Text = "{{{{746092{A1{{{ed1fa5a22b55404697e96b698b66e498";
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(400, 80);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(80, 23);
            this.lblQty.TabIndex = 11;
            this.lblQty.Text = "Qty:";
            // 
            // _txtQty
            // 
            this._txtQty.Location = new System.Drawing.Point(480, 77);
            this._txtQty.Name = "_txtQty";
            this._txtQty.Size = new System.Drawing.Size(100, 22);
            this._txtQty.TabIndex = 10;
            this._txtQty.Text = "100";
            // 
            // lblToolingId
            // 
            this.lblToolingId.Location = new System.Drawing.Point(200, 80);
            this.lblToolingId.Name = "lblToolingId";
            this.lblToolingId.Size = new System.Drawing.Size(80, 23);
            this.lblToolingId.TabIndex = 9;
            this.lblToolingId.Text = "Tooling ID:";
            // 
            // _txtToolingId
            // 
            this._txtToolingId.Location = new System.Drawing.Point(280, 77);
            this._txtToolingId.Name = "_txtToolingId";
            this._txtToolingId.Size = new System.Drawing.Size(100, 22);
            this._txtToolingId.TabIndex = 8;
            this._txtToolingId.Text = "TOOL01";
            // 
            // lblCassetteId
            // 
            this.lblCassetteId.Location = new System.Drawing.Point(10, 80);
            this.lblCassetteId.Name = "lblCassetteId";
            this.lblCassetteId.Size = new System.Drawing.Size(80, 23);
            this.lblCassetteId.TabIndex = 7;
            this.lblCassetteId.Text = "Cassette ID:";
            // 
            // _txtCassetteId
            // 
            this._txtCassetteId.Location = new System.Drawing.Point(90, 77);
            this._txtCassetteId.Name = "_txtCassetteId";
            this._txtCassetteId.Size = new System.Drawing.Size(100, 22);
            this._txtCassetteId.TabIndex = 6;
            this._txtCassetteId.Text = "CASS01";
            // 
            // lblMaterialId
            // 
            this.lblMaterialId.Location = new System.Drawing.Point(400, 50);
            this.lblMaterialId.Name = "lblMaterialId";
            this.lblMaterialId.Size = new System.Drawing.Size(80, 23);
            this.lblMaterialId.TabIndex = 5;
            this.lblMaterialId.Text = "Material ID:";
            // 
            // _txtMaterialId
            // 
            this._txtMaterialId.Location = new System.Drawing.Point(480, 47);
            this._txtMaterialId.Name = "_txtMaterialId";
            this._txtMaterialId.Size = new System.Drawing.Size(100, 22);
            this._txtMaterialId.TabIndex = 4;
            this._txtMaterialId.Text = "MAT001";
            // 
            // lblWorkOrder
            // 
            this.lblWorkOrder.Location = new System.Drawing.Point(200, 50);
            this.lblWorkOrder.Name = "lblWorkOrder";
            this.lblWorkOrder.Size = new System.Drawing.Size(80, 23);
            this.lblWorkOrder.TabIndex = 3;
            this.lblWorkOrder.Text = "Work Order:";
            // 
            // _txtWorkOrder
            // 
            this._txtWorkOrder.Location = new System.Drawing.Point(280, 47);
            this._txtWorkOrder.Name = "_txtWorkOrder";
            this._txtWorkOrder.Size = new System.Drawing.Size(100, 22);
            this._txtWorkOrder.TabIndex = 2;
            this._txtWorkOrder.Text = "2P26325UP133";
            // 
            // lblUserId
            // 
            this.lblUserId.Location = new System.Drawing.Point(10, 20);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(60, 23);
            this.lblUserId.TabIndex = 1;
            this.lblUserId.Text = "User ID:";
            // 
            // _txtUserId
            // 
            this._txtUserId.Location = new System.Drawing.Point(70, 17);
            this._txtUserId.Name = "_txtUserId";
            this._txtUserId.Size = new System.Drawing.Size(100, 22);
            this._txtUserId.TabIndex = 0;
            this._txtUserId.Text = "746092";
            // 
            // _grpActions
            // 
            this._grpActions.Controls.Add(this.flowActions);
            this._grpActions.Dock = System.Windows.Forms.DockStyle.Top;
            this._grpActions.Enabled = false;
            this._grpActions.Location = new System.Drawing.Point(0, 210);
            this._grpActions.Name = "_grpActions";
            this._grpActions.Size = new System.Drawing.Size(784, 100);
            this._grpActions.TabIndex = 2;
            this._grpActions.TabStop = false;
            this._grpActions.Text = "Simulation Actions";
            // 
            // flowActions
            // 
            this.flowActions.Controls.Add(this.btnUser);
            this.flowActions.Controls.Add(this.btnTrackIn);
            this.flowActions.Controls.Add(this.btnTrackOut);
            this.flowActions.Controls.Add(this.btnPanelIn);
            this.flowActions.Controls.Add(this.btnPanelOut);
            this.flowActions.Controls.Add(this.btnRtnTraceData);
            this.flowActions.Controls.Add(this.btnAlarm);
            this.flowActions.Controls.Add(this.pnlStatusReport);
            this.flowActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowActions.Location = new System.Drawing.Point(3, 18);
            this.flowActions.Name = "flowActions";
            this.flowActions.Padding = new System.Windows.Forms.Padding(5);
            this.flowActions.Size = new System.Drawing.Size(778, 79);
            this.flowActions.TabIndex = 0;
            // 
            // btnUser
            // 
            this.btnUser.Location = new System.Drawing.Point(8, 8);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(120, 23);
            this.btnUser.TabIndex = 0;
            this.btnUser.Text = "Login (UserVerify)";
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.BtnUser_Click);
            // 
            // btnTrackIn
            // 
            this.btnTrackIn.Location = new System.Drawing.Point(134, 8);
            this.btnTrackIn.Name = "btnTrackIn";
            this.btnTrackIn.Size = new System.Drawing.Size(120, 23);
            this.btnTrackIn.TabIndex = 1;
            this.btnTrackIn.Text = "Track In Request";
            this.btnTrackIn.UseVisualStyleBackColor = true;
            this.btnTrackIn.Click += new System.EventHandler(this.BtnTrackIn_Click);
            // 
            // btnTrackOut
            // 
            this.btnTrackOut.Location = new System.Drawing.Point(260, 8);
            this.btnTrackOut.Name = "btnTrackOut";
            this.btnTrackOut.Size = new System.Drawing.Size(120, 23);
            this.btnTrackOut.TabIndex = 2;
            this.btnTrackOut.Text = "Track Out Request";
            this.btnTrackOut.UseVisualStyleBackColor = true;
            this.btnTrackOut.Click += new System.EventHandler(this.BtnTrackOut_Click);
            // 
            // btnPanelIn
            // 
            this.btnPanelIn.Location = new System.Drawing.Point(386, 8);
            this.btnPanelIn.Name = "btnPanelIn";
            this.btnPanelIn.Size = new System.Drawing.Size(90, 23);
            this.btnPanelIn.TabIndex = 3;
            this.btnPanelIn.Text = "Panel In";
            this.btnPanelIn.UseVisualStyleBackColor = true;
            this.btnPanelIn.Click += new System.EventHandler(this.BtnPanelIn_Click);
            // 
            // btnPanelOut
            // 
            this.btnPanelOut.Location = new System.Drawing.Point(482, 8);
            this.btnPanelOut.Name = "btnPanelOut";
            this.btnPanelOut.Size = new System.Drawing.Size(90, 23);
            this.btnPanelOut.TabIndex = 4;
            this.btnPanelOut.Text = "Panel Out";
            this.btnPanelOut.UseVisualStyleBackColor = true;
            this.btnPanelOut.Click += new System.EventHandler(this.BtnPanelOut_Click);
            // 
            // btnRtnTraceData
            // 
            this.btnRtnTraceData.Location = new System.Drawing.Point(578, 8);
            this.btnRtnTraceData.Name = "btnRtnTraceData";
            this.btnRtnTraceData.Size = new System.Drawing.Size(100, 23);
            this.btnRtnTraceData.TabIndex = 5;
            this.btnRtnTraceData.Text = "Trace Data";
            this.btnRtnTraceData.UseVisualStyleBackColor = true;
            this.btnRtnTraceData.Click += new System.EventHandler(this.BtnRtnTraceData_Click);
            // 
            // btnAlarm
            // 
            this.btnAlarm.Location = new System.Drawing.Point(8, 37);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(120, 23);
            this.btnAlarm.TabIndex = 6;
            this.btnAlarm.Text = "Trigger ALARM";
            this.btnAlarm.UseVisualStyleBackColor = true;
            this.btnAlarm.Click += new System.EventHandler(this.BtnAlarm_Click);
            // 
            // pnlStatusReport
            // 
            this.pnlStatusReport.Controls.Add(this._cmbStatus);
            this.pnlStatusReport.Controls.Add(this.btnReportStatus);
            this.pnlStatusReport.Location = new System.Drawing.Point(134, 37);
            this.pnlStatusReport.Name = "pnlStatusReport";
            this.pnlStatusReport.Size = new System.Drawing.Size(250, 30);
            this.pnlStatusReport.TabIndex = 7;
            // 
            // _cmbStatus
            // 
            this._cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbStatus.FormattingEnabled = true;
            this._cmbStatus.Location = new System.Drawing.Point(3, 3);
            this._cmbStatus.Name = "_cmbStatus";
            this._cmbStatus.Size = new System.Drawing.Size(120, 20);
            this._cmbStatus.TabIndex = 0;
            // 
            // btnReportStatus
            // 
            this.btnReportStatus.Location = new System.Drawing.Point(129, 1);
            this.btnReportStatus.Name = "btnReportStatus";
            this.btnReportStatus.Size = new System.Drawing.Size(110, 23);
            this.btnReportStatus.TabIndex = 1;
            this.btnReportStatus.Text = "Report Status";
            this.btnReportStatus.UseVisualStyleBackColor = true;
            this.btnReportStatus.Click += new System.EventHandler(this.BtnReportStatus_Click);
            // 
            // _txtLog
            // 
            this._txtLog.BackColor = System.Drawing.Color.Black;
            this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            this._txtLog.ForeColor = System.Drawing.Color.Lime;
            this._txtLog.Location = new System.Drawing.Point(0, 310);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtLog.Size = new System.Drawing.Size(784, 251);
            this._txtLog.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._txtLog);
            this.Controls.Add(this._grpActions);
            this.Controls.Add(this.grpParams);
            this.Controls.Add(this.panelTop);
            this.Name = "MainForm";
            this.Text = "EQ Protocol Simulator v1.0.3";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.grpParams.ResumeLayout(false);
            this.grpParams.PerformLayout();
            this._grpActions.ResumeLayout(false);
            this.flowActions.ResumeLayout(false);
            this.pnlStatusReport.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblBroker;
        private System.Windows.Forms.TextBox _txtBroker;
        private System.Windows.Forms.Label lblEqId;
        private System.Windows.Forms.TextBox _txtEqId;
        private System.Windows.Forms.Label lblBaseTopic;
        private System.Windows.Forms.TextBox _txtTopic;
        private System.Windows.Forms.Button _btnConnect;
        private System.Windows.Forms.GroupBox grpParams;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TextBox _txtUserId;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox _txtBarcode;
        private System.Windows.Forms.Label lblWorkOrder;
        private System.Windows.Forms.TextBox _txtWorkOrder;
        private System.Windows.Forms.Label lblMaterialId;
        private System.Windows.Forms.TextBox _txtMaterialId;
        private System.Windows.Forms.Label lblCassetteId;
        private System.Windows.Forms.TextBox _txtCassetteId;
        private System.Windows.Forms.Label lblToolingId;
        private System.Windows.Forms.TextBox _txtToolingId;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox _txtQty;
        private System.Windows.Forms.GroupBox _grpActions;
        private System.Windows.Forms.FlowLayoutPanel flowActions;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.Button btnTrackIn;
        private System.Windows.Forms.Button btnTrackOut;
        private System.Windows.Forms.Button btnPanelIn;
        private System.Windows.Forms.Button btnPanelOut;
        private System.Windows.Forms.Button btnRtnTraceData;
        private System.Windows.Forms.Button btnAlarm;
        private System.Windows.Forms.Panel pnlStatusReport;
        private System.Windows.Forms.ComboBox _cmbStatus;
        private System.Windows.Forms.Button btnReportStatus;
        private System.Windows.Forms.TextBox _txtLog;
    }
}
