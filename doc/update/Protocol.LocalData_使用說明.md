# Protocol.LocalData.dll 使用說明

本文件說明 `Protocol.LocalData.dll` 的用途、初始設定、全部 Function 的描述與使用方式，供串接此 DLL 的開發人員參考。

---

## 概述

`Protocol.LocalData.dll` 是一支本地端資料存取函式庫，負責將 EAP（設備自動化程式）收集到的設備資料寫入本地端 PostgreSQL 資料庫，供前端網頁查詢顯示。

涵蓋的資料領域：

| 領域 | 說明 |
|---|---|
| 設備清單 | EAP 管轄的設備編號與名稱 |
| 設備狀態 | RUN / IDLE / DOWN / ALARM 等即時與歷史狀態 |
| 機台參數 | 各設備的實際值、上下限、告警狀態（EAV 動態結構） |
| CycleTime | 每個生產週期的週期時間 |
| Availability | 可動率區段統計 |
| 工單 | 目前工單號碼與異動歷程 |
| Alarm | EAP 上報的設備告警,含開始/結束時間配對 |
| 產出 | 產出數量與不良數 |

---

## 環境需求

- .NET 8.0 以上
- PostgreSQL（本地端安裝）
- NuGet 套件：`Npgsql 10.0.3`（已內含於 DLL，無需額外安裝）

---

## 初始設定

### 步驟 1：建立資料庫

在 pgAdmin 或 psql 建立一個新的資料庫，例如：

```sql
CREATE DATABASE Protocol_localdata;
```

### 步驟 2：執行 Migration 建立資料表

執行 `Migrations\run_migrations.bat`，會依序建立所有資料表與分區維護函式。

> 執行前請確認 `run_migrations.bat` 內的連線設定（Host、Port、Database、Username、Password）與 psql.exe 路徑正確。

```
Migrations\
 ├─ 001_init_schema.sql               ← 建立所有資料表與索引
 ├─ 002_partition_maintenance.sql     ← 建立機台參數分區維護函式
 ├─ 003_equipment_registry.sql        ← 建立設備清單表
 ├─ 004_add_param_unit.sql            ← 機台參數新增單位欄位(param_unit)
 ├─ 005_equipment_alarm.sql           ← 建立設備 Alarm 現況/歷程表
 ├─ 006_workorder_multi.sql           ← 工單支援一台設備對應多張工單(分槽/合併批次)
 ├─ 007_workorder_flow_id.sql         ← 工單支援回流(flow_id 加入主鍵)
 ├─ 008_workorder_piece.sql           ← 建立片級追溯表(workorder_piece / equipment_workorder_piece_log)
 ├─ 009_cycle_time_flow_piece.sql     ← CycleTime 新增 flow_id / piece_id
 ├─ 010_output_flow_input_qty.sql     ← 產出新增 flow_id / input_qty
 ├─ 011_status_light_map.sql          ← 建立狀態燈號對照表(含14筆種子資料)
 ├─ 012_oee_daily_log.sql             ← 建立單日 OEE / 四色秒數彙總表
 ├─ 013_quality_spc.sql               ← 建立量測項目/SPC規則主檔與SPC量測明細
 ├─ 014_mes_code_cache.sql            ← 建立 MES 代碼快取表
 ├─ 015_equipment_registry_mes_fields.sql ← equipment_registry 新增 MES 機台主檔欄位
 ├─ 016_workorder_mes_fields.sql       ← 工單新增 MES 情境欄位(mat_no/line_code/work_center_no)
 ├─ 017_carrier_support.sql            ← 載具支援(equipment_registry.trace_capability + equipment_workorder_piece_log.carrier_id)
 ├─ 018_oee_availability_calc.sql      ← OEE/Availability 改由 DB 端 calc_oee_daily()/calc_availability_period() 計算
 └─ run_migrations.bat                ← 雙擊執行，一鍵完成所有建表動作
```

---

## 使用方式

### 初始化（必讀）

`LocalDataService` **必須以單一長期存活的實例使用**，不可每次呼叫都 `new` 一個新實例，否則機台參數的記憶體異動快取會失效，導致每筆資料都被寫入 DB，失去「異動才寫」的效果。

```csharp
// 正確：整個 App 生命週期共用同一個實例
private static readonly LocalDataService _localData = new LocalDataService(
    "Host=localhost;Port=5432;Database=Protocol_localdata;Username=postgres;Password=xxxxxx"
);
```

### 時間注意事項

所有時間參數**一律傳入 UTC 時間**，請使用 `DateTimeOffset.UtcNow`，不可使用 `DateTimeOffset.Now`（含本地時區 +08:00 會造成寫入錯誤）。

```csharp
// 正確
DateTimeOffset.UtcNow

// 錯誤（會報錯）
DateTimeOffset.Now
```

---

## EAP 啟動流程（建議順序）

```csharp
// 1. 同步設備清單（UPSERT 清單內 + 刪除清單外）
_localData.SyncEquipments(new[]
{
    ("EQ001", (string?)"射出機 A"),
    ("EQ002", (string?)"射出機 B"),
});

// 2. 確保各分區表未來分區存在（每天至少呼叫一次）
_localData.EnsureParameterLogPartitions();
_localData.EnsureWorkorderPieceLogPartitions();
_localData.EnsureSpcMeasurementLogPartitions();

// 3. 啟動每日 Timer，確保分區持續維護
var partitionTimer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
partitionTimer.Elapsed += (_, _) =>
{
    _localData.EnsureParameterLogPartitions();
    _localData.EnsureWorkorderPieceLogPartitions();
    _localData.EnsureSpcMeasurementLogPartitions();
};
partitionTimer.AutoReset = true;
partitionTimer.Start();
```

> **Availability / OEE 日彙總不需要 EAP 自己排程呼叫。** `CalcAvailabilityPeriod` / `CalcOeeDaily` 改由獨立的 `Protocol.LocalData.Scheduler.exe` 搭配 Windows 工作排程器每小時觸發一次計算，EAP 端不需要、也不建議再自己開 Timer 呼叫這兩個方法（曾經這樣建議過，2026-08-31 起已移除；理由與部署方式見下方「Availability」「OEE 日彙總」章節，以及文件最後「外部排程程式」章節）。
>
> `EnsureWorkorderPieceLogPartitions` / `EnsureSpcMeasurementLogPartitions` 只負責預先建立未來分區,**不會**像 `EnsureParameterLogPartitions` 一樣刪除舊分區——`equipment_workorder_piece_log` / `equipment_spc_measurement_log` 是追溯資料,不設保留期自動清除。

---

## Function 說明

### 維運

#### `EnsureParameterLogPartitions(retentionDays = 14, lookaheadDays = 2)`

確保 `equipment_parameter_log` 未來分區存在，並刪除超過保留期的舊分區。

- **使用時機**：EAP 啟動時呼叫一次，並搭配每日 Timer 定時呼叫。
- **重複呼叫安全**：同一天呼叫多次完全沒問題，已存在的分區會自動跳過。
- **未呼叫的風險**：超過 `lookaheadDays`（預設 2 天）未呼叫，寫入機台參數時會報錯。

```csharp
_localData.EnsureParameterLogPartitions();
// 或自訂保留天數
_localData.EnsureParameterLogPartitions(retentionDays: 14, lookaheadDays: 2);
```
Timer範例
var partitionTimer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
partitionTimer.Elapsed += (_, _) => localData.EnsureParameterLogPartitions();
partitionTimer.AutoReset = true;
partitionTimer.Start();
---

### 設備清單

#### `SyncEquipments(equipments)`

同步設備清單：UPSERT 清單內的設備，**自動刪除** DB 中不在清單內的設備。

- **使用時機**：EAP 啟動時，傳入完整的設備管轄清單。
- **效果**：DB 的設備清單與程式設定完全一致，無需手動管理差異。

```csharp
_localData.SyncEquipments(new[]
{
    ("EQ001", (string?)"射出機 A"),
    ("EQ002", (string?)"射出機 B"),
    ("EQ003", (string?)"CNC 加工機"),
});
```

#### `RegisterEquipments(equipments)`

批次登錄設備，只新增或更新，**不刪除** DB 中其他設備。

- **使用時機**：需要新增設備但不想影響現有清單時使用。

```csharp
_localData.RegisterEquipments(new[]
{
    ("EQ004", (string?)"檢測站"),
});
```

#### `RegisterEquipment(equipmentNo, equipmentName)`

單筆設備登錄。

```csharp
_localData.RegisterEquipment("EQ005", "包裝機");
```

#### `UnregisterEquipment(equipmentNo)`

手動刪除單台設備。

- **使用時機**：某台設備臨時調離此 EAP 管轄時。

```csharp
_localData.UnregisterEquipment("EQ005");
```

#### `UpdateEquipmentMesInfo(equipmentNo, lineCode, enable, machAlias, groupName)`

寫入這台設備的 MES 機台主檔資訊（產線代碼／啟用狀態／別名／群組）。

- **架構原則**：Dashboard 純讀取、不會直接呼叫 MES,所以只要畫面上要顯示「來自 MES 的資訊」,就必須由 **EAP** 主動呼叫 MES、把結果寫進這張表,Dashboard 才讀得到。
- **使用時機**：EAP 呼叫 MES 機台主檔查詢 API（依 equipmentNo）取得結果後呼叫,異動頻率低,直接覆蓋更新、不留歷史。
- 需先透過 `RegisterEquipment`/`SyncEquipments` 建立過這筆設備,本方法只 `UPDATE`、不會新增設備。**若該設備尚未登錄,會拋出 `InvalidOperationException`**（2026-09 起的行為;修正前 UPDATE 比對到 0 筆會無聲無息地不做任何事,呼叫端完全不會發現資料沒寫進去）,呼叫端請確保 EAP 啟動流程一定先呼叫過 `SyncEquipments`/`RegisterEquipment`,或自行 try/catch 處理。

```csharp
_localData.UpdateEquipmentMesInfo("EQ001", lineCode: "L01", enable: "Y", machAlias: "射出機A-1號機", groupName: "射出成型");
```

#### `UpdateEquipmentTraceCapability(equipmentNo, traceCapability)`

寫入這台設備一般具備的感測能力：`"PIECE_ONLY"`（只認片）/ `"CARRIER_ONLY"`（只認載具）/ `"BOTH"`（兩者都認）。

- 這是給 EAP 寫入邏輯/資料品質檢核參考用的**靜態分類**，不是每筆事件的強制規則——下面「片/載具追蹤」小節的 `StartEquipmentTraceEvent`/`EndEquipmentTraceEvent` 實際要不要填 `pieceId`/`carrierId`，以該次事件實際感測到什麼為準，不受這個分類限制。
- 同一台設備實際上可能三種情況都會遇到（例如裝卸片站平常兩者都讀得到，但偶爾只讀到其中一個），這裡只是記一個「一般來說」的分類。
- 需先透過 `RegisterEquipment`/`SyncEquipments` 建立過這筆設備,本方法只 `UPDATE`。**若該設備尚未登錄,會拋出 `InvalidOperationException`**（理由同上面 `UpdateEquipmentMesInfo`）。

```csharp
_localData.UpdateEquipmentTraceCapability("EQ001", "BOTH");
```

---

### 設備狀態

#### `WriteEquipmentStatus(equipmentNo, statusCode, statusDesc, eventTime)`

寫入設備狀態，同時更新現況快照（`equipment_status_current`）與歷史紀錄（`equipment_status_log`）。

- **使用時機**：設備狀態發生變化時呼叫。
- **statusCode**：實際上是 `-1`、`0`~`12` 共 14 種數值代碼（詳見 `status_light_map`），不是文字 RUN/IDLE/DOWN/ALARM。
- **statusDesc（選填）**：`status_code` 的通用說明已經有 `status_light_map` 可查（14 種代碼的種子資料），**不需要每次都重複填一樣的固定文字**（例如 status_code="0" 就填"自動運行"）——這種情況直接傳 `null`，查詢端 JOIN `status_light_map` 取得即可。只有這次事件有「比通用說明更具體」的情境資訊時才填（例如是哪個感測器斷線、哪個原因觸發這次黃燈），讓 `status_desc` 承載的是額外資訊，不是 `status_light_map` 已經有的重複內容。

```csharp
// 一般情況：通用說明交給 status_light_map，不重複填
_localData.WriteEquipmentStatus("EQ001", "0", null, DateTimeOffset.UtcNow);

// 有比通用說明更具體的情境時才填 statusDesc
_localData.WriteEquipmentStatus("EQ001", "1", "TEMP1 感測器讀值超過上限 15°C", DateTimeOffset.UtcNow);
```

---

### 機台參數

#### `WriteEquipmentParameters(equipmentNo, parameters)` ← **批量寫入（建議優先使用）**

批量寫入同一台設備的多個參數，**整批只開一次 DB 連線 / 一個 Transaction**。

- **使用時機**：每個 scan cycle 收完整台設備的所有參數後，一次呼叫。
- **效能差異**：與單筆呼叫相比，10 台 × 20 個參數，每輪從 200 次連線降為 10 次。
- **threshold 機制相同**：每個參數仍套用記憶體快取比對，未異動的參數不寫入 DB，也不影響 Transaction 內容。

```csharp
var now = DateTimeOffset.UtcNow;
var parameters = new[]
{
    new ParameterData("TEMP1",     "加熱溫度", 185.3m, 200m, 150m, false, now, threshold: 0.5m, paramUnit: "°C"),
    new ParameterData("PRESSURE1", "射出壓力",  95.0m, 150m,  55m, false, now, threshold: 2.0m, paramUnit: "bar"),
    new ParameterData("SPEED1",    "螺桿轉速",  980m, 1200m, 850m, false, now, threshold: 5.0m, paramUnit: "rpm"),
};

_localData.WriteEquipmentParameters("EQ001", parameters);
```

> **paramUnit** 為選填，未提供時寫入 NULL。既有呼叫端不需修改即可繼續使用。

#### `WriteEquipmentParameter(equipmentNo, paramCode, paramName, actualValue, upperLimit, lowerLimit, isAlarm, recordTime, threshold = 0, paramUnit = null)` ← 單筆寫入

單筆寫入一個參數，每次各自開一個連線。

- **使用時機**：僅需寫入少數幾個參數、或參數不會同時收齊時使用。
- 邏輯與批量版本完全相同（記憶體快取 threshold 判斷）。

```csharp
_localData.WriteEquipmentParameter(
    equipmentNo: "EQ001",
    paramCode:   "TEMP1",
    paramName:   "加熱溫度",
    actualValue: 185.3m,
    upperLimit:  200m,
    lowerLimit:  150m,
    isAlarm:     false,
    recordTime:  DateTimeOffset.UtcNow,
    threshold:   0.5m,
    paramUnit:   "°C"
);
```

> **isAlarm** 由呼叫端計算（`actualValue > upperLimit || actualValue < lowerLimit`），告警狀態改變時一定會寫入，不受 threshold 限制。

---

### CycleTime

#### `WriteCycleTime(equipmentNo, woNo, cycleTimeSec, recordTime, flowId = null, pieceId = null)`

寫入單筆生產週期時間。

- **使用時機**：每完成一個生產週期呼叫一次。
- **flowId**：工單回流到同一台設備時,用來區分不同流程站次；無回流情境可省略。
- **pieceId**：對應片級追溯碼(邏輯上 FK → `workorder_piece`,DB 層未建實體約束)；無片級追溯情境可省略。

```csharp
_localData.WriteCycleTime("EQ001", "WO20260630001", cycleTimeSec: 12.5m, DateTimeOffset.UtcNow);

// 有回流站次 + 片級追溯時
_localData.WriteCycleTime("EQ001", "WO20260630001", 12.5m, DateTimeOffset.UtcNow, flowId: "F2", pieceId: "P00012345");
```

---

### Availability

> **2026-08-25 起改由 DB 端排程計算**，不再由 EAP 自己維護狀態計時器、算好結果再推送。原本 EAP 端要自己疊加 run/down 時間、呼叫 `WriteAvailability` 的做法已經不需要了——`WriteAvailability` 方法還留在原始碼裡（用 `/* ... */` 註解掉），沒有刪除，供未來需要退回應用層計算或人工覆蓋單筆資料時參考，但目前不建議呼叫。

#### `CalcAvailabilityPeriod(periodStart, periodEnd)`

呼叫 DB 端的 `calc_availability_period()` function，直接從 `equipment_status_log` 算出 `[periodStart, periodEnd)` 這段區間裡每一台設備的可動率（綠燈代碼 0、6 視為運轉中，其餘視為停機），寫入一筆新的 `equipment_availability_log`。

- **使用時機**：由獨立的 `Protocol.LocalData.Scheduler.exe` 搭配 Windows 工作排程器排程呼叫，例如每小時呼叫一次算「剛結束的這一小時」——**EAP 端不需要、也不建議自己再開 Timer 呼叫這個方法**。
- **為什麼不用 pg_cron**：地端 Postgres 環境不一定裝得到這個擴充套件（這台機器實測也沒裝），所以排程邏輯改放在獨立的 .NET 排程程式，而不是寫進 DB。
- **為什麼不寫進 EAP 裡的 Timer（2026-08-31 前的做法）**：Timer 埋在 EAP 程式碼裡，排程會跟著 EAP process 的生死走（EAP 當掉/重啟時排程也停了，且不會有任何提示），而且要求每個接 DLL 的工程師自己把 Timer 程式碼抄對，容易漏寫或寫錯。改用獨立的 `Protocol.LocalData.Scheduler.exe`（隨 DLL 一起發布在 `publish_scheduler/` 資料夾）搭配 Windows 工作排程器，排程本身是作業系統層級的，跟 EAP 是否存活無關，也自帶執行歷史可查、失敗會被看到。詳細部署方式見文件最後「外部排程程式 Protocol.LocalData.Scheduler.exe」章節。
- **稼動率計算公式**：`可動率 = 運轉時間 / (運轉時間 + 停機時間)`，由 function 內部算好。

```csharp
// Protocol.LocalData.Scheduler 內部呼叫方式(僅供參考,現場不需要自己寫這段)
var now = DateTimeOffset.UtcNow;
_localData.CalcAvailabilityPeriod(now.AddHours(-1), now);
```

---

### 工單

一台設備可能同時對應多張工單（分槽/分站設備，例如電鍍、滾鍍，一台設備下有多個槽體各自對應一張工單；或條件相同的合併批次設備，一台設備同時掛多張工單一起跑、沒有物理分位），因此工單改採「開始/結束」各自獨立呼叫的設計，不再是「設定唯一的現在工單」。

#### `StartWorkOrder(equipmentNo, woNo, slotNo, startTime, flowId = null, matNo = null, lineCode = null, workCenterNo = null)`

開始一張工單。

- **slotNo**：分槽/分站設備才傳（例如槽位編號），DLL 會先關閉同一槽位上「別的工單」（不同 wo_no，或同 wo_no 不同 flowId）原本進行中的舊工單，再開立這張新工單——同一槽位同時只會有一張進行中工單。
- 若設備沒有物理分位（合併批次設備），`slotNo` 傳 `null`：不會關閉該設備上其他任何工單，單純新增一筆進行中工單，可與其他工單並存。
- **flowId**：流程/站次序號。工單可能回流到同一台設備，同一 `woNo` 在不同流程站次會被視為不同的進行中紀錄（`equipment_workorder_current` 主鍵已擴充為 `(equipment_no, wo_no, flow_id)`）。省略時視為單一預設站次，行為與升級前完全相同。
- **matNo / lineCode / workCenterNo**：這張工單當下的產品料號／產線代碼／工作站代碼。**架構原則：Dashboard 不會直接呼叫 MES**，所以這三個值必須由 **EAP** 建立這筆工單紀錄時,自己呼叫 MES 依 `woNo` 查詢當下情境後傳入——DLL 只負責寫入 current 與 log 兩張表,不會替你查 MES。Dashboard 之後 JOIN 這兩張表就能拿到,不用再呼叫 MES。

```csharp
// 分槽設備：槽位 3 換上新工單，原本槽位 3 的舊工單自動結清
_localData.StartWorkOrder("EQ001", "WO20260630002", "3", DateTimeOffset.UtcNow);

// 沒有分位的設備：新增一張工單，不影響設備上其他還在跑的工單
_localData.StartWorkOrder("EQ002", "WO20260630003", null, DateTimeOffset.UtcNow);

// 工單回流同一台設備的第二個站次(同產品/同產線,只是工作站換了)
_localData.StartWorkOrder(
    "EQ001", "WO20260630002", "5", DateTimeOffset.UtcNow, flowId: "2",
    matNo: "MAT001", lineCode: "L01", workCenterNo: "WC02");
```

#### `EndWorkOrder(equipmentNo, woNo, endTime, flowId = null)`

明確結束「這一張」工單：從現況移除、補上歷程的 `end_time`，不影響同一台設備上其他還在進行中的工單（含同 `woNo` 不同 `flowId` 的另一筆）。

- **使用時機**：這張工單真正結案/下機時呼叫。沒有分位的設備要換單時，必須先呼叫本方法結束舊工單，再呼叫 `StartWorkOrder` 開新工單——兩者不會自動互相取代。
- **flowId**：需與對應的 `StartWorkOrder` 一致，才能配對到同一筆。

```csharp
_localData.EndWorkOrder("EQ002", "WO20260630003", DateTimeOffset.UtcNow);
_localData.EndWorkOrder("EQ001", "WO20260630002", DateTimeOffset.UtcNow, flowId: "2");
```

#### `SyncWorkOrders(equipmentNo, workOrders, asOfTime)`

整批同步一台設備目前所有進行中的工單：UPSERT 清單內的工單、結束 DB 現況裡「不在這次清單內」的舊工單，不需要呼叫端自己算出誰新增誰結束。寫法比照既有的 `SyncEquipments`。

- **使用時機**：EAP 每次輪詢就回報「目前全部槽位/全部工單現況」時整批呼叫，取代逐筆呼叫 `StartWorkOrder`/`EndWorkOrder`。適合分槽設備（一次回報 10 個槽位現況）或合併批次設備（一次回報目前掛的所有工單）。
- 清單傳空陣列，代表這台設備現在沒有任何進行中工單，DLL 會把現況裡所有還開著的工單全部結清。
- **workOrders 的 tuple 是 `(WoNo, SlotNo, StartTime, FlowId, MatNo, LineCode, WorkCenterNo)`**：是否「還在這次清單內」一律以 `(WoNo, FlowId)` 這組合判斷，不能只看 `WoNo`（工單可能回流到同一台設備）。`MatNo`/`LineCode`/`WorkCenterNo` 意義與 `StartWorkOrder` 的同名參數一致,一樣要由 EAP 查完 MES 再傳入。無回流/無 MES 情境對應欄位傳 `null` 即可。

```csharp
// 分槽設備：一次回報 3 個槽位目前各自在跑的工單
_localData.SyncWorkOrders("EQ001", new[]
{
    ("WO20260630001", (string?)"1", DateTimeOffset.UtcNow.AddMinutes(-30), (string?)null, (string?)"MAT001", (string?)"L01", (string?)"WC01"),
    ("WO20260630002", (string?)"2", DateTimeOffset.UtcNow.AddMinutes(-10), (string?)null, (string?)"MAT002", (string?)"L01", (string?)"WC02"),
    ("WO20260630003", (string?)"3", DateTimeOffset.UtcNow, (string?)null, (string?)"MAT001", (string?)"L01", (string?)"WC03"),
}, DateTimeOffset.UtcNow);

// 合併批次設備：一次回報目前掛的 5 張工單(無分位)
_localData.SyncWorkOrders("EQ002", fiveWorkOrders.Select(w =>
    (w.WoNo, (string?)null, w.StartTime, (string?)null, (string?)w.MatNo, (string?)w.LineCode, (string?)w.WorkCenterNo)), DateTimeOffset.UtcNow);
```

---

### Alarm

一台設備可同時存在多個獨立 alarm(以 `equipment_no` + `alarm_id` 區分),與工單相同採「現況 + 歷程(含 end_time)」設計,由呼叫端在 alarm 發生與解除時分別呼叫對應的 Function。

#### `RaiseAlarm(equipmentNo, alarmId, alarmText, startTime)`

Alarm 開始(EAP 上報告警發生)。寫入/更新 `equipment_alarm_current`,並在 `equipment_alarm_log` 開立一筆 `end_time = NULL` 的歷程。

- **使用時機**:收到 EAP 的 alarm set 事件時呼叫(例如 SEMI E30 的 ALID 觸發)。
- **alarmId**:EAP 的 Alarm ID/代碼,同一設備、同一 alarmId 視為同一個 alarm。
- **重複呼叫安全**:若該 alarm 已在告警中(例如斷線重連後的狀態重同步、訊息重送)重複呼叫,不會覆蓋原本的 `start_time`、也不會在歷程多開一筆,只會更新 `alarm_text`。

```csharp
_localData.RaiseAlarm("EQ001", "A101", "加熱溫度超上限", DateTimeOffset.UtcNow);
```

#### `ClearAlarm(equipmentNo, alarmId, endTime)`

Alarm 結束(EAP 上報告警解除)。從 `equipment_alarm_current` 移除該筆,並補上 `equipment_alarm_log` 中對應那筆尚未結束紀錄的 `end_time`。

- **使用時機**:收到 EAP 的 alarm clear 事件時呼叫。
- **注意**:`equipmentNo` + `alarmId` 需與對應的 `RaiseAlarm` 一致,才能正確配對到同一筆歷程。

```csharp
_localData.ClearAlarm("EQ001", "A101", DateTimeOffset.UtcNow);
```

#### `SyncAlarms(equipmentNo, syncTime)`

Alarm 重連校正。無條件清空該設備 `equipment_alarm_current` 裡的所有 alarm、把 `equipment_alarm_log` 中所有尚未結束的紀錄補上 `end_time = syncTime`。這裡不做任何比對判斷,單純全部清空。

- **使用時機**:EAP 與設備斷線後重新連線時呼叫,解決斷線期間 Clear 事件漏送、現況表殘留舊資料的問題。
- **呼叫順序是硬性前提**:EAP 必須**先**呼叫本方法清空,**之後**再依向設備重新查詢到的真實現況逐筆呼叫 `RaiseAlarm`,把仍在告警中的 alarm 補回來。順序顛倒(先補 `RaiseAlarm` 才呼叫 `SyncAlarms`)會導致剛補回的真實 alarm 被一併清空,等同誤殺,而且不會有任何錯誤或例外提示——這個順序目前完全靠呼叫端自律,DB 層無法偵測或防止。
- **已知取捨**:對斷線前後其實從未真正解除的 alarm,歷程會在這個流程中被切成兩筆(舊的在 SyncAlarms 結清、EAP 重新 RaiseAlarm 後開新的一筆),而非維持一筆連續紀錄。統計告警次數/持續時間(MTTR)時需注意每次重連都可能對持續中的 alarm 產生一次額外的斷點。
- **`syncTime`**:寫入被結清歷程的 `end_time`,代表「重同步當下」而非真實解除時間(斷線期間實際結束的時間點已不可考)。

```csharp
// EAP 重連時序:先清空,再依真實現況補回
_localData.SyncAlarms("EQ001", DateTimeOffset.UtcNow);

// 接著向設備查詢真實現況,把仍在告警中的逐筆補回
_localData.RaiseAlarm("EQ001", "A101", "加熱溫度超上限", DateTimeOffset.UtcNow);
```

---

### 產出

#### `WriteOutput(equipmentNo, woNo, outputQty, ngQty, recordTime, flowId = null, inputQty = 0)`

寫入產出與不良數。

- **使用時機**：每批次或每週期完成後呼叫一次。
- **flowId**：工單回流到同一台設備時用來區分不同站次；無回流情境可省略。
- **inputQty**：當站投入量（= 工單進站數量），預設 0。

```csharp
_localData.WriteOutput("EQ001", "WO20260630001", outputQty: 100, ngQty: 2, DateTimeOffset.UtcNow);

// 有回流站次 + 投入量時
_localData.WriteOutput("EQ001", "WO20260630001", 100, 2, DateTimeOffset.UtcNow, flowId: "2", inputQty: 102);
```

---

### 片/載具追蹤

`workorder_piece`（片主檔）+ `equipment_workorder_piece_log`（片/載具在設備上的履歷,依日分區）。

> **備註：資料紀錄的粒度分層**。目前資料庫設計的紀錄維度由粗到細大致是「載具 → 工單 → 片」，是逐層往下疊加的選配細粒度追溯，**不是每個產品都會用滿三層**——工單這一層（`equipment_workorder_current/log`、CycleTime、產出、OEE 日彙總等）是所有產品都會有的基本紀錄；載具、片這兩層是否有資料，取決於這個產品/這台設備實際有沒有做到那個粒度的感測（見下方 `UpdateEquipmentTraceCapability` 的三種分類）。**有的產品只做到工單這一層，沒有片/載具明細**，屬於正常情況，查詢/報表邏輯不能假設每張工單底下一定找得到對應的片或載具紀錄。

設備感測能力分三種：只認片、只認載具、兩者都認，**同一台設備可能三種情況都會遇到**，不是固定死的單一模式。因此 `equipment_workorder_piece_log` 不拆成片級表/載具級表兩張，`piece_id`、`carrier_id` 兩欄都允許 NULL（DB 層有 CHECK 限制,至少要有一個非 NULL）,由每一筆事件實際感測到什麼就填什麼欄位。`carrier_id` 全程都是純粹的識別碼文字值，不對應任何主檔表，DLL 也沒有載具主檔或載具↔工單的表。

#### `UpsertPiece(pieceId, woNo, seqNo, status)`

登錄/更新一片的追溯主檔。`woNo` 只在新增時寫入——已確認同一片不會因重工而更換工單號，重複呼叫（例如狀態變化）不會覆蓋原本的 `woNo`，只更新 `status`/`updated_at`。

```csharp
_localData.UpsertPiece("P00012345", "WO20260630001", seqNo: 12, status: "在途");
```

#### `StartEquipmentTraceEvent(equipmentNo, woNo, flowId, pieceId, carrierId, startTime, maxOpenAgeDays = 7)`

片/載具進入這台設備/這個流程站次，開一筆履歷（`end_time = NULL`）。`pieceId`、`carrierId` **至少要傳一個**（省略的那個傳 `null`），否則丟 `ArgumentException`。

- **重複呼叫判斷（2026-09 起的行為）**：若該設備上已有一筆進行中的履歷，且 `pieceId`/`carrierId` **兩邊都完全相同**（缺的一邊視為相同），視為重複呼叫，不會重複開。
- **孤兒紀錄自動結清**：若已有一筆進行中的履歷**只有部分識別碼相同**（例如載具 ID 循環重複使用，但前一片的 `EndEquipmentTraceEvent` 因故沒有正常呼叫而留下孤兒紀錄），會先把那筆孤兒紀錄的 `end_time` 補上（= 本次 `startTime`）結掉，再正常開這一筆新的——修正前的版本會把這種情況誤判為「重複呼叫」而整個跳過寫入，導致新事件完全沒有紀錄，且沒有任何錯誤提示。
- `maxOpenAgeDays`（預設 7）：判斷「進行中」履歷時只往回找最近這幾天內開始的紀錄。`equipment_workorder_piece_log` 是追溯表、分區永久保留不刪除，不設這個下界的話，隨著部署時間拉長，每次找進行中紀錄都要掃過全部歷史分區，效能會逐漸變差。一般不需要調整，除非有片/載具在設備上停留超過 7 天的特殊情境。
- **裝卸片站（兩者都認）的重要提醒**：實務上載具碼和片碼是先後兩次讀到（先掃到載具碼，幾秒後才掃到片碼），不是同一次掃描同時拿到兩個值。**請由 EAP 端自行在記憶體暫存先讀到的那個值，等兩個都拿到後,一次呼叫本方法把兩者一起傳入**，寫成同一列——不要分兩次各傳一個、開成兩筆，否則「片被裝上這個載具」的事實會斷成兩筆對不起來的紀錄。DLL 本身不做這個暫存/合併，是純寫入方法。

```csharp
// 只認片的設備
_localData.StartEquipmentTraceEvent("EQ001", "WO20260630001", "2", pieceId: "P00012345", carrierId: null, DateTimeOffset.UtcNow);

// 只認載具的設備
_localData.StartEquipmentTraceEvent("EQ002", "WO20260630001", null, pieceId: null, carrierId: "CARRIER001", DateTimeOffset.UtcNow);

// 裝卸片站(兩者都認):EAP 已經把先讀到的載具碼跟後讀到的片碼暫存合併好,一次傳入
_localData.StartEquipmentTraceEvent("EQ003", "WO20260630001", "3", pieceId: "P00012345", carrierId: "CARRIER001", DateTimeOffset.UtcNow);
```

#### `EndEquipmentTraceEvent(equipmentNo, pieceId, carrierId, endTime, maxOpenAgeDays = 7)`

片/載具離開這台設備，補上「最新一筆尚未結束、且比對得上 `pieceId` 或 `carrierId`」履歷的 `end_time`。不需要另建 current 表，查詢在途片/載具直接用 `end_time IS NULL`。`pieceId`、`carrierId` 一樣至少要傳一個。`maxOpenAgeDays`（預設 7）理由與預設值同 `StartEquipmentTraceEvent`。

```csharp
_localData.EndEquipmentTraceEvent("EQ001", pieceId: "P00012345", carrierId: null, DateTimeOffset.UtcNow);
```

#### `EnsureWorkorderPieceLogPartitions(lookaheadDays = 2)`

確保 `equipment_workorder_piece_log` 未來分區存在。**不會**刪除舊分區——追溯資料不設保留期。使用時機同 `EnsureParameterLogPartitions`。

#### 反查邏輯（DLL 沒有提供對應查詢方法，由 Dashboard 直接下 SQL）

- **某一片目前在哪個載具 / 某載具目前裝著哪些片**：對 `piece_id IS NOT NULL AND carrier_id IS NOT NULL` 的列，用 `ROW_NUMBER() OVER (PARTITION BY piece_id ORDER BY start_time DESC)` 取每個 `piece_id` 最新一筆，其 `carrier_id` 即為當時綁定的載具。
- **某載具目前在做哪張工單**：用上面查到的「載具目前裝著的片」去 JOIN `workorder_piece.wo_no`。
- **某一片完整跨設備站點歷程**：`SELECT ... WHERE piece_id = :piece_id`，一張表查完。

---

### OEE 日彙總

`equipment_oee_daily_log`：單日 + 設備 + 工單一組合一列，四色秒數 / OEE / TBS 等統計值。

> **2026-08-25 起改由 DB 端排程計算**，不再由呼叫端算好結果後寫入。原本的 `UpsertOeeDaily` 方法已用 `/* ... */` 註解保留在原始碼裡，供未來需要退回應用層計算或人工覆蓋單筆資料時參考，目前不建議呼叫。

#### `CalcOeeDaily(recordDate)`

呼叫 DB 端的 `calc_oee_daily()` function，直接從 `equipment_status_log`（綠/黃/滅燈）、`equipment_alarm_log`（紅燈，優先權最高，會從status_log秒數扣掉重疊時間，避免alarm期間status_code沒同步切換時被重複計算）、`equipment_workorder_log`（拆到 wo_no）、`equipment_output_log`（良率）算出 `recordDate` 這一天每台設備/每張工單的四色秒數、OEE、TBS，`UPSERT` 進 `equipment_oee_daily_log`。

- **使用時機**：由獨立的 `Protocol.LocalData.Scheduler.exe` 搭配 Windows 工作排程器排程呼叫，**EAP 端不需要自己開 Timer**（理由同「Availability」章節）。同一 `(recordDate, equipmentNo, woNo)` 重複呼叫會覆蓋更新（UPSERT），所以可以放心重複算。
- **recordDate** 為 `DateOnly`，以自然日 00:00~24:00 切分；同一工單跨日執行時，兩天各存一列。
- **oee_rate 是簡化版**：`Availability × Quality`（沒有理論 CycleTime 基準，無法算 Performance 節拍因子），之後如果有理論產能資料可以再擴充公式。
- **跨午夜補算**：`recordDate` 是「今天」時，function 內部會把計算上界夾在 `now()`，不會把還沒發生的未來時間當成「維持最後已知狀態」灌進秒數。`Protocol.LocalData.Scheduler.exe` 每次觸發會同時算「今天」與「昨天」，確保昨天的資料在跨過午夜後能被補算完整。
- **同一張工單同時掛在多個槽位時的去重（2026-09 修正）**：一台分槽設備可能同時有多筆進行中工單（見「工單」章節），若同一 `wo_no` 同時出現在兩個槽位（不同 `flow_id`），修正前會把重疊時段的狀態/alarm 秒數重複加總兩次，導致該工單當天的 `green_sec`/`red_sec`/`tbs_min` 超過實際經過的牆鐘時間。現已改為先合併同一 `(equipment_no, wo_no)` 的重疊時段再計算，不會再重複計入。

```csharp
// Protocol.LocalData.Scheduler 內部呼叫方式(僅供參考,現場不需要自己寫這段)
var today = DateOnly.FromDateTime(DateTime.UtcNow);
_localData.CalcOeeDaily(today);
_localData.CalcOeeDaily(today.AddDays(-1));
```

---

### 量測項目 / SPC 規則主檔

`measurement_item_master`、`spc_chart_master`：品質系統自有的主檔資料（非 MES 提供）。

#### `UpsertMeasurementItem(sysId, measurementItem, fieldCode, itemName, unit, isSpc, enable)`

```csharp
_localData.UpsertMeasurementItem("ITEM001", "厚度", "THICK", "Thickness", "mm", isSpc: "Y", enable: "Y");
```

#### `UpsertSpcChart(chartSysId, matNo, machNo, workCenterNo, usl, lsl, ucl, lcl, cl, rUcl, rLcl, sampleQty, chartType, chartSpecType, status)`

```csharp
_localData.UpsertSpcChart(
    "CHART001", "MAT001", "EQ001", "WC001",
    usl: 10m, lsl: 1m, ucl: 9m, lcl: 2m, cl: 5m, rUcl: 1m, rLcl: 0m,
    sampleQty: 5, chartType: 1, chartSpecType: 1, status: "ACTIVE");
```

---

### SPC 量測明細

`equipment_spc_measurement_log`：量大，依 `record_time` 每日分區。

#### `WriteSpcMeasurement(edcRawdataSysId, woNo, flowId, pieceId, equipmentNo, itemSysId, chartSysId, specMax, specMin, dataValues, recordTime)`

`dataValues` 依序對應 `data_01`~`data_50`，筆數不足 50 的部分自動補 `NULL`，超過 50 筆會丟 `ArgumentException`；傳入 `null` 會丟 `ArgumentNullException`（2026-09 起明確驗證，修正前會丟出不易理解的 `NullReferenceException`）。

```csharp
var data = new decimal?[] { 5.01m, 5.02m, 4.99m }; // 只量測 3 點,其餘 47 欄自動補 NULL
_localData.WriteSpcMeasurement(
    "EDC0001", "WO20260630001", "2", "P00012345", "EQ001", "ITEM001", "CHART001",
    specMax: 10m, specMin: 1m, dataValues: data, recordTime: DateTimeOffset.UtcNow);
```

#### `EnsureSpcMeasurementLogPartitions(lookaheadDays = 2)`

確保 `equipment_spc_measurement_log` 未來分區存在。**不會**刪除舊分區——追溯資料不設保留期。使用時機同 `EnsureParameterLogPartitions`。

---

### MES 代碼快取

`mes_code_cache`：MES 沒有提供「列出全部產線/產品別/工作站」的清單查詢 API，Dashboard 平常查工單/機台主檔拿到代碼+名稱時，順便寫入這張表，累積成篩選下拉選單的清單來源。

#### `UpsertMesCode(codeType, code, name)`

- **codeType**：`LINE`(產線) / `MAT_GROUP`(產品別) / `WORK_CENTER`(工作站)。

```csharp
_localData.UpsertMesCode("LINE", "L01", "產線一");
```

---

## 外部排程程式 Protocol.LocalData.Scheduler.exe

`Protocol.LocalData.Scheduler` 是隨這支 DLL 一起發布的獨立排程程式，**不是常駐服務、內部不開自己的 Timer**——每次被觸發就依序把以下步驟各跑一輪就結束，重複觸發的時機完全交給 **Windows 工作排程器（Task Scheduler）** 決定：

1. `EnsureParameterLogPartitions` / `EnsureWorkorderPieceLogPartitions` / `EnsureSpcMeasurementLogPartitions`（三張分區表的分區維護，2026-09 起併入這支排程，理由見下方提醒）
2. `CalcAvailabilityPeriod`（算剛過去 1 小時的可動率）
3. `CalcOeeDaily`（算今天 + 昨天的 OEE 日彙總）

> **為什麼分區維護也併進 Scheduler.exe，而不是只交給 EAP 自己的 Timer？** 理由跟 Availability/OEE 當初移出 EAP 一樣：EAP 啟動流程建議的分區 Timer（見文件前面「EAP 啟動流程」章節）綁在 EAP process 的生死上，EAP 當掉/重啟超過 lookahead 天數（預設 2 天）就會悄悄斷檔，之後對應表的寫入會直接失敗，且不會有任何提示。Scheduler.exe 由 Windows 工作排程器獨立觸發，不受 EAP 存活狀態影響，作為更可靠的保險；**EAP 端原本的分區 Timer 仍可以保留**，兩邊都呼叫是安全的（底層皆為 `IF NOT EXISTS`，重複呼叫不會出錯，也不會建立重複分區）。

發布內容在 `publish_scheduler/` 資料夾：

```
publish_scheduler/
 ├─ Protocol.LocalData.Scheduler.exe   ← 排程本體
 ├─ install_scheduler_task.bat         ← 一鍵安裝(以系統管理員身分執行)
 ├─ uninstall_scheduler_task.bat       ← 一鍵移除
 └─ scheduler.log                      ← 每次執行的結果(執行後自動產生)
```

### 打包成單一 .exe

`Protocol.LocalData.Scheduler.csproj` 已設定 `SelfContained=true` + `PublishSingleFile=true`（連 .NET 8 執行環境本身都包進去），所以只要在專案根目錄執行：

```
dotnet publish Protocol.LocalData.Scheduler/Protocol.LocalData.Scheduler.csproj -c Release -o publish_scheduler
```

輸出到 `publish_scheduler/` 的就是**一支獨立的 `Protocol.LocalData.Scheduler.exe`**（約 65~70 MB，因為內嵌了整個 .NET Runtime），旁邊不會再有 `Npgsql.dll`、`Protocol.LocalData.dll`、`*.deps.json`、`*.runtimeconfig.json` 這些散落檔案——**目標機器不需要另外安裝 .NET 8 執行環境**，複製這一支 exe（+`install_scheduler_task.bat`/`uninstall_scheduler_task.bat`）過去就能跑。發布動作只會複製出以下四個檔案（`scheduler.log` 是執行後才產生）：

```
publish_scheduler/
 ├─ Protocol.LocalData.Scheduler.exe
 ├─ install_scheduler_task.bat
 ├─ uninstall_scheduler_task.bat
 └─ scheduler.log            ← 執行後自動產生,一開始沒有
```

> 這台 Scheduler 專案引用了 `Protocol.LocalData.csproj` 裡標記 `CopyToOutputDirectory` 的 Migrations SQL 檔案,`dotnet publish` 有時仍會多帶出一個 `Migrations/` 資料夾——Scheduler.exe 執行時不會讀取它,純屬建置系統的附帶產物,可以直接刪掉不影響運作,不刪也無妨。

### 安裝步驟

1. 打開 `install_scheduler_task.bat`，把 `PGCONNSTRING` 改成正確的連線字串。
2. 以**系統管理員身分**執行這支 `.bat`（要用 SYSTEM 帳號建立排程，需要管理員權限）。
3. 它會建立一個每小時觸發一次的工作排程，並立即手動觸發一次驗證連線——完成後打開 `scheduler.log` 確認最後幾行是 `OK` 不是 `ERROR`。
4. 要移除排程，執行同資料夾的 `uninstall_scheduler_task.bat`。

### 這支程式該放在哪裡？放置位置會不會影響排程執行？

**會影響，而且是實務上最容易踩到的坑，建議放在一個固定不變、跟 EAP 程式安裝路徑分開的目錄**，例如：

- `C:\ProgramData\Protocol\Scheduler\`（建議：機器層級的固定路徑，一般使用者帳號也有寫入權限，方便手動測試執行）
- 或 `C:\Protocol\Scheduler\`

放置位置具體會影響排程執行的原因有三個：

1. **路徑在安裝當下就寫死進工作排程器了。** `install_scheduler_task.bat` 用 `%~dp0` 組出 exe 的絕對路徑，寫進 `schtasks /create ... /tr "<絕對路徑>"`。這個絕對路徑會原封不動存進工作排程器的動作定義裡——**事後把資料夾搬到別的地方，工作排程器還是會去舊路徑找 exe，執行會直接失敗**（Task Scheduler 的執行歷史會顯示找不到檔案/路徑錯誤）。搬家後必須用系統管理員身分重新執行一次 `install_scheduler_task.bat`（`/f` 參數會覆蓋掉舊的工作定義，不用先手動刪除）。
2. **排程是用 SYSTEM 帳號執行的**（`install_scheduler_task.bat` 裡的 `/ru SYSTEM`）。SYSTEM 帳號沒有一般使用者「已對應的磁碟機代號」（mapped network drive），所以**不能放在網路磁碟機（例如 Z:\）**——一定要放在本機實體磁碟路徑，若真的要放遠端，得用完整 UNC 路徑（`\\server\share\...`）且該機器的電腦帳號對那個共用有存取權限。
3. **不要放在 EAP 程式自己的安裝資料夾裡。** 排程程式若跟 EAP 裝在同一個目錄下，日後 EAP 改版重新安裝/解除安裝時，很容易連同這個資料夾一起被清掉或覆蓋，排程就會悄悄失效而不會有任何提示——建議獨立成單獨資料夾，跟 EAP 的部署生命週期脫鉤，EAP 重裝/升級不會影響到它。

不受放置位置影響的部分：`scheduler.log` 一律寫在 exe 自己所在的資料夾（程式內用 `AppContext.BaseDirectory` 取路徑），跟工作排程器設定的「啟動位置」（Start in）無關，所以不需要額外設定工作目錄。

> **安全性提醒**：連線字串（含資料庫密碼）是以明碼參數存在工作排程器的動作定義裡，任何能開啟「工作排程器」介面或執行 `schtasks /query /tn Protocol_OEE_Availability_Scheduler /v /fo list` 的使用者都看得到明碼密碼。建議這台機器上該資料庫帳號的權限僅限本機存取、且權限最小化（2026-09 起 Scheduler.exe 除了 `calc_availability_period()` / `calc_oee_daily()`，也會呼叫三張分區表的 `*_manage_partitions()` 維護函式，該帳號需要能執行這五個 function、並寫入/建立對應的表與分區）。

---

## 注意事項總覽

| 項目 | 說明 |
|---|---|
| **單一實例** | `LocalDataService` 整個 App 只能 `new` 一次，長期保存使用 |
| **時間使用 UTC** | 所有時間參數傳入 `DateTimeOffset.UtcNow`，前端顯示時自行轉換本地時區 |
| **EnsureParameterLogPartitions 每日必呼叫** | 建議 EAP 啟動時 + 每日 Timer，超過 2 天未呼叫機台參數寫入會失敗 |
| **WriteEquipmentParameter threshold** | 每種參數類型設定適合的容許誤差，避免頻繁寫入 DB |
| **SyncEquipments 會刪除清單外設備** | EAP 啟動傳入的清單即為最終清單，不在清單內的設備會從 `equipment_registry` 移除 |
| **Availability / OEE 不用 EAP 排程** | 改由獨立的 `Protocol.LocalData.Scheduler.exe` + Windows 工作排程器觸發，EAP 不用自己開 Timer |
| **分區維護也由 Scheduler.exe 負責一份** | 2026-09 起 Scheduler.exe 也會呼叫三張分區表的維護函式，不完全依賴 EAP 自己的 Timer；EAP 端原本的分區 Timer 仍建議保留，兩邊都呼叫是安全的 |
| **Scheduler.exe 路徑固定後不要搬家** | 路徑在安裝時已寫死進工作排程器，搬移資料夾必須重新執行 `install_scheduler_task.bat` |
| **UpdateEquipmentMesInfo / UpdateEquipmentTraceCapability 會拋例外** | 設備尚未透過 `RegisterEquipment`/`SyncEquipments` 登錄就呼叫，會拋 `InvalidOperationException`，不會再無聲無息地寫入失敗 |
| **StartEquipmentTraceEvent 的孤兒紀錄處理** | 載具 ID 重複使用、前一筆漏呼叫 `EndEquipmentTraceEvent` 時，會自動結清孤兒紀錄再開新的，不會整筆跳過不寫 |

---

┌─────────────────────────────┬────────────────────────┐
│             表              │          說明           │
├─────────────────────────────┼────────────────────────┤
│ equipment_registry          │ 設備清單                │
├─────────────────────────────┼────────────────────────┤
│ equipment_status_current    │ 設備狀態現況             │
├─────────────────────────────┼────────────────────────┤
│ equipment_status_log        │ 設備狀態歷史             │
├─────────────────────────────┼────────────────────────┤
│ equipment_parameter_current │ 機台參數現況             │
├─────────────────────────────┼────────────────────────┤
│ equipment_parameter_log     │ 機台參數歷史（分區表）     │
├─────────────────────────────┼────────────────────────┤
│ equipment_cycle_time_log    │ CycleTime              │
├─────────────────────────────┼────────────────────────┤
│ equipment_availability_log  │ Availability           │
├─────────────────────────────┼────────────────────────┤
│ equipment_workorder_current │ 工單現況                 │
├─────────────────────────────┼────────────────────────┤
│ equipment_workorder_log     │ 工單歷史                │
├─────────────────────────────┼────────────────────────┤
│ equipment_alarm_current     │ Alarm 現況(告警中)      │
├─────────────────────────────┼────────────────────────┤
│ equipment_alarm_log         │ Alarm 歷史(含起止時間)  │
├─────────────────────────────┼────────────────────────┤
│ equipment_output_log        │ 產出                    │
├─────────────────────────────┼────────────────────────┤
│ status_light_map            │ 狀態燈號對照(種子資料)   │
├─────────────────────────────┼────────────────────────┤
│ workorder_piece             │ 片級追溯主檔             │
├─────────────────────────────┼────────────────────────┤
│ equipment_workorder_piece_log│ 片在設備上的履歷(分區表)│
├─────────────────────────────┼────────────────────────┤
│ equipment_oee_daily_log     │ 單日 OEE / 四色秒數彙總  │
├─────────────────────────────┼────────────────────────┤
│ measurement_item_master     │ 量測項目主檔             │
├─────────────────────────────┼────────────────────────┤
│ spc_chart_master            │ SPC 規則主檔             │
├─────────────────────────────┼────────────────────────┤
│ equipment_spc_measurement_log│ SPC 量測明細(分區表)    │
├─────────────────────────────┼────────────────────────┤
│ mes_code_cache              │ MES 代碼快取             │
└─────────────────────────────┴────────────────────────┘

