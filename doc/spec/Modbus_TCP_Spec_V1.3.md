# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.3

本規格書採用「流程連續」設計，將每個通訊程序的讀取與寫入位址集中管理，以優化記憶體使用空間。

## 1. 狀態與控制位元組 (Packed Bits)
為了優化效能，所有布林訊號集中在連續的兩個 Word 中。

### 1.1 PLC -> EAP 狀態字組 (位址 40001)
| Bit | 名稱 | 說明 |
| :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 0/1 週期切換 |
| Bit 1 | TimeSync Req | 請求時間校正 |
| Bit 2 | Status Query Req | 請求狀態同步 |
| Bit 3 | UserVerify Req | 人員登入驗證請求 |
| Bit 4 | TrackIn Req | 進站報工請求 |
| Bit 5 | TrackOut Req | 出站報工請求 |
| Bit 6 | Ready Status | 機台就緒訊號 |
| Bit 7 | Alarm Status | 機台警報訊號 |

### 1.2 EAP -> PLC 控制字組 (位址 40002)
| Bit | 名稱 | 說明 |
| :--- | :--- | :--- |
| Bit 1 | TimeSync ACK | 校正完成 |
| Bit 2 | Status Query ACK | 同步完成 |
| Bit 3 | UserVerify ACK | 驗證完成 |
| Bit 4 | TrackIn ACK | 進站完成 |
| Bit 5 | TrackOut ACK | 出站完成 |
| Bit 6 | Remote Stop | EAP 停機指令 |
| Bit 7 | Remote Start | EAP 啟動指令 |

---

## 2. 流程連續資料區 (Data Blocks)

### 2.1 人員驗證專區 (位址 40010 - 40049)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40010 | 10w | ASCII | PLC→EAP | **[R]** Barcode (人員代碼) |
| 40020 | 20w | ASCII | EAP→PLC | **[W]** Result Message (驗證結果訊息) |

### 2.2 進站報工專區 (位址 40050 - 40099)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40050 | 10w | ASCII | PLC→EAP | **[R]** Cassette ID (進站載具編號) |
| 40060 | 10w | ASCII | PLC→EAP | **[R]** Work Order (進站工單號碼) |
| 40070 | 10w | ASCII | EAP→PLC | **[W]** Recipe ID (下換之程式名) |
| 40080 | 20w | ASCII | EAP→PLC | **[W]** Result Message (進站處理訊息) |

### 2.3 出站報工專區 (位址 40100 - 40149)
| 位址 | 長度 | 類型 | 方向 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40100 | 10w | ASCII | PLC→EAP | **[R]** Cassette ID (出站載具編號) |
| 40110 | 10w | ASCII | PLC→EAP | **[R]** Work Order (出站工單號碼) |
| 40120 | 1w  | WORD  | PLC→EAP | **[R]** Output Qty (產出數量) |
| 40130 | 20w | ASCII | EAP→PLC | **[W]** Result Message (出站處理訊息) |

### 2.4 背景上報與系統專區 (位址 40150 - 40199)
| 位址 | 長度 | 類型 | 方向 | 說明 | 觸發模式 |
| :--- | :--- | :--- | :--- | :--- | :--- |
| 40150 | 1w  | WORD  | PLC→EAP | **[R]** Alarm Trigger | ROLLING |
| 40151 | 1w  | WORD  | PLC→EAP | **[R]** Alarm Code | - |
| 40152 | 1w  | WORD  | EAP→PLC | **[W]** Alarm ACK | - |
| 40160 | 1w  | WORD  | PLC→EAP | **[R]** Data Trigger | ROLLING |
| 40161 | 10w | ASCII | PLC→EAP | **[R]** Process Data | - |
| 40162 | 1w  | WORD  | EAP→PLC | **[W]** Data ACK | - |
| 40180 | 7w  | ASCII | EAP→PLC | **[W]** System Time | BIT 1 |

---

## 3. 交握邏輯摘要 (V1.0.3 標準)

1.  **BIT 交握 (0/1)**: 適用於 40001/40002 中的 Bit。PLC Set 1 -> EAP Done Set 1 -> PLC Clear 0 -> EAP Clear 0。
2.  **ROLLING 交握**: 適用於 40150/40160。數值變化即觸發，完成後 EAP 寫回相同值至 ACK 位址。

---
**版本**: V1.3 | **產出日期**: 2026-03-30
