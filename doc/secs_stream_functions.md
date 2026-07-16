# EAPDashboard - SECS 模組已實作 Stream & Function 分析報告

本文件條列並解析 `EAPDashboard` 專案中，[SecsEquipment.cs](file:///D:/SourceCode/CS/EAPDashboard/Models/SecsEquipment.cs) 模組目前已實作的 **SECS-II Stream & Function (SxFy)** 訊息類型及其在工業自動化系統中所代表之業務邏輯。

---

## 1. 設備端主動發送 (Equipment-Initiated)

這類訊息由設備端主動發報，EAP 負責接收、解析並處理後續的 MES 業務邏輯，最後回覆對應的應答訊息（Acknowledge）。

### 📌 S1F1 $\rightarrow$ S1F2 (Are You There $\rightarrow$ On Line Data)
* **訊息方向**：設備 $\rightarrow$ EAP (S1F1)；EAP $\rightarrow$ 設備 (S1F2)
* **用途**：設備端主動確認 EAP 主機是否在線（連通性檢測）。
* **EAP 處理行為**：EAP 會以 `S1F2` 回覆，回傳封裝 EAP 名稱與識別碼的列表結構：`Item.L(Item.A(Name), Item.A(Id))`。

### 📌 S1F13 $\rightarrow$ S1F14 (Establish Communications Request $\rightarrow$ Acknowledge)
* **訊息方向**：設備 $\rightarrow$ EAP (S1F13)；EAP $\rightarrow$ 設備 (S1F14)
* **用途**：設備端主動發起 HSMS 連線建鏈交握。
* **EAP 處理行為**：EAP 回覆 `S1F14` 並帶有狀態位元組 `Item.B(0)`，代表建鏈交握成功。

### 📌 S5F1 $\rightarrow$ S5F2 (Alarm Report Send $\rightarrow$ Acknowledge)
* **訊息方向**：設備 $\rightarrow$ EAP (S5F1)；EAP $\rightarrow$ 設備 (S5F2)
* **用途**：設備端主動回報警報狀態變更（發生警報或消除警報）。
* **EAP 處理行為**：
  1. 解析 `ALCD`（警報代碼位元組），判斷是警報觸發（`ALCD` 包含 `0x80` 或為 `1`）或是警報解除。
  2. 提取 `ALTX`（警報內容文字）作為原始警報碼，並透過 `TransformAlarmCode` 對映為 MES 警報代碼。
  3. 將警報狀態上報給 MES（`StatusCode: "1"` 代表觸發，`"2"` 代表解除）。
  4. 回覆 `S5F2`（帶有位元組 `0`）告知設備已收到。

### 📌 S6F11 $\rightarrow$ S6F12 (Event Report Send $\rightarrow$ Acknowledge)
* **訊息方向**：設備 $\rightarrow$ EAP (S6F11)；EAP $\rightarrow$ 設備 (S6F12)
* **用途**：設備端主動回報機台製程事件（以 `CEID` 事件識別碼區分）。
* **EAP 處理行為**：EAP 解析其 `SecsItem` 以取得 `CEID` 以及關聯的 `VID`（變數識別碼），並回覆 `S6F12`（帶有位元組 `0`）。系統會根據 `CEID` 進行以下自動化分流：
  * **`CEID = "200"` (Track In Event / 進料事件)**：讀取 `VID 300` 或 `320` 取得載具 ID (Cassette ID)，向 Gateway 查詢綁定工單，並向 MES 查詢製程配方名稱後，主動發送 `S2F41` (PP-SELECT) 下載配方。
  * **`CEID = "206"` (Track Out Event / 出料事件)**：讀取 `VID 111` 取得工單，並呼叫 MES 進行出站報工（WOCHECKOUT）。
  * **`CEID = "50", "51", "52", "53"` (Status Change / 狀態變更)**：讀取狀態變更變數（`VID 102`），更新 EAP UI 上的設備狀態，並上報 MES。

---

## 2. EAP 主動發送 (Host-Initiated)

這類訊息由 EAP 主動發起，用於連線初始化、時間校正或下達控制指令。

### 📌 S1F13 $\rightarrow$ S1F14 (Establish Communications Request $\rightarrow$ Acknowledge)
* **訊息方向**：EAP $\rightarrow$ 設備 (S1F13)；設備 $\rightarrow$ EAP (S1F14)
* **用途**：在 HSMS 連線狀態轉移為 `Selected` 時，EAP 主動發起建鏈。
* **EAP 處理行為**：發送 `S1F13`，等待設備回傳 `S1F14`。當確認設備回覆之首個位元組值為 `0` 時，將 EAP 連線狀態標記為 `Connected`。

### 📌 S2F31 $\rightarrow$ S2F32 (Date and Time Set Request $\rightarrow$ Acknowledge)
* **訊息方向**：EAP $\rightarrow$ 設備 (S2F31)；設備 $\rightarrow$ EAP (S2F32)
* **用途**：時間校正/同步。
* **EAP 處理行為**：當使用者在 Dashboard 選單中點擊時間校正時，EAP 會發送 `S2F31`，將目前主機時間格式化為 `"yyyyMMddHHmmss"` 格式傳送給設備，進行時間同步。

### 📌 S2F41 $\rightarrow$ S2F42 (Host Command Send $\rightarrow$ Acknowledge)
* **訊息方向**：EAP $\rightarrow$ 設備 (S2F41)；設備 $\rightarrow$ EAP (S2F42)
* **用途**：主機遠端命令下發。
* **EAP 處理行為**：於進站交握流程中（`CEID = "200"`），EAP 從 MES 獲取配方名稱後，會主動向設備發送指令名稱為 `"PP-SELECT"` 的 `S2F41` 訊息，並帶上參數 `PPID = [配方名稱]`。EAP 會等待設備回應 `S2F42`，確認其 `HCACK`（第一個位元組）為 `0` (Success) 後，方執行 MES 進站報工完成（WOCHECKIN）。
