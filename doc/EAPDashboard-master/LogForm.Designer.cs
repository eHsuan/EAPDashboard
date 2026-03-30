namespace EAPDashboard
{
    partial class LogForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox richTextBoxLogs;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnLiveView;
        private System.Windows.Forms.SaveFileDialog saveFileDialogCsv;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTo;

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
            this.richTextBoxLogs = new System.Windows.Forms.RichTextBox();
            this.panelControls = new System.Windows.Forms.Panel();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnLiveView = new System.Windows.Forms.Button();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.saveFileDialogCsv = new System.Windows.Forms.SaveFileDialog();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.richTextBoxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLogs.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLogs.ForeColor = System.Drawing.Color.Gainsboro;
            this.richTextBoxLogs.Location = new System.Drawing.Point(0, 58);
            this.richTextBoxLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.ReadOnly = true;
            this.richTextBoxLogs.Size = new System.Drawing.Size(1067, 461);
            this.richTextBoxLogs.TabIndex = 0;
            this.richTextBoxLogs.Text = "";
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.labelTo);
            this.panelControls.Controls.Add(this.labelFrom);
            this.panelControls.Controls.Add(this.btnExport);
            this.panelControls.Controls.Add(this.btnSearch);
            this.panelControls.Controls.Add(this.btnLiveView);
            this.panelControls.Controls.Add(this.dateTimePickerEnd);
            this.panelControls.Controls.Add(this.dateTimePickerStart);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1067, 58);
            this.panelControls.TabIndex = 1;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(320, 21);
            this.labelTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(27, 15);
            this.labelTo.TabIndex = 5;
            this.labelTo.Text = "To:";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(16, 21);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(42, 15);
            this.labelFrom.TabIndex = 4;
            this.labelFrom.Text = "From:";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(767, 14);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 29);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "匯出 CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(639, 14);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 29);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查詢";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnLiveView
            // 
            this.btnLiveView.Location = new System.Drawing.Point(895, 14);
            this.btnLiveView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLiveView.Name = "btnLiveView";
            this.btnLiveView.Size = new System.Drawing.Size(120, 29);
            this.btnLiveView.TabIndex = 4;
            this.btnLiveView.Text = "即時日誌";
            this.btnLiveView.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(359, 16);
            this.dateTimePickerEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(225, 25);
            this.dateTimePickerEnd.TabIndex = 1;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(68, 16);
            this.dateTimePickerStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(225, 25);
            this.dateTimePickerStart.TabIndex = 0;
            // 
            // saveFileDialogCsv
            // 
            this.saveFileDialogCsv.DefaultExt = "csv";
            this.saveFileDialogCsv.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            this.saveFileDialogCsv.Title = "匯出日誌";
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 519);
            this.Controls.Add(this.richTextBoxLogs);
            this.Controls.Add(this.panelControls);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "LogForm";
            this.Text = "Equipment Log";
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
