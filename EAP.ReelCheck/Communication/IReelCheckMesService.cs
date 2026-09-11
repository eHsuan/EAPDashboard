using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EAP.ReelCheck.Models;

namespace EAP.ReelCheck.Communication;

/// <summary>
/// 防混檢驗所需之 MES 通訊服務介面契約
/// 可對接 Cyntec WCF 服務，亦可對接 EAP 既有之 Protocol.MES 或 Mock 測試
/// </summary>
public interface IReelCheckMesService
{
    /// <summary>
    /// 檢驗條碼是否歸屬於其他工單 (反查全廠防混)
    /// </summary>
    Task<string?> QueryOtherWorkOrderByBarcodeAsync(string barcode, CancellationToken ct = default);

    /// <summary>
    /// 查詢工單綁定之 Reel 清單
    /// </summary>
    Task<List<ReelItem>> QueryReelListAsync(string woNo, CancellationToken ct = default);

    /// <summary>
    /// 重大混料異常時，於 MES 端強制鎖定工單
    /// </summary>
    Task<bool> BlockWorkOrderAsync(string woNo, string reasonCode, string remark, CancellationToken ct = default);

    /// <summary>
    /// 更新 Reel 在 MES 的上機狀態 (例如 11:作業中)
    /// </summary>
    Task<bool> UpdateReelStatusAsync(string reelId, string statusCode, string machNo, string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default);

    /// <summary>
    /// 寫入 Reel 上料歷程日誌 (Action 00005)
    /// </summary>
    Task<bool> AddReelLogAsync(string woNo, string reelId, string matNo, string batchNo, string userId, string action = "00005", CancellationToken ct = default);

    /// <summary>
    /// 儲存防呆檢驗統計記錄
    /// </summary>
    Task<bool> SaveReelCheckRecordAsync(string woNo, string machNo, string reelId, string matNo, string batchNo, int reelCount, int matCount, int tpCount, CancellationToken ct = default);

    /// <summary>
    /// 完工包裝過帳
    /// </summary>
    Task<bool> SavePackingAsync(string woNo, string batchNo, string matNo, string reelId, string userId, string workCenterNo, CancellationToken ct = default);

    /// <summary>
    /// 向 MES 查詢工單詳細資訊與機台防呆檢驗門檻
    /// </summary>
    Task<WorkOrderContext> QueryWorkOrderInfoAsync(string woNo, string machNo, string userId, CancellationToken ct = default);

    /// <summary>
    /// 測試與 MES WCF 服務之連線健康狀態
    /// </summary>
    Task<bool> PingAsync(CancellationToken ct = default);
}
