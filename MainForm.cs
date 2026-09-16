using CyntecMqttLib.Interfaces;
using EAPDashboard.Controls;
using EAPDashboard.Models;
using Newtonsoft.Json;
using Protocol.LocalData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EAPDashboard
{
    public class PostgresDbSettings
    {
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; } = true;
        public int RetentionDays { get; set; } = 14;
    }

    public class AppConfig
    {
        public string ExitPassword { get; set; } = "admin";
        public string EapId { get; set; }
        public string ControllerTopic { get; set; }
        public string MqttBrokerHost { get; set; }
        public int MqttBrokerPort { get; set; }
        public int HeartbeatIntervalSeconds { get; set; }
        public int EqpStatusReportIntervalSeconds { get; set; } = 60;
        public bool EnableCassetteBinding { get; set; } = false;
        public bool MinimizeToTray { get; set; } = false;
        public MesSettings MesSettings { get; set; }
        public YieldSettings YieldSettings { get; set; } = new YieldSettings();
        public EAPDashboard.Models.DbSettings DbSettings { get; set; }
        public PostgresDbSettings PostgresDbSettings { get; set; }
        public LogSettings LogSettings { get; set; }
        public List<EquipmentConfig> Equipments { get; set; }
    }

    public class YieldSettings
    {
        public double MinYieldRate { get; set; } = 0.95;
        public double MaxYieldRate { get; set; } = 1.00;
    }

    public class MesSettings
    {
        public string ProductionUrl { get; set; }
        public string TestUrl { get; set; }
        public string ActiveEnvironment { get; set; }
        public List<string> FilterNullApis { get; set; } = new List<string>();
    }

    public class EquipmentConfig
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public ushort DeviceId { get; set; }
        public bool IsActive { get; set; } = true;
        public string DictionaryPath { get; set; }
        public string Topic { get; set; }
        public string TimeFormat { get; set; } = "16";
        public string StatusType { get; set; } = "Delta";
        public bool? EnableCassetteBinding { get; set; }
        public int? EqpStatusReportIntervalSeconds { get; set; }
        public bool AutoReportStatus { get; set; } = false;
    }

    public partial class MainForm : Form
    {
        private List<EquipmentBase> _equipments;
        private AppConfig _appConfig;
        private CancellationTokenSource _mainCts;
        private MqttBus _bus;
        private LocalDataService _localDataService;
        private System.Timers.Timer _partitionTimer;
        private bool _isInternalRestart = false;
        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _trayContextMenu;
        private FormWindowState _previousWindowState = FormWindowState.Maximized;
        private bool _isHandlingResize = false;

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; 
            this.Resize += MainForm_Resize;
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
            this.FormClosed += MainForm_FormClosed;
            _mainCts = new CancellationTokenSource();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfig();  
            InitializeNotifyIcon();
            PopulateDashboard();
            AdjustCardSize();

            // 立即啟動設備通訊 (不等待 MQTT 連線)
            StartEquipments();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isInternalRestart) return;

            // 僅攔截使用者手動關閉行為 (右上角 X 按鈕、工作列右鍵關閉、Alt+F4)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                string targetPassword = _appConfig?.ExitPassword;
                if (string.IsNullOrEmpty(targetPassword))
                {
                    targetPassword = "admin";
                }

                using (var dlg = new PasswordPromptForm(targetPassword))
                {
                    dlg.StartPosition = FormStartPosition.CenterScreen;
                    dlg.TopMost = true;
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
            if (_trayContextMenu != null)
            {
                _trayContextMenu.Dispose();
                _trayContextMenu = null;
            }

            _mainCts?.Cancel();
            _mainCts?.Dispose();
            foreach (var eq in _equipments) eq.Stop();
            if (_bus != null) _bus.Dispose();
        }

        private void LoadConfig()
        {
            _equipments = new List<EquipmentBase>();
            var path = Path.Combine(Application.StartupPath, "config.json");
            if (!File.Exists(path)) return;
            try {
                var json = File.ReadAllText(path);
                _appConfig = JsonConvert.DeserializeObject<AppConfig>(json);

                var ver = typeof(MainForm).Assembly.GetName().Version;
                string versionStr = ver != null ? $"{ver.Major}.{ver.Minor}.{ver.Build}" : Application.ProductVersion?.Split('+')[0];

                if (_appConfig != null && !string.IsNullOrEmpty(_appConfig.EapId))
                {
                    this.Text = $"EAP Dashboard v{versionStr} - {_appConfig.EapId}";
                }
                else
                {
                    this.Text = $"EAP Dashboard v{versionStr}";
                }
                if (_appConfig.MesSettings != null) {
                    string env = _appConfig.MesSettings.ActiveEnvironment ?? "Production";
                    string targetUrl = env.Equals("Production", StringComparison.OrdinalIgnoreCase) ? _appConfig.MesSettings.ProductionUrl : _appConfig.MesSettings.TestUrl;
                    if (!string.IsNullOrEmpty(targetUrl))
                    {
                        try {
                            var asm = System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Protocol.MES") 
                                       ?? System.Reflection.Assembly.Load("Protocol.MES");
                            var settingsType = asm?.GetType("Protocol.MES.Properties.Settings");
                            if (settingsType != null)
                            {
                                var defaultProp = settingsType.GetProperty("Default", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                                var defaultObj = defaultProp?.GetValue(null);
                                if (defaultObj != null)
                                {
                                    var itemProp = settingsType.GetProperty("Item");
                                    itemProp?.SetValue(defaultObj, targetUrl, new object[] { "Protocol_MES_corp_delta_twcynmes01_Eqp_Portal" });
                                }
                            }
                        } catch { }
                    }

                    if (_appConfig.MesSettings.FilterNullApis != null && _appConfig.MesSettings.FilterNullApis.Count > 0)
                    {
                        MesJsonContractResolver.SetWhitelist(_appConfig.MesSettings.FilterNullApis);
                    }
                }
                bool isSim = !(_appConfig.MesSettings?.ActiveEnvironment ?? "Production").Equals("Production", StringComparison.OrdinalIgnoreCase);

                if (_appConfig.PostgresDbSettings != null && _appConfig.PostgresDbSettings.Enabled && !string.IsNullOrEmpty(_appConfig.PostgresDbSettings.ConnectionString))
                {
                    try
                    {
                        _localDataService = new LocalDataService(_appConfig.PostgresDbSettings.ConnectionString);
                        
                        var activeEqs = _appConfig.Equipments?
                            .Where(e => e.IsActive)
                            .Select(e => (e.Id, (string?)e.Name))
                            .ToArray();

                        if (activeEqs != null && activeEqs.Length > 0)
                        {
                            _localDataService.SyncEquipments(activeEqs);
                        }

                        int retention = _appConfig.PostgresDbSettings.RetentionDays > 0 ? _appConfig.PostgresDbSettings.RetentionDays : 14;
                        _localDataService.EnsureParameterLogPartitions(retentionDays: retention, lookaheadDays: 2);

                        _partitionTimer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
                        _partitionTimer.Elapsed += (s, e) => {
                            try { _localDataService?.EnsureParameterLogPartitions(retentionDays: retention, lookaheadDays: 2); } catch { }
                        };
                        _partitionTimer.AutoReset = true;
                        _partitionTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"LocalDataService Init Failed: {ex.Message}");
                    }
                }

                // 啟動日誌生命週期維運管理（歷史壓縮、過期刪除）
                try
                {
                    LogMaintenanceService.Start(_appConfig.LogSettings ?? new LogSettings());
                }
                catch { }

                if (_appConfig.Equipments != null) {
                    foreach (var config in _appConfig.Equipments) {
                        if (!config.IsActive) continue;

                        EquipmentBase eq = null;

                        string dictPath = !string.IsNullOrEmpty(config.DictionaryPath)
                            ? (Path.IsPathRooted(config.DictionaryPath) ? config.DictionaryPath : Path.Combine(Application.StartupPath, config.DictionaryPath))
                            : null;

                        if (config.Protocol == "MQTT") {
                            eq = new MqttEquipment { 
                                Id = config.Id, 
                                Name = config.Name, 
                                Topic = config.Topic, 
                                StatusType = config.StatusType ?? "Delta",
                                DictionaryPath = dictPath,
                                DbConfig = _appConfig.DbSettings,
                                IsMesSimulator = _appConfig.MesSettings.ActiveEnvironment == "Test"? true : false,
                                LocalDataServiceInstance = _localDataService
                            };
                        }
                        else if (config.Protocol == "SECS") {
                            eq = new SecsEquipment { 
                                Id = config.Id, 
                                Name = config.Name, 
                                Ip = config.Ip, 
                                Port = config.Port, 
                                DeviceId = config.DeviceId, 
                                IsActive = config.IsActive, 
                                DictionaryPath = dictPath, 
                                DbConfig = _appConfig.DbSettings, 
                                TimeFormat = string.IsNullOrEmpty(config.TimeFormat) ? "16" : config.TimeFormat,
                                LocalDataServiceInstance = _localDataService
                            };
                        }
                        
                        if (eq != null) {
                            eq.IsSimulator = isSim;
                            eq.EnableCassetteBinding = config.EnableCassetteBinding ?? _appConfig.EnableCassetteBinding;
                            eq.StatusReportIntervalSeconds = config.EqpStatusReportIntervalSeconds ?? _appConfig.EqpStatusReportIntervalSeconds;
                            eq.AutoReportStatus = config.AutoReportStatus;
                            eq.PostgresConnectionString = _appConfig.PostgresDbSettings?.ConnectionString;
                            eq.MinYieldRate = _appConfig.YieldSettings?.MinYieldRate ?? 0.95;
                            eq.MaxYieldRate = _appConfig.YieldSettings?.MaxYieldRate ?? 1.00;
                            _equipments.Add(eq);
                        }
                    }
                }
                foreach (var eq in _equipments) eq.LoadAlarmMappings();
            } catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void PopulateDashboard()
        {
            flowLayoutPanel.Controls.Clear();
            foreach (var eq in _equipments) {
                eq.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(EquipmentBase.State)) SaveStates(); };
                flowLayoutPanel.Controls.Add(new EquipmentCard(eq));

                if (eq.IsActive)
                {
                    // 方案 A：啟動時背景非同步向 MES 查詢設備主檔並同步至 LocalData
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await eq.ProcessGetEqpInfoAsync();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[GetEqpInfo] Background init failed for {eq.Id}: {ex.Message}");
                        }
                    });
                }
            }
            InitMqtt();
        }

        private async void InitMqtt()
        {
            try {
                // EAP uses a fixed ClientID and CleanSession = false to ensure no message loss
                _bus = new MqttBus(
                    host: _appConfig.MqttBrokerHost ?? "127.0.0.1", 
                    port: _appConfig.MqttBrokerPort > 0 ? _appConfig.MqttBrokerPort : 1883, 
                    clientId: string.Format("EAP_{0}_{1}", _appConfig.EapId, Dns.GetHostName()),
                    username: null,
                    password: null,
                    cleanSession: false
                );
                _bus.Connected += async () => {
                    UpdateBrokerStatus(true);
                    await SubscribeToControllerCommands();
                };
                _bus.Disconnected += () => UpdateBrokerStatus(false);
                await _bus.ConnectAsync();

                // 啟動報工任務
                _ = Task.Run(() => ReportStatusToControllerLoop(_mainCts.Token));
            } catch { UpdateBrokerStatus(false); }
        }

        private async Task SubscribeToControllerCommands()
        {
            if (string.IsNullOrEmpty(_appConfig.ControllerTopic) || string.IsNullOrEmpty(_appConfig.EapId)) return;
            string topic = $"{_appConfig.ControllerTopic}/{_appConfig.EapId}/Command";
            await _bus.SubscribeAsync(topic, (t, payload) => {
                HandleControllerCommand(payload);
            });
        }

        private void HandleControllerCommand(string json)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[ControllerCommand] Received: {0}", json));
                var cmd = JsonConvert.DeserializeAnonymousType(json, new { CommandType = "", Timestamp = "" });
                
                if (cmd != null && cmd.CommandType == "SafeRestart")
                {
                    _ = Task.Run(() => ExecuteSafeRestart());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[ControllerCommand] Parse Error: {0}", ex.Message));
            }
        }

        private async Task ExecuteSafeRestart()
        {
            System.Diagnostics.Debug.WriteLine("!!! Remote Safe Restart Command Received. Starting Shutdown Procedure...");

            // 1. 標記進入停止階段 (禁收新 Request)
            foreach (var eq in _equipments) eq.IsStopping = true;

            // 2. 等待業務消化 (最大等待 20 秒)
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 20)
            {
                int totalActive = _equipments.Sum(eq => eq.ActiveTaskCount);
                if (totalActive == 0) break;
                
                System.Diagnostics.Debug.WriteLine(string.Format("[Shutdown] Waiting for {0} active tasks to complete...", totalActive));
                await Task.Delay(1000);
            }

            // 3. 安全中斷與清理
            System.Diagnostics.Debug.WriteLine("[Shutdown] Business drained. Disconnecting all services...");
            
            // 發送最後一筆重啟中狀態
            if (_bus != null && _bus.IsConnected)
            {
                var report = new { EapId = _appConfig.EapId, Status = "Restarting", Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string topic = string.Format("{0}/{1}/Report", _appConfig.ControllerTopic, _appConfig.EapId);
                await _bus.PublishAsync(topic, report, 0, true);
                await Task.Delay(500); // 確保訊息發出
            }

            foreach (var eq in _equipments) eq.Stop();
            if (_bus != null) _bus.Dispose();

            // 4. 執行重啟
            System.Diagnostics.Debug.WriteLine("[Shutdown] All services cleaned. Restarting Application NOW.");
            _isInternalRestart = true;
            Application.Restart();
        }

        private async Task ReportStatusToControllerLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (_bus != null && _bus.IsConnected && !string.IsNullOrEmpty(_appConfig.ControllerTopic) && !string.IsNullOrEmpty(_appConfig.EapId))
                {
                    try
                    {
                        var report = new
                        {
                            EapId = _appConfig.EapId,
                            Status = "Running", // 基本狀態
                            Version = Application.ProductVersion,
                            Ip = GetLocalIpAddress(),
                            HasException = _equipments.Any(eq => eq.Connection == ConnectionStatus.Disconnected), // 簡易判定異常
                            DeviceStatus = _equipments.ToDictionary(eq => eq.Id, eq => eq.Connection.ToString()),
                            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        string topic = string.Format("{0}/{1}/Report", _appConfig.ControllerTopic, _appConfig.EapId);
                        string payload = JsonConvert.SerializeObject(report);
                        
                        System.Diagnostics.Debug.WriteLine(string.Format("[ControllerReport] Publishing to {0}", topic));
                        
                        // QoS = 0, Retain = True
                        await _bus.PublishAsync(topic, payload, 0, true);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ControllerReport] Error: {ex.Message}");
                    }
                }
                await Task.Delay(10000, ct); // 每 10 秒報工一次
            }
        }

        private string GetLocalIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) return ip.ToString();
                }
            }
            catch { }
            return "127.0.0.1";
        }

        private void StartEquipments()
        {
            foreach (var eq in _equipments) eq.Start(_mainCts.Token, _bus, _appConfig.HeartbeatIntervalSeconds);
        }

        private void UpdateBrokerStatus(bool isConnected)
        {
            if (lblBrokerStatus.InvokeRequired) { lblBrokerStatus.Invoke(new Action(() => UpdateBrokerStatus(isConnected))); return; }
            lblBrokerStatus.ForeColor = isConnected ? Color.Lime : Color.Red;
            lblBrokerStatus.Text = isConnected ? "● MQTT Broker: Connected" : "● MQTT Broker: Disconnected";
        }

        private void SaveStates()
        {
            try {
                var dict = new Dictionary<string, object>();
                foreach (var eq in _equipments) dict[eq.Id] = new { eq.State, eq.CurrentRecipe };
                File.WriteAllText(Path.Combine(Application.StartupPath, "equipment_state.json"), JsonConvert.SerializeObject(dict));
            } catch { }
        }

        private Icon LoadApplicationIcon()
        {
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "EAP.ico");
                if (File.Exists(iconPath))
                {
                    return new Icon(iconPath);
                }
            }
            catch { }

            try
            {
                var exeIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (exeIcon != null) return exeIcon;
            }
            catch { }

            return SystemIcons.Application;
        }

        private void InitializeNotifyIcon()
        {
            var appIcon = LoadApplicationIcon();
            if (appIcon != null)
            {
                this.Icon = appIcon;
            }

            _trayContextMenu = new ContextMenuStrip();
            var itemShow = new ToolStripMenuItem("開啟主視窗", null, (s, e) => RestoreFromTray());
            itemShow.Font = new Font(_trayContextMenu.Font, FontStyle.Bold);
            var itemExit = new ToolStripMenuItem("結束", null, (s, e) => this.Close());
            _trayContextMenu.Items.Add(itemShow);
            _trayContextMenu.Items.Add(new ToolStripSeparator());
            _trayContextMenu.Items.Add(itemExit);

            string title = string.IsNullOrEmpty(_appConfig?.EapId) ? "EAP Dashboard" : $"EAP Dashboard - {_appConfig.EapId}";
            if (title.Length > 63)
            {
                title = title.Substring(0, 63);
            }

            _notifyIcon = new NotifyIcon
            {
                Icon = appIcon,
                Text = title,
                ContextMenuStrip = _trayContextMenu,
                Visible = false
            };

            _notifyIcon.DoubleClick += (s, e) => RestoreFromTray();
        }

        private void RestoreFromTray()
        {
            if (_isHandlingResize) return;
            _isHandlingResize = true;
            try
            {
                this.Show();
                this.WindowState = (_previousWindowState == FormWindowState.Minimized) ? FormWindowState.Maximized : _previousWindowState;
                this.Activate();
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                }
            }
            finally
            {
                _isHandlingResize = false;
            }
            AdjustCardSize();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_isHandlingResize) return;
            _isHandlingResize = true;
            try
            {
                if (_appConfig != null && _appConfig.MinimizeToTray)
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.Hide();
                        if (_notifyIcon != null)
                        {
                            _notifyIcon.Visible = true;
                            _notifyIcon.ShowBalloonTip(1500, "EAP Dashboard", "程式已最小化至系統工作列通知區。", ToolTipIcon.Info);
                        }
                        return;
                    }
                    else
                    {
                        _previousWindowState = this.WindowState;
                    }
                }
            }
            finally
            {
                _isHandlingResize = false;
            }

            AdjustCardSize();
        }
        private void MenuSettings_Click(object sender, EventArgs e) { using (var f = new SettingsForm()) f.ShowDialog(this); }
        private async void MenuTimeSync_Click(object sender, EventArgs e) { foreach (var eq in _equipments) if (eq.Connection == ConnectionStatus.Connected) await eq.SendTimeCalibration(); }
        private void MenuTerminalDisplay_Click(object sender, EventArgs e) { using (var f = new TerminalDisplayForm(_equipments)) f.ShowDialog(this); }

        private void AdjustCardSize()
        {
            if (flowLayoutPanel.Width < 100) return;
            int cardsPerRow = flowLayoutPanel.Width < 600 ? 1 : (flowLayoutPanel.Width < 900 ? 2 : (flowLayoutPanel.Width < 1200 ? 3 : 4));
            int cardWidth = (flowLayoutPanel.Width / cardsPerRow) - 20;
            foreach (Control c in flowLayoutPanel.Controls) if (c is EquipmentCard card) card.Size = new Size(cardWidth, 180);
        }
    }
}
