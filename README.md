# 🚀 EAP Dashboard (Equipment Automation Program)

![.NET Version](https://img.shields.io/badge/.NET-8.0--windows-blue.svg)
![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)
![Language](https://img.shields.io/badge/Language-C%23-blue.svg)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)

`EAP Dashboard` 為資深半導體與電子零組件製造廠打造的現代化**設備自動化控制與監控儀表板 (Equipment Automation Program)**。系統基於 .NET 8 WinForms 架構，整合了 **SECS/GEM** 與 **Cyntec MQTT** 雙通訊協定，並實現了與 **MES 系統 (Manufacturing Execution System)** 的即時雙向通訊、自動化報工、管理項目快取以及本地端歷史資料庫 (PostgreSQL) 的即時寫入與孤立警報自動校正。

---

## 🌟 核心功能特色 (Key Features)

### 1. 🔌 多通訊協定整合 (Multi-Protocol Support)
* **SECS/GEM Protocol (Protocol.Drivers.Secs)**：
  - 支援基於 TCP/IP 的 **HSMS 通訊**。
  - 連線建立時自動發送 `S1F13` 與 `S1F1` 握手連線，並透過 `S1F3` 進行狀態點位 (SVID) 查詢。
  - 解析 `S6F11` Event Report（包含 `TRACK_IN`、`TRACK_OUT`、`PROCESS_DATA`、`STATUS_CHANGE` 等事件）。
* **Cyntec MQTT Protocol (CyntecMqttLib)**：
  - 支援即時 MQTT Broker 訊息發布與訂閱。
  - 機台狀態與告警訊息的即時佇列 (Queue) 處理與多執行緒解耦。

### 2. 🏭 工廠 MES 雙向整合 (Protocol.MES)
* **自動化報工與檢核流程**：
  - **`WOQRY` (工單查詢)**：查詢工單詳細資訊及進出站必須填寫的管理項目 (`InputCodeListDT`)。
  - **`WOCHECKIN` (進站報工)**：自動帶入 `ManagementItems` 字典對應點位與 Operator 資訊完成工單開工。
  - **`WOCHECKOUT` (出站報工)**：帶入產出數、不良數與 Operator 資訊完成工單完工。
  - **`UserVerify` (人員驗證)**：驗證操作員刷卡 Barcode 權限。
* **高可讀性 Indented JSON 日誌**：
  - 內建 `CallMesWithLog` 模組，完整格式化印出 `[SEND]` 與 `[RECV]` 的 JSON 封包與呼叫歷程。
* **多環境彈性切換**：
  - 支援在 `config.json` 快速切換 `"ActiveEnvironment": "Test"` (連線至本地 MESSimulator `http://localhost:8070`) 或 `"Production"` (連線正式 MES 伺服器)。

### 3. 🛡️ 孤立警報校正機制 (Orphan Alarm Reconciliation)
* 針對 EAP 重啟或斷線期間可能遺漏 `Alarm Clear` 事件的 Edge Case，設計了**自癒校正流程**：
  - **情境 A (啟動時為 RUN/IDLE)**：立即清空本地 DB `equipment_alarm_current` 並補齊 `equipment_alarm_log` 的 `end_time`。
  - **情境 B (啟動時為 ALARM/DOWN)**：維持旗標，直到狀態轉為 RUN/IDLE 後啟動 **30 秒倒數防護計時器**，確認機台持續穩定運作後自動清空結算。

### 4. 📚 字典檔與管理項目快取 (Dictionary Preloading)
* 系統啟動時會預先將各設備專屬的 JSON 字典檔（例如 `Dictionaries/TS0003.json`）讀取並快取於記憶體 (`ConcurrentDictionary`) 中，支援不區分大小寫的高效能 `ManagementItems` 秒級檢索。

### 5. 🧪 完整模擬器與測試測試套件 (Simulators & Test Suite)
* **EQSimulator**：模擬實體機台上報 SECS/MQTT 狀態、事件與告警。
* **MESSimulator**：完整模擬 MES 伺服器回應，支援區分進站/出站 RequireInput 邏輯與工單狀態地圖檢核。
* **EAPDashboard.Tests**：基於 xUnit + FluentAssertions 的單元測試。

---

## 🏗️ 系統架構 (System Architecture)

```
EAPDashboard/
 ├── Controls/                     # 現代化自訂 UI 控制項 (如卡片式機台狀態視窗)
 ├── CyntecMqttLib/                # Cyntec 專屬 MQTT 通訊驅動專案
 ├── Dictionaries/                 # 各機台字典檔 (定義 AlarmMapping 與 ManagementItems)
 │    ├── TS0003.json
 │    └── TOWA_YPM.json
 ├── EAPController/                # 控制層邏輯
 ├── EAPDashboard.Tests/           # xUnit 單元測試專案
 ├── EAPGateway/                   # 微服務 API Gateway 模組
 ├── EQSimulator/                  # 機台模擬器應用程式
 ├── MESSimulator/                 # MES 伺服器模擬器應用程式
 ├── Models/                       # 核心領域模型
 │    ├── EquipmentBase.cs         # 設備抽象基底類別 (包含報工、快取與校正)
 │    ├── SecsEquipment.cs         # SECS 設備驅動類別
 │    └── MqttEquipment.cs         # MQTT 設備驅動類別
 ├── Protocol.Drivers.Secs/        # SECS 協議封包驅動
 ├── config.json                   # 系統全域設定檔 (含環境連線與設備清單)
 └── dll/                          # 核心編譯 DLL 庫 (Protocol.MES, Protocol.LocalData)
```

---

## ⚡ 快速開始 (Quick Start)

### 1. 環境需求
* **作業系統**：Windows 10 / 11 / Windows Server
* **執行環境**：[.NET 8.0 SDK / Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
* **資料庫**：PostgreSQL 12+ (本地或遠端)

### 2. 資料庫初始化 (Local DB)
請執行 `dll/Protocol.LocalData/` 目錄下的 SQL 腳本建立資料表：
```bash
# 在 PostgreSQL 中建立資料庫
CREATE DATABASE Protocol_localdata;

# 執行 001_init_schema.sql 與 005_equipment_alarm.sql 腳本
```

### 3. 編譯與建置專案
開啟 PowerShell 或 Command Prompt 執行：
```powershell
# 建置整個 Solution
dotnet build EAPDashboard.sln

# 執行單元測試
dotnet test EAPDashboard.Tests/EAPDashboard.Tests.csproj
```

### 4. 啟動應用程式
```powershell
# 執行 EAP Dashboard
dotnet run --project EAPDashboard.csproj
```

---

## ⚙️ 設定檔說明 (`config.json`)

```json
{
  "ExitPassword": "admin",
  "ActiveEnvironment": "Test",
  "EnableCassetteBinding": true,
  "EqpStatusReportIntervalSeconds": 60,
  "MesSettings": {
    "TestUrl": "http://localhost:8070/Eqp_Portal.asmx",
    "ProductionUrl": "http://twcynmes01.delta.corp/Eqp_Portal/Eqp_Portal.asmx"
  },
  "PostgresDbSettings": {
    "ConnectionString": "Host=localhost;Port=5432;Database=Protocol_localdata;Username=postgres;Password=your_password"
  },
  "Equipments": [
    {
      "Id": "ZZ0304",
      "Name": "TOWA 封裝機 01",
      "Protocol": "SECS",
      "Ip": "127.0.0.1",
      "Port": 5000,
      "DeviceId": 1,
      "DictionaryPath": "Dictionaries/TS0003.json"
    }
  ]
}
```

---

## 🛠️ 開發與維護團隊 (Development)

* **開發團隊**：Advanced Agentic Coding / Cyntec Automation Team
* **授權協議**：Proprietary / Internal Use Only
