using System;
using System.Text.RegularExpressions;

namespace EAP.ReelCheck.Parsers;

/// <summary>
/// 條碼前置清洗、格式過濾與硬體異常修復
/// </summary>
public static class BarcodeFilter
{
    private static readonly Regex PureNumberRegex = new(@"^[0-9]+$", RegexOptions.Compiled);

    /// <summary>
    /// 對掃描輸入進行清洗與格式校驗
    /// </summary>
    public static FilterResult SanitizeAndValidate(string rawBarcode, string expectedMatNo, string expectedTapingNo)
    {
        if (string.IsNullOrWhiteSpace(rawBarcode))
        {
            return new FilterResult { IsIgnored = true, Reason = "Empty barcode input" };
        }

        string cleaned = rawBarcode.Trim().Replace("\r", "").Replace("\n", "");

        // 1. 硬體異常標籤 (NG-9801)
        if (cleaned.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase))
        {
            return new FilterResult
            {
                IsHardwareError = true,
                CleanedBarcode = cleaned,
                ErrorCode = "NG-9801",
                Reason = "掃描槍硬體讀取異常 (ERROR)"
            };
        }

        // 2. 異常符號嘗試修復 (NG-9802: 包含 ':' 與 '%')
        if (cleaned.Contains(':') && cleaned.Contains('%'))
        {
            string[] splitColon = cleaned.Split(':');
            cleaned = splitColon[0].Trim();
        }

        // 3. 異常符號嘗試修復 (NG-9803: 包含 '?')
        if (cleaned.Contains('?'))
        {
            string[] splitQuestion = cleaned.Split('?');
            foreach (var part in splitQuestion)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    cleaned = part.Trim();
                    break;
                }
            }
        }

        // 4. 防呆過濾：純數字 (NG-9901)
        if (PureNumberRegex.IsMatch(cleaned))
        {
            return new FilterResult
            {
                IsIgnored = true,
                CleanedBarcode = cleaned,
                ErrorCode = "NG-9901",
                Reason = "條碼內容為純數字，判定為無效雜訊"
            };
        }

        // 5. 防呆過濾：含特定特殊符號 (NG-9902)
        if (cleaned.Contains('+') || cleaned.Contains('.') || cleaned.Contains('_') || cleaned.StartsWith('!') || cleaned.Contains('%'))
        {
            return new FilterResult
            {
                IsIgnored = true,
                CleanedBarcode = cleaned,
                ErrorCode = "NG-9902",
                Reason = "條碼內容包含特殊符號 (+, ., _, !, %)"
            };
        }

        // 6. 防呆過濾：長度不足 9 碼 (NG-9903)
        if (cleaned.Length < 9)
        {
            return new FilterResult
            {
                IsIgnored = true,
                CleanedBarcode = cleaned,
                ErrorCode = "NG-9903",
                Reason = "條碼內容長度不足 9 碼"
            };
        }

        // 7. 防呆過濾：料號與批號黏著 (NG-9904)
        // 注意：此規則專為 1D 大標籤黏著異常設計。2D 條碼（含 '{' 分隔符）本身即同時攜帶料號與批號，不應判定為異常黏著
        if (!cleaned.Contains('{') && !string.IsNullOrEmpty(expectedMatNo) && !string.IsNullOrEmpty(expectedTapingNo))
        {
            if (cleaned.Contains(expectedMatNo) && cleaned.Contains(expectedTapingNo) &&
                cleaned.Length >= expectedMatNo.Length + expectedTapingNo.Length)
            {
                return new FilterResult
                {
                    IsIgnored = true,
                    CleanedBarcode = cleaned,
                    ErrorCode = "NG-9904",
                    Reason = "條碼同時黏附料號與批號"
                };
            }
        }

        return new FilterResult
        {
            IsValid = true,
            CleanedBarcode = cleaned
        };
    }
}

/// <summary>
/// 條碼前置過濾結果
/// </summary>
public record FilterResult
{
    public bool IsValid { get; init; }
    public bool IsIgnored { get; init; }
    public bool IsHardwareError { get; init; }
    public string CleanedBarcode { get; init; } = string.Empty;
    public string ErrorCode { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
}
