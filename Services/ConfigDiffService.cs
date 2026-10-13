using System;
using System.Collections.Generic;
using System.Linq;
using EAPDashboard.Models;

namespace EAPDashboard.Services
{
    /// <summary>
    /// 設定檔差異比對服務
    /// </summary>
    public static class ConfigDiffService
    {
        /// <summary>
        /// 比較新舊 AppConfig 的差異
        /// </summary>
        /// <param name="oldConfig">目前的設定檔</param>
        /// <param name="newConfig">使用者修改後的新設定檔</param>
        /// <returns>差分比對結果</returns>
        public static ConfigDiffResult Compare(AppConfig oldConfig, AppConfig newConfig)
        {
            var result = new ConfigDiffResult();

            if (oldConfig == null && newConfig == null)
            {
                return result;
            }

            if (oldConfig == null)
            {
                result.GlobalChanges.Add(new GlobalChangeItem("全域設定", "初始化設定", "(無)", "新設定檔"));
                if (newConfig?.Equipments != null)
                {
                    foreach (var eq in newConfig.Equipments)
                    {
                        if (eq.IsActive)
                        {
                            result.EquipmentDiffs.Add(new EquipmentDiffItem
                            {
                                EquipmentId = eq.Id,
                                EquipmentName = eq.Name,
                                DiffType = EquipmentDiffType.Added,
                                NewConfig = eq,
                                ChangeDetails = new List<string> { "全新加入機台" }
                            });
                        }
                    }
                }
                return result;
            }

            if (newConfig == null)
            {
                result.GlobalChanges.Add(new GlobalChangeItem("全域設定", "清空設定", "現有設定檔", "(無)"));
                return result;
            }

            // 1. 全域設定比對
            CompareGlobalSettings(oldConfig, newConfig, result.GlobalChanges);

            // 2. 機台清單比對
            CompareEquipments(oldConfig.Equipments ?? new List<EquipmentConfig>(),
                              newConfig.Equipments ?? new List<EquipmentConfig>(),
                              result.EquipmentDiffs);

            return result;
        }

        private static void CompareGlobalSettings(AppConfig oldCfg, AppConfig newCfg, List<GlobalChangeItem> changes)
        {
            // 基礎系統設定
            CheckChange(changes, "系統設定", "關閉保護密碼", oldCfg.ExitPassword, newCfg.ExitPassword);
            CheckChange(changes, "系統設定", "EAP ID", oldCfg.EapId, newCfg.EapId);
            CheckChange(changes, "系統設定", "縮小至系統匣", oldCfg.MinimizeToTray.ToString(), newCfg.MinimizeToTray.ToString());
            CheckChange(changes, "系統設定", "字典檔目錄", oldCfg.DictionaryDirectory, newCfg.DictionaryDirectory);

            // 通訊設定
            CheckChange(changes, "MQTT 通訊", "Controller Topic", oldCfg.ControllerTopic, newCfg.ControllerTopic);
            CheckChange(changes, "MQTT 通訊", "Broker 位址", oldCfg.MqttBrokerHost, newCfg.MqttBrokerHost);
            CheckChange(changes, "MQTT 通訊", "Broker 埠號", oldCfg.MqttBrokerPort.ToString(), newCfg.MqttBrokerPort.ToString());
            CheckChange(changes, "MQTT 通訊", "機台主題前綴", oldCfg.MqttTopicPrefix, newCfg.MqttTopicPrefix);
            CheckChange(changes, "通訊參數", "心跳間隔 (秒)", oldCfg.HeartbeatIntervalSeconds.ToString(), newCfg.HeartbeatIntervalSeconds.ToString());
            CheckChange(changes, "通訊參數", "狀態上報間隔 (秒)", oldCfg.EqpStatusReportIntervalSeconds.ToString(), newCfg.EqpStatusReportIntervalSeconds.ToString());
            CheckChange(changes, "通訊參數", "全域卡匣綁定開關", oldCfg.EnableCassetteBinding.ToString(), newCfg.EnableCassetteBinding.ToString());

            // MES 設定
            var oldMes = oldCfg.MesSettings;
            var newMes = newCfg.MesSettings;
            if (oldMes == null && newMes != null)
            {
                changes.Add(new GlobalChangeItem("MES 設定", "MES 模組", "(未設定)", "已配置"));
            }
            else if (oldMes != null && newMes == null)
            {
                changes.Add(new GlobalChangeItem("MES 設定", "MES 模組", "已配置", "(未設定)"));
            }
            else if (oldMes != null && newMes != null)
            {
                CheckChange(changes, "MES 設定", "啟用環境", oldMes.ActiveEnvironment, newMes.ActiveEnvironment);
                CheckChange(changes, "MES 設定", "正式環境網址", oldMes.ProductionUrl, newMes.ProductionUrl);
                CheckChange(changes, "MES 設定", "測試環境網址", oldMes.TestUrl, newMes.TestUrl);

                string oldFilter = string.Join(",", (oldMes.FilterNullApis ?? new List<string>()).OrderBy(x => x));
                string newFilter = string.Join(",", (newMes.FilterNullApis ?? new List<string>()).OrderBy(x => x));
                CheckChange(changes, "MES 設定", "過濾 Null APIs", oldFilter, newFilter);
            }

            // 料捲防混料設定 (ReelCheckSettings)
            var oldReel = oldCfg.ReelCheckSettings;
            var newReel = newCfg.ReelCheckSettings;
            if (oldReel == null && newReel != null)
            {
                changes.Add(new GlobalChangeItem("料捲設定", "ReelCheck 模組", "(未設定)", "已配置"));
            }
            else if (oldReel != null && newReel == null)
            {
                changes.Add(new GlobalChangeItem("料捲設定", "ReelCheck 模組", "已配置", "(未設定)"));
            }
            else if (oldReel != null && newReel != null)
            {
                CheckChange(changes, "料捲設定", "啟用環境", oldReel.ActiveEnvironment, newReel.ActiveEnvironment);
                CheckChange(changes, "料捲設定", "正式環境網址", oldReel.ProductionUrl, newReel.ProductionUrl);
                CheckChange(changes, "料捲設定", "測試環境網址", oldReel.TestUrl, newReel.TestUrl);
                CheckChange(changes, "料捲設定", "本機服務網址", oldReel.ClientUrl, newReel.ClientUrl);
                CheckChange(changes, "料捲設定", "逾時時間 (秒)", oldReel.TimeoutSeconds.ToString(), newReel.TimeoutSeconds.ToString());
            }

            // 良率設定
            var oldYield = oldCfg.YieldSettings;
            var newYield = newCfg.YieldSettings;
            if (oldYield != null && newYield != null)
            {
                CheckChange(changes, "良率設定", "最低良率", oldYield.MinYieldRate.ToString(), newYield.MinYieldRate.ToString());
                CheckChange(changes, "良率設定", "最高良率", oldYield.MaxYieldRate.ToString(), newYield.MaxYieldRate.ToString());
            }

            // PostgreSQL 本地資料庫設定
            var oldPg = oldCfg.PostgresDbSettings;
            var newPg = newCfg.PostgresDbSettings;
            if (oldPg != null && newPg != null)
            {
                CheckChange(changes, "PostgreSQL", "啟用狀態", oldPg.Enabled.ToString(), newPg.Enabled.ToString());
                CheckChange(changes, "PostgreSQL", "連線字串", oldPg.ConnectionString, newPg.ConnectionString);
                CheckChange(changes, "PostgreSQL", "保留天數", oldPg.RetentionDays.ToString(), newPg.RetentionDays.ToString());
            }

            // SQL Server 卡匣資料庫設定
            var oldDb = oldCfg.DbSettings;
            var newDb = newCfg.DbSettings;
            if (oldDb != null && newDb != null)
            {
                CheckChange(changes, "卡匣資料庫", "連線字串", oldDb.ConnectionString, newDb.ConnectionString);
                CheckChange(changes, "卡匣資料庫", "資料表名稱", oldDb.BindingTable, newDb.BindingTable);
                CheckChange(changes, "卡匣資料庫", "卡匣欄位", oldDb.CassetteColumn, newDb.CassetteColumn);
                CheckChange(changes, "卡匣資料庫", "工單欄位", oldDb.WoColumn, newDb.WoColumn);
            }

            // 日誌設定
            var oldLog = oldCfg.LogSettings;
            var newLog = newCfg.LogSettings;
            if (oldLog != null && newLog != null)
            {
                CheckChange(changes, "日誌設定", "啟用狀態", oldLog.Enabled.ToString(), newLog.Enabled.ToString());
                CheckChange(changes, "日誌設定", "壓縮天數", oldLog.CompressDays.ToString(), newLog.CompressDays.ToString());
                CheckChange(changes, "日誌設定", "保留天數", oldLog.RetentionDays.ToString(), newLog.RetentionDays.ToString());
                CheckChange(changes, "日誌設定", "檔案大小上限(MB)", oldLog.MaxFileSizeMB.ToString(), newLog.MaxFileSizeMB.ToString());
                CheckChange(changes, "日誌設定", "記錄雙向存活交握日誌 (AreYouThere)", oldLog.LogAreYouThere.ToString(), newLog.LogAreYouThere.ToString());
            }
            if (oldCfg.LogAreYouThere.HasValue != newCfg.LogAreYouThere.HasValue ||
                (oldCfg.LogAreYouThere.HasValue && newCfg.LogAreYouThere.HasValue && oldCfg.LogAreYouThere.Value != newCfg.LogAreYouThere.Value))
            {
                CheckChange(changes, "日誌設定", "全域存活交握旗標 (相容屬性)", oldCfg.LogAreYouThere?.ToString(), newCfg.LogAreYouThere?.ToString());
            }
        }

        private static void CompareEquipments(List<EquipmentConfig> oldList, List<EquipmentConfig> newList, List<EquipmentDiffItem> diffs)
        {
            var oldDict = oldList.Where(e => !string.IsNullOrWhiteSpace(e.Id))
                                 .ToDictionary(e => e.Id.Trim(), e => e, StringComparer.OrdinalIgnoreCase);
            var newDict = newList.Where(e => !string.IsNullOrWhiteSpace(e.Id))
                                 .ToDictionary(e => e.Id.Trim(), e => e, StringComparer.OrdinalIgnoreCase);

            var allKeys = new HashSet<string>(oldDict.Keys, StringComparer.OrdinalIgnoreCase);
            allKeys.UnionWith(newDict.Keys);

            foreach (var key in allKeys)
            {
                bool hasOld = oldDict.TryGetValue(key, out var oldEq);
                bool hasNew = newDict.TryGetValue(key, out var newEq);

                if (!hasOld && hasNew)
                {
                    // 全新加入
                    if (newEq.IsActive)
                    {
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = newEq.Id,
                            EquipmentName = newEq.Name,
                            DiffType = EquipmentDiffType.Added,
                            NewConfig = newEq,
                            ChangeDetails = new List<string> { $"新增並啟用機台 ({newEq.Protocol})" }
                        });
                    }
                    else
                    {
                        // 新增但未啟用
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = newEq.Id,
                            EquipmentName = newEq.Name,
                            DiffType = EquipmentDiffType.Unchanged,
                            NewConfig = newEq,
                            ChangeDetails = new List<string> { "新增但處於停用狀態 (無連線作業)" }
                        });
                    }
                }
                else if (hasOld && !hasNew)
                {
                    // 被刪除
                    if (oldEq.IsActive)
                    {
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = oldEq.Id,
                            EquipmentName = oldEq.Name,
                            DiffType = EquipmentDiffType.Removed,
                            OldConfig = oldEq,
                            ChangeDetails = new List<string> { "自設定檔移除 (即將停止通訊並卸載)" }
                        });
                    }
                    else
                    {
                        // 原本就沒啟用且被刪除，不需中斷通訊
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = oldEq.Id,
                            EquipmentName = oldEq.Name,
                            DiffType = EquipmentDiffType.Unchanged,
                            OldConfig = oldEq,
                            ChangeDetails = new List<string> { "已停用機台自設定檔移除" }
                        });
                    }
                }
                else if (hasOld && hasNew)
                {
                    // 兩者皆存在
                    var details = new List<string>();

                    // 1. 啟用狀態改變
                    if (oldEq.IsActive != newEq.IsActive)
                    {
                        if (newEq.IsActive)
                        {
                            diffs.Add(new EquipmentDiffItem
                            {
                                EquipmentId = newEq.Id,
                                EquipmentName = newEq.Name,
                                DiffType = EquipmentDiffType.Added,
                                OldConfig = oldEq,
                                NewConfig = newEq,
                                ChangeDetails = new List<string> { "機台由停用轉為啟用 (即將建立連線)" }
                            });
                        }
                        else
                        {
                            diffs.Add(new EquipmentDiffItem
                            {
                                EquipmentId = newEq.Id,
                                EquipmentName = newEq.Name,
                                DiffType = EquipmentDiffType.Removed,
                                OldConfig = oldEq,
                                NewConfig = newEq,
                                ChangeDetails = new List<string> { "機台由啟用轉為停用 (即將中斷通訊並卸載)" }
                            });
                        }
                        continue;
                    }

                    // 如果原本與新版皆未啟用，視為無變更
                    if (!oldEq.IsActive && !newEq.IsActive)
                    {
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = newEq.Id,
                            EquipmentName = newEq.Name,
                            DiffType = EquipmentDiffType.Unchanged,
                            OldConfig = oldEq,
                            NewConfig = newEq,
                            ChangeDetails = new List<string> { "維持停用狀態" }
                        });
                        continue;
                    }

                    // 原本與新版皆啟用：檢查核心通訊指紋
                    bool coreChanged = false;

                    if (!StringEqual(oldEq.Protocol, newEq.Protocol))
                    {
                        coreChanged = true;
                        details.Add($"通訊協定: {oldEq.Protocol} ➔ {newEq.Protocol}");
                    }


                    if (string.Equals(newEq.Protocol, "SECS", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!StringEqual(oldEq.Ip, newEq.Ip))
                        {
                            coreChanged = true;
                            details.Add($"IP 位址: {oldEq.Ip} ➔ {newEq.Ip}");
                        }
                        if (oldEq.Port != newEq.Port)
                        {
                            coreChanged = true;
                            details.Add($"Port 埠號: {oldEq.Port} ➔ {newEq.Port}");
                        }
                        if (oldEq.DeviceId != newEq.DeviceId)
                        {
                            coreChanged = true;
                            details.Add($"Device ID: {oldEq.DeviceId} ➔ {newEq.DeviceId}");
                        }
                        if (!StringEqual(oldEq.TimeFormat, newEq.TimeFormat))
                        {
                            coreChanged = true;
                            details.Add($"時間格式: {oldEq.TimeFormat} ➔ {newEq.TimeFormat}");
                        }
                    }
                    else // MQTT
                    {
                        if (!StringEqual(oldEq.StatusType, newEq.StatusType))
                        {
                            coreChanged = true;
                            details.Add($"狀態類型: {oldEq.StatusType} ➔ {newEq.StatusType}");
                        }
                    }

                    // 非核心屬性比對（僅記錄資訊，若無核心變更則判定為 Unchanged，不中斷通訊）
                    if (!StringEqual(oldEq.Name, newEq.Name))
                    {
                        details.Add($"名稱更新: {oldEq.Name} ➔ {newEq.Name}");
                    }
                    if (oldEq.AutoReportStatus != newEq.AutoReportStatus)
                    {
                        details.Add($"自動上報: {oldEq.AutoReportStatus} ➔ {newEq.AutoReportStatus}");
                    }
                    if (oldEq.EqpStatusReportIntervalSeconds != newEq.EqpStatusReportIntervalSeconds)
                    {
                        details.Add($"狀態上報間隔: {oldEq.EqpStatusReportIntervalSeconds} ➔ {newEq.EqpStatusReportIntervalSeconds}");
                    }
                    if (oldEq.EnableCassetteBinding != newEq.EnableCassetteBinding)
                    {
                        details.Add($"卡匣綁定設定: {oldEq.EnableCassetteBinding} ➔ {newEq.EnableCassetteBinding}");
                    }
                    if (oldEq.EnableReelCheck != newEq.EnableReelCheck)
                    {
                        details.Add($"料捲防混料: {oldEq.EnableReelCheck} ➔ {newEq.EnableReelCheck}");
                    }
                    if (oldEq.EnableTrackOutValidation != newEq.EnableTrackOutValidation)
                    {
                        details.Add($"出站結算與NG/良率檢核: {oldEq.EnableTrackOutValidation} ➔ {newEq.EnableTrackOutValidation}");
                    }

                    if (coreChanged)
                    {
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = newEq.Id,
                            EquipmentName = newEq.Name,
                            DiffType = EquipmentDiffType.Modified,
                            OldConfig = oldEq,
                            NewConfig = newEq,
                            ChangeDetails = details
                        });
                    }
                    else
                    {
                        // 核心連線未變動！維持現狀不中斷
                        diffs.Add(new EquipmentDiffItem
                        {
                            EquipmentId = newEq.Id,
                            EquipmentName = newEq.Name,
                            DiffType = EquipmentDiffType.Unchanged,
                            OldConfig = oldEq,
                            NewConfig = newEq,
                            ChangeDetails = details.Count > 0 ? details : new List<string> { "連線設定完全一致，通訊持續運行不中斷" }
                        });
                    }
                }
            }
        }

        private static void CheckChange(List<GlobalChangeItem> changes, string category, string name, string oldVal, string newVal)
        {
            if (!StringEqual(oldVal, newVal))
            {
                changes.Add(new GlobalChangeItem(category, name, oldVal, newVal));
            }
        }

        private static bool StringEqual(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b)) return true;
            return string.Equals(a, b, StringComparison.Ordinal);
        }
    }
}
