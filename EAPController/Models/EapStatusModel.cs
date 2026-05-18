using System;
using System.Collections.Generic;

namespace EAPController.Models
{
    public class EapStatusModel
    {
        public string EapId { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public string Ip { get; set; }
        public bool HasException { get; set; }
        public Dictionary<string, string> DeviceStatus { get; set; }
        public string Timestamp { get; set; }
        
        // 輔助屬性：用於 UI 判定是否紅燈 (逾時)
        public DateTime LastUpdated { get; set; }

        public EapStatusModel()
        {
            LastUpdated = DateTime.Now;
        }
    }

    public class ControllerConfig
    {
        public string MqttBrokerHost { get; set; }
        public int MqttBrokerPort { get; set; }
        public string ControllerTopic { get; set; }
        public List<string> MonitoredEaps { get; set; }
    }
}
