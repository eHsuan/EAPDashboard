using System;
using System.Drawing;
using System.Windows.Forms;

namespace EAPDashboard
{
    public class PasswordPromptForm : Form
    {
        private readonly string _targetPassword;

        private Label lblPrompt;
        private TextBox txtPassword;
        private Button btnOk;
        private Button btnCancel;

        public PasswordPromptForm(string targetPassword, string title = "系統關閉授權驗證", string prompt = "請輸入關閉程式之授權密碼：", string okButtonText = "確認關閉")
        {
            _targetPassword = !string.IsNullOrEmpty(targetPassword) ? targetPassword : "CIM";
            InitializeComponent();

            if (!string.IsNullOrEmpty(title)) this.Text = title;
            if (!string.IsNullOrEmpty(prompt)) this.lblPrompt.Text = prompt;
            if (!string.IsNullOrEmpty(okButtonText)) this.btnOk.Text = okButtonText;
        }

        private void InitializeComponent()
        {
            this.lblPrompt = new Label();
            this.txtPassword = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // 視窗屬性設定
            this.Text = "系統關閉授權驗證";
            this.ClientSize = new Size(380, 160);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            // 提示文字標籤
            this.lblPrompt.Text = "請輸入關閉程式之授權密碼：";
            this.lblPrompt.Location = new Point(25, 20);
            this.lblPrompt.Size = new Size(330, 25);
            this.lblPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // 密碼輸入框
            this.txtPassword.Location = new Point(25, 55);
            this.txtPassword.Size = new Size(330, 27);
            this.txtPassword.Font = new Font("Segoe UI", 11F);
            this.txtPassword.UseSystemPasswordChar = true;

            // 確認按鈕
            this.btnOk.Text = "確認關閉";
            this.btnOk.Location = new Point(145, 105);
            this.btnOk.Size = new Size(100, 34);
            this.btnOk.FlatStyle = FlatStyle.Flat;
            this.btnOk.BackColor = Color.FromArgb(217, 83, 79);
            this.btnOk.ForeColor = Color.White;
            this.btnOk.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnOk.Click += BtnOk_Click;

            // 取消按鈕
            this.btnCancel.Text = "取消";
            this.btnCancel.Location = new Point(255, 105);
            this.btnCancel.Size = new Size(100, 34);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9.5F);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // 快捷鍵關聯
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;

            // 控制項加入
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.Equals(this.txtPassword.Text, _targetPassword, StringComparison.Ordinal))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string msg = this.Text.Contains("設定") ? "密碼錯誤，禁止開啟設定頁面！" : "密碼錯誤，禁止關閉程式！";
                MessageBox.Show(this, msg, "授權失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPassword.Clear();
                this.txtPassword.Focus();
            }
        }
    }
}
