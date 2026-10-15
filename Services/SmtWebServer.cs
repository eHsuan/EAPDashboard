using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EAPDashboard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAPDashboard.Services
{
    /// <summary>
    /// SMT 全域單一輕量 Web 伺服器 (提供多產線狀態監控與人工強制結單 WebUI)
    /// </summary>
    public class SmtWebServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly SmtLineManager _lineManager;
        private readonly int _port;
        private CancellationTokenSource _cts;
        private bool _isRunning;

        public event Action<LogLevel, string> LogReceived;

        public SmtWebServer(SmtLineManager lineManager, int port = 8088)
        {
            _lineManager = lineManager ?? throw new ArgumentNullException(nameof(lineManager));
            _port = port > 0 ? port : 8088;
            _listener = new HttpListener();
        }

        public void Start()
        {
            if (_isRunning) return;

            try
            {
                _cts = new CancellationTokenSource();
                _listener.Prefixes.Clear();
                // 嘗試綁定萬用前綴與 localhost
                _listener.Prefixes.Add($"http://*:{_port}/");
                _listener.Start();
                _isRunning = true;
                OnLog(LogLevel.Info, $"[SMT WebServer] Started successfully on port {_port} (http://localhost:{_port}/)");
            }
            catch (HttpListenerException)
            {
                // 若未以管理員權限執行萬用前綴失敗，退回綁定 localhost
                try
                {
                    _listener.Prefixes.Clear();
                    _listener.Prefixes.Add($"http://localhost:{_port}/");
                    _listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
                    _listener.Start();
                    _isRunning = true;
                    OnLog(LogLevel.Info, $"[SMT WebServer] Started on localhost only (port {_port})");
                }
                catch (Exception ex)
                {
                    OnLog(LogLevel.Error, $"[SMT WebServer] Failed to start on port {_port}: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT WebServer] Failed to start on port {_port}: {ex.Message}");
                return;
            }

            Task.Run(() => ListenLoopAsync(_cts.Token));
        }

        public void Stop()
        {
            _isRunning = false;
            try
            {
                _cts?.Cancel();
                _listener?.Stop();
                OnLog(LogLevel.Info, "[SMT WebServer] Stopped.");
            }
            catch { }
        }

        public void Dispose()
        {
            Stop();
            try { _listener?.Close(); } catch { }
            _cts?.Dispose();
        }

        private async Task ListenLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && _isRunning)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = ProcessRequestAsync(context);
                }
                catch (HttpListenerException) when (!_isRunning) { break; }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (_isRunning)
                    {
                        OnLog(LogLevel.Warn, $"[SMT WebServer] Listener error: {ex.Message}");
                    }
                }
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;

            // 支援 CORS
            res.Headers.Add("Access-Control-Allow-Origin", "*");
            res.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            res.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            if (req.HttpMethod == "OPTIONS")
            {
                res.StatusCode = (int)HttpStatusCode.OK;
                res.Close();
                return;
            }

            try
            {
                string path = req.Url.AbsolutePath.TrimEnd('/');
                if (string.IsNullOrEmpty(path) || path.Equals("/index.html", StringComparison.OrdinalIgnoreCase))
                {
                    await ServeHtmlAsync(res);
                }
                else if (path.Equals("/api/smt/lines", StringComparison.OrdinalIgnoreCase) && req.HttpMethod == "GET")
                {
                    var statuses = _lineManager.GetAllLineStatuses();
                    await WriteJsonResponseAsync(res, statuses);
                }
                else if (path.Equals("/api/smt/force-trackout", StringComparison.OrdinalIgnoreCase) && req.HttpMethod == "POST")
                {
                    await HandleForceTrackOutAsync(req, res);
                }
                else
                {
                    res.StatusCode = (int)HttpStatusCode.NotFound;
                    byte[] notFound = Encoding.UTF8.GetBytes("Not Found");
                    res.ContentType = "text/plain; charset=utf-8";
                    res.ContentLength64 = notFound.Length;
                    await res.OutputStream.WriteAsync(notFound, 0, notFound.Length);
                    res.Close();
                }
            }
            catch (Exception ex)
            {
                OnLog(LogLevel.Error, $"[SMT WebServer] Error handling request: {ex.Message}");
                try
                {
                    res.StatusCode = (int)HttpStatusCode.InternalServerError;
                    byte[] err = Encoding.UTF8.GetBytes($"Server Error: {ex.Message}");
                    res.ContentType = "text/plain; charset=utf-8";
                    res.ContentLength64 = err.Length;
                    await res.OutputStream.WriteAsync(err, 0, err.Length);
                    res.Close();
                }
                catch { }
            }
        }

        private async Task HandleForceTrackOutAsync(HttpListenerRequest req, HttpListenerResponse res)
        {
            string body;
            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }

            string lineId = null;
            string station = null;
            string wo = null;
            string user = "WEB_USER";

            try
            {
                var jo = JObject.Parse(body);
                lineId = jo["lineId"]?.ToString();
                station = jo["station"]?.ToString() ?? "1";
                wo = jo["workOrder"]?.ToString();
                user = jo["userId"]?.ToString() ?? "WEB_USER";
            }
            catch
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteJsonResponseAsync(res, new { success = false, message = "無效的 JSON 請求內容" });
                return;
            }

            if (string.IsNullOrWhiteSpace(lineId) || string.IsNullOrWhiteSpace(wo))
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteJsonResponseAsync(res, new { success = false, message = "缺少必要參數 lineId 或 workOrder" });
                return;
            }

            var lineService = _lineManager.GetLine(lineId);
            if (lineService == null)
            {
                res.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteJsonResponseAsync(res, new { success = false, message = $"查無此 SMT 線別: {lineId}" });
                return;
            }

            var (ok, msg, qty) = await lineService.ForceTrackOutAsync(station, wo, user);
            await WriteJsonResponseAsync(res, new { success = ok, message = msg, actualQty = qty });
        }

        private static async Task WriteJsonResponseAsync(HttpListenerResponse res, object data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            res.ContentType = "application/json; charset=utf-8";
            res.ContentLength64 = bytes.Length;
            res.StatusCode = (int)HttpStatusCode.OK;
            await res.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            res.Close();
        }

        private static async Task ServeHtmlAsync(HttpListenerResponse res)
        {
            string html = GetEmbeddedHtml();
            byte[] bytes = Encoding.UTF8.GetBytes(html);
            res.ContentType = "text/html; charset=utf-8";
            res.ContentLength64 = bytes.Length;
            res.StatusCode = (int)HttpStatusCode.OK;
            await res.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            res.Close();
        }

        private void OnLog(LogLevel level, string msg)
        {
            LogReceived?.Invoke(level, msg);
            System.Diagnostics.Debug.WriteLine($"[{level}] {msg}");
        }

        private static string GetEmbeddedHtml()
        {
            return @"<!DOCTYPE html>
<html lang=""zh-TW"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>SMT 串線監控與出站管理中心</title>
    <style>
        :root {
            --bg-color: #1a1a1f;
            --card-bg: #26262e;
            --card-border: #3b3b47;
            --accent-blue: #3d7eff;
            --accent-green: #2ecc71;
            --accent-yellow: #f1c40f;
            --accent-red: #e74c3c;
            --text-main: #f0f0f5;
            --text-sub: #a0a0b0;
        }
        body {
            font-family: -apple-system, BlinkMacSystemFont, ""Microsoft JhengHei"", ""Segoe UI"", Roboto, sans-serif;
            background-color: var(--bg-color);
            color: var(--text-main);
            margin: 0;
            padding: 24px;
        }
        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 1px solid var(--card-border);
            padding-bottom: 16px;
            margin-bottom: 24px;
        }
        .header h1 {
            margin: 0;
            font-size: 24px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .badge {
            background-color: var(--accent-blue);
            color: white;
            font-size: 13px;
            padding: 3px 10px;
            border-radius: 12px;
            font-weight: normal;
        }
        .tabs {
            display: flex;
            gap: 8px;
            margin-bottom: 20px;
        }
        .tab-btn {
            background: #202028;
            border: 1px solid var(--card-border);
            color: var(--text-sub);
            padding: 10px 22px;
            border-radius: 8px;
            cursor: pointer;
            font-size: 15px;
            font-weight: 500;
            transition: all 0.2s;
        }
        .tab-btn:hover {
            color: white;
            border-color: var(--accent-blue);
        }
        .tab-btn.active {
            background: var(--accent-blue);
            color: white;
            border-color: var(--accent-blue);
        }
        .card {
            background-color: var(--card-bg);
            border: 1px solid var(--card-border);
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.25);
        }
        .card-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 16px;
        }
        .card-title {
            font-size: 18px;
            font-weight: 600;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }
        th, td {
            text-align: left;
            padding: 12px 14px;
            border-bottom: 1px solid #333340;
            font-size: 14px;
        }
        th {
            background-color: #212128;
            color: var(--text-sub);
            font-weight: 600;
        }
        tr:hover {
            background-color: #2c2c36;
        }
        .tag {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: 500;
            display: inline-block;
        }
        .tag-green { background: rgba(46, 204, 113, 0.2); color: #2ecc71; border: 1px solid #2ecc71; }
        .tag-yellow { background: rgba(241, 196, 15, 0.2); color: #f1c40f; border: 1px solid #f1c40f; }
        .tag-blue { background: rgba(61, 126, 255, 0.2); color: #3d7eff; border: 1px solid #3d7eff; }
        .btn-action {
            background-color: #e67e22;
            color: white;
            border: none;
            padding: 6px 14px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 13px;
            font-weight: bold;
            transition: opacity 0.2s;
        }
        .btn-action:hover {
            opacity: 0.85;
        }
        .btn-action:disabled {
            background-color: #555;
            cursor: not-allowed;
            opacity: 0.5;
        }
        .status-dot {
            height: 10px;
            width: 10px;
            border-radius: 50%;
            display: inline-block;
            margin-right: 6px;
        }
        .dot-green { background-color: var(--accent-green); }
        .dot-yellow { background-color: var(--accent-yellow); }
    </style>
</head>
<body>
    <div class=""header"">
        <h1>
            <span>🏭 SMT 串線雙站監控與強制結單中心</span>
            <span class=""badge"">EAP WebUI</span>
        </h1>
        <div>
            <span id=""lastUpdate"" style=""color: var(--text-sub); font-size: 13px;"">更新時間: 讀取中...</span>
        </div>
    </div>

    <!-- 線別頁籤 -->
    <div class=""tabs"" id=""lineTabs""></div>

    <!-- 工單動態列表 -->
    <div class=""card"">
        <div class=""card-header"">
            <div class=""card-title"" id=""currentLineTitle"">線別狀態載入中...</div>
            <div id=""lineMeta"" style=""color: var(--text-sub); font-size: 14px;""></div>
        </div>

        <table>
            <thead>
                <tr>
                    <th>工單號碼 (WO)</th>
                    <th>第一站 (Station 1) 刷入/刷出</th>
                    <th>第一站結單狀況</th>
                    <th>第二站 (Post AOI) 刷入/刷出</th>
                    <th>第二站結單門檻 (抽片防呆)</th>
                    <th>第二站結單狀況</th>
                    <th>人工操作</th>
                </tr>
            </thead>
            <tbody id=""woTableBody"">
                <tr><td colspan=""7"" style=""text-align:center; padding:30px;"">載入資料中...</td></tr>
            </tbody>
        </table>
    </div>

    <script>
        let allLines = [];
        let activeLineId = '';

        async function fetchStatuses() {
            try {
                const res = await fetch('/api/smt/lines');
                if (!res.ok) return;
                allLines = await res.json();
                
                document.getElementById('lastUpdate').innerText = '最後更新: ' + new Date().toLocaleTimeString();
                renderTabs();
                renderTable();
            } catch (err) {
                console.error(err);
            }
        }

        function renderTabs() {
            const tabsDiv = document.getElementById('lineTabs');
            tabsDiv.innerHTML = '';
            
            if (allLines.length === 0) {
                tabsDiv.innerHTML = '<span style=""color:var(--text-sub);"">目前無啟用的 SMT 線別設定</span>';
                return;
            }

            if (!activeLineId && allLines.length > 0) {
                activeLineId = allLines[0].LineId;
            }

            allLines.forEach(line => {
                const btn = document.createElement('button');
                btn.className = 'tab-btn' + (line.LineId === activeLineId ? ' active' : '');
                btn.innerText = (line.LineName || line.LineId) + (line.ActiveWorkOrders.length ? ' (' + line.ActiveWorkOrders.length + ')' : '');
                btn.onclick = () => {
                    activeLineId = line.LineId;
                    renderTabs();
                    renderTable();
                };
                tabsDiv.appendChild(btn);
            });
        }

        function renderTable() {
            const currentLine = allLines.find(l => l.LineId === activeLineId);
            const titleEl = document.getElementById('currentLineTitle');
            const metaEl = document.getElementById('lineMeta');
            const tbody = document.getElementById('woTableBody');

            if (!currentLine) {
                titleEl.innerText = '請選擇線別';
                metaEl.innerText = '';
                tbody.innerHTML = '<tr><td colspan=""7"" style=""text-align:center; padding:30px;"">請選擇有效線別</td></tr>';
                return;
            }

            titleEl.innerText = (currentLine.LineName || currentLine.LineId) + ' 即時生產在製狀態';
            metaEl.innerText = '滿批次上限: ' + currentLine.BatchCapacity + ' 片 | 站1當前單: ' + (currentLine.Station1_CurrentWO || '無') + ' | 站2當前單: ' + (currentLine.Station2_CurrentWO || '無');

            if (!currentLine.ActiveWorkOrders || currentLine.ActiveWorkOrders.length === 0) {
                tbody.innerHTML = '<tr><td colspan=""7"" style=""text-align:center; padding:30px; color:var(--text-sub);"">目前此線別無進行中之工單紀錄</td></tr>';
                return;
            }

            tbody.innerHTML = '';
            currentLine.ActiveWorkOrders.forEach(wo => {
                const tr = document.createElement('tr');

                // 門檻顯示
                let st2Target = '滿 ' + currentLine.BatchCapacity + ' 片';
                if (wo.Station1_Finished && wo.Station1_Actual_QTY != null) {
                    st2Target = '第一站已出 ' + wo.Station1_Actual_QTY + ' 片 (抽片結單門檻)';
                }

                // 站1狀態
                let st1FinHtml = wo.Station1_Finished 
                    ? '<span class=""tag tag-green"">已結單 (' + (wo.Station1_Actual_QTY != null ? wo.Station1_Actual_QTY : wo.Station1_Out_Count) + ' 片)</span>'
                    : '<span class=""tag tag-yellow"">進行中</span>';

                // 站2狀態
                let st2FinHtml = wo.Station2_Finished
                    ? '<span class=""tag tag-green"">已結單 (' + (wo.Station2_Actual_QTY != null ? wo.Station2_Actual_QTY : wo.Station2_Out_Count) + ' 片)</span>'
                    : '<span class=""tag tag-yellow"">進行中</span>';

                tr.innerHTML = `
                    <td><strong>${wo.WorkOrder}</strong></td>
                    <td>In: ${wo.Station1_In_Count} / <strong>Out: ${wo.Station1_Out_Count}</strong></td>
                    <td>${st1FinHtml}</td>
                    <td>In: ${wo.Station2_In_Count} / <strong>Out: ${wo.Station2_Out_Count}</strong></td>
                    <td><span class=""tag tag-blue"">${st2Target}</span></td>
                    <td>${st2FinHtml}</td>
                    <td>
                        <button class=""btn-action"" ${wo.Station1_Finished ? 'disabled' : ''} onclick=""forceTrackOut('${currentLine.LineId}', 'Station1', '${wo.WorkOrder}', ${wo.Station1_Out_Count})"">第一站結單</button>
                        <button class=""btn-action"" style=""background-color:#c0392b;"" ${wo.Station2_Finished ? 'disabled' : ''} onclick=""forceTrackOut('${currentLine.LineId}', 'Station2', '${wo.WorkOrder}', ${wo.Station2_Out_Count})"">第二站結單</button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        }

        async function forceTrackOut(lineId, station, workOrder, currentQty) {
            const stName = station === 'Station1' ? '第一站' : '第二站 (Post AOI)';
            if (!confirm(`確定要針對線別 [${lineId}] 工單 [${workOrder}] 在【${stName}】執行強制結單出站嗎？\n(目前累積出站片數: ${currentQty})`)) {
                return;
            }

            try {
                const res = await fetch('/api/smt/force-trackout', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        lineId: lineId,
                        station: station,
                        workOrder: workOrder,
                        userId: 'WEB_USER'
                    })
                });
                const result = await res.json();
                alert(result.message);
                fetchStatuses();
            } catch (err) {
                alert('操作失敗: ' + err.message);
            }
        }

        // 初始化與定期更新
        fetchStatuses();
        setInterval(fetchStatuses, 2500);
    </script>
</body>
</html>";
        }
    }
}
