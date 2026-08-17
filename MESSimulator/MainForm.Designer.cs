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
            panelTop = new System.Windows.Forms.Panel();
            lblInRequireInputCode = new System.Windows.Forms.Label();
            txtInRequireInputCode = new System.Windows.Forms.TextBox();
            lblOutRequireInputCode = new System.Windows.Forms.Label();
            txtOutRequireInputCode = new System.Windows.Forms.TextBox();
            btnStop = new System.Windows.Forms.Button();
            btnStart = new System.Windows.Forms.Button();
            txtPort = new System.Windows.Forms.TextBox();
            lblPort = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            splitContainer = new System.Windows.Forms.SplitContainer();
            grpRequest = new System.Windows.Forms.GroupBox();
            txtRequest = new System.Windows.Forms.TextBox();
            grpLog = new System.Windows.Forms.GroupBox();
            txtLog = new System.Windows.Forms.TextBox();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            grpRequest.SuspendLayout();
            grpLog.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            lblHandshakeResponses = new System.Windows.Forms.Label();
            chkUserVerifyPass = new System.Windows.Forms.CheckBox();
            chkWOQRYPass = new System.Windows.Forms.CheckBox();
            chkWOCHECKINPass = new System.Windows.Forms.CheckBox();
            chkWOCHECKOUTPass = new System.Windows.Forms.CheckBox();
            chkEQPSTATUSPass = new System.Windows.Forms.CheckBox();
            chkRecipeMismatch = new System.Windows.Forms.CheckBox();
            chkEnableNGCodeList = new System.Windows.Forms.CheckBox();
            panelTop.Controls.Add(lblHandshakeResponses);
            panelTop.Controls.Add(chkUserVerifyPass);
            panelTop.Controls.Add(chkWOQRYPass);
            panelTop.Controls.Add(chkWOCHECKINPass);
            panelTop.Controls.Add(chkWOCHECKOUTPass);
            panelTop.Controls.Add(chkEQPSTATUSPass);
            panelTop.Controls.Add(chkRecipeMismatch);
            panelTop.Controls.Add(chkEnableNGCodeList);
            panelTop.Controls.Add(lblInRequireInputCode);
            panelTop.Controls.Add(txtInRequireInputCode);
            panelTop.Controls.Add(lblOutRequireInputCode);
            panelTop.Controls.Add(txtOutRequireInputCode);
            panelTop.Controls.Add(btnStop);
            panelTop.Controls.Add(btnStart);
            panelTop.Controls.Add(txtPort);
            panelTop.Controls.Add(lblPort);
            panelTop.Controls.Add(lblStatus);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1176, 80);
            panelTop.TabIndex = 0;
            // 
            // lblInRequireInputCode
            // 
            lblInRequireInputCode.AutoSize = true;
            lblInRequireInputCode.Location = new System.Drawing.Point(408, 29);
            lblInRequireInputCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblInRequireInputCode.Name = "lblInRequireInputCode";
            lblInRequireInputCode.Size = new System.Drawing.Size(113, 15);
            lblInRequireInputCode.TabIndex = 5;
            lblInRequireInputCode.Text = "進站 Require Input:";
            // 
            // txtInRequireInputCode
            // 
            txtInRequireInputCode.Location = new System.Drawing.Point(537, 25);
            txtInRequireInputCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtInRequireInputCode.Name = "txtInRequireInputCode";
            txtInRequireInputCode.Size = new System.Drawing.Size(116, 23);
            txtInRequireInputCode.TabIndex = 6;
            txtInRequireInputCode.Text = "TE003";
            // 
            // lblOutRequireInputCode
            // 
            lblOutRequireInputCode.AutoSize = true;
            lblOutRequireInputCode.Location = new System.Drawing.Point(671, 29);
            lblOutRequireInputCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutRequireInputCode.Name = "lblOutRequireInputCode";
            lblOutRequireInputCode.Size = new System.Drawing.Size(113, 15);
            lblOutRequireInputCode.TabIndex = 7;
            lblOutRequireInputCode.Text = "出站 Require Input:";
            // 
            // txtOutRequireInputCode
            // 
            txtOutRequireInputCode.Location = new System.Drawing.Point(799, 25);
            txtOutRequireInputCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtOutRequireInputCode.Name = "txtOutRequireInputCode";
            txtOutRequireInputCode.Size = new System.Drawing.Size(116, 23);
            txtOutRequireInputCode.TabIndex = 8;
            txtOutRequireInputCode.Text = "EX001";
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new System.Drawing.Point(303, 19);
            btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(88, 38);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += BtnStop_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(198, 19);
            btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(88, 38);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += BtnStart_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(70, 25);
            txtPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(93, 23);
            txtPort.TabIndex = 2;
            txtPort.Text = "8070";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new System.Drawing.Point(23, 29);
            lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPort.Name = "lblPort";
            lblPort.Size = new System.Drawing.Size(33, 15);
            lblPort.TabIndex = 1;
            lblStatus.Text = "● Server Stopped";
            // 
            // lblHandshakeResponses
            // 
            lblHandshakeResponses.AutoSize = true;
            lblHandshakeResponses.Location = new System.Drawing.Point(23, 52);
            lblHandshakeResponses.Name = "lblHandshakeResponses";
            lblHandshakeResponses.Size = new System.Drawing.Size(125, 15);
            lblHandshakeResponses.TabIndex = 9;
            lblHandshakeResponses.Text = "交握回復 (PASS):";
            // 
            // chkUserVerifyPass
            // 
            chkUserVerifyPass.AutoSize = true;
            chkUserVerifyPass.Checked = true;
            chkUserVerifyPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkUserVerifyPass.Location = new System.Drawing.Point(155, 50);
            chkUserVerifyPass.Name = "chkUserVerifyPass";
            chkUserVerifyPass.Size = new System.Drawing.Size(81, 19);
            chkUserVerifyPass.TabIndex = 10;
            chkUserVerifyPass.Text = "UserVerify";
            chkUserVerifyPass.UseVisualStyleBackColor = true;
            // 
            // chkWOQRYPass
            // 
            chkWOQRYPass.AutoSize = true;
            chkWOQRYPass.Checked = true;
            chkWOQRYPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOQRYPass.Location = new System.Drawing.Point(245, 50);
            chkWOQRYPass.Name = "chkWOQRYPass";
            chkWOQRYPass.Size = new System.Drawing.Size(71, 19);
            chkWOQRYPass.TabIndex = 11;
            chkWOQRYPass.Text = "WOQRY";
            chkWOQRYPass.UseVisualStyleBackColor = true;
            // 
            // chkWOCHECKINPass
            // 
            chkWOCHECKINPass.AutoSize = true;
            chkWOCHECKINPass.Checked = true;
            chkWOCHECKINPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOCHECKINPass.Location = new System.Drawing.Point(325, 50);
            chkWOCHECKINPass.Name = "chkWOCHECKINPass";
            chkWOCHECKINPass.Size = new System.Drawing.Size(95, 19);
            chkWOCHECKINPass.TabIndex = 12;
            chkWOCHECKINPass.Text = "WOCHECKIN";
            chkWOCHECKINPass.UseVisualStyleBackColor = true;
            // 
            // chkWOCHECKOUTPass
            // 
            chkWOCHECKOUTPass.AutoSize = true;
            chkWOCHECKOUTPass.Checked = true;
            chkWOCHECKOUTPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWOCHECKOUTPass.Location = new System.Drawing.Point(430, 50);
            chkWOCHECKOUTPass.Name = "chkWOCHECKOUTPass";
            chkWOCHECKOUTPass.Size = new System.Drawing.Size(107, 19);
            chkWOCHECKOUTPass.TabIndex = 13;
            chkWOCHECKOUTPass.Text = "WOCHECKOUT";
            chkWOCHECKOUTPass.UseVisualStyleBackColor = true;
            // 
            // chkEQPSTATUSPass
            // 
            chkEQPSTATUSPass.AutoSize = true;
            chkEQPSTATUSPass.Checked = true;
            chkEQPSTATUSPass.CheckState = System.Windows.Forms.CheckState.Checked;
            chkEQPSTATUSPass.Location = new System.Drawing.Point(545, 50);
            chkEQPSTATUSPass.Name = "chkEQPSTATUSPass";
            chkEQPSTATUSPass.Size = new System.Drawing.Size(89, 19);
            chkEQPSTATUSPass.TabIndex = 14;
            chkEQPSTATUSPass.Text = "EQPSTATUS";
            chkEQPSTATUSPass.UseVisualStyleBackColor = true;
            // 
            // chkRecipeMismatch
            // 
            chkRecipeMismatch.AutoSize = true;
            chkRecipeMismatch.Checked = false;
            chkRecipeMismatch.Location = new System.Drawing.Point(645, 50);
            chkRecipeMismatch.Name = "chkRecipeMismatch";
            chkRecipeMismatch.Size = new System.Drawing.Size(185, 19);
            chkRecipeMismatch.TabIndex = 15;
            chkRecipeMismatch.Text = "多工單 Recipe 不一致 (模擬 Fail)";
            chkRecipeMismatch.UseVisualStyleBackColor = true;
            // 
            // chkEnableNGCodeList
            // 
            chkEnableNGCodeList.AutoSize = true;
            chkEnableNGCodeList.Checked = true;
            chkEnableNGCodeList.CheckState = System.Windows.Forms.CheckState.Checked;
            chkEnableNGCodeList.Location = new System.Drawing.Point(845, 50);
            chkEnableNGCodeList.Name = "chkEnableNGCodeList";
            chkEnableNGCodeList.Size = new System.Drawing.Size(165, 19);
            chkEnableNGCodeList.TabIndex = 16;
            chkEnableNGCodeList.Text = "啟用 NGCodeListDT 模擬";
            chkEnableNGCodeList.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 80);
            splitContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(grpRequest);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(grpLog);
            splitContainer.Size = new System.Drawing.Size(1176, 836);
            splitContainer.SplitterDistance = 583;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 1;
            // 
            // grpRequest
            // 
            grpRequest.Controls.Add(txtRequest);
            grpRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            grpRequest.Location = new System.Drawing.Point(0, 0);
            grpRequest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpRequest.Name = "grpRequest";
            grpRequest.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpRequest.Size = new System.Drawing.Size(583, 836);
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
            txtRequest.Location = new System.Drawing.Point(4, 20);
            txtRequest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtRequest.Multiline = true;
            txtRequest.Name = "txtRequest";
            txtRequest.ReadOnly = true;
            txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtRequest.Size = new System.Drawing.Size(575, 812);
            txtRequest.TabIndex = 0;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpLog.Location = new System.Drawing.Point(0, 0);
            grpLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpLog.Name = "grpLog";
            grpLog.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            grpLog.Size = new System.Drawing.Size(588, 836);
            grpLog.TabIndex = 0;
            grpLog.TabStop = false;
            grpLog.Text = "Communication Log";
            // 
            // txtLog
            // 
            txtLog.BackColor = System.Drawing.Color.Black;
            txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            txtLog.ForeColor = System.Drawing.Color.Lime;
            txtLog.Location = new System.Drawing.Point(4, 20);
            txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtLog.Size = new System.Drawing.Size(580, 812);
            txtLog.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1176, 911);
            Controls.Add(splitContainer);
            Controls.Add(panelTop);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "MainForm";
            Text = "MES WebService Simulator v1.0.0";
            Load += MainForm_Load;
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
            ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblInRequireInputCode;
        private System.Windows.Forms.TextBox txtInRequireInputCode;
        private System.Windows.Forms.Label lblOutRequireInputCode;
        private System.Windows.Forms.TextBox txtOutRequireInputCode;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
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
    }
}
