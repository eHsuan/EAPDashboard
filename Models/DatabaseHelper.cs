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
                // 自動備份機制
                try {
                    string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"EAPConfig_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    File.Copy(DbPath, backupPath, true);
                } catch { }
            }

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                
                // 1. 執行既有的遷移邏輯 (Modbus/BitOffset)
                MigrateModbusToPlc(conn);
                MigrateBitOffsetToTrigAck(conn);

                // 2. 執行新版「設備三層架構」遷移
                MigrateToTripleLayerSchema(conn);

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

                    CREATE TABLE IF NOT EXISTS Sub_Eqp_Master (
                        SubEqpMasterId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        SubEqpName TEXT,
                        MesReportId TEXT,
                        RecipeParamKey TEXT,
                        RecipeDataKey TEXT,
                        FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                    );

                    CREATE TABLE IF NOT EXISTS Plc_Events (
                        EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubEqpMasterId INTEGER,
                        EventName TEXT,
                        TriggerAddr TEXT,
                        AckAddr TEXT,
                        HandshakeType TEXT,
                        TrigBitOffset INTEGER DEFAULT 0,
                        AckBitOffset INTEGER DEFAULT 0,
                        Description TEXT,
                        FOREIGN KEY(SubEqpMasterId) REFERENCES Sub_Eqp_Master(SubEqpMasterId)
                    );

                    CREATE TABLE IF NOT EXISTS Plc_DataPoints (
                        PointId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventId INTEGER,
                        DataKey TEXT,
                        Address TEXT,
                        Length INTEGER,
                        DataType TEXT,
                        AccessType TEXT DEFAULT 'READ',
                        Description TEXT,
                        FOREIGN KEY(EventId) REFERENCES Plc_Events(EventId)
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

                    CREATE TABLE IF NOT EXISTS Plc_Key_Para_Mapping (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        ParaIndex INTEGER,
                        ParameterName TEXT,
                        DecimalPlaces INTEGER DEFAULT 0,
                        IsEnabled INTEGER DEFAULT 1
                    );

                    -- 建立維護用視圖：整合主機、子機、事件與點位
                    DROP VIEW IF EXISTS PLC_CONFIG_MAP;
                    CREATE VIEW PLC_CONFIG_MAP AS
                    SELECT 
                        Eq.Name AS '實體主機',
                        M.SubEqpName AS '邏輯子機',
                        M.MesReportId AS 'MES報工ID',
                        E.EventName AS '事件名稱',
                        E.TriggerAddr AS '觸發位址',
                        D.DataKey AS '點位鍵值',
                        D.Address AS 'PLC位址',
                        D.DataType AS '資料型別',
                        D.AccessType AS '讀寫模式',
                        D.Description AS '點位描述'
                    FROM Plc_DataPoints D
                    INNER JOIN Plc_Events E ON D.EventId = E.EventId
                    INNER JOIN Sub_Eqp_Master M ON E.SubEqpMasterId = M.SubEqpMasterId
                    INNER JOIN Equipment Eq ON M.EqpId = Eq.EqpId;
                ";

                using (var cmd = new SQLiteCommand(schema, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void MigrateToTripleLayerSchema(SQLiteConnection conn)
        {
            // 檢查是否需要遷移 (判斷 Plc_Events 是否還在使用舊的 EqpId 欄位)
            bool isOldSchema = false;
            using (var cmd = new SQLiteCommand("PRAGMA table_info(Plc_Events)", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) if (reader["name"].ToString() == "EqpId") isOldSchema = true;
                }
            }

            if (!isOldSchema) return;

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // 1. 建立 Sub_Eqp_Master 並從舊的 Sub_Equipment 與 Plc_Events 匯入資料
                    new SQLiteCommand(@"
                        CREATE TABLE IF NOT EXISTS Sub_Eqp_Master (
                            SubEqpMasterId INTEGER PRIMARY KEY AUTOINCREMENT,
                            EqpId TEXT,
                            SubEqpName TEXT,
                            MesReportId TEXT,
                            RecipeParamKey TEXT,
                            RecipeDataKey TEXT
                        );", conn).ExecuteNonQuery();

                    // 匯入現有的子設備配方設定
                    new SQLiteCommand(@"
                        INSERT INTO Sub_Eqp_Master (EqpId, SubEqpName, MesReportId, RecipeParamKey, RecipeDataKey)
                        SELECT EqpId, SubEqpName, EqpId, MesParamKey, DataKey FROM Sub_Equipment;", conn).ExecuteNonQuery();

                    // 針對 Plc_Events 中有 SubEqpID 但在 Master 中還沒有的機台建立主檔
                    new SQLiteCommand(@"
                        INSERT INTO Sub_Eqp_Master (EqpId, SubEqpName, MesReportId)
                        SELECT DISTINCT EqpId, SubEqpID, SubEqpID FROM Plc_Events 
                        WHERE SubEqpID IS NOT NULL AND SubEqpID NOT IN (SELECT SubEqpName FROM Sub_Eqp_Master);", conn).ExecuteNonQuery();

                    // 2. 升級 Plc_Events：遷移至新結構
                    new SQLiteCommand("ALTER TABLE Plc_Events RENAME TO Plc_Events_Old;", conn).ExecuteNonQuery();
                    new SQLiteCommand(@"
                        CREATE TABLE Plc_Events (
                            EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                            SubEqpMasterId INTEGER,
                            EventName TEXT,
                            TriggerAddr TEXT,
                            AckAddr TEXT,
                            HandshakeType TEXT,
                            TrigBitOffset INTEGER DEFAULT 0,
                            AckBitOffset INTEGER DEFAULT 0,
                            Description TEXT
                        );", conn).ExecuteNonQuery();

                    // 根據 EqpId 與 SubEqpID 建立連結
                    new SQLiteCommand(@"
                        INSERT INTO Plc_Events (EventId, SubEqpMasterId, EventName, TriggerAddr, AckAddr, HandshakeType, TrigBitOffset, AckBitOffset)
                        SELECT E.EventId, M.SubEqpMasterId, E.EventName, E.TriggerAddr, E.AckAddr, E.HandshakeType, E.TrigBitOffset, E.AckBitOffset
                        FROM Plc_Events_Old E
                        LEFT JOIN Sub_Eqp_Master M ON E.EqpId = M.EqpId AND (E.SubEqpID = M.SubEqpName OR (E.SubEqpID IS NULL AND M.MesReportId = E.EqpId));", conn).ExecuteNonQuery();

                    // 3. 升級 Plc_DataPoints：增加 Description 欄位
                    bool hasDpDesc = false;
                    using (var cmd = new SQLiteCommand("PRAGMA table_info(Plc_DataPoints)", conn)) {
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) if (reader["name"].ToString() == "Description") hasDpDesc = true;
                        }
                    }
                    if (!hasDpDesc) new SQLiteCommand("ALTER TABLE Plc_DataPoints ADD COLUMN Description TEXT;", conn).ExecuteNonQuery();

                    // 4. 清理舊表
                    new SQLiteCommand("DROP TABLE Plc_Events_Old;", conn).ExecuteNonQuery();
                    new SQLiteCommand("DROP TABLE Sub_Equipment;", conn).ExecuteNonQuery();

                    transaction.Commit();
                }
                catch { transaction.Rollback(); }
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
                        return (result != null && !string.IsNullOrEmpty(result.ToString())) ? result.ToString() : mesCode;
                    }
                }
            } catch { return mesCode; }
        }

        public static List<SubEqpConfig> GetSubEqpConfigs(string eqpId)
        {
            var configs = new List<SubEqpConfig>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Sub_Eqp_Master WHERE EqpId = @eqid";
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
                                MesReportId = reader["MesReportId"].ToString(),
                                RecipeParamKey = reader["RecipeParamKey"]?.ToString(),
                                RecipeDataKey = reader["RecipeDataKey"]?.ToString()
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
                // 使用 JOIN 同步抓出子機的 MES 上報 ID 與 名稱
                string sqlEvt = @"
                    SELECT E.*, M.MesReportId, M.SubEqpName, M.EqpId AS MasterEqpId
                    FROM Plc_Events E
                    INNER JOIN Sub_Eqp_Master M ON E.SubEqpMasterId = M.SubEqpMasterId
                    WHERE M.EqpId = @eqid";
                
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
                                EqpId = reader["MasterEqpId"].ToString(),
                                SubEqpID = reader["MesReportId"].ToString(), // 給程式邏輯用的 MES 上報 ID
                                SubEqpName = reader["SubEqpName"].ToString(), // 給 Log 用的中文名稱
                                EventName = reader["EventName"].ToString(),
                                TriggerAddr = reader["TriggerAddr"].ToString(),
                                AckAddr = reader["AckAddr"].ToString(),
                                HandshakeType = reader["HandshakeType"].ToString(),
                                TrigBitOffset = Convert.ToInt32(reader["TrigBitOffset"]),
                                AckBitOffset = Convert.ToInt32(reader["AckBitOffset"]),
                                Description = reader["Description"]?.ToString()
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
                                    AccessType = reader["AccessType"].ToString(),
                                    Description = reader["Description"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            return events;
        }

        public static List<KpiMappingInfo> GetKpiMappings(string eqpId)
        {
            var list = new List<KpiMappingInfo>();
            try {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM Plc_Key_Para_Mapping WHERE EqpId = @eqid";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@eqid", eqpId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new KpiMappingInfo
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    EqpId = reader["EqpId"].ToString(),
                                    ParaIndex = Convert.ToInt32(reader["ParaIndex"]),
                                    ParameterName = reader["ParameterName"].ToString(),
                                    DecimalPlaces = Convert.ToInt32(reader["DecimalPlaces"]),
                                    IsEnabled = Convert.ToInt32(reader["IsEnabled"]) == 1
                                });
                            }
                        }
                    }
                }
            } catch { }
            return list;
        }

        public static SQLiteConnection GetConnection() => new SQLiteConnection(ConnectionString);

        private static void MigrateModbusToPlc(SQLiteConnection conn)
        {
            bool modbusTableExists = false;
            using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='Modbus_Events'", conn))
                modbusTableExists = cmd.ExecuteScalar() != null;

            if (modbusTableExists)
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        new SQLiteCommand("CREATE TABLE IF NOT EXISTS Plc_Events (EventId INTEGER PRIMARY KEY AUTOINCREMENT, EqpId TEXT, SubEqpID TEXT, EventName TEXT, TriggerAddr TEXT, AckAddr TEXT, HandshakeType TEXT, TrigBitOffset INTEGER DEFAULT 0, AckBitOffset INTEGER DEFAULT 0);", conn).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO Plc_Events (EventId, EqpId, EventName, TriggerAddr, AckAddr, HandshakeType, TrigBitOffset, AckBitOffset) SELECT EventId, EqpId, EventName, CAST(TriggerAddr AS TEXT), CAST(AckAddr AS TEXT), HandshakeType, BitOffset, BitOffset FROM Modbus_Events", conn).ExecuteNonQuery();
                        new SQLiteCommand("CREATE TABLE IF NOT EXISTS Plc_DataPoints (PointId INTEGER PRIMARY KEY AUTOINCREMENT, EventId INTEGER, DataKey TEXT, Address TEXT, Length INTEGER, DataType TEXT, AccessType TEXT DEFAULT 'READ');", conn).ExecuteNonQuery();
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
                    while (reader.Read()) if (reader["name"].ToString() == "BitOffset") hasBitOffset = true;
            }
            if (hasBitOffset)
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        new SQLiteCommand("ALTER TABLE Plc_Events RENAME TO Plc_Events_Old", conn).ExecuteNonQuery();
                        new SQLiteCommand("CREATE TABLE Plc_Events (EventId INTEGER PRIMARY KEY AUTOINCREMENT, EqpId TEXT, SubEqpID TEXT, EventName TEXT, TriggerAddr TEXT, AckAddr TEXT, HandshakeType TEXT, TrigBitOffset INTEGER DEFAULT 0, AckBitOffset INTEGER DEFAULT 0);", conn).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO Plc_Events (EventId, EqpId, SubEqpID, EventName, TriggerAddr, AckAddr, HandshakeType, TrigBitOffset, AckBitOffset) SELECT EventId, EqpId, NULL, EventName, TriggerAddr, AckAddr, HandshakeType, BitOffset, BitOffset FROM Plc_Events_Old", conn).ExecuteNonQuery();
                        new SQLiteCommand("DROP TABLE Plc_Events_Old", conn).ExecuteNonQuery();
                        transaction.Commit();
                    } catch { transaction.Rollback(); }
                }
            }
        }

        public static List<PendingBinding> GetPendingBindings()
        {
            var list = new List<PendingBinding>();
            try {
                using (var conn = GetConnection()) {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("SELECT * FROM Local_Binding_Buffer", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read()) list.Add(new PendingBinding { Id = Convert.ToInt32(reader["Id"]), CassetteID = reader["CassetteID"].ToString(), WorkOrder = reader["WorkOrder"].ToString(), PanelList = reader["PanelList"].ToString() });
                }
            } catch { }
            return list;
        }

        public static void DeletePendingBinding(int id) { try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("DELETE FROM Local_Binding_Buffer WHERE Id = @id", conn)) { cmd.Parameters.AddWithValue("@id", id); cmd.ExecuteNonQuery(); } } } catch { } }
        public static List<string> GetPendingUnbindings() { var list = new List<string>(); try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT CassetteID FROM Local_Unbinding_Buffer", conn)) using (var reader = cmd.ExecuteReader()) while (reader.Read()) list.Add(reader["CassetteID"].ToString()); } } catch { } return list; }
        public static void DeletePendingUnbinding(string cid) { try { using (var conn = GetConnection()) { conn.Open(); using (var cmd = new SQLiteCommand("DELETE FROM Local_Unbinding_Buffer WHERE CassetteID = @cid", conn)) { cmd.Parameters.AddWithValue("@cid", cid); cmd.ExecuteNonQuery(); } } } catch { } }
    }

    public class SubEqpConfig
    {
        public string SubEqpName { get; set; }
        public string MesReportId { get; set; }
        public string RecipeParamKey { get; set; }
        public string RecipeDataKey { get; set; }
    }

    public class PlcEventConfig
    {
        public int EventId { get; set; }
        public string EqpId { get; set; }
        public string SubEqpID { get; set; } // MES 報工 ID
        public string SubEqpName { get; set; } // 顯示名稱
        public string EventName { get; set; }
        public string TriggerAddr { get; set; }
        public string AckAddr { get; set; }
        public string HandshakeType { get; set; }
        public int TrigBitOffset { get; set; }
        public int AckBitOffset { get; set; }
        public string Description { get; set; }
        public List<PlcDataPoint> DataPoints { get; set; } = new List<PlcDataPoint>();
    }

    public class PlcDataPoint
    {
        public string Key { get; set; }
        public string Address { get; set; }
        public ushort Length { get; set; }
        public string DataType { get; set; }
        public string AccessType { get; set; }
        public string Description { get; set; }
    }

    public class KpiMappingInfo
    {
        public int Id { get; set; }
        public string EqpId { get; set; }
        public int ParaIndex { get; set; }
        public string ParameterName { get; set; }
        public int DecimalPlaces { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class PendingBinding { public int Id { get; set; } public string CassetteID { get; set; } public string WorkOrder { get; set; } public string PanelList { get; set; } }
}
