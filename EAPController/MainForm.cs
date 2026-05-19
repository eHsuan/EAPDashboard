using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CyntecMqttLib.Interfaces;
using EAPController.Controls;
using EAPController.Models;
using EAPController.Forms;
using Newtonsoft.Json;

namespace EAPController
{
    public partial class MainForm : Form
    {
        private MqttBus _bus;
        private ControllerConfig _config;
        private readonly Dictionary<string, EapStatusModel> _eapStates = new Dictionary<string, EapStatusModel>();
        private System.Windows.Forms.Timer _uiTimer;
        private readonly Dictionary<string, EapControlForm> _openForms = new Dictionary<string, EapControlForm>();

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfig();
            PrePopulateUI();

            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 5000;
            _uiTimer.Tick += (s, ev) => UpdateUI();
            _uiTimer.Start();

            await InitMqtt();
        }

        private void LoadConfig()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    _config = JsonConvert.DeserializeObject<ControllerConfig>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to load config: {0}", ex.Message));
            }

            if (_config == null)
            {
                _config = new ControllerConfig
                {
                    MqttBrokerHost = "127.0.0.1",
                    MqttBrokerPort = 1883,
                    ControllerTopic = "CYNTEC/EAP/CONTROLLER",
                    MonitoredEaps = new List<string>()
                };
            }
        }

        private void PrePopulateUI()
        {
            flowLayoutPanel1.Controls.Clear();
            if (_config.MonitoredEaps == null) return;

            foreach (string eapId in _config.MonitoredEaps)
            {
                EapMonitorCard card = new EapMonitorCard(eapId);
                card.CardClicked += (c) => OpenControlPanel(c);
                // 初始狀態為 Disconnected (灰底或紅底)
                EapStatusModel initialModel = new EapStatusModel
                {
                    EapId = eapId,
                    Status = "Initializing",
                    LastUpdated = DateTime.MinValue // 確保觸發紅燈
                };
                card.UpdateStatus(initialModel);
                flowLayoutPanel1.Controls.Add(card);

                lock (_eapStates)
                {
                    _eapStates[eapId] = initialModel;
                }
            }

            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel1.Refresh();
        }

        private void OpenControlPanel(EapMonitorCard card)
        {
            if (_openForms.TryGetValue(card.EapId, out var existingForm))
            {
                existingForm.Activate();
                return;
            }

            var form = new EapControlForm(card.EapId, card.LastRawJson, this);
            form.FormClosed += (s, e) => _openForms.Remove(card.EapId);
            _openForms[card.EapId] = form;
            form.Show();
        }

        public async Task SendRestartCommand(string eapId)
        {
            if (_bus == null || !_bus.IsConnected) return;
            string topic = string.Format("{0}/{1}/Command", _config.ControllerTopic, eapId);
            var cmd = new { CommandType = "SafeRestart", Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            await _bus.PublishAsync(topic, cmd);
        }

        private async Task InitMqtt()
        {
            try
            {
                _bus = new MqttBus(
                    _config.MqttBrokerHost, 
                    _config.MqttBrokerPort, 
                    string.Format("Controller_{0}", Environment.MachineName), 
                    null, 
                    null,
                    true
                );

                _bus.Connected += () => UpdateBrokerStatus(true);
                _bus.Disconnected += () => UpdateBrokerStatus(false);
                
                await _bus.ConnectAsync();
                
                // 根據 MonitoredEaps 列表進行精確訂閱
                if (_config.MonitoredEaps != null)
                {
                    foreach (string eapId in _config.MonitoredEaps)
                    {
                        string subTopic = string.Format("{0}/{1}/Report", _config.ControllerTopic, eapId);
                        System.Diagnostics.Debug.WriteLine(string.Format("[MQTT] Subscribing to {0}", subTopic));
                        await _bus.SubscribeAsync(subTopic, (topic, payload) => {
                            ProcessReport(topic, payload);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("MQTT Init Error: {0}", ex.Message));
                UpdateBrokerStatus(false);
            }
        }

        private void UpdateBrokerStatus(bool isConnected)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(delegate { UpdateBrokerStatus(isConnected); }));
                return;
            }
            lblBrokerStatus.ForeColor = isConnected ? Color.LimeGreen : Color.Crimson;
            lblBrokerStatus.Text = isConnected ? "● MQTT Broker: Connected" : "● MQTT Broker: Disconnected";
        }

        private void ProcessReport(string topic, string json)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[MQTT] Received from {0}: {1}", topic, json));
                
                // 1. 解析外層封裝 (CyntecMqttLib 的 MessageEnvelope 格式)
                var envelope = JsonConvert.DeserializeAnonymousType(json, new { Payload = "" });
                if (envelope == null || string.IsNullOrEmpty(envelope.Payload)) return;

                // 2. 解析內層實際的 EAP 狀態資料
                EapStatusModel model = JsonConvert.DeserializeObject<EapStatusModel>(envelope.Payload);
                if (model == null || string.IsNullOrEmpty(model.EapId)) return;

                lock (_eapStates)
                {
                    model.LastUpdated = DateTime.Now;
                    _eapStates[model.EapId] = model;
                }

                // 更新 UI 相關
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        var card = flowLayoutPanel1.Controls.OfType<EapMonitorCard>()
                            .FirstOrDefault(c => string.Equals(c.EapId, model.EapId, StringComparison.OrdinalIgnoreCase));
                        if (card != null) card.LastRawJson = envelope.Payload;
                        
                        if (_openForms.TryGetValue(model.EapId, out var form))
                        {
                            form.UpdateJson(envelope.Payload);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[MQTT] Parse Error: {0}", ex.Message));
            }
        }

        private void UpdateUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUI));
                return;
            }

            lock (_eapStates)
            {
                foreach (var kvp in _eapStates)
                {
                    EapMonitorCard card = flowLayoutPanel1.Controls.OfType<EapMonitorCard>()
                        .FirstOrDefault(c => string.Equals(c.EapId, kvp.Key, StringComparison.OrdinalIgnoreCase));
                    
                    if (card != null)
                    {
                        card.UpdateStatus(kvp.Value);
                    }
                }
            }
        }
    }
}
