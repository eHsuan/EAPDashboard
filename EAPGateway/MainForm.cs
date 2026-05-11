using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using EAPGateway.Models;
using EAPGateway.Services;

namespace EAPGateway
{
    public partial class MainForm : Form
    {
        private readonly GatewayConfig _config;
        private readonly DatabaseService _dbService;
        private readonly LogService _logger;

        public MainForm(GatewayConfig config, DatabaseService dbService, LogService logger)
        {
            InitializeComponent();
            _config = config;
            _dbService = dbService;
            _logger = logger;

            _logger.OnLog += (time, level, msg, eqId) => Log(msg);
            
            lblPrimary.Text = $"Primary: {ExtractServer(_config.PrimaryDb)}";
            lblSecondary.Text = $"Secondary: {ExtractServer(_config.SecondaryDb)}";
            lblPort.Text = $"Listening on Port: {_config.Port}";

            _logger.Info("EAPGateway System UI initialized.");

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += async (s, e) => await CheckDbStatus();
            timer.Start();
        }

        private string ExtractServer(string connStr)
        {
            try {
                var builder = new SqlConnectionStringBuilder(connStr);
                return builder.DataSource;
            } catch { return "Unknown"; }
        }

        private async Task CheckDbStatus()
        {
            pbPrimary.BackColor = await TestConn(_config.PrimaryDb) ? Color.LimeGreen : Color.Crimson;
            pbSecondary.BackColor = await TestConn(_config.SecondaryDb) ? Color.LimeGreen : Color.Crimson;
        }

        private async Task<bool> TestConn(string connStr)
        {
            if (string.IsNullOrEmpty(connStr)) return false;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    return true;
                }
            }
            catch { return false; }
        }

        private void Log(string msg)
        {
            if (this.IsDisposed) return;
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(msg)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
            if (txtLog.TextLength > 20000) txtLog.Clear();
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }
    }
}
