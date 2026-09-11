# 發布內容說明（2026-09-07）

本次發布為 code-review 修復（詳見 `LocalData 修復報告`）+ Protocol.MES 新增 `GetEqpInfo`（設備整合資訊需求 2.109 節）後的重新建置版本。皆以 Release 組態建置，並通過 `Protocol.MES.sln` 整包重建驗證。

## 資料夾結構

```
發布包_20260907/
├─ Protocol.LocalData/
│   ├─ Protocol.LocalData.dll          ← 本地端資料存取 DLL（Npgsql 已內嵌，無需額外安裝）
│   ├─ Migrations/                     ← 001~018 號 SQL migration + run_migrations.bat
│   └─ Protocol.LocalData_使用說明.md   ← 這支 DLL 的完整 Function 說明
│
├─ Protocol.MES/
│   ├─ Protocol.MES.dll                ← MES 溝通 DLL（含本次新增的 GetEqpInfo）
│   └─ *.dll / app.config              ← 隨附的相依套件（Newtonsoft.Json、System.ServiceModel 系列等），
│                                         需與 Protocol.MES.dll 放在同一資料夾
│
├─ Protocol.LocalData.Scheduler/
│   ├─ Protocol.LocalData.Scheduler.exe← 獨立排程程式（自含 .NET Runtime，單一 exe）
│   ├─ install_scheduler_task.bat      ← 一鍵安裝 Windows 工作排程器任務（需系統管理員身分執行）
│   └─ uninstall_scheduler_task.bat    ← 一鍵移除
│
└─ EAP_DLL_整合手冊.md                  ← 串接兩支 DLL 的整合手冊（含分節說明、範例程式碼）
```

## 安裝／部署順序建議

1. **資料庫**：在目標 PostgreSQL 執行 `Protocol.LocalData/Migrations/run_migrations.bat`（會依序套用 001~018 號 migration；已執行過的環境只需補跑新增的號碼）。
2. **DLL**：把 `Protocol.LocalData.dll` 與 `Protocol.MES/` 資料夾內全部檔案放進 EAP 專案可參考到的路徑，依 `EAP_DLL_整合手冊.md` 串接。
3. **排程**：把 `Protocol.LocalData.Scheduler/` 整個資料夾複製到固定路徑（例如 `C:\ProgramData\Protocol\Scheduler\`，**不要**跟 EAP 裝在同一資料夾），編輯 `install_scheduler_task.bat` 內的連線字串後以系統管理員身分執行。

## 本次版本重點

- `Protocol.LocalData.dll`：修復 7 項 code-review 發現的問題（詳見另外交付的「LocalData 修復報告」），涵蓋設備主檔寫入例外處理、片/載具追溯孤兒紀錄結清、OEE 雙槽位去重、SPC 寫入效能等。
- `Protocol.LocalData.Scheduler.exe`：新增分區維護呼叫，不再完全依賴 EAP 存活。
- `Protocol.MES.dll`：新增 `GetEqpInfo` 交易（查詢設備資訊，對應設備整合資訊需求文件第 2.109 節）。

如需舊版比對或原始碼，請洽 Protocol.MES 版控（GitLab）。
