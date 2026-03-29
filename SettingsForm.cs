using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EAPDashboard
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            GenerateReport();
        }

        private void GenerateReport()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "config.json");
                if (!File.Exists(path))
                {
                    txtConfig.Text = "錯誤：找不到 config.json 檔案。";
                    return;
                }

                string json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<AppConfig>(json);

                StringBuilder sb = new StringBuilder();
                string line = new string('-', 60);

                // 1. 通訊基礎設定
                sb.AppendLine("【 通訊基礎設定 (MQTT) 】");
                sb.AppendLine(line);
                sb.AppendLine($"  ● 伺服器位址 : {config.MqttBrokerHost ?? "127.0.0.1"}");
                sb.AppendLine($"  ● 伺服器埠號 : {config.MqttBrokerPort}");
                sb.AppendLine($"  ● 心跳間隔   : {config.HeartbeatIntervalSeconds} 秒");
                sb.AppendLine();
                sb.AppendLine();

                // 2. MES 整合設定
                sb.AppendLine("【 MES 整合設定 (WebService) 】");
                sb.AppendLine(line);
                if (config.MesSettings != null)
                {
                    sb.AppendLine($"  ● 目前啟動環境 : {config.MesSettings.ActiveEnvironment ?? "Production"}");
                    sb.AppendLine($"  ● 正式環境網址 : {config.MesSettings.ProductionUrl ?? "N/A"}");
                    sb.AppendLine($"  ● 測試環境網址 : {config.MesSettings.TestUrl ?? "N/A"}");
                }
                else
                {
                    sb.AppendLine("  ( 尚未設定 MES 環境參數 )");
                }
                sb.AppendLine();
                sb.AppendLine();

                // 3. 監控設備清單
                int eqCount = config.Equipments?.Count ?? 0;
                sb.AppendLine($"【 監控設備清單 】 (共 {eqCount} 台)");
                sb.AppendLine(line);
                if (config.Equipments != null)
                {
                    foreach (var eq in config.Equipments)
                    {
                        sb.AppendLine($"  ○ 設備編號: {eq.Id}");
                        sb.AppendLine($"    - 顯示名稱: {eq.Name ?? "N/A"}");
                        sb.AppendLine($"    - 協定類型: {eq.Protocol ?? "MQTT"}");
                        if (!string.IsNullOrEmpty(eq.Topic))
                            sb.AppendLine($"    - 通訊主題: {eq.Topic}");
                        if (!string.IsNullOrEmpty(eq.Ip))
                            sb.AppendLine($"    - IP 位址 : {eq.Ip}:{eq.Port}");
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("  ( 尚未設定任何設備 )");
                }

                txtConfig.Text = sb.ToString();
                
                // 捲動到最上方
                txtConfig.SelectionStart = 0;
                txtConfig.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                txtConfig.Text = $"產生配置報表時發生錯誤：\n{ex.Message}";
            }
        }
    }
}
