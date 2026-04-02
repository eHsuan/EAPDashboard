using CyntecMqttLib.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
        
        protected string _lastOperator = "UNKNOWN";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
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
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"載入 Alarm Mapping 失敗: {ex.Message}"); }
        }

        protected string TransformAlarmCode(string rawCode)
        {
            if (string.IsNullOrEmpty(rawCode)) return rawCode;
            return _alarmMappings.TryGetValue(rawCode, out string mapped) ? mapped : rawCode;
        }

        // --- 核心業務邏輯 (V1.0.3 標準流程) ---

        public async Task<string> ProcessUserVerifyAsync(string barcode)
        {
            try
            {
                OnLogReceived(LogLevel.Info, $"[業務] 執行人員驗證: {barcode}");
                var req = new UserVerifyRequest { EqpNo = Id, User2DBarCode = barcode };
                var reply = await Task.Run(() => MESMessageFactory.CallMes<UserVerifyRequest, UserVerifyReply>(req));
                
                if (reply.Result == "success")
                {
                    _lastOperator = reply.UserID;
                    return $"PASS|{reply.UserID}|歡迎登入, {reply.UserName}";
                }
                return $"FAIL||{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR||驗證失敗: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackInAsync(string cassetteId, string workOrder)
        {
            try
            {
                string wo = workOrder;
                if (string.IsNullOrEmpty(wo) && !string.IsNullOrEmpty(cassetteId))
                {
                    OnLogReceived(LogLevel.Info, $"[業務] 查詢載具 {cassetteId} 綁定工單...");
                    wo = QueryWorkOrderFromDb(cassetteId);
                }

                if (string.IsNullOrEmpty(wo)) return "FAIL|載具未綁定工單";

                // 1. WOQRY
                var qryReq = new WOQRYRequest { WONO = wo, EqpNo = Id, UserID = _lastOperator };
                var qryReply = await Task.Run(() => MESMessageFactory.CallMes<WOQRYRequest, WOQRYReply>(qryReq));
                if (qryReply.Result != "success") return $"FAIL|MES WOQRY 失敗: {qryReply.Message}";

                string recipe = ExtractRecipeFromMes(qryReply.InputParamListDT);
                
                // 2. PP-SELECT (回傳 RecipeID 讓子類別執行機台下換)
                return $"PP-SELECT|{wo}|{recipe}";
            }
            catch (Exception ex) { return $"ERROR|進站流程異常: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackInFinalAsync(string workOrder)
        {
            try
            {
                var req = new WOCHECKINRequest { WONO = workOrder, EqpNo = Id, UserID = _lastOperator };
                var reply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKINRequest, WOCHECKINReply>(req));
                return reply.Result == "success" || reply.Result == "PASS" ? "PASS|" + reply.Message : $"FAIL|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|報工失敗: {ex.Message}"; }
        }

        public async Task<string> ProcessTrackOutAsync(string workOrder, string qty)
        {
            try
            {
                var req = new WOCHECKOUTRequest { WONO = workOrder, EqpNo = Id, UserID = _lastOperator, OutPut = qty };
                var reply = await Task.Run(() => MESMessageFactory.CallMes<WOCHECKOUTRequest, WOCHECKOUTReply>(req));
                return reply.Result == "success" || reply.Result == "PASS" ? "PASS|" + reply.Message : $"FAIL|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|出站失敗: {ex.Message}"; }
        }

        // --- 本地數據持久化 (SQLite + CSV 雙軌) ---

        public async Task SaveProductionDataAsync(string eventName, string workOrder, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            DateTime now = DateTime.Now;

            // 1. 存入 SQLite
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO Production_Logs (EqpId, EventName, WorkOrder, DataJson) VALUES (@eqid, @evt, @wo, @json)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
                        cmd.Parameters.AddWithValue("@evt", eventName);
                        cmd.Parameters.AddWithValue("@wo", workOrder ?? "");
                        cmd.Parameters.AddWithValue("@json", json);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"SQLite 數據存檔失敗: {ex.Message}"); }

            // 2. 存入 CSV 備份
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataLogs", now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string filePath = Path.Combine(dir, $"{this.Id}.csv");
                bool exists = File.Exists(filePath);

                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    if (!exists) sw.WriteLine("Timestamp,Event,WorkOrder,DataPayload");
                    
                    // 為了 CSV 格式，將 JSON 中的逗號替換掉或用引號包覆
                    string escapedJson = $"\"{json.Replace("\"", "\"\"")}\"";
                    sw.WriteLine($"{now:yyyy-MM-dd HH:mm:ss},{eventName},{workOrder ?? ""},{escapedJson}");
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"CSV 數據備份失敗: {ex.Message}"); }
        }

        // --- 工具方法 ---

        protected string QueryWorkOrderFromDb(string cassetteId)
        {
            if (DbConfig == null || string.IsNullOrEmpty(DbConfig.ConnectionString)) return null;
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string sql = $"SELECT TOP 1 {DbConfig.WoColumn} FROM {DbConfig.BindingTable} WHERE {DbConfig.CassetteColumn} = @cid";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cassetteId);
                        return cmd.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch { return null; }
        }

        protected string ExtractRecipeFromMes(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try
            {
                var list = JsonConvert.DeserializeObject<List<JObject>>(inputParamListDt);
                var recipeObj = list.FirstOrDefault(x => x["ParamName"]?.ToString() == "RECIPEID" || x["RefFieldCode"]?.ToString() == "RECIPE ID");
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            }
            catch { return "ERROR"; }
        }

        // --- 屬性與抽象定義 ---

        public ConnectionStatus Connection
        {
            get => _connectionStatus;
            set { if (_connectionStatus != value) { _connectionStatus = value; OnPropertyChanged(nameof(Connection)); } }
        }

        public EquipmentState State
        {
            get => _state;
            set { if (_state != value) { _state = value; OnPropertyChanged(nameof(State)); } }
        }
        
        public string CurrentRecipe
        {
            get => _currentRecipe;
            set { if (_currentRecipe != value) { _currentRecipe = value; OnPropertyChanged(nameof(CurrentRecipe)); } }
        }

        protected virtual void OnLogReceived(string message) => OnLogReceived(LogLevel.Info, message);
        protected virtual void OnLogReceived(LogLevel level, string message)
        {
            var timestamp = DateTime.Now;
            using (log4net.LogicalThreadContext.Stacks["EquipmentID"].Push(this.Id))
            {
                var logger = LogManager.GetLogger(typeof(EquipmentBase));
                switch (level)
                {
                    case LogLevel.Debug: logger.Debug(message); break;
                    case LogLevel.Info: logger.Info(message); break;
                    case LogLevel.Warn: logger.Warn(message); break;
                    case LogLevel.Error: logger.Error(message); break;
                    case LogLevel.Fatal: logger.Fatal(message); break;
                }
            }
            LogReceived?.Invoke(timestamp, level, message);
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public abstract void Start(CancellationToken cancellationToken, MqttBus _bus, int heartbeatIntervalSeconds);
        public abstract Task SendTimeCalibration();
        public virtual void Stop() { }

        public static Color GetColorForState(EquipmentState state)
        {
            switch (state)
            {
                case EquipmentState.Running: return Color.LimeGreen;
                case EquipmentState.Alarm:
                case EquipmentState.Down: return Color.Crimson;
                case EquipmentState.Paused:
                case EquipmentState.Idle: return Color.Gold;
                default: return Color.Black;
            }
        }
    }
}
