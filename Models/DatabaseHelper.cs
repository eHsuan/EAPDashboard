using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

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

                    CREATE TABLE IF NOT EXISTS Modbus_Events (
                        EventId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        EventName TEXT,
                        TriggerAddr INTEGER,
                        AckAddr INTEGER,
                        HandshakeType TEXT, -- 'ROLLING', 'BIT', 'HEARTBEAT'
                        BitOffset INTEGER DEFAULT 0, -- 新增：位元偏移量 (0-15)
                        FOREIGN KEY(EqpId) REFERENCES Equipment(EqpId)
                    );

                    CREATE TABLE IF NOT EXISTS Modbus_DataPoints (
                        PointId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EventId INTEGER,
                        DataKey TEXT,
                        Address INTEGER,
                        Length INTEGER,
                        DataType TEXT, -- 'BIT', 'WORD', 'ASCII', 'DATETIME'
                        AccessType TEXT DEFAULT 'READ', -- 'READ', 'WRITE'
                        FOREIGN KEY(EventId) REFERENCES Modbus_Events(EventId)
                    );

                    CREATE TABLE IF NOT EXISTS Alarm_Mapping (
                        EqpId TEXT,
                        RawCode TEXT,
                        MappedCode TEXT,
                        Description TEXT,
                        PRIMARY KEY(EqpId, RawCode)
                    );

                    -- 5. 生產數據紀錄表 (雙軌儲存源)
                    CREATE TABLE IF NOT EXISTS Production_Logs (
                        LogId INTEGER PRIMARY KEY AUTOINCREMENT,
                        EqpId TEXT,
                        EventName TEXT,
                        WorkOrder TEXT,
                        DataJson TEXT,
                        Timestamp DATETIME DEFAULT (datetime('now', 'localtime'))
                    );
                ";

                using (var cmd = new SQLiteCommand(schema, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public static List<ModbusEventConfig> GetModbusConfigs(string eqpId)
        {
            var events = new List<ModbusEventConfig>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sqlEvt = "SELECT * FROM Modbus_Events WHERE EqpId = @eqid";
                using (var cmdEvt = new SQLiteCommand(sqlEvt, conn))
                {
                    cmdEvt.Parameters.AddWithValue("@eqid", eqpId);
                    using (var reader = cmdEvt.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var evt = new ModbusEventConfig
                            {
                                EventId = Convert.ToInt32(reader["EventId"]),
                                EventName = reader["EventName"].ToString(),
                                TriggerAddr = (ushort)Convert.ToInt32(reader["TriggerAddr"]),
                                AckAddr = (ushort)Convert.ToInt32(reader["AckAddr"]),
                                HandshakeType = reader["HandshakeType"].ToString(),
                                BitOffset = Convert.ToInt32(reader["BitOffset"])
                            };
                            events.Add(evt);
                        }
                    }
                }

                foreach (var evt in events)
                {
                    string sqlDp = "SELECT * FROM Modbus_DataPoints WHERE EventId = @evid";
                    using (var cmdDp = new SQLiteCommand(sqlDp, conn))
                    {
                        cmdDp.Parameters.AddWithValue("@evid", evt.EventId);
                        using (var reader = cmdDp.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                evt.DataPoints.Add(new ModbusDataPoint
                                {
                                    Key = reader["DataKey"].ToString(),
                                    Address = (ushort)Convert.ToInt32(reader["Address"]),
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

    public class ModbusEventConfig
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public ushort TriggerAddr { get; set; }
        public ushort AckAddr { get; set; }
        public string HandshakeType { get; set; }
        public int BitOffset { get; set; }
        public List<ModbusDataPoint> DataPoints { get; set; } = new List<ModbusDataPoint>();
    }

    public class ModbusDataPoint
    {
        public string Key { get; set; }
        public ushort Address { get; set; }
        public ushort Length { get; set; }
        public string DataType { get; set; }
        public string AccessType { get; set; }
    }
}
