using System;
using System.Collections.Generic;

namespace EAP.ReelCheck.Configuration;

/// <summary>
/// EAP 防混料 WCF 連線與參數組態
/// </summary>
public class ReelCheckOptions
{
    /// <summary>
    /// MES WCF 主要伺服端點 (例如 http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc)
    /// </summary>
    public string ServerUrl { get; set; } = "http://twcynmeswebqa01/WcfEntranceTest/WCF/Wcf.svc";

    /// <summary>
    /// 本地客戶端回呼端點 (HTTP 雙工或識別用)
    /// </summary>
    public string ClientUrl { get; set; } = "http://127.0.0.1:8080/EAP_ReelCheck";

    /// <summary>
    /// 備用伺服端點清單 (容錯移轉 Failover 用)
    /// </summary>
    public List<string> BackupServerUrls { get; set; } = new();

    /// <summary>
    /// 連線與交易逾時秒數 (預設 30 秒)
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// 客戶端識別名稱前綴
    /// </summary>
    public string ClientIdPrefix { get; set; } = "ReelCheck @ ";

    /// <summary>
    /// 是否啟用斷線自癒與自動重連
    /// </summary>
    public bool AutoReconnect { get; set; } = true;
}
