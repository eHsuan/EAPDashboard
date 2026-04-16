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

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                
                // 1. 執行遷移邏輯
                MigrateModbusToPlc(conn);
                MigrateBitOffsetToTrigAck(conn);

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
                        IsActive INTEGER DEFAULT 1
                    );

                    CREATE TABLE IF NOT EXISTS Plc_Events (
                        EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        SubEqpID TEXT,
                        EventName TEXT,
                        TriggerAddr TEXT,
                        AckAddr TEXT,
                        HandshakeType TEXT,
                        TrigBitOffset INTEGER DEFAULT 0,
                        AckBitOffset INTEGER DEFAULT 0,
                        FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                    );

                    CREATE TABLE IF NOT EXISTS Plc_DataPoints (
                        PointId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventId INTEGER,
                        DataKey TEXT,
                        Address TEXT,
                        Length INTEGER,
                        DataType TEXT,
                        AccessType TEXT DEFAULT 'READ',
                        FOREIGN KEY(EventId) REFERENCES Plc_Events(EventId)
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

                    CREATE TABLE IF NOT EXISTS Sub_Equipment (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        SubEqpName TEXT,
                        MesParamKey TEXT,
                        DataKey TEXT,
                        FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                    );

                    CREATE TABLE IF NOT EXISTS Result_Mapping (
                        MappingId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventName TEXT,
                        MesResultCode TEXT,
                        MappedRtnCode TEXT,
                        Description TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Local_Binding_Buffer (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CassetteID TEXT,
                        WorkOrder TEXT,
                        PanelList TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );

                    CREATE TABLE IF NOT EXISTS Local_Unbinding_Buffer (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CassetteID TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );
                ";

                using (var cmd = new SQLiteCommand(schema, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string GetMappedRtnCode(string eventName, string mesCode)
        {
            if (string.IsNullOrEmpty(mesCode)) return mesCode;
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT MappedRtnCode FROM Result_Mapping WHERE EventName = @evt AND MesResultCode = @mes";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@evt", eventName);
                        cmd.Parameters.AddWithValue("@mes", mesCode);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? mesCode;
                    }
                }
            } catch { return mesCode; }
        }

        public static List<PendingBinding> GetPendingBindings()
        {
            var list = new List<PendingBinding>();
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM Local_Binding_Buffer";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new PendingBinding
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CassetteID = reader["CassetteID"].ToString(),
                                    WorkOrder = reader["WorkOrder"].ToString(),
                                    PanelList = reader["PanelList"].ToString()
                                });
                            }
                        }
                    }
                }
            } catch { }
            return list;
        }

        public static void DeletePendingBinding(int id)
        {
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "DELETE FROM Local_Binding_Buffer WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { }
        }

        public static List<string> GetPendingUnbindings()
        {
            var list = new List<string>();
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT CassetteID FROM Local_Unbinding_Buffer";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) list.Add(reader["CassetteID"].ToString());
                        }
                    }
                }
            } catch { }
            return list;
        }

        public static void DeletePendingUnbinding(string cid)
        {
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "DELETE FROM Local_Unbinding_Buffer WHERE CassetteID = @cid";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cid);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { }
        }

        private static void MigrateModbusToPlc(SQLiteConnection conn)
        {
            bool modbusTableExists = false;
            using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='Modbus_Events'", conn))
            {
                modbusTableExists = cmd.ExecuteScalar() != null;
            }

            if (modbusTableExists)
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string createPlcEvents = @"
                            CREATE TABLE IF NOT EXISTS Plc_Events (
                                EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                                EqpId TEXT,
                                SubEqpID TEXT,
                                EventName TEXT,
                                TriggerAddr TEXT,
                                AckAddr TEXT,
                                HandshakeType TEXT,
                                TrigBitOffset INTEGER DEFAULT 0,
                                AckBitOffset INTEGER DEFAULT 0,
                                FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                            );";
                        new SQLiteCommand(createPlcEvents, conn).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO Plc_Events (EventId, EqpId, EventName, TriggerAddr, AckAddr, HandshakeType, TrigBitOffset, AckBitOffset) SELECT EventId, EqpId, EventName, CAST(TriggerAddr AS TEXT), CAST(AckAddr AS TEXT), HandshakeType, BitOffset, BitOffset FROM Modbus_Events", conn).ExecuteNonQuery();

                        string createPlcDataPoints = @"
                            CREATE TABLE IF NOT EXISTS Plc_DataPoints (
                                PointId INTEGER PRIMARY KEY AUTOINCREMENT,
                                EventId INTEGER,
                                DataKey TEXT,
                                Address TEXT,
                                Length INTEGER,
                                DataType TEXT,
                                AccessType TEXT DEFAULT 'READ',
                                FOREIGN KEY(EventId) REFERENCES Plc_Events(EventId)
                            );";
                        new SQLiteCommand(createPlcDataPoints, conn).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO Plc_DataPoints SELECT PointId, EventId, DataKey, CAST(Address AS TEXT), Length, DataType, AccessType FROM Modbus_DataPoints", conn).ExecuteNonQuery();

                        new SQLiteCommand("DROP TABLE Modbus_DataPoints", conn).ExecuteNonQuery();
                        new SQLiteCommand("DROP TABLE Modbus_Events", conn).ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); }
                }
            }
        }

        private static void MigrateBitOffsetToTrigAck(SQLiteConnection conn)
        {
            bool hasBitOffset = false;
            using (var cmd = new SQLiteCommand("PRAGMA table_info(Plc_Events)", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) if (reader["name"].ToString() == "BitOffset") hasBitOffset = true;
                }
            }

            if (hasBitOffset)
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        new SQLiteCommand("ALTER TABLE Plc_Events RENAME TO Plc_Events_Old", conn).ExecuteNonQuery();
                        string createNew = @"
                            CREATE TABLE Plc_Events (
                                EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                                EqpId TEXT,
                                SubEqpID TEXT,
                                EventName TEXT,
                                TriggerAddr TEXT,
                                AckAddr TEXT,
                                HandshakeType TEXT,
                                TrigBitOffset INTEGER DEFAULT 0,
                                AckBitOffset INTEGER DEFAULT 0,
                                FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                            );";
                        new SQLiteCommand(createNew, conn).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO Plc_Events (EventId, EqpId, SubEqpID, EventName, TriggerAddr, AckAddr, HandshakeType, TrigBitOffset, AckBitOffset) SELECT EventId, EqpId, NULL, EventName, TriggerAddr, AckAddr, HandshakeType, BitOffset, BitOffset FROM Plc_Events_Old", conn).ExecuteNonQuery();
                        new SQLiteCommand("DROP TABLE Plc_Events_Old", conn).ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); }
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public static List<SubEqpConfig> GetSubEqpConfigs(string eqpId)
        {
            var configs = new List<SubEqpConfig>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Sub_Equipment WHERE EqpId = @eqid";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eqid", eqpId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            configs.Add(new SubEqpConfig
                            {
                                SubEqpName = reader["SubEqpName"].ToString(),
                                MesParamKey = reader["MesParamKey"].ToString(),
                                DataKey = reader["DataKey"].ToString()
                            });
                        }
                    }
                }
            }
            return configs;
        }

        public static List<PlcEventConfig> GetPlcConfigs(string eqpId)
        {
            var events = new List<PlcEventConfig>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sqlEvt = "SELECT * FROM Plc_Events WHERE EqpId = @eqid";
                using (var cmdEvt = new SQLiteCommand(sqlEvt, conn))
                {
                    cmdEvt.Parameters.AddWithValue("@eqid", eqpId);
                    using (var reader = cmdEvt.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var evt = new PlcEventConfig
                            {
                                EventId = Convert.ToInt32(reader["EventId"]),
                                EqpId = reader["EqpId"].ToString(),
                                SubEqpID = reader["SubEqpID"]?.ToString(),
                                EventName = reader["EventName"].ToString(),
                                TriggerAddr = reader["TriggerAddr"].ToString(),
                                AckAddr = reader["AckAddr"].ToString(),
                                HandshakeType = reader["HandshakeType"].ToString(),
                                TrigBitOffset = Convert.ToInt32(reader["TrigBitOffset"]),
                                AckBitOffset = Convert.ToInt32(reader["AckBitOffset"])
                            };
                            events.Add(evt);
                        }
                    }
                }

                foreach (var evt in events)
                {
                    string sqlDp = "SELECT * FROM Plc_DataPoints WHERE EventId = @evid";
                    using (var cmdDp = new SQLiteCommand(sqlDp, conn))
                    {
                        cmdDp.Parameters.AddWithValue("@evid", evt.EventId);
                        using (var reader = cmdDp.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                evt.DataPoints.Add(new PlcDataPoint
                                {
                                    Key = reader["DataKey"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Length = (ushort)Convert.ToInt32(reader["Length"]),
                                    DataType = reader["DataType"].ToString(),
                                    AccessType = reader["AccessType"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return events;
        }
    }

    public class PlcEventConfig
    {
        public int EventId { get; set; }
        public string EqpId { get; set; }
        public string SubEqpID { get; set; }
        public string EventName { get; set; }
        public string TriggerAddr { get; set; }
        public string AckAddr { get; set; }
        public string HandshakeType { get; set; }
        public int TrigBitOffset { get; set; }
        public int AckBitOffset { get; set; }
        public List<PlcDataPoint> DataPoints { get; set; } = new List<PlcDataPoint>();
    }

    public class PlcDataPoint
    {
        public string Key { get; set; }
        public string Address { get; set; }
        public ushort Length { get; set; }
        public string DataType { get; set; }
        public string AccessType { get; set; }
    }

    public class SubEqpConfig
    {
        public string SubEqpName { get; set; }
        public string MesParamKey { get; set; }
        public string DataKey { get; set; }
    }

    public class PendingBinding
    {
        public int Id { get; set; }
        public string CassetteID { get; set; }
        public string WorkOrder { get; set; }
        public string PanelList { get; set; }
    }
}
