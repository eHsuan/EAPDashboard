namespace EAPDashboard.Controls
{
    partial class EquipmentCard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblEquipmentId;
        private System.Windows.Forms.Label lblEquipmentName;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRecipe;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Button btnReel;

        private void InitializeComponent()
        {
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblEquipmentId = new System.Windows.Forms.Label();
            this.lblEquipmentName = new System.Windows.Forms.Label();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRecipe = new System.Windows.Forms.Label();
            this.lblConnection = new System.Windows.Forms.Label();
            this.btnReel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            // 
            // panelStatus
            // 
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelStatus.Location = new System.Drawing.Point(0, 0);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(12, 180);
            this.panelStatus.TabIndex = 0;
            
            // 
            // lblEquipmentId
            // 
            this.lblEquipmentId.AutoSize = true;
            this.lblEquipmentId.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipmentId.Location = new System.Drawing.Point(20, 10);
            this.lblEquipmentId.Name = "lblEquipmentId";
            this.lblEquipmentId.Size = new System.Drawing.Size(160, 45);
            this.lblEquipmentId.TabIndex = 1;
            this.lblEquipmentId.Text = "EQP-001";
            
            // 
            // lblEquipmentName
            // 
            this.lblEquipmentName.AutoSize = true;
            this.lblEquipmentName.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipmentName.ForeColor = System.Drawing.Color.DimGray;
            this.lblEquipmentName.Location = new System.Drawing.Point(24, 54);
            this.lblEquipmentName.Name = "lblEquipmentName";
            this.lblEquipmentName.Size = new System.Drawing.Size(0, 19);
            this.lblEquipmentName.TabIndex = 6;
            
            // 
            // lblProtocol
            // 
            this.lblProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProtocol.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProtocol.ForeColor = System.Drawing.Color.Gray;
            this.lblProtocol.Location = new System.Drawing.Point(250, 5);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(90, 20);
            this.lblProtocol.TabIndex = 2;
            this.lblProtocol.Text = "PROTOCOL";
            this.lblProtocol.TextAlign = System.Drawing.ContentAlignment.TopRight;
            
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(24, 78);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(71, 21);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "STATUS";
            
            // 
            // lblRecipe
            // 
            this.lblRecipe.AutoSize = true;
            this.lblRecipe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecipe.Location = new System.Drawing.Point(25, 108);
            this.lblRecipe.Name = "lblRecipe";
            this.lblRecipe.Size = new System.Drawing.Size(84, 15);
            this.lblRecipe.TabIndex = 4;
            this.lblRecipe.Text = "Recipe: N/A";
            
            // 
            // lblConnection
            // 
            this.lblConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblConnection.AutoSize = true;
            this.lblConnection.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnection.Location = new System.Drawing.Point(24, 145);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(111, 19);
            this.lblConnection.TabIndex = 5;
            this.lblConnection.Text = "DISCONNECTED";
            
            // 
            // btnReel
            // 
            this.btnReel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReel.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnReel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReel.ForeColor = System.Drawing.Color.White;
            this.btnReel.Location = new System.Drawing.Point(260, 140);
            this.btnReel.Name = "btnReel";
            this.btnReel.Size = new System.Drawing.Size(75, 28);
            this.btnReel.TabIndex = 7;
            this.btnReel.Text = "Reel";
            this.btnReel.UseVisualStyleBackColor = false;
            this.btnReel.Visible = false;
            // 
            // EquipmentCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnReel);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.lblRecipe);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblEquipmentName);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.lblEquipmentId);
            this.Controls.Add(this.panelStatus);
            this.Name = "EquipmentCard";
            this.Size = new System.Drawing.Size(350, 180);
            this.Margin = new System.Windows.Forms.Padding(10);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
