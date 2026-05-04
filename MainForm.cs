using EAPDashboard.Controls;
using EAPDashboard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;
using System.Net.Sockets;
using Modbus.Device;

namespace EAPDashboard
{
    public class AppConfig
    {
        public string MqttBrokerHost { get; set; }
        public int MqttBrokerPort { get; set; }
        public int HeartbeatIntervalSeconds { get; set; }
        public MesSettings MesSettings { get; set; }
        public DbSettings DbSettings { get; set; }
        public List<EquipmentConfig> Equipments { get; set; }
    }

    public class MesSettings
    {
        public string ProductionUrl { get; set; }
        public string TestUrl { get; set; }
        public string ActiveEnvironment { get; set; }
    }

    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string BindingTable { get; set; }
        public string CassetteColumn { get; set; }
        public string WoColumn { get; set; }
    }

    public class EquipmentConfig
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public ushort DeviceId { get; set; }
        public bool IsActive { get; set; }
        public string DictionaryPath { get; set; }
        public string Topic { get; set; }
    }

    public partial class MainForm : Form
    {
        private List<EquipmentBase> _equipments;
        private AppConfig _appConfig;
        private CancellationTokenSource _mainCts;
        private MqttBus _bus;

        // Modbus 連線池管理器
        private static readonly Dictionary<string, IModbusMaster> _modbusPool = new Dictionary<string, IModbusMaster>();
        private static readonly Dictionary<string, TcpClient> _tcpPool = new Dictionary<string, TcpClient>();
        private static readonly SemaphoreSlim _poolLock = new SemaphoreSlim(1, 1);

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; 
            this.Resize += MainForm_Resize;
            this.Load += MainForm_Load;
            this.FormClosed += MainForm_FormClosed;
            _mainCts = new CancellationTokenSource();
        }

        public static async Task<IModbusMaster> GetModbusMasterAsync(string ip, int port)
        {
            string key = $"{ip}:{port}";
            await _poolLock.WaitAsync();
            try {
                if (_tcpPool.TryGetValue(key, out var client) && client.Connected) return _modbusPool[key];
                
                var newClient = new TcpClient();
                await newClient.ConnectAsync(ip, port);
                var master = ModbusIpMaster.CreateIp(newClient);
                
                _tcpPool[key] = newClient;
                _modbusPool[key] = master;
                return master;
            } finally { _poolLock.Release(); }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfig();  
            PopulateDashboard();
            AdjustCardSize();

            // 立即啟動設備通訊 (不等待 MQTT 連線)
            StartEquipments();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mainCts?.Cancel();
            _mainCts?.Dispose();
            foreach (var eq in _equipments) eq.Stop();
            foreach (var tcp in _tcpPool.Values) tcp.Close();
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
                if (_appConfig.MesSettings != null) {
                    string env = _appConfig.MesSettings.ActiveEnvironment ?? "Production";
                    Protocol.MES.Factory.MESMessageFactory.TargetUrl = env.Equals("Production", StringComparison.OrdinalIgnoreCase) ? _appConfig.MesSettings.ProductionUrl : _appConfig.MesSettings.TestUrl;
                }
                bool isSim = !(_appConfig.MesSettings?.ActiveEnvironment ?? "Production").Equals("Production", StringComparison.OrdinalIgnoreCase);

                if (_appConfig.Equipments != null) {
                    foreach (var config in _appConfig.Equipments) {
                        EquipmentBase eq = null;
                        if (config.Protocol == "MQTT") {
                            eq = new MqttEquipment { 
                                Id = config.Id, 
                                Name = config.Name, 
                                Topic = config.Topic, 
                                DbConfig = _appConfig.DbSettings 
                            };
                        }
                        else if (config.Protocol == "SECS") eq = new SecsEquipment { Id = config.Id, Name = config.Name, Ip = config.Ip, Port = config.Port, DeviceId = config.DeviceId, IsActive = config.IsActive, DictionaryPath = Path.Combine(Application.StartupPath, config.DictionaryPath), DbConfig = _appConfig.DbSettings };
                        
                        if (eq != null) {
                            eq.IsSimulator = isSim;
                            _equipments.Add(eq);
                        }
                    }
                }
                LoadSQLiteEquipments(isSim);
                foreach (var eq in _equipments) eq.LoadAlarmMappings();
            } catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void LoadSQLiteEquipments(bool isSim)
        {
            try {
                using (var conn = DatabaseHelper.GetConnection()) {
                    conn.Open();
                    using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT * FROM Equipment WHERE IsActive = 1", conn)) {
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                string eqId = reader["EqpId"].ToString();
                                string name = reader["Name"].ToString();
                                string protocol = reader["Protocol"].ToString().ToUpper();
                                List<string> missing = new List<string>();
                                EquipmentBase eq = null;
                                if (protocol == "MQTT") {
                                    string topic = reader["Topic"].ToString();
                                    if (string.IsNullOrEmpty(topic)) missing.Add("Topic");
                                    else eq = new MqttEquipment { Id = eqId, Name = name, Topic = topic, DbConfig = _appConfig.DbSettings };
                                } else if (protocol == "SECS") {
                                    if (string.IsNullOrEmpty(reader["Ip"].ToString())) missing.Add("Ip");
                                    else eq = new SecsEquipment { Id = eqId, Name = name, Ip = reader["Ip"].ToString(), Port = Convert.ToInt32(reader["Port"]), DeviceId = (ushort)Convert.ToInt32(reader["DeviceId"]), DictionaryPath = Path.Combine(Application.StartupPath, reader["DictionaryPath"].ToString()), DbConfig = _appConfig.DbSettings };
                                } else if (protocol == "MODBUSTCP") {
                                    if (string.IsNullOrEmpty(reader["Ip"].ToString())) missing.Add("Ip");
                                    else {
                                        var modbusDriver = new ModbusDriver();
                                        var modEq = new PlcEquipmentBase(modbusDriver) { 
                                            Id = eqId, Name = name, Protocol = protocol, 
                                            Ip = reader["Ip"].ToString(), Port = Convert.ToInt32(reader["Port"]),
                                            DbConfig = _appConfig.DbSettings 
                                        };
                                        modEq.SetEvents(DatabaseHelper.GetPlcConfigs(eqId));
                                        eq = modEq;
                                    }
                                } else if (protocol == "MC") {
                                    if (string.IsNullOrEmpty(reader["Ip"].ToString())) missing.Add("Ip");
                                    else {
                                        var mcDriver = new McDriver();
                                        var mcEq = new PlcEquipmentBase(mcDriver) { 
                                            Id = eqId, Name = name, Protocol = protocol, 
                                            Ip = reader["Ip"].ToString(), Port = Convert.ToInt32(reader["Port"]),
                                            DbConfig = _appConfig.DbSettings 
                                        };
                                        mcEq.SetEvents(DatabaseHelper.GetPlcConfigs(eqId));
                                        eq = mcEq;
                                    }
                                }
                                if (eq != null) {
                                    eq.IsSimulator = isSim;
                                    _equipments.Add(eq);
                                }
                                if (missing.Count > 0) System.Diagnostics.Debug.WriteLine($"[DB] {eqId} missing: {string.Join(",", missing)}");
                            }
                        }
                    }
                }
            } catch { }
        }

        private void PopulateDashboard()
        {
            flowLayoutPanel.Controls.Clear();
            foreach (var eq in _equipments) {
                eq.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(EquipmentBase.State)) SaveStates(); };
                flowLayoutPanel.Controls.Add(new EquipmentCard(eq));
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
                    clientId: $"EAP_{Dns.GetHostName()}",
                    cleanSession: false
                );
                _bus.Connected += () => UpdateBrokerStatus(true);
                _bus.Disconnected += () => UpdateBrokerStatus(false);
                await _bus.ConnectAsync();
            } catch { UpdateBrokerStatus(false); }
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

        private void MainForm_Resize(object sender, EventArgs e) => AdjustCardSize();
        private void MenuSettings_Click(object sender, EventArgs e) { using (var f = new SettingsForm()) f.ShowDialog(this); }
        private async void MenuTimeSync_Click(object sender, EventArgs e) { foreach (var eq in _equipments) if (eq.Connection == ConnectionStatus.Connected) await eq.SendTimeCalibration(); }

        private void AdjustCardSize()
        {
            if (flowLayoutPanel.Width < 100) return;
            int cardsPerRow = flowLayoutPanel.Width < 600 ? 1 : (flowLayoutPanel.Width < 900 ? 2 : (flowLayoutPanel.Width < 1200 ? 3 : 4));
            int cardWidth = (flowLayoutPanel.Width / cardsPerRow) - 20;
            foreach (Control c in flowLayoutPanel.Controls) if (c is EquipmentCard card) card.Size = new Size(cardWidth, 180);
        }
    }
}
