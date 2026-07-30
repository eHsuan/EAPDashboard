namespace EAPDashboard
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblBrokerStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuSystem;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuCommand;
        private System.Windows.Forms.ToolStripMenuItem menuTimeSync;
        private System.Windows.Forms.ToolStripMenuItem menuTerminalDisplay;

        private void InitializeComponent()
        {
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblBrokerStatus = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTimeSync = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTerminalDisplay = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlStatus.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 64);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1200, 736);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlStatus.Controls.Add(this.lblBrokerStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(0, 24);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1200, 40);
            this.pnlStatus.TabIndex = 1;
            // 
            // lblBrokerStatus
            // 
            this.lblBrokerStatus.AutoSize = true;
            this.lblBrokerStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerStatus.ForeColor = System.Drawing.Color.Red;
            this.lblBrokerStatus.Location = new System.Drawing.Point(12, 9);
            this.lblBrokerStatus.Name = "lblBrokerStatus";
            this.lblBrokerStatus.Size = new System.Drawing.Size(201, 21);
            this.lblBrokerStatus.TabIndex = 0;
            this.lblBrokerStatus.Text = "● MQTT Broker: Disconnected";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSystem,
            this.menuCommand});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuSystem
            // 
            this.menuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings});
            this.menuSystem.Name = "menuSystem";
            this.menuSystem.Size = new System.Drawing.Size(43, 20);
            this.menuSystem.Text = "系統";
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(180, 22);
            this.menuSettings.Text = "設定";
            this.menuSettings.Click += new System.EventHandler(this.MenuSettings_Click);
            // 
            // menuCommand
            // 
            this.menuCommand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTimeSync,
            this.menuTerminalDisplay});
            this.menuCommand.Name = "menuCommand";
            this.menuCommand.Size = new System.Drawing.Size(43, 20);
            this.menuCommand.Text = "指令";
            // 
            // menuTimeSync
            // 
            this.menuTimeSync.Name = "menuTimeSync";
            this.menuTimeSync.Size = new System.Drawing.Size(180, 22);
            this.menuTimeSync.Text = "時間校正";
            this.menuTimeSync.Click += new System.EventHandler(this.MenuTimeSync_Click);
            // 
            // menuTerminalDisplay
            // 
            this.menuTerminalDisplay.Name = "menuTerminalDisplay";
            this.menuTerminalDisplay.Size = new System.Drawing.Size(180, 22);
            this.menuTerminalDisplay.Text = "發送遠端訊息 (Terminal Display)";
            this.menuTerminalDisplay.Click += new System.EventHandler(this.MenuTerminalDisplay_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "EAP Dashboard Prototype";
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
