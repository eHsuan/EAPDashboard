# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V2.0

本規格書優化了背景上報流程，移除 Rolling Counter 不必要的 ACK 回覆，僅保留核心交易的 Word ACK 交握機制。

## 1. 觸發與控制區

### 1.1 交易觸發 (PLC -> EAP)
位址 **40001** (Bit-Mapped)：
| Bit | 名稱 | 說明 | 交握模式 |
| :--- | :--- | :--- | :--- |
| Bit 0 | PLC Heartbeat | 心跳位元 (0/1 切換) | HEARTBEAT |
| Bit 1 | TimeSync Req | 請求時間校正 | BIT 0/1 |
| Bit 2 | Status Query Req | 請求狀態同步 | BIT 0/1 |
| Bit 3 | UserVerify Req | 人員登入驗證請求 | BIT 0/1 |
| Bit 4 | TrackIn Req | 進站報工請求 | BIT 0/1 |
| Bit 5 | TrackOut Req | 出站報工請求 | BIT 0/1 |
| Bit 6 | Remote Stop | 遠端停機請求 | BIT 0/1 |

### 1.2 背景觸發 (PLC -> EAP)
| 位址 | 名稱 | 說明 | 觸發模式 |
| :--- | :--- | :--- | :--- |
| 40003 | Alarm Trigger | 警報上報觸發 | ROLLING (無 ACK) |
| 40004 | Data Trigger | 製程數據上報觸發 | ROLLING (無 ACK) |
| 40005 | Status Trigger | 機台狀態變更觸發 | ROLLING (無 ACK) |
| 40006 | Panel In Trigger | Panel In觸發 | ROLLING (無 ACK) |
| 40007 | Panel Out Trigger | Panel Out觸發 | ROLLING (無 ACK) |

---

## 2. 流程資料專區 (1:1 JSON Mapping)

### 2.1 人員驗證 (Bit 3 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | 10w | PLC→EAP | **[R]** UserID |
| 40020 | 40w | PLC→EAP | **[R]** Barcode (80字元) |
| 40060 | 20w | EAP→PLC | **[W]** ResultMsg |
| 40080 | 1w | EAP→PLC | **[W]** ResultCode (1:OK, 2:NG) |

### 2.2 進站報工 (Bit 4 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
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

### 2.3 出站報工 (Bit 5 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40300 | 10w | ASCII | PLC→EAP | **[R]** UserID (工號) |
| 40310 | 10w | ASCII | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40320 | 10w | ASCII | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40330 | 1w | WORD | PLC→EAP | **[R]** Qty (產出數量) |
| 40340 | 10w | ASCII | PLC→EAP | **[R]** Result (PASS/FAIL) |
| 40350 | 10w | ASCII | PLC→EAP | **[R]** NGCode (不良代碼) |
| 40360 | 20w | ASCII | EAP→PLC | **[W]** ResultMsg (處理結果) |
| 40380 | 1w | WORD | EAP→PLC | **[W] ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.4 背景上報區 (無須回覆)
| 資料位址 | 長度 | 觸發源 | 說明 |
| :--- | :--- | :--- | :--- |
| 40400 | 10w | 40003 | **[Alarm]** AlarmCode |
| 40500 | 10w | 40004 | **[Data]** WorkOrder |
| 40510 | 30w | 40004 | **[Data]** DataPayload (60字元) |
| 40600 | 1w | 40005 | **[Status]** NewStatus |
| 40601 | 1w | 40005 | **[Status]** OldStatus |
| 40602 | 10w | 40005 | **[Status]** ReasonCode |

---

## 3. 交握邏輯說明

### 3.1 Word ACK 模式 (交易類)
*   PLC Set TriggerBit=1 -> EAP Process -> EAP Set ResultCode=1/2 -> PLC Clear TriggerBit=0 -> EAP Clear ResultCode=0。

### 3.2 Rolling 模式 (上報類)
*   PLC Increment Counter -> EAP Detect Change -> EAP Record Data. (No write-back).

---
**版本**: V2.0 | **日期**: 2026-03-30
