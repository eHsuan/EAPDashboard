using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MESSimulator.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MESSimulator
{
    public partial class MainForm : Form
    {
        private HttpListener _listener;
        private bool _isRunning = false;

        private readonly ConcurrentDictionary<string, bool> _woAlreadyInMap = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, List<string>> _woRequiredCodesMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        private MqttBrokerService _brokerService;
        private EapSimulatorService _eapSimulatorService;
        private BindingList<EapLogicRuleItem> _eapRulesBindingList;

        public MainForm()
        {
            InitializeComponent();
            InitializeBrokerAndEapServices();
        }

        private void InitializeBrokerAndEapServices()
        {
            // 1. 初始化內建 MQTT Broker 服務
            _brokerService = new MqttBrokerService();
            _brokerService.LogMessage += msg => AppendBrokerLog(msg);
            _brokerService.ClientsChanged += () => RefreshClientsGrid();

            SetupBrokerClientsGrid();

            // 2. 初始化 EAP 模擬器服務 (直接串接內部 GenerateMockReply 並同步記錄至 MES Log)
            _eapSimulatorService = new EapSimulatorService(ExecuteInternalMesTransaction);
            _eapSimulatorService.LogMessage += msg => AppendEapLog(msg);

            if (cboRemoteCmdType.Items.Count > 0 && cboRemoteCmdType.SelectedIndex < 0)
            {
                cboRemoteCmdType.SelectedIndex = 0;
            }

            InitializeNetworkIpControls();
            SetupEapRulesGrid();
        }

        private void InitializeNetworkIpControls()
        {
            var localIps = MqttBrokerService.GetLocalIpAddresses();

            // 1. Tab 1: MES WebService 監聽 IP
            cboMesIp.Items.Clear();
            cboMesIp.Items.Add("* (全部網卡)");
            cboMesIp.Items.Add("localhost");
            cboMesIp.Items.Add("127.0.0.1");
            foreach (var ip in localIps)
            {
                cboMesIp.Items.Add(ip);
            }
            cboMesIp.SelectedIndex = 0;

            // 2. Tab 2: 內建 MQTT Broker 監聽 IP
            cboBrokerIp.Items.Clear();
            cboBrokerIp.Items.Add("0.0.0.0 (全部網卡)");
            cboBrokerIp.Items.Add("127.0.0.1");
            foreach (var ip in localIps)
            {
                cboBrokerIp.Items.Add(ip);
            }
            cboBrokerIp.SelectedIndex = 0;

            // 3. Tab 3: EAP 模擬器連線目標 Broker IP
            cboEapBrokerIp.Items.Clear();
            cboEapBrokerIp.Items.Add("127.0.0.1");
            foreach (var ip in localIps)
            {
                cboEapBrokerIp.Items.Add(ip);
            }
            cboEapBrokerIp.SelectedIndex = 0;
        }

        private string ExecuteInternalMesTransaction(string jsonReq)
        {
            string txnName = "Unknown";
            try { txnName = JObject.Parse(jsonReq)["TransactionName"]?.ToString() ?? "Unknown"; } catch { }

            Log($"[EAP->MES RECV] ({txnName}) {jsonReq}");
            string reply = GenerateMockReply(jsonReq);
            Log($"[EAP<-MES SEND] ({txnName}) {reply}");
            return reply;
        }

        #region DataGridView 初始化與資料綁定

        private void SetupBrokerClientsGrid()
        {
            dgvClients.AutoGenerateColumns = false;
            dgvClients.Columns.Clear();

            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ConnectedClientInfo.ClientId),
                HeaderText = "Client ID",
                Width = 220
            });
            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ConnectedClientInfo.Endpoint),
                HeaderText = "連線端點 (IP:Port)",
                Width = 170
            });
            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ConnectedClientInfo.ConnectedTime),
                HeaderText = "連線時間",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });
            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ConnectedClientInfo.LastActivityTime),
                HeaderText = "最後活動時間",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });
            dgvClients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ConnectedClientInfo.SubscribedTopicsDisplay),
                HeaderText = "已訂閱 Topics (Subscribed Topics)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void SetupEapRulesGrid()
        {
            dgvEapRules.AutoGenerateColumns = false;
            dgvEapRules.Columns.Clear();

            dgvEapRules.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.AutoReply),
                HeaderText = "自動回覆 (Auto Reply)",
                Width = 95,
                ReadOnly = false
            });
            dgvEapRules.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.ForceFail),
                HeaderText = "強制回覆失敗 (Force Fail)",
                Width = 105,
                ReadOnly = false
            });
            dgvEapRules.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.DisplayName),
                HeaderText = "邏輯名稱",
                Width = 250,
                ReadOnly = true
            });
            dgvEapRules.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.TriggerCommand),
                HeaderText = "設備上報指令 (Trigger)",
                Width = 190,
                ReadOnly = true
            });
            dgvEapRules.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.ReplyCommand),
                HeaderText = "EAP 回覆指令 (Reply)",
                Width = 190,
                ReadOnly = true
            });
            dgvEapRules.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EapLogicRuleItem.Description),
                HeaderText = "動作與檢查說明",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            _eapRulesBindingList = new BindingList<EapLogicRuleItem>(_eapSimulatorService.Rules);
            dgvEapRules.DataSource = _eapRulesBindingList;

            // 讓 CheckBox 點擊後立即觸發更新，不需移開焦點
            dgvEapRules.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvEapRules.IsCurrentCellDirty)
                {
                    dgvEapRules.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            dgvEapRules.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < _eapRulesBindingList.Count)
                {
                    var item = _eapRulesBindingList[e.RowIndex];
                    AppendEapLog($"[規則變更] 「{item.DisplayName}」 -> 自動回覆={(item.AutoReply ? "ON" : "OFF")}, 強制失敗={(item.ForceFail ? "ON" : "OFF")}");
                    dgvEapRules.InvalidateRow(e.RowIndex);
                }
            };

            dgvEapRules.CellFormatting += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < _eapRulesBindingList.Count)
                {
                    var item = _eapRulesBindingList[e.RowIndex];
                    if (!item.AutoReply)
                    {
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
                    }
                    else if (item.ForceFail)
                    {
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                    }
                    else
                    {
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        dgvEapRules.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            };
        }

        private void RefreshClientsGrid()
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                try { this.BeginInvoke(new Action(RefreshClientsGrid)); } catch { }
                return;
            }

            var snapshot = _brokerService.GetConnectedClientsSnapshot();
            lblClientCount.Text = $"目前連線數：{snapshot.Count}";
            dgvClients.DataSource = null;
            dgvClients.DataSource = snapshot;
        }

        #endregion

        #region Tab 2: MQTT Broker 事件處理

        private async void BtnBrokerStart_Click(object sender, EventArgs e)
        {
            await StartBrokerInternalAsync();
        }

        private async Task StartBrokerInternalAsync()
        {
            if (_brokerService.IsRunning) return;

            if (!int.TryParse(txtBrokerPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("請輸入有效的 MQTT Broker Port (1 ~ 65535)。", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string bindIp = cboBrokerIp?.Text?.Trim() ?? "0.0.0.0";
                await _brokerService.StartAsync(port, bindIp);
                UpdateBrokerUI(true);
            }
            catch (Exception ex)
            {
                AppendBrokerLog($"[錯誤] 啟動 MQTT Broker 失敗: {ex.Message}");
                UpdateBrokerUI(false);
            }
        }

        private async void BtnBrokerStop_Click(object sender, EventArgs e)
        {
            if (!_brokerService.IsRunning) return;

            try
            {
                // 若 EAP 模擬器連線在本地 Broker，先停止 EAP 模擬器
                if (_eapSimulatorService.IsRunning)
                {
                    await _eapSimulatorService.StopAsync();
                    UpdateEapUI(false);
                }

                await _brokerService.StopAsync();
                UpdateBrokerUI(false);
                RefreshClientsGrid();
            }
            catch (Exception ex)
            {
                AppendBrokerLog($"[錯誤] 停止 MQTT Broker 失敗: {ex.Message}");
            }
        }

        private void UpdateBrokerUI(bool running)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateBrokerUI(running))); return; }

            btnBrokerStart.Enabled = !running;
            btnBrokerStop.Enabled = running;
            if (cboBrokerIp != null) cboBrokerIp.Enabled = !running;
            txtBrokerPort.Enabled = !running;
            lblBrokerStatus.Text = running ? $"● Running ({_brokerService.BoundIp}:{_brokerService.Port})" : "● Broker Stopped";
            lblBrokerStatus.ForeColor = running ? Color.Lime : Color.Red;
        }

        private void AppendBrokerLog(string message)
        {
            if (this.IsDisposed || txtBrokerLog == null || txtBrokerLog.IsDisposed) return;
            if (txtBrokerLog.InvokeRequired)
            {
                try { txtBrokerLog.BeginInvoke(new Action(() => AppendBrokerLog(message))); } catch { }
                return;
            }
            txtBrokerLog.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}");
        }

        #endregion

        #region Tab 3: EAP 模擬器事件處理

        private async void BtnEapStart_Click(object sender, EventArgs e)
        {
            await StartEapInternalAsync();
        }

        private async Task StartEapInternalAsync()
        {
            if (_eapSimulatorService.IsRunning) return;

            string brokerIp = MqttBrokerService.ExtractCleanIp(cboEapBrokerIp?.Text);
            if (string.IsNullOrWhiteSpace(brokerIp) || brokerIp == "0.0.0.0" || brokerIp == "*")
            {
                brokerIp = "127.0.0.1";
            }

            string portText = txtEapBrokerPort != null ? txtEapBrokerPort.Text.Trim() : txtBrokerPort.Text.Trim();
            if (!int.TryParse(portText, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("請輸入有效的目標 Broker Port (1 ~ 65535)。", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await _eapSimulatorService.StartAsync(brokerIp, port);
                UpdateEapUI(_eapSimulatorService.IsRunning);
            }
            catch (Exception ex)
            {
                AppendEapLog($"[錯誤] 啟動 EAP 模擬器失敗 ({brokerIp}:{port}): {ex.Message}");
                UpdateEapUI(false);
            }
        }

        private async void BtnEapStop_Click(object sender, EventArgs e)
        {
            if (!_eapSimulatorService.IsRunning) return;

            try
            {
                await _eapSimulatorService.StopAsync();
                UpdateEapUI(false);
            }
            catch (Exception ex)
            {
                AppendEapLog($"[錯誤] 停止 EAP 模擬器失敗: {ex.Message}");
            }
        }

        private void UpdateEapUI(bool running)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateEapUI(running))); return; }

            btnEapStart.Enabled = !running;
            btnEapStop.Enabled = running;
            if (cboEapBrokerIp != null) cboEapBrokerIp.Enabled = !running;
            if (txtEapBrokerPort != null) txtEapBrokerPort.Enabled = !running;
            string ip = MqttBrokerService.ExtractCleanIp(cboEapBrokerIp?.Text);
            if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";
            string port = txtEapBrokerPort?.Text?.Trim() ?? "1883";
            lblEapStatus.Text = running ? $"● EAP Running ({ip}:{port})" : "● EAP Sim Stopped";
            lblEapStatus.ForeColor = running ? Color.Lime : Color.Red;
        }

        private void BtnSelectAllAutoReply_Click(object sender, EventArgs e)
        {
            foreach (var rule in _eapRulesBindingList)
            {
                rule.AutoReply = true;
            }
            dgvEapRules.Refresh();
            AppendEapLog("[設定] 已全部勾選「自動回覆 (Auto Reply)」。");
        }

        private void BtnUnselectAllAutoReply_Click(object sender, EventArgs e)
        {
            foreach (var rule in _eapRulesBindingList)
            {
                rule.AutoReply = false;
            }
            dgvEapRules.Refresh();
            AppendEapLog("[設定] 已全部取消「自動回覆 (Auto Reply)」。");
        }

        private void BtnResetForceFail_Click(object sender, EventArgs e)
        {
            foreach (var rule in _eapRulesBindingList)
            {
                rule.ForceFail = false;
            }
            dgvEapRules.Refresh();
            AppendEapLog("[設定] 已全部重置「強制失敗 (Force Fail)」為正常模式。");
        }

        private string GetTargetCommandTopicAndMachine(out string eqpId)
        {
            string topic = txtTargetCmdTopic.Text.Trim();
            if (string.IsNullOrEmpty(topic))
            {
                topic = $"TN2/YELLOW/machine/{EapSimulatorService.DefaultEquipmentId}/Command";
                txtTargetCmdTopic.Text = topic;
            }
            eqpId = EapSimulatorService.ExtractMachineIdFromCommandTopic(topic);
            return topic;
        }

        private async void BtnSendAreYouThere_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            await _eapSimulatorService.SendAreYouThereAsync(eqpId, topic);
        }

        private async void BtnSendStatusReq_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            await _eapSimulatorService.SendStatusReqAsync(eqpId, topic);
        }

        private async void BtnSendTimeCalibrate_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            await _eapSimulatorService.SendTimeCalibrateAsync(eqpId, topic);
        }

        private async void BtnSendInitTraceData_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            string timerSec = txtTraceInterval.Text.Trim();
            if (string.IsNullOrEmpty(timerSec)) timerSec = "5";

            string rawEnable = txtTraceParams.Text.Trim();
            bool enable = !(rawEnable.Equals("false", StringComparison.OrdinalIgnoreCase) || rawEnable == "0");

            await _eapSimulatorService.SendInitTraceDataAsync(eqpId, topic, timerSec, enable);
        }

        private async void BtnSendRemoteCmd_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            string rcmd = cboRemoteCmdType.Text.Trim();
            string recipe = txtRemoteRecipe.Text.Trim();
            if (string.IsNullOrEmpty(rcmd))
            {
                MessageBox.Show("請選擇 RemoteCMD 指令。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            await _eapSimulatorService.SendRemoteCmdAsync(eqpId, topic, rcmd, recipe);
        }

        private async void BtnSendTerminalMsg_Click(object sender, EventArgs e)
        {
            string topic = GetTargetCommandTopicAndMachine(out string eqpId);
            using (var form = new MESSimulator.Forms.TerminalDisplayForm(eqpId, topic))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    string text = form.TerminalMessage;
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        await _eapSimulatorService.SendTerminalDisplayAsync(eqpId, topic, text);
                    }
                }
            }
        }

        private void BtnExportEapLog_Click(object sender, EventArgs e)
        {
            ExportLogWithDialog(txtEapLog?.Text, "EAP_Simulator_Log", "儲存 EAP 模擬器日誌");
        }

        private void BtnExportMesLog_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(txtLog?.Text))
            {
                sb.AppendLine("==================== [MES Transaction Log] ====================");
                sb.AppendLine(txtLog.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtRequest?.Text))
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendLine("==================== [SOAP Request History] ====================");
                sb.AppendLine(txtRequest.Text);
            }

            ExportLogWithDialog(sb.ToString(), "MES_Simulator_Log", "儲存 MES 模擬器日誌");
        }

                private void BtnClearMesLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            txtRequest.Clear();
        }

        private void BtnClearBrokerLog_Click(object sender, EventArgs e)
        {
            txtBrokerLog.Clear();
        }

        private void BtnClearEapLog_Click(object sender, EventArgs e)
        {
            txtEapLog.Clear();
        }

        private void ExportLogWithDialog(string logContent, string defaultFileNamePrefix, string dialogTitle)
        {
            if (string.IsNullOrWhiteSpace(logContent))
            {
                MessageBox.Show("目前沒有可輸出的日誌內容。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = dialogTitle;
                sfd.Filter = "Log 檔案 (*.log)|*.log|文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
                sfd.DefaultExt = "log";
                sfd.FileName = $"{defaultFileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.log";

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, logContent, Encoding.UTF8);
                        MessageBox.Show($"日誌已成功儲存至：\n{sfd.FileName}", "輸出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"儲存日誌失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AppendEapLog(string message)
        {
            if (this.IsDisposed || txtEapLog == null || txtEapLog.IsDisposed) return;
            if (txtEapLog.InvokeRequired)
            {
                try { txtEapLog.BeginInvoke(new Action(() => AppendEapLog(message))); } catch { }
                return;
            }
            txtEapLog.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}");
        }

        #endregion

        #region Tab 1: MES WebService 模擬器原有功能

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Start(isManualClick: true);
        }

        private void Start(bool isManualClick = false)
        {
            if (_isRunning) return;

            string port = txtPort.Text.Trim();
            string selectedIp = MqttBrokerService.ExtractCleanIp(cboMesIp?.Text);
            if (string.IsNullOrWhiteSpace(selectedIp))
            {
                selectedIp = "localhost";
            }

            try
            {
                _listener = new HttpListener();
                if (selectedIp == "*" || selectedIp == "+" || selectedIp == "0.0.0.0")
                {
                    _listener.Prefixes.Add($"http://+:{port}/");
                }
                else if (selectedIp.Equals("localhost", StringComparison.OrdinalIgnoreCase) || selectedIp == "127.0.0.1")
                {
                    _listener.Prefixes.Add($"http://localhost:{port}/");
                    _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                }
                else
                {
                    _listener.Prefixes.Add($"http://{selectedIp}:{port}/");
                    _listener.Prefixes.Add($"http://localhost:{port}/");
                }

                _listener.Start();
                _isRunning = true;
                UpdateUI(true);
                var localIps = MqttBrokerService.GetLocalIpAddresses();
                string ipHint = (selectedIp == "*" || selectedIp == "+" || selectedIp == "0.0.0.0") && localIps.Count > 0
                    ? $" (本機可連線 IP: {string.Join(", ", localIps)}, localhost)"
                    : "";
                Log($"MES WebService 已啟動於 http://{selectedIp}:{port}/{ipHint}");

                Task.Run(() => ListenLoop());
            }
            catch (HttpListenerException httpEx) when (httpEx.ErrorCode == 5)
            {
                // ErrorCode 5 = Access Denied (未以系統管理員身分執行時綁定非 localhost IP)
                try { _listener?.Close(); } catch { }
                if (!isManualClick)
                {
                    // 程式啟動自動綁定時，自動降級為 localhost 確保能直接啟動
                    try
                    {
                        if (cboMesIp != null) cboMesIp.Text = "localhost";
                        _listener = new HttpListener();
                        _listener.Prefixes.Add($"http://localhost:{port}/");
                        _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                        _listener.Start();
                        _isRunning = true;
                        UpdateUI(true);
                        var localIps = MqttBrokerService.GetLocalIpAddresses();
                        string lanIpTip = localIps.Count > 0 ? string.Join(", ", localIps) : "區網 IP";
                        Log($"MES WebService 已啟動於 http://localhost:{port}/ (僅限本機連線)。");
                        Log($"[提示] 若需讓外部電腦連入 MES WebService ({lanIpTip}:{port})，請『以系統管理員身分執行』本程式，並將監聽 IP 選為 * (全部網卡) 或指定區網 IP。");
                        Task.Run(() => ListenLoop());
                        return;
                    }
                    catch (Exception fallbackEx)
                    {
                        Log($"[ERR] MES WebService 啟動失敗: {fallbackEx.Message}");
                    }
                }
                else
                {
                    MessageBox.Show(
                        $"綁定外部網卡 IP ({selectedIp}:{port}) 被 Windows 拒絕存取 (Error 5)。\n\n" +
                        "解決方式：\n" +
                        "1. 請關閉程式，對 MESSimulator.exe 按右鍵選擇『以系統管理員身分執行』即可綁定區網 IP。\n" +
                        "2. 若僅在本機測試，請將監聽 IP 改選為 localhost 或 127.0.0.1。",
                        "需要系統管理員權限",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動 MES WebService 失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (!_isRunning) return;
            
            _isRunning = false;
            _listener.Stop();
            _listener.Close();
            UpdateUI(false);
            Log("Server stopped.");
        }

        private async Task ListenLoop()
        {
            while (_isRunning)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
                catch { }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                string soapRequest = "";
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    soapRequest = await reader.ReadToEndAsync();
                }

                string soapAction = context.Request.Headers["SOAPAction"] ?? "";
                string jsonReq = ExtractJsonFromSoap(soapRequest);

                JObject parsedReq = null;
                try
                {
                    if (!string.IsNullOrWhiteSpace(jsonReq))
                    {
                        parsedReq = JObject.Parse(jsonReq);
                    }
                }
                catch { }

                bool isInsertLog = soapAction.IndexOf("InsertEqpTransactionLog", StringComparison.OrdinalIgnoreCase) >= 0
                    || soapRequest.IndexOf("InsertEqpTransactionLog", StringComparison.OrdinalIgnoreCase) >= 0
                    || parsedReq?["LogType"] != null;

                bool isInsertErrorLog = soapAction.IndexOf("InsertEqpErrorLog", StringComparison.OrdinalIgnoreCase) >= 0
                    || soapRequest.IndexOf("InsertEqpErrorLog", StringComparison.OrdinalIgnoreCase) >= 0;

                string soapResponse;

                if (isInsertLog)
                {
                    string logType = parsedReq?["LogType"]?.ToString() ?? "InsertEqpTransactionLog";
                    string machId = parsedReq?["MachID"]?.ToString() ?? parsedReq?["EqpNo"]?.ToString() ?? "";
                    string logId = parsedReq?["LogID"]?.ToString() ?? "";

                    this.Invoke(new Action(() => {
                        string header = $"[{DateTime.Now:HH:mm:ss.fff}] ======= {logType} Request (Mach: {machId}) =======";
                        txtRequest.AppendText(header + Environment.NewLine);
                        txtRequest.AppendText(soapRequest + Environment.NewLine);
                        txtRequest.AppendText(new string('=', header.Length) + Environment.NewLine + Environment.NewLine);
                    }));

                    Log($"[RECV-LOG] ({logType}) MachID={machId}, LogID={logId}");
                    soapResponse = WrapInSoapResponse("true", "InsertEqpTransactionLog");
                    Log($"[SEND-LOG] ({logType}) Result=true");
                }
                else if (isInsertErrorLog)
                {
                    this.Invoke(new Action(() => {
                        string header = $"[{DateTime.Now:HH:mm:ss.fff}] ======= InsertEqpErrorLog Request =======";
                        txtRequest.AppendText(header + Environment.NewLine);
                        txtRequest.AppendText(soapRequest + Environment.NewLine);
                        txtRequest.AppendText(new string('=', header.Length) + Environment.NewLine + Environment.NewLine);
                    }));

                    Log($"[RECV-ERRORLOG] {jsonReq}");
                    soapResponse = WrapInSoapResponse("true", "InsertEqpErrorLog");
                    Log($"[SEND-ERRORLOG] Result=true");
                }
                else
                {
                    string txnName = parsedReq?["TransactionName"]?.ToString() ?? "Unknown";

                    this.Invoke(new Action(() => {
                        string header = $"[{DateTime.Now:HH:mm:ss.fff}] ======= {txnName} Request =======";
                        txtRequest.AppendText(header + Environment.NewLine);
                        txtRequest.AppendText(soapRequest + Environment.NewLine);
                        txtRequest.AppendText(new string('=', header.Length) + Environment.NewLine + Environment.NewLine);
                    }));

                    Log($"[RECV] {jsonReq}");

                    // 2. 根據 TransactionName 生成回覆
                    string jsonReply = GenerateMockReply(jsonReq);
                    Log($"[SEND] {jsonReply}");

                    // 3. 封裝成 SOAP Response
                    soapResponse = WrapInSoapResponse(jsonReply, "EqpTransaction");
                }

                byte[] buffer = Encoding.UTF8.GetBytes(soapResponse);
                context.Response.ContentType = "text/xml; charset=utf-8";
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Log($"[ERR] {ex.Message}");
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
        }

        private string ExtractJsonFromSoap(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("mes", "CyntecMES");

                XmlNode node = doc.SelectSingleNode("//mes:pParameter", nsmgr);
                return node?.InnerText ?? "";
            }
            catch { return ""; }
        }

        private bool IsHandshakePass(string txnName)
        {
            if (this.InvokeRequired)
            {
                return (bool)this.Invoke(new Func<bool>(() => IsHandshakePass(txnName)));
            }

            switch (txnName?.ToUpper())
            {
                case "USERVERIFY":
                case "USER_VERIFY":
                case "USERIDVERIFY":
                    return chkUserVerifyPass == null || chkUserVerifyPass.Checked;
                case "WOQRY":
                    return chkWOQRYPass == null || chkWOQRYPass.Checked;
                case "WOCHECKIN":
                case "WOCHECKIN_MULTI":
                    return chkWOCHECKINPass == null || chkWOCHECKINPass.Checked;
                case "WOCHECKOUT":
                case "WOCHECKOUT_MULTI":
                    return chkWOCHECKOUTPass == null || chkWOCHECKOUTPass.Checked;
                case "EQPSTATUS":
                    return chkEQPSTATUSPass == null || chkEQPSTATUSPass.Checked;
                default:
                    return true;
            }
        }

        private string GenerateMockReply(string jsonReq)
        {
            if (string.IsNullOrEmpty(jsonReq)) return "{\"Result\":\"FAIL\", \"Message\":\"Empty Request\"}";

            try
            {
                var req = JObject.Parse(jsonReq);
                string txn = req["TransactionName"]?.ToString();

                if (!IsHandshakePass(txn))
                {
                    JObject failReply = new JObject();
                    failReply["TransactionName"] = txn;
                    failReply["Result"] = "FAIL";
                    failReply["ResultCode"] = "ERR_SIM_FAIL";
                    failReply["Message"] = $"[Simulation] Forced FAIL response for {txn}";
                    return failReply.ToString(Newtonsoft.Json.Formatting.None);
                }

                JObject reply = new JObject();
                reply["TransactionName"] = txn;
                reply["Result"] = "success";

                switch (txn?.ToUpper())
                {
                    case "USERVERIFY":
                    case "USER_VERIFY":
                    case "USERIDVERIFY":
                        string barcode = req["User2DBarCode"]?.ToString() ?? "";
                        string extractedId = req["UserID"]?.ToString() ?? "";
                        if (barcode.StartsWith("{{{{"))
                        {
                            var parts = barcode.Substring(4).Split('{');
                            if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0]))
                                extractedId = parts[0];
                        }
                        else if (!string.IsNullOrEmpty(barcode))
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(barcode, @"\d{6}");
                            if (match.Success)
                            {
                                extractedId = match.Value;
                            }
                        }

                        // 確保工號為 6 碼純數字 (例如: 123456)
                        if (string.IsNullOrEmpty(extractedId))
                        {
                            extractedId = "123456";
                        }
                        else
                        {
                            string digitsOnly = new string(extractedId.Where(char.IsDigit).ToArray());
                            if (string.IsNullOrEmpty(digitsOnly))
                            {
                                extractedId = "123456";
                            }
                            else if (digitsOnly.Length >= 6)
                            {
                                extractedId = digitsOnly.Substring(0, 6);
                            }
                            else
                            {
                                extractedId = digitsOnly.PadLeft(6, '0');
                            }
                        }

                        reply["UserID"] = extractedId;
                        reply["UserName"] = "模擬測試員";
                        reply["Result"] = "success";
                        reply["ResultCode"] = "0000";
                        reply["Message"] = extractedId;
                        reply["RtnMsg"] = extractedId;
                        break;

                    case "WOQRY":
                        string wonoQry = req["WONO"]?.ToString() ?? "";

                        // 檢查是否有設定自訂 TrackIn 失敗代碼 (ResultCode)
                        string customFailResultCode = "";
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                customFailResultCode = (string)this.Invoke(new Func<string>(() => txtSimWoQryResultCode?.Text?.Trim() ?? ""));
                            }
                            else
                            {
                                customFailResultCode = txtSimWoQryResultCode?.Text?.Trim() ?? "";
                            }
                        }
                        catch { }

                        if (!string.IsNullOrWhiteSpace(customFailResultCode))
                        {
                            reply["Result"] = "FAIL";
                            reply["ResultCode"] = customFailResultCode;
                            reply["Message"] = $"[Simulation] TrackIn WOQRY Simulated Failure with ResultCode: {customFailResultCode}";
                            break;
                        }

                        bool simError0049 = false;
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                simError0049 = (bool)this.Invoke(new Func<bool>(() => chkSimError0049 != null && chkSimError0049.Checked));
                            }
                            else
                            {
                                simError0049 = chkSimError0049 != null && chkSimError0049.Checked;
                            }
                        }
                        catch { }

                        if (simError0049 || wonoQry.IndexOf("ERROR0049", StringComparison.OrdinalIgnoreCase) >= 0 || wonoQry.IndexOf("ERR0049", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            reply["Result"] = "FAIL";
                            reply["ResultCode"] = "ERROR0049";
                            reply["Message"] = "此工單不屬於此站點可進站之工單";
                            break;
                        }

                        bool alreadyIn = _woAlreadyInMap.TryGetValue(wonoQry, out bool isIn) && isIn;
                        string customWipQty = "1000";
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                customWipQty = (string)this.Invoke(new Func<string>(() => txtWipQty?.Text?.Trim() ?? "1000"));
                            }
                            else
                            {
                                customWipQty = txtWipQty?.Text?.Trim() ?? "1000";
                            }
                        }
                        catch { }

                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "";
                        reply["MatGroupNo"] = "CL";
                        reply["LineCode"] = "CL01";
                        reply["MatNo"] = "9032910N06N";
                        reply["WIPQty"] = customWipQty;
                        reply["QTY"] = customWipQty;
                        reply["MatClassCode"] = "01005D";
                        reply["WorkCenterNo"] = "CLEX0008";
                        reply["FlowID"] = "100";
                        reply["AlreadyInFlag"] = alreadyIn ? "Y" : "N";
                        reply["SToPcs"] = "1";
                        reply["WToPcs"] = "5";
                        reply["ProductStage"] = "MP";
                        reply["RefInputQty"] = "4.000";
                        reply["BatchNo"] = "HPA2009000A7";
                        reply["WorkCenterName"] = "模擬測試站";
                        reply["IsUrgent"] = "N";
                        reply["CustomerDeliveryTime"] = "9999-12-31 00:00:00.000";

                        bool isRecipeMismatch = false;
                        string customRecipe = "";
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                isRecipeMismatch = (bool)this.Invoke(new Func<bool>(() => chkRecipeMismatch != null && chkRecipeMismatch.Checked));
                                customRecipe = (string)this.Invoke(new Func<string>(() => txtRecipeId?.Text?.Trim() ?? ""));
                            }
                            else
                            {
                                isRecipeMismatch = chkRecipeMismatch != null && chkRecipeMismatch.Checked;
                                customRecipe = txtRecipeId?.Text?.Trim() ?? "";
                            }
                        }
                        catch { }

                        string mockRecipe = "";
                        if (isRecipeMismatch)
                        {
                            int lastVal = !string.IsNullOrEmpty(wonoQry) ? (int)wonoQry[wonoQry.Length - 1] : 0;
                            mockRecipe = $"RECIPE_MISMATCH_{(lastVal % 2 == 0 ? "A" : "B")}_{wonoQry}";
                        }
                        else if (!string.IsNullOrEmpty(customRecipe))
                        {
                            mockRecipe = customRecipe;
                        }
                        else if (wonoQry.IndexOf("DIFF", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            mockRecipe = $"SIM_RECIPE_DIFF_{wonoQry}";
                        }
                        else
                        {
                            mockRecipe = new Random().Next(1000, 9999).ToString();
                        }

                        var paramDt = new JArray();
                        paramDt.Add(new JObject { 
                            ["ParamCode"] = "001",
                            ["ParamName"] = "RECIPEID", 
                            ["RequireInput"] = "N",
                            ["DataListItem"] = "",
                            ["DefaultValue"] = mockRecipe,
                            ["RefFieldCode"] = "RECIPE ID",
                            ["UpperValue"] = "",
                            ["LowerValue"] = "",
                            ["Showable"] = "Y"
                        });
                        reply["InputParamListDT"] = paramDt.ToString(Newtonsoft.Json.Formatting.None);

                        // 根據已經進站與否判斷是 1st WOQRY 還是 2nd WOQRY
                        string reqCodeStr = "";
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                reqCodeStr = (string)this.Invoke(new Func<string>(() =>
                                    alreadyIn
                                        ? (txtOutRequireInputCode != null ? txtOutRequireInputCode.Text : "EX001")
                                        : (txtInRequireInputCode != null ? txtInRequireInputCode.Text : "")));
                            }
                            else
                            {
                                reqCodeStr = alreadyIn
                                    ? (txtOutRequireInputCode != null ? txtOutRequireInputCode.Text : "EX001")
                                    : (txtInRequireInputCode != null ? txtInRequireInputCode.Text : "");
                            }
                        }
                        catch { }

                        var codeDt = new JArray();
                        var requiredList = new List<string>();
                        if (!string.IsNullOrWhiteSpace(reqCodeStr) && !reqCodeStr.Equals("無", StringComparison.OrdinalIgnoreCase) && !reqCodeStr.Equals("NONE", StringComparison.OrdinalIgnoreCase))
                        {
                            var codes = reqCodeStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var c in codes)
                            {
                                string cleanCode = c.Trim();
                                if (!string.IsNullOrEmpty(cleanCode))
                                {
                                    requiredList.Add(cleanCode);
                                    codeDt.Add(new JObject {
                                        ["InputCode"] = cleanCode,
                                        ["InputCodeName"] = $"管理項目{cleanCode}",
                                        ["RequireInput"] = "Y",
                                        ["DataListItem"] = "",
                                        ["DefaultValue"] = ""
                                    });
                                }
                            }
                        }
                        reply["InputCodeListDT"] = codeDt.ToString(Newtonsoft.Json.Formatting.None);
                        _woRequiredCodesMap[wonoQry] = requiredList;

                        bool enableNGCodeList = true;
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                enableNGCodeList = (bool)this.Invoke(new Func<bool>(() => chkEnableNGCodeList == null || chkEnableNGCodeList.Checked));
                            }
                            else
                            {
                                enableNGCodeList = chkEnableNGCodeList == null || chkEnableNGCodeList.Checked;
                            }
                        }
                        catch { }

                        if (enableNGCodeList)
                        {
                            var ngArray = new JArray
                            {
                                new JObject { ["NGCode"] = "UPNG501", ["NGName"] = "機台卡片" },
                                new JObject { ["NGCode"] = "UPNG502", ["NGName"] = "人為疏失折片" },
                                new JObject { ["NGCode"] = "UPNG503", ["NGName"] = "工程驗證抽片" },
                                new JObject { ["NGCode"] = "QC_KEEP", ["NGName"] = "QC Keep Sample" },
                                new JObject { ["NGCode"] = "FREE_SUP", ["NGName"] = "免補料" }
                            };
                            reply["NGCodeListDT"] = ngArray.ToString(Newtonsoft.Json.Formatting.None);
                        }
                        else
                        {
                            reply["NGCodeListDT"] = "[]";
                        }
                        break;

                    case "WOCHECKIN":
                        string wonoIn = req["WONO"]?.ToString() ?? "";
                        if (_woRequiredCodesMap.TryGetValue(wonoIn, out var reqInCodes) && reqInCodes != null && reqInCodes.Count > 0)
                        {
                            string inputListJson = req["InputListDt"]?.ToString();
                            var providedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            if (!string.IsNullOrEmpty(inputListJson))
                            {
                                try
                                {
                                    var arr = JArray.Parse(inputListJson);
                                    foreach (JObject item in arr)
                                    {
                                        string code = item["InputCode"]?.ToString();
                                        string val = item["InputValue"]?.ToString();
                                        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(val))
                                        {
                                            providedCodes.Add(code.Trim());
                                        }
                                    }
                                }
                                catch { }
                            }

                            foreach (var reqCode in reqInCodes)
                            {
                                if (!providedCodes.Contains(reqCode))
                                {
                                    reply["Result"] = "FAIL";
                                    reply["ResultCode"] = "ERR_MISSING_INPUT";
                                    reply["Message"] = $"[WOCHECKIN] Missing required InputCode: {reqCode}";
                                    return reply.ToString(Newtonsoft.Json.Formatting.None);
                                }
                            }
                        }
                        _woAlreadyInMap[wonoIn] = true;
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "WOCHECKIN_MULTI":
                        var wosInMulti = new List<string>();
                        if (req["WoNoDt"] != null)
                        {
                            try
                            {
                                var arr = JArray.Parse(req["WoNoDt"].ToString());
                                foreach (var item in arr)
                                {
                                    string wo = item["WoNo"]?.ToString();
                                    if (!string.IsNullOrEmpty(wo)) wosInMulti.Add(wo);
                                }
                            }
                            catch { }
                        }
                        else if (req["WoNoList"] != null && req["WoNoList"] is JArray arrIn)
                        {
                            foreach (var item in arrIn)
                            {
                                string wo = item["WoNo"]?.ToString();
                                if (!string.IsNullOrEmpty(wo)) wosInMulti.Add(wo);
                            }
                        }

                        foreach (var wo in wosInMulti)
                        {
                            _woAlreadyInMap[wo] = true;
                        }
                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "Simulation Multi-Checkin Success";
                        break;

                    case "WOCHECKOUT":
                        string wonoOut = req["WONO"]?.ToString() ?? "";
                        if (_woRequiredCodesMap.TryGetValue(wonoOut, out var reqOutCodes) && reqOutCodes != null && reqOutCodes.Count > 0)
                        {
                            string inputListJson = req["InputListDt"]?.ToString();
                            var providedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            if (!string.IsNullOrEmpty(inputListJson))
                            {
                                try
                                {
                                    var arr = JArray.Parse(inputListJson);
                                    foreach (JObject item in arr)
                                    {
                                        string code = item["InputCode"]?.ToString();
                                        string val = item["InputValue"]?.ToString();
                                        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(val))
                                        {
                                            providedCodes.Add(code.Trim());
                                        }
                                    }
                                }
                                catch { }
                            }

                            foreach (var reqCode in reqOutCodes)
                            {
                                if (!providedCodes.Contains(reqCode))
                                {
                                    reply["Result"] = "FAIL";
                                    reply["ResultCode"] = "ERR_MISSING_INPUT";
                                    reply["Message"] = $"[WOCHECKOUT] Missing required InputCode: {reqCode}";
                                    return reply.ToString(Newtonsoft.Json.Formatting.None);
                                }
                            }
                        }
                        string ngListOut = req["NGListDt"]?.ToString() ?? req["NGListDT"]?.ToString();
                        if (!string.IsNullOrEmpty(ngListOut))
                        {
                            Log($"[WOCHECKOUT] Received NGListDt for {wonoOut}: {ngListOut}");
                        }

                        _woAlreadyInMap[wonoOut] = false;
                        _woRequiredCodesMap.TryRemove(wonoOut, out _);
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "WOCHECKOUT_MULTI":
                        var wosOutMulti = new List<string>();
                        if (req["WoNoDt"] != null)
                        {
                            try
                            {
                                var arr = JArray.Parse(req["WoNoDt"].ToString());
                                foreach (var item in arr)
                                {
                                    string wo = item["WoNo"]?.ToString();
                                    if (!string.IsNullOrEmpty(wo)) wosOutMulti.Add(wo);
                                }
                            }
                            catch { }
                        }
                        else if (req["WoNoList"] != null && req["WoNoList"] is JArray arrOut)
                        {
                            foreach (var item in arrOut)
                            {
                                string wo = item["WoNo"]?.ToString();
                                if (!string.IsNullOrEmpty(wo)) wosOutMulti.Add(wo);
                            }
                        }

                        foreach (var wo in wosOutMulti)
                        {
                            _woAlreadyInMap[wo] = false;
                            _woRequiredCodesMap.TryRemove(wo, out _);
                        }
                        reply["Result"] = "success";
                        reply["ResultCode"] = "W001";
                        reply["Message"] = "Simulation Multi-Checkout Success";
                        break;

                    case "EQPSTATUS":
                        reply["Result"] = "success";
                        reply["Message"] = "Simulation Success";
                        break;

                    case "ADDPRODUCTLEFTOVER":
                    case "ADD_PRODUCT_LEFTOVER":
                        string leftoverWono = req["WONO"]?.ToString() ?? "";
                        string leftoverQty = req["Qty"]?.ToString() ?? "0";
                        string leftoverReelId = req["ReelID"]?.ToString() ?? "";
                        string leftoverEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string leftoverUserId = req["UserID"]?.ToString() ?? "";

                        Log($"[AddProductLeftover] Received WONO: {leftoverWono}, Qty: {leftoverQty}, ReelID: {leftoverReelId}, EqpNo: {leftoverEqpNo}, UserID: {leftoverUserId}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation AddProductLeftover Success";
                        break;

                    case "GETLEFTOVERWONO":
                    case "GET_LEFTOVER_WONO":
                        string getWo = req["WONO"]?.ToString() ?? "";
                        string getReelId = req["ReelID"]?.ToString() ?? "";
                        string getEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string getUserId = req["UserID"]?.ToString() ?? "";

                        Log($"[GetLeftoverWONo] Received WONO: {getWo}, ReelID: {getReelId}, EqpNo: {getEqpNo}, UserID: {getUserId}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation GetLeftoverWONo Success";
                        reply["Qty"] = "10";
                        reply["WONO"] = getWo;
                        reply["BachNo"] = "BATCH_SIM";
                        reply["Loc"] = "LOC01";
                        break;

                    case "GETEQPINFO":
                    case "GET_EQP_INFO":
                        string infoEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string infoEqpUser = req["UserID"]?.ToString() ?? "";
                        Log($"[GetEqpInfo] Received EqpNo: {infoEqpNo}, UserID: {infoEqpUser}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation GetEqpInfo Success";
                        reply["MachID"] = infoEqpNo;
                        reply["EqpName"] = infoEqpNo;
                        reply["EqpAlias"] = $"{infoEqpNo}_別名";
                        reply["GroupName"] = "捲帶編帶課";
                        reply["EnableCheck"] = "Y";
                        reply["ClassCode"] = "TP_LINE01";
                        break;

                    case "USEPRODUCTLEFTOVER":
                    case "USE_PRODUCT_LEFTOVER":
                        string useWono = req["WONO"]?.ToString() ?? "";
                        string useReelId = req["ReelID"]?.ToString() ?? "";
                        string useLeftoverWo = req["LeftOverWONo"]?.ToString() ?? "";
                        string useLeftoverBatch = req["LeftOverBatchNo"]?.ToString() ?? "";
                        string useAddQty = req["AddQty"]?.ToString() ?? "0";
                        string useGapQty = req["GapQty"]?.ToString() ?? "0";
                        string useEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string useUserId = req["UserID"]?.ToString() ?? "";

                        Log($"[UseProductLeftover] Received WONO: {useWono}, ReelID: {useReelId}, LeftOverWONo: {useLeftoverWo}, LeftOverBatchNo: {useLeftoverBatch}, AddQty: {useAddQty}, GapQty: {useGapQty}, EqpNo: {useEqpNo}, UserID: {useUserId}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation UseProductLeftover Success";
                        break;

                    case "EQPWONGADD":
                    case "EQP_WO_NG_ADD":
                        string wongWono = req["WONo"]?.ToString() ?? req["WONO"]?.ToString() ?? "";
                        string wongFlowId = req["FlowID"]?.ToString() ?? "";
                        string wongEqpNo = req["EqpNo"]?.ToString() ?? "";
                        string wongUserId = req["UserID"]?.ToString() ?? "";
                        string wongNgListDt = req["NGListDt"]?.ToString() ?? "";

                        Log($"[EqpWONGAdd] Received WONo: {wongWono}, FlowID: {wongFlowId}, EqpNo: {wongEqpNo}, UserID: {wongUserId}, NGListDt: {wongNgListDt}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "000";
                        reply["Message"] = "Simulation EqpWONGAdd Success";
                        break;

                    case "CONNECTION":
                        string connEqpNo = req["EqpNo"]?.ToString() ?? "";
                        Log($"[CONNECTION] Received EqpNo: {connEqpNo}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "0000";
                        reply["Message"] = "Simulation Connection Success";
                        var resultTable = new Newtonsoft.Json.Linq.JArray(
                            new Newtonsoft.Json.Linq.JObject
                            {
                                ["EqpNo"] = connEqpNo,
                                ["Status"] = "ONLINE",
                                ["ConnectionTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            }
                        );
                        reply["ResultDt"] = resultTable.ToString(Newtonsoft.Json.Formatting.None);
                        break;

                    case "EDCDATAADD":
                    case "EDC_DATA_ADD":
                        string edcWono = req["WONO"]?.ToString() ?? req["WONo"]?.ToString() ?? "";
                        string edcEqpNo = req["EqpNo"]?.ToString() ?? "";
                        Log($"[EDCDATAADD] Received WONO: {edcWono}, EqpNo: {edcEqpNo}");
                        reply["Result"] = "success";
                        reply["ResultCode"] = "0000";
                        reply["Message"] = "Simulation EDCDATAADD Success";
                        break;

                    default:
                        reply["Result"] = "FAIL";
                        reply["Message"] = $"Unsupported Transaction: {txn}";
                        break;
                }

                return reply.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch (Exception ex)
            {
                return $"{{\"Result\":\"FAIL\", \"Message\":\"Simulator Error: {ex.Message}\"}}";
            }
        }

        private string WrapInSoapResponse(string replyContent, string methodName = "EqpTransaction")
        {
            string innerContent = methodName == "EqpTransaction" ? SecurityElement.Escape(replyContent) : replyContent;
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <soap:Body>
    <{methodName}Response xmlns=""CyntecMES"">
      <{methodName}Result>{innerContent}</{methodName}Result>
    </{methodName}Response>
  </soap:Body>
</soap:Envelope>";
        }

        private void UpdateUI(bool running)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateUI(running))); return; }
            
            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            if (cboMesIp != null) cboMesIp.Enabled = !running;
            txtPort.Enabled = !running;
            string ip = MqttBrokerService.ExtractCleanIp(cboMesIp?.Text);
            if (string.IsNullOrWhiteSpace(ip)) ip = "localhost";
            lblStatus.Text = running ? $"● Server Running ({ip}:{txtPort.Text.Trim()})" : "● Server Stopped";
            lblStatus.ForeColor = running ? Color.Lime : Color.Red;
        }

        private void Log(string message)
        {
            if (this.IsDisposed || txtLog == null || txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired)
            {
                try { txtLog.BeginInvoke(new Action(() => Log(message))); } catch { }
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        #endregion

        #region Form Lifecycle

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // 1. 啟動 MES SOAP Server
            Start();

            // 2. 啟動內建 MQTT Broker (預設 Port 1883)
            await StartBrokerInternalAsync();

            // 3. EAP 模擬器預設維持停止狀態，由使用者手動點擊「啟動 EAP」
            UpdateEapUI(false);
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_isRunning)
                {
                    _isRunning = false;
                    _listener?.Stop();
                    _listener?.Close();
                }

                if (_eapSimulatorService != null && _eapSimulatorService.IsRunning)
                {
                    await _eapSimulatorService.StopAsync();
                }

                if (_brokerService != null && _brokerService.IsRunning)
                {
                    await _brokerService.StopAsync();
                }
            }
            catch { }
        }

        #endregion
    }
}
