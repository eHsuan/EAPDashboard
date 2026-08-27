using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EAPDashboard.Models;

namespace EAPDashboard
{
    public class TerminalDisplayForm : Form
    {
        private List<EquipmentBase> _equipments;

        private Label lblEquipment;
        private ComboBox cmbEquipments;
        private Label lblMessage;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnCancel;

        public TerminalDisplayForm(List<EquipmentBase> equipments)
        {
            _equipments = equipments ?? new List<EquipmentBase>();
            InitializeComponent();
            PopulateEquipments();
        }

        private void InitializeComponent()
        {
            this.lblEquipment = new Label();
            this.cmbEquipments = new ComboBox();
            this.lblMessage = new Label();
            this.txtMessage = new TextBox();
            this.btnSend = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            // Form settings
            this.Text = "遠端發送訊息 (Terminal Display)";
            this.Size = new Size(500, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // lblEquipment
            this.lblEquipment.Text = "目標設備 (Target Equipment):";
            this.lblEquipment.Location = new Point(20, 20);
            this.lblEquipment.Size = new Size(440, 20);
            this.lblEquipment.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // cmbEquipments
            this.cmbEquipments.Location = new Point(20, 45);
            this.cmbEquipments.Size = new Size(440, 25);
            this.cmbEquipments.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEquipments.Font = new Font("Segoe UI", 9.5F);

            // lblMessage
            this.lblMessage.Text = "顯示訊息內容 (Terminal Message):";
            this.lblMessage.Location = new Point(20, 85);
            this.lblMessage.Size = new Size(440, 20);
            this.lblMessage.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // txtMessage
            this.txtMessage.Location = new Point(20, 110);
            this.txtMessage.Size = new Size(440, 110);
            this.txtMessage.Multiline = true;
            this.txtMessage.ScrollBars = ScrollBars.Vertical;
            this.txtMessage.Font = new Font("Segoe UI", 10F);

            // btnSend
            this.btnSend.Text = "發送 (Send)";
            this.btnSend.Location = new Point(260, 235);
            this.btnSend.Size = new Size(95, 32);
            this.btnSend.FlatStyle = FlatStyle.Flat;
            this.btnSend.BackColor = Color.FromArgb(0, 122, 204);
            this.btnSend.ForeColor = Color.White;
            this.btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSend.Click += BtnSend_Click;

            // btnCancel
            this.btnCancel.Text = "取消 (Cancel)";
            this.btnCancel.Location = new Point(365, 235);
            this.btnCancel.Size = new Size(95, 32);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F);
            this.btnCancel.Click += (s, e) => this.Close();

            // Controls Add
            this.Controls.Add(this.lblEquipment);
            this.Controls.Add(this.cmbEquipments);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
        }

        private void PopulateEquipments()
        {
            cmbEquipments.Items.Clear();
            foreach (var eq in _equipments)
            {
                cmbEquipments.Items.Add($"{eq.Id} - {eq.Name} ({eq.Protocol}) [{eq.Connection}]");
            }
            if (cmbEquipments.Items.Count > 0)
            {
                cmbEquipments.SelectedIndex = 0;
            }
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (cmbEquipments.SelectedIndex < 0 || cmbEquipments.SelectedIndex >= _equipments.Count)
            {
                MessageBox.Show("請選擇目標設備！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rawMsg = txtMessage.Text?.Trim();
            if (string.IsNullOrEmpty(rawMsg))
            {
                MessageBox.Show("請輸入欲發送的訊息內容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sanitizedMsg = EquipmentBase.SanitizeTerminalMessage(rawMsg);
            if (string.IsNullOrEmpty(sanitizedMsg))
            {
                MessageBox.Show("訊息內容必須包含英文字母或數字！(Terminal 僅支援 ASCII 字元)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var targetEq = _equipments[cmbEquipments.SelectedIndex];
            btnSend.Enabled = false;
            try
            {
                await targetEq.SendTerminalDisplay(sanitizedMsg);
                MessageBox.Show($"訊息已成功發送至設備 [{targetEq.Id}]！\n發送內容: {sanitizedMsg}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發送訊息失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }
    }
}
