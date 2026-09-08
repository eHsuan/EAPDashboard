# 【技術需求聯絡單】TP 標籤防混檢驗模組升級 .NET 8 (x64) 專案修改清單

| 文件版本 | 發行日期 | 提出單位 | 接收單位 | 升級目標 |
| :---: | :---: | :---: | :---: | :---: |
| **v3.0 (直球升級版)** | **2026-09-08** | EAP 自動化系統開發組 | IT 製造執行系統 (MES) 開發組 | **維持現有 DLL 結構，全面直上 .NET 8 (x64)** |

---

## 1. 升級目標與原則說明

* **核心目標**：**維持 IT 現有的各 DLL 模組結構（不需要合併 DLL）**，亦**不需考慮舊版相容問題**。
* **唯一目的**：將現有的專案原始碼直接改造成 **.NET 8 (x64)**，編譯產出 64 位元的 DLL 供 EAP 直接引用。
* **IT 開發團隊實際要做的事只有 3 點**：
  1. **專案檔升級**：將現有各 DLL 專案檔改為 .NET 8 SDK 格式（`<TargetFramework>net8.0</TargetFramework>`、`<PlatformTarget>x64</PlatformTarget>`）。
  2. **套件更換**：移除舊版 GAC 引用，改裝微軟官方 WCF 8.x NuGet 套件。
  3. **代碼微調**：僅有 **2 個檔案**需要修改程式碼（`cyntec.wcfConnection` 移除雙工 HTTP、`cyntec.Tools` 替換序列化元件），其餘 6 個 DLL 均為純邏輯重新編譯。

---

## 2. 各 DLL 專案具體修改清單 (1 對 1 對照)

維持 IT 現有 DLL 劃分，各專案修改指引如下：

| 序號 | 專案名稱 (DLL) | 程式碼需修改？ | 專案檔 (csproj) 修改指引 | 相依套件與代碼修改重點 |
| :---: | :--- | :---: | :--- | :--- |
| **1** | **`cyntec.wcfConnection`** | **需要** | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 1. 移除舊版 GAC `System.ServiceModel`。<br>2. 引入 NuGet 套件：<br>　- `System.ServiceModel.Primitives` (8.1.0)<br>　- `System.ServiceModel.Http` (8.1.0)<br>　- `System.ServiceModel.NetTcp` (8.1.0)<br>3. 修改 `wcfServiceConnection.cs`：移除 `WSDualHttpBinding`，改用 `BasicHttpBinding`（詳見第 3.1 節）。 |
| **2** | **`cyntec.Tools`** | **需要** | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 1. 移除過時的 `System.Web.Extensions` 引用。<br>2. 修改 `cyntecJsonDictionary.cs`：改用 `System.Text.Json`（詳見第 3.2 節）。<br>3. 若有使用 `DotNetZip`，請更新至 .NET Standard 相容版本或使用 `System.IO.Compression`。 |
| **3** | **`cyntec.Base`** | 免改 (加旗標) | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>`<br>`<EnableUnsafeBinaryFormatterSerialization>true</EnableUnsafeBinaryFormatterSerialization>` | 因 `cyntecByteConvert.cs` 有使用 `BinaryFormatter`，專案檔加上上述旗標即可於 .NET 8 正常運作，程式碼無需修改。 |
| **4** | **`cyntec.MES`** | 免改代碼 | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 1. 移除 GAC `System.ServiceModel` 與 `System.Runtime.Serialization`。<br>2. 引入 NuGet 套件 `System.ServiceModel.Primitives` (8.1.0)。<br>3. 業務資料合約代碼 100% 相容。 |
| **5** | **`cyntec.wcfInterface`** | 免改代碼 | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 引入 NuGet 套件 `System.ServiceModel.Primitives` (8.1.0)，介面代碼免改。 |
| **6** | **`cyntec.WinMESLite`**<br>(防混檢驗核心) | 免改代碼 | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 核心防混驗證邏輯（`ReelBarcodeCheck.cs`）完全相容，引用上述升級後的 .NET 8 DLL 直接重新編譯即可。 |
| **7** | **`cyntec.Delta`** | 免改代碼 | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 2D 條碼解析規格（`Barcode2D88.cs`），純字串邏輯，直接編譯為 .NET 8 x64。 |
| **8** | **`cyntec.wcfMessage`** | 免改代碼 | `<TargetFramework>net8.0</TargetFramework>`<br>`<PlatformTarget>x64</PlatformTarget>` | 純訊息傳輸結構，直接編譯為 .NET 8 x64。 |
| **9** | **`cyntec.winBase`**<br>(若有需要 UI) | 免改代碼 | `<TargetFramework>net8.0-windows</TargetFramework>`<br>`<UseWindowsForms>true</UseWindowsForms>`<br>`<PlatformTarget>x64</PlatformTarget>` | WinForms 對話框控制項。EAP 主要是呼叫防混邏輯，若不使用此 UI，亦可不編譯此組件。 |

---

## 3. 具體需要修改程式碼的 2 處細節 (Before vs After)

IT 開發人員只需修改以下兩處程式碼，即可排除所有編譯與連線錯誤：

### 3.1 檔案一：`cyntec.wcfConnection` -> `wcfServiceConnection.cs`
* **問題**：微軟官方 .NET 8 WCF Client 徹底移除了 `WSDualHttpBinding`。
* **位置**：約第 274 行附近。
* **修改對照**：

```csharp
// ========== 【修改前 原代碼】 ==========
objBinding = new WSDualHttpBinding();
(objBinding as WSDualHttpBinding).ClientBaseAddress = new Uri(m_ClientUrl);
(objBinding as WSDualHttpBinding).MaxReceivedMessageSize = 104857600L;
(objBinding as WSDualHttpBinding).MaxBufferPoolSize = 1000000L;
(objBinding as WSDualHttpBinding).ReaderQuotas.MaxStringContentLength = 10000000;
(objBinding as WSDualHttpBinding).ReaderQuotas.MaxArrayLength = 123000000;
(objBinding as WSDualHttpBinding).Security.Mode = WSDualHttpSecurityMode.None;
objBinding.OpenTimeout = new TimeSpan(0, 0, 3);
objBinding.CloseTimeout = new TimeSpan(0, 0, 15);
objBinding.SendTimeout = new TimeSpan(0, 0, 15);

// ========== 【修改後 建議代碼 (改為標準 BasicHttpBinding)】 ==========
BasicHttpBinding basicBinding = new BasicHttpBinding();
basicBinding.MaxReceivedMessageSize = 104857600L;
basicBinding.MaxBufferPoolSize = 1000000L;
basicBinding.ReaderQuotas.MaxStringContentLength = 10000000;
basicBinding.ReaderQuotas.MaxArrayLength = 123000000;
basicBinding.Security.Mode = BasicHttpSecurityMode.None;
basicBinding.OpenTimeout = new TimeSpan(0, 0, 10);
basicBinding.CloseTimeout = new TimeSpan(0, 0, 15);
basicBinding.SendTimeout = new TimeSpan(0, 0, 30);
objBinding = basicBinding;
```
*(注：若伺服端支援 NetTcp，原本程式碼中的 `NetTcpBinding` 區塊保留即可，.NET 8 完全支援 NetTcp)*

---

### 3.2 檔案二：`cyntec.Tools` -> `cyntecJsonDictionary.cs`
* **問題**：.NET 8 已移除 ASP.NET 舊有的 `System.Web.Script.Serialization.JavaScriptSerializer`。
* **修改對照**：

```csharp
// ========== 【修改前 原代碼】 ==========
using System.Web.Script.Serialization;

public static Dictionary<string, object> ToDictionary(string jsonText)
{
    JavaScriptSerializer serializer = new JavaScriptSerializer();
    return serializer.Deserialize<Dictionary<string, object>>(jsonText);
}

// ========== 【修改後 建議代碼 (改用 .NET 8 內建 System.Text.Json)】 ==========
using System.Text.Json;

public static Dictionary<string, object> ToDictionary(string jsonText)
{
    return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonText);
}
```

---

## 4. 交付清單與驗收條件

### 4.1 交付檔案清單
請 IT 提供以下以 **.NET 8 (x64)** 編譯完成之二進位 DLL（共 8 個）：
* `cyntec.Base.dll`
* `cyntec.Delta.dll`
* `cyntec.MES.dll`
* `cyntec.Tools.dll`
* `cyntec.wcfConnection.dll`
* `cyntec.wcfInterface.dll`
* `cyntec.wcfMessage.dll`
* `cyntec.WinMESLite.dll`

### 4.2 驗收標準
1. **架構驗證**：DLL 經 PowerShell 檢驗其 `ProcessorArchitecture` 為 **`MSIL / AnyCPU`** 或 **`AMD64 (x64)`**，目標框架為 **`.NETCoreApp,Version=v8.0`**。
2. **運行驗證**：EAP (64位元 .NET 8) 專案直接引用上述 DLL，可正常完成工單查詢（`WoInfo_Get`）、條碼比對（`m_ReelCheckInput_onMessageArrive`）與過帳記錄（`WO_ReelID_Update`），無拋出 `BadImageFormatException` 或 WCF 連線異常。
