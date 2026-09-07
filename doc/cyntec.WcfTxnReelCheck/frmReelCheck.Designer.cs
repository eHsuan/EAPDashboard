namespace cyntec.Windows
{
    partial class frmReelCheck
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnWoQry = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReel_Scan_No = new System.Windows.Forms.TextBox();
            this.txtWoNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.txtMatNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMachNo = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReelID = new System.Windows.Forms.TextBox();
            this.btnReelBarcode = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.chkTPNo = new System.Windows.Forms.CheckBox();
            this.chkMatNo = new System.Windows.Forms.CheckBox();
            this.chk2D = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWOFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnWoQry
            // 
            this.btnWoQry.Font = new System.Drawing.Font("新細明體", 11.89565F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnWoQry.Location = new System.Drawing.Point(510, 30);
            this.btnWoQry.Name = "btnWoQry";
            this.btnWoQry.Size = new System.Drawing.Size(281, 34);
            this.btnWoQry.TabIndex = 0;
            this.btnWoQry.Text = "#2 標籤程式初始化(工單查詢)";
            this.btnWoQry.UseVisualStyleBackColor = true;
            this.btnWoQry.Click += new System.EventHandler(this.btnWoQry_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(35, 390);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 49;
            this.dataGridView1.RowTemplate.Height = 26;
            this.dataGridView1.Size = new System.Drawing.Size(846, 223);
            this.dataGridView1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 363);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "掃描狀態";
            // 
            // txtReel_Scan_No
            // 
            this.txtReel_Scan_No.Location = new System.Drawing.Point(97, 360);
            this.txtReel_Scan_No.Name = "txtReel_Scan_No";
            this.txtReel_Scan_No.Size = new System.Drawing.Size(499, 24);
            this.txtReel_Scan_No.TabIndex = 3;
            // 
            // txtWoNo
            // 
            this.txtWoNo.Location = new System.Drawing.Point(95, 30);
            this.txtWoNo.Name = "txtWoNo";
            this.txtWoNo.Size = new System.Drawing.Size(221, 24);
            this.txtWoNo.TabIndex = 4;
            this.txtWoNo.Text = "071226000031";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "工單:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "工號:";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(95, 66);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(221, 24);
            this.txtUserID.TabIndex = 7;
            this.txtUserID.Text = "736984";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "設備編號:";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(95, 111);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(221, 24);
            this.txtBatchNo.TabIndex = 9;
            this.txtBatchNo.Text = "HPA262800002";
            // 
            // txtMatNo
            // 
            this.txtMatNo.Location = new System.Drawing.Point(95, 154);
            this.txtMatNo.Name = "txtMatNo";
            this.txtMatNo.Size = new System.Drawing.Size(221, 24);
            this.txtMatNo.TabIndex = 10;
            this.txtMatNo.Text = "CYTNN8303AALMDCTAA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "TP No:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "料號:";
            // 
            // txtMachNo
            // 
            this.txtMachNo.Location = new System.Drawing.Point(97, 204);
            this.txtMachNo.Name = "txtMachNo";
            this.txtMachNo.Size = new System.Drawing.Size(219, 24);
            this.txtMachNo.TabIndex = 13;
            this.txtMachNo.Text = "TP0870";
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(97, 296);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(501, 58);
            this.txtMsg.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 309);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "訊息:";
            // 
            // txtReelID
            // 
            this.txtReelID.Location = new System.Drawing.Point(97, 249);
            this.txtReelID.Name = "txtReelID";
            this.txtReelID.Size = new System.Drawing.Size(219, 24);
            this.txtReelID.TabIndex = 16;
            this.txtReelID.Text = "{{{{{86{{{1ODKESE9QQQQEQQQS";
            // 
            // btnReelBarcode
            // 
            this.btnReelBarcode.Font = new System.Drawing.Font("新細明體", 11.89565F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnReelBarcode.Location = new System.Drawing.Point(510, 90);
            this.btnReelBarcode.Name = "btnReelBarcode";
            this.btnReelBarcode.Size = new System.Drawing.Size(281, 34);
            this.btnReelBarcode.TabIndex = 17;
            this.btnReelBarcode.Text = "#3 模擬Reel條碼掃描";
            this.btnReelBarcode.UseVisualStyleBackColor = true;
            this.btnReelBarcode.Click += new System.EventHandler(this.btnReelBarcode_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 15);
            this.label9.TabIndex = 18;
            this.label9.Text = "2D條碼:";
            // 
            // chkTPNo
            // 
            this.chkTPNo.AutoSize = true;
            this.chkTPNo.Location = new System.Drawing.Point(322, 116);
            this.chkTPNo.Name = "chkTPNo";
            this.chkTPNo.Size = new System.Drawing.Size(116, 19);
            this.chkTPNo.TabIndex = 19;
            this.chkTPNo.Text = "條碼輸入模擬";
            this.chkTPNo.UseVisualStyleBackColor = true;
            // 
            // chkMatNo
            // 
            this.chkMatNo.AutoSize = true;
            this.chkMatNo.Location = new System.Drawing.Point(322, 159);
            this.chkMatNo.Name = "chkMatNo";
            this.chkMatNo.Size = new System.Drawing.Size(116, 19);
            this.chkMatNo.TabIndex = 19;
            this.chkMatNo.Text = "條碼輸入模擬";
            this.chkMatNo.UseVisualStyleBackColor = true;
            // 
            // chk2D
            // 
            this.chk2D.AutoSize = true;
            this.chk2D.Checked = true;
            this.chk2D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk2D.Location = new System.Drawing.Point(322, 249);
            this.chk2D.Name = "chk2D";
            this.chk2D.Size = new System.Drawing.Size(116, 19);
            this.chk2D.TabIndex = 19;
            this.chk2D.Text = "條碼輸入模擬";
            this.chk2D.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("新細明體", 11.89565F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClose.Location = new System.Drawing.Point(510, 234);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(281, 34);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "#5 關閉標籤程式";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnWOFinish
            // 
            this.btnWOFinish.Font = new System.Drawing.Font("新細明體", 11.89565F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnWOFinish.Location = new System.Drawing.Point(510, 157);
            this.btnWOFinish.Name = "btnWOFinish";
            this.btnWOFinish.Size = new System.Drawing.Size(281, 34);
            this.btnWOFinish.TabIndex = 21;
            this.btnWOFinish.Text = "#4 工單結束作業";
            this.btnWOFinish.UseVisualStyleBackColor = true;
            this.btnWOFinish.Click += new System.EventHandler(this.btnWOFinish_Click);
            // 
            // frmReelCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 625);
            this.Controls.Add(this.btnWOFinish);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chk2D);
            this.Controls.Add(this.chkMatNo);
            this.Controls.Add(this.chkTPNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnReelBarcode);
            this.Controls.Add(this.txtReelID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtMachNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMatNo);
            this.Controls.Add(this.txtBatchNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWoNo);
            this.Controls.Add(this.txtReel_Scan_No);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnWoQry);
            this.Name = "frmReelCheck";
            this.Text = "cyntec.WcfTxnSampleUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReelCheck_FormClosing);
            this.Load += new System.EventHandler(this.frmTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWoQry;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReel_Scan_No;
        private System.Windows.Forms.TextBox txtWoNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.TextBox txtMatNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMachNo;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtReelID;
        private System.Windows.Forms.Button btnReelBarcode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkTPNo;
        private System.Windows.Forms.CheckBox chkMatNo;
        private System.Windows.Forms.CheckBox chk2D;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWOFinish;
    }
}