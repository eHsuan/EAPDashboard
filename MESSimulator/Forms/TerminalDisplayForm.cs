using System;
using System.Drawing;
using System.Windows.Forms;

namespace MESSimulator.Forms
{
    public partial class TerminalDisplayForm : Form
    {
        public string TerminalMessage => txtMessage.Text.Trim();

        public TerminalDisplayForm(string eqpId, string topic)
        {
            InitializeComponent();
            lblTargetInfo.Text = $"目標機台: {eqpId} | Topic: {topic}";
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("請輸入要顯示於機台螢幕的文字訊息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
