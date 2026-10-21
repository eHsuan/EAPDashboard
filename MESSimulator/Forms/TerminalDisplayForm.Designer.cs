namespace MESSimulator.Forms
{
    partial class TerminalDisplayForm
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
            lblPrompt = new System.Windows.Forms.Label();
            lblTargetInfo = new System.Windows.Forms.Label();
            txtMessage = new System.Windows.Forms.TextBox();
            btnSend = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.AutoSize = true;
            lblPrompt.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            lblPrompt.Location = new System.Drawing.Point(16, 14);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new System.Drawing.Size(262, 18);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "請輸入要發送給機台螢幕顯示的訊息內容：";
            // 
            // lblTargetInfo
            // 
            lblTargetInfo.AutoSize = true;
            lblTargetInfo.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            lblTargetInfo.ForeColor = System.Drawing.Color.DimGray;
            lblTargetInfo.Location = new System.Drawing.Point(16, 40);
            lblTargetInfo.Name = "lblTargetInfo";
            lblTargetInfo.Size = new System.Drawing.Size(183, 15);
            lblTargetInfo.TabIndex = 1;
            lblTargetInfo.Text = "目標機台: TEST01 | Topic: ...";
            // 
            // txtMessage
            // 
            txtMessage.Font = new System.Drawing.Font("Consolas", 10F);
            txtMessage.Location = new System.Drawing.Point(19, 65);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtMessage.Size = new System.Drawing.Size(485, 120);
            txtMessage.TabIndex = 2;
            txtMessage.Text = "EAP TEST MESSAGE";
            // 
            // btnSend
            // 
            btnSend.BackColor = System.Drawing.Color.FromArgb(40, 110, 180);
            btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSend.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSend.ForeColor = System.Drawing.Color.White;
            btnSend.Location = new System.Drawing.Point(294, 200);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(100, 34);
            btnSend.TabIndex = 3;
            btnSend.Text = "發送";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += BtnSend_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(404, 200);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(100, 34);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // TerminalDisplayForm
            // 
            AcceptButton = btnSend;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(524, 248);
            Controls.Add(btnCancel);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(lblTargetInfo);
            Controls.Add(lblPrompt);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TerminalDisplayForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "發送機台螢幕訊息 (TerminalDisplay)";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblTargetInfo;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
    }
}
