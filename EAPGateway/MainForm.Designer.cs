namespace EAPGateway
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblSecondary = new System.Windows.Forms.Label();
            this.pbSecondary = new System.Windows.Forms.PictureBox();
            this.lblPrimary = new System.Windows.Forms.Label();
            this.pbPrimary = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSecondary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrimary)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtLog.Location = new System.Drawing.Point(0, 80);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(800, 370);
            this.txtLog.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel1.Controls.Add(this.lblPort);
            this.panel1.Controls.Add(this.lblSecondary);
            this.panel1.Controls.Add(this.pbSecondary);
            this.panel1.Controls.Add(this.lblPrimary);
            this.panel1.Controls.Add(this.pbPrimary);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 80);
            this.panel1.TabIndex = 1;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.ForeColor = System.Drawing.Color.White;
            this.lblPort.Location = new System.Drawing.Point(550, 30);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(120, 15);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Listening on Port: 5000";
            // 
            // lblSecondary
            // 
            this.lblSecondary.AutoSize = true;
            this.lblSecondary.ForeColor = System.Drawing.Color.White;
            this.lblSecondary.Location = new System.Drawing.Point(320, 30);
            this.lblSecondary.Name = "lblSecondary";
            this.lblSecondary.Size = new System.Drawing.Size(65, 15);
            this.lblSecondary.TabIndex = 3;
            this.lblSecondary.Text = "Secondary: -";
            // 
            // pbSecondary
            // 
            this.pbSecondary.BackColor = System.Drawing.Color.Gray;
            this.pbSecondary.Location = new System.Drawing.Point(290, 27);
            this.pbSecondary.Name = "pbSecondary";
            this.pbSecondary.Size = new System.Drawing.Size(20, 20);
            this.pbSecondary.TabIndex = 2;
            this.pbSecondary.TabStop = false;
            // 
            // lblPrimary
            // 
            this.lblPrimary.AutoSize = true;
            this.lblPrimary.ForeColor = System.Drawing.Color.White;
            this.lblPrimary.Location = new System.Drawing.Point(50, 30);
            this.lblPrimary.Name = "lblPrimary";
            this.lblPrimary.Size = new System.Drawing.Size(53, 15);
            this.lblPrimary.TabIndex = 1;
            this.lblPrimary.Text = "Primary: -";
            // 
            // pbPrimary
            // 
            this.pbPrimary.BackColor = System.Drawing.Color.Gray;
            this.pbPrimary.Location = new System.Drawing.Point(20, 27);
            this.pbPrimary.Name = "pbPrimary";
            this.pbPrimary.Size = new System.Drawing.Size(20, 20);
            this.pbPrimary.TabIndex = 0;
            this.pbPrimary.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "EAPGateway - Dual DB Proxy";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSecondary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrimary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPrimary;
        private System.Windows.Forms.PictureBox pbPrimary;
        private System.Windows.Forms.Label lblSecondary;
        private System.Windows.Forms.PictureBox pbSecondary;
        private System.Windows.Forms.Label lblPort;
    }
}
