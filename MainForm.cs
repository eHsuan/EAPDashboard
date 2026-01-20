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
using CyntecMqttLib.Interfaces;

namespace EAPDashboard
{
    // Class to represent the overall configuration structure
    public class AppConfig
    {
        public bool EnableSimulation { get; set; }
        public List<EquipmentConfig> Equipments { get; set; }
    }

    // Class to represent individual equipment configuration
    public class EquipmentConfig
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        // For SECS
        public string Ip { get; set; }
        public int Port { get; set; }
        // For MQTT
        public string Topic { get; set; }
    }

    public partial class MainForm : Form
    {
        private List<EquipmentBase> _equipments;
        private AppConfig _appConfig;
        private CancellationTokenSource _mainCts;
     
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Start maximized
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
            // Cancel all ongoing simulation tasks
            _mainCts?.Cancel();
            _mainCts?.Dispose();
            
            // Explicitly call StopSimulation on each equipment
            if (_equipments != null)
            {
                foreach (var equipment in _equipments)
                {
                    equipment.StopSimulation();
                }
            }
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
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"設定檔格式錯誤 (config.json)：\n{ex.Message}", "解析錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _appConfig = new AppConfig();
                return;
            }

            if (_appConfig?.Equipments == null)
            {
                MessageBox.Show("No equipment configurations found in config.json!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var config in _appConfig.Equipments)
            {
                EquipmentBase eq = null;
                if (config.Protocol.Equals("MQTT", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(config.Topic))
                    {
                        MessageBox.Show($"設備 {config.Id} ({config.Name}) 設定錯誤：MQTT 協定必須設定 Topic。\n該設備將不會連線。", 
                                        "設定錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    eq = new MqttEquipment 
                    {
                        Id = config.Id, 
                        Name = config.Name,
                        Topic = config.Topic
                    };
                }
                else if (config.Protocol.Equals("SECS", StringComparison.OrdinalIgnoreCase))
                {
                    IPAddress ipAddress;
                    bool isValidIp = IPAddress.TryParse(config.Ip, out ipAddress);

                    if (!isValidIp || config.Port <= 0)
                    {
                        MessageBox.Show($"設備 {config.Id} ({config.Name}) 設定錯誤：SECS 協定必須設定有效的 IP (例如 192.168.1.1) 與 Port。\n目前設定 IP: '{config.Ip}', Port: {config.Port}", 
                                        "設定錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    eq = new SecsEquipment 
                    {
                        Id = config.Id, 
                        Name = config.Name,
                        Ip = config.Ip,
                        Port = config.Port
                    };
                }

                if (eq != null)
                {
                    _equipments.Add(eq);
                }
            }
        }
        public MqttBus _bus;
        private async void PopulateDashboard()
        {
            _bus = new MqttBus(
                      host: "10.197.160.154",
                      port: 1883,
                      clientId: "測試1"
                   );
            await _bus.ConnectAsync();
            foreach (var equipment in _equipments)
            {
                var card = new EquipmentCard(equipment);
                flowLayoutPanel.Controls.Add(card);

                if (_appConfig.EnableSimulation)
                {
                    equipment.StartSimulation(_mainCts.Token , _bus);
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            AdjustCardSize();
        }

        private void AdjustCardSize()
        {
            const int columns = 3;
            const int rowsToFit = 5; // Aim to fit 5 rows (15 items) on screen
            var panelWidth = flowLayoutPanel.ClientSize.Width;
            var panelHeight = flowLayoutPanel.ClientSize.Height;

            // Get the margin from the first card, if it exists
            int horizontalMargin = 0;
            int verticalMargin = 0;
            if (flowLayoutPanel.Controls.Count > 0)
            {
                var firstCard = flowLayoutPanel.Controls[0];
                horizontalMargin = firstCard.Margin.Horizontal;
                verticalMargin = firstCard.Margin.Vertical;
            }

            // Calculate width to fit 3 columns
            // Subtract a bit more for the potential scrollbar width to prevent horizontal scroll
            int cardWidth = (panelWidth / columns) - horizontalMargin - 10; 
            if (cardWidth < 200) cardWidth = 200;

            // Calculate height to fit exactly 5 rows on the current screen height
            int cardHeight = (panelHeight / rowsToFit) - verticalMargin;

            // Safety check: ensure cards don't become too small to read
            if (cardHeight < 160) cardHeight = 160;

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
