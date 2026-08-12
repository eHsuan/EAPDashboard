# 📘 CyntecMqttLib.dll 使用說明與開發指南

本文件專為機台 (EQ) 端及相關系統開發人員撰寫，詳細說明如何引用 `CyntecMqttLib.dll` 進行 MQTT 通訊連線、訊息發布、主題訂閱以及標準外層 JSON 封包 (`MessageEnvelope`) 的打包與解析。

---

## 📑 目錄

1. [模組簡介](#1-模組簡介)
2. [系統需求與相依套件](#2-系統需求與相依套件)
3. [核心類別與架構](#3-核心類別與架構)
4. [MQTT 外層標準封包規格 (Message Envelope)](#4-mqtt-外層標準封包規格-message-envelope)
5. [C# 開發實作範例](#5-c-開發實作範例)
   - [5.1 連線初始化 (MqttBus)](#51-連線初始化-mqttbus)
   - [5.2 自動打包並發布訊息 (PublishAsync)](#52-自動打包並發布訊息-publishasync)
   - [5.3 主題訂閱與訊息接收 (SubscribeAsync)](#53-主題訂閱與訊息接收-subscribeasync)
   - [5.4 獨立手動打包封包 (MessageEnvelope.Create)](#54-獨立手動打包封包-messageenvelopecreate)
6. [最佳實踐與注意事項](#6-最佳實踐與注意事項)

---

## 1. 模組簡介

`CyntecMqttLib.dll` 為 Cyntec 自動化團隊針對設備與 EAP/MES 通訊設計的現代化 MQTT 通訊驅動程式庫。模組內建了：
* 高階通訊匯流排 (`MqttBus`)，整合 MQTT 連線管理與自動重連機制。
* 全廠統一的 **MQTT 外層標準封包打包器 (`MessageEnvelope`)**，確保傳輸之 JSON 封包具備唯一追蹤識別碼 (`MessageId`) 與 UTC 時間戳記 (`TimestampUtc`)。

---

## 2. 系統需求與相依套件

### 執行與開發環境
* **目標架構**：.NET 8.0 (`net8.0`) 或以上版本。
* **開發語言**：C# 10.0+ / .NET 8 SDK。

### 交付與引用套件清單 (Dependencies)
提供給 EQ 端專案引用時，請確保目標建置目錄包含以下 **3 個核心 DLL 檔案**：

| 檔案名稱 | 版本 | 說明 |
| :--- | :--- | :--- |
| **`CyntecMqttLib.dll`** | `1.0.0` | Cyntec MQTT 核心驅動與打包元件 |
| **`MQTTnet.dll`** | `3.0.16` | 底層 MQTT 通訊協定庫 |
| **`Newtonsoft.Json.dll`** | `13.0.3` | JSON 序列化與反序列化模組 |

---

## 3. 核心類別與架構

| 命名空間 (Namespace) | 類別名稱 (Class) | 說明 |
| :--- | :--- | :--- |
| `CyntecMqttLib.Interfaces` | **`MqttBus`** | 高階 MQTT 通訊匯流排，負責連線、中斷、發布與訂閱。 |
| `CyntecMqttLib.Contracts` | **`MessageEnvelope`** | MQTT 標準外層封包格式與打包靜態方法。 |
| `CyntecMqttLib.Models` | **`TopicSchema`** | MQTT 主題格式標準模型。 |

---

## 4. MQTT 外層標準封包規格 (Message Envelope)

所有經由 `CyntecMqttLib` 發布的訊息，內部都會自動包覆於 `MessageEnvelope` 結構中。

### JSON 封包結構範例

```json
{
  "MessageId": "c4b2a8d1-7e89-4a92-91ef-3b10287a942e",
  "TimestampUtc": "2026-08-12T01:15:30.1234567Z",
  "Payload": {
    "EqpId": "TS0003",
    "Status": "RUN",
    "RecipeName": "RCP_SEMI_01",
    "AlarmCode": "0"
  }
}
```

### 欄位規格說明

| 欄位名稱 | 資料型態 | 範例與說明 |
| :--- | :--- | :--- |
| **`MessageId`** | `string` | GUID/UUID 唯一訊息追蹤碼 (例如 `"c4b2a8d1-..."`)。 |
| **`TimestampUtc`** | `DateTime` | ISO 8601 格式之 UTC 時間戳記 (例如 `"2026-08-12T01:15:30...Z"`)。 |
| **`Payload`** | `object` | EQ 端實際發送的業務資料內文 (可為任何自訂 C# 物件或 Anonymous Type)。 |

---

## 5. C# 開發實作範例

### 5.1 連線初始化 (`MqttBus`)

```csharp
using System;
using System.Threading.Tasks;
using CyntecMqttLib.Interfaces;

public class MqttEquipmentService
{
    private MqttBus _mqttBus;

    public async Task InitializeAsync(string brokerHost, int brokerPort = 1883, string eqpId = "EQ01")
    {
        // 建立 MqttBus 實例
        _mqttBus = new MqttBus(
            host: brokerHost,
            port: brokerPort,
            clientId: $"EQP_{eqpId}_{Guid.NewGuid():N}",
            cleanSession: true
        );

        // 訂閱連線狀態事件
        _mqttBus.Connected += () => Console.WriteLine("[MQTT] 成功連線至 Broker");
        _mqttBus.Disconnected += () => Console.WriteLine("[MQTT] 與 Broker 中斷連線");

        // 執行非同步連線
        await _mqttBus.ConnectAsync();
    }
}
```

---

### 5.2 自動打包並發布訊息 (`PublishAsync`)

呼叫 `PublishAsync` 時，`MqttBus` 會**自動將傳入的 payload 物件包裝為 `MessageEnvelope`** 並序列化傳送：

```csharp
public async Task ReportEquipmentStatusAsync(string eqpId, string status, string currentRecipe)
{
    if (!_mqttBus.IsConnected)
    {
        Console.WriteLine("MQTT 未連線，無法發送訊息");
        return;
    }

    string topic = $"cyntec/eqp/{eqpId}/status";

    // 自訂的機台資料 Payload
    var statusPayload = new
    {
        EqpId = eqpId,
        Status = status,
        Recipe = currentRecipe,
        ReportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    // 發布訊息（內部自動調用 MessageEnvelope.Create 打包）
    await _mqttBus.PublishAsync(topic, statusPayload, qos: 1, retain: false);
    Console.WriteLine($"[MQTT] 已發送狀態至主題: {topic}");
}
```

---

### 5.3 主題訂閱與訊息接收 (`SubscribeAsync`)

```csharp
public async Task SubscribeControlCommandsAsync(string eqpId)
{
    string commandTopic = $"cyntec/eqp/{eqpId}/command";

    await _mqttBus.SubscribeAsync(commandTopic, (topic, rawJson) =>
    {
        Console.WriteLine($"[MQTT] 收到主題 [{topic}] 訊息: {rawJson}");

        try
        {
            // 反序列化外層 MessageEnvelope 封包
            var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<CyntecMqttLib.Contracts.MessageEnvelope>(rawJson);
            
            Console.WriteLine($"訊息識別碼 MessageId: {envelope.MessageId}");
            Console.WriteLine($"發送時間 TimestampUtc: {envelope.TimestampUtc}");
            Console.WriteLine($"內文 Payload: {envelope.Payload}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解析封包失敗: {ex.Message}");
        }
    });
}
```

---

### 5.4 獨立手動打包封包 (`MessageEnvelope.Create`)

若 EQ 端使用自訂或既存的第三方 MQTT Client，僅需要 `CyntecMqttLib` 的**打包格式**：

```csharp
using CyntecMqttLib.Contracts;
using Newtonsoft.Json;

public string PackCustomData<T>(T rawPayload)
{
    // 使用 MessageEnvelope 靜態泛型方法進行打包
    MessageEnvelope envelope = MessageEnvelope.Create(rawPayload);

    // 轉為 JSON 字串
    string jsonOutput = JsonConvert.SerializeObject(envelope, Formatting.Indented);
    return jsonOutput;
}
```

---

## 6. 最佳實踐與注意事項

1. **非同步呼叫**：
   * 所有連線 (`ConnectAsync`) 與發布 (`PublishAsync`) 均為非同步方法，建議使用 `await` 呼叫，避免主 UI 執行緒凍結。
2. **資源釋放**：
   * `MqttBus` 實作了 `IDisposable` 介面。當程式關閉或服務停止時，請務必呼叫 `_mqttBus.Dispose()` 釋放 TCP 連線與內部資源。
3. **時區與時間戳記**：
   * `MessageEnvelope.TimestampUtc` 一律使用 **UTC 時間戳記**，接收端（如 EAP Dashboard）會再依需要轉換為當地時區。

---

*文件發行團隊：Cyntec Automation & EAP Development Team*
