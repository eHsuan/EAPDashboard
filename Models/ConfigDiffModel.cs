using System.Collections.Generic;

namespace EAPDashboard.Models
{
    /// <summary>
    /// 機台設定之差分狀態類型
    /// </summary>
    public enum EquipmentDiffType
    {
        /// <summary>
        /// 未變更：核心連線參數完全一致，保持原連線不中斷
        /// </summary>
        Unchanged,

        /// <summary>
        /// 新增啟用：新設定中新增之機台
        /// </summary>
        Added,

        /// <summary>
        /// 停用或刪除：原本運作中，在新設定中被移除或改為停用
        /// </summary>
        Removed,

        /// <summary>
        /// 核心修改：通訊協定、IP、Port、Topic、字典檔等核心連線參數有變更，需重啟連線
        /// </summary>
        Modified
    }

    /// <summary>
    /// 全域設定變更項目細節
    /// </summary>
    public class GlobalChangeItem
    {
        public string PropertyCategory { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public GlobalChangeItem(string category, string name, string oldValue, string newValue)
        {
            PropertyCategory = category;
            PropertyName = name;
            OldValue = oldValue ?? "(無)";
            NewValue = newValue ?? "(無)";
        }
    }

    /// <summary>
    /// 單一機台之差分比對結果
    /// </summary>
    public class EquipmentDiffItem
    {
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public EquipmentDiffType DiffType { get; set; }
        public List<string> ChangeDetails { get; set; } = new List<string>();
        public EquipmentConfig OldConfig { get; set; }
        public EquipmentConfig NewConfig { get; set; }

        public string DiffTypeDisplay
        {
            get
            {
                switch (DiffType)
                {
                    case EquipmentDiffType.Added:
                        return "🟢 新增";
                    case EquipmentDiffType.Removed:
                        return "🔴 停用/刪除";
                    case EquipmentDiffType.Modified:
                        return "🟡 核心變更";
                    case EquipmentDiffType.Unchanged:
                    default:
                        return "⚪ 維持不變";
                }
            }
        }
    }

    /// <summary>
    /// 設定檔總體差分比對結果
    /// </summary>
    public class ConfigDiffResult
    {
        /// <summary>
        /// 是否包含任何全域參數變更（若為 true 需提醒重啟程式）
        /// </summary>
        public bool HasGlobalChanges => GlobalChanges != null && GlobalChanges.Count > 0;

        /// <summary>
        /// 是否包含任何機台變更（新增、停用、修改）
        /// </summary>
        public bool HasEquipmentChanges
        {
            get
            {
                if (EquipmentDiffs == null) return false;
                foreach (var eq in EquipmentDiffs)
                {
                    if (eq.DiffType != EquipmentDiffType.Unchanged)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 是否有任何變更（全域或機台）
        /// </summary>
        public bool HasAnyChange => HasGlobalChanges || HasEquipmentChanges;

        /// <summary>
        /// 全域設定變更清單
        /// </summary>
        public List<GlobalChangeItem> GlobalChanges { get; set; } = new List<GlobalChangeItem>();

        /// <summary>
        /// 機台清單差分比對結果
        /// </summary>
        public List<EquipmentDiffItem> EquipmentDiffs { get; set; } = new List<EquipmentDiffItem>();
    }
}
