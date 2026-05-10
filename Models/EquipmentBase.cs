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

    public abstract class EquipmentBase : INotifyPropertyChanged
    {
        private ConnectionStatus _connectionStatus;
        private EquipmentState _state;
        private string _currentRecipe;
        private Dictionary<string, string> _alarmMappings = new Dictionary<string, string>();
        private static readonly object _logLock = new object();
        
        protected string _lastOperator = "UNKNOWN";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public bool IsSimulator { get; set; }
        public bool IsMesSimulator { get; set; } // 新增：判定 MES 是否為模擬器
        public DbSettings DbConfig { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DateTime, LogLevel, string> LogReceived;

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
                var reply = await Task.Run(() => MESMessageFactory.CallMes<UserVerifyRequest, UserVerifyReply>(req));
                
                string resPrefix = (reply.Result == "success") ? "PASS" : "FAIL";
                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}|{reply.UserID}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Verify Failed: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackInAsync(string cassetteId, string workOrder, string userId, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string wo = workOrder;
                if (string.IsNullOrEmpty(wo) && !string.IsNullOrEmpty(cassetteId))
                {
                    OnLogReceived(LogLevel.Info, $"[Business] {eid} Querying WO for Cassette {cassetteId}");
                    wo = QueryWorkOrderFromDb(cassetteId);
                }

                if (string.IsNullOrEmpty(wo)) return "FAIL|W001|Cassette Not Bound to WO|";

                var panelList = QueryPanelListFromDb(cassetteId);
                string panelStr = Newtonsoft.Json.JsonConvert.SerializeObject(panelList);

                var qryReq = new WOQRYRequest { WONO = wo, EqpNo = eid, UserID = userId };
                var qryReply = await Task.Run(() => MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq));
                
                if (qryReply.Result != "success") 
                    return $"FAIL|{qryReply.ResultCode}|MES WOQRY Failed: {qryReply.Message}|{panelStr}";

                string recipe = ExtractRecipeFromMes(qryReply.InputParamListDT);
                return $"PP-SELECT|{wo}|{recipe}|{qryReply.ResultCode}|{qryReply.Message}|{qryReply.InputParamListDT}|{panelStr}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|TrackIn Error: {ex.Message}|"; }
        }

        protected List<string> QueryPanelListFromDb(string cassetteId)
        {
            var list = new List<string>();
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return list;
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT PanelID FROM EAP_PanelBind WHERE CassetteID = @cid ORDER BY SlotNo ASC";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) list.Add(reader["PanelID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"Query Cloud PanelList Failed: {ex.Message}"); }
            return list;
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
                var reply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(req));
                
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
                var reply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(req));
                
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

        protected async Task<bool> UpdateBindingInDbAsync(string cassetteId, string workOrder, List<string> panels)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return false;
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString))
                {
                    await conn.OpenAsync();
                    using (var tx = conn.BeginTransaction())
                    {
                        try
                        {
                            string delSql = "DELETE FROM EAP_CassetteBind WHERE CassetteID = @cid";
                            using (var cmdDel = new System.Data.SqlClient.SqlCommand(delSql, conn, tx))
                            {
                                cmdDel.Parameters.AddWithValue("@cid", cassetteId);
                                await cmdDel.ExecuteNonQueryAsync();
                            }

                            string insMasterSql = "INSERT INTO EAP_CassetteBind (CassetteID, WorkOrder, BindTime) VALUES (@cid, @wo, GETDATE())";
                            using (var cmdInsM = new System.Data.SqlClient.SqlCommand(insMasterSql, conn, tx))
                            {
                                cmdInsM.Parameters.AddWithValue("@cid", cassetteId);
                                cmdInsM.Parameters.AddWithValue("@wo", workOrder);
                                await cmdInsM.ExecuteNonQueryAsync();
                            }

                            for (int i = 0; i < panels.Count; i++)
                            {
                                string pid = panels[i];
                                if (string.IsNullOrEmpty(pid)) continue;

                                string insDetailSql = "INSERT INTO EAP_PanelBind (PanelID, CassetteID, SlotNo, BindTime) VALUES (@pid, @cid, @slot, GETDATE())";
                                using (var cmdInsD = new System.Data.SqlClient.SqlCommand(insDetailSql, conn, tx))
                                {
                                    cmdInsD.Parameters.AddWithValue("@pid", pid);
                                    cmdInsD.Parameters.AddWithValue("@cid", cassetteId);
                                    cmdInsD.Parameters.AddWithValue("@slot", i + 1);
                                    await cmdInsD.ExecuteNonQueryAsync();
                                }
                            }

                            tx.Commit();
                            OnLogReceived(LogLevel.Info, $"[DB] {this.Id} Successfully bound Cassette {cassetteId} to WO {workOrder} with {panels.Count} panels.");
                            return true;
                        }
                        catch { tx.Rollback(); throw; }
                    }
                }
            }
            catch { return false; }
        }

        protected async Task<bool> UnbindFromDbAsync(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return false;
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString))
                {
                    await conn.OpenAsync();
                    string sql = "DELETE FROM EAP_CassetteBind WHERE CassetteID = @cid";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                OnLogReceived(LogLevel.Info, $"[DB] {this.Id} Successfully unbound Cassette {cassetteId}.");
                return true;
            }
            catch { return false; }
        }

        private async Task SavePendingBindingAsync(string cid, string wo, List<string> panels)
        {
            try {
                using (var conn = DatabaseHelper.GetConnection()) {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO Local_Binding_Buffer (CassetteID, WorkOrder, PanelList) VALUES (@cid, @wo, @json)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
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
                    string sql = "INSERT INTO Local_Unbinding_Buffer (CassetteID) VALUES (@cid)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
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
                    var pendings = DatabaseHelper.GetPendingBindings();
                    foreach (var p in pendings) {
                        var panels = JsonConvert.DeserializeObject<List<string>>(p.PanelList);
                        if (await UpdateBindingInDbAsync(p.CassetteID, p.WorkOrder, panels)) DatabaseHelper.DeletePendingBinding(p.Id);
                    }
                    var unpendings = DatabaseHelper.GetPendingUnbindings();
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
                OnLogReceived(LogLevel.Info, $"[STATUS] {eid} Status Changed: {oldStatus} -> {newStatus}");

                if (int.TryParse(newStatus, out int sc))
                {
                    if (sc == 0 || sc == 6) State = EquipmentState.Running;
                    else if (sc == 1 || sc == 10) State = EquipmentState.Alarm;
                    else State = EquipmentState.Paused;
                }

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = newStatus };
                var reply = await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Status Update Failed: {ex.Message}"; }
        }

        public async Task<string> ProcessPanelInAsync(string panelId, string cassetteId, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, $"[PanelIn] {eid} - Panel: {panelId}, Cassette: {cassetteId}");
            await SaveProductionDataAsync("PanelIn", "", new { PanelID = panelId, CassetteID = cassetteId, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessPanelOutAsync(string panelId, string cassetteId, string result, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, $"[PanelOut] {eid} - Panel: {panelId}, Result: {result}");
            await SaveProductionDataAsync("PanelOut", "", new { PanelID = panelId, CassetteID = cassetteId, Result = result, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessKeyParametersReportAsync(List<Protocol.Core.Messages.ParameterResultItem> parameters, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, $"[KPIV/KPOV] {eid} KPI Report: {parameters.Count} records");
            await SaveProductionDataAsync("KeyParametersReport", "", new { EqpID = eid, ProcessData = parameters });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessAlarmAsync(string alarmCode, bool isSet, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string mc = TransformAlarmCode(alarmCode);
                OnLogReceived(LogLevel.Warn, $"[ALARM] {eid} Alarm Report: Code={alarmCode} (Mapped={mc}), Set={isSet}");

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = isSet ? "1" : "2", ErrorCode = mc };
                var reply = await Task.Run(() => MESMessageFactory.CallMes<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) 
            { 
                OnLogReceived(LogLevel.Error, $"Alarm Report Exception: {ex.Message}");
                return $"ERROR|UNKNOWN|{ex.Message}";
            }
        }

        public async Task SaveProductionDataAsync(string eventName, string workOrder, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            DateTime now = DateTime.Now;
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
            try {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataLogs", now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string filePath = Path.Combine(dir, $"{this.Id}.csv");
                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8)) {
                    sw.WriteLine($"{now:yyyy-MM-dd HH:mm:ss},{eventName},{workOrder ?? ""},\"{json.Replace("\"", "\"\"")}\"");
                }
            } catch { }
        }

        protected string QueryWorkOrderFromDb(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return null;
            try {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString)) {
                    conn.Open();
                    string sql = "SELECT TOP 1 WorkOrder FROM EAP_CassetteBind WHERE CassetteID = @cid";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        return cmd.ExecuteScalar()?.ToString();
                    }
                }
            } catch { return null; }
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
                    string filePath = Path.Combine(logDir, $"{timestamp:HH}.txt");
                    string evtPart = string.IsNullOrEmpty(eventName) ? "" : $" [{eventName}]";
                    File.AppendAllText(filePath, $"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{level}]{evtPart} : {message}{Environment.NewLine}");
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
