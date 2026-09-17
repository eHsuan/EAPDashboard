#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EAP.ReelCheck.Communication;
using EAP.ReelCheck.Configuration;
using EAP.ReelCheck.Models;
using EAP.ReelCheck.Parsers;
using EAP.ReelCheck.Service;
using EAPDashboard.Models;

namespace EAPDashboard;

public partial class EquipmentReelForm : Form
{
    private readonly EquipmentBase _equipment;
    private readonly ReelCheckSettings? _reelCheckSettings;
    private readonly MesSettings? _mesSettings;
    private ReelCheckService? _reelCheckService;
    private IReelCheckMesService? _mesService;
    private WorkOrderContext? _currentContext;

    public EquipmentReelForm(EquipmentBase equipment, ReelCheckSettings? reelCheckSettings = null, MesSettings? mesSettings = null)
    {
        InitializeComponent();
        _equipment = equipment ?? throw new ArgumentNullException(nameof(equipment));
        _reelCheckSettings = reelCheckSettings ?? LoadReelCheckSettingsFromConfig();
        _mesSettings = mesSettings ?? LoadMesSettingsFromConfig();

        this.Text = $"TP 防混料監控 - {_equipment.Id} ({_equipment.Name})";
        _txtMachineNo.Text = _equipment.Id;

        // 若機台在 EAP 已有最新工單資訊，自動帶入
        TryPreloadWorkOrderFromEquipment();

        AttachEventHandlers();
    }

    private static ReelCheckSettings? LoadReelCheckSettingsFromConfig()
    {
        try
        {
            string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (System.IO.File.Exists(configPath))
            {
                string json = System.IO.File.ReadAllText(configPath);
                var jObj = Newtonsoft.Json.Linq.JObject.Parse(json);
                var rObj = jObj["ReelCheckSettings"];
                if (rObj != null)
                {
                    return rObj.ToObject<ReelCheckSettings>();
                }
            }
        }
        catch { }
        return null;
    }

    private static MesSettings? LoadMesSettingsFromConfig()
    {
        try
        {
            string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (System.IO.File.Exists(configPath))
            {
                string json = System.IO.File.ReadAllText(configPath);
                var jObj = Newtonsoft.Json.Linq.JObject.Parse(json);
                var mesObj = jObj["MesSettings"];
                if (mesObj != null)
                {
                    return mesObj.ToObject<MesSettings>();
                }
            }
        }
        catch { }
        return null;
    }

    private void TryPreloadWorkOrderFromEquipment()
    {
        try
        {
            var totalCounts = _equipment.GetAllTotalCounts();
            if (totalCounts != null && totalCounts.Count > 0)
            {
                var latestWo = totalCounts.Values
                    .OrderByDescending(x => x.UpdatedTime)
                    .FirstOrDefault();

                if (latestWo != null && !string.IsNullOrWhiteSpace(latestWo.WorkOrder))
                {
                    _txtWoNo.Text = latestWo.WorkOrder;
                }
            }
        }
        catch
        {
            // 忽略非關鍵初始化錯誤
        }
    }

    private void AttachEventHandlers()
    {
        _btnInitWorkOrder.Click += async (s, e) => await InitializeWorkOrderAsync();
        _btnFinishWorkOrder.Click += async (s, e) => await ProcessFinishWorkOrderAsync();
        _btnSubmitScan.Click += async (s, e) => await ProcessScanAsync();

        _txtBarcodeScan.KeyDown += async (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await ProcessScanAsync();
            }
        };

        _btnTestScanMatNo.Click += (s, e) =>
        {
            if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.MatNo;
        };

        _btnTestScanBatchNo.Click += (s, e) =>
        {
            if (_currentContext != null) _txtBarcodeScan.Text = _currentContext.BatchNo;
        };

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
                var options = new ReelCheckOptions();
                if (_reelCheckSettings != null)
                {
                    if (!string.IsNullOrWhiteSpace(_reelCheckSettings.ServerUrl))
                    {
                        options.ServerUrl = _reelCheckSettings.ServerUrl;
                    }
                    if (!string.IsNullOrWhiteSpace(_reelCheckSettings.ClientUrl))
                    {
                        options.ClientUrl = _reelCheckSettings.ClientUrl;
                    }
                    if (_reelCheckSettings.TimeoutSeconds > 0)
                    {
                        options.TimeoutSeconds = _reelCheckSettings.TimeoutSeconds;
                    }
                }
                else if (_mesSettings != null)
                {
                    string targetUrl = _mesSettings.ActiveEnvironment == "Test"
                        ? _mesSettings.TestUrl
                        : _mesSettings.ProductionUrl;

                    if (!string.IsNullOrWhiteSpace(targetUrl))
                    {
                        options.ServerUrl = targetUrl;
                    }
                }

                _mesService = new WcfReelCheckMesService(options);
                Log($"模式: MES WCF 實體連線 ({options.ServerUrl})", Color.Yellow);
            }

            _reelCheckService = new ReelCheckService(_mesService);
            _reelCheckService.OnScanResult += (s, result) =>
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() => UpdateScanResultUI(result)));
                }
            };
            _reelCheckService.OnReelBlocked += (s, result) =>
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() => ShowBlockedAlert(result)));
                }
            };

            _currentContext = await _reelCheckService.InitializeWorkOrderAsync(woNo, machNo, userId);

            // 更新 UI 顯示
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
            _dgvReels.Rows.Clear();

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

    private async Task ProcessFinishWorkOrderAsync()
    {
        if (_reelCheckService == null || !_reelCheckService.IsInitialized)
        {
            MessageBox.Show("請先完成工單開工初始化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var dialogResult = MessageBox.Show(
            $"確定要結束工單 [{_currentContext?.WONo}] 生產作業嗎？\n系統將對最後一捲料捲執行防呆指標結算與過帳。",
            "確認工單結束",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (dialogResult != DialogResult.Yes) return;

        Log($"[FINISH] 正在執行工單結案 (WoFinish)...", Color.Plum);
        var result = await _reelCheckService.FinishWorkOrderAsync();
        RefreshReelsGrid();

        if (result.IsSuccess)
        {
            SetStatusLight("FINISHED 工單已結案", result.Message, Color.FromArgb(142, 68, 173));
            Log($"[FINISH OK] {result.Message}", Color.Plum);
            MessageBox.Show($"工單 [{_currentContext?.WONo}] 結案成功！\n\n{result.Message}", "結案完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            SetStatusLight($"LOCK 結案異常 [{result.ErrorCode}]", result.Message, Color.FromArgb(192, 57, 43));
            Log($"[FINISH NG] 結案失敗：[{result.ErrorCode}] {result.Message}", Color.OrangeRed);
            MessageBox.Show($"工單結案異常：\n\n代碼：{result.ErrorCode}\n原因：{result.Message}", "結案防呆阻斷", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
        string lockDetail = string.IsNullOrWhiteSpace(result.ProblemWoNo)
            ? "已觸發 MES 工單鎖定並通知現場停機！"
            : $"已觸發 MES 雙向工單鎖定！\n（包含當前工單與被混料之問題工單 [{result.ProblemWoNo}]）\n並即刻通知現場停機！";

        MessageBox.Show($"【防混料重大異常】\n\n錯誤代碼：{result.ErrorCode}\n原因：{result.Message}\n\n{lockDetail}", "重大混料警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
internal class MockOfflineMesService : IReelCheckMesService
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
            ConfirmInTime = DateTime.Now,
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

    public Task<WorkOrderStatusCheckResult> CheckWorkOrderStatusAsync(string woNo, string workCenterNo, DateTime initialInTime, System.Threading.CancellationToken ct = default)
    {
        if (woNo.Contains("NG-0201", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(WorkOrderStatusCheckResult.Ng("NG-0201", "工單狀態異動(NG-0201), 工單可能已被刪除 Confirm。"));
        if (woNo.Contains("NG-0202", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(WorkOrderStatusCheckResult.Ng("NG-0202", "工單狀態異動(NG-0202), 工單已被重新過帳"));
        if (woNo.Contains("NG-0203", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(WorkOrderStatusCheckResult.Ng("NG-0203", "工單狀態變動(NG-0203), 工單出站狀態改變: 工單已出站"));

        return Task.FromResult(WorkOrderStatusCheckResult.Ok());
    }

    public Task<string?> QueryWorkOrderByBatchNoAsync(string batchNo, System.Threading.CancellationToken ct = default)
    {
        if (batchNo.Contains("WRONG", StringComparison.OrdinalIgnoreCase) ||
            batchNo.Contains("OTHER", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult<string?>("WO2026098888");
        }
        return Task.FromResult<string?>(null);
    }

    public Task<bool> IsMaterialExistsAsync(string matNo, System.Threading.CancellationToken ct = default)
    {
        if (matNo.StartsWith("MAT-", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> FinishWorkOrderAsync(string woNo, ReelItem? lastReel, string machNo, string userId, System.Threading.CancellationToken ct = default)
    {
        return Task.FromResult(true);
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
    {
        if (barcode.Contains("WRONG", StringComparison.OrdinalIgnoreCase) ||
            barcode.Contains("OTHER", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult<string?>("WO2026099999");
        }
        return Task.FromResult<string?>(null);
    }

    public Task<bool> SavePackingAsync(string woNo, string batchNo, string matNo, string reelId, string userId, string workCenterNo, System.Threading.CancellationToken ct = default)
        => Task.FromResult(true);

    public Task<bool> PingAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(true);
}
