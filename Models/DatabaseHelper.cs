using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

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
    }

    public class PendingBinding { public int Id { get; set; } public string CassetteID { get; set; } public string WorkOrder { get; set; } public string PanelList { get; set; } }
}
