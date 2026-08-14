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
            panelTop = new System.Windows.Forms.Panel();
            lblBroker = new System.Windows.Forms.Label();
            _txtBroker = new System.Windows.Forms.TextBox();
            lblEqId = new System.Windows.Forms.Label();
            _txtEqId = new System.Windows.Forms.TextBox();
            lblBaseTopic = new System.Windows.Forms.Label();
            _txtTopic = new System.Windows.Forms.TextBox();
            _btnSaveSettings = new System.Windows.Forms.Button();
            _btnConnect = new System.Windows.Forms.Button();
            grpParams = new System.Windows.Forms.GroupBox();
            lblBarcode = new System.Windows.Forms.Label();
            _txtBarcode = new System.Windows.Forms.TextBox();
            lblQty = new System.Windows.Forms.Label();
            _txtQty = new System.Windows.Forms.TextBox();
            lblToolingId = new System.Windows.Forms.Label();
            _txtToolingId = new System.Windows.Forms.TextBox();
            lblCassetteId = new System.Windows.Forms.Label();
            _txtCassetteId = new System.Windows.Forms.TextBox();
            lblMaterialId = new System.Windows.Forms.Label();
            _txtMaterialId = new System.Windows.Forms.TextBox();
            lblWorkOrder = new System.Windows.Forms.Label();
            _txtWorkOrder = new System.Windows.Forms.TextBox();
            lblUserId = new System.Windows.Forms.Label();
            _txtUserId = new System.Windows.Forms.TextBox();
            lblPanelList = new System.Windows.Forms.Label();
            _txtPanelList = new System.Windows.Forms.TextBox();
            _grpActions = new System.Windows.Forms.GroupBox();
            flowActions = new System.Windows.Forms.FlowLayoutPanel();
            btnUser = new System.Windows.Forms.Button();
            btnTrackIn = new System.Windows.Forms.Button();
            btnTrackOut = new System.Windows.Forms.Button();
            btnPanelIn = new System.Windows.Forms.Button();
            btnPanelOut = new System.Windows.Forms.Button();
            btnRtnTraceData = new System.Windows.Forms.Button();
            btnAlarm = new System.Windows.Forms.Button();
            _btnAutoTest = new System.Windows.Forms.Button();
            chkPPSelectPass = new System.Windows.Forms.CheckBox();
            pnlStatusReport = new System.Windows.Forms.Panel();
            _cmbStatusType = new System.Windows.Forms.ComboBox();
            _cmbStatus = new System.Windows.Forms.ComboBox();
            btnReportStatus = new System.Windows.Forms.Button();
            _txtLog = new System.Windows.Forms.TextBox();
            panelTop.SuspendLayout();
            grpParams.SuspendLayout();
            _grpActions.SuspendLayout();
            flowActions.SuspendLayout();
            pnlStatusReport.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblBroker);
            panelTop.Controls.Add(_txtBroker);
            panelTop.Controls.Add(lblEqId);
            panelTop.Controls.Add(_txtEqId);
            panelTop.Controls.Add(lblBaseTopic);
            panelTop.Controls.Add(_txtTopic);
            panelTop.Controls.Add(_btnSaveSettings);
            panelTop.Controls.Add(_btnConnect);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            panelTop.Size = new System.Drawing.Size(915, 100);
            panelTop.TabIndex = 0;
            // 
            // lblBroker
            // 
            lblBroker.Location = new System.Drawing.Point(12, 19);
            lblBroker.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBroker.Name = "lblBroker";
            lblBroker.Size = new System.Drawing.Size(58, 29);
            lblBroker.TabIndex = 0;
            lblBroker.Text = "Broker:";
            // 
            // _txtBroker
            // 
            _txtBroker.Location = new System.Drawing.Point(70, 15);
            _txtBroker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtBroker.Name = "_txtBroker";
            _txtBroker.Size = new System.Drawing.Size(139, 23);
            _txtBroker.TabIndex = 1;
            _txtBroker.Text = "127.0.0.1";
            // 
            // lblEqId
            // 
            lblEqId.Location = new System.Drawing.Point(233, 19);
            lblEqId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblEqId.Name = "lblEqId";
            lblEqId.Size = new System.Drawing.Size(58, 29);
            lblEqId.TabIndex = 2;
            lblEqId.Text = "EQ ID:";
            // 
            // _txtEqId
            // 
            _txtEqId.Location = new System.Drawing.Point(292, 15);
            _txtEqId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtEqId.Name = "_txtEqId";
            _txtEqId.Size = new System.Drawing.Size(116, 23);
            _txtEqId.TabIndex = 3;
            _txtEqId.Text = "ZZ0304";
            // 
            // lblBaseTopic
            // 
            lblBaseTopic.Location = new System.Drawing.Point(12, 56);
            lblBaseTopic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBaseTopic.Name = "lblBaseTopic";
            lblBaseTopic.Size = new System.Drawing.Size(93, 29);
            lblBaseTopic.TabIndex = 4;
            lblBaseTopic.Text = "Base Topic:";
            // 
            // _txtTopic
            // 
            _txtTopic.Location = new System.Drawing.Point(105, 52);
            _txtTopic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtTopic.Name = "_txtTopic";
            _txtTopic.Size = new System.Drawing.Size(303, 23);
            _txtTopic.TabIndex = 5;
            _txtTopic.Text = "TN2/Floor2/Yellow/ZZ0304/";
            // 
            // _btnSaveSettings
            // 
            _btnSaveSettings.Location = new System.Drawing.Point(465, 13);
            _btnSaveSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _btnSaveSettings.Name = "_btnSaveSettings";
            _btnSaveSettings.Size = new System.Drawing.Size(100, 69);
            _btnSaveSettings.TabIndex = 6;
            _btnSaveSettings.Text = "記憶設定";
            _btnSaveSettings.UseVisualStyleBackColor = true;
            _btnSaveSettings.Click += BtnSaveSettings_Click;
            // 
            // _btnConnect
            // 
            _btnConnect.Location = new System.Drawing.Point(580, 13);
            _btnConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _btnConnect.Name = "_btnConnect";
            _btnConnect.Size = new System.Drawing.Size(117, 69);
            _btnConnect.TabIndex = 7;
            _btnConnect.Text = "Connect";
            _btnConnect.UseVisualStyleBackColor = true;
            _btnConnect.Click += BtnConnect_Click;
            // 
            // grpParams
            // 
            grpParams.Controls.Add(lblBarcode);
            grpParams.Controls.Add(_txtBarcode);
            grpParams.Controls.Add(lblWorkOrder);
            grpParams.Controls.Add(_txtWorkOrder);
            grpParams.Controls.Add(lblMaterialId);
            grpParams.Controls.Add(_txtMaterialId);
            grpParams.Controls.Add(lblCassetteId);
            grpParams.Controls.Add(_txtCassetteId);
            grpParams.Controls.Add(lblToolingId);
            grpParams.Controls.Add(_txtToolingId);
            grpParams.Controls.Add(lblQty);
            grpParams.Controls.Add(_txtQty);
            grpParams.Controls.Add(lblUserId);
            grpParams.Controls.Add(_txtUserId);
            grpParams.Controls.Add(lblPanelList);
            grpParams.Controls.Add(_txtPanelList);
            grpParams.Dock = System.Windows.Forms.DockStyle.Top;
            grpParams.Location = new System.Drawing.Point(0, 100);
            grpParams.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpParams.Name = "grpParams";
            grpParams.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpParams.Size = new System.Drawing.Size(915, 185);
            grpParams.TabIndex = 1;
            grpParams.TabStop = false;
            grpParams.Text = "Request Parameters";
            // 
            // lblUserId
            // 
            lblUserId.Location = new System.Drawing.Point(12, 25);
            lblUserId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new System.Drawing.Size(70, 29);
            lblUserId.TabIndex = 0;
            lblUserId.Text = "User ID:";
            // 
            // _txtUserId
            // 
            _txtUserId.Location = new System.Drawing.Point(82, 21);
            _txtUserId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtUserId.Name = "_txtUserId";
            _txtUserId.Size = new System.Drawing.Size(116, 23);
            _txtUserId.TabIndex = 1;
            _txtUserId.Text = "746092";
            // 
            // lblBarcode
            // 
            lblBarcode.Location = new System.Drawing.Point(210, 25);
            lblBarcode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new System.Drawing.Size(70, 29);
            lblBarcode.TabIndex = 2;
            lblBarcode.Text = "Barcode:";
            // 
            // _txtBarcode
            // 
            _txtBarcode.Location = new System.Drawing.Point(280, 21);
            _txtBarcode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtBarcode.Name = "_txtBarcode";
            _txtBarcode.Size = new System.Drawing.Size(396, 23);
            _txtBarcode.TabIndex = 3;
            _txtBarcode.Text = "{{{{746092{A1{{{ed1fa5a22b55404697e96b698b66e498";
            // 
            // lblWorkOrder
            // 
            lblWorkOrder.Location = new System.Drawing.Point(12, 62);
            lblWorkOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblWorkOrder.Name = "lblWorkOrder";
            lblWorkOrder.Size = new System.Drawing.Size(93, 29);
            lblWorkOrder.TabIndex = 4;
            lblWorkOrder.Text = "Work Order:";
            // 
            // _txtWorkOrder
            // 
            _txtWorkOrder.Location = new System.Drawing.Point(105, 59);
            _txtWorkOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtWorkOrder.Name = "_txtWorkOrder";
            _txtWorkOrder.Size = new System.Drawing.Size(116, 23);
            _txtWorkOrder.TabIndex = 5;
            _txtWorkOrder.Text = "W0000000000002";
            // 
            // lblMaterialId
            // 
            lblMaterialId.Location = new System.Drawing.Point(233, 62);
            lblMaterialId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMaterialId.Name = "lblMaterialId";
            lblMaterialId.Size = new System.Drawing.Size(93, 29);
            lblMaterialId.TabIndex = 6;
            lblMaterialId.Text = "Material ID:";
            // 
            // _txtMaterialId
            // 
            _txtMaterialId.Location = new System.Drawing.Point(327, 59);
            _txtMaterialId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtMaterialId.Name = "_txtMaterialId";
            _txtMaterialId.Size = new System.Drawing.Size(116, 23);
            _txtMaterialId.TabIndex = 7;
            _txtMaterialId.Text = "MAT001";
            // 
            // lblCassetteId
            // 
            lblCassetteId.Location = new System.Drawing.Point(12, 100);
            lblCassetteId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCassetteId.Name = "lblCassetteId";
            lblCassetteId.Size = new System.Drawing.Size(93, 29);
            lblCassetteId.TabIndex = 8;
            lblCassetteId.Text = "Cassette ID:";
            // 
            // _txtCassetteId
            // 
            _txtCassetteId.Location = new System.Drawing.Point(105, 96);
            _txtCassetteId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtCassetteId.Name = "_txtCassetteId";
            _txtCassetteId.Size = new System.Drawing.Size(116, 23);
            _txtCassetteId.TabIndex = 9;
            _txtCassetteId.Text = "C0000000000002";
            // 
            // lblToolingId
            // 
            lblToolingId.Location = new System.Drawing.Point(233, 100);
            lblToolingId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblToolingId.Name = "lblToolingId";
            lblToolingId.Size = new System.Drawing.Size(93, 29);
            lblToolingId.TabIndex = 10;
            lblToolingId.Text = "Tooling ID:";
            // 
            // _txtToolingId
            // 
            _txtToolingId.Location = new System.Drawing.Point(327, 96);
            _txtToolingId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtToolingId.Name = "_txtToolingId";
            _txtToolingId.Size = new System.Drawing.Size(116, 23);
            _txtToolingId.TabIndex = 11;
            _txtToolingId.Text = "TOOL01";
            // 
            // lblQty
            // 
            lblQty.Location = new System.Drawing.Point(467, 100);
            lblQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblQty.Name = "lblQty";
            lblQty.Size = new System.Drawing.Size(40, 29);
            lblQty.TabIndex = 12;
            lblQty.Text = "Qty:";
            // 
            // _txtQty
            // 
            _txtQty.Location = new System.Drawing.Point(510, 96);
            _txtQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtQty.Name = "_txtQty";
            _txtQty.Size = new System.Drawing.Size(116, 23);
            _txtQty.TabIndex = 13;
            _txtQty.Text = "100";
            // 
            // lblPanelList
            // 
            lblPanelList.Location = new System.Drawing.Point(12, 140);
            lblPanelList.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPanelList.Name = "lblPanelList";
            lblPanelList.Size = new System.Drawing.Size(93, 29);
            lblPanelList.TabIndex = 14;
            lblPanelList.Text = "Panel List:";
            // 
            // _txtPanelList
            // 
            _txtPanelList.Location = new System.Drawing.Point(105, 136);
            _txtPanelList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtPanelList.Name = "_txtPanelList";
            _txtPanelList.Size = new System.Drawing.Size(571, 23);
            _txtPanelList.TabIndex = 15;
            _txtPanelList.Text = "P0000000000011,P0000000000012,P0000000000013,P0000000000014,P0000000000015,P0000000000016,P0000000000017,P0000000000018,P0000000000019,P0000000000020";
            // 
            // _grpActions
            // 
            _grpActions.Controls.Add(flowActions);
            _grpActions.Dock = System.Windows.Forms.DockStyle.Top;
            _grpActions.Enabled = false;
            _grpActions.Location = new System.Drawing.Point(0, 285);
            _grpActions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _grpActions.Name = "_grpActions";
            _grpActions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _grpActions.Size = new System.Drawing.Size(915, 125);
            _grpActions.TabIndex = 2;
            _grpActions.TabStop = false;
            _grpActions.Text = "Simulation Actions";
            // 
            // flowActions
            // 
            flowActions.Controls.Add(btnUser);
            flowActions.Controls.Add(btnTrackIn);
            flowActions.Controls.Add(btnTrackOut);
            flowActions.Controls.Add(btnPanelIn);
            flowActions.Controls.Add(btnPanelOut);
            flowActions.Controls.Add(btnRtnTraceData);
            flowActions.Controls.Add(btnAlarm);
            flowActions.Controls.Add(_btnAutoTest);
            flowActions.Controls.Add(pnlStatusReport);
            flowActions.Controls.Add(chkPPSelectPass);
            flowActions.Dock = System.Windows.Forms.DockStyle.Fill;
            flowActions.Location = new System.Drawing.Point(4, 20);
            flowActions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            flowActions.Name = "flowActions";
            flowActions.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            flowActions.Size = new System.Drawing.Size(907, 101);
            flowActions.TabIndex = 0;
            // 
            // btnUser
            // 
            btnUser.Location = new System.Drawing.Point(10, 10);
            btnUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnUser.Name = "btnUser";
            btnUser.Size = new System.Drawing.Size(140, 29);
            btnUser.TabIndex = 0;
            btnUser.Text = "Login (UserVerify)";
            btnUser.UseVisualStyleBackColor = true;
            btnUser.Click += BtnUser_Click;
            // 
            // btnTrackIn
            // 
            btnTrackIn.Location = new System.Drawing.Point(158, 10);
            btnTrackIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnTrackIn.Name = "btnTrackIn";
            btnTrackIn.Size = new System.Drawing.Size(140, 29);
            btnTrackIn.TabIndex = 1;
            btnTrackIn.Text = "Track In Request";
            btnTrackIn.UseVisualStyleBackColor = true;
            btnTrackIn.Click += BtnTrackIn_Click;
            // 
            // btnTrackOut
            // 
            btnTrackOut.Location = new System.Drawing.Point(306, 10);
            btnTrackOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnTrackOut.Name = "btnTrackOut";
            btnTrackOut.Size = new System.Drawing.Size(140, 29);
            btnTrackOut.TabIndex = 2;
            btnTrackOut.Text = "Track Out Request";
            btnTrackOut.UseVisualStyleBackColor = true;
            btnTrackOut.Click += BtnTrackOut_Click;
            // 
            // btnPanelIn
            // 
            btnPanelIn.Location = new System.Drawing.Point(454, 10);
            btnPanelIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnPanelIn.Name = "btnPanelIn";
            btnPanelIn.Size = new System.Drawing.Size(105, 29);
            btnPanelIn.TabIndex = 3;
            btnPanelIn.Text = "Panel In";
            btnPanelIn.UseVisualStyleBackColor = true;
            btnPanelIn.Click += BtnPanelIn_Click;
            // 
            // btnPanelOut
            // 
            btnPanelOut.Location = new System.Drawing.Point(567, 10);
            btnPanelOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnPanelOut.Name = "btnPanelOut";
            btnPanelOut.Size = new System.Drawing.Size(105, 29);
            btnPanelOut.TabIndex = 4;
            btnPanelOut.Text = "Panel Out";
            btnPanelOut.UseVisualStyleBackColor = true;
            btnPanelOut.Click += BtnPanelOut_Click;
            // 
            // btnRtnTraceData
            // 
            btnRtnTraceData.Location = new System.Drawing.Point(680, 10);
            btnRtnTraceData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnRtnTraceData.Name = "btnRtnTraceData";
            btnRtnTraceData.Size = new System.Drawing.Size(117, 29);
            btnRtnTraceData.TabIndex = 5;
            btnRtnTraceData.Text = "Trace Data";
            btnRtnTraceData.UseVisualStyleBackColor = true;
            btnRtnTraceData.Click += BtnRtnTraceData_Click;
            // 
            // btnAlarm
            // 
            btnAlarm.Location = new System.Drawing.Point(10, 47);
            btnAlarm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnAlarm.Name = "btnAlarm";
            btnAlarm.Size = new System.Drawing.Size(140, 29);
            btnAlarm.TabIndex = 6;
            btnAlarm.Text = "Trigger ALARM";
            btnAlarm.UseVisualStyleBackColor = true;
            btnAlarm.Click += BtnAlarm_Click;
            // 
            // _btnAutoTest
            // 
            _btnAutoTest.Location = new System.Drawing.Point(158, 47);
            _btnAutoTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _btnAutoTest.Name = "_btnAutoTest";
            _btnAutoTest.Size = new System.Drawing.Size(120, 29);
            _btnAutoTest.TabIndex = 7;
            _btnAutoTest.Text = "Start Auto Test";
            _btnAutoTest.UseVisualStyleBackColor = true;
            _btnAutoTest.Click += BtnAutoTest_Click;
            // 
            // 
            // pnlStatusReport
            // 
            pnlStatusReport.Controls.Add(_cmbStatusType);
            pnlStatusReport.Controls.Add(_cmbStatus);
            pnlStatusReport.Controls.Add(btnReportStatus);
            pnlStatusReport.Location = new System.Drawing.Point(286, 47);
            pnlStatusReport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnlStatusReport.Name = "pnlStatusReport";
            pnlStatusReport.Size = new System.Drawing.Size(390, 38);
            pnlStatusReport.TabIndex = 8;
            // 
            // _cmbStatusType
            // 
            _cmbStatusType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbStatusType.FormattingEnabled = true;
            _cmbStatusType.Items.AddRange(new object[] { "Cyntec", "Delta" });
            _cmbStatusType.Location = new System.Drawing.Point(4, 4);
            _cmbStatusType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _cmbStatusType.Name = "_cmbStatusType";
            _cmbStatusType.Size = new System.Drawing.Size(80, 23);
            _cmbStatusType.TabIndex = 2;
            // 
            // _cmbStatus
            // 
            _cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbStatus.FormattingEnabled = true;
            _cmbStatus.Location = new System.Drawing.Point(90, 4);
            _cmbStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _cmbStatus.Name = "_cmbStatus";
            _cmbStatus.Size = new System.Drawing.Size(160, 23);
            _cmbStatus.TabIndex = 0;
            // 
            // btnReportStatus
            // 
            btnReportStatus.Location = new System.Drawing.Point(255, 1);
            btnReportStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnReportStatus.Name = "btnReportStatus";
            btnReportStatus.Size = new System.Drawing.Size(128, 29);
            btnReportStatus.TabIndex = 1;
            btnReportStatus.Text = "Report Status";
            btnReportStatus.UseVisualStyleBackColor = true;
            btnReportStatus.Click += BtnReportStatus_Click;
            // 
            // chkPPSelectPass
            // 
            chkPPSelectPass.AutoSize = true;
            chkPPSelectPass.Checked = true;
            chkPPSelectPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkPPSelectPass.Location = new System.Drawing.Point(680, 52);
            chkPPSelectPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            chkPPSelectPass.Name = "chkPPSelectPass";
            chkPPSelectPass.Size = new System.Drawing.Size(110, 19);
            chkPPSelectPass.TabIndex = 9;
            chkPPSelectPass.Text = "PP_SELECT Pass";
            chkPPSelectPass.UseVisualStyleBackColor = true;
            // 
            // _txtLog
            // 
            _txtLog.BackColor = System.Drawing.Color.Black;
            _txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            _txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            _txtLog.ForeColor = System.Drawing.Color.Lime;
            _txtLog.Location = new System.Drawing.Point(0, 410);
            _txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            _txtLog.Multiline = true;
            _txtLog.Name = "_txtLog";
            _txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _txtLog.Size = new System.Drawing.Size(915, 291);
            _txtLog.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(915, 701);
            Controls.Add(_txtLog);
            Controls.Add(_grpActions);
            Controls.Add(grpParams);
            Controls.Add(panelTop);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "MainForm";
            Text = "Equipment MQTT Simulator v1.0.0";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            grpParams.ResumeLayout(false);
            grpParams.PerformLayout();
            _grpActions.ResumeLayout(false);
            flowActions.ResumeLayout(false);
            flowActions.PerformLayout();
            pnlStatusReport.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblBroker;
        private System.Windows.Forms.TextBox _txtBroker;
        private System.Windows.Forms.Label lblEqId;
        private System.Windows.Forms.TextBox _txtEqId;
        private System.Windows.Forms.Label lblBaseTopic;
        private System.Windows.Forms.TextBox _txtTopic;
        private System.Windows.Forms.Button _btnSaveSettings;
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
        private System.Windows.Forms.Label lblPanelList;
        private System.Windows.Forms.TextBox _txtPanelList;
        private System.Windows.Forms.GroupBox _grpActions;
        private System.Windows.Forms.FlowLayoutPanel flowActions;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.Button btnTrackIn;
        private System.Windows.Forms.Button btnTrackOut;
        private System.Windows.Forms.Button btnPanelIn;
        private System.Windows.Forms.Button btnPanelOut;
        private System.Windows.Forms.Button btnRtnTraceData;
        private System.Windows.Forms.Button btnAlarm;
        private System.Windows.Forms.Button _btnAutoTest;
        private System.Windows.Forms.CheckBox chkPPSelectPass;
        private System.Windows.Forms.Panel pnlStatusReport;
        private System.Windows.Forms.ComboBox _cmbStatusType;
        private System.Windows.Forms.ComboBox _cmbStatus;
        private System.Windows.Forms.Button btnReportStatus;
        private System.Windows.Forms.TextBox _txtLog;
    }
}
