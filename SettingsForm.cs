using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EAPDashboard.Models;
using EAPDashboard.Services;
using Newtonsoft.Json;

namespace EAPDashboard
{
    public partial class SettingsForm : Form
    {
        private readonly MainForm _mainForm;
        private AppConfig _originalConfig;

        public SettingsForm() : this(null, null)
        {
        }

        public SettingsForm(MainForm mainForm, AppConfig currentConfig)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _originalConfig = currentConfig != null ? CloneConfig(currentConfig) : LoadConfigFromFile();

            // 若外部未傳入，但原設定存在
            if (_originalConfig == null)
            {
                _originalConfig = new AppConfig();
            }

            SetupDataGridViewStyle();
            LoadConfigToUI(_originalConfig);
        }

        private void SetupDataGridViewStyle()
        {
            dgvEquipments.EnableHeadersVisualStyles = false;
            dgvEquipments.GridColor = Color.FromArgb(55, 55, 62);
            dgvEquipments.BackgroundColor = Color.FromArgb(28, 28, 32);

            // 標題列樣式 (深灰底白字)
            var headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(45, 48, 58),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 9.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                SelectionBackColor = Color.FromArgb(45, 48, 58),
                SelectionForeColor = Color.White
            };
            dgvEquipments.ColumnHeadersDefaultCellStyle = headerStyle;

            // 儲存格預設樣式 (深底白字，解決白底白字看不到內容之問題)
            var defaultStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(36, 36, 42),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(50, 95, 150),
                SelectionForeColor = Color.White
            };
            dgvEquipments.DefaultCellStyle = defaultStyle;

            // 交替行底色
            var altStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 30, 35),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(50, 95, 150),
                SelectionForeColor = Color.White
            };
            dgvEquipments.AlternatingRowsDefaultCellStyle = altStyle;

            // 進入編輯模式時，確保輸入框或下拉選單也是深底白字
            dgvEquipments.EditingControlShowing += DgvEquipments_EditingControlShowing;
        }

        private void DgvEquipments_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.BackColor = Color.FromArgb(45, 45, 52);
                tb.ForeColor = Color.White;
            }
            else if (e.Control is ComboBox cb)
            {
                cb.BackColor = Color.FromArgb(45, 45, 52);
                cb.ForeColor = Color.White;
            }
        }

        private AppConfig LoadConfigFromFile()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "config.json");
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<AppConfig>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"讀取 config.json 發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new AppConfig();
        }

        private static AppConfig CloneConfig(AppConfig source)
        {
            if (source == null) return new AppConfig();
            string json = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }

        private void LoadConfigToUI(AppConfig config)
        {
            // 1. 系統與通訊
            txtEapId.Text = config.EapId ?? "";
            txtExitPassword.Text = config.ExitPassword ?? "CIM";
            chkMinimizeToTray.Checked = config.MinimizeToTray;
            txtMqttHost.Text = config.MqttBrokerHost ?? "127.0.0.1";
            numMqttPort.Value = config.MqttBrokerPort > 0 ? config.MqttBrokerPort : 1883;
            txtControllerTopic.Text = config.ControllerTopic ?? "CYNTEC/EAP/CONTROLLER";
            numHeartbeat.Value = config.HeartbeatIntervalSeconds > 0 ? config.HeartbeatIntervalSeconds : 30;
            numEqpStatusInterval.Value = config.EqpStatusReportIntervalSeconds > 0 ? config.EqpStatusReportIntervalSeconds : 60;
            txtDictionaryDir.Text = config.DictionaryDirectory ?? "Dictionaries";
            txtMqttTopicPrefix.Text = config.MqttTopicPrefix ?? "TN2/YELLOW/machine";

            // 2. MES
            var mes = config.MesSettings ?? new MesSettings();
            cboMesEnv.SelectedItem = string.Equals(mes.ActiveEnvironment, "Production", StringComparison.OrdinalIgnoreCase)
                ? "Production"
                : "Test";
            txtMesProdUrl.Text = mes.ProductionUrl ?? "";
            txtMesTestUrl.Text = mes.TestUrl ?? "";
            chkCassetteBinding.Checked = config.EnableCassetteBinding;

            // 3. ReelCheck
            var reel = config.ReelCheckSettings ?? new ReelCheckSettings();
            cboReelEnv.SelectedItem = string.Equals(reel.ActiveEnvironment, "Production", StringComparison.OrdinalIgnoreCase)
                ? "Production"
                : "Test";
            txtReelProdUrl.Text = reel.ProductionUrl ?? "";
            txtReelTestUrl.Text = reel.TestUrl ?? "";
            txtReelClientUrl.Text = reel.ClientUrl ?? "";
            numReelTimeout.Value = reel.TimeoutSeconds > 0 ? reel.TimeoutSeconds : 30;

            // 4. PostgreSQL
            var pg = config.PostgresDbSettings ?? new PostgresDbSettings();
            chkPgEnabled.Checked = pg.Enabled;
            txtPgConnStr.Text = pg.ConnectionString ?? "";
            numPgRetentionDays.Value = pg.RetentionDays > 0 ? pg.RetentionDays : 14;

            // 5. Log
            var log = config.LogSettings ?? new LogSettings();
            chkLogEnabled.Checked = log.Enabled;
            numLogRetentionDays.Value = log.RetentionDays > 0 ? log.RetentionDays : 30;
            numLogCompressDays.Value = log.CompressDays > 0 ? log.CompressDays : 3;
            numLogMaxSize.Value = log.MaxFileSizeMB > 0 ? log.MaxFileSizeMB : 20;
            chkLogAreYouThere.Checked = config.LogAreYouThere ?? log.LogAreYouThere;

            // 6. 機台清單
            dgvEquipments.Rows.Clear();
            if (config.Equipments != null)
            {
                foreach (var eq in config.Equipments)
                {
                    int rowIdx = dgvEquipments.Rows.Add(
                        eq.IsActive,
                        eq.Id ?? "",
                        eq.Name ?? "",
                        string.Equals(eq.Protocol, "SECS", StringComparison.OrdinalIgnoreCase) ? "SECS" : "MQTT",
                        string.Equals(eq.StatusType, "Cyntec", StringComparison.OrdinalIgnoreCase) ? "Cyntec" : "Delta",
                        eq.Ip ?? "",
                        eq.Port.ToString(),
                        eq.DeviceId.ToString(),
                        eq.AutoReportStatus,
                        eq.EnableReelCheck,
                        eq.EnableTrackOutValidation
                    );
                }
            }
        }

        private AppConfig BuildNewConfigFromUI()
        {
            var config = CloneConfig(_originalConfig);

            // 1. 系統與通訊
            config.EapId = txtEapId.Text.Trim();
            config.ExitPassword = txtExitPassword.Text;
            config.MinimizeToTray = chkMinimizeToTray.Checked;
            config.MqttBrokerHost = txtMqttHost.Text.Trim();
            config.MqttBrokerPort = (int)numMqttPort.Value;
            config.ControllerTopic = txtControllerTopic.Text.Trim();
            config.HeartbeatIntervalSeconds = (int)numHeartbeat.Value;
            config.EqpStatusReportIntervalSeconds = (int)numEqpStatusInterval.Value;
            config.DictionaryDirectory = string.IsNullOrWhiteSpace(txtDictionaryDir.Text) ? "Dictionaries" : txtDictionaryDir.Text.Trim();
            config.MqttTopicPrefix = string.IsNullOrWhiteSpace(txtMqttTopicPrefix.Text) ? "TN2/YELLOW/machine" : txtMqttTopicPrefix.Text.Trim();

            // 2. MES
            if (config.MesSettings == null) config.MesSettings = new MesSettings();
            config.MesSettings.ActiveEnvironment = cboMesEnv.SelectedItem?.ToString() ?? "Test";
            config.MesSettings.ProductionUrl = txtMesProdUrl.Text.Trim();
            config.MesSettings.TestUrl = txtMesTestUrl.Text.Trim();
            config.EnableCassetteBinding = chkCassetteBinding.Checked;

            // 3. ReelCheck
            if (config.ReelCheckSettings == null) config.ReelCheckSettings = new ReelCheckSettings();
            config.ReelCheckSettings.ActiveEnvironment = cboReelEnv.SelectedItem?.ToString() ?? "Test";
            config.ReelCheckSettings.ProductionUrl = txtReelProdUrl.Text.Trim();
            config.ReelCheckSettings.TestUrl = txtReelTestUrl.Text.Trim();
            config.ReelCheckSettings.ClientUrl = txtReelClientUrl.Text.Trim();
            config.ReelCheckSettings.TimeoutSeconds = (int)numReelTimeout.Value;

            // 4. PostgreSQL
            if (config.PostgresDbSettings == null) config.PostgresDbSettings = new PostgresDbSettings();
            config.PostgresDbSettings.Enabled = chkPgEnabled.Checked;
            config.PostgresDbSettings.ConnectionString = txtPgConnStr.Text.Trim();
            config.PostgresDbSettings.RetentionDays = (int)numPgRetentionDays.Value;

            // 5. Log
            if (config.LogSettings == null) config.LogSettings = new LogSettings();
            config.LogSettings.Enabled = chkLogEnabled.Checked;
            config.LogSettings.RetentionDays = (int)numLogRetentionDays.Value;
            config.LogSettings.CompressDays = (int)numLogCompressDays.Value;
            config.LogSettings.MaxFileSizeMB = (int)numLogMaxSize.Value;
            config.LogSettings.LogAreYouThere = chkLogAreYouThere.Checked;
            config.LogAreYouThere = _originalConfig.LogAreYouThere.HasValue ? chkLogAreYouThere.Checked : (bool?)null;

            // 6. 機台清單
            var newEquipments = new List<EquipmentConfig>();
            foreach (DataGridViewRow row in dgvEquipments.Rows)
            {
                if (row.IsNewRow) continue;

                string id = row.Cells["colEqId"].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(id)) continue;

                bool isActive = row.Cells["colEqActive"].Value is bool b && b;
                string name = row.Cells["colEqName"].Value?.ToString()?.Trim() ?? id;
                string protocol = row.Cells["colEqProtocol"].Value?.ToString() ?? "MQTT";
                string statusType = row.Cells["colEqStatusType"].Value?.ToString() ?? "Delta";
                string ip = row.Cells["colEqIp"].Value?.ToString()?.Trim();
                int.TryParse(row.Cells["colEqPort"].Value?.ToString(), out int port);
                ushort.TryParse(row.Cells["colEqDeviceId"].Value?.ToString(), out ushort deviceId);
                bool autoReport = row.Cells["colEqAutoReport"].Value is bool ab && ab;
                bool enableReelCheck = row.Cells["colEqReelCheck"].Value is bool rcb && rcb;
                bool enableTrackOutVal = row.Cells["colEqTrackOutValidation"].Value is bool tov && tov;

                // 保留原設定之特定欄位 (若存在)
                var existingEq = _originalConfig.Equipments?.FirstOrDefault(e => string.Equals(e.Id, id, StringComparison.OrdinalIgnoreCase));
                var eqCfg = existingEq != null ? JsonConvert.DeserializeObject<EquipmentConfig>(JsonConvert.SerializeObject(existingEq)) : new EquipmentConfig();

                eqCfg.Id = id;
                eqCfg.Name = name;
                eqCfg.IsActive = isActive;
                eqCfg.Protocol = protocol;
                eqCfg.Topic = null;
                eqCfg.StatusType = statusType;
                eqCfg.Ip = ip;
                eqCfg.Port = port;
                eqCfg.DeviceId = deviceId;
                eqCfg.DictionaryPath = null;
                eqCfg.AutoReportStatus = autoReport;
                eqCfg.EnableReelCheck = enableReelCheck;
                eqCfg.EnableTrackOutValidation = enableTrackOutVal;

                newEquipments.Add(eqCfg);
            }
            config.Equipments = newEquipments;

            return config;
        }

        private void BtnAddEq_Click(object sender, EventArgs e)
        {
            int idx = dgvEquipments.Rows.Add(
                true,
                "NEW_EQ",
                "新建設備",
                "MQTT",
                "Cyntec",
                "",
                "5000",
                "0",
                false,
                false,
                false
            );
            dgvEquipments.ClearSelection();
            dgvEquipments.Rows[idx].Selected = true;
        }

        private void BtnDeleteEq_Click(object sender, EventArgs e)
        {
            if (dgvEquipments.SelectedRows.Count == 0)
            {
                MessageBox.Show("請先點選欲刪除之設備資料列。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("確定要自清單中刪除所選取的設備嗎？\n(若該機台目前運作中，套用後將會中斷連線並卸載)", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvEquipments.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dgvEquipments.Rows.Remove(row);
                    }
                }
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabJsonPreview)
            {
                try
                {
                    var previewConfig = BuildNewConfigFromUI();
                    txtJsonPreview.Text = JsonConvert.SerializeObject(previewConfig, Formatting.Indented);
                }
                catch (Exception ex)
                {
                    txtJsonPreview.Text = $"無法產生 JSON 預覽：{ex.Message}";
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 驗證基本輸入
            if (string.IsNullOrWhiteSpace(txtEapId.Text))
            {
                MessageBox.Show("EAP 站別編號 (EapId) 不能為空白！", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabSystem;
                txtEapId.Focus();
                return;
            }

            // 驗證機台重複 ID
            var eqIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataGridViewRow row in dgvEquipments.Rows)
            {
                if (row.IsNewRow) continue;
                string id = row.Cells["colEqId"].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(id))
                {
                    if (eqIds.Contains(id))
                    {
                        MessageBox.Show($"機台清單中存在重複的設備編號：'{id}'，請修正後再行儲存。", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tabControl.SelectedTab = tabEquipments;
                        return;
                    }
                    eqIds.Add(id);
                }
            }

            // 組裝新設定檔
            AppConfig newConfig = BuildNewConfigFromUI();

            // 執行深度比對
            ConfigDiffResult diff = ConfigDiffService.Compare(_originalConfig, newConfig);

            // 若沒有任何變動
            if (!diff.HasAnyChange)
            {
                MessageBox.Show("設定內容未有任何變動，無需套用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 彈出二次確認視窗
            using (var diffForm = new ConfigDiffForm(diff))
            {
                if (diffForm.ShowDialog(this) != DialogResult.OK)
                {
                    // 使用者取消或放棄返回
                    return;
                }
            }

            // 二次確認通過，進行儲存與熱套用
            bool success = false;
            if (_mainForm != null)
            {
                success = _mainForm.ApplyConfiguration(newConfig, diff);
            }
            else
            {
                success = SaveConfigToFileDirectly(newConfig);
                if (success)
                {
                    if (diff.HasGlobalChanges)
                    {
                        MessageBox.Show("全域參數已修改並儲存至 config.json！\n請手動重新啟動應用程式以完全生效。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("機台設定已成功寫入 config.json。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            if (success)
            {
                _originalConfig = CloneConfig(newConfig);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool SaveConfigToFileDirectly(AppConfig config)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "config.json");
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"寫入 config.json 失敗：{ex.Message}", "儲存錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
