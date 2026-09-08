# cyntec.WcfTxnReelCheck (.NET 8 x64 測試外殼專案)

本專案是專為 Cyntec IT 團隊準備的 **.NET 8 (x64) 驗證測試外殼**。
其 UI 介面與測試邏輯 **100% 仿照 IT 原生範例程式**（`doc\cyntec.WcfTxnReelCheck`），但已全面升級為現代化 **.NET 8 (x64) Windows Forms** 執行環境。

---

## 快速測試驗證流程 (5 分鐘)

當 IT 團隊依據需求清單將各 DLL 專案升級為 .NET 8 (x64) 後，請依以下步驟進行驗收：

### 步驟 1：放入改好之 .NET 8 (x64) DLL
將編譯完成的 8~9 個 DLL 檔案直接覆蓋至本專案的 `dll/` 目錄下：
* `dll/cyntec.Base.dll`
* `dll/cyntec.Delta.dll`
* `dll/cyntec.MES.dll`
* `dll/cyntec.Tools.dll`
* `dll/cyntec.wcfConnection.dll`
* `dll/cyntec.wcfInterface.dll`
* `dll/cyntec.wcfMessage.dll`
* `dll/cyntec.WinMESLite.dll`
* `dll/cyntec.winBase.dll` (若有)

### 步驟 2：建置專案 (Build)
開啟終端機（PowerShell / CMD）切換至本目錄，執行：
```powershell
dotnet build
```
* **驗收標準**：建置輸出為 `0 個錯誤`，且不再出現 `CS8012 (以不同的處理器為目標)` 警告。

### 步驟 3：啟動測試 (Run)
在終端機執行：
```powershell
dotnet run
```
或直接以 Visual Studio 開啟專案按 `F5` 啟動。

### 步驟 4：功能測試驗收
1. **連線設定**：確認 `cyntec.ReelCheck.xml` 指向正確的 MES 測試伺服端（`SERVER_URL`）。
2. **工單查詢**：輸入工單號、機台號、工號，點擊 **「#2 標籤程式初始化(工單查詢)」**，確認順利取回工單資訊與 Reel 清單。
3. **防混掃碼**：勾選條碼類型並點擊測試條碼，驗證防呆狀態是否正確亮綠燈（正常）或紅燈（異常）。
4. **工單完工**：點擊 **「#4 工單結束作業」**，確認過帳與狀態回寫正常。

---

若上述測試全數通過，代表該批 DLL 已完全相容 64 位元 .NET 8 環境，可直接交付給 EAP 自動化系統上線使用！
