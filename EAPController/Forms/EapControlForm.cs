using System;
using System.Drawing;
using System.Windows.Forms;
using EAPController.Models;
using Newtonsoft.Json;

namespace EAPController.Forms
{
    public class EapControlForm : Form
    {
        private TextBox txtJson;
        private Button btnRestart;
        private Label lblInfo;
        private readonly string _eapId;
        private readonly MainForm _parent;

        public EapControlForm(string eapId, string initialJson, MainForm parent)
        {
            _eapId = eapId;
            _parent = parent;
            InitializeComponent();
            this.Text = string.Format("Control Panel - {0}", eapId);
            txtJson.Text = initialJson;
        }

        private void InitializeComponent()
        {
            this.txtJson = new TextBox();
            this.btnRestart = new Button();
            this.lblInfo = new Label();
            this.SuspendLayout();

            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new Point(12, 15);
            this.lblInfo.Text = string.Format("Latest JSON Report from {0}:", _eapId);

            this.txtJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtJson.Location = new Point(12, 40);
            this.txtJson.Multiline = true;
            this.txtJson.ReadOnly = true;
            this.txtJson.ScrollBars = ScrollBars.Vertical;
            this.txtJson.Size = new Size(560, 300);
            this.txtJson.Font = new Font("Consolas", 9F);

            this.btnRestart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnRestart.Location = new Point(452, 350);
            this.btnRestart.Size = new Size(120, 40);
            this.btnRestart.Text = "Safe Restart";
            this.btnRestart.BackColor = Color.OrangeRed;
            this.btnRestart.ForeColor = Color.White;
            this.btnRestart.FlatStyle = FlatStyle.Flat;
            this.btnRestart.Click += BtnRestart_Click;

            this.ClientSize = new Size(584, 402);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtJson);
            this.Controls.Add(this.btnRestart);
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void BtnRestart_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                string.Format("Are you sure you want to trigger a SAFE RESTART for {0}?\n\nThis will stop new requests and wait for pending tasks to finish.", _eapId),
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (res == DialogResult.Yes)
            {
                await _parent.SendRestartCommand(_eapId);
                MessageBox.Show("Safe Restart Command Sent.");
                this.Close();
            }
        }

        public void UpdateJson(string json)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateJson(json))); return; }
            txtJson.Text = json;
        }
    }
}
