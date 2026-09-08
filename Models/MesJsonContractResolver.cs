using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Protocol.MES.Base;
using Protocol.MES.Enums;

namespace EAPDashboard.Models
{
    /// <summary>
    /// MES 專屬 JSON 序列化解析器：
    /// 依據白名單設定（預設為 WOCHECKIN_MULTI, WOCHECKOUT_MULTI，亦可由 config.json 動態指定），
    /// 當發送特定的 MES API 請求時，自動將值為 NULL、空字串、空白字元、空集合或空 JSON 陣列/物件的欄位自酬載中剔除。
    /// 未列入白名單的 API 則 100% 保持原始序列化行為。
    /// </summary>
    public class MesJsonContractResolver : DefaultContractResolver
    {
        public static readonly MesJsonContractResolver Instance = new MesJsonContractResolver();

        private static bool _isConfigured = false;
        private static readonly object _configLock = new object();

        private static readonly HashSet<string> _whitelist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "WOCHECKIN_MULTI",
            "WOCHECKOUT_MULTI"
        };
        private static readonly object _whitelistLock = new object();

        /// <summary>
        /// 設定白名單 API 清單 (例如來自 config.json 的 FilterNullApis)
        /// </summary>
        public static void SetWhitelist(IEnumerable<string> apis)
        {
            if (apis == null) return;
            lock (_whitelistLock)
            {
                _whitelist.Clear();
                foreach (var api in apis)
                {
                    if (!string.IsNullOrWhiteSpace(api))
                    {
                        _whitelist.Add(api.Trim());
                    }
                }
            }
        }

        /// <summary>
        /// 取得目前生效的白名單清單副本
        /// </summary>
        public static IReadOnlyCollection<string> GetWhitelist()
        {
            lock (_whitelistLock)
            {
                return _whitelist.ToList();
            }
        }

        /// <summary>
        /// 判斷指定之 MES 交易是否啟用了 NULL/空集合過濾
        /// </summary>
        public static bool IsFilterEnabled(MESTransactionName transactionName)
        {
            return IsFilterEnabled(transactionName.ToString());
        }

        /// <summary>
        /// 判斷指定之交易名稱是否啟用了 NULL/空集合過濾 (支援大小寫與底線正規化比對)
        /// </summary>
        public static bool IsFilterEnabled(string transactionName)
        {
            if (string.IsNullOrWhiteSpace(transactionName)) return false;

            lock (_whitelistLock)
            {
                if (_whitelist.Count == 0) return false;

                // 1. 直接比對 (忽略大小寫)
                if (_whitelist.Contains(transactionName)) return true;

                // 2. 正規化比對 (去除底線與連字號)
                string normalizedTxn = NormalizeName(transactionName);
                return _whitelist.Any(w => NormalizeName(w).Equals(normalizedTxn, StringComparison.OrdinalIgnoreCase));
            }
        }

        private static string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            return name.Replace("_", "").Replace("-", "").Trim();
        }

        /// <summary>
        /// 設定 Newtonsoft.Json 全域預設序列化設定，使 Protocol.MES 內部與 EAP 均套用此解析器
        /// </summary>
        public static void ConfigureDefaultSettings()
        {
            if (_isConfigured) return;
            lock (_configLock)
            {
                if (_isConfigured) return;

                var existingFactory = JsonConvert.DefaultSettings;
                JsonConvert.DefaultSettings = () =>
                {
                    var settings = existingFactory != null ? existingFactory() : new JsonSerializerSettings();
                    settings.ContractResolver = Instance;
                    return settings;
                };

                _isConfigured = true;
            }
        }

        /// <summary>
        /// 序列化 MES 物件為 JSON 字串
        /// </summary>
        public static string Serialize(object obj, Formatting formatting = Formatting.Indented)
        {
            if (obj == null) return string.Empty;

            var settings = new JsonSerializerSettings
            {
                Formatting = formatting,
                ContractResolver = Instance
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // 檢查宣告型別是否為 MES 相關型別 (繼承自 MESBaseMessage 或位於 Protocol.MES 命名空間)
            bool isMesType = typeof(MESBaseMessage).IsAssignableFrom(member.DeclaringType)
                             || (member.DeclaringType?.Namespace != null && member.DeclaringType.Namespace.StartsWith("Protocol.MES"));

            if (isMesType)
            {
                bool isOutPutProp = string.Equals(property.PropertyName, "OutPut", StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(member.Name, "OutPut", StringComparison.OrdinalIgnoreCase);
                if (isOutPutProp && property.ValueProvider != null)
                {
                    property.ValueProvider = new OutPutFallbackValueProvider(property.ValueProvider);
                }

                var prevShouldSerialize = property.ShouldSerialize;
                property.ShouldSerialize = instance =>
                {
                    if (prevShouldSerialize != null && !prevShouldSerialize(instance))
                        return false;

                    // 針對工單出站 (含 CHECKOUT) 之 OutPut 欄位特別防護：
                    // 若為空字串則填 0，因此必須序列化，不可被空值白名單過濾掉
                    if (instance is MESBaseMessage checkMesMsg && checkMesMsg.TransactionName != null)
                    {
                        string txn = checkMesMsg.TransactionName.ToString();
                        if (txn.IndexOf("CHECKOUT", StringComparison.OrdinalIgnoreCase) >= 0 && isOutPutProp)
                        {
                            return true;
                        }
                    }

                    // 針對工單進站 (WOCHECKIN 與 WOCHECKIN_MULTI) 之 WOCarrierDt 欄位特別防護：
                    // 若為空值、"[]"、"{}" 或 "N/A" / "NA"，一律刪除該欄位 (不予序列化)
                    if (instance is MESBaseMessage mesMsg)
                    {
                        bool isCheckIn = mesMsg.TransactionName == MESTransactionName.WOCHECKIN
                                         || mesMsg.TransactionName == MESTransactionName.WOCHECKIN_MULTI
                                         || string.Equals(mesMsg.TransactionName.ToString(), "WOCHECKIN", StringComparison.OrdinalIgnoreCase)
                                         || string.Equals(mesMsg.TransactionName.ToString(), "WOCHECKIN_MULTI", StringComparison.OrdinalIgnoreCase)
                                         || string.Equals(mesMsg.TransactionName.ToString(), "WOCHECKINMULTI", StringComparison.OrdinalIgnoreCase);

                        bool isWoCarrierDt = string.Equals(property.PropertyName, "WOCarrierDt", StringComparison.OrdinalIgnoreCase)
                                             || string.Equals(member.Name, "WOCarrierDt", StringComparison.OrdinalIgnoreCase);

                        if (isCheckIn && isWoCarrierDt)
                        {
                            object carrierVal = null;
                            try { carrierVal = property.ValueProvider.GetValue(instance); } catch { }
                            if (IsWoCarrierDtEmptyOrNa(carrierVal))
                                return false;
                        }
                    }

                    // 檢查當前實例是否為 MES 訊息，且其交易名稱是否在過濾白名單內
                    bool filterActive = false;
                    if (instance is MESBaseMessage mMsg)
                    {
                        filterActive = IsFilterEnabled(mMsg.TransactionName);
                    }

                    // 若未啟用過濾 (不在白名單內)，100% 保持原始行為
                    if (!filterActive)
                        return true;

                    object val = null;
                    try
                    {
                        val = property.ValueProvider.GetValue(instance);
                    }
                    catch
                    {
                        return true;
                    }

                    // 1. NULL 值剔除
                    if (val == null)
                        return false;

                    // 2. 字串判斷：若為空字串、純空白、代表空陣列/空物件的 JSON 字串 ("[]", "{}") 或 "N/A" 則不序列化 (刪除該欄位)
                    if (val is string str)
                    {
                        string trimmed = str.Trim();
                        if (string.IsNullOrEmpty(trimmed) || trimmed == "[]" || trimmed == "{}"
                            || trimmed.Equals("N/A", StringComparison.OrdinalIgnoreCase)
                            || trimmed.Equals("NA", StringComparison.OrdinalIgnoreCase))
                            return false;
                    }

                    // 3. 實體集合判斷 (Array, List, ICollection, IEnumerable 但排除 string)
                    if (val is IEnumerable enumerable && !(val is string))
                    {
                        var enumerator = enumerable.GetEnumerator();
                        if (!enumerator.MoveNext())
                            return false;
                    }

                    // 4. JToken 集合判斷
                    if (val is JArray jarr && jarr.Count == 0)
                        return false;
                    if (val is JObject jobj && jobj.Count == 0)
                        return false;
                    if (val is JValue jval)
                    {
                        if (jval.Value == null) return false;
                        if (jval.Type == JTokenType.String)
                        {
                            string s = jval.Value.ToString().Trim();
                            if (string.IsNullOrEmpty(s) || s == "[]" || s == "{}"
                                || s.Equals("N/A", StringComparison.OrdinalIgnoreCase)
                                || s.Equals("NA", StringComparison.OrdinalIgnoreCase))
                                return false;
                        }
                    }

                    return true;
                };
            }

            return property;
        }

        /// <summary>
        /// 判斷 WOCarrierDt 之值是否為空、空集合或 N/A
        /// </summary>
        public static bool IsWoCarrierDtEmptyOrNa(object val)
        {
            if (val == null) return true;

            if (val is string str)
            {
                string trimmed = str.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed == "[]" || trimmed == "{}"
                    || trimmed.Equals("N/A", StringComparison.OrdinalIgnoreCase)
                    || trimmed.Equals("NA", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    try
                    {
                        var arr = JArray.Parse(trimmed);
                        if (arr.Count == 0) return true;
                        bool hasValidCarrier = false;
                        foreach (var item in arr)
                        {
                            string cno = item["CarrierNo"]?.ToString()?.Trim();
                            if (!string.IsNullOrEmpty(cno)
                                && !cno.Equals("N/A", StringComparison.OrdinalIgnoreCase)
                                && !cno.Equals("NA", StringComparison.OrdinalIgnoreCase))
                            {
                                hasValidCarrier = true;
                                break;
                            }
                        }
                        if (!hasValidCarrier) return true;
                    }
                    catch { }
                }
                return false;
            }

            if (val is IEnumerable enumerable && !(val is string))
            {
                var enumerator = enumerable.GetEnumerator();
                if (!enumerator.MoveNext()) return true;
            }

            if (val is JArray jarr && jarr.Count == 0) return true;
            if (val is JObject jobj && jobj.Count == 0) return true;

            return false;
        }
    }

    /// <summary>
    /// 工單出站 (CHECKOUT) 專用之 OutPut 欄位防護：
    /// 若 OutPut 欄位為空值、空字串或純空白，自動回傳 "0"，防止 MES 拒絕或發生空值錯誤。
    /// </summary>
    internal class OutPutFallbackValueProvider : IValueProvider
    {
        private readonly IValueProvider _inner;

        public OutPutFallbackValueProvider(IValueProvider inner)
        {
            _inner = inner;
        }

        public object GetValue(object target)
        {
            object val = _inner.GetValue(target);

            if (target is MESBaseMessage mesMsg && mesMsg.TransactionName != null)
            {
                string txn = mesMsg.TransactionName.ToString();
                if (txn.IndexOf("CHECKOUT", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (val == null || (val is string s && string.IsNullOrWhiteSpace(s)))
                    {
                        return "0";
                    }
                }
            }

            return val;
        }

        public void SetValue(object target, object value)
        {
            _inner.SetValue(target, value);
        }
    }
}
