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
        public string Topic { get; set; }
    }

    public partial class MainForm : Form
    {
        private List<EquipmentBase> _equipments;
        private AppConfig _appConfig;
        private CancellationTokenSource _mainCts;
        private MqttBus _bus;
     
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; 
            this.Resize += MainForm_Resize;
            this.Load += MainForm_Load;
            this.FormClosed += MainForm_FormClosed;
            _mainCts = new CancellationTokenSource();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfig();  
            PopulateDashboard();
            AdjustCardSize();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mainCts?.Cancel();
            _mainCts?.Dispose();
            
            if (_equipments != null)
            {
                foreach (var equipment in _equipments)
                {
                    equipment.Stop();
                }
            }
            if (_bus != null) _bus.Dispose();
        }

        private void LoadConfig()
        {
            _equipments = new List<EquipmentBase>();
            var path = Path.Combine(Application.StartupPath, "config.json");
            if (!File.Exists(path))
            {
                MessageBox.Show("config.json not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var json = File.ReadAllText(path);
                _appConfig = JsonConvert.DeserializeObject<AppConfig>(json);
                
                if (_appConfig.MesSettings != null)
                {
                    string env = _appConfig.MesSettings.ActiveEnvironment ?? "Production";
                    string url = env.Equals("Production", StringComparison.OrdinalIgnoreCase) 
                                 ? _appConfig.MesSettings.ProductionUrl 
                                 : _appConfig.MesSettings.TestUrl;
                    
                    Protocol.MES.Factory.MESMessageFactory.TargetUrl = url;
                }

                if (_appConfig.Equipments != null)
                {
                    foreach (var config in _appConfig.Equipments)
                    {
                        if (config.Protocol == "MQTT")
                        {
                            var mqttEq = new MqttEquipment
                            {
                                Id = config.Id,
                                Name = config.Name,
                                Topic = config.Topic
                            };
                            // 傳遞資料庫設定
                            mqttEq.DbConfig = _appConfig.DbSettings;
                            _equipments.Add(mqttEq);
                        }
                    }
                }

                // 載入持久化狀態
                LoadStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"設定檔讀取失敗：\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _appConfig = new AppConfig();
            }
        }

        private void PopulateDashboard()
        {
            flowLayoutPanel.Controls.Clear();
            foreach (var equipment in _equipments)
            {
                // 訂閱狀態變更以即時存檔
                equipment.PropertyChanged += (s, e) => {
                    if (e.PropertyName == nameof(EquipmentBase.State) || e.PropertyName == nameof(EquipmentBase.CurrentRecipe))
                    {
                        SaveStates();
                    }
                };

                var card = new EquipmentCard(equipment);
                flowLayoutPanel.Controls.Add(card);
            }

            InitMqtt();
        }

        private async void InitMqtt()
        {
            try
            {
                _bus = new MqttBus(
                          host: _appConfig.MqttBrokerHost ?? "127.0.0.1",
                          port: _appConfig.MqttBrokerPort > 0 ? _appConfig.MqttBrokerPort : 1883,
                          clientId: $"EAPDashboard_{Dns.GetHostName()}"
                       );

                _bus.Connected += () => UpdateBrokerStatus(true);
                _bus.Disconnected += () => UpdateBrokerStatus(false);

                await _bus.ConnectAsync();
            }
            catch (Exception ex)
            {
                UpdateBrokerStatus(false);
                System.Diagnostics.Debug.WriteLine($"MQTT 連線失敗: {ex.Message}");
            }
        }

        private void StartEquipments()
        {
            foreach (var equipment in _equipments)
            {
                int interval = _appConfig.HeartbeatIntervalSeconds > 0 ? _appConfig.HeartbeatIntervalSeconds : 30;
                equipment.Start(_mainCts.Token, _bus, interval);
            }
        }

        private void UpdateBrokerStatus(bool isConnected)
        {
            if (lblBrokerStatus.InvokeRequired)
            {
                lblBrokerStatus.Invoke(new Action(() => UpdateBrokerStatus(isConnected)));
                return;
            }

            if (isConnected)
            {
                lblBrokerStatus.ForeColor = Color.Lime;
                lblBrokerStatus.Text = "● MQTT Broker: Connected";
                StartEquipments();
            }
            else
            {
                lblBrokerStatus.ForeColor = Color.Red;
                lblBrokerStatus.Text = "● MQTT Broker: Disconnected (Retrying...)";
            }
        }

        private void SaveStates()
        {
            try
            {
                var states = new Dictionary<string, object>();
                foreach (var eq in _equipments)
                {
                    states[eq.Id] = new { eq.State, eq.CurrentRecipe };
                }
                string json = JsonConvert.SerializeObject(states, Formatting.Indented);
                File.WriteAllText(Path.Combine(Application.StartupPath, "equipment_state.json"), json);
            }
            catch { }
        }

        private void LoadStates()
        {
            string path = Path.Combine(Application.StartupPath, "equipment_state.json");
            if (!File.Exists(path)) return;
            try
            {
                string json = File.ReadAllText(path);
                var states = JsonConvert.DeserializeObject<Dictionary<string, Newtonsoft.Json.Linq.JObject>>(json);
                foreach (var eq in _equipments)
                {
                    if (states.TryGetValue(eq.Id, out var stateObj))
                    {
                        if (stateObj["State"] != null) eq.State = stateObj["State"].ToObject<EquipmentState>();
                        if (stateObj["CurrentRecipe"] != null) eq.CurrentRecipe = stateObj["CurrentRecipe"].ToString();
                    }
                }
            }
            catch { }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            AdjustCardSize();
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm())
            {
                form.ShowDialog(this);
            }
        }

        private void MenuTimeSync_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (var eq in _equipments)
            {
                if (eq.Connection == ConnectionStatus.Connected && eq is MqttEquipment mqttEq)
                {
                    mqttEq.SendTimeCalibration();
                    count++;
                }
            }
            MessageBox.Show($"已對 {count} 台連線中的設備發出時間校正指令。", "指令已發送", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AdjustCardSize()
        {
            if (flowLayoutPanel.Width < 100) return;

            int margin = 10;
            int cardsPerRow = 4;
            
            if (flowLayoutPanel.Width < 600) cardsPerRow = 1;
            else if (flowLayoutPanel.Width < 900) cardsPerRow = 2;
            else if (flowLayoutPanel.Width < 1200) cardsPerRow = 3;

            int cardWidth = (flowLayoutPanel.Width / cardsPerRow) - (margin * 2);
            int cardHeight = 180;

            foreach (Control control in flowLayoutPanel.Controls)
            {
                if (control is EquipmentCard card)
                {
                    card.Size = new Size(cardWidth, cardHeight);
                }
            }
        }
    }
}
