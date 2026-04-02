# Modbus TCP 通訊規格書 (EAP V1.0.3 標準)

本文件定義了 EAP Dashboard 系統與 PLC 之間透過 Modbus TCP 進行交握的位址配置。

## 1. 基本通訊參數
*   **通訊協定**: Modbus TCP (Ethernet)
*   **資料格式**: Big-Endian (高位元在前)
*   **預設埠號**: 502
*   **資料區塊**: Holding Registers (4xxxx)

---

## 2. PLC -> EAP 讀取區 (Block 40001 - 40100)

本區塊由 PLC 寫入資料，EAP 負責主動輪詢讀取。

### 2.1 Rolling Triggers (交易觸發計數器)
| 位址 | 名稱 | 說明 | 觸發模式 |
| :--- | :--- | :--- | :--- |
| 40001 | UserVerify Trigger | 人員登入驗證觸發 | ROLLING |
| 40002 | TrackIn Trigger | 進站請求觸發 | ROLLING |
| 40003 | TrackOut Trigger | 出站請求觸發 | ROLLING |
| 40004 | Alarm Trigger | 警報上報觸發 | ROLLING |
| 40005 | DataReport Trigger | 製程數據上報觸發 | ROLLING |
| 40006 | Status Trigger | 狀態變更上報觸發 | ROLLING |

### 2.2 PLC Status Word (機台狀態位元組)
位址 **40010**：採 Bit-Mapped 方式定義。
| Bit | 名稱 | 說明 | 交握模式 |
| :--- | :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 心跳位元 (0/1 週期切換) | HEARTBEAT |
| Bit 1 | TimeSync Req | 請求 EAP 校正時間 (0 -> 1) | BIT 0/1 |
| Bit 2 | Status Query Req | 請求 EAP 更新狀態 (0 -> 1) | BIT 0/1 |
| Bit 3 | Ready Status | 機台就緒訊號 (1:Ready, 0:Busy) | LEVEL |
| Bit 4 | Alarm Status | 機台警報訊號 (1:Alarm, 0:Normal) | LEVEL |
| Bit 5-15 | Reserved | 預留 | - |

### 2.3 PLC Data Area (生產資料緩衝區)
| 位址 | 長度 (Word) | 名稱 | 說明 |
| :--- | :--- | :--- | :--- |
| 40020 | 10 | Barcode / UserID | 人員識別碼 (ASCII) |
| 40030 | 10 | Cassette ID | 載具編號 (ASCII) |
| 40040 | 10 | WorkOrder | 工單號碼 (ASCII) |
| 40050 | 1 | Alarm Code | 原始警報代碼 (Integer) |
| 40051 | 10 | Process Data | 製程數據內容 (ASCII/WORD) |

---

## 3. EAP -> PLC 寫入區 (Block 41001 - 41100)

本區塊由 EAP 寫入回覆資料，PLC 負責讀取並完成後續動作。

### 3.1 ACK Counters (交握回覆計數器)
| 位址 | 名稱 | 說明 | 對應位址 |
| :--- | :--- | :--- | :--- |
| 41001 | UserVerify ACK | 人員驗證完成回覆 | 40001 |
| 41002 | TrackIn ACK | 進站處理完成回覆 | 40002 |
| 41003 | TrackOut ACK | 出站處理完成回覆 | 40003 |
| 41004 | Alarm ACK | 警報處理完成回覆 | 40004 |
| 41005 | DataReport ACK | 數據記錄完成回覆 | 40005 |
| 41006 | Status ACK | 狀態同步完成回覆 | 40006 |

### 3.2 EAP Command Word (EAP 控制位元組)
位址 **41010**：採 Bit-Mapped 方式定義。
| Bit | 名稱 | 說明 | 對應位址 |
| :--- | :--- | :--- | :--- |
| Bit 0 | TimeSync ACK | 已完成時間寫入 (1:Done, 0:Idle) | 40010.1 |
| Bit 1 | Status Query ACK | 已完成狀態同步 (1:Done, 0:Idle) | 40010.2 |
| Bit 2 | Remote Stop | EAP 下發停機指令 (1:Stop, 0:Clear) | BIT 0/1 |
| Bit 3 | Remote Start | EAP 下發啟動指令 (1:Start, 0:Clear) | BIT 0/1 |
| Bit 4-15 | Reserved | 預留 | - |

### 3.3 EAP Reply Area (回傳資料緩衝區)
| 位址 | 長度 (Word) | 名稱 | 說明 |
| :--- | :--- | :--- | :--- |
| 41020 | 10 | Recipe ID | MES 回傳之 Recipe (ASCII) |
| 41030 | 20 | Result Message | MES 處理訊息 (ASCII) |
| 41050 | 7 | System Time | 當前系統時間 (YYYYMMDDHHmmss) |

---

## 4. 交握邏輯說明 (Handshake Logic)

### 4.1 Rolling Counter 模式 (適用於連續交易)
1.  **觸發**: PLC 將 `TriggerAddr` 數值 +1 (或是任意不等於 0 且不等於上次值的數值)。
2.  **處理**: EAP 偵測到數值改變，讀取 Data Area 並執行業務邏輯。
3.  **回覆**: EAP 處理完成後，將該數值寫入對應的 `ACKAddr`。
4.  **結束**: PLC 收到 `ACK == Trigger` 後，方可準備下一次觸發。

### 4.2 Bit 0/1 模式 (適用於單次請求)
1.  **觸發**: PLC 將 `TriggerBit` 設為 1。
2.  **回覆**: EAP 執行完成後，將 `ACKBit` 設為 1。
3.  **歸零**: PLC 偵測到 `ACKBit == 1` 後，主動將 `TriggerBit` 歸 0。
4.  **結束**: EAP 偵測到 `TriggerBit == 0` 後，將 `ACKBit` 歸 0。

---
**文件版本**: V1.0
**產出日期**: 2026-03-30
