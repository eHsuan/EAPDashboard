using Microsoft.Data.SqlClient;
using EAPGateway.Models;
using EAPGateway.Services;

namespace EAPGateway
{
    public partial class MainForm : Form
    {
        private readonly GatewayConfig _config;
        private readonly DatabaseService _dbService;
        private readonly RetryBackgroundService _retryService;

        public MainForm(GatewayConfig config, DatabaseService dbService, RetryBackgroundService retryService)
        {
            InitializeComponent();
            _config = config;
            _dbService = dbService;
            _retryService = retryService;

            _dbService.OnLog += Log;
            _retryService.OnLog += Log;
            
            lblPrimary.Text = $"Primary: {ExtractServer(_config.PrimaryDb)}";
            lblSecondary.Text = $"Secondary: {ExtractServer(_config.SecondaryDb)}";
            lblPort.Text = $"Listening on Port: {_config.Port}";

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
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(msg)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
            if (txtLog.TextLength > 10000) txtLog.Clear();
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }
    }
}
