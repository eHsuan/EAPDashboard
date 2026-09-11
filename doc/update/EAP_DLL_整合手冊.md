# EAP DLL 整合手冊

給呼叫 `Protocol.LocalData.dll` 的 EAP 工程師使用。

## 定位

`Protocol.LocalData.dll` 是 EAP（設備自動化程式）唯一的資料庫寫入通道，負責把設備狀態、機台參數、工單、Alarm、產出、片/載具追蹤等即時生產資料寫進 PostgreSQL，供後端 Dashboard 純讀取。

> **架構原則**：所有資料一律由 EAP 主動寫入——包含向 MES 查來的機台主檔、工單情境資訊。Dashboard 只會對這個資料庫下 `SELECT`，不會、也不能直接呼叫 MES 或任何外部 API。

| 項目 | 說明 |
|---|---|
| 執行環境 | .NET 8.0 以上 |
| 資料庫 | PostgreSQL |
| 依賴套件 | Npgsql 10.0.3（已內含於 DLL，無需額外安裝） |
| 資料表數 | 21 張（migrations 001–018） |

---

## ⚠ 兩條硬性規則

**1. 單一實例**：`LocalDataService` 必須以單一長期存活的實例使用，整個 App 生命週期只 `new` 一次。機台參數的「異動才寫」比對是靠記憶體快取（`ConcurrentDictionary`）做的，每次都 `new` 新實例會讓快取失效，變成每筆都寫入 DB。

**2. 時間一律 UTC**：所有時間參數傳入 `DateTimeOffset.UtcNow`，不可用 `DateTimeOffset.Now`（含 +08:00 本地時區會造成寫入錯誤或資料不一致）。前端顯示時再自行轉換本地時區。

---

## EAP 啟動流程

1. 建立唯一的 `LocalDataService` 實例（連線字串由呼叫端提供）。
2. 呼叫 `SyncEquipments` 同步這台 EAP 管轄的設備清單。
3. 呼叫三個 `Ensure*Partitions` 確保分區表未來分區存在，並掛上每日 Timer 持續呼叫。
4. （選用）呼叫 `UpdateEquipmentMesInfo` / `UpdateEquipmentTraceCapability` 寫入向 MES 查到的機台主檔資訊。
5. 進入主迴圈：狀態、參數、CycleTime、工單、Alarm、產出等即時事件發生時逐一呼叫對應方法。

```csharp
// 唯一實例，整個 App 共用
private static readonly LocalDataService _localData = new LocalDataService(
    "Host=localhost;Port=5432;Database=Protocol_localdata;Username=postgres;Password=xxxxxx"
);

_localData.SyncEquipments(new[] { ("EQ001", (string?)"射出機 A") });

_localData.EnsureParameterLogPartitions();
_localData.EnsureWorkorderPieceLogPartitions();
_localData.EnsureSpcMeasurementLogPartitions();

// 每日 Timer，持續維護分區
var t = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
t.Elapsed += (_, _) => {
    _localData.EnsureParameterLogPartitions();
    _localData.EnsureWorkorderPieceLogPartitions();
    _localData.EnsureSpcMeasurementLogPartitions();
};
t.AutoReset = true; t.Start();
```

---

## 00 · 分區維護

三張高頻寫入表依日期分區，這三個方法只負責「確保分區存在」，不寫業務資料。

| 方法 | 對應表 | 是否清除舊分區 |
|---|---|---|
| `EnsureParameterLogPartitions(retentionDays=14, lookaheadDays=2)` | equipment_parameter_log | **會**，操作型資料只留 2 週 |
| `EnsureWorkorderPieceLogPartitions(lookaheadDays=2)` | equipment_workorder_piece_log | 不會，追溯資料 |
| `EnsureSpcMeasurementLogPartitions(lookaheadDays=2)` | equipment_spc_measurement_log | 不會，追溯資料 |

> 超過 `lookaheadDays` 未呼叫，對應表的寫入會直接報錯。

---

## 01 · 設備清單

對應 `equipment_registry`。

### `SyncEquipments(equipments)`
同步設備清單：UPSERT 清單內設備，**自動刪除**不在清單內的設備。EAP 啟動時呼叫，傳入完整管轄清單。

```csharp
_localData.SyncEquipments(new[] {
    ("EQ001", (string?)"射出機 A"),
    ("EQ002", (string?)"射出機 B"),
});
```

### `RegisterEquipments(equipments)` / `RegisterEquipment(equipmentNo, equipmentName)`
批次／單筆登錄，只新增或更新，**不刪除**其他設備。只想新增設備、不想影響現有清單時使用。

### `UnregisterEquipment(equipmentNo)`
手動下架單台設備（臨時調離此 EAP 管轄時）。

### `UpdateEquipmentMesInfo(equipmentNo, lineCode, enable, machAlias, groupName)`
寫入向 MES 機台主檔查詢 API 取得的產線代碼／啟用狀態／別名／群組。異動頻率低，直接覆蓋、不留歷史。需先透過 `SyncEquipments` 建過這筆設備，本方法只 `UPDATE`。**若該設備尚未登錄會拋出 `InvalidOperationException`**（2026-09 起的行為），呼叫端請確保啟動流程一定先呼叫過 `SyncEquipments`/`RegisterEquipment`。

```csharp
_localData.UpdateEquipmentMesInfo("EQ001", lineCode: "L01", enable: "Y",
    machAlias: "射出機A-1號機", groupName: "射出成型");
```

### `UpdateEquipmentTraceCapability(equipmentNo, traceCapability)`
寫入這台設備**一般**具備的感測能力：`"PIECE_ONLY"` / `"CARRIER_ONLY"` / `"BOTH"`。只是靜態分類參考，不是每筆事件的強制規則——第 09 節事件實際要不要填 piece/carrier 以該次事件實際感測到什麼為準。同樣需先透過 `SyncEquipments` 建過這筆設備，**若尚未登錄會拋出 `InvalidOperationException`**。

```csharp
_localData.UpdateEquipmentTraceCapability("EQ001", "BOTH");
```

---

## 02 · 設備狀態

對應 `equipment_status_current/log`。`status_code` 是 `-1`、`0`~`12` 共 14 種數值代碼（對照表在 `status_light_map`，種子資料，DLL 不寫）。

### `WriteEquipmentStatus(equipmentNo, statusCode, statusDesc, eventTime)`
寫入設備狀態，同時更新現況快照與歷史紀錄。設備離線時，EAP 偵測斷線/逾時應主動寫入 `status_code = "-1"`。

- **statusDesc（選填）**：`status_code` 的通用說明已經有 `status_light_map` 可查，**不需要每次都重複填一樣的固定文字**（例如 status_code="0" 就填"自動運行"）——這種情況直接傳 `null`，查詢端 JOIN `status_light_map` 取得即可。只有這次事件有「比通用說明更具體」的情境資訊時才填（例如是哪個感測器斷線、哪個原因觸發這次黃燈）。

```csharp
// 一般情況：通用說明交給 status_light_map，不重複填
_localData.WriteEquipmentStatus("EQ001", "0", null, DateTimeOffset.UtcNow);

// 有比通用說明更具體的情境時才填
_localData.WriteEquipmentStatus("EQ001", "1", "TEMP1 感測器讀值超過上限 15°C", DateTimeOffset.UtcNow);
```

---

## 03 · 機台參數

對應 `equipment_parameter_current/log`（EAV 設計）。`equipment_parameter_log` 依日分區、保留 2 週、**異動才寫**——每個參數都套用記憶體快取比對，變化量未超過 threshold 且告警狀態未變時直接跳過。

### `WriteEquipmentParameters(equipmentNo, parameters)` ← 建議優先使用
批量寫入，整批只開一次連線／一個 Transaction。每個 scan cycle 收完整台設備所有參數後一次呼叫；10 台 × 20 參數，連線數從 200 降到 10。

```csharp
var now = DateTimeOffset.UtcNow;
var parameters = new[] {
    new ParameterData("TEMP1", "加熱溫度", 185.3m, 200m, 150m, false, now, threshold: 0.5m, paramUnit: "°C"),
};
_localData.WriteEquipmentParameters("EQ001", parameters);
```

### `WriteEquipmentParameter(equipmentNo, paramCode, paramName, actualValue, upperLimit, lowerLimit, isAlarm, recordTime, threshold=0, paramUnit=null)`
單筆寫入，每次各自開一個連線。`isAlarm` 由呼叫端計算，告警狀態改變一定會寫入，不受 threshold 限制。

---

## 04 · CycleTime

對應 `equipment_cycle_time_log`。

### `WriteCycleTime(equipmentNo, woNo, cycleTimeSec, recordTime, flowId=null, pieceId=null)`
寫入單筆生產週期時間。`flowId` 用於工單回流站次；`pieceId` 對應片級追溯（邏輯 FK → workorder_piece，DB 層未建實體約束）。兩者無對應情境可省略。

```csharp
_localData.WriteCycleTime("EQ001", "WO20260630001", 12.5m, DateTimeOffset.UtcNow,
    flowId: "2", pieceId: "P00012345");
```

---

## 05 · Availability

對應 `equipment_availability_log`（任意區間二態統計，跟第 10 節 OEE 日彙總不同用途、互不取代）。

> **2026-08-25 起改由 DB 端排程計算**，不再由 EAP 算好結果後推送。原本的 `WriteAvailability` 方法已註解保留在原始碼裡（沒刪除，供未來需要退回應用層計算或人工覆蓋單筆資料時參考），目前不建議呼叫。

### `CalcAvailabilityPeriod(periodStart, periodEnd)`
呼叫 DB 端 `calc_availability_period()`，直接從 `equipment_status_log` 算 `[periodStart, periodEnd)` 這段區間每台設備的可動率（綠燈=運轉中，其餘=停機），寫入一筆新的 `equipment_availability_log`。**用 .NET Timer 排程呼叫**，不依賴 pg_cron（地端環境不保證裝得到這個擴充套件）。

```csharp
// 建議:每小時呼叫一次,算剛結束的這一小時
var t = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
t.Elapsed += (_, _) => {
    var now = DateTimeOffset.UtcNow;
    _localData.CalcAvailabilityPeriod(now.AddHours(-1), now);
};
t.AutoReset = true; t.Start();
```

> **2026-08-26 起，正式環境建議改用 `Protocol.LocalData.Scheduler` 這支獨立排程程式，不要把 Timer 寫進 EAP 裡。**
> 理由：Timer 埋在 EAP 程式碼裡，排程會跟著 EAP process 的生死走（EAP 當掉/重啟時排程也停了，而且不會有任何提示），而且要求每個接 DLL 的工程師自己把 Timer 程式碼抄對、抄好，容易漏寫或寫錯。改用獨立的 `Protocol.LocalData.Scheduler.exe`（跟這支 DLL 一起發布在 `publish_scheduler/` 資料夾），搭配 Windows 工作排程器（Task Scheduler）每小時觸發一次，好處是：
> - 排程本身是作業系統層級的，跟 EAP 是否存活無關；
> - Windows 工作排程器自帶執行歷史（成功/失敗、上次執行時間），不用自己刻監控；
> - 每次執行都會把結果寫進同資料夾的 `scheduler.log`。
>
> **打包成單一 .exe**：`Protocol.LocalData.Scheduler.csproj` 已設定 `SelfContained=true` + `PublishSingleFile=true`，執行 `dotnet publish Protocol.LocalData.Scheduler/Protocol.LocalData.Scheduler.csproj -c Release -o publish_scheduler` 產出的就是一支約 65~70 MB、內嵌整個 .NET Runtime 的獨立 `Protocol.LocalData.Scheduler.exe`，旁邊不會有 `Npgsql.dll`/`*.deps.json` 這些散落檔案，**目標機器不需要另外裝 .NET 8**。
>
> **現場工程師安裝步驟**：拿到 `publish_scheduler/` 整個資料夾後，打開 `install_scheduler_task.bat` 編輯 `PGCONNSTRING` 那一行改成正確的連線資訊，存檔後**以系統管理員身分**執行這支 `.bat`（因為要用 SYSTEM 帳號建立排程，需要管理員權限）。它會自動建立一個每小時觸發一次的工作排程，並立刻手動觸發一次驗證連線是否正常——跑完打開 `scheduler.log` 確認最後幾行是 `OK` 不是 `ERROR` 就完成了。要移除排程用同資料夾的 `uninstall_scheduler_task.bat`。
>
> `CalcOeeDaily`（見第 10 節）也是同一支 `Scheduler.exe` 一起觸發，不需要另外裝一份。
>
> **2026-09 起，`EnsureParameterLogPartitions` / `EnsureWorkorderPieceLogPartitions` / `EnsureSpcMeasurementLogPartitions` 三張分區表的分區維護也併進 `Scheduler.exe` 一起觸發**，理由跟上面 Availability/OEE 移出 EAP 一樣：第 02 節建議 EAP 自己開的分區 Timer 一樣綁在 EAP process 的生死上，EAP 當掉/重啟超過 lookahead 天數（預設 2 天）就會悄悄斷檔，之後對應表的寫入會直接失敗且沒有提示。`Scheduler.exe` 由 Windows 工作排程器獨立觸發，作為更可靠的一份保險；**第 02 節 EAP 自己的分區 Timer 仍建議保留**，兩邊都呼叫是安全的（底層皆為 `IF NOT EXISTS`，重複呼叫不會出錯也不會重複建分區）。
>
> **這支 `publish_scheduler/` 資料夾要放在哪裡？**建議放在固定不變、跟 EAP 安裝路徑分開的目錄（例如 `C:\ProgramData\Protocol\Scheduler\`），理由：
> 1. 路徑在 `install_scheduler_task.bat` 執行當下就用 `%~dp0` 寫死進工作排程器的動作定義裡——**事後搬家，工作排程器還是會去舊路徑找 exe，執行會直接失敗**，搬家後必須用系統管理員身分重新執行一次 `install_scheduler_task.bat`（`/f` 會覆蓋舊定義）。
> 2. 排程用 **SYSTEM 帳號**執行（`/ru SYSTEM`），SYSTEM 沒有一般使用者的「已對應磁碟機代號」，**不能放網路磁碟機（Z:\ 之類）**，只能放本機路徑或有正確權限的 UNC 路徑。
> 3. **不要跟 EAP 裝在同一個資料夾**，否則 EAP 改版重裝/解除安裝時容易被一併清掉，排程會悄悄失效且沒有任何提示。
>
> `scheduler.log` 一律寫在 exe 自己所在資料夾（`AppContext.BaseDirectory`），跟放哪裡、工作排程器的「Start in」設定無關。另外連線字串（含密碼）是明碼存在工作排程器的動作定義裡，該資料庫帳號權限建議最小化（需要能執行 `calc_availability_period()` / `calc_oee_daily()` 與三張分區表的 `*_manage_partitions()` 共五個 function，並寫入/建立對應的表與分區）。

---

## 06 · 工單

對應 `equipment_workorder_current/log`。一台設備可能同時對應多張工單（分槽設備／合併批次設備），主鍵是 `(equipment_no, wo_no, flow_id)`——工單可能回流到同一台設備，同一 `woNo` 在不同流程站次視為不同進行中紀錄。

### `StartWorkOrder(equipmentNo, woNo, slotNo, startTime, flowId=null, matNo=null, lineCode=null, workCenterNo=null)`
開始一張工單。

- **slotNo**：分槽設備才傳，DLL 會先關閉同槽位上「別的工單」（不同 wo_no，或同 wo_no 不同 flowId）；傳 `null`（合併批次設備）則不影響其他工單。
- **matNo / lineCode / workCenterNo**：這張工單當下的產品料號／產線／工作站，由 EAP 呼叫 MES 依 woNo 查詢後傳入——DLL 只負責寫入，不會替你查 MES。
- 重複呼叫安全：同一 (woNo, flowId) 已存在且槽位不變時不會覆蓋 start_time、不會重複開歷程。

```csharp
// 分槽設備：槽位 3 換上新工單，原本槽位 3 的舊工單自動結清
_localData.StartWorkOrder("EQ001", "WO20260630002", "3", DateTimeOffset.UtcNow);

// 工單回流同一台設備的第二個站次（同產品/同產線，只是工作站換了）
_localData.StartWorkOrder("EQ001", "WO20260630002", "5", DateTimeOffset.UtcNow, flowId: "2",
    matNo: "MAT001", lineCode: "L01", workCenterNo: "WC02");
```

### `EndWorkOrder(equipmentNo, woNo, endTime, flowId=null)`
明確結束「這一張」工單（以 woNo + flowId 識別），不影響同設備上其他進行中工單。沒有分位的設備要換單，必須先呼叫本方法，再呼叫 `StartWorkOrder`——兩者不會自動互相取代。

### `SyncWorkOrders(equipmentNo, workOrders, asOfTime)`
整批同步一台設備目前所有進行中工單：UPSERT 清單內、結束「不在清單內」的舊工單。`workOrders` 的 tuple 是 `(WoNo, SlotNo, StartTime, FlowId, MatNo, LineCode, WorkCenterNo)`；是否「還在清單內」以 `(WoNo, FlowId)` 組合判斷。清單傳空陣列＝這台設備現在沒有任何進行中工單。

---

## 07 · 設備 Alarm

對應 `equipment_alarm_current/log`。一台設備可同時存在多個獨立 alarm，以 `(equipment_no, alarm_id)` 為鍵。安全停機、品質停機也會寫入這張表，不是只有報警。

### `RaiseAlarm(equipmentNo, alarmId, alarmText, startTime)`
Alarm 開始。重複呼叫安全：若該 alarm 已在告警中，不會覆蓋 start_time、不會重複開歷程，只更新 alarm_text。收到 EAP 的 alarm set 事件時呼叫（例如 SEMI E30 的 ALID 觸發）。

### `ClearAlarm(equipmentNo, alarmId, endTime)`
Alarm 結束，從現況移除、補上歷程 end_time。

### `SyncAlarms(equipmentNo, syncTime)`
Alarm 重連校正：無條件清空現況、結清所有未結束歷程（end_time = syncTime，代表重同步當下而非真實解除時間）。

> **呼叫順序是硬性前提**：必須先呼叫 `SyncAlarms` 清空，「之後」再依真實現況逐筆呼叫 `RaiseAlarm` 補回。順序顛倒會把剛補回的真實 alarm 一併清空，且不會有任何錯誤提示。

---

## 08 · 產出

對應 `equipment_output_log`。產出不拆到片級，維持「數量彙總」設計。

### `WriteOutput(equipmentNo, woNo, outputQty, ngQty, recordTime, flowId=null, inputQty=0)`
寫入產出與不良數。`inputQty`＝當站投入量（與「工單進站數量」同一概念），預設 0。每批次或每週期完成後呼叫。

---

## 09 · 片／載具追蹤

對應 `workorder_piece`（片主檔）＋ `equipment_workorder_piece_log`（片/載具在設備上的履歷，依日分區）。

> **備註：資料紀錄的粒度分層**。目前資料庫設計的紀錄維度由粗到細大致是「載具 → 工單 → 片」，是逐層往下疊加的選配細粒度追溯，**不是每個產品都會用滿三層**——工單這一層（第 06 節工單現況/歷程、CycleTime、產出、第 10 節 OEE 日彙總等）是所有產品都會有的基本紀錄；載具、片這兩層有沒有資料，取決於這個產品/這台設備實際做到哪個粒度的感測（見 01 節 `UpdateEquipmentTraceCapability` 的三種分類）。**有的產品只做到工單這一層，沒有片/載具明細**，屬於正常情況，Dashboard 查詢/報表邏輯不能假設每張工單底下一定找得到對應的片或載具紀錄。

設備感測能力分三種——只認片、只認載具、兩者都認，**同一台設備可能三種情況都會遇到**。`piece_id`／`carrier_id` 兩欄都允許 NULL（DB 層 CHECK 至少一個非 NULL），依事件實際感測到什麼填什麼。`carrier_id` 全程是純識別碼文字值，不對應任何主檔表。

### `UpsertPiece(pieceId, woNo, seqNo, status)`
登錄/更新一片的追溯主檔。`woNo` 只在新增時寫入——同一片不會因重工換工單號，重複呼叫只更新 status。

### `StartEquipmentTraceEvent(equipmentNo, woNo, flowId, pieceId, carrierId, startTime, maxOpenAgeDays = 7)`
片/載具進入設備，開一筆履歷。`pieceId`、`carrierId` **至少要傳一個**，否則丟 `ArgumentException`。

> **重複呼叫 vs. 孤兒紀錄（2026-09 起）**：`pieceId`/`carrierId` 兩邊都完全相同（缺的一邊視為相同）才視為重複呼叫、不重複開。若進行中的舊履歷**只有部分識別碼相同**（例如載具 ID 循環重複使用、但前一片漏呼叫 `EndEquipmentTraceEvent`），會先自動把舊履歷結清（`end_time` = 本次 `startTime`）再開新的一筆——修正前的版本會把這種情況誤判成重複呼叫而整個跳過寫入，新事件完全沒有紀錄。`maxOpenAgeDays`（預設 7）限制往回找「進行中」履歷的天數，避免追溯表分區永久保留、隨部署時間拉長越查越慢。

> **裝卸片站（兩者都認）的關鍵提醒**：實務上載具碼與片碼是**先後兩次**讀到（先掃到載具碼，幾秒後才掃到片碼），不是同一次掃描同時拿到。**請由 EAP 端自行在記憶體暫存先讀到的值，等兩個都拿到後一次呼叫本方法把兩者一起傳入**，寫成同一列——不要分兩次各傳一個、開成兩筆履歷。DLL 本身不做暫存/合併，是純寫入方法。

```csharp
// 只認片的設備
_localData.StartEquipmentTraceEvent("EQ001", "WO20260630001", "2", pieceId: "P00012345", carrierId: null, DateTimeOffset.UtcNow);

// 裝卸片站：EAP 已把先讀到的載具碼跟後讀到的片碼合併好，一次傳入
_localData.StartEquipmentTraceEvent("EQ003", "WO20260630001", "3", pieceId: "P00012345", carrierId: "CARRIER001", DateTimeOffset.UtcNow);
```

### `EndEquipmentTraceEvent(equipmentNo, pieceId, carrierId, endTime, maxOpenAgeDays = 7)`
片/載具離開設備，補上「最新一筆尚未結束、且比對得上 pieceId 或 carrierId」履歷的 end_time。不需要另建 current 表，在途查詢直接用 `end_time IS NULL`。`maxOpenAgeDays` 理由同 `StartEquipmentTraceEvent`。

---

## 10 · OEE 日彙總

對應 `equipment_oee_daily_log`：單日 + 設備 + 工單一組合一列。與 Availability（任意區間二態）不同，這張是「單日 + 四色秒數 + OEE/TBS」彙總。

> **2026-08-25 起改由 DB 端排程計算**，不再由 EAP 算好結果後推送。原本的 `UpsertOeeDaily` 方法已註解保留在原始碼裡，目前不建議呼叫。

### `CalcOeeDaily(recordDate)`
呼叫 DB 端 `calc_oee_daily()`，直接從 `equipment_status_log`（四色中的綠/黃/滅燈）、`equipment_alarm_log`（紅燈，優先權最高，會從status_log秒數扣掉重疊時間，避免alarm期間status_code沒同步切換時被重複計算）、`equipment_workorder_log`（拆到 wo_no）、`equipment_output_log`（良率）算出 `recordDate` 這一天每台設備/每張工單的四色秒數、OEE、TBS，`UPSERT` 進 `equipment_oee_daily_log`。**用 .NET Timer 排程呼叫**。

- **oee_rate 是簡化版**：`Availability × Quality`（沒有理論 CycleTime 基準，無法算 Performance 節拍因子）。之後如果有理論產能資料可以再擴充。
- **跨午夜**：`recordDate` 是「今天」時，function 內部會把計算上界夾在 `now()`（不會把還沒發生的未來時間當成「維持最後已知狀態」灌進秒數）。建議排程同時算「今天」與「昨天」，確保昨天的資料在跨過午夜後能被補算完整。
- **同一張工單同時掛在多個槽位時的去重（2026-09 修正）**：一台分槽設備可能同時有多筆進行中工單（見第 06 節），若同一 `wo_no` 同時出現在兩個槽位（不同 `flow_id`），修正前 `calc_oee_daily()` 會把重疊時段的狀態/alarm 秒數重複加總兩次，導致該工單當天的 `green_sec`/`red_sec`/`tbs_min` 超過實際經過的牆鐘時間。現已改為先合併同一 `(equipment_no, wo_no)` 的重疊時段再計算，不會再重複計入。

```csharp
// 建議:每小時呼叫一次,同時算今天和昨天
var t = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
t.Elapsed += (_, _) => {
    var today = DateOnly.FromDateTime(DateTime.UtcNow);
    _localData.CalcOeeDaily(today);
    _localData.CalcOeeDaily(today.AddDays(-1));
};
t.AutoReset = true; t.Start();
```

---

## 11 · 品質／SPC

對應 `measurement_item_master`（量測項目主檔）、`spc_chart_master`（SPC 規則主檔）、`equipment_spc_measurement_log`（量測明細，依日分區）。品質系統自有規則，非 MES 提供。

- `UpsertMeasurementItem(sysId, measurementItem, fieldCode, itemName, unit, isSpc, enable)`
- `UpsertSpcChart(chartSysId, matNo, machNo, workCenterNo, usl, lsl, ucl, lcl, cl, rUcl, rLcl, sampleQty, chartType, chartSpecType, status)`
- `WriteSpcMeasurement(edcRawdataSysId, woNo, flowId, pieceId, equipmentNo, itemSysId, chartSysId, specMax, specMin, dataValues, recordTime)` — `dataValues` 依序對應 `data_01`~`data_50`，筆數不足 50 自動補 NULL，超過 50 丟 `ArgumentException`；傳 `null` 丟 `ArgumentNullException`（2026-09 起）。

```csharp
var data = new decimal?[] { 5.01m, 5.02m, 4.99m }; // 只量測 3 點
_localData.WriteSpcMeasurement("EDC0001", "WO20260630001", "2", "P00012345", "EQ001",
    "ITEM_THICK", "CHART_THICK", specMax: 5m, specMin: 2m, dataValues: data, recordTime: DateTimeOffset.UtcNow);
```

---

## 12 · MES 代碼快取

對應 `mes_code_cache`：MES 沒有提供「列出全部產線/產品別/工作站」的清單查詢 API，EAP 平常查工單/機台主檔拿到代碼＋名稱時順便 UPSERT 進這張表，累積成 Dashboard 篩選下拉選單的來源。

### `UpsertMesCode(codeType, code, name)`
`codeType`：`"LINE"` / `"MAT_GROUP"` / `"WORK_CENTER"`。

```csharp
_localData.UpsertMesCode("LINE", "L01", "產線一");
```

---

## 附錄：方法 → 資料表速查

| 方法 | 寫入資料表 |
|---|---|
| `SyncEquipments` / `RegisterEquipment(s)` / `UnregisterEquipment` / `UpdateEquipmentMesInfo` / `UpdateEquipmentTraceCapability` | equipment_registry |
| `WriteEquipmentStatus` | equipment_status_current, equipment_status_log |
| `WriteEquipmentParameter(s)` | equipment_parameter_current, equipment_parameter_log |
| `WriteCycleTime` | equipment_cycle_time_log |
| `CalcAvailabilityPeriod`（DB 端排程計算,取代 `WriteAvailability`） | equipment_availability_log |
| `StartWorkOrder` / `EndWorkOrder` / `SyncWorkOrders` | equipment_workorder_current, equipment_workorder_log |
| `RaiseAlarm` / `ClearAlarm` / `SyncAlarms` | equipment_alarm_current, equipment_alarm_log |
| `WriteOutput` | equipment_output_log |
| `UpsertPiece` | workorder_piece |
| `StartEquipmentTraceEvent` / `EndEquipmentTraceEvent` | equipment_workorder_piece_log |
| `CalcOeeDaily`（DB 端排程計算,取代 `UpsertOeeDaily`） | equipment_oee_daily_log |
| `UpsertMeasurementItem` | measurement_item_master |
| `UpsertSpcChart` | spc_chart_master |
| `WriteSpcMeasurement` | equipment_spc_measurement_log |
| `UpsertMesCode` | mes_code_cache |
| *（無，migration 種子資料）* | status_light_map |

資料表之間完整的關聯圖另見 [`資料表關聯圖.md`](./資料表關聯圖.md)（給網頁看板團隊）。
