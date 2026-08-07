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
using Protocol.MES.Base;
using Protocol.MES.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System.Net.Http;
using System.Collections.Concurrent;
using Protocol.Drivers.Secs;
using Protocol.LocalData;

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

    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string BindingTable { get; set; }
        public string CassetteColumn { get; set; }
        public string WoColumn { get; set; }
    }

    public class GatewayConfig 
    { 
        public string PrimaryUrl { get; set; } 
        public string SecondaryUrl { get; set; } 
    }

    public class BindingInfo
    {
        public string WorkOrder { get; set; }
        public List<string> Panels { get; set; } = new List<string>();
    }

    public abstract class EquipmentBase : INotifyPropertyChanged
    {
        private ConnectionStatus _connectionStatus;
        private EquipmentState _state;
        private string _currentRecipe;
        private Dictionary<string, string> _alarmMappings = new Dictionary<string, string>();
        private static readonly object _logLock = new object();
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;

        protected TReply CallMesWithLog<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                string prettyReq = JsonConvert.SerializeObject((object)request, Newtonsoft.Json.Formatting.Indented);
                OnLogReceived(LogLevel.Info, $"[SEND] MES {request.TransactionName}:\n{prettyReq}");
            }
            catch
            {
                OnLogReceived(LogLevel.Info, $"[SEND] MES {request.TransactionName}");
            }

            TReply reply = MESMessageFactory.CallMes<TRequest, TReply>(request);

            if (reply != null)
            {
                try
                {
                    string prettyReply = JsonConvert.SerializeObject((object)reply, Newtonsoft.Json.Formatting.Indented);
                    dynamic dReply = reply;
                    string resStr = dReply?.Result?.ToString() ?? "";
                    LogLevel level = (resStr.Equals("success", StringComparison.OrdinalIgnoreCase) || resStr.Equals("PASS", StringComparison.OrdinalIgnoreCase)) ? LogLevel.Info : LogLevel.Error;
                    OnLogReceived(level, $"[RECV] MES {request.TransactionName}:\n{prettyReply}");
                }
                catch
                {
                    OnLogReceived(LogLevel.Info, $"[RECV] MES {request.TransactionName}");
                }
            }

            return reply;
        }
        
        protected string _lastOperator = "UNKNOWN";
        protected DateTimeOffset _statusStartTime = DateTimeOffset.UtcNow;
        protected string _currentStatusMapped = "RUN";
        protected decimal _runTimeSec = 0;
        protected decimal _downTimeSec = 0;
        protected System.Timers.Timer _hourlyAvailabilityTimer;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSimulator { get; set; } = false;
        public bool IsMesSimulator { get; set; } = false;
        public bool EnableCassetteBinding { get; set; } = false;
        public int StatusReportIntervalSeconds { get; set; } = 60;
        public LocalDataService LocalDataServiceInstance { get; set; }
        public GatewayConfig GatewayConfig { get; set; }
        public DbSettings DbConfig { get; set; }
        public string DictionaryPath { get; set; } = string.Empty;

        protected string _lastStatusCode = "0";
        protected string _lastErrorCode = null;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DateTime, LogLevel, string> LogReceived;

        public void SetLastStatus(string statusCode, string errorCode = null)
        {
            if (!string.IsNullOrEmpty(statusCode)) _lastStatusCode = statusCode;
            if (errorCode != null) _lastErrorCode = errorCode;
        }

        protected async Task StartPeriodicStatusReportLoop(CancellationToken ct)
        {
            if (StatusReportIntervalSeconds <= 0) return;
            OnLogReceived(LogLevel.Info, $"[MES PERIODIC] Started periodic EQPSTATUS report loop for {Id} (Interval: {StatusReportIntervalSeconds}s).");

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(StatusReportIntervalSeconds), ct);

                    if (Connection == ConnectionStatus.Connected)
                    {
                        var req = new EQPSTATUSRequest
                        {
                            EqpNo = Id,
                            StatusCode = _lastStatusCode ?? "0",
                            ErrorCode = _lastErrorCode
                        };

                        OnLogReceived(LogLevel.Info, $"[MES PERIODIC SEND] Periodic EQPSTATUS Request for {Id}: StatusCode={req.StatusCode}, ErrorCode={req.ErrorCode}");

                        var replyTask = Task.Run(() => CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(req), ct);
                        if (await Task.WhenAny(replyTask, Task.Delay(25000, ct)) == replyTask)
                        {
                            var reply = await replyTask;
                            if (reply != null)
                            {
                                OnLogReceived(LogLevel.Info, $"[MES PERIODIC RECV] EQPSTATUS Reply: Result={reply.Result}, ResultCode={reply.ResultCode}, Message={reply.Message}");
                            }
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, $"[MES PERIODIC] Periodic EQPSTATUS report timeout (25s) for {Id}.");
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    OnLogReceived(LogLevel.Error, $"[MES PERIODIC ERROR] Periodic EQPSTATUS report error for {Id}: {ex.Message}");
                }
            }
        }

        public void WriteStatusToLocalDb(string statusCode, string statusDesc = "")
        {
            if (LocalDataServiceInstance == null) return;
            try
            {
                string mappedCode = MapStatusCodeToStandard(statusCode);
                decimal elapsed = (decimal)(DateTimeOffset.UtcNow - _statusStartTime).TotalSeconds;
                if (elapsed > 0)
                {
                    if (_currentStatusMapped == "RUN") _runTimeSec += elapsed;
                    else _downTimeSec += elapsed;
                }

                _statusStartTime = DateTimeOffset.UtcNow;
                _currentStatusMapped = mappedCode;

                LocalDataServiceInstance.WriteEquipmentStatus(Id, mappedCode, statusDesc ?? "", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"WriteEquipmentStatus Error: {ex.Message}");
            }
        }

        protected string MapStatusCodeToStandard(string statusCode)
        {
            if (string.IsNullOrEmpty(statusCode)) return "IDLE";
            if (statusCode == "0" || statusCode == "3" || statusCode.Equals("RUN", StringComparison.OrdinalIgnoreCase)) return "RUN";
            if (statusCode == "1" || statusCode == "10" || statusCode.Equals("ALARM", StringComparison.OrdinalIgnoreCase)) return "ALARM";
            if (statusCode == "5" || statusCode.Equals("DOWN", StringComparison.OrdinalIgnoreCase)) return "DOWN";
            if (statusCode == "4" || statusCode.Equals("MAINTENANCE", StringComparison.OrdinalIgnoreCase)) return "MAINTENANCE";
            return "IDLE";
        }

        public void StartHourlyAvailabilityTimer(CancellationToken ct)
        {
            if (_hourlyAvailabilityTimer != null) return;
            _statusStartTime = DateTimeOffset.UtcNow;
            _hourlyAvailabilityTimer = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            _hourlyAvailabilityTimer.Elapsed += (s, e) => WriteHourlyAvailabilityToLocalDb();
            _hourlyAvailabilityTimer.AutoReset = true;
            _hourlyAvailabilityTimer.Start();

            ct.Register(() => {
                _hourlyAvailabilityTimer?.Stop();
                _hourlyAvailabilityTimer?.Dispose();
            });
        }

        public void WriteHourlyAvailabilityToLocalDb()
        {
            if (LocalDataServiceInstance == null) return;
            try
            {
                decimal elapsed = (decimal)(DateTimeOffset.UtcNow - _statusStartTime).TotalSeconds;
                if (elapsed > 0)
                {
                    if (_currentStatusMapped == "RUN") _runTimeSec += elapsed;
                    else _downTimeSec += elapsed;
                }

                decimal total = _runTimeSec + _downTimeSec;
                decimal rate = total > 0 ? _runTimeSec / total : 0;

                LocalDataServiceInstance.WriteAvailability(
                    equipmentNo: Id,
                    periodStart: DateTimeOffset.UtcNow.AddHours(-1),
                    periodEnd: DateTimeOffset.UtcNow,
                    runTimeSec: _runTimeSec,
                    downTimeSec: _downTimeSec,
                    availabilityRate: rate
                );

                _runTimeSec = 0;
                _downTimeSec = 0;
                _statusStartTime = DateTimeOffset.UtcNow;
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"WriteAvailability Error: {ex.Message}");
            }
        }

        public void SetCurrentWorkOrderLocalDb(string woNo)
        {
            if (LocalDataServiceInstance == null || string.IsNullOrEmpty(woNo)) return;
            try
            {
                LocalDataServiceInstance.SetCurrentWorkOrder(Id, woNo, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"SetCurrentWorkOrder LocalDb Error: {ex.Message}");
            }
        }

        public void WriteOutputLocalDb(string woNo, int outputQty, int ngQty = 0)
        {
            if (LocalDataServiceInstance == null) return;
            try
            {
                LocalDataServiceInstance.WriteOutput(Id, woNo ?? "", outputQty, ngQty, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"WriteOutput LocalDb Error: {ex.Message}");
            }
        }

        public void WriteCycleTimeLocalDb(string woNo, decimal cycleTimeSec)
        {
            if (LocalDataServiceInstance == null) return;
            try
            {
                LocalDataServiceInstance.WriteCycleTime(Id, woNo ?? "", cycleTimeSec, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"WriteCycleTime LocalDb Error: {ex.Message}");
            }
        }

        public void WriteParametersLocalDb(IEnumerable<ParameterData> parameters)
        {
            if (LocalDataServiceInstance == null || parameters == null) return;
            try
            {
                LocalDataServiceInstance.WriteEquipmentParameters(Id, parameters);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"WriteEquipmentParameters LocalDb Error: {ex.Message}");
            }
        }

        public void RaiseAlarmLocalDb(string alarmId, string alarmText = "")
        {
            if (LocalDataServiceInstance == null || string.IsNullOrEmpty(alarmId)) return;
            try
            {
                LocalDataServiceInstance.RaiseAlarm(Id, alarmId, alarmText ?? "", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"RaiseAlarm LocalDb Error: {ex.Message}");
            }
        }

        public void ClearAlarmLocalDb(string alarmId)
        {
            if (LocalDataServiceInstance == null || string.IsNullOrEmpty(alarmId)) return;
            try
            {
                LocalDataServiceInstance.ClearAlarm(Id, alarmId, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"ClearAlarm LocalDb Error: {ex.Message}");
            }
        }

        public EquipmentBase()
        {
            var fallbackPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<TaskCanceledException>()
                .FallbackAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    OnLogReceived(LogLevel.Warn, "[GATEWAY] Resilience triggered: switching to backup logic.");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
                }));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

            _resiliencePolicy = Policy.WrapAsync<HttpResponseMessage>(fallbackPolicy, timeoutPolicy);
        }

        private readonly ConcurrentDictionary<string, string> _managementItems = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void LoadAlarmMappings(string path = null)
        {
            try
            {
                _alarmMappings.Clear();
                _managementItems.Clear();

                string dictPath = !string.IsNullOrEmpty(path) ? path : this.DictionaryPath;
                if (string.IsNullOrEmpty(dictPath)) return;

                string fullPath = Path.IsPathRooted(dictPath) 
                    ? dictPath 
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dictPath);

                if (!File.Exists(fullPath))
                {
                    OnLogReceived(LogLevel.Debug, $"[Dictionary] Dictionary file not found for Alarm Mapping: {fullPath}");
                    return;
                }

                string json = File.ReadAllText(fullPath);
                var jObj = JObject.Parse(json);
                
                var alarmToken = jObj["AlarmMapping"] ?? jObj["Alarms"];
                if (alarmToken != null && alarmToken.Type == JTokenType.Object)
                {
                    foreach (var prop in ((JObject)alarmToken).Properties())
                    {
                        _alarmMappings[prop.Name] = prop.Value.ToString();
                    }
                    OnLogReceived(LogLevel.Info, $"[Dictionary] Loaded {_alarmMappings.Count} alarm mappings from {Path.GetFileName(fullPath)} for {Id}.");
                }
                else
                {
                    OnLogReceived(LogLevel.Info, $"[Dictionary] No 'AlarmMapping' node found in {Path.GetFileName(fullPath)} for {Id}.");
                }

                var mgmtObj = jObj["ManagementItems"] as JObject;
                if (mgmtObj != null)
                {
                    foreach (var prop in mgmtObj.Properties())
                    {
                        _managementItems[prop.Name.Trim()] = prop.Value?.ToString();
                    }
                    OnLogReceived(LogLevel.Info, $"[Dictionary] Loaded {_managementItems.Count} management items from {Path.GetFileName(fullPath)} for {Id}.");
                }
            }
            catch (Exception ex) { OnLogReceived(LogLevel.Error, $"Load Alarm Mapping Failed for {Id}: {ex.Message}"); }
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
                
                var replyTask = Task.Run(() => CallMesWithLog<UserVerifyRequest, UserVerifyReply>(req));
                if (await Task.WhenAny(replyTask, Task.Delay(25000)) != replyTask)
                    return "FAIL|T001|MES Call Timeout (25s)|";

                var reply = await replyTask;
                string resPrefix = (reply.Result == "success") ? "PASS" : "FAIL";
                return $"{resPrefix}|{reply.ResultCode}|{reply.Message}|{reply.UserID}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Verify Failed: {ex.Message}"; }
        }

        private readonly ConcurrentDictionary<string, List<string>> _trackInRequireInputMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, List<string>> _trackOutRequireInputMap = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public virtual string GetManagementItemValue(string inputCode)
        {
            if (string.IsNullOrEmpty(inputCode)) return null;
            return _managementItems.TryGetValue(inputCode.Trim(), out var val) ? val : null;
        }

        public async Task<string> ProcessTrackInAsync(List<string> cassetteIds, List<string> workOrders, List<string> panels, string userId, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                List<string> wos = workOrders ?? new List<string>();
                List<string> cids = cassetteIds ?? new List<string>();
                List<string> currentPanels = panels ?? new List<string>();

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).ToList();
                if (validWos.Count == 0) return "FAIL|W001|Cassette Not Bound to WO|[]";

                string combinedWo = string.Join(",", validWos);
                var qryReq = new WOQRYRequest { WONO = combinedWo, EqpNo = eid, UserID = userId };
                var qryTask = Task.Run(() => CallMesWithLog<WOQRYRequest, WOQRYReply>(qryReq));
                if (await Task.WhenAny(qryTask, Task.Delay(25000)) != qryTask)
                    return $"FAIL|T001|MES WOQRY Timeout (25s) for WOs: {combinedWo}|[]";

                var qryReply = await qryTask;
                if (qryReply.Result != "success") 
                    return $"FAIL|{qryReply.ResultCode}|MES WOQRY Failed for WOs {combinedWo}: {qryReply.Message}|[]";

                string recipe = ExtractRecipeFromMes(qryReply.InputParamListDT) ?? "";

                // 解析 InputCodeList，收集 RequireInput == "Y" 的 InputCode (以工單號為 Key 記錄)
                var reqInputCodes = new List<string>();
                if (qryReply.InputCodeList != null && qryReply.InputCodeList.Count > 0)
                {
                    foreach (var item in qryReply.InputCodeList)
                    {
                        if ("Y".Equals(item.RequireInput, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(item.InputCode))
                        {
                            reqInputCodes.Add(item.InputCode.Trim());
                        }
                    }
                }
                _trackInRequireInputMap[combinedWo] = reqInputCodes;

                string panelsJson = JsonConvert.SerializeObject(currentPanels);
                return $"PP-SELECT|{combinedWo}|{recipe}|{qryReply.ResultCode}|{qryReply.Message}|{qryReply.InputParamListDT}|{panelsJson}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|TrackIn Error: {ex.Message}|[]"; }
        }

        public Task<string> ProcessTrackInAsync(string cassetteId, string workOrder, List<string> panels, string userId, string reportEqpId = null)
        {
            return ProcessTrackInAsync(
                string.IsNullOrEmpty(cassetteId) ? new List<string>() : new List<string> { cassetteId },
                string.IsNullOrEmpty(workOrder) ? new List<string>() : new List<string> { workOrder },
                panels, userId, reportEqpId);
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

        public async Task<string> ProcessTrackInFinalAsync(List<string> workOrders, string userId, List<string> cassetteIds, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                List<string> wos = workOrders ?? new List<string>();
                List<string> cids = cassetteIds ?? new List<string>();

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).ToList();
                string combinedWo = string.Join(",", validWos);

                var req = new WOCHECKINRequest { WONO = combinedWo, EqpNo = eid, UserID = userId };
                var validCids = cids.Where(c => !string.IsNullOrEmpty(c)).ToList();
                if (validCids.Count > 0)
                {
                    req.WOCarrierDt = JsonConvert.SerializeObject(validCids.Select(c => new CarrierItem { CarrierNo = c }).ToList());
                }

                // 檢查 _trackInRequireInputMap 中該工單 RequireInput == "Y" 的 InputCode，比對設備字典 ManagementItems
                if (_trackInRequireInputMap.TryGetValue(combinedWo, out var reqCodes) && reqCodes != null && reqCodes.Count > 0)
                {
                    var inputItems = new List<InputItem>();
                    foreach (var code in reqCodes)
                    {
                        string val = GetManagementItemValue(code);
                        if (!string.IsNullOrEmpty(val))
                        {
                            inputItems.Add(new InputItem { InputCode = code, InputValue = val });
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Error, $"[TrackIn] Management Item Code '{code}' required by MES was not found in Equipment Dictionary for {Id}.");
                        }
                    }
                    if (inputItems.Count > 0)
                    {
                        req.InputListDt = JsonConvert.SerializeObject(inputItems);
                    }
                }

                var checkinTask = Task.Run(() => CallMesWithLog<WOCHECKINRequest, WOCHECKINReply>(req));
                if (await Task.WhenAny(checkinTask, Task.Delay(25000)) != checkinTask)
                    return $"FAIL|T001|MES WOCHECKIN Timeout (25s) for WOs: {combinedWo}|";

                var reply = await checkinTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                if (resPrefix != "PASS")
                {
                    OnLogReceived(LogLevel.Error, $"[TrackIn] MES WOCHECKIN Failed for WOs {combinedWo}. Code: {reply.ResultCode}, Message: {reply.Message}");
                    return $"{resPrefix}|{reply.ResultCode}|MES WOCHECKIN Failed for WOs {combinedWo}: {reply.Message}";
                }

                if (EnableCassetteBinding)
                {
                    foreach (var cid in validCids)
                    {
                        bool ok = await UnbindFromDbAsync(cid);
                        if (!ok) await SavePendingUnbindingAsync(cid);
                    }
                }

                SetCurrentWorkOrderLocalDb(combinedWo);

                // 進站成功後，發送第二次工單查詢 (WOQRYRequest)
                _ = Task.Run(() =>
                {
                    try
                    {
                        var qryReq2 = new WOQRYRequest { WONO = combinedWo, EqpNo = eid, UserID = userId };
                        var qryReply2 = CallMesWithLog<WOQRYRequest, WOQRYReply>(qryReq2);
                        if (qryReply2?.Result != "success")
                        {
                            OnLogReceived(LogLevel.Error, $"[TrackIn] Second WOQRY failed for WO {combinedWo}: {qryReply2?.Message}");
                            _trackOutRequireInputMap[combinedWo] = new List<string>();
                            return;
                        }

                        var reqTrackOutCodes = new List<string>();
                        if (qryReply2.InputCodeList != null && qryReply2.InputCodeList.Count > 0)
                        {
                            foreach (var item in qryReply2.InputCodeList)
                            {
                                if ("Y".Equals(item.RequireInput, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(item.InputCode))
                                {
                                    reqTrackOutCodes.Add(item.InputCode.Trim());
                                }
                            }
                        }
                        _trackOutRequireInputMap[combinedWo] = reqTrackOutCodes;
                        OnLogReceived(LogLevel.Info, $"[TrackIn] Second WOQRY success for WO {combinedWo}. RequireInput codes for TrackOut: {string.Join(",", reqTrackOutCodes)}");
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Error, $"[TrackIn] Second WOQRY exception for WO {combinedWo}: {ex.Message}");
                        _trackOutRequireInputMap[combinedWo] = new List<string>();
                    }
                });

                return $"PASS|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|Report Failed: {ex.Message}"; }
        }

        public Task<string> ProcessTrackInFinalAsync(string workOrder, string userId, string cassetteId, string reportEqpId = null)
        {
            return ProcessTrackInFinalAsync(
                string.IsNullOrEmpty(workOrder) ? new List<string>() : new List<string> { workOrder },
                userId,
                string.IsNullOrEmpty(cassetteId) ? new List<string>() : new List<string> { cassetteId },
                reportEqpId);
        }

        public async Task<string> ProcessTrackOutAsync(List<string> workOrders, string qty, string userId, List<string> cassetteIds, List<string> panels, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                List<string> wos = workOrders ?? new List<string>();
                List<string> cids = cassetteIds ?? new List<string>();

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).ToList();
                string combinedWo = string.Join(",", validWos);

                OnLogReceived(LogLevel.Info, $"[WOCHECKOUT] {eid} - WOs: {combinedWo}, Qty: {qty}, CIDs: {string.Join(",", cids)}");

                var req = new WOCHECKOUTRequest { WONO = combinedWo, EqpNo = eid, UserID = userId, Operator = userId, OutPut = qty };

                // 比對第二次 WOQRY 保存之 TrackOutRequireInputCodes 與字典 ManagementItems
                if (_trackOutRequireInputMap.TryGetValue(combinedWo, out var reqCodes) && reqCodes != null && reqCodes.Count > 0)
                {
                    var inputItems = new List<InputItem>();
                    foreach (var code in reqCodes)
                    {
                        string val = GetManagementItemValue(code);
                        if (!string.IsNullOrEmpty(val))
                        {
                            inputItems.Add(new InputItem { InputCode = code, InputValue = val });
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Error, $"[TrackOut] Management Item Code '{code}' required by MES was not found in Equipment Dictionary for {Id}.");
                        }
                    }
                    if (inputItems.Count > 0)
                    {
                        req.InputListDt = JsonConvert.SerializeObject(inputItems);
                    }
                }

                var checkoutTask = Task.Run(() => CallMesWithLog<WOCHECKOUTRequest, WOCHECKOUTReply>(req));
                if (await Task.WhenAny(checkoutTask, Task.Delay(25000)) != checkoutTask)
                    return $"FAIL|T001|MES WOCHECKOUT Timeout (25s) for WOs: {combinedWo}|";

                var reply = await checkoutTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                if (resPrefix != "PASS")
                {
                    OnLogReceived(LogLevel.Error, $"[TrackOut] MES WOCHECKOUT Failed for WOs {combinedWo}. Code: {reply.ResultCode}, Message: {reply.Message}");
                    return $"{resPrefix}|{reply.ResultCode}|MES WOCHECKOUT Failed for WOs {combinedWo}: {reply.Message}";
                }

                // 清理此工單快取
                _trackInRequireInputMap.TryRemove(combinedWo, out _);
                _trackOutRequireInputMap.TryRemove(combinedWo, out _);

                if (EnableCassetteBinding)
                {
                    for (int i = 0; i < Math.Max(cids.Count, wos.Count); i++)
                    {
                        string cid = i < cids.Count ? cids[i] : (cids.FirstOrDefault() ?? "UNKNOWN");
                        string wo = i < wos.Count ? wos[i] : (wos.FirstOrDefault() ?? "");
                        bool dbOk = await UpdateBindingInDbAsync(cid, wo, panels);
                        if (!dbOk) await SavePendingBindingAsync(cid, wo, panels);
                    }
                }

                if (int.TryParse(qty, out int outQty))
                {
                    WriteOutputLocalDb(combinedWo, outQty, 0);
                }

                return $"PASS|{reply.ResultCode}|{reply.Message}";
            }
            catch (Exception ex) { return $"ERROR|UNKNOWN|TrackOut Failed: {ex.Message}"; }
        }

        public Task<string> ProcessTrackOutAsync(string workOrder, string qty, string userId, string cassetteId, List<string> panels, string reportEqpId = null)
        {
            return ProcessTrackOutAsync(
                string.IsNullOrEmpty(workOrder) ? new List<string>() : new List<string> { workOrder },
                qty, userId,
                string.IsNullOrEmpty(cassetteId) ? new List<string>() : new List<string> { cassetteId },
                panels, reportEqpId);
        }

        protected BindingInfo QueryBindingFromGateway(string cassetteId)
        {
            if (!EnableCassetteBinding || GatewayConfig == null) return null;
            try
            {
                var queryPolicy = Policy<HttpResponseMessage>
                    .Handle<Exception>()
                    .FallbackAsync(async (ct) => {
                        OnLogReceived(LogLevel.Warn, "[GATEWAY] Query Primary failed, trying Secondary...");
                        return await _httpClient.GetAsync(string.Format("{0}/api/binding/query/{1}/{2}", GatewayConfig.SecondaryUrl, this.Id, cassetteId), ct);
                    });

                return Task.Run(async () => {
                    var resp = await queryPolicy.ExecuteAsync(async (ct) => {
                        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                        {
                            return await _httpClient.GetAsync(string.Format("{0}/api/binding/query/{1}/{2}", GatewayConfig.PrimaryUrl, this.Id, cassetteId), cts.Token);
                        }
                    }, CancellationToken.None);

                    if (resp.IsSuccessStatusCode) 
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BindingInfo>(json);
                    }
                    return null;
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex) 
            { 
                OnLogReceived(LogLevel.Error, string.Format("[GATEWAY] Binding Query Failed Final: {0}", ex.Message));
                return null; 
            }
        }

        protected async Task<bool> UpdateBindingInDbAsync(string cassetteId, string workOrder, List<string> panels)
        {
            if (GatewayConfig == null) return false;
            try
            {
                var req = new { EqpId = this.Id, CassetteId = cassetteId, WorkOrder = workOrder, Panels = panels };
                var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                var resp = await _resiliencePolicy.ExecuteAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    try {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/bind", GatewayConfig.PrimaryUrl), content, ct);
                    } catch {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/bind", GatewayConfig.SecondaryUrl), content, ct);
                    }
                }), CancellationToken.None);

                if (resp.IsSuccessStatusCode) {
                    OnLogReceived(LogLevel.Info, string.Format("[DB] {0} Successfully bound Cassette {1} to WO {2} via Gateway.", this.Id, cassetteId, workOrder));
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        protected async Task<bool> UnbindFromDbAsync(string cassetteId)
        {
            if (GatewayConfig == null) return false;
            try
            {
                var req = new { EqpId = this.Id, CassetteId = cassetteId };
                var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                var resp = await _resiliencePolicy.ExecuteAsync(new Func<CancellationToken, Task<HttpResponseMessage>>(async (ct) => {
                    try {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/unbind", GatewayConfig.PrimaryUrl), content, ct);
                    } catch {
                        return await _httpClient.PostAsync(string.Format("{0}/api/binding/unbind", GatewayConfig.SecondaryUrl), content, ct);
                    }
                }), CancellationToken.None);

                if (resp.IsSuccessStatusCode) {
                    OnLogReceived(LogLevel.Info, string.Format("[DB] {0} Successfully unbound Cassette {1} via Gateway.", this.Id, cassetteId));
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        private async Task SavePendingBindingAsync(string cid, string wo, List<string> panels)
        {
            try {
                using (var conn = DatabaseHelper.GetConnection()) {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO Local_Binding_Buffer (EqpId, CassetteID, WorkOrder, PanelList) VALUES (@eqid, @cid, @wo, @json)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
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
                    string sql = "INSERT INTO Local_Unbinding_Buffer (EqpId, CassetteID) VALUES (@eqid, @cid)";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue("@eqid", this.Id);
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
                    var pendings = DatabaseHelper.GetPendingBindings(this.Id);
                    foreach (var p in pendings) {
                        var panels = JsonConvert.DeserializeObject<List<string>>(p.PanelList);
                        if (await UpdateBindingInDbAsync(p.CassetteID, p.WorkOrder, panels)) DatabaseHelper.DeletePendingBinding(p.Id);
                    }
                    var unpendings = DatabaseHelper.GetPendingUnbindings(this.Id);
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
                SetLastStatus(newStatus);
                WriteStatusToLocalDb(newStatus);
                OnLogReceived(LogLevel.Info, string.Format("[STATUS] {0} Status Changed: {1} -> {2}", eid, oldStatus, newStatus));

                if (int.TryParse(newStatus, out int sc))
                {
                    if (sc == 0 || sc == 6) State = EquipmentState.Running;
                    else if (sc == 1 || sc == 10) State = EquipmentState.Alarm;
                    else State = EquipmentState.Paused;
                }

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = newStatus };
                
                var statusTask = Task.Run(() => CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                if (await Task.WhenAny(statusTask, Task.Delay(25000)) != statusTask)
                {
                    OnLogReceived(LogLevel.Error, $"[STATUS] {eid} MES EQPSTATUS Timeout (25s)");
                    return "FAIL|T001|MES EQPSTATUS Timeout (25s)";
                }

                var reply = await statusTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                OnLogReceived(LogLevel.Info, $"[STATUS] {eid} MES Reply: Result={reply.Result}, ResultCode={reply.ResultCode}, Message={reply.Message}");
                return string.Format("{0}|{1}|{2}", resPrefix, reply.ResultCode, reply.Message);
            }
            catch (Exception ex) { 
                OnLogReceived(LogLevel.Error, $"[STATUS] {reportEqpId ?? this.Id} Status Update Failed: {ex.Message}");
                return string.Format("ERROR|UNKNOWN|Status Update Failed: {0}", ex.Message); 
            }
        }

        public async Task<string> ProcessPanelInAsync(string panelId, string cassetteId, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, "Panel_In", string.Format("[PLC] {0} - Panel: {1}, Cassette: {2}", eid, panelId, cassetteId));
            await SaveProductionDataAsync("Panel_In", "", new { PanelID = panelId, CassetteID = cassetteId, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessPanelOutAsync(string panelId, string cassetteId, string result, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, "Panel_Out", string.Format("[PLC] {0} - Panel: {1}, Result: {2}", eid, panelId, result));
            await SaveProductionDataAsync("Panel_Out", "", new { PanelID = panelId, CassetteID = cassetteId, Result = result, EqpID = eid });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessKeyParametersReportAsync(List<Protocol.Core.Messages.ParameterResultItem> parameters, string reportEqpId = null)
        {
            string eid = reportEqpId ?? this.Id;
            OnLogReceived(LogLevel.Info, string.Format("[KPIV/KPOV] {0} KPI Report: {1} records", eid, parameters.Count));
            await SaveProductionDataAsync("KeyParametersReport", "", new { EqpID = eid, ProcessData = parameters });
            return "PASS|OK|Success";
        }

        public async Task<string> ProcessAlarmAsync(string alarmCode, bool isSet, string reportEqpId = null, string alarmMsg = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string mc = TransformAlarmCode(alarmCode);
                OnLogReceived(LogLevel.Warn, string.Format("[ALARM] {0} Alarm Report: Code={1} (Mapped={2}), Set={3}", eid, alarmCode, mc, isSet));

                if (isSet)
                {
                    string text = !string.IsNullOrEmpty(alarmMsg) ? alarmMsg : $"Alarm {alarmCode} (Mapped: {mc})";
                    RaiseAlarmLocalDb(alarmCode, text);
                }
                else
                {
                    ClearAlarmLocalDb(alarmCode);
                }

                var statusReq = new EQPSTATUSRequest { EqpNo = eid, StatusCode = isSet ? "1" : "2", ErrorCode = mc };
                
                var alarmTask = Task.Run(() => CallMesWithLog<EQPSTATUSRequest, EQPSTATUSReply>(statusReq));
                if (await Task.WhenAny(alarmTask, Task.Delay(25000)) != alarmTask)
                {
                    OnLogReceived(LogLevel.Error, $"[ALARM] {eid} MES ALARM Timeout (25s)");
                    return "FAIL|T001|MES ALARM Timeout (25s)";
                }

                var reply = await alarmTask;
                string resPrefix = (reply.Result == "success" || reply.Result == "PASS") ? "PASS" : "FAIL";
                OnLogReceived(LogLevel.Info, $"[ALARM] {eid} MES Reply: Result={reply.Result}, ResultCode={reply.ResultCode}, Message={reply.Message}");
                return string.Format("{0}|{1}|{2}", resPrefix, reply.ResultCode, reply.Message);
            }
            catch (Exception ex) 
            { 
                OnLogReceived(LogLevel.Error, string.Format("Alarm Report Exception: {0}", ex.Message));
                return string.Format("ERROR|UNKNOWN|{0}", ex.Message);
            }
        }

        public async Task SaveProductionDataAsync(string eventName, string workOrder, object data, bool saveToDb = true)
        {
            string json = JsonConvert.SerializeObject(data);
            DateTime now = DateTime.Now;
            if (saveToDb)
            {
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
            }
            try {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataLogs", now.ToString("yyyyMMdd"));
                if (!Directory.Exists(dir)) Directory.Exists(dir);
                string filePath = Path.Combine(dir, string.Format("{0}.csv", this.Id));
                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8)) {
                    sw.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss},{1},{2},\"{3}\"", now, eventName, workOrder ?? "", json.Replace("\"", "\"\"")));
                }
            } catch { }
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

        public bool IsInitializingReconciliation { get; set; } = true;
        public string PostgresConnectionString { get; set; }
        private CancellationTokenSource _reconciliationCts;
        private bool _isFirstStateUpdate = true;

        public Task ReconcileOrphanAlarmsLocalDbAsync()
        {
            if (LocalDataServiceInstance == null) 
            {
                OnLogReceived(LogLevel.Warn, $"[Reconciliation] Skip DB reconciliation for {Id}: LocalDataServiceInstance is null.");
                return Task.CompletedTask;
            }
            try
            {
                OnLogReceived(LogLevel.Info, $"[Reconciliation] Invoking LocalDataService.SyncAlarms for equipment {Id}.");
                LocalDataServiceInstance.SyncAlarms(Id, DateTimeOffset.UtcNow);
                IsInitializingReconciliation = false;
                OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} orphan DB alarms reconciled & synchronized via SyncAlarms. IsInitializingReconciliation set to FALSE.");
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[Reconciliation] SyncAlarms Error for {Id}: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        protected void OnEquipmentStateUpdated(EquipmentState newState)
        {
            if (!IsInitializingReconciliation) return;

            bool isNormalState = (newState == EquipmentState.Running || newState == EquipmentState.Idle || newState == EquipmentState.Paused);

            if (_isFirstStateUpdate)
            {
                _isFirstStateUpdate = false;
                if (isNormalState)
                {
                    OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} initial state is {newState}. Executing immediate DB alarm reconciliation.");
                    Task.Run(() => ReconcileOrphanAlarmsLocalDbAsync());
                    return;
                }
                else if (newState == EquipmentState.Alarm || newState == EquipmentState.Down)
                {
                    OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} initial state is {newState}. Pending 30s reconciliation upon status change.");
                    return;
                }
            }

            if (isNormalState)
            {
                if (_reconciliationCts == null)
                {
                    _reconciliationCts = new CancellationTokenSource();
                    var ct = _reconciliationCts.Token;
                    OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} changed state to {newState}. Started 30s timer for orphan DB alarm reconciliation.");

                    Task.Run(async () =>
                    {
                        try
                        {
                            await Task.Delay(30000, ct);
                            if (ct.IsCancellationRequested) return;

                            if (State == EquipmentState.Running || State == EquipmentState.Idle || State == EquipmentState.Paused)
                            {
                                await ReconcileOrphanAlarmsLocalDbAsync();
                            }
                            else
                            {
                                OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} 30s timer expired but state is {State}. Skipping reconciliation.");
                            }
                        }
                        catch (TaskCanceledException) { }
                        catch (Exception ex)
                        {
                            OnLogReceived(LogLevel.Error, $"[Reconciliation] Timer Task Exception for {Id}: {ex.Message}");
                        }
                        finally
                        {
                            _reconciliationCts = null;
                        }
                    });
                }
            }
            else if (newState == EquipmentState.Alarm || newState == EquipmentState.Down)
            {
                if (_reconciliationCts != null)
                {
                    _reconciliationCts.Cancel();
                    _reconciliationCts = null;
                    OnLogReceived(LogLevel.Info, $"[Reconciliation] Equipment {Id} state returned to {newState}. Cancelled pending 30s reconciliation timer.");
                }
            }
        }

        public ConnectionStatus Connection { get => _connectionStatus; set { if (_connectionStatus != value) { _connectionStatus = value; OnPropertyChanged(nameof(Connection)); } } }
        public EquipmentState State 
        { 
            get => _state; 
            set 
            { 
                if (_state != value || _isFirstStateUpdate) 
                { 
                    _state = value; 
                    OnPropertyChanged(nameof(State)); 
                    OnEquipmentStateUpdated(value);
                } 
            } 
        }
        public string CurrentRecipe { get => _currentRecipe; set { if (_currentRecipe != value) { _currentRecipe = value; OnPropertyChanged(nameof(CurrentRecipe)); } } }

        // 安全重啟相關屬性
        public bool IsStopping { get; set; }
        private int _activeTaskCount = 0;
        public int ActiveTaskCount { get { return _activeTaskCount; } }

        public void IncrementTask() { Interlocked.Increment(ref _activeTaskCount); }
        public void DecrementTask() { Interlocked.Decrement(ref _activeTaskCount); }

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
                    string filePath = Path.Combine(logDir, string.Format("{0:HH}.txt", timestamp));
                    string evtPart = string.IsNullOrEmpty(eventName) ? "" : string.Format(" [{0}]", eventName);
                    File.AppendAllText(filePath, string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}]{2} : {3}{4}", timestamp, level, evtPart, message, Environment.NewLine));
                }
            } catch { }
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public abstract void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30);
        public abstract Task SendTimeCalibration();
        public virtual Task SendTerminalDisplay(string message) => Task.CompletedTask;
        public virtual void Stop() { }
        public static Color GetColorForState(EquipmentState state) { switch (state) { case EquipmentState.Running: return Color.LimeGreen; case EquipmentState.Alarm: case EquipmentState.Down: return Color.Crimson; case EquipmentState.Paused: case EquipmentState.Idle: return Color.Gold; default: return Color.Black; } }
    }
}
