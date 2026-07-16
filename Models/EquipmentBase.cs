using CyntecMqttLib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Protocol.MES.Factory;
using Protocol.MES.Messages;
using Protocol.MES.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System.Net.Http;

namespace EAPDashboard.Models
{
    public enum LogLevel { Debug, Info, Warn, Error, Fatal }
    public enum ConnectionStatus { Disconnected, Connecting, Connected }
    public enum EquipmentState 
    { 
        [Description("RUNNING")] Running, 
        [Description("ALARM")] Alarm, 
        [Description("PAUSED")] Paused, 
        [Description("IDLE")] Idle, 
        [Description("DOWN")] Down 
    }

    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string BindingTable { get; set; }
        public string CassetteColumn { get; set; }
        public string WoColumn { get; set; }
    }

    public class GatewayConfig 
    { 
        public string PrimaryUrl { get; set; } 
        public string SecondaryUrl { get; set; } 
    }

    public class BindingInfo
    {
        public string WorkOrder { get; set; }
        public List<string> Panels { get; set; } = new List<string>();
    }

    public abstract class EquipmentBase : INotifyPropertyChanged
    {
        private ConnectionStatus _connectionStatus;
        private EquipmentState _state;
        private string _currentRecipe;
        private Dictionary<string, string> _alarmMappings = new Dictionary<string, string>();
        private static readonly object _logLock = new object();
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;
        
        protected string _lastOperator = "UNKNOWN";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public bool IsSimulator { get; set; }
        public bool IsMesSimulator { get; set; } 
        public GatewayConfig GatewayConfig { get; set; }
        public DbSettings DbConfig { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DateTime, LogLevel, string> LogReceived;

        public EquipmentBase()
        {
            var fallbackPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<TaskCanceledException>()
                .FallbackAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    OnLogReceived(LogLevel.Warn, "[GATEWAY] Resilience triggered: switching to backup logic.");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
                }));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

            _resiliencePolicy = Policy.WrapAsync<HttpResponseMessage>(fallbackPolicy, timeoutPolicy);
        }

        public void LoadAlarmMappings()
        {
            try
            {
                _alarmMappings.Clear();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT RawCode, MappedCode FROM Alarm_Mapping WHERE EqpId = @eqid";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _alarmMappings[reader["RawCode"].ToString()] = reader["MappedCode"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"Load Alarm Mapping Failed: {ex.Message}"); }
        }

        protected string TransformAlarmCode(string rawCode)
        {
            if (string.IsNullOrEmpty(rawCode)) return rawCode;
            return _alarmMappings.TryGetValue(rawCode, out string mapped) ? mapped : rawCode;
        }

        // --- Core Business Logic ---

        public async Task<string> ProcessUserVerifyAsync(string barcode, string userId, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                OnLogReceived(LogLevel.Info, $"[Business] {eid} User Verify: Barcode={barcode}, UserID={userId}");
                var req = new UserVerifyRequest { EqpNo = eid, User2DBarCode = barcode, UserID = userId };
                
                var replyTask = Task.Run(() => MESMessageFactory.CallMes<UserVerifyRequest, UserVerifyReply>(req));
                if (await Task.WhenAny(replyTask, Task.Delay(25000)) != replyTask)
                    return "FAIL|T001|MES Call Timeout (25s)|";

                var reply = await replyTask;
                string resPrefix = (reply.Result == "success") ? "PASS" : "FAIL";
                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}|{reply.UserID}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Verify Failed: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackInAsync(string cassetteId, string workOrder, List<string> panels, string userId, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string wo = workOrder;
                List<string> currentPanels = panels ?? new List<string>();

                if (string.IsNullOrEmpty(wo)) return "FAIL|W001|Cassette Not Bound to WO|[]";

                var qryReq = new WOQRYRequest { WONO = wo, EqpNo = eid, UserID = userId };
                var qryTask = Task.Run(() => MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq));
                if (await Task.WhenAny(qryTask, Task.Delay(25000)) != qryTask)
                    return "FAIL|T001|MES WOQRY Timeout (25s)|[]";

                var qryReply = await qryTask;
                if (qryReply.Result != "success") 
                    return $"FAIL|{qryReply.ResultCode}|MES WOQRY Failed: {qryReply.Message}|[]";

                string recipe = ExtractRecipeFromMes(qryReply.InputParamListDT);
                string panelsJson = JsonConvert.SerializeObject(currentPanels);
                return $"PP-SELECT|{wo}|{recipe}|{qryReply.ResultCode}|{qryReply.Message}|{qryReply.InputParamListDT}|{panelsJson}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|TrackIn Error: {ex.Message}|[]"; }
        }

        protected string GetMesParamValue(string inputParamListDt, string paramName)
        {
            if (string.IsNullOrEmpty(inputParamListDt) || string.IsNullOrEmpty(paramName)) return null;
            try
            {
                var list = JsonConvert.DeserializeObject<List<JObject>>(inputParamListDt);
                var paramObj = list.FirstOrDefault(x => x["ParamName"]?.ToString().Equals(paramName, StringComparison.OrdinalIgnoreCase) == true 
                                                    || x["ParamCode"]?.ToString().Equals(paramName, StringComparison.OrdinalIgnoreCase) == true);
                return paramObj?["DefaultValue"]?.ToString();
            }
            catch { return null; }
        }

        public async Task<string> ProcessTrackInFinalAsync(string workOrder, string userId, string cassetteId, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                var req = new WOCHECKINRequest { WONO = workOrder, EqpNo = eid, UserID = userId };
                
                var checkinTask = Task.Run(() => MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(req));
                if (await Task.WhenAny(checkinTask, Task.Delay(25000)) != checkinTask)
                    return "FAIL|T001|MES WOCHECKIN Timeout (25s)|";

                var reply = await checkinTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                
                if (resPrefix == "PASS" && !string.IsNullOrEmpty(cassetteId))
                {
                    bool ok = await UnbindFromDbAsync(cassetteId);
                    if (!ok) await SavePendingUnbindingAsync(cassetteId);
                }

                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Report Failed: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackOutAsync(string workOrder, string qty, string userId, string cassetteId, List<string> panels, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                OnLogReceived(LogLevel.Info, $"[WOCHECKOUT] {eid} - WO: {workOrder}, Qty: {qty}, CID: {cassetteId}");
                
                var req = new WOCHECKOUTRequest { WONO = workOrder, EqpNo = eid, UserID = userId, OutPut = qty };
                
                var checkoutTask = Task.Run(() => MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(req));
                if (await Task.WhenAny(checkoutTask, Task.Delay(25000)) != checkoutTask)
                    return "FAIL|T001|MES WOCHECKOUT Timeout (25s)|";

                var reply = await checkoutTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                
                if (resPrefix == "PASS")
                {
                    bool dbOk = await UpdateBindingInDbAsync(cassetteId, workOrder, panels);
                    if (!dbOk) await SavePendingBindingAsync(cassetteId, workOrder, panels);
                }

                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|TrackOut Failed: {ex.Message}"; }
        }

        protected BindingInfo QueryBindingFromGateway(string cassetteId)
        {
            if (GatewayConfig == null) return null;
            try
            {
                var queryPolicy = Policy<HttpResponseMessage>
                    .Handle<Exception>()
                    .FallbackAsync(async (ct) => {
                        OnLogReceived(LogLevel.Warn, "[GATEWAY] Query Primary failed, trying Secondary...");
                        return await _httpClient.GetAsync(string.Format("{0}/api/binding/query/{1}/{2}", GatewayConfig.SecondaryUrl, this.Id, cassetteId), ct);
                    });

                return Task.Run(async () => {
                    var resp = await queryPolicy.ExecuteAsync(async (ct) => {
                        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                        {
                            return await _httpClient.GetAsync(string.Format("{0}/api/binding/query/{1}/{2}", GatewayConfig.PrimaryUrl, this.Id, cassetteId), cts.Token);
                        }
                    }, CancellationToken.None);

                    if (resp.IsSuccessStatusCode) 
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BindingInfo>(json);
                    }
                    return null;
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex) 
            { 
                OnLogReceived(LogLevel.Error, string.Format("[GATEWAY] Binding Query Failed Final: {0}", ex.Message));
                return null; 
            }
        }

        protected async Task<bool> UpdateBindingInDbAsync(string cassetteId, string workOrder, List<string> panels)
        {
            if (GatewayConfig == null) return false;
            try
            {
                var req = new { EqpId = this.Id, CassetteId = cassetteId, WorkOrder = workOrder, Panels = panels };
                var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                var resp = await _resiliencePolicy.ExecuteAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    try {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/bind", GatewayConfig.PrimaryUrl), content, ct);
                    } catch {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/bind", GatewayConfig.SecondaryUrl), content, ct);
                    }
                }), CancellationToken.None);

                if (resp.IsSuccessStatusCode) {
                    OnLogReceived(LogLevel.Info, string.Format("[DB] {0} Successfully bound Cassette {1} to WO {2} via Gateway.", this.Id, cassetteId, workOrder));
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        protected async Task<bool> UnbindFromDbAsync(string cassetteId)
        {
            if (GatewayConfig == null) return false;
            try
            {
                var req = new { EqpId = this.Id, CassetteId = cassetteId };
                var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                var resp = await _resiliencePolicy.ExecuteAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    try {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/unbind", GatewayConfig.PrimaryUrl), content, ct);
                    } catch {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/unbind", GatewayConfig.SecondaryUrl), content, ct);
                    }
                }), CancellationToken.None);

                if (resp.IsSuccessStatusCode) {
                    OnLogReceived(LogLevel.Info, string.Format("[DB] {0} Successfully unbound Cassette {1} via Gateway.", this.Id, cassetteId));
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        private async Task SavePendingBindingAsync(string cid, string wo, List<string> panels)
        {
            try {
                using (var conn = DatabaseHelper.GetConnection()) {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO Local_Binding_Buffer (EqpId, CassetteID, WorkOrder, PanelList) VALUES (@eqid, @cid, @wo, @json)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
                        cmd.Parameters.AddWithValue("@cid", cid); cmd.Parameters.AddWithValue("@wo", wo);
                        cmd.Parameters.AddWithValue("@json", JsonConvert.SerializeObject(panels));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            } catch { }
        }

        private async Task SavePendingUnbindingAsync(string cid)
        {
            try {
                using (var conn = DatabaseHelper.GetConnection()) {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO Local_Unbinding_Buffer (EqpId, CassetteID) VALUES (@eqid, @cid)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
                        cmd.Parameters.AddWithValue("@cid", cid);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            } catch { }
        }

        protected async Task StartBindingRetryProcessor(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var pendings = DatabaseHelper.GetPendingBindings(this.Id);
                    foreach (var p in pendings) {
                        var panels = JsonConvert.DeserializeObject<List<string>>(p.PanelList);
                        if (await UpdateBindingInDbAsync(p.CassetteID, p.WorkOrder, panels)) DatabaseHelper.DeletePendingBinding(p.Id);
                    }
                    var unpendings = DatabaseHelper.GetPendingUnbindings(this.Id);
                    foreach (var cid in unpendings) {
                        if (await UnbindFromDbAsync(cid)) DatabaseHelper.DeletePendingUnbinding(cid);
                    }
                }
                catch { }
                await Task.Delay(30000, ct);
            }
        }

        public async Task<string> ProcessStatusChangeAsync(string oldStatus, string newStatus, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                OnLogReceived(LogLevel.Info, string.Format("[STATUS] {0} Status Changed: {1} -> {2}", eid, oldStatus, newStatus));

                if (int.TryParse(newStatus, out int sc))
                {
                    if (sc == 0 || sc == 6) State = EquipmentState.Running;
                    else if (sc == 1 || sc == 10) State = EquipmentState.Alarm;
                    else State = EquipmentState.Paused;
                }

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = newStatus };
                
                var statusTask = Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                if (await Task.WhenAny(statusTask, Task.Delay(25000)) != statusTask)
                    return "FAIL|T001|MES EQPSTATUS Timeout (25s)";

                var reply = await statusTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                return string.Format("{0}|{1}|{2}", resPrefix, reply.ResultCode, reply.Message);
            }
            catch (Exception ex) { return string.Format("ERROR|UNKNOWN|Status Update Failed: {0}", ex.Message); }
        }

        public async Task<string> ProcessPanelInAsync(string panelId, string cassetteId, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, "Panel_In", string.Format("[PLC] {0} - Panel: {1}, Cassette: {2}", eid, panelId, cassetteId));
            await SaveProductionDataAsync("Panel_In", "", new { PanelID = panelId, CassetteID = cassetteId, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessPanelOutAsync(string panelId, string cassetteId, string result, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, "Panel_Out", string.Format("[PLC] {0} - Panel: {1}, Result: {2}", eid, panelId, result));
            await SaveProductionDataAsync("Panel_Out", "", new { PanelID = panelId, CassetteID = cassetteId, Result = result, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessKeyParametersReportAsync(List<Protocol.Core.Messages.ParameterResultItem> parameters, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, string.Format("[KPIV/KPOV] {0} KPI Report: {1} records", eid, parameters.Count));
            await SaveProductionDataAsync("KeyParametersReport", "", new { EqpID = eid, ProcessData = parameters });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessAlarmAsync(string alarmCode, bool isSet, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string mc = TransformAlarmCode(alarmCode);
                OnLogReceived(LogLevel.Warn, string.Format("[ALARM] {0} Alarm Report: Code={1} (Mapped={2}), Set={3}", eid, alarmCode, mc, isSet));

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = isSet ? "1" : "2", ErrorCode = mc };
                
                var alarmTask = Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                if (await Task.WhenAny(alarmTask, Task.Delay(25000)) != alarmTask)
                    return "FAIL|T001|MES ALARM Timeout (25s)";

                var reply = await alarmTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                return string.Format("{0}|{1}|{2}", resPrefix, reply.ResultCode, reply.Message);
            }
            catch (Exception ex) 
            { 
                OnLogReceived(LogLevel.Error, string.Format("Alarm Report Exception: {0}", ex.Message));
                return string.Format("ERROR|UNKNOWN|{0}", ex.Message);
            }
        }

        public async Task SaveProductionDataAsync(string eventName, string workOrder, object data, bool saveToDb = true)
        {
            string json = JsonConvert.SerializeObject(data);
            DateTime now = DateTime.Now;
            if (saveToDb)
            {
                try {
                    using (var conn = DatabaseHelper.GetConnection()) {
                        await conn.OpenAsync();
                        string sql = "INSERT INTO Production_Logs (EqpId, EventName, WorkOrder, DataJson) VALUES (@eqid, @evt, @wo, @json)";
                        using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
                            cmd.Parameters.AddWithValue("@eqid", this.Id); cmd.Parameters.AddWithValue("@evt", eventName);
                            cmd.Parameters.AddWithValue("@wo", workOrder ?? ""); cmd.Parameters.AddWithValue("@json", json);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                } catch { }
            }
            try {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataLogs", now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir)) Directory.Exists(dir);
                string filePath = Path.Combine(dir, string.Format("{0}.csv", this.Id));
                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8)) {
                    sw.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss},{1},{2},\"{3}\"", now, eventName, workOrder ?? "", json.Replace("\"", "\"\"")));
                }
            } catch { }
        }

        protected string ExtractRecipeFromMes(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try {
                var list = JsonConvert.DeserializeObject<List<JObject>>(inputParamListDt);
                var recipeObj = list.FirstOrDefault(x => x["ParamName"]?.ToString() == "RECIPEID" || x["RefFieldCode"]?.ToString() == "RECIPE ID");
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            } catch { return "ERROR"; }
        }

        public ConnectionStatus Connection { get => _connectionStatus; set { if (_connectionStatus != value) { _connectionStatus = value; OnPropertyChanged(nameof(Connection)); } } }
        public EquipmentState State { get => _state; set { if (_state != value) { _state = value; OnPropertyChanged(nameof(State)); } } }
        public string CurrentRecipe { get => _currentRecipe; set { if (_currentRecipe != value) { _currentRecipe = value; OnPropertyChanged(nameof(CurrentRecipe)); } } }

        // 安全重啟相關屬性
        public bool IsStopping { get; set; }
        private int _activeTaskCount = 0;
        public int ActiveTaskCount { get { return _activeTaskCount; } }

        public void IncrementTask() { Interlocked.Increment(ref _activeTaskCount); }
        public void DecrementTask() { Interlocked.Decrement(ref _activeTaskCount); }

        protected virtual void OnLogReceived(string message) => OnLogReceived(LogLevel.Info, null, message);
        protected virtual void OnLogReceived(LogLevel level, string message) => OnLogReceived(level, null, message);
        protected virtual void OnLogReceived(LogLevel level, string eventName, string message)
        {
            var timestamp = DateTime.Now;
            LogReceived?.Invoke(timestamp, level, message);
            try {
                lock (_logLock) {
                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", timestamp.ToString("yyyyMMdd"), this.Id ?? "System");
                    if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                    string filePath = Path.Combine(logDir, string.Format("{0:HH}.txt", timestamp));
                    string evtPart = string.IsNullOrEmpty(eventName) ? "" : string.Format(" [{0}]", eventName);
                    File.AppendAllText(filePath, string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}]{2} : {3}{4}", timestamp, level, evtPart, message, Environment.NewLine));
                }
            } catch { }
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public abstract void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30);
        public abstract Task SendTimeCalibration();
        public virtual void Stop() { }
        public static Color GetColorForState(EquipmentState state) { switch (state) { case EquipmentState.Running: return Color.LimeGreen; case EquipmentState.Alarm: case EquipmentState.Down: return Color.Crimson; case EquipmentState.Paused: case EquipmentState.Idle: return Color.Gold; default: return Color.Black; } }
    }
}
