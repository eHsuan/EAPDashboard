namespace EAPDashboard
{
    partial class ConfigDiffForm
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
            this.pnlTopBanner = new System.Windows.Forms.Panel();
            this.lblBannerText = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpGlobal = new System.Windows.Forms.GroupBox();
            this.dgvGlobalChanges = new System.Windows.Forms.DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpEquipments = new System.Windows.Forms.GroupBox();
            this.dgvEquipmentDiffs = new System.Windows.Forms.DataGridView();
            this.colEqStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEqDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlTopBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpGlobal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobalChanges)).BeginInit();
            this.grpEquipments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipmentDiffs)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBanner
            // 
            this.pnlTopBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.pnlTopBanner.Controls.Add(this.lblBannerText);
            this.pnlTopBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBanner.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBanner.Name = "pnlTopBanner";
            this.pnlTopBanner.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.pnlTopBanner.Size = new System.Drawing.Size(900, 60);
            this.pnlTopBanner.TabIndex = 0;
            // 
            // lblBannerText
            // 
            this.lblBannerText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBannerText.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblBannerText.ForeColor = System.Drawing.Color.White;
            this.lblBannerText.Location = new System.Drawing.Point(15, 10);
            this.lblBannerText.Name = "lblBannerText";
            this.lblBannerText.Size = new System.Drawing.Size(870, 40);
            this.lblBannerText.TabIndex = 0;
            this.lblBannerText.Text = "請檢閱即將變更之項目與內容，並確認是否套用：";
            this.lblBannerText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 60);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grpGlobal);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.grpEquipments);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitMain.Size = new System.Drawing.Size(900, 490);
            this.splitMain.SplitterDistance = 210;
            this.splitMain.TabIndex = 1;
            // 
            // grpGlobal
            // 
            this.grpGlobal.Controls.Add(this.dgvGlobalChanges);
            this.grpGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGlobal.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpGlobal.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpGlobal.Location = new System.Drawing.Point(10, 10);
            this.grpGlobal.Name = "grpGlobal";
            this.grpGlobal.Padding = new System.Windows.Forms.Padding(8);
            this.grpGlobal.Size = new System.Drawing.Size(880, 190);
            this.grpGlobal.TabIndex = 0;
            this.grpGlobal.TabStop = false;
            this.grpGlobal.Text = " 📌 全域設定變更項目 (Global Settings Changes) ";
            // 
            // dgvGlobalChanges
            // 
            this.dgvGlobalChanges.AllowUserToAddRows = false;
            this.dgvGlobalChanges.AllowUserToDeleteRows = false;
            this.dgvGlobalChanges.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGlobalChanges.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvGlobalChanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGlobalChanges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colProperty,
            this.colOldValue,
            this.colNewValue});
            this.dgvGlobalChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGlobalChanges.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.dgvGlobalChanges.Location = new System.Drawing.Point(8, 25);
            this.dgvGlobalChanges.Name = "dgvGlobalChanges";
            this.dgvGlobalChanges.ReadOnly = true;
            this.dgvGlobalChanges.RowHeadersVisible = false;
            this.dgvGlobalChanges.RowTemplate.Height = 24;
            this.dgvGlobalChanges.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGlobalChanges.Size = new System.Drawing.Size(864, 157);
            this.dgvGlobalChanges.TabIndex = 0;
            // 
            // colCategory
            // 
            this.colCategory.FillWeight = 25F;
            this.colCategory.HeaderText = "分類";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colProperty
            // 
            this.colProperty.FillWeight = 35F;
            this.colProperty.HeaderText = "設定項目";
            this.colProperty.Name = "colProperty";
            this.colProperty.ReadOnly = true;
            // 
            // colOldValue
            // 
            this.colOldValue.FillWeight = 40F;
            this.colOldValue.HeaderText = "原設定值 (Old)";
            this.colOldValue.Name = "colOldValue";
            this.colOldValue.ReadOnly = true;
            // 
            // colNewValue
            // 
            this.colNewValue.FillWeight = 40F;
            this.colNewValue.HeaderText = "新設定值 (New)";
            this.colNewValue.Name = "colNewValue";
            this.colNewValue.ReadOnly = true;
            // 
            // grpEquipments
            // 
            this.grpEquipments.Controls.Add(this.dgvEquipmentDiffs);
            this.grpEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEquipments.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpEquipments.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpEquipments.Location = new System.Drawing.Point(10, 10);
            this.grpEquipments.Name = "grpEquipments";
            this.grpEquipments.Padding = new System.Windows.Forms.Padding(8);
            this.grpEquipments.Size = new System.Drawing.Size(880, 256);
            this.grpEquipments.TabIndex = 0;
            this.grpEquipments.TabStop = false;
            this.grpEquipments.Text = " 🏭 機台設備差分狀態 (Equipments Live Reload Analysis) ";
            // 
            // dgvEquipmentDiffs
            // 
            this.dgvEquipmentDiffs.AllowUserToAddRows = false;
            this.dgvEquipmentDiffs.AllowUserToDeleteRows = false;
            this.dgvEquipmentDiffs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEquipmentDiffs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvEquipmentDiffs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipmentDiffs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEqStatus,
            this.colEqId,
            this.colEqName,
            this.colEqDetails});
            this.dgvEquipmentDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEquipmentDiffs.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.dgvEquipmentDiffs.Location = new System.Drawing.Point(8, 25);
            this.dgvEquipmentDiffs.Name = "dgvEquipmentDiffs";
            this.dgvEquipmentDiffs.ReadOnly = true;
            this.dgvEquipmentDiffs.RowHeadersVisible = false;
            this.dgvEquipmentDiffs.RowTemplate.Height = 24;
            this.dgvEquipmentDiffs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipmentDiffs.Size = new System.Drawing.Size(864, 223);
            this.dgvEquipmentDiffs.TabIndex = 0;
            // 
            // colEqStatus
            // 
            this.colEqStatus.FillWeight = 20F;
            this.colEqStatus.HeaderText = "處置狀態";
            this.colEqStatus.Name = "colEqStatus";
            this.colEqStatus.ReadOnly = true;
            // 
            // colEqId
            // 
            this.colEqId.FillWeight = 20F;
            this.colEqId.HeaderText = "機台編號";
            this.colEqId.Name = "colEqId";
            this.colEqId.ReadOnly = true;
            // 
            // colEqName
            // 
            this.colEqName.FillWeight = 30F;
            this.colEqName.HeaderText = "機台名稱";
            this.colEqName.Name = "colEqName";
            this.colEqName.ReadOnly = true;
            // 
            // colEqDetails
            // 
            this.colEqDetails.FillWeight = 70F;
            this.colEqDetails.HeaderText = "異動細節說明 (未變動者保持連線不中斷)";
            this.colEqDetails.Name = "colEqDetails";
            this.colEqDetails.ReadOnly = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(30)))));
            this.pnlBottom.Controls.Add(this.lblSummary);
            this.pnlBottom.Controls.Add(this.btnConfirm);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 550);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(900, 60);
            this.pnlBottom.TabIndex = 2;
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblSummary.ForeColor = System.Drawing.Color.Khaki;
            this.lblSummary.Location = new System.Drawing.Point(15, 12);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(630, 36);
            this.lblSummary.TabIndex = 2;
            this.lblSummary.Text = "統計資訊載入中...";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(128)))), ((int)(((byte)(75)))));
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(660, 12);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(120, 36);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "✔ 確認套用";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(790, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 36);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "✖ 放棄返回";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ConfigDiffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(900, 610);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTopBanner);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDiffForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "二次確認：設定檔修改項目檢閱";
            this.pnlTopBanner.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpGlobal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGlobalChanges)).EndInit();
            this.grpEquipments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipmentDiffs)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTopBanner;
        private System.Windows.Forms.Label lblBannerText;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpGlobal;
        private System.Windows.Forms.DataGridView dgvGlobalChanges;
        private System.Windows.Forms.GroupBox grpEquipments;
        private System.Windows.Forms.DataGridView dgvEquipmentDiffs;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEqDetails;
    }
}
