using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAPDashboard.Models;
using EAPDashboard.Models.Smt;
using CyntecMqttLib.Models;
using CyntecMqttLib.Interfaces;

namespace EAPDashboard.Services
{
    /// <summary>
    /// SMT 多線別集中管理服務 (管理 SMT1, SMT2 等所有線別實例)
    /// </summary>
    public class SmtLineManager
    {
        private readonly ConcurrentDictionary<string, SmtLineService> _lines = new ConcurrentDictionary<string, SmtLineService>(StringComparer.OrdinalIgnoreCase);
        private MqttBus _mqttBus;

        public event Action<LogLevel, string> LogReceived;

        public SmtLineManager()
        {
        }

        public void InitializeLines(IEnumerable<LineConfig> configs, MqttBus bus = null)
        {
            _mqttBus = bus;
            _lines.Clear();

            if (configs == null) return;

            foreach (var cfg in configs.Where(c => c.IsActive && string.Equals(c.LineType, LineTypes.SMT, StringComparison.OrdinalIgnoreCase)))
            {
                var lineService = new SmtLineService(cfg);
                lineService.LogReceived += (level, msg) => LogReceived?.Invoke(level, msg);
                _lines[cfg.LineId] = lineService;

                OnLog(LogLevel.Info, $"[SMT LineManager] Registered line: {cfg.LineId} ({cfg.LineName}), Capacity={cfg.BatchCapacity}");

                // 註冊 MQTT 訂閱 (若有提供 bus)
                if (_mqttBus != null)
                {
                    SubscribeLineMqtt(lineService);
                }
            }
        }

        public void AttachMqttBus(MqttBus bus)
        {
            _mqttBus = bus;
            if (_mqttBus == null) return;

            foreach (var line in _lines.Values)
            {
                SubscribeLineMqtt(line);
            }
        }

        private void SubscribeLineMqtt(SmtLineService line)
        {
            var cfg = line.Config;

            // 1. Station 1 主題訂閱
            string st1InTopic = !string.IsNullOrWhiteSpace(cfg.Station1?.PanelInTopic) 
                ? cfg.Station1.PanelInTopic 
                : $"TN2/SMT/{cfg.LineId}/Station1/PanelIn";
            string st1OutTopic = !string.IsNullOrWhiteSpace(cfg.Station1?.PanelOutTopic) 
                ? cfg.Station1.PanelOutTopic 
                : $"TN2/SMT/{cfg.LineId}/Station1/PanelOut";

            _mqttBus.SubscribeAsync(st1InTopic, async (topic, payload) =>
            {
                string panelId = ExtractPanelIdFromPayload(payload);
                await line.HandleStation1PanelInAsync(panelId);
            });

            _mqttBus.SubscribeAsync(st1OutTopic, async (topic, payload) =>
            {
                string panelId = ExtractPanelIdFromPayload(payload);
                await line.HandleStation1PanelOutAsync(panelId);
            });

            // 2. Station 2 主題訂閱
            string st2InTopic = !string.IsNullOrWhiteSpace(cfg.Station2?.PanelInTopic) 
                ? cfg.Station2.PanelInTopic 
                : $"TN2/SMT/{cfg.LineId}/Station2/PanelIn";
            string st2OutTopic = !string.IsNullOrWhiteSpace(cfg.Station2?.PanelOutTopic) 
                ? cfg.Station2.PanelOutTopic 
                : $"TN2/SMT/{cfg.LineId}/Station2/PanelOut";

            _mqttBus.SubscribeAsync(st2InTopic, async (topic, payload) =>
            {
                string panelId = ExtractPanelIdFromPayload(payload);
                await line.HandleStation2PanelInAsync(panelId);
            });

            _mqttBus.SubscribeAsync(st2OutTopic, async (topic, payload) =>
            {
                string panelId = ExtractPanelIdFromPayload(payload);
                await line.HandleStation2PanelOutAsync(panelId);
            });

            OnLog(LogLevel.Info, $"[SMT LineManager] Subscribed MQTT topics for {cfg.LineId}: St1 In={st1InTopic}, Out={st1OutTopic}; St2 In={st2InTopic}, Out={st2OutTopic}");
        }

        /// <summary>
        /// 從 MQTT Payload (純字串或 JSON) 解析 Panel ID
        /// </summary>
        public static string ExtractPanelIdFromPayload(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return string.Empty;
            string clean = payload.Trim();
            if (clean.StartsWith("{") && clean.EndsWith("}"))
            {
                try
                {
                    var jo = Newtonsoft.Json.Linq.JObject.Parse(clean);
                    return jo["PanelID"]?.ToString() ?? jo["PanelId"]?.ToString() ?? jo["Barcode"]?.ToString() ?? clean;
                }
                catch { }
            }
            return clean;
        }

        public SmtLineService GetLine(string lineId)
        {
            if (string.IsNullOrWhiteSpace(lineId)) return null;
            _lines.TryGetValue(lineId, out var service);
            return service;
        }

        public List<SmtLineService> GetAllLines()
        {
            return _lines.Values.ToList();
        }

        /// <summary>
        /// 取得所有線別之最新狀態摘要 (供 WebUI 查詢)
        /// </summary>
        public List<SmtLineStatusSummary> GetAllLineStatuses()
        {
            var summaries = new List<SmtLineStatusSummary>();
            foreach (var line in _lines.Values)
            {
                var (st1Wo, st2Wo) = line.GetCurrentLastWos();
                var activeWos = line.GetCurrentWorkOrders();

                summaries.Add(new SmtLineStatusSummary
                {
                    LineId = line.Config.LineId,
                    LineName = line.Config.LineName,
                    IsActive = line.Config.IsActive,
                    BatchCapacity = line.Config.BatchCapacity,
                    Station1_CurrentWO = st1Wo,
                    Station2_CurrentWO = st2Wo,
                    ActiveWorkOrders = activeWos
                });
            }
            return summaries;
        }

        private void OnLog(LogLevel level, string msg)
        {
            LogReceived?.Invoke(level, msg);
            System.Diagnostics.Debug.WriteLine($"[{level}] {msg}");
        }
    }
}
