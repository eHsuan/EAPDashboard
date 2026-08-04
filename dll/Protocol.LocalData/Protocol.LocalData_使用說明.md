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
 ├─ 001_init_schema.sql            ← 建立所有資料表與索引
 ├─ 002_partition_maintenance.sql  ← 建立機台參數分區維護函式
 ├─ 003_equipment_registry.sql     ← 建立設備清單表
 └─ run_migrations.bat             ← 雙擊執行，一鍵完成所有建表動作
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

// 2. 確保機台參數分區存在（每天至少呼叫一次）
_localData.EnsureParameterLogPartitions();

// 3. 啟動每日 Timer，確保分區持續維護
var partitionTimer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
partitionTimer.Elapsed += (_, _) => _localData.EnsureParameterLogPartitions();
partitionTimer.AutoReset = true;
partitionTimer.Start();
```

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

---

### 設備狀態

#### `WriteEquipmentStatus(equipmentNo, statusCode, statusDesc, eventTime)`

寫入設備狀態，同時更新現況快照（`equipment_status_current`）與歷史紀錄（`equipment_status_log`）。

- **使用時機**：設備狀態發生變化時呼叫。
- **statusCode 建議值**：`RUN`、`IDLE`、`DOWN`、`ALARM`、`MAINTENANCE`

```csharp
_localData.WriteEquipmentStatus("EQ001", "RUN", "正常運轉", DateTimeOffset.UtcNow);
_localData.WriteEquipmentStatus("EQ001", "ALARM", "溫度超標", DateTimeOffset.UtcNow);
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
    new ParameterData("TEMP1",     "加熱溫度", 185.3m, 200m, 150m, false, now, threshold: 0.5m),
    new ParameterData("PRESSURE1", "射出壓力",  95.0m, 150m,  55m, false, now, threshold: 2.0m),
    new ParameterData("SPEED1",    "螺桿轉速",  980m, 1200m, 850m, false, now, threshold: 5.0m),
};

_localData.WriteEquipmentParameters("EQ001", parameters);
```

#### `WriteEquipmentParameter(equipmentNo, paramCode, paramName, actualValue, upperLimit, lowerLimit, isAlarm, recordTime, threshold = 0)` ← 單筆寫入

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
    threshold:   0.5m
);
```

> **isAlarm** 由呼叫端計算（`actualValue > upperLimit || actualValue < lowerLimit`），告警狀態改變時一定會寫入，不受 threshold 限制。

---

### CycleTime

#### `WriteCycleTime(equipmentNo, woNo, cycleTimeSec, recordTime)`

寫入單筆生產週期時間。

- **使用時機**：每完成一個生產週期呼叫一次。

```csharp
_localData.WriteCycleTime("EQ001", "WO20260630001", cycleTimeSec: 12.5m, DateTimeOffset.UtcNow);
```

---

### Availability

#### `WriteAvailability(equipmentNo, periodStart, periodEnd, runTimeSec, downTimeSec, availabilityRate)`

寫入可動率區段統計。

- **使用時機**：每班或每小時彙總後呼叫一次（非即時，是彙總後的統計值）。
- **availabilityRate**：0~1 之間的小數（例如 0.888 代表 88.8%）。
- **重要**：此 Function 只負責寫入已計算好的結果，稼動率的計算邏輯需由 EAP 端自行維護。

#### 稼動率計算公式

```
稼動率 = 運轉時間 / (運轉時間 + 停機時間)
```

#### EAP 端建議實作方式

EAP 需在內部維護一個狀態計時器，每次呼叫 `WriteEquipmentStatus` 切換狀態時同步累加時間，並在每小時整點（或每班結束）彙總後呼叫 `WriteAvailability`。

```csharp
// EAP 內部維護狀態計時（每台設備各自維護）
private DateTimeOffset _statusStartTime = DateTimeOffset.UtcNow;
private string _currentStatus = "RUN";
private decimal _runTimeSec = 0;
private decimal _downTimeSec = 0;

// 每次狀態切換時呼叫，累加上一個狀態的持續時間
void OnStatusChanged(string equipmentNo, string newStatus)
{
    decimal elapsed = (decimal)(DateTimeOffset.UtcNow - _statusStartTime).TotalSeconds;

    if (_currentStatus == "RUN")
        _runTimeSec += elapsed;
    else
        _downTimeSec += elapsed;

    _statusStartTime = DateTimeOffset.UtcNow;
    _currentStatus = newStatus;

    _localData.WriteEquipmentStatus(equipmentNo, newStatus, "", DateTimeOffset.UtcNow);
}

// 每小時整點呼叫一次（或每班結束時呼叫）
void OnHourlyTick(string equipmentNo)
{
    // 補上這個小時最後一段尚未切換的時間
    decimal elapsed = (decimal)(DateTimeOffset.UtcNow - _statusStartTime).TotalSeconds;
    if (_currentStatus == "RUN") _runTimeSec += elapsed;
    else _downTimeSec += elapsed;

    decimal total = _runTimeSec + _downTimeSec;
    decimal rate = total > 0 ? _runTimeSec / total : 0;

    _localData.WriteAvailability(
        equipmentNo:      equipmentNo,
        periodStart:      DateTimeOffset.UtcNow.AddHours(-1),
        periodEnd:        DateTimeOffset.UtcNow,
        runTimeSec:       _runTimeSec,
        downTimeSec:      _downTimeSec,
        availabilityRate: rate
    );

    // 重置計時器，開始下一個統計區間
    _runTimeSec = 0;
    _downTimeSec = 0;
    _statusStartTime = DateTimeOffset.UtcNow;
}
```

#### 建議寫入頻率

| 時機 | 說明 |
|---|---|
| 每小時整點 | 最常見，前端可畫出每小時稼動率趨勢 |
| 每班結束時 | 例如日班(08:00~16:00)結束時寫一筆 |
| 兩者都寫 | 每小時細粒度 + 每班彙總，供不同顯示需求使用 |

---

### 工單

#### `SetCurrentWorkOrder(equipmentNo, woNo, startTime)`

設定設備目前執行的工單。

- **使用時機**：換工單時呼叫，DLL 會自動關閉前一筆工單歷程的 `end_time`，並開立新的現況與歷程紀錄。

```csharp
_localData.SetCurrentWorkOrder("EQ001", "WO20260630002", DateTimeOffset.UtcNow);
```

---

### 產出

#### `WriteOutput(equipmentNo, woNo, outputQty, ngQty, recordTime)`

寫入產出與不良數。

- **使用時機**：每批次或每週期完成後呼叫一次。

```csharp
_localData.WriteOutput("EQ001", "WO20260630001", outputQty: 100, ngQty: 2, DateTimeOffset.UtcNow);
```

---

## 注意事項總覽

| 項目 | 說明 |
|---|---|
| **單一實例** | `LocalDataService` 整個 App 只能 `new` 一次，長期保存使用 |
| **時間使用 UTC** | 所有時間參數傳入 `DateTimeOffset.UtcNow`，前端顯示時自行轉換本地時區 |
| **EnsureParameterLogPartitions 每日必呼叫** | 建議 EAP 啟動時 + 每日 Timer，超過 2 天未呼叫機台參數寫入會失敗 |
| **WriteEquipmentParameter threshold** | 每種參數類型設定適合的容許誤差，避免頻繁寫入 DB |
| **SyncEquipments 會刪除清單外設備** | EAP 啟動傳入的清單即為最終清單，不在清單內的設備會從 `equipment_registry` 移除 |

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
│ equipment_output_log        │ 產出                    │
└─────────────────────────────┴────────────────────────┘

