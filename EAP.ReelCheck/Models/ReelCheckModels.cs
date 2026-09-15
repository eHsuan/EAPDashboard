using System;

namespace EAP.ReelCheck.Models;

/// <summary>
/// 條碼辨識類型
/// </summary>
public enum BarcodeKind
{
    Unknown = 0,
    MatNo = 1,       // 1D 大標籤料號
    TapingNo = 2,    // 1D 大標籤批號 (TP No)
    Reel2D = 3       // 2D Reel 標籤條碼
}

/// <summary>
/// 防呆執行處置
/// </summary>
public enum CheckDisposition
{
    Pass = 0,               // 檢驗通過
    Ignore = 1,             // 雜訊忽略
    Warning = 2,            // 警告提示
    BlockWorkOrder = 3,     // 觸發 MES 鎖單 (重大混料)
    LockMachine = 4         // 觸發機台鎖機
}

/// <summary>
/// 工單防呆作業上下文環境
/// </summary>
public class WorkOrderContext
{
    public string WONo { get; set; } = string.Empty;
    public string MatNo { get; set; } = string.Empty;
    public string BatchNo { get; set; } = string.Empty; // TapingNo
    public string MachineNo { get; set; } = string.Empty;
    public string OperatorId { get; set; } = string.Empty;
    public string WorkCenterNo { get; set; } = string.Empty;

    public bool IsMainMaterial { get; set; } = true;
    public string SubMatNo { get; set; } = string.Empty;
    public string SubBatchNo { get; set; } = string.Empty; // 副料批號 (用於 NG-0103/NG-0104)
    public string OrderType { get; set; } = string.Empty; // 如 ZE01
    public bool IsCheckEnabled { get; set; } = true;     // Property20 != "N"

    // 防呆參數門檻
    public int MinMatNoScanCount { get; set; } = 1;
    public int MinTPNoScanCount { get; set; } = 1;
    public decimal MinReelDurationMinutes { get; set; } = 0;
    public double ReelUpdateDelayMinutes { get; set; } = 1.0;
}

/// <summary>
/// 料捲 (Reel) 狀態項目
/// </summary>
public class ReelItem
{
    public string ReelId { get; set; } = string.Empty;
    public string Status { get; set; } = "00";      // 00=待上機, 11=作業中
    public string TPStatus { get; set; } = "00";
    public string TPMachNo { get; set; } = string.Empty;
    public string MatNo { get; set; } = string.Empty;
    public string BatchNo { get; set; } = string.Empty;
    public string WONo { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.MinValue;
    public DateTime EndTime { get; set; } = DateTime.MinValue;
    public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

    public int ReelScanCount { get; set; }
    public int MatNoScanCount { get; set; }
    public int TPNoScanCount { get; set; }

    public bool IsCurrentActive { get; set; }
    public bool IsCompleted { get; set; }
}

/// <summary>
/// 防呆檢驗綜合結果
/// </summary>
public record ReelCheckResult
{
    public bool IsSuccess { get; init; }
    public CheckDisposition Disposition { get; init; } = CheckDisposition.Pass;
    public string ErrorCode { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    public string RawBarcode { get; init; } = string.Empty;
    public string DecodedReelId { get; init; } = string.Empty;
    public BarcodeKind BarcodeType { get; init; } = BarcodeKind.Unknown;
    public string ProblemWoNo { get; init; } = string.Empty; // 跨工單混料時原屬之問題工單號碼 (用於 NG-0401)

    public bool ShouldBlockWorkOrder => Disposition == CheckDisposition.BlockWorkOrder;
    public bool ShouldLockMachine => Disposition == CheckDisposition.LockMachine || ShouldBlockWorkOrder;

    public ReelItem? ActiveReel { get; init; }

    public static ReelCheckResult Ok(string rawBarcode, BarcodeKind kind, string message, ReelItem? reel = null) =>
        new()
        {
            IsSuccess = true,
            Disposition = CheckDisposition.Pass,
            RawBarcode = rawBarcode,
            BarcodeType = kind,
            Message = message,
            ActiveReel = reel
        };

    public static ReelCheckResult Ignored(string rawBarcode, string reason) =>
        new()
        {
            IsSuccess = true,
            Disposition = CheckDisposition.Ignore,
            RawBarcode = rawBarcode,
            BarcodeType = BarcodeKind.Unknown,
            Message = reason
        };

    public static ReelCheckResult Ng(string rawBarcode, string errorCode, string message, CheckDisposition disposition = CheckDisposition.Warning, BarcodeKind kind = BarcodeKind.Unknown) =>
        new()
        {
            IsSuccess = false,
            Disposition = disposition,
            ErrorCode = errorCode,
            RawBarcode = rawBarcode,
            BarcodeType = kind,
            Message = message
        };

    public static ReelCheckResult CrossWoNg(string rawBarcode, string errorCode, string message, string decodedReelId, string problemWoNo) =>
        new()
        {
            IsSuccess = false,
            Disposition = CheckDisposition.BlockWorkOrder,
            ErrorCode = errorCode,
            RawBarcode = rawBarcode,
            DecodedReelId = decodedReelId,
            BarcodeType = BarcodeKind.Reel2D,
            Message = message,
            ProblemWoNo = problemWoNo
        };
}
