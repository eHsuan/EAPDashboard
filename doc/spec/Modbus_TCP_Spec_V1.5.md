# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.5

本規格書嚴格對照 V1.0.3 JSON 訊息結構，確保所有通訊欄位皆有獨立 Modbus 位址，實現 JSON 與 Register 1:1 對應。

## 1. 狀態與控制位元組 (Packed Bits)
| 位址 | 名稱 | 方向 | Bit 定義 |
| :--- | :--- | :--- | :--- |
| **40001** | **PLC Status Word** | PLC→EAP | Bit0:HBT, Bit1:TimeSync, Bit2:StatusQuery, Bit3:UserVerify, Bit4:TrackIn, Bit5:TrackOut, Bit6:Ready, Bit7:Alarm |
| **40002** | **EAP Control Word** | EAP→PLC | Bit1:TimeSyncACK, Bit2:StatusACK, Bit3:UserVerifyACK, Bit4:TrackInACK, Bit5:TrackOutACK, Bit6:Stop, Bit7:Start |

---

## 2. 流程資料專區 (1:1 JSON Mapping)

### 2.1 人員驗證專區 (UserVerify)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | 10w | PLC→EAP | **[R]** UserID (工號) |
| 40020 | 40w | PLC→EAP | **[R]** Barcode (人員長代碼) |
| 41010 | 20w | EAP→PLC | **[W]** ResultMsg (驗證回覆訊息) |

### 2.2 進站報工專區 (Track-In)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | 10w | PLC→EAP | **[R]** UserID (工號) |
| 40110 | 10w | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40120 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40130 | 10w | PLC→EAP | **[R]** MaterialID (材料ID) |
| 40140 | 10w | PLC→EAP | **[R]** ToolingID (治具ID) |
| 40150 | 10w | PLC→EAP | **[R]** PanelID (板號) |
| 40160 | 1w | PLC→EAP | **[R]** Qty (進站數量) |
| 41100 | 20w | EAP→PLC | **[W]** RecipeID (MES指派程式) |
| 41120 | 20w | EAP→PLC | **[W]** ResultMsg (進站回覆訊息) |

### 2.3 出站報工專區 (Track-Out)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40200 | 10w | PLC→EAP | **[R]** UserID (工號) |
| 40210 | 10w | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40220 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40230 | 1w | PLC→EAP | **[R]** Qty (產出數量) |
| 40231 | 10w | PLC→EAP | **[R]** Result (PASS/FAIL) |
| 40241 | 10w | PLC→EAP | **[R]** NGCode (不良原因代碼) |
| 41200 | 20w | EAP→PLC | **[W]** ResultMsg (出站回覆訊息) |

### 2.4 狀態變更專區 (StatusChange)
使用 Rolling Trigger **40003** / ACK **41003** 觸發。
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40300 | 1w | PLC→EAP | **[R]** NewStatus (新狀態碼) |
| 40301 | 1w | PLC→EAP | **[R]** OldStatus (舊狀態碼) |
| 40302 | 10w | PLC→EAP | **[R]** ReasonCode (變更原因碼) |

### 2.5 警報上報專區 (Alarm)
使用 Rolling Trigger **40001** / ACK **41001** 觸發。
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40400 | 10w | PLC→EAP | **[R]** AlarmCode (警報代碼) |
| 40410 | 20w | PLC→EAP | **[R]** AlarmText (警報敘述) |

### 2.6 製程數據專區 (DataReport)
使用 Rolling Trigger **40002** / ACK **41002** 觸發。
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40500 | 10w | PLC→EAP | **[R]** WorkOrder (關聯工單) |
| 40510 | 20w | PLC→EAP | **[R]** DataPayload (數據內容字串) |

---

## 3. 系統時間同步 (TimeSync)
由 Bit 1 觸發。
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 41300 | 7w | EAP→PLC | **[W]** SystemTime (YYYYMMDDHHmmss) |

---
**版本**: V1.5 (JSON 對照版) | **日期**: 2026-03-30
