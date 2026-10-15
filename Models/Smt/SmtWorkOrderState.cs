using System;

namespace EAPDashboard.Models.Smt
{
    /// <summary>
    /// SMT 工單跨站狀態模型 (執行緒安全與持久化用)
    /// </summary>
    public class SmtWorkOrderState
    {
        public string LineId { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;

        // --- 第一站 (Station 1) ---
        public int Station1_In_Count { get; set; }
        public int Station1_Out_Count { get; set; }
        /// <summary>
        /// 第一站結單時記錄之最終真實數量 (例如抽片後出站之數量)
        /// </summary>
        public int? Station1_Actual_QTY { get; set; }
        public bool Station1_Finished { get; set; }
        public DateTime? Station1_FinishedTime { get; set; }

        // --- 第二站 (Station 2 / Post AOI) ---
        public int Station2_In_Count { get; set; }
        public int Station2_Out_Count { get; set; }
        public int? Station2_Actual_QTY { get; set; }
        public bool Station2_Finished { get; set; }
        public DateTime? Station2_FinishedTime { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 線別當前在製狀態摘要 (供 WebUI 查詢)
    /// </summary>
    public class SmtLineStatusSummary
    {
        public string LineId { get; set; }
        public string LineName { get; set; }
        public bool IsActive { get; set; }
        public int BatchCapacity { get; set; }
        public string Station1_CurrentWO { get; set; }
        public string Station2_CurrentWO { get; set; }
        public System.Collections.Generic.List<SmtWorkOrderState> ActiveWorkOrders { get; set; } = new System.Collections.Generic.List<SmtWorkOrderState>();
    }
}
