# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.1

本文件定義了 EAP Dashboard 系統與 PLC 之間透過 Modbus TCP 進行交握的位址配置。

## 1. 基本通訊參數
*   **通訊協定**: Modbus TCP (Ethernet)
*   **資料格式**: Big-Endian (高位元在前)
*   **資料區塊**: Holding Registers (4xxxx)

---

## 2. PLC -> EAP 讀取區 (Block 40001 - 40100)

### 2.1 Rolling Triggers (計數器觸發區)
| 位址 | 名稱 | 說明 | 觸發模式 |
| :--- | :--- | :--- | :--- |
| 40001 | Alarm Trigger | 警報上報觸發 | ROLLING |
| 40002 | DataReport Trigger | 製程數據上報觸發 | ROLLING |
| 40003 | Status Trigger | 狀態變更上報觸發 | ROLLING |

### 2.2 PLC Status Word (位元觸發區)
位址 **40010**：採 Bit-Mapped 方式定義 (BIT 0/1 交握)。
| Bit | 名稱 | 說明 | 交握模式 |
| :--- | :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 心跳位元 (0/1 週期切換) | HEARTBEAT |
| Bit 1 | TimeSync Req | 請求 EAP 校正時間 | BIT 0/1 |
| Bit 2 | Status Query Req | 請求 EAP 更新狀態 | BIT 0/1 |
| Bit 3 | UserVerify Req | **人員登入驗證請求** | BIT 0/1 |
| Bit 4 | TrackIn Req | **進站報工請求** | BIT 0/1 |
| Bit 5 | TrackOut Req | **出站報工請求** | BIT 0/1 |
| Bit 6 | Ready Status | 機台就緒訊號 (1:Ready) | LEVEL |
| Bit 7 | Alarm Status | 機台警報訊號 (1:Alarm) | LEVEL |

### 2.3 PLC Data Area (資料區)
位址 40020 起，存放對應事件所需的 Barcode、CassetteID、WorkOrder 等資料。

---

## 3. EAP -> PLC 寫入區 (Block 41001 - 41100)

### 3.1 ACK Counters (計數器回覆區)
| 位址 | 名稱 | 說明 | 對應位址 |
| :--- | :--- | :--- | :--- |
| 41001 | Alarm ACK | 警報處理完成回覆 | 40001 |
| 41002 | DataReport ACK | 數據記錄完成回覆 | 40002 |
| 41003 | Status ACK | 狀態同步完成回覆 | 40003 |

### 3.2 EAP Command Word (位元回覆/控制區)
位址 **41010**：採 Bit-Mapped 方式定義。
| Bit | 名稱 | 說明 | 對應位址 |
| :--- | :--- | :--- | :--- |
| Bit 1 | TimeSync ACK | 時間校正完成 | 40010.1 |
| Bit 2 | Status Query ACK | 狀態同步完成 | 40010.2 |
| Bit 3 | UserVerify ACK | **人員驗證完成** | 40010.3 |
| Bit 4 | TrackIn ACK | **進站報工完成** | 40010.4 |
| Bit 5 | TrackOut ACK | **出站報工完成** | 40010.5 |
| Bit 6 | Remote Stop | EAP 下發停機指令 | BIT 0/1 |
| Bit 7 | Remote Start | EAP 下發啟動指令 | BIT 0/1 |

---

## 4. 交握邏輯 (Handshake Detail)

### 4.1 Bit 0/1 四步交握 (適用於 40010 / 41010)
1.  **PLC**: 將 `TriggerBit` 設為 1。
2.  **EAP**: 偵測到 1，執行業務邏輯，完成後將 `ACKBit` 設為 1。
3.  **PLC**: 偵測到 `ACKBit == 1`，將 `TriggerBit` 歸 0。
4.  **EAP**: 偵測到 `TriggerBit == 0`，將 `ACKBit` 歸 0 (回到 Initial 狀態)。

---
**產出日期**: 2026-03-30 | **版本**: V1.1
