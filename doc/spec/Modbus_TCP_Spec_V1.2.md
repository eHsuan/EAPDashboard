# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.2

本規格書嚴格執行「流程隔離」原則，確保每個通訊程序擁有獨立的資料緩衝區，避免並發作業時產生資料覆蓋。

## 1. 狀態與觸發位元組 (Bit-Mapped Words)
為了優化效能，所有 Boolean 訊號併入以下字組。

### 1.1 PLC -> EAP 狀態字組 (位址 40010)
| Bit | 名稱 | 說明 |
| :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 0/1 週期切換 |
| Bit 1 | TimeSync Req | 請求時間校正 |
| Bit 2 | Status Query Req | 請求狀態同步 |
| Bit 3 | UserVerify Req | 人員登入驗證請求 |
| Bit 4 | TrackIn Req | 進站報工請求 |
| Bit 5 | TrackOut Req | 出站報工請求 |

### 1.2 EAP -> PLC 指令字組 (位址 41010)
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

## 2. 獨立資料緩衝區 (Data Buffers)

### 2.1 人員驗證專區 (UserVerify Area)
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40020 | 10w | ASCII | **[R]** Barcode (人員代碼) |
| 41020 | 20w | ASCII | **[W]** Result Message (驗證結果訊息) |

### 2.2 進站請求專區 (Track-In Area)
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | 10w | ASCII | **[R]** Cassette ID (載具編號) |
| 40110 | 10w | ASCII | **[R]** Work Order (工單號碼) |
| 41100 | 10w | ASCII | **[W]** Recipe ID (MES 指派之程式) |
| 41110 | 20w | ASCII | **[W]** Result Message (進站處理訊息) |

### 2.3 出站請求專區 (Track-Out Area)
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40200 | 10w | ASCII | **[R]** Cassette ID (出站載具編號) |
| 40210 | 10w | ASCII | **[R]** Work Order (出站工單號碼) |
| 40220 | 1w  | WORD  | **[R]** Output Qty (產出數量) |
| 41200 | 20w | ASCII | **[W]** Result Message (出站處理訊息) |

### 2.4 其他共用/背景專區
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40300 | 1w  | WORD  | **[R]** Alarm Code (Rolling Trigger 40001) |
| 40400 | 10w | ASCII | **[R]** Process Data (Rolling Trigger 40002) |
| 41300 | 7w  | ASCII | **[W]** System Time (TimeSync 專用) |

---

## 3. 修改計畫：更新資料庫範例 SQL

我建議將 `LDR01` 的設定更新如下，以落實上述隔離位址：

```sql
-- 清空舊設定
DELETE FROM Modbus_DataPoints;
DELETE FROM Modbus_Events;

-- 1. 進站請求 (TrackIn) - 使用 Bit 4
INSERT INTO Modbus_Events (EqpId, EventName, TriggerAddr, AckAddr, HandshakeType, BitOffset) 
VALUES ('LDR01', 'TrackIn', 40010, 41010, 'BIT', 4);

INSERT INTO Modbus_DataPoints (EventId, DataKey, Address, Length, DataType, AccessType) VALUES 
((SELECT last_insert_rowid()), 'CassetteID', 40100, 10, 'ASCII', 'READ'),
((SELECT last_insert_rowid()), 'WorkOrder', 40110, 10, 'ASCII', 'READ'),
((SELECT last_insert_rowid()), 'RecipeID', 41100, 10, 'ASCII', 'WRITE'),
((SELECT last_insert_rowid()), 'ResultMsg', 41110, 20, 'ASCII', 'WRITE');

-- 2. 出站請求 (TrackOut) - 使用 Bit 5
INSERT INTO Modbus_Events (EqpId, EventName, TriggerAddr, AckAddr, HandshakeType, BitOffset) 
VALUES ('LDR01', 'TrackOut', 40010, 41010, 'BIT', 5);

INSERT INTO Modbus_DataPoints (EventId, DataKey, Address, Length, DataType, AccessType) VALUES 
((SELECT last_insert_rowid()), 'CassetteID', 40200, 10, 'ASCII', 'READ'),
((SELECT last_insert_rowid()), 'WorkOrder', 40210, 10, 'ASCII', 'READ'),
((SELECT last_insert_rowid()), 'Qty', 40220, 1, 'WORD', 'READ'),
((SELECT last_insert_rowid()), 'ResultMsg', 41200, 20, 'ASCII', 'WRITE');
```

---
**版本**: V1.2 | **日期**: 2026-03-30
