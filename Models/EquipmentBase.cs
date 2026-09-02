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

    public class BindingInfo
    {
        public string WorkOrder { get; set; }
        public List<string> Panels { get; set; } = new List<string>();
    }

    public class NGCodeItem
    {
        public string NGCode { get; set; }
        public string NGName { get; set; }
        public string NGQty { get; set; } = null;
        public string NGUnit { get; set; } = "";
        public bool IsReported => NGQty != null;
    }

    public class NGItem
    {
        public string NGCode { get; set; }
        public string NGQty { get; set; }
        public string NGUnit { get; set; }
    }

    public class TotalCountItem
    {
        public string EquipmentId { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;
        public string ReelCount { get; set; } = string.Empty;
        public string QtyPerReel { get; set; } = string.Empty;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }

    public class LeftoverRecordItem
    {
        public string EquipmentId { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;
        public string Qty { get; set; } = string.Empty;
        public string ReelId { get; set; } = string.Empty;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }

    public class ProductLeftoverRecordItem
    {
        public string EquipmentId { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;
        public string AddQty { get; set; } = string.Empty;
        public string GapQty { get; set; } = string.Empty;
        public string ReelId { get; set; } = string.Empty;
        public string LeftoverWorkOrder { get; set; } = string.Empty;
        public string LeftoverBatchNo { get; set; } = string.Empty;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }

    public abstract class EquipmentBase : INotifyPropertyChanged
    {
        private ConnectionStatus _connectionStatus;
        private EquipmentState _state;
        private string _currentRecipe;
        private Dictionary<string, string> _alarmMappings = new Dictionary<string, string>();
        protected readonly ConcurrentDictionary<string, ConcurrentDictionary<string, NGCodeItem>> _workOrderNgCodesMap 
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, NGCodeItem>>(StringComparer.OrdinalIgnoreCase);
        protected readonly ConcurrentDictionary<string, TotalCountItem> _workOrderTotalCountMap
            = new ConcurrentDictionary<string, TotalCountItem>(StringComparer.OrdinalIgnoreCase);
        protected readonly ConcurrentDictionary<string, LeftoverRecordItem> _workOrderLeftoversMap
            = new ConcurrentDictionary<string, LeftoverRecordItem>(StringComparer.OrdinalIgnoreCase);
        protected readonly ConcurrentDictionary<string, ProductLeftoverRecordItem> _workOrderProductLeftoversMap
            = new ConcurrentDictionary<string, ProductLeftoverRecordItem>(StringComparer.OrdinalIgnoreCase);
        protected readonly ConcurrentDictionary<string, double> _workOrderInputQtyMap
            = new ConcurrentDictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        public double MinYieldRate { get; set; } = 0.95;
        public double MaxYieldRate { get; set; } = 1.00;

        private static readonly object _totalCountFileLock = new object();
        private static readonly object _ngCodeFileLock = new object();
        private static readonly object _leftoversFileLock = new object();
        private static readonly object _productLeftoversFileLock = new object();
        private static readonly object _inputQtyFileLock = new object();
        public static string CustomCacheDirectory { get; set; } = null;

        protected static string GetCacheDirectory()
        {
            if (!string.IsNullOrEmpty(CustomCacheDirectory))
            {
                if (!Directory.Exists(CustomCacheDirectory))
                {
                    Directory.CreateDirectory(CustomCacheDirectory);
                }
                return CustomCacheDirectory;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string cacheDir = Path.Combine(baseDir, "Cache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }
            return cacheDir;
        }

        protected static string GetTotalCountCacheFilePath() => Path.Combine(GetCacheDirectory(), "total_count_cache.json");
        protected static string GetNgCodeCacheFilePath() => Path.Combine(GetCacheDirectory(), "ng_code_cache.json");
        protected static string GetLeftoversCacheFilePath() => Path.Combine(GetCacheDirectory(), "leftovers_cache.json");
        protected static string GetProductLeftoversCacheFilePath() => Path.Combine(GetCacheDirectory(), "product_leftovers_cache.json");
        protected static string GetInputQtyCacheFilePath() => Path.Combine(GetCacheDirectory(), "input_qty_cache.json");

        public void LoadCachesFromDisk()
        {
            LoadTotalCountCache();
            LoadNgCodeCache();
            LoadLeftoversCache();
            LoadProductLeftoversCache();
            LoadInputQtyCache();
        }

        public void LoadTotalCountCache()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_totalCountFileLock)
                {
                    string filePath = GetTotalCountCacheFilePath();
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        var allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, TotalCountItem>>>(json);
                        if (allCache != null && allCache.TryGetValue(this.Id, out var eqpCache) && eqpCache != null)
                        {
                            foreach (var kvp in eqpCache)
                            {
                                _workOrderTotalCountMap[kvp.Key] = kvp.Value;
                            }
                            OnLogReceived(LogLevel.Info, $"[TotalCountCache] Loaded {eqpCache.Count} item(s) from disk for {Id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[TotalCountCache] Failed to load cache from disk for {Id}: {ex.Message}");
            }
        }

        public void SaveTotalCountCacheToDisk()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_totalCountFileLock)
                {
                    string filePath = GetTotalCountCacheFilePath();
                    Dictionary<string, Dictionary<string, TotalCountItem>> allCache = null;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, TotalCountItem>>>(existingJson);
                        }
                        catch { }
                    }
                    allCache ??= new Dictionary<string, Dictionary<string, TotalCountItem>>(StringComparer.OrdinalIgnoreCase);
                    allCache[this.Id] = new Dictionary<string, TotalCountItem>(_workOrderTotalCountMap, StringComparer.OrdinalIgnoreCase);

                    string newJson = JsonConvert.SerializeObject(allCache, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[TotalCountCache] Failed to save cache to disk for {Id}: {ex.Message}");
            }
        }

        public void LoadNgCodeCache()
        {
            if (string.IsNullOrEmpty(this.Id) || !IsTpEquipment) return;
            try
            {
                lock (_ngCodeFileLock)
                {
                    string filePath = GetNgCodeCacheFilePath();
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        var allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, NGCodeItem>>>>(json);
                        if (allCache != null && allCache.TryGetValue(this.Id, out var eqpCache) && eqpCache != null)
                        {
                            foreach (var woKvp in eqpCache)
                            {
                                var ngDict = new ConcurrentDictionary<string, NGCodeItem>(StringComparer.OrdinalIgnoreCase);
                                foreach (var ngKvp in woKvp.Value)
                                {
                                    ngDict[ngKvp.Key] = ngKvp.Value;
                                }
                                _workOrderNgCodesMap[woKvp.Key] = ngDict;
                            }
                            OnLogReceived(LogLevel.Info, $"[NgCodeCache] Loaded {eqpCache.Count} WO(s) from disk for {Id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[NgCodeCache] Failed to load cache from disk for {Id}: {ex.Message}");
            }
        }

        public void SaveNgCodeCacheToDisk()
        {
            if (string.IsNullOrEmpty(this.Id) || !IsTpEquipment) return;
            try
            {
                lock (_ngCodeFileLock)
                {
                    string filePath = GetNgCodeCacheFilePath();
                    Dictionary<string, Dictionary<string, Dictionary<string, NGCodeItem>>> allCache = null;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, NGCodeItem>>>>(existingJson);
                        }
                        catch { }
                    }
                    allCache ??= new Dictionary<string, Dictionary<string, Dictionary<string, NGCodeItem>>>(StringComparer.OrdinalIgnoreCase);

                    var eqpMap = new Dictionary<string, Dictionary<string, NGCodeItem>>(StringComparer.OrdinalIgnoreCase);
                    foreach (var kvp in _workOrderNgCodesMap)
                    {
                        eqpMap[kvp.Key] = new Dictionary<string, NGCodeItem>(kvp.Value, StringComparer.OrdinalIgnoreCase);
                    }
                    allCache[this.Id] = eqpMap;

                    string newJson = JsonConvert.SerializeObject(allCache, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[NgCodeCache] Failed to save cache to disk for {Id}: {ex.Message}");
            }
        }

        public void LoadLeftoversCache()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_leftoversFileLock)
                {
                    string filePath = GetLeftoversCacheFilePath();
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        var allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, LeftoverRecordItem>>>(json);
                        if (allCache != null && allCache.TryGetValue(this.Id, out var eqpCache) && eqpCache != null)
                        {
                            foreach (var kvp in eqpCache)
                            {
                                _workOrderLeftoversMap[kvp.Key] = kvp.Value;
                            }
                            OnLogReceived(LogLevel.Info, $"[LeftoversCache] Loaded {eqpCache.Count} item(s) from disk for {Id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[LeftoversCache] Failed to load cache from disk for {Id}: {ex.Message}");
            }
        }

        public void SaveLeftoversCacheToDisk()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_leftoversFileLock)
                {
                    string filePath = GetLeftoversCacheFilePath();
                    Dictionary<string, Dictionary<string, LeftoverRecordItem>> allCache = null;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, LeftoverRecordItem>>>(existingJson);
                        }
                        catch { }
                    }
                    allCache ??= new Dictionary<string, Dictionary<string, LeftoverRecordItem>>(StringComparer.OrdinalIgnoreCase);
                    allCache[this.Id] = new Dictionary<string, LeftoverRecordItem>(_workOrderLeftoversMap, StringComparer.OrdinalIgnoreCase);

                    string newJson = JsonConvert.SerializeObject(allCache, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[LeftoversCache] Failed to save cache to disk for {Id}: {ex.Message}");
            }
        }

        public void LoadProductLeftoversCache()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_productLeftoversFileLock)
                {
                    string filePath = GetProductLeftoversCacheFilePath();
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        var allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, ProductLeftoverRecordItem>>>(json);
                        if (allCache != null && allCache.TryGetValue(this.Id, out var eqpCache) && eqpCache != null)
                        {
                            foreach (var kvp in eqpCache)
                            {
                                _workOrderProductLeftoversMap[kvp.Key] = kvp.Value;
                            }
                            OnLogReceived(LogLevel.Info, $"[ProductLeftoversCache] Loaded {eqpCache.Count} item(s) from disk for {Id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[ProductLeftoversCache] Failed to load cache from disk for {Id}: {ex.Message}");
            }
        }

        public void SaveProductLeftoversCacheToDisk()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_productLeftoversFileLock)
                {
                    string filePath = GetProductLeftoversCacheFilePath();
                    Dictionary<string, Dictionary<string, ProductLeftoverRecordItem>> allCache = null;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, ProductLeftoverRecordItem>>>(existingJson);
                        }
                        catch { }
                    }
                    allCache ??= new Dictionary<string, Dictionary<string, ProductLeftoverRecordItem>>(StringComparer.OrdinalIgnoreCase);
                    allCache[this.Id] = new Dictionary<string, ProductLeftoverRecordItem>(_workOrderProductLeftoversMap, StringComparer.OrdinalIgnoreCase);

                    string newJson = JsonConvert.SerializeObject(allCache, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[ProductLeftoversCache] Failed to save cache to disk for {Id}: {ex.Message}");
            }
        }

        public void LoadInputQtyCache()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_inputQtyFileLock)
                {
                    string filePath = GetInputQtyCacheFilePath();
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        var allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
                        if (allCache != null && allCache.TryGetValue(this.Id, out var eqpCache) && eqpCache != null)
                        {
                            foreach (var kvp in eqpCache)
                            {
                                _workOrderInputQtyMap[kvp.Key] = kvp.Value;
                            }
                            OnLogReceived(LogLevel.Info, $"[InputQtyCache] Loaded {eqpCache.Count} item(s) from disk for {Id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[InputQtyCache] Failed to load cache from disk for {Id}: {ex.Message}");
            }
        }

        public void SaveInputQtyCacheToDisk()
        {
            if (string.IsNullOrEmpty(this.Id)) return;
            try
            {
                lock (_inputQtyFileLock)
                {
                    string filePath = GetInputQtyCacheFilePath();
                    Dictionary<string, Dictionary<string, double>> allCache = null;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(filePath);
                            allCache = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(existingJson);
                        }
                        catch { }
                    }
                    allCache ??= new Dictionary<string, Dictionary<string, double>>(StringComparer.OrdinalIgnoreCase);
                    allCache[this.Id] = new Dictionary<string, double>(_workOrderInputQtyMap, StringComparer.OrdinalIgnoreCase);

                    string newJson = JsonConvert.SerializeObject(allCache, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Warn, $"[InputQtyCache] Failed to save cache to disk for {Id}: {ex.Message}");
            }
        }

        public void RecordTotalCountReport(string workOrder, string reelCount, string qtyPerReel)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return;
            string trimmedWo = workOrder.Trim();
            var item = new TotalCountItem
            {
                EquipmentId = this.Id,
                WorkOrder = trimmedWo,
                ReelCount = reelCount?.Trim() ?? "",
                QtyPerReel = qtyPerReel?.Trim() ?? "",
                UpdatedTime = DateTime.Now
            };
            _workOrderTotalCountMap[trimmedWo] = item;
            SaveTotalCountCacheToDisk();
            OnLogReceived(LogLevel.Info, $"[TotalCountReport] Recorded for Eqp: {Id}, WO: {item.WorkOrder}, ReelCount: {item.ReelCount}, QtyPerReel: {item.QtyPerReel}");
        }

        public TotalCountItem GetTotalCount(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return null;
            return _workOrderTotalCountMap.TryGetValue(workOrder.Trim(), out var item) ? item : null;
        }

        public void RecordLeftoversReport(string workOrder, string qty, string reelId)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return;
            string trimmedWo = workOrder.Trim();
            var item = new LeftoverRecordItem
            {
                EquipmentId = this.Id,
                WorkOrder = trimmedWo,
                Qty = qty?.Trim() ?? "0",
                ReelId = reelId?.Trim() ?? "",
                UpdatedTime = DateTime.Now
            };
            _workOrderLeftoversMap[trimmedWo] = item;
            SaveLeftoversCacheToDisk();
            OnLogReceived(LogLevel.Info, $"[LeftoversReport] Recorded for Eqp: {Id}, WO: {item.WorkOrder}, ReelID: {item.ReelId}, Qty: {item.Qty}");
        }

        public LeftoverRecordItem GetLeftover(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return null;
            return _workOrderLeftoversMap.TryGetValue(workOrder.Trim(), out var item) ? item : null;
        }

        public void RecordProductLeftoverReport(string workOrder, string addQty, string gapQty, string reelId, string leftoverWo, string leftoverBatch)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return;
            string trimmedWo = workOrder.Trim();
            var item = new ProductLeftoverRecordItem
            {
                EquipmentId = this.Id,
                WorkOrder = trimmedWo,
                AddQty = addQty?.Trim() ?? "0",
                GapQty = gapQty?.Trim() ?? "0",
                ReelId = reelId?.Trim() ?? "",
                LeftoverWorkOrder = leftoverWo?.Trim() ?? "",
                LeftoverBatchNo = leftoverBatch?.Trim() ?? "",
                UpdatedTime = DateTime.Now
            };
            _workOrderProductLeftoversMap[trimmedWo] = item;
            SaveProductLeftoversCacheToDisk();
            OnLogReceived(LogLevel.Info, $"[ProductLeftoverReport] Recorded for Eqp: {Id}, WO: {item.WorkOrder}, AddQty: {item.AddQty}, GapQty: {item.GapQty}");
        }

        public ProductLeftoverRecordItem GetProductLeftover(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return null;
            return _workOrderProductLeftoversMap.TryGetValue(workOrder.Trim(), out var item) ? item : null;
        }

        public void RecordWorkOrderInputQty(string workOrder, double inputQty)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return;
            string trimmedWo = workOrder.Trim();
            _workOrderInputQtyMap[trimmedWo] = inputQty;
            SaveInputQtyCacheToDisk();
            OnLogReceived(LogLevel.Info, $"[InputQty] Recorded for Eqp: {Id}, WO: {trimmedWo}, InputQty: {inputQty}");
        }

        public double GetWorkOrderInputQty(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder)) return 0;
            return _workOrderInputQtyMap.TryGetValue(workOrder.Trim(), out double val) ? val : 0;
        }

        public (bool isSuccess, string message, int adjustedNgQty, double passQty) ValidateAndAdjustTpTrackOut(string workOrder)
        {
            if (string.IsNullOrWhiteSpace(workOrder))
            {
                return (false, "WorkOrder is empty", 0, 0);
            }

            string trimmedWo = workOrder.Trim();

            // 1. 檢查是否具備 LeftoversReport 與 TotalCountReport
            bool hasLeftovers = _workOrderLeftoversMap.TryGetValue(trimmedWo, out var leftoverItem);
            bool hasTotalCount = _workOrderTotalCountMap.TryGetValue(trimmedWo, out var totalCountItem);

            if (!hasLeftovers || !hasTotalCount)
            {
                var missingList = new List<string>();
                if (!hasLeftovers) missingList.Add("LeftoversReport");
                if (!hasTotalCount) missingList.Add("TotalCountReport");
                string missingMsg = $"TrackOut rejected: TP equipment '{Id}' requires both LeftoversReport and TotalCountReport before TrackOut for WO '{trimmedWo}'. Missing: {string.Join(", ", missingList)}";
                OnLogReceived(LogLevel.Warn, $"[TP TrackOut] {missingMsg}");
                return (false, missingMsg, 0, 0);
            }

            // 2. 提取數值與公式計算
            double inputQty = 0;
            if (_workOrderInputQtyMap.TryGetValue(trimmedWo, out double cachedInQty) && cachedInQty > 0)
            {
                inputQty = cachedInQty;
            }
            else
            {
                OnLogReceived(LogLevel.Warn, $"[TP TrackOut] InputQty for WO '{trimmedWo}' not found or <= 0. Defaulting to 0.");
            }

            double.TryParse(totalCountItem?.ReelCount, out double reelCount);
            double.TryParse(totalCountItem?.QtyPerReel, out double qtyPerReel);
            double.TryParse(leftoverItem?.Qty, out double leftoverQty);

            double addQty = 0;
            if (_workOrderProductLeftoversMap.TryGetValue(trimmedWo, out var pLeftoverItem))
            {
                double.TryParse(pLeftoverItem?.AddQty, out addQty);
            }

            double passQty = (reelCount * qtyPerReel) + leftoverQty - addQty;
            double calcNg = inputQty - passQty;

            OnLogReceived(LogLevel.Info, $"[TP TrackOut] WO: {trimmedWo}, InputQty: {inputQty}, ReelCount: {reelCount}, QtyPerReel: {qtyPerReel}, LeftoverQty: {leftoverQty}, AddQty: {addQty} => PassQty: {passQty}, CalcNG: {calcNg}");

            // 3. 判斷 1: 良率異常判斷 (Yield Check)
            if (inputQty > 0)
            {
                double yield = passQty / inputQty;
                if (yield < MinYieldRate)
                {
                    string failYieldLowMsg = $"TrackOut rejected: 良率過低異常 (Yield: {yield:P2} < {MinYieldRate:P2}). InputQty: {inputQty}, PassQty: {passQty}, CalcNG: {calcNg}. 需要人員介入處理。";
                    OnLogReceived(LogLevel.Error, $"[TP TrackOut] {failYieldLowMsg}");
                    return (false, failYieldLowMsg, 0, passQty);
                }
                if (yield > MaxYieldRate)
                {
                    string failYieldHighMsg = $"TrackOut rejected: 良率過高/溢出異常 (Yield: {yield:P2} > {MaxYieldRate:P2}). InputQty: {inputQty}, PassQty: {passQty}, CalcNG: {calcNg}. 需要人員介入處理。";
                    OnLogReceived(LogLevel.Error, $"[TP TrackOut] {failYieldHighMsg}");
                    return (false, failYieldHighMsg, 0, passQty);
                }
            }

            // 4. 判斷 2: NG 數特殊處理
            if (_workOrderNgCodesMap.TryGetValue(trimmedWo, out var ngDict) && ngDict != null)
            {
                var reportedItems = ngDict.Values.Where(x => x.IsReported && double.TryParse(x.NGQty, out double q) && q > 0).ToList();

                bool isSpecial(NGCodeItem it)
                {
                    string name = it.NGName?.Trim() ?? "";
                    string code = it.NGCode?.Trim() ?? "";
                    return name.Equals("QC Keep Sample", StringComparison.OrdinalIgnoreCase)
                        || name.Equals("免補料", StringComparison.OrdinalIgnoreCase)
                        || code.Equals("QC Keep Sample", StringComparison.OrdinalIgnoreCase)
                        || code.Equals("免補料", StringComparison.OrdinalIgnoreCase);
                }

                var specialItems = reportedItems.Where(isSpecial).ToList();
                var nonSpecialItems = reportedItems.Where(x => !isSpecial(x)).ToList();

                double specialSum = specialItems.Sum(x => double.TryParse(x.NGQty, out double q) ? q : 0);
                double totalReportedSum = reportedItems.Sum(x => double.TryParse(x.NGQty, out double q) ? q : 0);

                // 狀況 a: 特殊項目總和 > CalcNG => NG數溢出
                if (specialSum > calcNg)
                {
                    string failOverflowMsg = $"TrackOut rejected: NG數溢出 (Special NG items 'QC Keep Sample' and '免補料' total: {specialSum} > CalcNG: {calcNg}). 需要人員介入處理。";
                    OnLogReceived(LogLevel.Error, $"[TP TrackOut] {failOverflowMsg}");
                    return (false, failOverflowMsg, 0, passQty);
                }

                // 狀況 b: CalcNG < 機台上報所有 NG 總和 => 剩餘量平準化扣除非特殊項目 (特殊項目 QC Keep Sample 與 免補料 嚴格不扣)
                if (calcNg < totalReportedSum)
                {
                    double diff = totalReportedSum - calcNg;
                    OnLogReceived(LogLevel.Info, $"[TP TrackOut] CalcNG ({calcNg}) < Total Reported NG ({totalReportedSum}). Diff to deduct: {diff}. Balancing remaining levels of non-special items...");

                    var candidateItems = nonSpecialItems
                        .Where(x => double.TryParse(x.NGQty, out double q) && q > 0)
                        .Select(x => new { Item = x, OriginalQty = double.Parse(x.NGQty) })
                        .ToList();

                    if (candidateItems.Count > 0)
                    {
                        double nonSpecialOriginalSum = candidateItems.Sum(x => x.OriginalQty);
                        double targetRemainingTotal = Math.Max(0, nonSpecialOriginalSum - diff); // 目標非特殊剩餘總數 = CalcNG - SpecialSum

                        var remainingAllocations = candidateItems.ToDictionary(x => x.Item, x => 0.0);
                        double remainingToAllocate = targetRemainingTotal;

                        // 水位平準化分配 (Water-Level Fill)：由剩餘量最低者開始填補，上限為其 OriginalQty
                        while (remainingToAllocate > 0.0001)
                        {
                            var eligible = candidateItems
                                .Where(x => remainingAllocations[x.Item] < x.OriginalQty - 0.0001)
                                .OrderBy(x => remainingAllocations[x.Item])
                                .ThenBy(x => x.OriginalQty)
                                .ThenBy(x => x.Item.NGCode)
                                .ToList();

                            if (eligible.Count == 0) break;

                            double minAlloc = remainingAllocations[eligible.First().Item];
                            var lowestGroup = eligible.Where(x => Math.Abs(remainingAllocations[x.Item] - minAlloc) < 0.0001).ToList();

                            double step = Math.Floor(remainingToAllocate / lowestGroup.Count);
                            if (step <= 0) step = 1;

                            bool allocatedAny = false;
                            foreach (var c in lowestGroup)
                            {
                                if (remainingToAllocate <= 0.0001) break;

                                double maxAdd = c.OriginalQty - remainingAllocations[c.Item];
                                double toAdd = Math.Min(maxAdd, Math.Min(step, remainingToAllocate));

                                if (toAdd > 0)
                                {
                                    remainingAllocations[c.Item] += toAdd;
                                    remainingToAllocate -= toAdd;
                                    allocatedAny = true;
                                }
                            }

                            if (!allocatedAny) break;
                        }

                        foreach (var c in candidateItems)
                        {
                            double newQty = remainingAllocations[c.Item];
                            double deduct = c.OriginalQty - newQty;
                            c.Item.NGQty = newQty.ToString();
                            OnLogReceived(LogLevel.Info, $"[TP TrackOut] Level-adjusted NGCode '{c.Item.NGCode}' ({c.Item.NGName}): Original {c.OriginalQty} -> Deducted {deduct} -> Remaining {newQty}.");
                        }

                        SaveNgCodeCacheToDisk();
                    }
                }
            }

            return (true, "OK", (int)Math.Round(calcNg), passQty);
        }

        public IReadOnlyDictionary<string, TotalCountItem> GetAllTotalCounts() => _workOrderTotalCountMap;
        private static readonly object _logLock = new object();
        private static readonly HttpClient _httpClient = new HttpClient();

        protected virtual TReply CallMesWithLog<TRequest, TReply>(TRequest request) where TRequest : MESBaseMessage where TReply : MESBaseMessage
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                string prettyReq = MesJsonContractResolver.Serialize(request, Newtonsoft.Json.Formatting.Indented);
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
                    string prettyReply = MesJsonContractResolver.Serialize(reply, Newtonsoft.Json.Formatting.Indented);
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
        public bool IsTpEquipment => !string.IsNullOrEmpty(Id) && Id.StartsWith("TP", StringComparison.OrdinalIgnoreCase);
        public string Name { get; set; }
        public string Protocol { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSimulator { get; set; } = false;
        public bool IsMesSimulator { get; set; } = false;
        public bool EnableCassetteBinding { get; set; } = false;
        public int StatusReportIntervalSeconds { get; set; } = 60;
        public LocalDataService LocalDataServiceInstance { get; set; }
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
                string safeWo = woNo.Length > 50 ? woNo.Substring(0, 50) : woNo;
                LocalDataServiceInstance.SetCurrentWorkOrder(Id, safeWo, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"SetCurrentWorkOrder LocalDb Error: {ex.Message}");
            }
        }

        public void WriteOutputLocalDb(string woNo, int outputQty, int ngQty = 0)
        {
            if (LocalDataServiceInstance == null || string.IsNullOrEmpty(woNo)) return;
            try
            {
                string safeWo = woNo.Length > 50 ? woNo.Substring(0, 50) : woNo;
                LocalDataServiceInstance.WriteOutput(Id, safeWo, outputQty, ngQty, DateTimeOffset.UtcNow);
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

        static EquipmentBase()
        {
            MesJsonContractResolver.ConfigureDefaultSettings();
        }

        public EquipmentBase()
        {
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

        public async Task<string> ProcessUserVerifyAsync(string barcode, string userId, string userPassword = null, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                OnLogReceived(LogLevel.Info, $"[Business] {eid} User Verify: Barcode={barcode}, UserID={userId}, Password={(string.IsNullOrEmpty(userPassword) ? "N/A" : "***")}");
                var req = new UserVerifyRequest { EqpNo = eid, User2DBarCode = barcode, UserID = userId, UserPassword = userPassword };
                
                var replyTask = Task.Run(() => CallMesWithLog<UserVerifyRequest, UserVerifyReply>(req));
                if (await Task.WhenAny(replyTask, Task.Delay(25000)) != replyTask)
                    return "FAIL|T001|MES Call Timeout (25s)|";

                var reply = await replyTask;
                string resPrefix = (reply.Result == "success") ? "PASS" : "FAIL";
                string returnMsg = !string.IsNullOrEmpty(reply.Message) ? reply.Message : reply.UserID;
                return $"{resPrefix}|{reply.ResultCode}|{returnMsg}|{reply.UserID}";
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

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).Distinct().ToList();
                if (validWos.Count == 0) return "FAIL|W001|Cassette Not Bound to WO|[]";

                string combinedWo = string.Join(",", validWos);
                var woRecipeMap = new Dictionary<string, string>();
                var allReqInputCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var alreadyInWos = new List<string>();
                string lastResultCode = "W001";
                string lastMessage = "";
                string lastInputParamListDT = "";

                bool isTp = (!string.IsNullOrEmpty(reportEqpId) ? reportEqpId : this.Id)?.StartsWith("TP", StringComparison.OrdinalIgnoreCase) == true;

                // 分次逐筆向 MES 查詢工單 (WOQRY)
                foreach (var wo in validWos)
                {
                    var qryReq = new WOQRYRequest { WONO = wo, EqpNo = eid, UserID = userId };
                    var qryTask = Task.Run(() => CallMesWithLog<WOQRYRequest, WOQRYReply>(qryReq));
                    if (await Task.WhenAny(qryTask, Task.Delay(25000)) != qryTask)
                    {
                        string timeoutMsg = $"MES WOQRY Timeout (25s) for WO: {wo}";
                        OnLogReceived(LogLevel.Error, $"[TrackIn] {timeoutMsg}");
                        return $"FAIL|T001|{timeoutMsg}|[]";
                    }

                    var qryReply = await qryTask;
                    if (qryReply == null || (qryReply.Result != "success" && qryReply.Result != "PASS"))
                    {
                        string failMsg = $"MES WOQRY Failed for WO {wo}: {qryReply?.Message ?? "No response"}";
                        OnLogReceived(LogLevel.Error, $"[TrackIn] {failMsg}");

                        if (string.Equals(qryReply?.ResultCode, "ERROR0049", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string terminalMsg = $"WO {wo} is not valid for check-in at this station";
                                if (terminalMsg.Length > 200) terminalMsg = terminalMsg.Substring(0, 197) + "...";
                                await SendTerminalDisplay(terminalMsg);
                            }
                            catch (Exception ex)
                            {
                                OnLogReceived(LogLevel.Warn, $"[TrackIn] SendTerminalDisplay failed: {ex.Message}");
                            }
                        }

                        return $"FAIL|{qryReply?.ResultCode ?? "ERR_MES"}|{failMsg}|[]";
                    }

                    // 檢查 AlreadyInFlag (是否已進站)
                    string alreadyInFlag = null;
                    try
                    {
                        alreadyInFlag = (qryReply as dynamic)?.AlreadyInFlag?.ToString()?.Trim();
                    }
                    catch { }

                    if ("Y".Equals(alreadyInFlag, StringComparison.OrdinalIgnoreCase))
                    {
                        alreadyInWos.Add(wo);
                    }

                    lastResultCode = qryReply.ResultCode ?? "W001";
                    lastMessage = qryReply.Message ?? "";
                    lastInputParamListDT = qryReply.InputParamListDT ?? "";

                    string recipe = ExtractRecipeFromMes(qryReply.InputParamListDT) ?? "";
                    woRecipeMap[wo] = recipe;

                    // 收集進站管理項目代碼
                    if (qryReply.InputCodeList != null && qryReply.InputCodeList.Count > 0)
                    {
                        foreach (var item in qryReply.InputCodeList)
                        {
                            if ("Y".Equals(item.RequireInput, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(item.InputCode))
                            {
                                allReqInputCodes.Add(item.InputCode.Trim());
                            }
                        }
                    }

                    // 檢查並收集工單的 NG 碼清單 (NGCodeListDT) - 僅限 TP 設備
                    if (isTp)
                    {
                        string ngCodeListDtStr = null;
                        try
                        {
                            ngCodeListDtStr = (qryReply as dynamic)?.NGCodeListDT?.ToString();
                        }
                        catch { }

                        if (!string.IsNullOrWhiteSpace(ngCodeListDtStr) && ngCodeListDtStr != "[]")
                        {
                            try
                            {
                                var ngArray = JArray.Parse(ngCodeListDtStr);
                                var ngDict = new ConcurrentDictionary<string, NGCodeItem>(StringComparer.OrdinalIgnoreCase);
                                foreach (var item in ngArray)
                                {
                                    string code = item["NGCode"]?.ToString()?.Trim();
                                    string name = item["NGName"]?.ToString()?.Trim();
                                    if (!string.IsNullOrEmpty(code))
                                    {
                                        ngDict[code] = new NGCodeItem
                                        {
                                            NGCode = code,
                                            NGName = name ?? "",
                                            NGQty = null,
                                            NGUnit = ""
                                        };
                                    }
                                }
                                if (ngDict.Count > 0)
                                {
                                    _workOrderNgCodesMap[wo] = ngDict;
                                    SaveNgCodeCacheToDisk();
                                    OnLogReceived(LogLevel.Info, $"[TrackIn] Loaded {ngDict.Count} NGCode(s) for WO {wo}: {string.Join(",", ngDict.Keys)}");
                                }
                            }
                            catch (Exception ex)
                            {
                                OnLogReceived(LogLevel.Warn, $"[TrackIn] Parse NGCodeListDT failed for WO {wo}: {ex.Message}");
                            }
                        }
                    }

                    // 收集工單投入數 (MES WOQRY WIPQty)
                    try
                    {
                        string rawQtyStr = (qryReply as dynamic)?.WIPQty?.ToString();
                        if (double.TryParse(rawQtyStr, out double inQty) && inQty > 0)
                        {
                            RecordWorkOrderInputQty(wo, inQty);
                        }
                        else
                        {
                            OnLogReceived(LogLevel.Warn, $"[TrackIn] WIPQty for WO {wo} is invalid or empty: '{rawQtyStr}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Warn, $"[TrackIn] Parse WIPQty failed for WO {wo}: {ex.Message}");
                    }
                }

                // 檢查是否已進站 (單工單與多工單皆防呆阻擋)
                if (alreadyInWos.Count > 0)
                {
                    string alreadyInList = string.Join(",", alreadyInWos);
                    string errorMsg = $"{alreadyInList} 已進站，禁止重複進站";
                    OnLogReceived(LogLevel.Error, $"[TrackIn] {errorMsg}");

                    try
                    {
                        string terminalMsg = $"WO ALREADY CHECKED IN: {alreadyInList}";
                        if (terminalMsg.Length > 200) terminalMsg = terminalMsg.Substring(0, 197) + "...";
                        await SendTerminalDisplay(terminalMsg);
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Warn, $"[TrackIn] SendTerminalDisplay failed: {ex.Message}");
                    }

                    return $"FAIL|ERR_ALREADY_IN|{errorMsg}|[]";
                }

                // 比對所有工單查到的 RECIPEID 是否一致
                var distinctRecipes = woRecipeMap.Values.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                if (distinctRecipes.Count > 1)
                {
                    // Recipe 不一致，跳過 PP_SELECT，記錄錯誤日誌並直接回傳 TrackIn FAIL
                    var details = woRecipeMap.Select((kv, idx) => $"WO_{idx + 1}:{kv.Key}[{kv.Value}]");
                    string errorMsg = $"Recipe Mismatch! {string.Join(",", details)}";
                    
                    OnLogReceived(LogLevel.Error, $"[TrackIn] {errorMsg}");
                    try
                    {
                        string safeTerminalMsg = errorMsg.Length > 200 ? errorMsg.Substring(0, 197) + "..." : errorMsg;
                        await SendTerminalDisplay(safeTerminalMsg);
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Warn, $"[TrackIn] SendTerminalDisplay failed: {ex.Message}");
                    }

                    return $"FAIL|R001|{errorMsg}|[]";
                }

                string unifiedRecipe = distinctRecipes.FirstOrDefault() ?? "";

                // 單工單或查無 Recipe ID / 模擬 Recipe 異常判定
                if (validWos.Count == 1 && (string.IsNullOrWhiteSpace(unifiedRecipe) 
                    || unifiedRecipe.Equals("UNKNOWN", StringComparison.OrdinalIgnoreCase) 
                    || unifiedRecipe.Equals("ERROR", StringComparison.OrdinalIgnoreCase) 
                    || unifiedRecipe.StartsWith("RECIPE_MISMATCH_", StringComparison.OrdinalIgnoreCase) 
                    || unifiedRecipe.StartsWith("SIM_RECIPE_DIFF_", StringComparison.OrdinalIgnoreCase)))
                {
                    string errorMsg = "在MES找不到RECIPEID";
                    OnLogReceived(LogLevel.Error, $"[TrackIn] {errorMsg}");
                    try
                    {
                        await SendTerminalDisplay("RECIPEID NOT FOUND IN MES");
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Warn, $"[TrackIn] SendTerminalDisplay failed: {ex.Message}");
                    }

                    return $"FAIL|Y94|{errorMsg}|[]";
                }

                if (string.IsNullOrWhiteSpace(unifiedRecipe) 
                    || unifiedRecipe.Equals("UNKNOWN", StringComparison.OrdinalIgnoreCase) 
                    || unifiedRecipe.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
                {
                    string errorMsg = "在MES找不到RECIPEID";
                    OnLogReceived(LogLevel.Error, $"[TrackIn] {errorMsg}");
                    try
                    {
                        await SendTerminalDisplay("RECIPEID NOT FOUND IN MES");
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Warn, $"[TrackIn] SendTerminalDisplay failed: {ex.Message}");
                    }

                    return $"FAIL|Y94|{errorMsg}|[]";
                }

                _trackInRequireInputMap[combinedWo] = allReqInputCodes.ToList();

                string panelsJson = JsonConvert.SerializeObject(currentPanels);
                return $"PP-SELECT|{combinedWo}|{unifiedRecipe}|{lastResultCode}|{lastMessage}|{lastInputParamListDT}|{panelsJson}";
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

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).Distinct().ToList();
                string combinedWo = string.Join(",", validWos);
                var validCids = cids.Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();

                string resPrefix = "FAIL";
                string resultCode = "";
                string resultMessage = "";

                if (validWos.Count > 1)
                {
                    // 多工單進站報工改發 WOCHECKIN_MULTIRequest
                    var multiReq = new WOCHECKIN_MULTIRequest
                    {
                        EqpNo = eid,
                        UserID = userId,
                        WoNoDt = JsonConvert.SerializeObject(validWos.Select(w => new WoNoListItem { WoNo = w }).ToList())
                    };

                    if (validCids.Count > 0)
                    {
                        multiReq.WOCarrierDt = JsonConvert.SerializeObject(validCids.Select(c => new CarrierItem { CarrierNo = c }).ToList());
                    }

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
                            multiReq.InputListDt = JsonConvert.SerializeObject(inputItems);
                        }
                    }

                    var checkinTask = Task.Run(() => CallMesWithLog<WOCHECKIN_MULTIRequest, WOCHECKIN_MULTIReply>(multiReq));
                    if (await Task.WhenAny(checkinTask, Task.Delay(25000)) != checkinTask)
                        return $"FAIL|T001|MES WOCHECKIN_MULTI Timeout (25s) for WOs: {combinedWo}|";

                    var reply = await checkinTask;
                    resPrefix = (reply?.Result == "success" || reply?.Result == "PASS") ? "PASS" : "FAIL";
                    resultCode = reply?.ResultCode ?? "";
                    resultMessage = reply?.Message ?? "";
                }
                else
                {
                    // 單工單進站報工維持標準 WOCHECKINRequest
                    var singleReq = new WOCHECKINRequest { WONO = combinedWo, EqpNo = eid, UserID = userId };
                    if (validCids.Count > 0)
                    {
                        singleReq.WOCarrierDt = JsonConvert.SerializeObject(validCids.Select(c => new CarrierItem { CarrierNo = c }).ToList());
                    }

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
                            singleReq.InputListDt = JsonConvert.SerializeObject(inputItems);
                        }
                    }

                    var checkinTask = Task.Run(() => CallMesWithLog<WOCHECKINRequest, WOCHECKINReply>(singleReq));
                    if (await Task.WhenAny(checkinTask, Task.Delay(25000)) != checkinTask)
                        return $"FAIL|T001|MES WOCHECKIN Timeout (25s) for WOs: {combinedWo}|";

                    var reply = await checkinTask;
                    resPrefix = (reply?.Result == "success" || reply?.Result == "PASS") ? "PASS" : "FAIL";
                    resultCode = reply?.ResultCode ?? "";
                    resultMessage = reply?.Message ?? "";
                }

                if (resPrefix != "PASS")
                {
                    OnLogReceived(LogLevel.Error, $"[TrackIn] MES WOCHECKIN Failed for WOs {combinedWo}. Code: {resultCode}, Message: {resultMessage}");
                    return $"{resPrefix}|{resultCode}|MES WOCHECKIN Failed for WOs {combinedWo}: {resultMessage}";
                }

                foreach (var wo in validWos)
                {
                    SetCurrentWorkOrderLocalDb(wo);
                }

                // 進站成功後，逐筆發送第二次工單查詢 (WOQRYRequest) 獲取出站所需管理項目
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var allTrackOutCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (var wo in validWos)
                        {
                            var qryReq2 = new WOQRYRequest { WONO = wo, EqpNo = eid, UserID = userId };
                            var qryReply2 = CallMesWithLog<WOQRYRequest, WOQRYReply>(qryReq2);
                            if (qryReply2?.Result != "success" && qryReply2?.Result != "PASS")
                            {
                                OnLogReceived(LogLevel.Warn, $"[TrackIn] Second WOQRY failed for WO {wo}: {qryReply2?.Message}");
                                continue;
                            }

                            if (qryReply2.InputCodeList != null && qryReply2.InputCodeList.Count > 0)
                            {
                                foreach (var item in qryReply2.InputCodeList)
                                {
                                    if ("Y".Equals(item.RequireInput, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(item.InputCode))
                                    {
                                        allTrackOutCodes.Add(item.InputCode.Trim());
                                    }
                                }
                            }
                        }

                        _trackOutRequireInputMap[combinedWo] = allTrackOutCodes.ToList();
                        OnLogReceived(LogLevel.Info, $"[TrackIn] Second WOQRY completed for WOs {combinedWo}. RequireInput codes for TrackOut: {string.Join(",", allTrackOutCodes)}");
                    }
                    catch (Exception ex)
                    {
                        OnLogReceived(LogLevel.Error, $"[TrackIn] Second WOQRY exception for WOs {combinedWo}: {ex.Message}");
                        _trackOutRequireInputMap[combinedWo] = new List<string>();
                    }
                });

                return $"PASS|{resultCode}|{resultMessage}";
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

        public bool RecordNGCodeReport(string workOrder, string ngCode, string ngName, string ngQty, string ngUnit)
        {
            if (string.IsNullOrWhiteSpace(workOrder) || string.IsNullOrWhiteSpace(ngCode))
            {
                OnLogReceived(LogLevel.Warn, $"[NGCodeReport] Missing WorkOrder or NGCode (WO: '{workOrder}', NGCode: '{ngCode}')");
                return false;
            }

            string trimmedWo = workOrder.Trim();
            string trimmedCode = ngCode.Trim();

            if (_workOrderNgCodesMap.TryGetValue(trimmedWo, out var ngDict) && ngDict != null)
            {
                if (ngDict.TryGetValue(trimmedCode, out var item))
                {
                    item.NGQty = ngQty ?? "0";
                    if (!string.IsNullOrEmpty(ngUnit)) item.NGUnit = ngUnit.Trim();
                    if (!string.IsNullOrEmpty(ngName)) item.NGName = ngName.Trim();
                    SaveNgCodeCacheToDisk();
                    OnLogReceived(LogLevel.Info, $"[NGCodeReport] Recorded WO: {trimmedWo}, NGCode: {trimmedCode} ({item.NGName}), Qty: {item.NGQty}, Unit: {item.NGUnit}");
                    return true;
                }
                else
                {
                    OnLogReceived(LogLevel.Info, $"[NGCodeReport] Skipped: NGCode '{trimmedCode}' is not in the registered list for WO '{trimmedWo}'");
                    return false;
                }
            }
            else
            {
                OnLogReceived(LogLevel.Info, $"[NGCodeReport] Skipped: No registered NGCode list found for WO '{trimmedWo}'");
                return false;
            }
        }

        private void SetPropertySafely(object target, string propertyName, object value)
        {
            if (target == null || string.IsNullOrEmpty(propertyName)) return;
            try
            {
                var prop = target.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(target, value);
                }
            }
            catch { }
        }

        public async Task<(bool isSuccess, string resultCode, string message)> ProcessEqpWONGAddAsync(
            string workOrder, 
            string ngCode, 
            string ngQty, 
            string ngUnit = "", 
            string flowId = "", 
            string userId = null, 
            string reportEqpId = null)
        {
            string eid = !string.IsNullOrEmpty(reportEqpId) ? reportEqpId : this.Id;
            string wo = workOrder?.Trim() ?? "";
            string code = ngCode?.Trim() ?? "";
            string qty = ngQty?.Trim() ?? "0";
            string unit = ngUnit?.Trim() ?? "";
            string fid = flowId?.Trim() ?? "";

            if (string.IsNullOrEmpty(wo) || string.IsNullOrEmpty(code))
            {
                string err = $"Missing WorkOrder ('{wo}') or NGCode ('{code}')";
                OnLogReceived(LogLevel.Warn, $"[EqpWONGAdd] {err}");
                return (false, "ERR_INVALID_PARAM", err);
            }

            try
            {
                var req = new EqpWONGAddRequest
                {
                    WONo = wo,
                    FlowID = fid,
                    EqpNo = eid,
                    UserID = userId ?? ""
                };

                var ngItems = new List<Protocol.MES.Messages.NGListItem>
                {
                    new Protocol.MES.Messages.NGListItem { NGCode = code, Value = qty, NGUnit = unit }
                };
                req.NGListDt = JsonConvert.SerializeObject(ngItems);

                var addngTask = Task.Run(() => CallMesWithLog<EqpWONGAddRequest, EqpWONGAddReply>(req));
                if (await Task.WhenAny(addngTask, Task.Delay(25000)) != addngTask)
                {
                    string timeoutMsg = $"MES EqpWONGAdd Timeout (25s) for WO: {wo}, NGCode: {code}";
                    OnLogReceived(LogLevel.Error, $"[EqpWONGAdd] {timeoutMsg}");
                    return (false, "T001", timeoutMsg);
                }

                var reply = await addngTask;
                if (reply != null && (string.Equals(reply.Result, "success", StringComparison.OrdinalIgnoreCase) || string.Equals(reply.Result, "PASS", StringComparison.OrdinalIgnoreCase)))
                {
                    return (true, reply.ResultCode ?? "000", reply.Message ?? "Success");
                }
                else
                {
                    return (false, reply?.ResultCode ?? "ERR_MES", reply?.Message ?? "MES EqpWONGAdd Failed");
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[EqpWONGAdd] Exception: {ex.Message}");
                return (false, "EXCEPTION", ex.Message);
            }
        }

        public async Task<(bool isSuccess, string message)> ProcessAddProductLeftoverAsync(string workOrder, string qty, string reelId, string userId = null, string reportEqpId = null)
        {
            string eid = !string.IsNullOrEmpty(reportEqpId) ? reportEqpId : this.Id;
            string wo = workOrder?.Trim() ?? "";
            string q = qty?.Trim() ?? "0";
            string rid = reelId?.Trim() ?? "";

            if (string.IsNullOrEmpty(wo) || string.IsNullOrEmpty(rid))
            {
                string err = $"Missing WorkOrder ('{wo}') or ReelID ('{rid}')";
                OnLogReceived(LogLevel.Warn, $"[AddProductLeftover] {err}");
                return (false, err);
            }

            try
            {
                var req = new AddProductLeftoverRequest
                {
                    WONO = wo,
                    Qty = q,
                    ReelID = rid,
                    EqpNo = eid,
                    UserID = !string.IsNullOrEmpty(userId) ? userId : "EAP"
                };

                var leftoverTask = Task.Run(() => CallMesWithLog<AddProductLeftoverRequest, AddProductLeftoverReply>(req));
                if (await Task.WhenAny(leftoverTask, Task.Delay(25000)) != leftoverTask)
                {
                    string timeoutMsg = $"MES AddProductLeftover Timeout (25s) for WO: {wo}, ReelID: {rid}";
                    OnLogReceived(LogLevel.Error, $"[AddProductLeftover] {timeoutMsg}");
                    return (false, timeoutMsg);
                }

                var reply = await leftoverTask;
                if (reply != null && (string.Equals(reply.Result, "success", StringComparison.OrdinalIgnoreCase) || string.Equals(reply.Result, "PASS", StringComparison.OrdinalIgnoreCase)))
                {
                    RecordLeftoversReport(wo, q, rid);
                    return (true, reply.Message ?? "Success");
                }
                else
                {
                    return (false, reply?.Message ?? "MES AddProductLeftover Failed");
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[AddProductLeftover] Exception: {ex.Message}");
                return (false, ex.Message);
            }
        }

        public async Task<(bool isSuccess, string message)> ProcessUseProductLeftoverAsync(string workOrder, string reelId, string leftoverWorkOrder, string leftoverBatchNo, string addQty, string gapQty, string userId = null, string reportEqpId = null)
        {
            string eid = !string.IsNullOrEmpty(reportEqpId) ? reportEqpId : this.Id;
            string wo = workOrder?.Trim() ?? "";
            string rid = reelId?.Trim() ?? "";
            string lWo = leftoverWorkOrder?.Trim() ?? "";
            string lBatch = leftoverBatchNo?.Trim() ?? "";
            string aQty = addQty?.Trim() ?? "0";
            string gQty = gapQty?.Trim() ?? "0";

            if (string.IsNullOrEmpty(wo) || string.IsNullOrEmpty(rid))
            {
                string err = $"Missing WorkOrder ('{wo}') or ReelID ('{rid}')";
                OnLogReceived(LogLevel.Warn, $"[UseProductLeftover] {err}");
                return (false, err);
            }

            try
            {
                var req = new UseProductLeftoverRequest
                {
                    WONO = wo,
                    ReelID = rid,
                    LeftOverWONo = lWo,
                    LeftOverBatchNo = lBatch,
                    AddQty = aQty,
                    GapQty = gQty,
                    EqpNo = eid,
                    UserID = !string.IsNullOrEmpty(userId) ? userId : "EAP"
                };

                var useTask = Task.Run(() => CallMesWithLog<UseProductLeftoverRequest, UseProductLeftoverReply>(req));
                if (await Task.WhenAny(useTask, Task.Delay(25000)) != useTask)
                {
                    string timeoutMsg = $"MES UseProductLeftover Timeout (25s) for WO: {wo}, ReelID: {rid}";
                    OnLogReceived(LogLevel.Error, $"[UseProductLeftover] {timeoutMsg}");
                    return (false, timeoutMsg);
                }

                var reply = await useTask;
                if (reply != null && (string.Equals(reply.Result, "success", StringComparison.OrdinalIgnoreCase) || string.Equals(reply.Result, "PASS", StringComparison.OrdinalIgnoreCase)))
                {
                    RecordProductLeftoverReport(wo, aQty, gQty, rid, lWo, lBatch);
                    return (true, reply.Message ?? "Success");
                }
                else
                {
                    return (false, reply?.Message ?? "MES UseProductLeftover Failed");
                }
            }
            catch (Exception ex)
            {
                OnLogReceived(LogLevel.Error, $"[UseProductLeftover] Exception: {ex.Message}");
                return (false, ex.Message);
            }
        }

        public async Task<string> ProcessTrackOutAsync(List<string> workOrders, string qty, string userId, List<string> cassetteIds, List<string> panels, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                List<string> wos = workOrders ?? new List<string>();
                List<string> cids = cassetteIds ?? new List<string>();

                var validWos = wos.Where(w => !string.IsNullOrEmpty(w)).Distinct().ToList();
                string combinedWo = string.Join(",", validWos);

                OnLogReceived(LogLevel.Info, $"[WOCHECKOUT] {eid} - WOs: {combinedWo}, Qty: {qty}, CIDs: {string.Join(",", cids)}");

                // 僅 TP 開頭設備檢核所有工單的 NGCode 是否皆已全數上報
                bool isTp = (!string.IsNullOrEmpty(reportEqpId) ? reportEqpId : this.Id)?.StartsWith("TP", StringComparison.OrdinalIgnoreCase) == true;
                if (isTp)
                {
                    foreach (var wo in validWos)
                    {
                        if (_workOrderNgCodesMap.TryGetValue(wo, out var ngDict) && ngDict != null && ngDict.Count > 0)
                        {
                            var missingCodes = ngDict.Values.Where(x => !x.IsReported).Select(x => x.NGCode).ToList();
                            if (missingCodes.Count > 0)
                            {
                                string missingListStr = string.Join(",", missingCodes);
                                OnLogReceived(LogLevel.Error, $"[TrackOut] Rejected WO {wo}: Unreported NGCode(s): [{missingListStr}]");
                                return $"FAIL|NG_CHECK_FAIL|Unreported NGCode(s): {missingListStr}";
                            }
                        }
                    }
                }

                // 收集所有上報的 NG 碼資料組裝成 NGListDt
                var allNgItems = new List<NGItem>();
                foreach (var wo in validWos)
                {
                    if (_workOrderNgCodesMap.TryGetValue(wo, out var ngDict) && ngDict != null && ngDict.Count > 0)
                    {
                        foreach (var item in ngDict.Values)
                        {
                            allNgItems.Add(new NGItem
                            {
                                NGCode = item.NGCode,
                                NGQty = item.NGQty ?? "0",
                                NGUnit = item.NGUnit ?? ""
                            });
                        }
                    }
                }
                string ngListDtJson = allNgItems.Count > 0 ? JsonConvert.SerializeObject(allNgItems) : null;

                string resPrefix = "FAIL";
                string resultCode = "";
                string resultMessage = "";

                if (validWos.Count > 1)
                {
                    // 多工單出站報工改發 WOCHECKOUT_MULTIRequest
                    var multiReq = new WOCHECKOUT_MULTIRequest
                    {
                        EqpNo = eid,
                        UserID = userId,
                        Operator = userId,
                        OutPut = qty,
                        WoNoDt = JsonConvert.SerializeObject(validWos.Select(w => new WoNoListItem { WoNo = w }).ToList())
                    };

                    if (!string.IsNullOrEmpty(ngListDtJson))
                    {
                        SetPropertySafely(multiReq, "NGListDt", ngListDtJson);
                    }

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
                            multiReq.InputListDt = JsonConvert.SerializeObject(inputItems);
                        }
                    }

                    var checkoutTask = Task.Run(() => CallMesWithLog<WOCHECKOUT_MULTIRequest, WOCHECKOUT_MULTIReply>(multiReq));
                    if (await Task.WhenAny(checkoutTask, Task.Delay(25000)) != checkoutTask)
                        return $"FAIL|T001|MES WOCHECKOUT_MULTI Timeout (25s) for WOs: {combinedWo}|";

                    var reply = await checkoutTask;
                    resPrefix = (reply?.Result == "success" || reply?.Result == "PASS") ? "PASS" : "FAIL";
                    resultCode = reply?.ResultCode ?? "";
                    resultMessage = reply?.Message ?? "";
                }
                else
                {
                    // 單工單出站報工維持標準 WOCHECKOUTRequest
                    var singleReq = new WOCHECKOUTRequest { WONO = combinedWo, EqpNo = eid, UserID = userId, Operator = userId, OutPut = qty };

                    if (!string.IsNullOrEmpty(ngListDtJson))
                    {
                        SetPropertySafely(singleReq, "NGListDt", ngListDtJson);
                    }

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
                            singleReq.InputListDt = JsonConvert.SerializeObject(inputItems);
                        }
                    }

                    var checkoutTask = Task.Run(() => CallMesWithLog<WOCHECKOUTRequest, WOCHECKOUTReply>(singleReq));
                    if (await Task.WhenAny(checkoutTask, Task.Delay(25000)) != checkoutTask)
                        return $"FAIL|T001|MES WOCHECKOUT Timeout (25s) for WOs: {combinedWo}|";

                    var reply = await checkoutTask;
                    resPrefix = (reply?.Result == "success" || reply?.Result == "PASS") ? "PASS" : "FAIL";
                    resultCode = reply?.ResultCode ?? "";
                    resultMessage = reply?.Message ?? "";
                }

                if (resPrefix != "PASS")
                {
                    OnLogReceived(LogLevel.Error, $"[TrackOut] MES WOCHECKOUT Failed for WOs {combinedWo}. Code: {resultCode}, Message: {resultMessage}");
                    return $"{resPrefix}|{resultCode}|MES WOCHECKOUT Failed for WOs {combinedWo}: {resultMessage}";
                }

                // 清理此工單快取
                _trackInRequireInputMap.TryRemove(combinedWo, out _);
                _trackOutRequireInputMap.TryRemove(combinedWo, out _);
                foreach (var wo in validWos)
                {
                    _workOrderNgCodesMap.TryRemove(wo, out _);
                    _workOrderTotalCountMap.TryRemove(wo, out _);
                    _workOrderLeftoversMap.TryRemove(wo, out _);
                    _workOrderProductLeftoversMap.TryRemove(wo, out _);
                    _workOrderInputQtyMap.TryRemove(wo, out _);
                }
                SaveNgCodeCacheToDisk();
                SaveTotalCountCacheToDisk();
                SaveLeftoversCacheToDisk();
                SaveProductLeftoversCacheToDisk();
                SaveInputQtyCacheToDisk();

                if (int.TryParse(qty, out int outQty))
                {
                    foreach (var wo in validWos)
                    {
                        WriteOutputLocalDb(wo, outQty, 0);
                    }
                }

                return $"PASS|{resultCode}|{resultMessage}";
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

        public async Task<string> ProcessStatusChangeAsync(string oldStatus, string newStatus, string reportEqpId = null)
        {
            try
            {
                string eid = reportEqpId ?? this.Id;
                string newCode = int.TryParse(newStatus, out int nVal) ? nVal.ToString() : newStatus?.Trim();
                if (!string.IsNullOrEmpty(newCode) && string.Equals(newCode, _lastStatusCode, StringComparison.OrdinalIgnoreCase))
                {
                    return "PASS|DUPLICATE|NewStatus matches current status.";
                }

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
                    int maxMB = LogMaintenanceService.CurrentSettings?.MaxFileSizeMB ?? 20;
                    string filePath = LogMaintenanceService.GetLogFilePath(logDir, timestamp, maxMB);
                    string evtPart = string.IsNullOrEmpty(eventName) ? "" : string.Format(" [{0}]", eventName);
                    File.AppendAllText(filePath, string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}]{2} : {3}{4}", timestamp, level, evtPart, message, Environment.NewLine));
                }
            } catch { }
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public abstract void Start(CancellationToken externalCancellationToken, MqttBus bus = null, int heartbeatIntervalSeconds = 30);
        public abstract Task SendTimeCalibration();
        public virtual Task SendTerminalDisplay(string message) => Task.CompletedTask;
        public static string SanitizeTerminalMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;

            // 針對常見中文錯誤進行標準英文化轉譯
            string text = message;
            if (text.IndexOf("在MES找不到RECIPEID", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("找不到RECIPEID", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                text = "RECIPEID NOT FOUND IN MES";
            }

            // 僅保留標準 ASCII 可列印字元 (32-126) 及換行符號
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if ((c >= 32 && c <= 126) || c == '\r' || c == '\n')
                {
                    sb.Append(c);
                }
            }

            string result = sb.ToString().Trim();
            if (result.Length > 200)
            {
                result = result.Substring(0, 197) + "...";
            }

            return result;
        }
        public virtual void Stop() { }
        public static Color GetColorForState(EquipmentState state) { switch (state) { case EquipmentState.Running: return Color.LimeGreen; case EquipmentState.Alarm: case EquipmentState.Down: return Color.Crimson; case EquipmentState.Paused: case EquipmentState.Idle: return Color.Gold; default: return Color.Black; } }
    }
}
