using System;
using System.Drawing;
using System.Windows.Forms;
using EAPController.Models;

namespace EAPController.Controls
{
    public class EapMonitorCard : UserControl
    {
        private Label lblId;
        private Label lblStatus;
        private Label lblVersion;
        private Label lblIp;
        private readonly string _eapId;
        private EapStatusModel _model;

        public string EapId
        {
            get { return _eapId; }
        }

        public string LastRawJson { get; set; }
        public event Action<EapMonitorCard> CardClicked;

        public EapMonitorCard(string eapId)
        {
            _eapId = eapId;
            InitializeComponent();
            lblId.Text = eapId;

            // 點擊穿透處理
            this.Click += new EventHandler(delegate { if (CardClicked != null) CardClicked(this); });
            foreach (Control c in this.Controls)
            {
                c.Click += new EventHandler(delegate { if (CardClicked != null) CardClicked(this); });
            }
        }

        private void InitializeComponent()
        {
            this.lblId = new Label();
            this.lblStatus = new Label();
            this.lblVersion = new Label();
            this.lblIp = new Label();
            this.SuspendLayout();

            this.lblId.Font = new Font("Microsoft JhengHei", 14F, FontStyle.Bold);
            this.lblId.Location = new Point(10, 10);
            this.lblId.Size = new Size(280, 30);
            this.lblId.Text = "EAP-ID";

            this.lblStatus.Font = new Font("Microsoft JhengHei", 12F);
            this.lblStatus.Location = new Point(10, 45);
            this.lblStatus.Size = new Size(280, 25);
            this.lblStatus.Text = "Status: Unknown";

            this.lblVersion.Font = new Font("Microsoft JhengHei", 10F);
            this.lblVersion.Location = new Point(10, 75);
            this.lblVersion.Size = new Size(280, 20);
            this.lblVersion.Text = "Version: -";

            this.lblIp.Font = new Font("Microsoft JhengHei", 10F);
            this.lblIp.Location = new Point(10, 100);
            this.lblIp.Size = new Size(280, 20);
            this.lblIp.Text = "IP: -";

            this.Controls.Add(lblId);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblVersion);
            this.Controls.Add(lblIp);

            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(300, 150);
            this.Padding = new Padding(5);
            this.ResumeLayout(false);
        }

        public void UpdateStatus(EapStatusModel model)
        {
            _model = model;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(delegate { UpdateStatus(model); }));
                return;
            }

            lblId.Text = model.EapId;
            lblStatus.Text = string.Format("Status: {0}", model.Status);
            lblVersion.Text = string.Format("Version: {0}", model.Version);
            lblIp.Text = string.Format("IP: {0}", model.Ip);

            // 判斷顏色
            TimeSpan diff = DateTime.Now - model.LastUpdated;
            if (diff.TotalSeconds > 30)
            {
                this.BackColor = Color.Crimson; // 紅燈：無回應
                lblStatus.Text = "Status: Disconnected";
                lblStatus.ForeColor = Color.White;
                lblId.ForeColor = Color.White;
            }
            else if (model.Status == "Restarting")
            {
                this.BackColor = Color.Orange; // 橘色：重啟中
                lblStatus.ForeColor = Color.Black;
                lblId.ForeColor = Color.Black;
            }
            else if (model.HasException)
            {
                this.BackColor = Color.Gold; // 黃燈：異常
                lblStatus.ForeColor = Color.Black;
                lblId.ForeColor = Color.Black;
            }
            else
            {
                this.BackColor = Color.LimeGreen; // 綠燈：正常
                lblStatus.ForeColor = Color.Black;
                lblId.ForeColor = Color.Black;
            }
        }
    }
}
