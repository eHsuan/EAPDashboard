using System;
using System.Collections.Generic;

namespace EAPDashboard.Models.Smt
{
    /// <summary>
    /// 線別類型
    /// </summary>
    public static class LineTypes
    {
        public const string SMT = "SMT";
        public const string Normal = "Normal";
    }

    /// <summary>
    /// 線別設定模型
    /// </summary>
    public class LineConfig
    {
        /// <summary>
        /// 線別代號 (例如: SMT1, SMT2)
        /// </summary>
        public string LineId { get; set; } = string.Empty;

        /// <summary>
        /// 線別名稱 (例如: SMT 第一生產線)
        /// </summary>
        public string LineName { get; set; } = string.Empty;

        /// <summary>
        /// 線別類型 (預設 SMT)
        /// </summary>
        public string LineType { get; set; } = LineTypes.SMT;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 滿批次物理上限數量 (預設 10 片)
        /// </summary>
        public int BatchCapacity { get; set; } = 10;

        /// <summary>
        /// 第一站 (Station 1) 設備與主題設定
        /// </summary>
        public SmtStationConfig Station1 { get; set; } = new SmtStationConfig { StationCode = "Station1", StationName = "SMT 進站/印刷" };

        /// <summary>
        /// 第二站 (Station 2 / Post AOI) 設備與主題設定
        /// </summary>
        public SmtStationConfig Station2 { get; set; } = new SmtStationConfig { StationCode = "Post_AOI", StationName = "Post AOI 檢測" };
    }

    /// <summary>
    /// 站別設定模型
    /// </summary>
    public class SmtStationConfig
    {
        /// <summary>
        /// 站別代碼 (例如: Station1, Post_AOI)
        /// </summary>
        public string StationCode { get; set; } = string.Empty;

        /// <summary>
        /// 站別名稱
        /// </summary>
        public string StationName { get; set; } = string.Empty;

        /// <summary>
        /// 所屬設備代號清單
        /// </summary>
        public List<string> EquipmentIds { get; set; } = new List<string>();

        /// <summary>
        /// Panel In 回報主題 (選填，若未指定則由設備主題推導)
        /// </summary>
        public string PanelInTopic { get; set; }

        /// <summary>
        /// Panel Out 回報主題 (選填，若未指定則由設備主題推導)
        /// </summary>
        public string PanelOutTopic { get; set; }
    }
}
