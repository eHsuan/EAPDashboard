using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using EAPDashboard.Models.Smt;

namespace EAPDashboard.Models
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EAPConfig.db");
        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
            }
            else
            {
                try {
                    string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"EAPConfig_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    File.Copy(DbPath, backupPath, true);
                } catch { }
            }

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                MigrateBufferSchemaToAddEqpId(conn);
                MigrateEquipmentSchemaForTimeFormat(conn);

                string schema = @"
                    CREATE TABLE IF NOT EXISTS Equipment (
                        EqpId TEXT PRIMARY KEY,
                        Name TEXT,
                        Protocol TEXT NOT NULL,
                        Ip TEXT,
                        Port INTEGER,
                        Topic TEXT,
                        DeviceId INTEGER,
                        DictionaryPath TEXT,
                        TimeFormat TEXT DEFAULT '16',
                        IsActive INTEGER DEFAULT 1
                    );

                    CREATE TABLE IF NOT EXISTS Result_Mapping (
                        MappingId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventName TEXT,
                        MesResultCode TEXT,
                        MappedRtnCode TEXT,
                        Description TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Alarm_Mapping (
                        EqpId TEXT,
                        RawCode TEXT,
                        MappedCode TEXT,
                        Description TEXT,
                        PRIMARY KEY(EqpId, RawCode)
                    );

                    CREATE TABLE IF NOT EXISTS Production_Logs (
                        LogId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        EventName TEXT,
                        WorkOrder TEXT,
                        DataJson TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );

                    CREATE TABLE IF NOT EXISTS Local_Binding_Buffer (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        CassetteID TEXT,
                        WorkOrder TEXT,
                        PanelList TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );

                    CREATE TABLE IF NOT EXISTS Local_Unbinding_Buffer (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        CassetteID TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );

                    CREATE TABLE IF NOT EXISTS SMT_WorkOrder_State (
                        LineId TEXT NOT NULL,
                        WorkOrder TEXT NOT NULL,
                        Station1_In_Count INTEGER DEFAULT 0,
                        Station1_Out_Count INTEGER DEFAULT 0,
                        Station1_Actual_QTY INTEGER,
                        Station1_Finished INTEGER DEFAULT 0,
                        Station1_FinishedTime DATETIME,
                        Station2_In_Count INTEGER DEFAULT 0,
                        Station2_Out_Count INTEGER DEFAULT 0,
                        Station2_Actual_QTY INTEGER,
                        Station2_Finished INTEGER DEFAULT 0,
                        Station2_FinishedTime DATETIME,
                        CreatedTime DATETIME DEFAULT (datetime('now', 'localtime')),
                        UpdatedTime DATETIME DEFAULT (datetime('now', 'localtime')),
                        PRIMARY KEY(LineId, WorkOrder)
                    );

                    CREATE TABLE IF NOT EXISTS SMT_Line_Current (
                        LineId TEXT PRIMARY KEY,
                        Station1_Last_WO TEXT,
                        Station2_Last_WO TEXT,
                        UpdatedTime DATETIME DEFAULT (datetime('now', 'localtime'))
                    );
                ";

                using (var cmd = new SQLiteCommand(schema, conn)) { cmd.ExecuteNonQuery(); }
            }
        }

        private static void MigrateBufferSchemaToAddEqpId(SQLiteConnection conn)
        {
            try
            {
                // 檢查 Local_Binding_Buffer
                bool hasEqpId1 = false;
                using (var cmd = new SQLiteCommand("PRAGMA table_info(Local_Binding_Buffer)", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) if (reader["name"].ToString() == "EqpId") hasEqpId1 = true;
                }
                if (!hasEqpId1)
                {
                    using (var cmd = new SQLiteCommand("ALTER TABLE Local_Binding_Buffer ADD COLUMN EqpId TEXT", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 檢查 Local_Unbinding_Buffer
                bool hasEqpId2 = false;
                using (var cmd = new SQLiteCommand("PRAGMA table_info(Local_Unbinding_Buffer)", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) if (reader["name"].ToString() == "EqpId") hasEqpId2 = true;
                }
                if (!hasEqpId2)
                {
                    using (var cmd = new SQLiteCommand("ALTER TABLE Local_Unbinding_Buffer ADD COLUMN EqpId TEXT", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private static void MigrateEquipmentSchemaForTimeFormat(SQLiteConnection conn)
        {
            try
            {
                bool hasTimeFormat = false;
                using (var cmd = new SQLiteCommand("PRAGMA table_info(Equipment)", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) if (reader["name"].ToString() == "TimeFormat") hasTimeFormat = true;
                }
                if (!hasTimeFormat)
                {
                    using (var cmd = new SQLiteCommand("ALTER TABLE Equipment ADD COLUMN TimeFormat TEXT DEFAULT '16'", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static string GetMappedRtnCode(string eventName, string mesCode)
        {
            if (string.IsNullOrEmpty(mesCode)) return mesCode;
            try {
                using (var conn = GetConnection()) {
                    conn.Open();
                    string sql = "SELECT MappedRtnCode FROM Result_Mapping WHERE EventName = @evt AND MesResultCode = @mes";
                    using (var cmd = new SQLiteCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@evt", eventName); cmd.Parameters.AddWithValue("@mes", mesCode);
                        var result = cmd.ExecuteScalar();
                        return (result != null && !string.IsNullOrEmpty(result.ToString())) ? result.ToString() : mesCode;
                    }
                }
            } catch { return mesCode; }
        }

        public static SQLiteConnection GetConnection() => new SQLiteConnection(ConnectionString);
        public static List<PendingBinding> GetPendingBindings(string eqpId) { var list = new List<PendingBinding>(); try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT * FROM Local_Binding_Buffer WHERE EqpId = @eqpid", conn)) { cmd.Parameters.AddWithValue("@eqpid", eqpId); using (var reader = cmd.ExecuteReader()) while (reader.Read()) list.Add(new PendingBinding { Id = Convert.ToInt32(reader["Id"]), CassetteID = reader["CassetteID"].ToString(), WorkOrder = reader["WorkOrder"].ToString(), PanelList = reader["PanelList"].ToString() }); } } } catch { } return list; }
        public static void DeletePendingBinding(int id) { try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("DELETE FROM Local_Binding_Buffer WHERE Id = @id", conn)) { cmd.Parameters.AddWithValue("@id", id); cmd.ExecuteNonQuery(); } } } catch { } }
        public static List<string> GetPendingUnbindings(string eqpId) { var list = new List<string>(); try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT CassetteID FROM Local_Unbinding_Buffer WHERE EqpId = @eqpid", conn)) { cmd.Parameters.AddWithValue("@eqpid", eqpId); using (var reader = cmd.ExecuteReader()) while (reader.Read()) list.Add(reader["CassetteID"].ToString()); } } } catch { } return list; }
        public static void DeletePendingUnbinding(string cid) { try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("DELETE FROM Local_Unbinding_Buffer WHERE CassetteID = @cid", conn)) { cmd.Parameters.AddWithValue("@cid", cid); cmd.ExecuteNonQuery(); } } } catch { } }

        #region SMT Line State Management

        public static SmtWorkOrderState GetSmtWorkOrderState(string lineId, string workOrder)
        {
            if (string.IsNullOrWhiteSpace(lineId) || string.IsNullOrWhiteSpace(workOrder)) return null;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT * FROM SMT_WorkOrder_State WHERE LineId = @lineId AND WorkOrder = @wo";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lineId", lineId);
                        cmd.Parameters.AddWithValue("@wo", workOrder);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SmtWorkOrderState
                                {
                                    LineId = reader["LineId"]?.ToString(),
                                    WorkOrder = reader["WorkOrder"]?.ToString(),
                                    Station1_In_Count = Convert.ToInt32(reader["Station1_In_Count"]),
                                    Station1_Out_Count = Convert.ToInt32(reader["Station1_Out_Count"]),
                                    Station1_Actual_QTY = reader["Station1_Actual_QTY"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Station1_Actual_QTY"]),
                                    Station1_Finished = Convert.ToInt32(reader["Station1_Finished"]) == 1,
                                    Station1_FinishedTime = reader["Station1_FinishedTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["Station1_FinishedTime"]),
                                    Station2_In_Count = Convert.ToInt32(reader["Station2_In_Count"]),
                                    Station2_Out_Count = Convert.ToInt32(reader["Station2_Out_Count"]),
                                    Station2_Actual_QTY = reader["Station2_Actual_QTY"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Station2_Actual_QTY"]),
                                    Station2_Finished = Convert.ToInt32(reader["Station2_Finished"]) == 1,
                                    Station2_FinishedTime = reader["Station2_FinishedTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["Station2_FinishedTime"]),
                                    CreatedTime = reader["CreatedTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedTime"]),
                                    UpdatedTime = reader["UpdatedTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["UpdatedTime"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SMT DB] GetSmtWorkOrderState failed: {ex.Message}");
            }
            return null;
        }

        public static void SaveSmtWorkOrderState(SmtWorkOrderState state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.LineId) || string.IsNullOrWhiteSpace(state.WorkOrder)) return;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO SMT_WorkOrder_State (
                            LineId, WorkOrder, 
                            Station1_In_Count, Station1_Out_Count, Station1_Actual_QTY, Station1_Finished, Station1_FinishedTime,
                            Station2_In_Count, Station2_Out_Count, Station2_Actual_QTY, Station2_Finished, Station2_FinishedTime,
                            UpdatedTime
                        ) VALUES (
                            @lineId, @wo,
                            @s1In, @s1Out, @s1Actual, @s1Fin, @s1FinTime,
                            @s2In, @s2Out, @s2Actual, @s2Fin, @s2FinTime,
                            datetime('now', 'localtime')
                        )
                        ON CONFLICT(LineId, WorkOrder) DO UPDATE SET
                            Station1_In_Count = excluded.Station1_In_Count,
                            Station1_Out_Count = excluded.Station1_Out_Count,
                            Station1_Actual_QTY = excluded.Station1_Actual_QTY,
                            Station1_Finished = excluded.Station1_Finished,
                            Station1_FinishedTime = excluded.Station1_FinishedTime,
                            Station2_In_Count = excluded.Station2_In_Count,
                            Station2_Out_Count = excluded.Station2_Out_Count,
                            Station2_Actual_QTY = excluded.Station2_Actual_QTY,
                            Station2_Finished = excluded.Station2_Finished,
                            Station2_FinishedTime = excluded.Station2_FinishedTime,
                            UpdatedTime = datetime('now', 'localtime');";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lineId", state.LineId);
                        cmd.Parameters.AddWithValue("@wo", state.WorkOrder);
                        cmd.Parameters.AddWithValue("@s1In", state.Station1_In_Count);
                        cmd.Parameters.AddWithValue("@s1Out", state.Station1_Out_Count);
                        cmd.Parameters.AddWithValue("@s1Actual", (object)state.Station1_Actual_QTY ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@s1Fin", state.Station1_Finished ? 1 : 0);
                        cmd.Parameters.AddWithValue("@s1FinTime", (object)state.Station1_FinishedTime ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@s2In", state.Station2_In_Count);
                        cmd.Parameters.AddWithValue("@s2Out", state.Station2_Out_Count);
                        cmd.Parameters.AddWithValue("@s2Actual", (object)state.Station2_Actual_QTY ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@s2Fin", state.Station2_Finished ? 1 : 0);
                        cmd.Parameters.AddWithValue("@s2FinTime", (object)state.Station2_FinishedTime ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SMT DB] SaveSmtWorkOrderState failed: {ex.Message}");
            }
        }

        public static List<SmtWorkOrderState> GetActiveSmtWorkOrders(string lineId)
        {
            var list = new List<SmtWorkOrderState>();
            if (string.IsNullOrWhiteSpace(lineId)) return list;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT * FROM SMT_WorkOrder_State 
                                   WHERE LineId = @lineId AND (Station1_Finished = 0 OR Station2_Finished = 0)
                                   ORDER BY CreatedTime DESC";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lineId", lineId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SmtWorkOrderState
                                {
                                    LineId = reader["LineId"]?.ToString(),
                                    WorkOrder = reader["WorkOrder"]?.ToString(),
                                    Station1_In_Count = Convert.ToInt32(reader["Station1_In_Count"]),
                                    Station1_Out_Count = Convert.ToInt32(reader["Station1_Out_Count"]),
                                    Station1_Actual_QTY = reader["Station1_Actual_QTY"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Station1_Actual_QTY"]),
                                    Station1_Finished = Convert.ToInt32(reader["Station1_Finished"]) == 1,
                                    Station1_FinishedTime = reader["Station1_FinishedTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["Station1_FinishedTime"]),
                                    Station2_In_Count = Convert.ToInt32(reader["Station2_In_Count"]),
                                    Station2_Out_Count = Convert.ToInt32(reader["Station2_Out_Count"]),
                                    Station2_Actual_QTY = reader["Station2_Actual_QTY"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Station2_Actual_QTY"]),
                                    Station2_Finished = Convert.ToInt32(reader["Station2_Finished"]) == 1,
                                    Station2_FinishedTime = reader["Station2_FinishedTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["Station2_FinishedTime"]),
                                    CreatedTime = reader["CreatedTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedTime"]),
                                    UpdatedTime = reader["UpdatedTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["UpdatedTime"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SMT DB] GetActiveSmtWorkOrders failed: {ex.Message}");
            }
            return list;
        }

        public static (string St1LastWo, string St2LastWo) GetSmtLineCurrent(string lineId)
        {
            if (string.IsNullOrWhiteSpace(lineId)) return (null, null);
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT Station1_Last_WO, Station2_Last_WO FROM SMT_Line_Current WHERE LineId = @lineId";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lineId", lineId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (reader["Station1_Last_WO"]?.ToString(), reader["Station2_Last_WO"]?.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SMT DB] GetSmtLineCurrent failed: {ex.Message}");
            }
            return (null, null);
        }

        public static void SaveSmtLineCurrent(string lineId, string st1LastWo, string st2LastWo)
        {
            if (string.IsNullOrWhiteSpace(lineId)) return;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO SMT_Line_Current (LineId, Station1_Last_WO, Station2_Last_WO, UpdatedTime)
                        VALUES (@lineId, @st1, @st2, datetime('now', 'localtime'))
                        ON CONFLICT(LineId) DO UPDATE SET
                            Station1_Last_WO = COALESCE(@st1, Station1_Last_WO),
                            Station2_Last_WO = COALESCE(@st2, Station2_Last_WO),
                            UpdatedTime = datetime('now', 'localtime');";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lineId", lineId);
                        cmd.Parameters.AddWithValue("@st1", (object)st1LastWo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@st2", (object)st2LastWo ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SMT DB] SaveSmtLineCurrent failed: {ex.Message}");
            }
        }

        #endregion
    }

    public class PendingBinding { public int Id { get; set; } public string CassetteID { get; set; } public string WorkOrder { get; set; } public string PanelList { get; set; } }
}
