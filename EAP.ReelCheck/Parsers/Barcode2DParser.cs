using System;
using System.Text;

namespace EAP.ReelCheck.Parsers;

/// <summary>
/// 負責 Cyntec 2D 條碼拆解與專屬置換對映式加解密
/// </summary>
public static class Barcode2DParser
{
    // Cyntec 專屬字元對映字典
    private const string SourceAlpha = "-QXERPWS89AKCMNT56OF2HIYBLDV4U37GZ01J"; // 密文字典
    private const string TargetAlpha = "-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // 明文字典

    /// <summary>
    /// 解析 2D 條碼字串
    /// </summary>
    public static ParsedBarcode2D Parse(string rawBarcode)
    {
        if (string.IsNullOrWhiteSpace(rawBarcode))
        {
            return new ParsedBarcode2D { IsValid = false };
        }

        string trimmed = rawBarcode.Trim();
        string[] parts = trimmed.Split('{');

        // 必須包含至少 6 個欄位才可能具備 MatNo, TapingNo, TypeCode
        if (parts.Length < 6)
        {
            return new ParsedBarcode2D
            {
                RawBarcode = trimmed,
                IsValid = false,
                MatNo = parts.Length > 0 ? parts[0].Trim() : string.Empty
            };
        }

        string matNo = parts[0].Trim();
        string tapingNo = parts.Length > 4 ? parts[4].Trim() : string.Empty;
        string typeCode = parts.Length > 5 ? parts[5].Trim() : string.Empty;
        string rawReelPart = parts[^1].Trim();

        bool is2D = int.TryParse(typeCode, out _) && typeCode.Length == 2;
        string decodedReelId = is2D ? DecodeReelId(rawReelPart) : string.Empty;

        return new ParsedBarcode2D
        {
            RawBarcode = trimmed,
            IsValid = is2D && !string.IsNullOrWhiteSpace(decodedReelId),
            Is2D = is2D,
            MatNo = matNo,
            TapingNo = tapingNo,
            TypeCode = typeCode,
            RawReelId = rawReelPart,
            DecodedReelId = decodedReelId
        };
    }

    /// <summary>
    /// 還原置換加密的 Reel ID
    /// 演算法規則：略過第 0 個 Prefix 字元，其餘字元依據字典 1:1 映射還原
    /// </summary>
    public static string DecodeReelId(string rawCipher)
    {
        if (string.IsNullOrWhiteSpace(rawCipher))
            return string.Empty;

        string cipherText = rawCipher;
        // 若包含括號或大括號取最後一段
        if (cipherText.Contains('{'))
        {
            string[] parts = cipherText.Split('{');
            cipherText = parts[^1].Trim();
        }

        if (cipherText.Length <= 1)
            return string.Empty;

        var sb = new StringBuilder(cipherText.Length - 1);
        // 從索引 1 開始（略過首位字元）
        for (int i = 1; i < cipherText.Length; i++)
        {
            char c = cipherText[i];
            int idx = SourceAlpha.IndexOf(c);
            if (idx >= 0 && idx < TargetAlpha.Length)
            {
                sb.Append(TargetAlpha[idx]);
            }
            else
            {
                sb.Append(c); // 若不在置換表內保留原樣
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 將明文 Reel ID 編碼加密 (測試或校驗用)
    /// </summary>
    public static string EncodeReelId(string plainReelId, char prefix = 'R')
    {
        if (string.IsNullOrWhiteSpace(plainReelId))
            return string.Empty;

        var sb = new StringBuilder(plainReelId.Length + 1);
        sb.Append(prefix); // 保留 Prefix

        foreach (char c in plainReelId)
        {
            int idx = TargetAlpha.IndexOf(c);
            if (idx >= 0 && idx < SourceAlpha.Length)
            {
                sb.Append(SourceAlpha[idx]);
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}

/// <summary>
/// 2D 條碼解析結果 DTO
/// </summary>
public record ParsedBarcode2D
{
    public string RawBarcode { get; init; } = string.Empty;
    public bool IsValid { get; init; }
    public bool Is2D { get; init; }
    public string MatNo { get; init; } = string.Empty;
    public string TapingNo { get; init; } = string.Empty;
    public string TypeCode { get; init; } = string.Empty;
    public string RawReelId { get; init; } = string.Empty;
    public string DecodedReelId { get; init; } = string.Empty;
}
