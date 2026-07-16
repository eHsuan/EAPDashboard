# SECS-II 已實作 Stream & Function (SxFy) 清單

本文件記錄了 [SecsEquipment.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsEquipment.cs) 中已實作的 SECS-II 訊息交握規範。

## 1. Host 主動發送的訊息 (Host-Initiated Messages)

| Stream & Function | 訊息名稱 | 方向 | 觸發時機與業務邏輯 | 期待回覆 |
| :--- | :--- | :--- | :--- | :--- |
| **S1F1** | Are You There Request | Host $\rightarrow$ Eqp | 應用層存活檢測（每 30 秒定期發送）。若 T3 超時 (5秒) 則判定設備應用程式當機並斷線。 | **S1F2** |
| **S1F13** | Establish Communications Request | Host $\rightarrow$ Eqp | HSMS 進入 Selected 狀態時，主動發起建鏈。 | **S1F14** |
| **S2F23** | Trace Initialize Request | Host $\rightarrow$ Eqp | 1. **初始化清理**：通訊建立成功後，主動發送一次 `TOTS=0`, `DSPER=0` 重置 Trace。<br>2. **設定任務**：MES 或內部流程要求啟動 Trace 收集時發送。 | **S2F24** |
| **S2F31** | Date and Time Set Request | Host $\rightarrow$ Eqp | EAP 校時機制，定期向設備同步主機時間。 | **S2F32** |
| **S2F41** | Host Command Send | Host $\rightarrow$ Eqp | 遠端控制指令發送（如 `START`, `STOP`, `PAUSE`, `RESUME`, `PP-SELECT` 等，包含參數列表）。 | **S2F42** |

---

## 2. 設備發起、Host 處理並回覆的訊息 (Equipment-Initiated Messages)

| Stream & Function | 格式類型 | 方向 | EAP 處理邏輯與業務動作 | Host 回覆 |
| :--- | :--- | :--- | :--- | :--- |
| **S1F1** | Are You There Request | Eqp $\rightarrow$ Host | 設備主動確認 Host 存活。EAP 包裝 Host 的 Name 與 ID 回覆 S1F2。 | **S1F2** |
| **S1F13** | Establish Communications Request | Eqp $\rightarrow$ Host | 設備主動發起建鏈。EAP 回覆 `COMMACK = 0` 表示同意建鏈。 | **S1F14** |
| **S5F1** | Alarm Report Send | Eqp $\rightarrow$ Host | 設備警報發生或消除。EAP 優先提取 `ALID` 進行警報轉譯（降級使用 `ALTX`），並直接呼叫 `HandleEqpStatusCall` 報工給 MES。 | **S5F2** (`ACKC5=0`) |
| **S6F1** | Trace Data Send | Eqp $\rightarrow$ Host | 設備定期上報 Trace 數據。EAP 依 Index 對應順序還原為邏輯變數名稱，將資料寫入 CSV 記錄，並以 KPI 參數報工給 MES。 | **S6F2** (`ACKC6=0`) |
| **S6F11** | Event Report Send | Eqp $\rightarrow$ Host | 設備狀態/事件上報：<br>- **進站 (CEID 200)** / **出站 (CEID 206)**：執行自動報工。<br>- **製程資料 (CEID 102)**：動態提取所有 VID 值，寫入本地日誌並報工給 MES。<br>- **狀態變更 (CEID 50-53)**：更新 EAP 畫面狀態，並上傳狀態代碼給 MES。 | **S6F12** (`ACKC6=0`) |
