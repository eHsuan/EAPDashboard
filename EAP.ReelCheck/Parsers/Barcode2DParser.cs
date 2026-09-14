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
        string[] allParts = trimmed.Split('{');

        string matNo = string.Empty;
        string tapingNo = string.Empty;
        string typeCode = string.Empty;
        string rawReelPart = string.Empty;

        // 軌道一：標準 Cyntec 9 段固定格式 (array[0]=MatNo, array[4]=TapingNo, array[5]=TypeCode, array[8]或最後一段=ReelID)
        if (allParts.Length >= 9 && allParts[5].Trim().Length == 2 && int.TryParse(allParts[5].Trim(), out _))
        {
            matNo = allParts[0].Trim();
            tapingNo = allParts[4].Trim();
            typeCode = allParts[5].Trim();
            rawReelPart = allParts[^1].Trim();
        }
        // 軌道二：智慧特徵錨點容錯解析 (不管大括號是否被合併、跳脫或縮減皆能精準對齊)
        else
        {
            var tokens = allParts.Select(p => p.Trim())
                                 .Where(p => !string.IsNullOrEmpty(p))
                                 .ToList();

            // 尋找長度為 2 的純數字作為 TypeCode 特徵錨點 (如 88, 86)
            int typeIdx = tokens.FindIndex(t => t.Length == 2 && int.TryParse(t, out _));

            if (typeIdx >= 0)
            {
                typeCode = tokens[typeIdx];

                // 錨點後方的最後一個非空 Token 必為 Reel 密文
                if (tokens.Count > typeIdx + 1)
                {
                    rawReelPart = tokens[^1];
                }

                // 錨點正前方緊鄰的 Token 必為工單批號 TapingNo
                if (typeIdx >= 1)
                {
                    tapingNo = tokens[typeIdx - 1];
                }

                // 最前端第一個 Token 必為工單料號 MatNo
                if (typeIdx >= 2)
                {
                    matNo = tokens[0];
                }
            }
            else if (allParts.Length >= 6)
            {
                // 若無 2 位數純數字，保留基礎防呆取值
                matNo = allParts[0].Trim();
                tapingNo = allParts.Length > 4 ? allParts[4].Trim() : string.Empty;
                typeCode = allParts.Length > 5 ? allParts[5].Trim() : string.Empty;
                rawReelPart = allParts[^1].Trim();
            }
        }

        bool is2D = int.TryParse(typeCode, out _) && typeCode.Length == 2;
        string decodedReelId = is2D && !string.IsNullOrWhiteSpace(rawReelPart)
            ? DecodeReelId(rawReelPart)
            : string.Empty;

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
