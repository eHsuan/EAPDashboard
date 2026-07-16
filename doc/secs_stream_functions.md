# SECS-II 已實作 Stream & Function (SxFy) 清單

本文件記錄了 [SecsEquipment.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsEquipment.cs) 與 [SecsMessages.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsMessages.cs) 中已實作的 SECS-II 訊息交握與強型別封裝解包規範。

## 1. Host 主動發送的訊息 (Host-Initiated Requests & Replies)

這些訊息均已在 [SecsMessages.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsMessages.cs) 中封裝為強型別靜態工廠方法，支援開發人員直接調用：

| Stream & Function | 訊息名稱 | 方向 | 觸發時機與業務邏輯 | 期待回覆 |
| :--- | :--- | :--- | :--- | :--- |
| **S1F1** | Are You There Request | Host $\rightarrow$ Eqp | 應用層 Keep-Alive 存活檢測（每 30 秒定期發送）。若 T3 超時 (30秒) 則判定設備應用程式當機並斷線。 | **S1F2** |
| **S1F2** | Are You There Reply | Host $\rightarrow$ Eqp | 當設備向 EAP 發起 S1F1 探測時，EAP 回覆 Host 的 Name 與 ID。 | (無) |
| **S1F3** | Selected Status Request | Host $\rightarrow$ Eqp | 主動向設備查詢特定 VIDs 當前的數值。 | **S1F4** |
| **S1F11** | Status Variable Namelist Request | Host $\rightarrow$ Eqp | 主動向設備請求所有或特定 SV 的屬性資訊（如名稱、單位）。 | **S1F12** |
| **S1F13** | Establish Communications Request | Host $\rightarrow$ Eqp | HSMS 進入 Selected 狀態時，主動發起建鏈。 | **S1F14** |
| **S1F14** | Establish Communications Reply | Host $\rightarrow$ Eqp | 當設備向 EAP 主動請求建鏈時，回覆 `COMMACK = 0` 同意。 | (無) |
| **S1F15** | Request OFF-LINE | Host $\rightarrow$ Eqp | 主動下發 OFF-LINE 離線命令，強迫設備進入離線模式。 | **S1F16** |
| **S1F17** | Request ON-LINE | Host $\rightarrow$ Eqp | 主動下發 ON-LINE 連線命令，強迫設備切換為 Host 控制連線狀態。 | **S1F18** |
| **S1F21** | Equipment Constant Namelist Request | Host $\rightarrow$ Eqp | 主動查詢設備的 EC 常數屬性清單。 | **S1F22** |
| **S2F13** | Equipment Constant Request | Host $\rightarrow$ Eqp | 主動讀取設備特定 ECIDs 當前的常數配置值。 | **S2F14** |
| **S2F15** | New Equipment Constant Send | Host $\rightarrow$ Eqp | 主動下發修改設備的 ECID 常數配置值。 | **S2F16** |
| **S2F17** | Date and Time Request | Host $\rightarrow$ Eqp | 主動讀取設備當前的內部時間。 | **S2F18** |
| **S2F23** | Trace Initialize Request | Host $\rightarrow$ Eqp | 1. **初始化清理**：通訊建立成功後，主動發送一次 `TOTS=0`, `DSPER=0` 重置 Trace。<br>2. **設定任務**：MES 或內部流程要求啟動 Trace 收集時發送。 | **S2F24** |
| **S2F31** | Date and Time Set Request | Host $\rightarrow$ Eqp | EAP 校時機制，向設備同步主機當前時間。 | **S2F32** |
| **S2F33** | Define Report | Host $\rightarrow$ Eqp | 動態在機台端定義一組 Report (RPTID) 綁定 VIDs 收集項目。 | **S2F34** |
| **S2F35** | Link Event Report | Host $\rightarrow$ Eqp | 將動態定義好的 Report (RPTID) 綁定到特定的事件 (CEID)。 | **S2F36** |
| **S2F37** | Enable/Disable Event Report | Host $\rightarrow$ Eqp | 事件廣播 CEID 的全局總開關，開啟或關閉 S6F11 事件上報。 | **S2F38** |
| **S2F41** | Host Command Send | Host $\rightarrow$ Eqp | 遠端控制指令發送（支援 `START`, `STOP`, `PAUSE`, `RESUME`, `PP-SELECT` 等，包含參數列表）。 | **S2F42** |
| **S2F43** | Reset Spooling Data Request | Host $\rightarrow$ Eqp | 指令設備清除 Spooling 離線暫存快取。 | **S2F44** |
| **S5F2** | Alarm Report Acknowledge | Host $\rightarrow$ Eqp | 當設備向 EAP 上報 S5F1 警報時，EAP 回覆 Alarm Ack 0。 | (無) |
| **S5F3** | Enable/Disable Alarm Send | Host $\rightarrow$ Eqp | 警報廣播 ALID 的全局總開關，開啟或關閉 S5F1 警報上報。 | **S5F4** |
| **S5F5** | List Alarms Request | Host $\rightarrow$ Eqp | 主動向機台詢問 Alarm 的屬性與 Set/Clear 狀態。 | **S5F6** |
| **S6F2** | Trace Data Acknowledge | Host $\rightarrow$ Eqp | 當設備向 EAP 上報 S6F1 數據時，EAP 回覆 Trace Ack 0。 | (無) |
| **S6F12** | Event Report Acknowledge | Host $\rightarrow$ Eqp | 當設備向 EAP 上報 S6F11 事件時，EAP 回覆 Event Ack 0。 | (無) |
| **S7F1** | Process Program Request | Host $\rightarrow$ Eqp | 主動向設備請求下載指定的配方 (PPID) 內容. | **S7F2** |
| **S7F3** | Process Program Send | Host $\rightarrow$ Eqp | 主動上傳/下發 Recipe 二進制或文字配方內容給設備。 | **S7F4** |
| **S7F17** | Process Program Delete | Host $\rightarrow$ Eqp | 主動下發命令刪除設備端指定的 Recipe。 | **S7F18** |
| **S7F19** | Current EPID Request | Host $\rightarrow$ Eqp | 主動向設備請求當前已載入或已儲存的配方 (PPID) 列表。 | **S7F20** |
| **S10F3** | Terminal Display Single | Host $\rightarrow$ Eqp | 主動在設備面板（液晶螢幕）上顯示文字訊息。 | **S10F4** |
| **S10F4** | Terminal Display Reply | Host $\rightarrow$ Eqp | 當設備向 EAP 發起 S10F3 時，EAP 自動回覆確認收到。 | (無) |

---

## 2. 設備發起、Host 處理並解包的訊息 (Equipment-Initiated Requests & Replies)

這些接收訊息均已在 [SecsMessages.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsMessages.cs) 的 `SecsMessageExtensions` 擴充類別中實作了強型別解包器（`ParseSxFy`），使接收端代碼不需要再透過 Items Index 硬編碼解析：

| Stream & Function | 訊息名稱 | 方向 | 解包後的資料結構 | EAP 處理邏輯與業務動作 |
| :--- | :--- | :--- | :--- | :--- |
| **S1F4** | Selected Status Reply | Eqp $\rightarrow$ Host | `List<string> Values` | 狀態查詢回覆。取得 S1F3 查詢之 VIDs 對應的數值列表。 |
| **S1F12** | Status Variable Namelist Reply | Eqp $\rightarrow$ Host | `List<S1F12Item>` | 變數清單屬性。包含每個變數的 `Svid`、`Name`、`Units`。 |
| **S1F22** | Equipment Constant Namelist Reply | Eqp $\rightarrow$ Host | `List<S1F22Item>` | 設定常數屬性。包含 `Ecid`、`Name`、`MinValue`、`MaxValue`、`DefValue`、`Units`。 |
| **S2F18** | Date and Time Reply | Eqp $\rightarrow$ Host | `string TimeStr` | 時間查詢回覆。取得設備目前運行的內部時間字串。 |
| **S5F1** | Alarm Report Send | Eqp $\rightarrow$ Host | `S5F1Data` | 設備警報發生或消除。EAP 解包取得 `Alid`、`Altx` 與 `IsSet` (Set/Clear)，對接 MES 的 `HandleEqpStatusCall` 進行異常報工。 |
| **S5F6** | List Alarm Data | Eqp $\rightarrow$ Host | `List<S5F6Item>` | 警報狀態查詢回覆。取得所有 Alarm 的 `Alid`、`Altx` 與是否 Set 狀態。 |
| **S6F1** | Trace Data Send | Eqp $\rightarrow$ Host | `S6F1Data` | 設備定頻 Trace 上報。EAP 解包取得 `Trid`、`Smpln`、`Stime` 與採樣 VIDs 值，記錄至本地 CSV 並以 KPI 參數報工給 MES。 |
| **S6F11** | Event Report Send | Eqp $\rightarrow$ Host | `S6F11Data` | 設備事件/狀態廣播。EAP 解包取得 `Ceid` 與綁定的報告項目：<br>- **進站 (CEID 200)** / **出站 (CEID 206)**：執行 Track In/Out MES 報工與自動 PP-SELECT Recipe 下發。<br>- **製程 EDC 資料 (CEID 102)**：動態提取報告中所有 VID 數值，記錄並報工給 MES。<br>- **狀態變更 (CEID 50-53)**：更新 EAP 畫面狀態，並上傳狀態代碼給 MES。 |
| **S7F20** | Current EPID Data | Eqp $\rightarrow$ Host | `List<string> Ppids` | 配方清單回覆。取得目前設備上存在的所有配方檔案名稱列表。 |
| **S10F3** | Terminal Display Single | Eqp $\rightarrow$ Host | `S10F3Data` | 設備在 EAP 終端上顯示訊息。EAP 解包取得 `Tid` 與 `Text`，記錄至日誌並自動回覆 S10F4。 |
