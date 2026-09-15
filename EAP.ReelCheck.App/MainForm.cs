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

public partial class MainForm : Form
{
    // 核心服務
    private ReelCheckService? _reelCheckService;
    private IReelCheckMesService? _mesService;
    private ReelCheckOptions _options = new();
    private WorkOrderContext? _currentContext;

    public MainForm()
    {
        InitializeComponent();
        AttachEventHandlers();
        LoadConfiguration();
    }

    private void AttachEventHandlers()
    {
        _btnInitWorkOrder.Click += async (s, e) => await InitializeWorkOrderAsync();
        _btnSubmitScan.Click += async (s, e) => await ProcessScanAsync();
        _txtBarcodeScan.KeyDown += async (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await ProcessScanAsync();
            }
        };

        _btnTestScanMatNo.Click += (s, e) => { if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.MatNo; };
        _btnTestScanBatchNo.Click += (s, e) => { if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.BatchNo; };

        _btnTestScanLegalReel.Click += (s, e) =>
        {
            if (_currentContext != null && _dgvReels.Rows.Count > 0)
            {
                int colIdx = _colReelId?.Index >= 0 ? _colReelId.Index : 0;
                string legalReelId = _dgvReels.Rows[0].Cells[colIdx].Value?.ToString() ?? "REEL-001";
                string cipher = Barcode2DParser.EncodeReelId(legalReelId);
                _txtBarcodeScan.Text = _currentContext.MatNo + "{{{{" + _currentContext.BatchNo + "{88{{{" + cipher;
            }
        };

        _btnTestScanIllegalReel.Click += (s, e) =>
        {
            if (_currentContext != null)
            {
                string cipher = Barcode2DParser.EncodeReelId("REEL-WRONG-999");
                _txtBarcodeScan.Text = _currentContext.MatNo + "{{{{" + _currentContext.BatchNo + "{88{{{" + cipher;
            }
        };
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
            _currentContext = null;
            _txtMatNo.Text = string.Empty;
            _txtBatchNo.Text = string.Empty;
            _dgvReels.DataSource = null;

            Log($"工單初始化異常: {ex.Message}", Color.Red);
            SetStatusLight("INIT_ERROR", $"初始化失敗: {ex.Message}", Color.FromArgb(231, 76, 60));
            MessageBox.Show($"工單初始化失敗：\n\n{ex.Message}", "開工檢核異常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        if (woNo.Contains("FAIL", StringComparison.OrdinalIgnoreCase) || woNo.Contains("NOTFOUND", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"MES 查無工單 [{woNo}] 之基本資訊 (離線模擬查無工單)");
        }

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
