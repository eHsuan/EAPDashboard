using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CyntecMqttLib.Contracts;
using CyntecMqttLib.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Core.Base;
using Protocol.Core.Enums;
using Protocol.Core.Factory;
using Protocol.Core.Messages;
using Protocol.Drivers.Secs;

namespace MESSimulator.Services
{
    public class EapLogicRuleItem
    {
        public string RuleKey { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string TriggerCommand { get; set; } = string.Empty;
        public string ReplyCommand { get; set; } = string.Empty;
        public bool AutoReply { get; set; } = true;
        public bool ForceFail { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }

    public class EapSimulatorService : IDisposable
    {
        private MqttBus _bus;
        private readonly Func<string, string> _internalMesHandler;
        private readonly ConcurrentDictionary<string, EapLogicRuleItem> _rules = new ConcurrentDictionary<string, EapLogicRuleItem>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, SecsDictionary> _dictionaries = new ConcurrentDictionary<string, SecsDictionary>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, Dictionary<string, string>> _alarmMappings = new ConcurrentDictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, Dictionary<string, string>> _managementItems = new ConcurrentDictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        private class TrackInContext
        {
            public string Machine { get; set; } = string.Empty;
            public string CommandTopic { get; set; } = string.Empty;
            public List<string> WorkOrder { get; set; } = new List<string>();
            public string RecipeID { get; set; } = string.Empty;
            public string UserID { get; set; } = string.Empty;
            public List<string> CassetteID { get; set; } = new List<string>();
            public DateTime Timestamp { get; set; }
            public string TransactionID { get; set; } = string.Empty;
        }

        private readonly ConcurrentDictionary<string, TrackInContext> _pendingTrackIn = new ConcurrentDictionary<string, TrackInContext>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, List<string>> _trackInRequireInputMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, List<string>> _trackOutRequireInputMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, string> _lastStatusCodeMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, string> _lastCommandTopicMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _processedMessageIds = new HashSet<string>();
        private readonly object _msgIdLock = new object();

        private CancellationTokenSource _cts;

        public const string DefaultEquipmentId = "TEST01";
        public bool IsConnected => _bus != null && _bus.IsConnected;
        public bool IsRunning => IsConnected;
        public string DefaultTopicPrefix { get; set; } = "TN2/YELLOW/machine";

        public event Action<string> LogMessage;
        public event Action<string> KnownMachineDiscovered;
        public event Action KnownEquipmentsChanged;

        private static readonly JsonSerializerSettings _logSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        // 邏輯項目常數 Key
        public const string RULE_ARE_YOU_THERE = "AreYouThere";
        public const string RULE_STATUS_CHANGE = "StatusChangeReport";
        public const string RULE_ALARM_REPORT = "AlarmReport";
        public const string RULE_TRACK_IN_PP_SELECT = "TrackIn_PPSelect";
        public const string RULE_TRACK_IN_FINAL = "TrackIn_FinalReply";
        public const string RULE_TRACK_OUT = "TrackOutReq";
        public const string RULE_PROCESS_DATA = "ProcessDataReport";
        public const string RULE_TRACE_DATA = "RtnTraceData";
        public const string RULE_EVENT_USER_VERIFY = "Event_UserVerify";
        public const string RULE_EVENT_DEFAULT = "Event_Default";

        public EapSimulatorService(Func<string, string> internalMesHandler)
        {
            _internalMesHandler = internalMesHandler ?? throw new ArgumentNullException(nameof(internalMesHandler));
            InitializeDefaultRules();
        }

        private void InitializeDefaultRules()
        {
            var defaultList = new List<EapLogicRuleItem>
            {
                new EapLogicRuleItem
                {
                    RuleKey = RULE_ARE_YOU_THERE,
                    DisplayName = "1. 心跳確認 (AreYouThere)",
                    TriggerCommand = "AreYouThere",
                    ReplyCommand = "IamHere",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到機台 AreYouThere 心跳時，自動回復 IamHere (PASS)"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_STATUS_CHANGE,
                    DisplayName = "2. 機台狀態變更上報 (StatusChangeReport)",
                    TriggerCommand = "StatusChangeReport",
                    ReplyCommand = "ReplyStatusChangeReport",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到狀態變更時，同步轉拋 MES EQPSTATUS 並自動回復 ReplyStatusChangeReport"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_ALARM_REPORT,
                    DisplayName = "3. 機台警報上報 (AlarmReport)",
                    TriggerCommand = "AlarmReport",
                    ReplyCommand = "ReplyALARM",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到 Alarm 發生/解除時，轉換字典警報碼轉拋 MES EQPSTATUS 並回復 ReplyALARM"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_TRACK_IN_PP_SELECT,
                    DisplayName = "4. 進站階段一：工單查詢與配方切換 (TrackInReq -> PP_SELECT)",
                    TriggerCommand = "TrackInReq",
                    ReplyCommand = "RemoteCMD(PP_SELECT) / ReplyTrackInReq(FAIL)",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到 TrackInReq 後查 MES WOQRY (防重複進站、比對 Recipe)，通過後自動下發 RemoteCMD(PP_SELECT)；若不勾選但勾選階段二則跳過 PP_SELECT 直接進站"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_TRACK_IN_FINAL,
                    DisplayName = "5. 進站階段二：配方切換確認與進站過帳 (ReplyRemoteCMD -> ReplyTrackInReq)",
                    TriggerCommand = "ReplyRemoteCMD(PP_SELECT)",
                    ReplyCommand = "ReplyTrackInReq",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到機台 ReplyRemoteCMD(PP_SELECT) PASS 後，自動呼叫 MES WOCHECKIN 並回復 ReplyTrackInReq"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_TRACK_OUT,
                    DisplayName = "6. 出站過帳請求 (TrackOutReq)",
                    TriggerCommand = "TrackOutReq",
                    ReplyCommand = "ReplyTrackOutReq",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到機台 TrackOutReq 後呼叫 MES WOCHECKOUT (自動補齊字典出站管理項目) 並回復 ReplyTrackOutReq"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_PROCESS_DATA,
                    DisplayName = "7. 製程數據上報 (ProcessDataReport)",
                    TriggerCommand = "ProcessDataReport",
                    ReplyCommand = "ReplyProcessDataReport",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到機台製程生產數據上報時，自動回復 ReplyProcessDataReport"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_TRACE_DATA,
                    DisplayName = "8. 即時 Trace 數據上報 (RtnTraceData)",
                    TriggerCommand = "RtnTraceData",
                    ReplyCommand = "ReplyRtnTraceData",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到機台定期回傳之 RtnTraceData 時，自動回復 ReplyRtnTraceData"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_EVENT_USER_VERIFY,
                    DisplayName = "9. 事件上報：人員驗證 (EventReport: USER_VERIFY)",
                    TriggerCommand = "EventReport (USER_VERIFY)",
                    ReplyCommand = "ReplyEventReport",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "解析條碼或工號密碼，向 MES 發送 UserVerify 驗證後自動回復 ReplyEventReport"
                },
                new EapLogicRuleItem
                {
                    RuleKey = RULE_EVENT_DEFAULT,
                    DisplayName = "10. 事件上報：其他一般事件 (EventReport: Default)",
                    TriggerCommand = "EventReport (Other CEIDs)",
                    ReplyCommand = "ReplyEventReport",
                    AutoReply = true,
                    ForceFail = false,
                    Description = "收到其他自訂或未知 CEID 事件時，預設自動回復 ReplyEventReport (PASS)"
                }
            };

            foreach (var item in defaultList)
            {
                _rules[item.RuleKey] = item;
            }
        }

        public List<EapLogicRuleItem> Rules => GetRulesList();

        public List<EapLogicRuleItem> GetRulesList()
        {
            return _rules.Values.OrderBy(r =>
            {
                int dotIdx = r.DisplayName.IndexOf('.');
                if (dotIdx > 0 && int.TryParse(r.DisplayName.Substring(0, dotIdx), out int num))
                    return num;
                return 999;
            }).ToList();
        }

        public List<string> GetKnownEquipments()
        {
            var list = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { DefaultEquipmentId };
            foreach (var k in _lastCommandTopicMap.Keys)
            {
                if (!string.IsNullOrWhiteSpace(k)) list.Add(k);
            }
            return list.OrderBy(x => x).ToList();
        }

        public static string ExtractMachineIdFromCommandTopic(string cmdTopic)
        {
            if (string.IsNullOrWhiteSpace(cmdTopic)) return DefaultEquipmentId;
            var parts = cmdTopic.Trim().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 && parts[parts.Length - 1].Equals("Command", StringComparison.OrdinalIgnoreCase))
            {
                return parts[parts.Length - 2];
            }
            if (parts.Length >= 1 && !parts[parts.Length - 1].Equals("Command", StringComparison.OrdinalIgnoreCase))
            {
                return parts[parts.Length - 1];
            }
            return DefaultEquipmentId;
        }

        public bool IsRuleAutoReply(string ruleKey)
        {
            return _rules.TryGetValue(ruleKey, out var rule) && rule.AutoReply;
        }

        public bool IsRuleForceFail(string ruleKey)
        {
            return _rules.TryGetValue(ruleKey, out var rule) && rule.ForceFail;
        }

        public async Task StartAsync(string brokerHost = "127.0.0.1", int brokerPort = 1883)
        {
            if (IsConnected) return;

            _cts = new CancellationTokenSource();
            string clientId = $"EAP_Simulator_Internal_{Guid.NewGuid().ToString("N").Substring(0, 6)}";
            _bus = new MqttBus(brokerHost, brokerPort, clientId, cleanSession: true);

            await _bus.ConnectAsync();

            // 訂閱萬用字元以攔截所有設備商機台發送上來的 Report Topic (支援單層與多層路徑)
            await _bus.SubscribeAsync("#", (topic, payload) =>
            {
                if (topic != null && topic.EndsWith("/Report", StringComparison.OrdinalIgnoreCase))
                {
                    _ = Task.Run(() => HandleEquipmentReportAsync(topic, payload));
                }
            });

            _ = Task.Run(() => MonitorTrackInTimeoutLoop(_cts.Token));

            Log($"EAP 模擬器已連線至 Broker ({brokerHost}:{brokerPort})，ClientID={clientId}，監聽所有 */Report 頻道。");
        }

        public async Task StopAsync()
        {
            _cts?.Cancel();
            if (_bus != null)
            {
                try
                {
                    await _bus.DisconnectAsync();
                    _bus.Dispose();
                }
                catch { }
                _bus = null;
            }
            Log("EAP 模擬器已停止。");
        }

        private async Task MonitorTrackInTimeoutLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    foreach (var kvp in _pendingTrackIn.ToArray())
                    {
                        var ctx = kvp.Value;
                        if ((DateTime.Now - ctx.Timestamp).TotalSeconds > 30)
                        {
                            if (_pendingTrackIn.TryRemove(kvp.Key, out _))
                            {
                                Log($"[TrackIn] 機台 {ctx.Machine} 等待 ReplyRemoteCMD (PP_SELECT) 逾時 (30s)，自動回復 ReplyTrackInReq (FAIL)。");
                                if (IsRuleAutoReply(RULE_TRACK_IN_FINAL))
                                {
                                    var failReply = new ReplyTrackInReqMessage
                                    {
                                        CMD = CommandType.ReplyTrackInReq,
                                        Machine = ctx.Machine,
                                        TransactionID = ctx.TransactionID,
                                        WorkOrder = ctx.WorkOrder,
                                        CassetteID = ctx.CassetteID,
                                        UserID = ctx.UserID,
                                        RtnResult = RtnResult.FAIL,
                                        RtnMsg = "Timeout waiting for machine to change recipe (30s)."
                                    };
                                    await SendCommandToTopicAsync(ctx.CommandTopic, failReply);
                                }
                            }
                        }
                    }
                }
                catch { }
                await Task.Delay(3000, ct);
            }
        }

        public string ResolveCommandTopic(string reportTopic, string machineId)
        {
            if (string.IsNullOrEmpty(machineId) && !string.IsNullOrEmpty(reportTopic) && !reportTopic.Contains("/"))
            {
                machineId = reportTopic;
                reportTopic = null;
            }

            if (!string.IsNullOrEmpty(reportTopic) && reportTopic.EndsWith("/Report", StringComparison.OrdinalIgnoreCase))
            {
                string baseTopic = reportTopic.Substring(0, reportTopic.Length - "/Report".Length);
                string cmdTopic = $"{baseTopic}/Command";
                if (!string.IsNullOrEmpty(machineId))
                {
                    _lastCommandTopicMap[machineId] = cmdTopic;
                }
                return cmdTopic;
            }

            if (!string.IsNullOrEmpty(machineId) && _lastCommandTopicMap.TryGetValue(machineId, out var cachedTopic))
            {
                return cachedTopic;
            }

            string prefix = string.IsNullOrWhiteSpace(DefaultTopicPrefix) ? "TN2/YELLOW/machine" : DefaultTopicPrefix.TrimEnd('/');
            return $"{prefix}/{machineId}/Command";
        }

        private SecsDictionary GetOrLoadDictionary(string eqpId)
        {
            string cleanId = eqpId?.Trim() ?? "DEFAULT";
            return _dictionaries.GetOrAdd(cleanId, id =>
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] candidates = new[]
                {
                    Path.Combine(baseDir, "Dictionaries", $"{id}.json"),
                    Path.Combine(baseDir, "Dictionaries", "MQTT_Standard.json")
                };

                foreach (var path in candidates)
                {
                    if (File.Exists(path))
                    {
                        try
                        {
                            LoadExtraDictionaryMappings(id, path);
                            var dict = SecsDictionary.Load(path);
                            Log($"[Dictionary] 已為機台 '{id}' 載入字典檔: {Path.GetFileName(path)}");
                            return dict;
                        }
                        catch { }
                    }
                }
                return new SecsDictionary();
            });
        }

        private void LoadExtraDictionaryMappings(string eqpId, string fullPath)
        {
            try
            {
                string json = File.ReadAllText(fullPath);
                var jObj = JObject.Parse(json);

                var alarmMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var alarmToken = jObj["AlarmMapping"] ?? jObj["Alarms"];
                if (alarmToken is JObject almObj)
                {
                    foreach (var prop in almObj.Properties())
                    {
                        alarmMap[prop.Name] = prop.Value?.ToString() ?? "";
                    }
                }
                _alarmMappings[eqpId] = alarmMap;

                var mgmtMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                if (jObj["ManagementItems"] is JObject mgmtObj)
                {
                    foreach (var prop in mgmtObj.Properties())
                    {
                        mgmtMap[prop.Name.Trim()] = prop.Value?.ToString() ?? "";
                    }
                }
                _managementItems[eqpId] = mgmtMap;
            }
            catch { }
        }

        private string TransformAlarmCode(string eqpId, string rawCode)
        {
            if (string.IsNullOrEmpty(rawCode)) return rawCode ?? "0";
            GetOrLoadDictionary(eqpId);
            if (_alarmMappings.TryGetValue(eqpId ?? "", out var map) && map != null && map.TryGetValue(rawCode, out string mapped))
            {
                return mapped;
            }
            return rawCode;
        }

        private string GetManagementItemValue(string eqpId, string inputCode)
        {
            if (string.IsNullOrEmpty(inputCode)) return null;
            GetOrLoadDictionary(eqpId);
            if (_managementItems.TryGetValue(eqpId ?? "", out var map) && map != null && map.TryGetValue(inputCode.Trim(), out string val))
            {
                return val;
            }
            return "SIM_VAL"; // 若字典無設定，在模擬器環境提供預設值以利設備商順暢測試
        }

        private async Task HandleEquipmentReportAsync(string reportTopic, string rawPayload)
        {
            if (string.IsNullOrWhiteSpace(rawPayload)) return;

            try
            {
                string messageId = null;
                string innerJson = null;

                var jObj = JObject.Parse(rawPayload);
                if (jObj.TryGetValue("Payload", StringComparison.OrdinalIgnoreCase, out var payloadToken) && payloadToken != null && payloadToken.Type != JTokenType.Null)
                {
                    messageId = jObj["MessageId"]?.ToString();
                    innerJson = payloadToken.Type == JTokenType.String ? payloadToken.ToString() : payloadToken.ToString(Formatting.None);
                }
                else
                {
                    messageId = jObj["MessageId"]?.ToString();
                    innerJson = rawPayload;
                }

                if (string.IsNullOrWhiteSpace(innerJson)) return;

                if (!string.IsNullOrEmpty(messageId))
                {
                    lock (_msgIdLock)
                    {
                        if (_processedMessageIds.Contains(messageId)) return;
                        _processedMessageIds.Add(messageId);
                        if (_processedMessageIds.Count > 1000) _processedMessageIds.Remove(_processedMessageIds.First());
                    }
                }

                BaseMessage msg = MessageFactory.Create(innerJson);
                string eqpId = msg.Machine;
                if (string.IsNullOrWhiteSpace(eqpId))
                {
                    // 從 Topic 推斷機台代號: prefix/{eqpId}/Report
                    var parts = reportTopic.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2) eqpId = parts[parts.Length - 2];
                    else eqpId = "EQP_SIM";
                }

                bool isNewEqp = !_lastCommandTopicMap.ContainsKey(eqpId);
                string cmdTopic = ResolveCommandTopic(reportTopic, eqpId);
                KnownMachineDiscovered?.Invoke(eqpId);
                if (isNewEqp)
                {
                    KnownEquipmentsChanged?.Invoke();
                }

                Log($"[EAP RECV] Topic={reportTopic} | Machine={eqpId} | CMD={msg.CMD}\n{JsonConvert.SerializeObject(msg, _logSettings)}");

                switch (msg.CMD)
                {
                    case CommandType.AreYouThere:
                        if (!IsRuleAutoReply(RULE_ARE_YOU_THERE))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_ARE_YOU_THERE}' 未勾選自動回復。");
                            break;
                        }
                        var iamHere = new IamHereMessage
                        {
                            CMD = CommandType.IamHere,
                            RtnResult = IsRuleForceFail(RULE_ARE_YOU_THERE) ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = IsRuleForceFail(RULE_ARE_YOU_THERE) ? "Simulated FAIL" : "OK"
                        };
                        await SendReplyAsync(cmdTopic, iamHere, msg);
                        break;

                    case CommandType.IamHere:
                        Log($"[Heartbeat] 收到機台 {eqpId} 的 IamHere 回應。");
                        break;

                    case CommandType.ReplyStatusReq:
                        var rsr = (ReplyStatusReqMessage)msg;
                        string curSt = int.TryParse(rsr.CurrentStatus, out int sv) ? sv.ToString() : (rsr.CurrentStatus ?? "0");
                        _lastStatusCodeMap[eqpId] = curSt;
                        CallInternalMes("EQPSTATUS", new JObject
                        {
                            ["EqpNo"] = eqpId,
                            ["StatusCode"] = curSt,
                            ["ErrorCode"] = string.IsNullOrEmpty(rsr.AlarmCode) ? "0" : TransformAlarmCode(eqpId, rsr.AlarmCode)
                        });
                        Log($"[StatusReq Reply] 機台 {eqpId} 回報目前狀態: {rsr.CurrentStatus}, Alarm: {rsr.AlarmCode ?? "None"}");
                        break;

                    case CommandType.StatusChangeReport:
                        var scm = (StatusChangeReportMessage)msg;
                        string newSt = int.TryParse(scm.NewStatus, out int nval) ? nval.ToString() : (scm.NewStatus ?? "0");
                        _lastStatusCodeMap[eqpId] = newSt;

                        CallInternalMes("EQPSTATUS", new JObject
                        {
                            ["EqpNo"] = eqpId,
                            ["StatusCode"] = newSt
                        });

                        if (!IsRuleAutoReply(RULE_STATUS_CHANGE))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_STATUS_CHANGE}' 未勾選自動回復。");
                            break;
                        }

                        bool scFail = IsRuleForceFail(RULE_STATUS_CHANGE);
                        await SendReplyAsync(cmdTopic, new ReplyStatusChangeReportMessage
                        {
                            CMD = CommandType.ReplyStatusChangeReport,
                            RtnResult = scFail ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = scFail ? "[EAP Sim] Forced FAIL for StatusChangeReport" : "OK"
                        }, msg);
                        break;

                    case CommandType.AlarmReport:
                        var alm = (AlarmMessage)msg;
                        string mappedCode = TransformAlarmCode(eqpId, alm.AlarmCode);
                        bool isAlarmSet = string.IsNullOrEmpty(alm.AlarmStatus) || alm.AlarmStatus.Equals("Start", StringComparison.OrdinalIgnoreCase) || alm.AlarmStatus == "1";

                        CallInternalMes("EQPSTATUS", new JObject
                        {
                            ["EqpNo"] = eqpId,
                            ["StatusCode"] = isAlarmSet ? "1" : "2",
                            ["ErrorCode"] = mappedCode
                        });

                        if (!IsRuleAutoReply(RULE_ALARM_REPORT))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_ALARM_REPORT}' 未勾選自動回復。");
                            break;
                        }

                        bool almFail = IsRuleForceFail(RULE_ALARM_REPORT);
                        await SendReplyAsync(cmdTopic, new ReplyAlarmMessage
                        {
                            CMD = CommandType.ReplyALARM,
                            RtnResult = almFail ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = almFail ? "[EAP Sim] Forced FAIL for AlarmReport" : "OK"
                        }, msg);
                        break;

                    case CommandType.TrackInReq:
                        var ti = (TrackInReqMessage)msg;
                        await HandleTrackInReqAsync(eqpId, cmdTopic, ti);
                        break;

                    case CommandType.ReplyRemoteCMD:
                        var rrm = (ReplyRemoteCMDMessage)msg;
                        await HandleReplyRemoteCmdAsync(eqpId, cmdTopic, rrm);
                        break;

                    case CommandType.TrackOutReq:
                        var to = (TrackOutReqMessage)msg;
                        await HandleTrackOutReqAsync(eqpId, cmdTopic, to);
                        break;

                    case CommandType.ProcessDataReport:
                        if (!IsRuleAutoReply(RULE_PROCESS_DATA))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_PROCESS_DATA}' 未勾選自動回復。");
                            break;
                        }
                        bool pdrFail = IsRuleForceFail(RULE_PROCESS_DATA);
                        await SendReplyAsync(cmdTopic, new ReplyProcessDataReportMessage
                        {
                            CMD = CommandType.ReplyProcessDataReport,
                            RtnResult = pdrFail ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = pdrFail ? "[EAP Sim] Forced FAIL for ProcessDataReport" : "OK"
                        }, msg);
                        break;

                    case CommandType.RtnTraceData:
                        if (!IsRuleAutoReply(RULE_TRACE_DATA))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_TRACE_DATA}' 未勾選自動回復。");
                            break;
                        }
                        bool rtdFail = IsRuleForceFail(RULE_TRACE_DATA);
                        await SendReplyAsync(cmdTopic, new ReplyRtnTraceDataMessage
                        {
                            CMD = CommandType.ReplyRtnTraceData,
                            RtnResult = rtdFail ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = rtdFail ? "[EAP Sim] Forced FAIL for RtnTraceData" : "Trace data received successfully"
                        }, msg);
                        break;

                    case CommandType.EventReport:
                        var evm = (EventReportMessage)msg;
                        await HandleEventReportAsync(eqpId, cmdTopic, evm);
                        break;

                    case CommandType.ReplyInitTraceData:
                        var ritd = (ReplyInitTraceDataMessage)msg;
                        Log($"[InitTraceData Reply] 收到機台 {eqpId} 針對 InitTraceData 的回應: Result={ritd.RtnResult}, Msg={ritd.RtnMsg}");
                        break;

                    case CommandType.ReplyTimeCalibrate:
                    case CommandType.ReplyTerminalDisplay:
                    case CommandType.ReplySelectParameter:
                    case CommandType.ReplyEditParameter:
                        Log($"[ACK] 收到機台 {eqpId} 針對 {msg.CMD} 的回應。");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"[ERR] 處理機台上報訊息失敗: {ex.Message}");
            }
        }

        private async Task HandleTrackInReqAsync(string eqpId, string cmdTopic, TrackInReqMessage ti)
        {
            List<string> wos = ti.WorkOrder ?? new List<string>();
            List<string> cids = ti.CassetteID ?? new List<string>();

            // 若強制設定階段一失敗
            if (IsRuleForceFail(RULE_TRACK_IN_PP_SELECT))
            {
                if (IsRuleAutoReply(RULE_TRACK_IN_PP_SELECT))
                {
                    await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                    {
                        CMD = CommandType.ReplyTrackInReq,
                        WorkOrder = wos,
                        CassetteID = cids,
                        UserID = ti.UserID,
                        RtnResult = RtnResult.FAIL,
                        RtnMsg = "[EAP Sim] Forced FAIL on TrackIn Stage 1"
                    }, ti);
                }
                return;
            }

            string tir = ProcessTrackInGeneral(eqpId, cids, wos, ti.UserID, out string terminalErr);
            if (!string.IsNullOrEmpty(terminalErr))
            {
                await SendTerminalDisplayAsync(eqpId, cmdTopic, terminalErr);
            }

            if (tir.StartsWith("PP-SELECT"))
            {
                string[] tp = tir.Split('|');
                string finalRecipe = tp[2];

                if (IsRuleAutoReply(RULE_TRACK_IN_PP_SELECT))
                {
                    _pendingTrackIn[eqpId] = new TrackInContext
                    {
                        Machine = eqpId,
                        CommandTopic = cmdTopic,
                        WorkOrder = wos,
                        RecipeID = finalRecipe,
                        UserID = ti.UserID,
                        CassetteID = cids,
                        Timestamp = DateTime.Now,
                        TransactionID = ti.TransactionID
                    };

                    var rcmd = new RemoteCMDMessage
                    {
                        CMD = CommandType.RemoteCMD,
                        Machine = eqpId,
                        TransactionID = ti.TransactionID,
                        RemoteCMDType = RemoteCommandType.PP_SELECT,
                        RecipeID = finalRecipe
                    };
                    await SendCommandToTopicAsync(cmdTopic, rcmd);
                }
                else if (IsRuleAutoReply(RULE_TRACK_IN_FINAL))
                {
                    // 若使用者取消勾選 PP_SELECT 自動下發，但保留階段二自動回復，則直接完成進站並回復 ReplyTrackInReq
                    Log($"[TrackIn] 規則 '{RULE_TRACK_IN_PP_SELECT}' 未勾選，跳過 PP_SELECT 直接執行 WOCHECKIN 並回復 ReplyTrackInReq。");
                    string fin = ProcessTrackInFinalGeneral(eqpId, wos, ti.UserID, cids, out string finTermErr);
                    if (!string.IsNullOrEmpty(finTermErr))
                    {
                        await SendTerminalDisplayAsync(eqpId, cmdTopic, finTermErr);
                    }
                    bool isPass = fin.StartsWith("PASS") && !IsRuleForceFail(RULE_TRACK_IN_FINAL);
                    await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                    {
                        CMD = CommandType.ReplyTrackInReq,
                        WorkOrder = wos,
                        CassetteID = cids,
                        UserID = ti.UserID,
                        RtnResult = isPass ? RtnResult.PASS : RtnResult.FAIL,
                        RtnMsg = isPass ? fin.Split('|').Last() : "[EAP Sim] Forced or MES FAIL on TrackIn Final"
                    }, ti);
                }
                else
                {
                    Log($"[略過自動回復] 規則 '{RULE_TRACK_IN_PP_SELECT}' 與 '{RULE_TRACK_IN_FINAL}' 皆未勾選自動回復。");
                }
            }
            else
            {
                if (!IsRuleAutoReply(RULE_TRACK_IN_PP_SELECT) && !IsRuleAutoReply(RULE_TRACK_IN_FINAL))
                {
                    Log($"[略過自動回復] TrackIn 驗證失敗，但未勾選自動回復。");
                    return;
                }

                string[] failParts = tir.Split('|');
                string failMsg = failParts.Length > 2 && !string.IsNullOrEmpty(failParts[2]) && failParts[2] != "[]"
                    ? failParts[2]
                    : failParts.Last();

                await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                {
                    CMD = CommandType.ReplyTrackInReq,
                    WorkOrder = wos,
                    CassetteID = cids,
                    UserID = ti.UserID,
                    RtnResult = RtnResult.FAIL,
                    RtnMsg = failMsg
                }, ti);
            }
        }

        private async Task HandleReplyRemoteCmdAsync(string eqpId, string cmdTopic, ReplyRemoteCMDMessage rrm)
        {
            if (rrm.RemoteCMDType == RemoteCommandType.PP_SELECT)
            {
                if (_pendingTrackIn.TryRemove(eqpId, out var ctx))
                {
                    if (!IsRuleAutoReply(RULE_TRACK_IN_FINAL))
                    {
                        Log($"[略過自動回復] 收到機台 {eqpId} ReplyRemoteCMD(PP_SELECT)，但規則 '{RULE_TRACK_IN_FINAL}' 未勾選自動回復。");
                        return;
                    }

                    if (IsRuleForceFail(RULE_TRACK_IN_FINAL))
                    {
                        await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                        {
                            CMD = CommandType.ReplyTrackInReq,
                            WorkOrder = ctx.WorkOrder,
                            CassetteID = ctx.CassetteID,
                            UserID = ctx.UserID,
                            RtnResult = RtnResult.FAIL,
                            RtnMsg = "[EAP Sim] Forced FAIL on TrackIn Stage 2 (ReplyTrackInReq)"
                        }, rrm);
                        return;
                    }

                    if (rrm.RtnResult == RtnResult.PASS)
                    {
                        string fin = ProcessTrackInFinalGeneral(eqpId, ctx.WorkOrder, ctx.UserID, ctx.CassetteID, out string termErr);
                        if (!string.IsNullOrEmpty(termErr))
                        {
                            await SendTerminalDisplayAsync(eqpId, cmdTopic, termErr);
                        }
                        await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                        {
                            CMD = CommandType.ReplyTrackInReq,
                            WorkOrder = ctx.WorkOrder,
                            CassetteID = ctx.CassetteID,
                            UserID = ctx.UserID,
                            RtnResult = fin.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL,
                            RtnMsg = fin.Split('|').Last()
                        }, rrm);
                    }
                    else
                    {
                        Log($"[TrackIn] 機台 {eqpId} 拒絕 PP_SELECT: {rrm.RtnMsg}");
                        await SendReplyAsync(cmdTopic, new ReplyTrackInReqMessage
                        {
                            CMD = CommandType.ReplyTrackInReq,
                            WorkOrder = ctx.WorkOrder,
                            CassetteID = ctx.CassetteID,
                            UserID = ctx.UserID,
                            RtnResult = RtnResult.FAIL,
                            RtnMsg = $"Recipe Change Failed: {rrm.RtnMsg}"
                        }, rrm);
                    }
                }
                else
                {
                    Log($"[RemoteCMD] 收到機台 {eqpId} ReplyRemoteCMD(PP_SELECT) ({rrm.RtnResult})，無待處理之 TrackIn 上下文 (可能為手動觸發或已逾時)。");
                }
            }
            else
            {
                Log($"[RemoteCMD] 收到機台 {eqpId} ReplyRemoteCMD({rrm.RemoteCMDType}): Result={rrm.RtnResult}, Msg={rrm.RtnMsg}");
            }
        }

        private async Task HandleTrackOutReqAsync(string eqpId, string cmdTopic, TrackOutReqMessage to)
        {
            List<string> toWos = to.WorkOrder ?? new List<string>();
            List<string> toCassettes = to.CassetteID ?? new List<string>();
            string torQty = string.IsNullOrWhiteSpace(to.Qty) ? "0" : to.Qty.Trim();

            string tor = ProcessTrackOutGeneral(eqpId, toWos, torQty, to.UserID, toCassettes, out string termErr);
            if (!string.IsNullOrEmpty(termErr))
            {
                await SendTerminalDisplayAsync(eqpId, cmdTopic, termErr);
            }

            if (!IsRuleAutoReply(RULE_TRACK_OUT))
            {
                Log($"[略過自動回復] 規則 '{RULE_TRACK_OUT}' 未勾選自動回復。");
                return;
            }

            if (IsRuleForceFail(RULE_TRACK_OUT))
            {
                await SendReplyAsync(cmdTopic, new ReplyTrackOutReqMessage
                {
                    CMD = CommandType.ReplyTrackOutReq,
                    WorkOrder = toWos,
                    CassetteID = toCassettes,
                    UserID = to.UserID,
                    RtnResult = RtnResult.FAIL,
                    RtnMsg = "[EAP Sim] Forced FAIL for TrackOutReq"
                }, to);
                return;
            }

            string[] torParts = tor.Split('|');
            string rtnMsg = torParts.Length > 2 ? torParts[2] : (torParts.Length > 1 ? torParts[1] : "");
            await SendReplyAsync(cmdTopic, new ReplyTrackOutReqMessage
            {
                CMD = CommandType.ReplyTrackOutReq,
                WorkOrder = toWos,
                CassetteID = toCassettes,
                UserID = to.UserID,
                RtnResult = tor.StartsWith("PASS") ? RtnResult.PASS : RtnResult.FAIL,
                RtnMsg = rtnMsg
            }, to);
        }

        private async Task HandleEventReportAsync(string eqpId, string cmdTopic, EventReportMessage evm)
        {
            var dict = GetOrLoadDictionary(eqpId);
            string rawEventId = evm.EventID ?? "";
            string logicalEvent = dict?.GetEventName(rawEventId) ?? rawEventId;
            string upperEvent = logicalEvent.ToUpperInvariant();
            if (upperEvent.StartsWith("UNKNOWN_CEID_"))
            {
                upperEvent = upperEvent.Substring("UNKNOWN_CEID_".Length);
            }

            Log($"[EVENT] 機台={eqpId}, EventID={rawEventId} (映射名稱={logicalEvent})");

            string GetVal(string logicalName)
            {
                if (evm.Data == null) return null;
                return evm.Data.FirstOrDefault(x =>
                    string.Equals(x.Parameter, logicalName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(dict?.GetVariableName(x.Parameter) ?? x.Parameter, logicalName, StringComparison.OrdinalIgnoreCase) ||
                    (dict?.Variables != null && dict.Variables.Any(kvp => kvp.Value.Equals(logicalName, StringComparison.OrdinalIgnoreCase) && kvp.Key.Equals(x.Parameter, StringComparison.OrdinalIgnoreCase)))
                )?.Value;
            }

            switch (upperEvent)
            {
                case "USER_VERIFY":
                case "USERVERIFY":
                case "USERIDVERIFY":
                    {
                        string userBarcode = GetVal("User_Barcode") ?? GetVal("Barcode");
                        string userId = GetVal("UserID") ?? GetVal("User_ID");
                        string userPassword = GetVal("UserPassword") ?? GetVal("User_Password");

                        if (!string.IsNullOrEmpty(userBarcode))
                        {
                            userId = null;
                            userPassword = null;
                        }

                        var mesReply = CallInternalMes("UserVerify", new JObject
                        {
                            ["EqpNo"] = eqpId,
                            ["User2DBarCode"] = userBarcode ?? "",
                            ["UserID"] = userId ?? "",
                            ["UserPassword"] = userPassword ?? ""
                        });

                        if (!IsRuleAutoReply(RULE_EVENT_USER_VERIFY))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_EVENT_USER_VERIFY}' 未勾選自動回復。");
                            break;
                        }

                        bool isPass = !IsRuleForceFail(RULE_EVENT_USER_VERIFY) && IsMesSuccess(mesReply);
                        string rtnMsg = IsRuleForceFail(RULE_EVENT_USER_VERIFY)
                            ? "[EAP Sim] Forced FAIL for USER_VERIFY"
                            : (mesReply?["Message"]?.ToString() ?? mesReply?["UserID"]?.ToString() ?? "");

                        await SendReplyAsync(cmdTopic, new ReplyEventReportMessage
                        {
                            CMD = CommandType.ReplyEventReport,
                            RtnResult = isPass ? RtnResult.PASS : RtnResult.FAIL,
                            RtnMsg = rtnMsg
                        }, evm);
                        break;
                    }

                default:
                    {
                        // 注意：依需求不包含防混料、NGCODE上報、自動結帳、線別功能，其他事件統一走一般預設事件回覆
                        if (!IsRuleAutoReply(RULE_EVENT_DEFAULT))
                        {
                            Log($"[略過自動回復] 規則 '{RULE_EVENT_DEFAULT}' 未勾選自動回復 (Event={upperEvent})。");
                            break;
                        }

                        bool defFail = IsRuleForceFail(RULE_EVENT_DEFAULT);
                        await SendReplyAsync(cmdTopic, new ReplyEventReportMessage
                        {
                            CMD = CommandType.ReplyEventReport,
                            RtnResult = defFail ? RtnResult.FAIL : RtnResult.PASS,
                            RtnMsg = defFail ? $"[EAP Sim] Forced FAIL for Event {logicalEvent}" : "OK"
                        }, evm);
                        break;
                    }
            }
        }

        // --- EAP 一般核心商務邏輯 (移植自 EquipmentBase，排除防混料、NGCODE、線別功能) ---

        private string ProcessTrackInGeneral(string eqpId, List<string> cassetteIds, List<string> workOrders, string userId, out string terminalErrorMsg)
        {
            terminalErrorMsg = null;
            var validWos = (workOrders ?? new List<string>()).Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => w.Trim()).Distinct().ToList();
            if (validWos.Count == 0)
            {
                return "FAIL|W001|WorkOrder is empty|[]";
            }

            string combinedWo = string.Join(",", validWos);
            var woRecipeMap = new Dictionary<string, string>();
            var allReqInputCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var alreadyInWos = new List<string>();
            string lastResultCode = "W001";
            string lastMessage = "";
            string lastInputParamListDT = "";

            foreach (var wo in validWos)
            {
                var qryReply = CallInternalMes("WOQRY", new JObject
                {
                    ["WONO"] = wo,
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? ""
                });

                if (!IsMesSuccess(qryReply))
                {
                    string resCode = qryReply?["ResultCode"]?.ToString() ?? "ERR_MES";
                    string failMsg = $"MES WOQRY Failed for WO {wo}: {qryReply?["Message"] ?? "No response"}";
                    terminalErrorMsg = ResolveMesFailTerminalMessage(resCode, wo, userId);
                    return $"FAIL|{resCode}|{failMsg}|[]";
                }

                string alreadyInFlag = qryReply?["AlreadyInFlag"]?.ToString()?.Trim();
                if ("Y".Equals(alreadyInFlag, StringComparison.OrdinalIgnoreCase))
                {
                    alreadyInWos.Add(wo);
                }

                lastResultCode = qryReply?["ResultCode"]?.ToString() ?? "W001";
                lastMessage = qryReply?["Message"]?.ToString() ?? "";
                lastInputParamListDT = qryReply?["InputParamListDT"]?.ToString() ?? "";

                string recipe = ExtractRecipeFromMes(lastInputParamListDT) ?? "";
                woRecipeMap[wo] = recipe;

                string inputCodeListDt = qryReply?["InputCodeListDT"]?.ToString();
                if (!string.IsNullOrWhiteSpace(inputCodeListDt))
                {
                    try
                    {
                        var codeArr = JArray.Parse(inputCodeListDt);
                        foreach (var item in codeArr)
                        {
                            if ("Y".Equals(item["RequireInput"]?.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                string code = item["InputCode"]?.ToString()?.Trim();
                                if (!string.IsNullOrEmpty(code)) allReqInputCodes.Add(code);
                            }
                        }
                    }
                    catch { }
                }
            }

            if (alreadyInWos.Count > 0)
            {
                string alreadyInList = string.Join(",", alreadyInWos);
                string errorMsg = $"{alreadyInList} 已進站，禁止重複進站";
                terminalErrorMsg = $"WO ALREADY CHECKED IN: {alreadyInList}";
                return $"FAIL|ERR_ALREADY_IN|{errorMsg}|[]";
            }

            var distinctRecipes = woRecipeMap.Values.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (distinctRecipes.Count > 1)
            {
                var details = woRecipeMap.Select((kv, idx) => $"WO_{idx + 1}:{kv.Key}[{kv.Value}]");
                string errorMsg = $"Recipe Mismatch! {string.Join(",", details)}";
                terminalErrorMsg = errorMsg;
                return $"FAIL|R001|{errorMsg}|[]";
            }

            string unifiedRecipe = distinctRecipes.FirstOrDefault() ?? "";
            if (string.IsNullOrWhiteSpace(unifiedRecipe)
                || unifiedRecipe.Equals("UNKNOWN", StringComparison.OrdinalIgnoreCase)
                || unifiedRecipe.Equals("ERROR", StringComparison.OrdinalIgnoreCase)
                || (validWos.Count == 1 && (unifiedRecipe.StartsWith("RECIPE_MISMATCH_", StringComparison.OrdinalIgnoreCase) || unifiedRecipe.StartsWith("SIM_RECIPE_DIFF_", StringComparison.OrdinalIgnoreCase))))
            {
                string errorMsg = "在MES找不到RECIPEID";
                terminalErrorMsg = "RECIPEID NOT FOUND IN MES";
                return $"FAIL|Y94|{errorMsg}|[]";
            }

            _trackInRequireInputMap[$"{eqpId}:{combinedWo}"] = allReqInputCodes.ToList();
            return $"PP-SELECT|{combinedWo}|{unifiedRecipe}|{lastResultCode}|{lastMessage}|{lastInputParamListDT}|[]";
        }

        private string ProcessTrackInFinalGeneral(string eqpId, List<string> workOrders, string userId, List<string> cassetteIds, out string terminalErrorMsg)
        {
            terminalErrorMsg = null;
            var validWos = (workOrders ?? new List<string>()).Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => w.Trim()).Distinct().ToList();
            string combinedWo = string.Join(",", validWos);
            var validCids = (cassetteIds ?? new List<string>()).Where(c => !string.IsNullOrWhiteSpace(c) && !c.Equals("N/A", StringComparison.OrdinalIgnoreCase) && !c.Equals("NA", StringComparison.OrdinalIgnoreCase)).Distinct().ToList();

            string inputListDtJson = null;
            if (_trackInRequireInputMap.TryGetValue($"{eqpId}:{combinedWo}", out var reqCodes) && reqCodes != null && reqCodes.Count > 0)
            {
                var arr = new JArray();
                foreach (var code in reqCodes)
                {
                    string val = GetManagementItemValue(eqpId, code);
                    if (!string.IsNullOrEmpty(val))
                    {
                        arr.Add(new JObject { ["InputCode"] = code, ["InputValue"] = val });
                    }
                }
                if (arr.Count > 0) inputListDtJson = arr.ToString(Formatting.None);
            }

            JObject reply;
            if (validWos.Count > 1)
            {
                var reqObj = new JObject
                {
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? "",
                    ["WoNoDt"] = JsonConvert.SerializeObject(validWos.Select(w => new { WoNo = w }).ToList())
                };
                if (validCids.Count > 0)
                {
                    reqObj["WOCarrierDt"] = JsonConvert.SerializeObject(validCids.Select(c => new { CarrierNo = c }).ToList());
                }
                if (!string.IsNullOrEmpty(inputListDtJson)) reqObj["InputListDt"] = inputListDtJson;
                reply = CallInternalMes("WOCHECKIN_MULTI", reqObj);
            }
            else
            {
                var reqObj = new JObject
                {
                    ["WONO"] = combinedWo,
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? ""
                };
                if (validCids.Count > 0)
                {
                    reqObj["WOCarrierDt"] = JsonConvert.SerializeObject(validCids.Select(c => new { CarrierNo = c }).ToList());
                }
                if (!string.IsNullOrEmpty(inputListDtJson)) reqObj["InputListDt"] = inputListDtJson;
                reply = CallInternalMes("WOCHECKIN", reqObj);
            }

            string resultCode = reply?["ResultCode"]?.ToString() ?? "";
            string resultMessage = reply?["Message"]?.ToString() ?? "";

            if (!IsMesSuccess(reply))
            {
                terminalErrorMsg = ResolveMesFailTerminalMessage(!string.IsNullOrEmpty(resultCode) ? resultCode : "ERR_MES", combinedWo, userId);
                return $"FAIL|{resultCode}|MES WOCHECKIN Failed for WOs {combinedWo}: {resultMessage}";
            }

            // 進站成功後執行第二次 WOQRY 取得出站所需管理項目
            var allTrackOutCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var wo in validWos)
            {
                var qry2 = CallInternalMes("WOQRY", new JObject
                {
                    ["WONO"] = wo,
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? ""
                });
                string codeListStr = qry2?["InputCodeListDT"]?.ToString();
                if (!string.IsNullOrWhiteSpace(codeListStr))
                {
                    try
                    {
                        var arr = JArray.Parse(codeListStr);
                        foreach (var item in arr)
                        {
                            if ("Y".Equals(item["RequireInput"]?.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                string c = item["InputCode"]?.ToString()?.Trim();
                                if (!string.IsNullOrEmpty(c)) allTrackOutCodes.Add(c);
                            }
                        }
                    }
                    catch { }
                }
            }
            _trackOutRequireInputMap[$"{eqpId}:{combinedWo}"] = allTrackOutCodes.ToList();

            return $"PASS|{resultCode}|{(string.IsNullOrEmpty(resultMessage) ? "TrackIn Success" : resultMessage)}";
        }

        private string ProcessTrackOutGeneral(string eqpId, List<string> workOrders, string qty, string userId, List<string> cassetteIds, out string terminalErrorMsg)
        {
            terminalErrorMsg = null;
            var validWos = (workOrders ?? new List<string>()).Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => w.Trim()).Distinct().ToList();
            if (validWos.Count == 0)
            {
                return "FAIL|W001|WorkOrder is empty";
            }

            string combinedWo = string.Join(",", validWos);
            string safeQty = string.IsNullOrWhiteSpace(qty) ? "0" : qty.Trim();

            string inputListDtJson = null;
            if (_trackOutRequireInputMap.TryGetValue($"{eqpId}:{combinedWo}", out var reqCodes) && reqCodes != null && reqCodes.Count > 0)
            {
                var arr = new JArray();
                foreach (var code in reqCodes)
                {
                    string val = GetManagementItemValue(eqpId, code);
                    if (!string.IsNullOrEmpty(val))
                    {
                        arr.Add(new JObject { ["InputCode"] = code, ["InputValue"] = val });
                    }
                }
                if (arr.Count > 0) inputListDtJson = arr.ToString(Formatting.None);
            }

            JObject reply;
            if (validWos.Count > 1)
            {
                var reqObj = new JObject
                {
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? "",
                    ["Operator"] = userId ?? "",
                    ["OutPut"] = safeQty,
                    ["WoNoDt"] = JsonConvert.SerializeObject(validWos.Select(w => new { WoNo = w }).ToList())
                };
                if (!string.IsNullOrEmpty(inputListDtJson)) reqObj["InputListDt"] = inputListDtJson;
                reply = CallInternalMes("WOCHECKOUT_MULTI", reqObj);
            }
            else
            {
                var reqObj = new JObject
                {
                    ["WONO"] = combinedWo,
                    ["EqpNo"] = eqpId,
                    ["UserID"] = userId ?? "",
                    ["Operator"] = userId ?? "",
                    ["OutPut"] = safeQty
                };
                if (!string.IsNullOrEmpty(inputListDtJson)) reqObj["InputListDt"] = inputListDtJson;
                reply = CallInternalMes("WOCHECKOUT", reqObj);
            }

            string resultCode = reply?["ResultCode"]?.ToString() ?? "";
            string resultMessage = reply?["Message"]?.ToString() ?? "";

            if (!IsMesSuccess(reply))
            {
                terminalErrorMsg = ResolveMesFailTerminalMessage(!string.IsNullOrEmpty(resultCode) ? resultCode : "ERR_MES", combinedWo, userId);
                return $"FAIL|{resultCode}|MES WOCHECKOUT Failed for WOs {combinedWo}: {resultMessage}";
            }

            _trackInRequireInputMap.TryRemove($"{eqpId}:{combinedWo}", out _);
            _trackOutRequireInputMap.TryRemove($"{eqpId}:{combinedWo}", out _);

            return $"PASS|{resultCode}|{(string.IsNullOrEmpty(resultMessage) ? "TrackOut Success" : resultMessage)}";
        }

        private string ExtractRecipeFromMes(string inputParamListDt)
        {
            if (string.IsNullOrEmpty(inputParamListDt)) return "UNKNOWN";
            try
            {
                var list = JsonConvert.DeserializeObject<List<JObject>>(inputParamListDt);
                var recipeObj = list?.FirstOrDefault(x =>
                    string.Equals(x["ParamName"]?.ToString(), "RECIPEID", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(x["RefFieldCode"]?.ToString(), "RECIPE ID", StringComparison.OrdinalIgnoreCase));
                return recipeObj?["DefaultValue"]?.ToString() ?? "UNKNOWN";
            }
            catch
            {
                return "ERROR";
            }
        }

        private JObject CallInternalMes(string transactionName, JObject parameters)
        {
            try
            {
                parameters["TransactionName"] = transactionName;
                string reqJson = parameters.ToString(Formatting.None);
                string replyJson = _internalMesHandler(reqJson);
                return JObject.Parse(replyJson);
            }
            catch (Exception ex)
            {
                Log($"[Internal MES Error] {transactionName}: {ex.Message}");
                return new JObject
                {
                    ["TransactionName"] = transactionName,
                    ["Result"] = "FAIL",
                    ["Message"] = ex.Message
                };
            }
        }

        private bool IsMesSuccess(JObject reply)
        {
            if (reply == null) return false;
            string res = reply["Result"]?.ToString() ?? "";
            return res.Equals("success", StringComparison.OrdinalIgnoreCase) || res.Equals("PASS", StringComparison.OrdinalIgnoreCase);
        }

        public string ResolveMesFailTerminalMessage(string resultCode, string workOrder = null, string userId = null)
        {
            string fallbackCode = !string.IsNullOrWhiteSpace(resultCode) ? resultCode.Trim() : "ERR_MES";
            string fallbackMessage = $"MES ErrorCode : {fallbackCode}";

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(baseDir, "Mapping", "MES_Mapping.json");
                if (!File.Exists(filePath)) return fallbackMessage;

                string json = File.ReadAllText(filePath);
                var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (mapping == null) return fallbackMessage;

                var match = mapping.FirstOrDefault(kv => kv.Key.Equals(fallbackCode, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrWhiteSpace(match.Value)) return fallbackMessage;

                string resolved = match.Value
                    .Replace("{wo}", workOrder ?? "", StringComparison.OrdinalIgnoreCase)
                    .Replace("{0}", workOrder ?? "")
                    .Replace("{userid}", userId ?? "", StringComparison.OrdinalIgnoreCase)
                    .Replace("{user}", userId ?? "", StringComparison.OrdinalIgnoreCase)
                    .Replace("{1}", userId ?? "");

                while (resolved.Contains("  ")) resolved = resolved.Replace("  ", " ");
                return SanitizeTerminalMessage(resolved.Trim());
            }
            catch
            {
                return fallbackMessage;
            }
        }

        public static string SanitizeTerminalMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;
            string text = message;
            if (text.IndexOf("在MES找不到RECIPEID", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                text = "RECIPEID NOT FOUND IN MES";
            }

            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if ((c >= 32 && c <= 126) || c == '\r' || c == '\n')
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString().Trim();
            return result.Length > 200 ? result.Substring(0, 197) + "..." : result;
        }

        // --- 主動發送 EAP 指令給機台 (供設備商測試機台接收能力) ---

        public async Task SendAreYouThereAsync(string eqpId, string customCmdTopic = null)
        {
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new AreYouThereMessage { CMD = CommandType.AreYouThere, Machine = resolvedEqp });
        }

        public async Task SendStatusReqAsync(string eqpId, string customCmdTopic = null)
        {
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new StatusReqMessage { CMD = CommandType.StatusReq, Machine = resolvedEqp });
        }

        public async Task SendTimeCalibrateAsync(string eqpId, string customCmdTopic = null)
        {
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new TimeCalibrateMessage
            {
                CMD = CommandType.TimeCalibrate,
                Machine = resolvedEqp,
                Date = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        public async Task SendInitTraceDataAsync(string eqpId, string customCmdTopic, string timerSeconds, bool enable = true)
        {
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new InitTraceDataMessage
            {
                CMD = CommandType.InitTraceData,
                Machine = resolvedEqp,
                Enable = enable,
                Timer = string.IsNullOrWhiteSpace(timerSeconds) ? "5" : timerSeconds.Trim()
            });
        }

        public async Task SendRemoteCmdAsync(string eqpId, RemoteCommandType cmdType, string recipeId = null, string customCmdTopic = null)
        {
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new RemoteCMDMessage
            {
                CMD = CommandType.RemoteCMD,
                Machine = resolvedEqp,
                RemoteCMDType = cmdType,
                RecipeID = recipeId ?? ""
            });
        }

        public async Task SendRemoteCmdAsync(string eqpId, string customCmdTopic, string rcmdStr, string recipeId = null)
        {
            if (!Enum.TryParse<RemoteCommandType>(rcmdStr, true, out var cmdType))
            {
                cmdType = RemoteCommandType.PP_SELECT;
            }
            await SendRemoteCmdAsync(eqpId, cmdType, recipeId, customCmdTopic);
        }

        public async Task SendTerminalDisplayAsync(string eqpId, string customCmdTopic, string text)
        {
            string safeMsg = SanitizeTerminalMessage(text);
            if (string.IsNullOrEmpty(safeMsg)) return;
            string resolvedEqp = !string.IsNullOrWhiteSpace(eqpId) ? eqpId : ExtractMachineIdFromCommandTopic(customCmdTopic);
            string topic = !string.IsNullOrWhiteSpace(customCmdTopic) ? customCmdTopic : ResolveCommandTopic(null, resolvedEqp);
            await SendCommandToTopicAsync(topic, new TerminalDisplayMessage
            {
                CMD = CommandType.TerminalDisplay,
                Machine = resolvedEqp,
                Message = safeMsg
            });
        }

        private async Task SendReplyAsync<T>(string cmdTopic, T replyMsg, BaseMessage requestMsg = null) where T : BaseMessage
        {
            replyMsg.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (requestMsg != null)
            {
                replyMsg.Machine = requestMsg.Machine;
                replyMsg.TransactionID = requestMsg.TransactionID;
            }
            await SendCommandToTopicAsync(cmdTopic, replyMsg);
        }

        private async Task SendCommandToTopicAsync(string cmdTopic, BaseMessage cmd)
        {
            if (_bus == null || !_bus.IsConnected)
            {
                Log($"[WARN] EAP 模擬器尚未連線至 Broker，無法發送 {cmd.CMD} 至 {cmdTopic}");
                return;
            }

            if (string.IsNullOrEmpty(cmd.TransactionID))
            {
                cmd.TransactionID = Guid.NewGuid().ToString("N");
            }
            if (string.IsNullOrEmpty(cmd.Date))
            {
                cmd.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            await _bus.PublishAsync(cmdTopic, cmd);
            Log($"[EAP SEND] Topic={cmdTopic} | Machine={cmd.Machine} | CMD={cmd.CMD}\n{JsonConvert.SerializeObject(cmd, _logSettings)}");
        }

        private void Log(string msg)
        {
            LogMessage?.Invoke(msg);
        }

        public void Dispose()
        {
            try
            {
                _cts?.Cancel();
                _bus?.Dispose();
            }
            catch { }
        }
    }
}
