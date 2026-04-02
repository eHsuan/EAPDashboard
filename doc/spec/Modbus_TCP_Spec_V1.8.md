# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.8

本規格書採用「Bit 觸發 + Word ACK」混和設計，利用 Word (1:OK, 2:NG) 提供更強健的交易結果回饋。

## 1. 狀態與觸發位元組 (PLC -> EAP)
位址 **40001**：採 Bit-Mapped 方式定義 (EAP 輪詢讀取)。
| Bit | 名稱 | 說明 |
| :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 0/1 切換 |
| Bit 1 | TimeSync Req | 請求時間校正 |
| Bit 2 | Status Query Req | 請求狀態同步 |
| Bit 3 | UserVerify Req | 人員登入驗證請求 |
| Bit 4 | TrackIn Req | 進站報工請求 |
| Bit 5 | TrackOut Req | 出站報工請求 |
| Bit 6 | Ready Status | 機台就緒訊號 |
| Bit 7 | Alarm Status | 機台警報訊號 |

---

## 2. 流程資料專區 (含 Word ACK)

### 2.1 人員驗證專區 (UserVerify)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40010 | 10w | ASCII | PLC→EAP | **[R]** UserID (工號) |
| 40020 | 40w | ASCII | PLC→EAP | **[R]** Barcode (長代碼) |
| 40060 | 20w | ASCII | EAP→PLC | **[W]** ResultMsg (回覆訊息) |
| 40080 | 1w | WORD | EAP→PLC | **[W] ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.2 進站報工專區 (Track-In)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40100 | 10w | ASCII | PLC→EAP | **[R]** UserID (工號) |
| 40110 | 10w | ASCII | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40120 | 10w | ASCII | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40130 | 30w | ASCII | PLC→EAP | **[R]** MaterialID (材料ID) |
| 40160 | 30w | ASCII | PLC→EAP | **[R]** ToolingID (治具ID) |
| 40190 | 10w | ASCII | PLC→EAP | **[R]** PanelID (板號) |
| 40200 | 1w | WORD | PLC→EAP | **[R]** Qty (進站數量) |
| 40210 | 20w | ASCII | EAP→PLC | **[W]** RecipeID (MES 指派程式) |
| 40230 | 20w | ASCII | EAP→PLC | **[W]** ResultMsg (處理結果) |
| 40250 | 1w | WORD | EAP→PLC | **[W] ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.3 出站報工專區 (Track-Out)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40300 | 10w | ASCII | PLC→EAP | **[R]** UserID (工號) |
| 40310 | 10w | ASCII | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40320 | 10w | ASCII | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40330 | 1w | WORD | PLC→EAP | **[R]** Qty (產出數量) |
| 40340 | 10w | ASCII | PLC→EAP | **[R]** Result (PASS/FAIL) |
| 40350 | 10w | ASCII | PLC→EAP | **[R]** NGCode (不良代碼) |
| 40360 | 20w | ASCII | EAP→PLC | **[W]** ResultMsg (處理結果) |
| 40380 | 1w | WORD | EAP→PLC | **[W] ResultCode (0:Idle, 1:OK, 2:NG)** |

---

## 3. 交握邏輯說明 (Word ACK 四步法)

1.  **PLC**: 準備資料 -> 將 `TriggerBit` (40001) 設為 **1**。
2.  **EAP**: 偵測到 1 -> 執行業務 -> 寫入資料與訊息 -> 將 **`ResultCode`** 設為 **1 (OK)** 或 **2 (NG)**。
3.  **PLC**: 偵測到 `ResultCode > 0` -> 執行機台動作 -> 將 `TriggerBit` 歸 **0**。
4.  **EAP**: 偵測到 `TriggerBit == 0` -> 將 **`ResultCode` 歸 0** (回到初始空閒狀態)。

---
**版本**: V1.8 | **日期**: 2026-03-30
