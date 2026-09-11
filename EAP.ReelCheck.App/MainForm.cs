using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using EAP.ReelCheck.Communication;
using EAP.ReelCheck.Configuration;
using EAP.ReelCheck.Engine;
using EAP.ReelCheck.Models;
using EAP.ReelCheck.Parsers;
using EAP.ReelCheck.Service;

namespace EAP.ReelCheck.App;

public class MainForm : Form
{
    // 核心服務
    private ReelCheckService? _reelCheckService;
    private IReelCheckMesService? _mesService;
    private ReelCheckOptions _options = new();
    private WorkOrderContext? _currentContext;

    // UI 控制項 - 頂部工單設定區
    private TextBox _txtMachineNo = null!;
    private TextBox _txtWoNo = null!;
    private TextBox _txtUserId = null!;
    private TextBox _txtMatNo = null!;
    private TextBox _txtBatchNo = null!;
    private Button _btnInitWorkOrder = null!;
    private CheckBox _chkSimulateOffline = null!;

    // UI 控制項 - 條碼輸入與檢驗狀態區
    private TextBox _txtBarcodeScan = null!;
    private Button _btnSubmitScan = null!;
    private Panel _pnlStatusLight = null!;
    private Label _lblStatusText = null!;
    private Label _lblStatusDetail = null!;
    private Label _lblDecodedReelId = null!;

    // UI 控制項 - 快捷測試工具按鈕
    private Button _btnTestScanMatNo = null!;
    private Button _btnTestScanBatchNo = null!;
    private Button _btnTestScanLegalReel = null!;
    private Button _btnTestScanIllegalReel = null!;

    // UI 控制項 - 表格與日誌
    private DataGridView _dgvReels = null!;
    private RichTextBox _rtbLog = null!;

    public MainForm()
    {
        InitializeCustomComponents();
        LoadConfiguration();
    }

    private void InitializeCustomComponents()
    {
        this.Text = "EAP 標籤防混檢驗展示系統 (EAP.ReelCheck Demo)";
        this.Size = new Size(1200, 850);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        this.BackColor = Color.FromArgb(245, 247, 250);

        // 主容器
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(12)
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // 1. 工單設定
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F)); // 2. 掃碼檢驗看板
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));    // 3. Reel 清單表格
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));    // 4. 即時日誌

        // 1. 工單設定群組
        var grpWo = new GroupBox
        {
            Text = "🏭 工單與機台作業設定",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
        };
        var woPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            Padding = new Padding(8)
        };

        woPanel.Controls.Add(new Label { Text = "機台編號:", AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
        _txtMachineNo = new TextBox { Text = "TP-01", Width = 90 };
        woPanel.Controls.Add(_txtMachineNo);

        woPanel.Controls.Add(new Label { Text = "工單號碼:", AutoSize = true, Margin = new Padding(10, 8, 4, 0) });
        _txtWoNo = new TextBox { Text = "WO2026091101", Width = 130 };
        woPanel.Controls.Add(_txtWoNo);

        woPanel.Controls.Add(new Label { Text = "作業人員:", AutoSize = true, Margin = new Padding(10, 8, 4, 0) });
        _txtUserId = new TextBox { Text = "OP001", Width = 90 };
        woPanel.Controls.Add(_txtUserId);

        _btnInitWorkOrder = new Button
        {
            Text = "🚀 開工初始化",
            Width = 110,
            Height = 30,
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        _btnInitWorkOrder.Click += async (s, e) => await InitializeWorkOrderAsync();
        woPanel.Controls.Add(_btnInitWorkOrder);

        _chkSimulateOffline = new CheckBox
        {
            Text = "離線模擬模式 (免接 MES)",
            AutoSize = true,
            Checked = true,
            Margin = new Padding(15, 6, 0, 0)
        };
        woPanel.Controls.Add(_chkSimulateOffline);

        // 第二行顯示工單料號與批號
        var woDetailPanel = new FlowLayoutPanel { Width = 1100, Height = 35, Margin = new Padding(0, 4, 0, 0) };
        woDetailPanel.Controls.Add(new Label { Text = "工單料號 (MatNo):", AutoSize = true, Margin = new Padding(0, 6, 4, 0) });
        _txtMatNo = new TextBox { Width = 150, ReadOnly = true, BackColor = Color.FromArgb(235, 235, 235) };
        woDetailPanel.Controls.Add(_txtMatNo);

        woDetailPanel.Controls.Add(new Label { Text = "工單批號 (BatchNo):", AutoSize = true, Margin = new Padding(20, 6, 4, 0) });
        _txtBatchNo = new TextBox { Width = 150, ReadOnly = true, BackColor = Color.FromArgb(235, 235, 235) };
        woDetailPanel.Controls.Add(_txtBatchNo);
        woPanel.Controls.Add(woDetailPanel);

        grpWo.Controls.Add(woPanel);
        mainLayout.Controls.Add(grpWo, 0, 0);

        // 2. 條碼掃描與檢驗結果看板
        var grpScan = new GroupBox
        {
            Text = "🔍 條碼掃描與防混檢驗",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
        };
        var scanLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        scanLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        scanLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

        // 左側：輸入框與快捷按鈕
        var scanLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6) };
        scanLeft.Controls.Add(new Label { Text = "請掃入條碼 (1D 料號/批號 或 2D Reel 標籤):", AutoSize = true, Font = new Font("Segoe UI", 9F) });
        
        var inputRow = new FlowLayoutPanel { Width = 550, Height = 35 };
        _txtBarcodeScan = new TextBox { Width = 400, Font = new Font("Consolas", 11F) };
        _txtBarcodeScan.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await ProcessScanAsync(); } };
        inputRow.Controls.Add(_txtBarcodeScan);

        _btnSubmitScan = new Button { Text = "驗證 ⏎", Width = 80, Height = 28, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        _btnSubmitScan.Click += async (s, e) => await ProcessScanAsync();
        inputRow.Controls.Add(_btnSubmitScan);
        scanLeft.Controls.Add(inputRow);

        // 快捷模擬測試按鈕
        var testButtonsRow = new FlowLayoutPanel { Width = 600, Height = 35, Margin = new Padding(0, 4, 0, 0) };
        _btnTestScanMatNo = new Button { Text = "刷 1D 料號", Width = 95, Height = 26 };
        _btnTestScanMatNo.Click += (s, e) => { if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.MatNo; };

        _btnTestScanBatchNo = new Button { Text = "刷 1D 批號", Width = 95, Height = 26 };
        _btnTestScanBatchNo.Click += (s, e) => { if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.BatchNo; };

        _btnTestScanLegalReel = new Button { Text = "刷 合法 Reel", Width = 110, Height = 26, BackColor = Color.FromArgb(230, 247, 235) };
        _btnTestScanLegalReel.Click += (s, e) =>
        {
            if (_currentContext != null && _dgvReels.Rows.Count > 0)
            {
                string legalReelId = _dgvReels.Rows[0].Cells["ReelId"].Value?.ToString() ?? "REEL001";
                string cipher = Barcode2DParser.EncodeReelId(legalReelId);
                _txtBarcodeScan.Text = $"{_currentContext.MatNo}{{{{BATCH-888888{{88{{{{{cipher}";
            }
        };

        _btnTestScanIllegalReel = new Button { Text = "刷 混料 Reel (NG)", Width = 130, Height = 26, BackColor = Color.FromArgb(255, 235, 235), ForeColor = Color.DarkRed };
        _btnTestScanIllegalReel.Click += (s, e) =>
        {
            if (_currentContext != null)
            {
                string cipher = Barcode2DParser.EncodeReelId("REEL-WRONG-999");
                _txtBarcodeScan.Text = $"{_currentContext.MatNo}{{{{BATCH-888888{{88{{{{{cipher}";
            }
        };

        testButtonsRow.Controls.AddRange(new Control[] { _btnTestScanMatNo, _btnTestScanBatchNo, _btnTestScanLegalReel, _btnTestScanIllegalReel });
        scanLeft.Controls.Add(testButtonsRow);
        scanLayout.Controls.Add(scanLeft, 0, 0);

        // 右側：大型狀態指示面板
        _pnlStatusLight = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(230, 230, 230),
            Padding = new Padding(10)
        };
        _lblStatusText = new Label
        {
            Text = "STANDBY 等待開工",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.DimGray,
            Dock = DockStyle.Top,
            Height = 35,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _lblStatusDetail = new Label
        {
            Text = "請點擊 [開工初始化] 載入工單",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(60, 60, 60),
            Dock = DockStyle.Top,
            Height = 25,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _lblDecodedReelId = new Label
        {
            Text = "還原 Reel ID: ---",
            Font = new Font("Consolas", 10.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 40, 40),
            Dock = DockStyle.Top,
            Height = 25,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _pnlStatusLight.Controls.Add(_lblDecodedReelId);
        _pnlStatusLight.Controls.Add(_lblStatusDetail);
        _pnlStatusLight.Controls.Add(_lblStatusText);
        scanLayout.Controls.Add(_pnlStatusLight, 1, 0);

        grpScan.Controls.Add(scanLayout);
        mainLayout.Controls.Add(grpScan, 0, 1);

        // 3. Reel 清單表格
        var grpGrid = new GroupBox
        {
            Text = "📋 工單料捲 (Reel) 狀態清單",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
        };
        _dgvReels = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible = false,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular)
        };
        _dgvReels.Columns.Add("ReelId", "Reel ID (料捲編號)");
        _dgvReels.Columns.Add("Status", "狀態");
        _dgvReels.Columns.Add("MachNo", "綁定機台");
        _dgvReels.Columns.Add("StartTime", "上機起始時間");
        _dgvReels.Columns.Add("ReelScanCount", "Reel 刷次");
        _dgvReels.Columns.Add("MatScanCount", "料號刷次");
        _dgvReels.Columns.Add("TPScanCount", "批號刷次");
        grpGrid.Controls.Add(_dgvReels);
        mainLayout.Controls.Add(grpGrid, 0, 2);

        // 4. 即時日誌區
        var grpLog = new GroupBox
        {
            Text = "📜 即時操作與通訊歷程 (Log)",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
        };
        _rtbLog = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.FromArgb(220, 220, 220),
            Font = new Font("Consolas", 9F),
            BorderStyle = BorderStyle.None
        };
        grpLog.Controls.Add(_rtbLog);
        mainLayout.Controls.Add(grpLog, 0, 3);

        this.Controls.Add(mainLayout);
    }

    private void LoadConfiguration()
    {
        try
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);
                if (jObj["ReelCheck"] != null)
                {
                    _options.ServerUrl = jObj["ReelCheck"]?["ServerUrl"]?.ToString() ?? _options.ServerUrl;
                    _options.ClientUrl = jObj["ReelCheck"]?["ClientUrl"]?.ToString() ?? _options.ClientUrl;
                }
                if (jObj["Defaults"] != null)
                {
                    _txtMachineNo.Text = jObj["Defaults"]?["MachineNo"]?.ToString() ?? _txtMachineNo.Text;
                    _txtWoNo.Text = jObj["Defaults"]?["WoNo"]?.ToString() ?? _txtWoNo.Text;
                    _txtUserId.Text = jObj["Defaults"]?["OperatorId"]?.ToString() ?? _txtUserId.Text;
                }
            }
        }
        catch (Exception ex)
        {
            Log($"載入設定檔失敗: {ex.Message}", Color.Orange);
        }
    }

    private async Task InitializeWorkOrderAsync()
    {
        string woNo = _txtWoNo.Text.Trim();
        string machNo = _txtMachineNo.Text.Trim();
        string userId = _txtUserId.Text.Trim();

        if (string.IsNullOrWhiteSpace(woNo))
        {
            MessageBox.Show("請輸入工單號碼！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _btnInitWorkOrder.Enabled = false;
        Log($"開始工單開工初始化... WO=[{woNo}], Machine=[{machNo}]", Color.Cyan);

        try
        {
            // 判斷是否使用離線模擬
            if (_chkSimulateOffline.Checked)
            {
                _mesService = new MockOfflineMesService();
                Log("模式: 離線模擬服務 (MockOfflineMesService)", Color.LightGreen);
            }
            else
            {
                _mesService = new WcfReelCheckMesService(_options);
                Log($"模式: MES WCF 實體連線 ({_options.ServerUrl})", Color.Yellow);
            }

            _reelCheckService = new ReelCheckService(_mesService);
            _reelCheckService.OnScanResult += (s, result) => this.Invoke(new Action(() => UpdateScanResultUI(result)));
            _reelCheckService.OnReelBlocked += (s, result) => this.Invoke(new Action(() => ShowBlockedAlert(result)));

            _currentContext = await _reelCheckService.InitializeWorkOrderAsync(woNo, machNo, userId);

            // 更新 UI
            _txtMatNo.Text = _currentContext.MatNo;
            _txtBatchNo.Text = _currentContext.BatchNo;

            RefreshReelsGrid();
            SetStatusLight("READY", "工單初始化完成，請開始掃描", Color.FromArgb(52, 152, 219));
            Log($"工單初始化成功！料號={_currentContext.MatNo}, 批號={_currentContext.BatchNo}", Color.White);

            _txtBarcodeScan.Focus();
        }
        catch (Exception ex)
        {
            Log($"工單初始化異常: {ex.Message}", Color.Red);
            SetStatusLight("INIT_ERROR", ex.Message, Color.FromArgb(231, 76, 60));
            MessageBox.Show($"初始化失敗: {ex.Message}", "連線異常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _btnInitWorkOrder.Enabled = true;
        }
    }

    private async Task ProcessScanAsync()
    {
        string raw = _txtBarcodeScan.Text.Trim();
        if (string.IsNullOrWhiteSpace(raw)) return;

        if (_reelCheckService == null || !_reelCheckService.IsInitialized)
        {
            MessageBox.Show("請先完成工單開工初始化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _txtBarcodeScan.SelectAll();
        Log($"[SCAN] 掃描輸入: {raw}", Color.LightSkyBlue);

        var result = await _reelCheckService.ScanAsync(raw);
        RefreshReelsGrid();
    }

    private void UpdateScanResultUI(ReelCheckResult result)
    {
        if (result.IsSuccess)
        {
            if (result.Disposition == CheckDisposition.Ignore)
            {
                SetStatusLight("IGNORED", result.Message, Color.FromArgb(243, 156, 18));
                Log($"[IGNORE] {result.Message}", Color.Yellow);
            }
            else
            {
                string reelText = string.IsNullOrEmpty(result.DecodedReelId) ? "---" : result.DecodedReelId;
                _lblDecodedReelId.Text = $"還原 Reel ID: [{reelText}]";
                SetStatusLight("PASS 檢驗合格", result.Message, Color.FromArgb(46, 204, 113));
                Log($"[PASS] {result.Message} (Reel={reelText})", Color.LightGreen);
            }
        }
        else
        {
            string reelText = string.IsNullOrEmpty(result.DecodedReelId) ? "---" : result.DecodedReelId;
            _lblDecodedReelId.Text = $"還原 Reel ID: [{reelText}]";

            if (result.ShouldBlockWorkOrder)
            {
                SetStatusLight($"LOCK 混料鎖定 [{result.ErrorCode}]", result.Message, Color.FromArgb(192, 57, 43));
                Log($"[BLOCK] 觸發重大防混鎖單！代碼: {result.ErrorCode}, 內容: {result.Message}", Color.OrangeRed);
            }
            else
            {
                SetStatusLight($"WARN 警告 [{result.ErrorCode}]", result.Message, Color.FromArgb(230, 126, 34));
                Log($"[WARN] 警告: {result.ErrorCode} - {result.Message}", Color.Orange);
            }
        }
    }

    private void ShowBlockedAlert(ReelCheckResult result)
    {
        MessageBox.Show($"【防混料重大異常】\n\n錯誤代碼：{result.ErrorCode}\n原因：{result.Message}\n\n已觸發 MES 工單鎖定並通知現場停機！", "重大混料警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void SetStatusLight(string title, string detail, Color color)
    {
        _pnlStatusLight.BackColor = color;
        _lblStatusText.Text = title;
        _lblStatusText.ForeColor = Color.White;
        _lblStatusDetail.Text = detail;
        _lblStatusDetail.ForeColor = Color.White;
        _lblDecodedReelId.ForeColor = Color.White;
    }

    private void RefreshReelsGrid()
    {
        if (_reelCheckService == null) return;

        var reels = _reelCheckService.GetReelList();
        _dgvReels.Rows.Clear();

        foreach (var r in reels)
        {
            string statusDisplay = r.Status switch
            {
                "00" => "待上機 (00)",
                "11" => "作業中 (11) 🟢",
                _ => r.Status
            };

            string startTimeStr = r.StartTime == DateTime.MinValue ? "---" : r.StartTime.ToString("HH:mm:ss");

            int rowIdx = _dgvReels.Rows.Add(r.ReelId, statusDisplay, r.TPMachNo, startTimeStr, r.ReelScanCount, r.MatNoScanCount, r.TPNoScanCount);

            if (r.Status == "11")
            {
                _dgvReels.Rows[rowIdx].DefaultCellStyle.BackColor = Color.FromArgb(235, 250, 240);
                _dgvReels.Rows[rowIdx].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
        }
    }

    private void Log(string message, Color color)
    {
        if (_rtbLog.InvokeRequired)
        {
            _rtbLog.Invoke(new Action(() => Log(message, color)));
            return;
        }

        string time = DateTime.Now.ToString("HH:mm:ss.fff");
        _rtbLog.SelectionStart = _rtbLog.TextLength;
        _rtbLog.SelectionLength = 0;
        _rtbLog.SelectionColor = Color.Gray;
        _rtbLog.AppendText($"[{time}] ");

        _rtbLog.SelectionColor = color;
        _rtbLog.AppendText(message + Environment.NewLine);
        _rtbLog.ScrollToCaret();
    }
}

/// <summary>
/// 離線模擬 MES 服務，用於無 MES WCF 伺服器之離線開發與驗證
/// </summary>
public class MockOfflineMesService : IReelCheckMesService
{
    private readonly List<ReelItem> _mockReels = new();

    public Task<WorkOrderContext> QueryWorkOrderInfoAsync(string woNo, string machNo, string userId, System.Threading.CancellationToken ct = default)
    {
        _mockReels.Clear();
        _mockReels.Add(new ReelItem { ReelId = "REEL-001", Status = "00", TPMachNo = machNo, MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" });
        _mockReels.Add(new ReelItem { ReelId = "REEL-002", Status = "00", TPMachNo = machNo, MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" });
        _mockReels.Add(new ReelItem { ReelId = "REEL-003", Status = "00", TPMachNo = machNo, MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" });

        return Task.FromResult(new WorkOrderContext
        {
            WONo = woNo,
            MachineNo = machNo,
            OperatorId = userId,
            MatNo = "MAT-1001-CYN",
            BatchNo = "BATCH-888888",
            MinMatNoScanCount = 1,
            MinTPNoScanCount = 1,
            MinReelDurationMinutes = 0,
            IsCheckEnabled = true
        });
    }

    public Task<List<ReelItem>> QueryReelListAsync(string woNo, System.Threading.CancellationToken ct = default)
    {
        return Task.FromResult(_mockReels.ToList());
    }

    public Task<bool> UpdateReelStatusAsync(string reelId, string statusCode, string machNo, string userId, DateTime startTime, DateTime endTime, System.Threading.CancellationToken ct = default)
    {
        var target = _mockReels.FirstOrDefault(r => r.ReelId == reelId);
        if (target != null)
        {
            target.Status = statusCode;
            target.StartTime = startTime;
        }
        return Task.FromResult(true);
    }

    public Task<bool> AddReelLogAsync(string woNo, string reelId, string matNo, string batchNo, string userId, string action = "00005", System.Threading.CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<bool> SaveReelCheckRecordAsync(string woNo, string machNo, string reelId, string matNo, string batchNo, int reelCount, int matCount, int tpCount, System.Threading.CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<bool> BlockWorkOrderAsync(string woNo, string reasonCode, string remark, System.Threading.CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<string?> QueryOtherWorkOrderByBarcodeAsync(string barcode, System.Threading.CancellationToken ct = default)
        => Task.FromResult<string?>(null);

    public Task<bool> SavePackingAsync(string woNo, string batchNo, string matNo, string reelId, string userId, string workCenterNo, System.Threading.CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<bool> PingAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(true);
}
